using OfficeOpenXml;
using SummaryCreator.Configuration;
using SummaryCreator.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SummaryCreator.Output.Excel
{
    /// <summary>
    /// Write data to excel file.
    /// https://github.com/JanKallman/EPPlus
    /// </summary>
    public sealed class EppExcelWriter : ISummaryWriter
    {
        private const int DATETIME_COLUMN = 1;
        private const char COL_SEPERATOR = ';';

        private int lastRow = 1;

        public void Write(IEnumerable<ITimeSeries> timeSeriesGroup, Stream contentStream, SummaryConfig excelConfig)
        {
            if (timeSeriesGroup == null) throw new ArgumentNullException(nameof(timeSeriesGroup));
            if (timeSeriesGroup.Any(x => x == null)) throw new ArgumentException("IEnumerable contains null values.", nameof(timeSeriesGroup));

            if (excelConfig.Sheet == null) throw new ArgumentNullException(nameof(excelConfig.Sheet));
            if (excelConfig.Row <= 0) throw new ArgumentOutOfRangeException(nameof(excelConfig.Row), "Value must be higher than 0.");

            if (!(timeSeriesGroup is TimeSeriesGroup timeSeriesGroupInstance))
            {
                timeSeriesGroupInstance = new TimeSeriesGroup();
                timeSeriesGroupInstance.AddRange(timeSeriesGroup);
            }

            using var excelPack = new ExcelPackage(contentStream);

            var worksheet = excelPack.Workbook.Worksheets[excelConfig.Sheet];
            if (worksheet == null)
            {
                throw new InvalidDataException($"Table '{excelConfig.Sheet}' not found.");
            }

            var edrWorksheet = new EppWorksheet(worksheet);
            FillDataIntoTable(edrWorksheet, timeSeriesGroupInstance, excelConfig.Row);

            excelPack.Save();
        }

        private void FillDataIntoTable(IWorksheet worksheet, TimeSeriesGroup data, int excelIdRowIndex)
        {
            DateTime start = data.FirstDataPoint.CapturedAt.Date;
            DateTime current = start;
            DateTime end = data.LastDataPoint.CapturedAt.Date;

            while (current <= end)
            {
                var rowIndex = GetOrCreateRowByDateTime(worksheet, current);
                InsertData(worksheet, data, rowIndex, current, excelIdRowIndex);

                current = NextDay(data, current);
            }
        }

        private static string[] GetIds(string value, char seperator)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Array.Empty<string>();
            }

            return value.Split(seperator).Select(x => x.Trim()).ToArray();
        }

        private void InsertData(IWorksheet worksheet, TimeSeriesGroup data, int rowIndex, DateTime startDateTime, int excelIdRowIndex)
        {
            var colsCount = worksheet.Cols;
            for (int col = 1; col <= colsCount; col++)
            {
                var idStr = worksheet[excelIdRowIndex, col]?.ToString();
                var ids = GetIds(idStr, COL_SEPERATOR);

                if (ids.Length == 0)
                {
                    continue;
                }

                if (ids.Length == 1 && data[ids[0]] is MeteoTimeSeries)
                {
                    var meteoTimeSeries = data[ids[0]] as MeteoTimeSeries;
                    var endDateTime = startDateTime + TimeSpan.FromDays(1);

                    var dataPoint = meteoTimeSeries.FirstOrDefault(x =>
                        x.CapturedAt >= startDateTime &&
                        x.CapturedAt < endDateTime &&
                        x.CapturedAt.ToUniversalTime().Hour == 12);

                    if (dataPoint != null)
                    {
                        var value = dataPoint.Value.ToString("0.##", CultureInfo.InvariantCulture);
                        worksheet[rowIndex, col] = value;
                    }
                }
                else
                {
                    var total = GetTotalByIdAndDay(data, ids, startDateTime);

                    // change unit (Wh to kWh)
                    total /= 1000.0;

                    // rounding to 2 fractional digits
                    total = Math.Round(total, 2);

                    // write value to excel field
                    if (!double.IsNaN(total))
                    {
                        var value = total.ToString("0.##", CultureInfo.InvariantCulture);
                        worksheet[rowIndex, col] = value;
                    }
                }
            }
        }

        private int GetOrCreateRowByDateTime(IWorksheet worksheet, DateTime dateTime)
        {
            var rowsCount = worksheet.Rows;

            // find row start at last found row
            int rowIndex = FindRowIndexByDateTime(worksheet, dateTime, lastRow, rowsCount, DATETIME_COLUMN);

            // if not found, start search at first row
            if (rowIndex < 1)
            {
                rowIndex = FindRowIndexByDateTime(worksheet, dateTime, 1, rowsCount, DATETIME_COLUMN);
            }

            // if still not found, create a new entry at the end
            if (rowIndex < 1)
            {
                rowIndex = rowsCount + 1;
                worksheet[rowIndex, DATETIME_COLUMN] = dateTime.ToShortDateString();
            }
            else
            {
                lastRow = rowIndex;
            }

            return rowIndex;
        }

        private static int FindRowIndexByDateTime(IWorksheet worksheet, DateTime targetDateTime, int startRow, int endRow, int col)
        {
            Debug.Assert(startRow >= 0, $"{nameof(startRow)} must be greater than or equal 0");
            Debug.Assert(worksheet.Rows >= endRow, $"Number of rows must be greater or equal {nameof(endRow)}");
            Debug.Assert(endRow >= startRow, $"{nameof(startRow)} must be greater or equal {nameof(endRow)}");

            var rowsCount = worksheet.Rows;
            for (int i = startRow; i <= rowsCount; i++)
            {
                var row = worksheet[i, col];

                if (row is DateTime dt)
                {
                    if (targetDateTime.CompareTo(dt) == 0)
                    {
                        return i;
                    }
                }
                else if (row is string && DateTime.TryParse(row as string, out DateTime dtFromStr))
                {
                    if (targetDateTime.CompareTo(dtFromStr) == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private static DateTime NextDay(TimeSeriesGroup dataPoints, DateTime currentDateTime)
        {
            var tomorrow = currentDateTime + TimeSpan.FromDays(1);

            if (dataPoints.Any(x => tomorrow >= x.First.CapturedAt && tomorrow < x.Last.CapturedAt))
            {
                return tomorrow;
            }

            var leftTimeSeries = dataPoints.OrderBy(x => x.Last.CapturedAt).SkipWhile(x => tomorrow > x.Last.CapturedAt);
            return !leftTimeSeries.Any() ?
                tomorrow :
                leftTimeSeries
                    .Aggregate((minItem, nextItem) => minItem.First.CapturedAt < nextItem.First.CapturedAt ? minItem : nextItem)
                    .First.CapturedAt.Date;
        }

        private static double GetTotalByIdAndDay(TimeSeriesGroup data, string[] ids, DateTime startDateTime)
        {
            var endDateTime = startDateTime + TimeSpan.FromDays(1);

            if (ids.Length == 1)
            {
                var timeSeries = data[ids[0]];

                if (timeSeries?.AnyBetween(startDateTime, endDateTime) == true)
                {
                    return timeSeries.TotalUntil(endDateTime);
                }
            }
            else if (ids.Length > 1)
            {
                // create a sub group of all time series
                TimeSeriesGroup group = new TimeSeriesGroup();
                foreach (var exId in ids)
                {
                    var item = data[exId];
                    if (item != null)
                    {
                        group.Add(item);
                    }
                }

                if (group.AnyBetween(startDateTime, endDateTime))
                {
                    return group.TotalUntil(endDateTime);
                }
            }

            return double.NaN;
        }
    }
}