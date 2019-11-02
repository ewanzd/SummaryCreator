using OfficeOpenXml;
using SummaryCreator.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SummaryCreator.IO.Excel
{
    public class EppExcelWriter : IDataWriter
    {
        private const int DATETIME_COLUMN = 1;
        private const char COL_SEPERATOR = ';';

        private readonly FileInfo excelFile;
        private readonly string excelSheetName;
        private readonly int excelIdRowIndex;

        private int lastRow = 1;

        public EppExcelWriter(FileInfo targetFile, string sheetName, int idRow)
        {
            Debug.Assert(targetFile != null, $"{nameof(targetFile)} must not be null.");
            Debug.Assert(!string.IsNullOrEmpty(sheetName), $"{nameof(sheetName)} must not be null.");
            Debug.Assert(idRow > 0, $"{nameof(idRow)} must be bigger than 0.");

            excelFile = targetFile;
            excelSheetName = sheetName;
            excelIdRowIndex = idRow;
        }

        public void Write(IEnumerable<IContainer> containers)
        {
            if (!(containers is ContainerGroup containerGroup))
            {
                containerGroup = new ContainerGroup();
                containerGroup.AddRange(containers);
            }

            using var excelPack = new ExcelPackage(excelFile);

            var worksheet = excelPack.Workbook.Worksheets[excelSheetName];
            if (worksheet == null)
            {
                throw new InvalidDataException($"Table '{excelSheetName}' not found.");
            }

            var edrWorksheet = new EppWorksheet(worksheet);
            FillDataIntoTable(edrWorksheet, containerGroup);

            excelPack.Save();
        }

        private void FillDataIntoTable(IWorksheet worksheet, ContainerGroup data)
        {
            DateTime start = data.FirstDataPoint.CapturedAt.Date;
            DateTime current = start;
            DateTime end = data.LastDataPoint.CapturedAt.Date;

            while (current <= end)
            {
                var rowIndex = GetOrCreateRowByDateTime(worksheet, current);
                InsertData(worksheet, data, rowIndex, current);

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

        private void InsertData(IWorksheet worksheet, ContainerGroup data, int rowIndex, DateTime startDateTime)
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

                if (ids.Length == 1 && data[ids[0]] is MeteoContainer)
                {
                    var meteoContainer = data[ids[0]] as MeteoContainer;
                    var endDateTime = startDateTime + TimeSpan.FromDays(1);

                    var dataPoint = meteoContainer.FirstOrDefault(x =>
                        x.CapturedAt >= startDateTime &&
                        x.CapturedAt < endDateTime &&
                        x.CapturedAt.ToUniversalTime().Hour == 12);

                    if (dataPoint != null)
                    {
                        var value = dataPoint.Value.ToString("0.###", CultureInfo.InvariantCulture);
                        worksheet[rowIndex, col] = value;
                    }
                }
                else
                {
                    var total = GetTotalByIdAndDay(data, ids, startDateTime);
                    if (!double.IsNaN(total))
                    {
                        var value = total.ToString("0.###", CultureInfo.InvariantCulture);
                        worksheet[rowIndex, col] = value;
                    }

                    var sum = GetSumByIdAndDay(data, ids, startDateTime);
                    if (!double.IsNaN(sum))
                    {
                        var value = sum.ToString("0.###", CultureInfo.InvariantCulture);
                        worksheet[rowIndex, col + 1] = value;
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

        private static DateTime NextDay(ContainerGroup dataPoints, DateTime currentDateTime)
        {
            var tomorrow = currentDateTime + TimeSpan.FromDays(1);

            if (dataPoints.Any(x => tomorrow >= x.First.CapturedAt && tomorrow < x.Last.CapturedAt))
            {
                return tomorrow;
            }

            var leftContainers = dataPoints.OrderBy(x => x.Last.CapturedAt).SkipWhile(x => tomorrow > x.Last.CapturedAt);
            return !leftContainers.Any() ?
                tomorrow :
                leftContainers
                    .Aggregate((minItem, nextItem) => minItem.First.CapturedAt < nextItem.First.CapturedAt ? minItem : nextItem)
                    .First.CapturedAt.Date;
        }

        private static double GetSumByIdAndDay(ContainerGroup data, string[] ids, DateTime startDateTime)
        {
            var endDateTime = startDateTime + TimeSpan.FromDays(1);

            if (ids.Length == 1)
            {
                var container = data[ids[0]];

                if (container != null && container.AnyBetween(startDateTime, endDateTime))
                {
                    return container.Sum(startDateTime, endDateTime);
                }
            }
            else if (ids.Length > 1)
            {
                // create a sub group of all containers
                ContainerGroup group = new ContainerGroup();
                foreach (var exId in ids)
                {
                    var container = data[exId];
                    if (container != null)
                    {
                        group.Add(container);
                    }
                }

                if (group.AnyBetween(startDateTime, endDateTime))
                {
                    return group.Sum(startDateTime, endDateTime);
                }
            }

            return double.NaN;
        }

        private static double GetTotalByIdAndDay(ContainerGroup data, string[] ids, DateTime startDateTime)
        {
            var endDateTime = startDateTime + TimeSpan.FromDays(1);

            if (ids.Length == 1)
            {
                var container = data[ids[0]];

                if (container != null && container.AnyBetween(startDateTime, endDateTime))
                {
                    return container.Total(endDateTime);
                }
            }
            else if (ids.Length > 1)
            {
                // create a sub group of all containers
                ContainerGroup group = new ContainerGroup();
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
                    return group.Total(endDateTime);
                }
            }

            return double.NaN;
        }
    }
}