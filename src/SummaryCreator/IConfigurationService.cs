namespace SummaryCreator
{
    /// <summary>
    /// Load and save configurations.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Path to file with meteo data.
        /// </summary>
        string MeteoFilePath { get; set; }

        /// <summary>
        /// Path to directory with sensor data.
        /// </summary>
        string SensorDirectoryPath { get; set; }

        /// <summary>
        /// Path to file to write the result data.
        /// </summary>
        string ResultExcelFilePath { get; set; }

        /// <summary>
        /// Name of sheet in excel file.
        /// </summary>
        string ResultExcelSheetName { get; set; }

        /// <summary>
        /// Index of row in excel sheet.
        /// </summary>
        int ResultExcelSheetRowIndex { get; set; }

        /// <summary>
        /// Set default values.
        /// </summary>
        void SetDefault();

        /// <summary>
        /// Save all configuration data to file.
        /// </summary>
        void Save();

        /// <summary>
        /// Load data from configuration file or set default data if the file doesn't exist.
        /// </summary>
        void Reload();
    }
}