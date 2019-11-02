using OfficeOpenXml;
using System;

namespace SummaryCreator.IO.Excel
{
    /// <summary>
    /// For easy access to
    /// </summary>
    public sealed class EppWorksheet : IWorksheet
    {
        private readonly ExcelWorksheet worksheet;

        public EppWorksheet(ExcelWorksheet worksheet)
        {
            if (worksheet == null) throw new ArgumentNullException(nameof(worksheet));

            this.worksheet = worksheet;
        }

        public object this[int row, int col] {
            get {
                if (row <= 0) throw new ArgumentOutOfRangeException(nameof(row));
                if (col <= 0) throw new ArgumentOutOfRangeException(nameof(col));

                return worksheet.GetValue(row, col);
            }
            set {
                if (row <= 0) throw new ArgumentOutOfRangeException(nameof(row));
                if (col <= 0) throw new ArgumentOutOfRangeException(nameof(col));

                worksheet.SetValue(row, col, value);
            }
        }

        public int Rows {
            get {
                var cells = worksheet.Cells;
                var data = cells.Value as object[,];
                return data.GetLength(0);
            }
        }

        public int Cols {
            get {
                var cells = worksheet.Cells;
                var data = cells.Value as object[,];
                return data.GetLength(1);
            }
        }
    }
}