using OfficeOpenXml;

namespace SummaryCreator.IO.Excel
{
    /// <summary>
    /// Editable worksheet.
    /// https://github.com/JanKallman/EPPlus
    /// </summary>
    public class EppWorksheet : IWorksheet
    {
        private readonly ExcelWorksheet worksheet;

        public EppWorksheet(ExcelWorksheet worksheet)
        {
            this.worksheet = worksheet;
        }

        public object this[int row, int col] {
            get {
                return worksheet.GetValue(row, col);
            }
            set {
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