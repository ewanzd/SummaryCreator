namespace SummaryCreator.IO.Excel
{
    /// <summary>
    /// Simple interface to work with worksheets.
    /// </summary>
    public interface IWorksheet
    {
        /// <summary>
        /// Number of rows.
        /// </summary>
        int Rows { get; }

        /// <summary>
        /// Number of cols.
        /// </summary>
        int Cols { get; }

        /// <summary>
        /// Read and write values.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <param name="col">Col index.</param>
        /// <returns>Value in cell.</returns>
        object this[int row, int col] { get; set; }
    }
}