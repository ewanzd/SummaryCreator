using IniParser;
using IniParser.Model;
using System.Globalization;
using System.IO;

namespace SummaryCreator
{
    /// <summary>
    /// Manage configuration data.
    /// https://github.com/rickyah/ini-parser
    /// </summary>
    public sealed class IniConfigurationService
    {
        private readonly FileInfo file;
        private IniData data;

        /// <summary>
        /// For logging.
        /// </summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Path to file with meteo data.
        /// </summary>
        public string MeteoFilePath {
            get {
                return data["meteo"]["path"];
            }
            set {
                data["meteo"]["path"] = value;
            }
        }

        /// <summary>
        /// Path to directory with sensor data.
        /// </summary>
        public string SensorDirectoryPath {
            get {
                return data["sensor"]["dir"];
            }
            set {
                data["sensor"]["dir"] = value;
            }
        }

        /// <summary>
        /// Path to file to write the result data.
        /// </summary>
        public string ResultExcelFilePath {
            get {
                return data["result"]["excelPath"];
            }
            set {
                data["result"]["excelPath"] = value;
            }
        }

        /// <summary>
        /// Name of sheet in excel file.
        /// </summary>
        public string ResultExcelSheetName {
            get {
                return data["result"]["excelSheetName"];
            }
            set {
                data["result"]["excelSheetName"] = value;
            }
        }

        /// <summary>
        /// Index of row in excel sheet.
        /// </summary>
        public int ResultExcelSheetRowIndex {
            get {
                var idRow = data["result"]["excelRowIndex"];
                if (int.TryParse(idRow, out int id))
                {
                    return id;
                }
                return -1;
            }
            set {
                data["result"]["excelRowIndex"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Create new configuration service with a ini file as persistence store.
        /// </summary>
        /// <param name="path">Path to ini file.</param>
        public IniConfigurationService(string path)
        {
            file = new FileInfo(path);
            data = new IniData();
        }

        /// <summary>
        /// Set default values.
        /// </summary>
        public void SetDefault()
        {
            MeteoFilePath = @"P:\200_Infrastruktur\07_Energie\2018.05.03.forecast.pl.xml";
            SensorDirectoryPath = @"P:\200_Infrastruktur\07_Energie\15 Ablesungen Smart-me";
            ResultExcelFilePath = @"P:\200_Infrastruktur\07_Energie\Heiz- und Stromzähler_Verbrauch 1.xlsx";
            ResultExcelSheetName = "Heizungsablesung täglich";
            ResultExcelSheetRowIndex = 4;

            Logger.Info("Configuration set to default.");
        }

        /// <summary>
        /// Save all configuration data to file.
        /// </summary>
        public void Save()
        {
            Directory.CreateDirectory(file.DirectoryName);
            using (var writer = file.CreateText())
            {
                var parser = new FileIniDataParser();
                parser.WriteData(writer, data);
            }

            Logger.Info("Configuration saved.");
        }

        /// <summary>
        /// Load data from configuration file or set default data if the file doesn't exist.
        /// </summary>
        public void Reload()
        {
            if (file.Exists)
            {
                using var reader = file.OpenText();
                var parser = new FileIniDataParser();
                data = parser.ReadData(reader);
            }
            else
            {
                data = new IniData();
                SetDefault();
            }

            Logger.Info("Configuration reloaded.");
        }
    }
}