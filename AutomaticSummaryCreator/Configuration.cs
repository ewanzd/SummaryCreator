using IniParser;
using IniParser.Model;
using System;
using System.Diagnostics;
using System.IO;

namespace AutomaticSummaryCreator
{
    /// <summary>
    /// Stellt die Daten der Ini-Datei zur Verfügung.
    /// https://github.com/rickyah/ini-parser
    /// </summary>
    public class Configuration
    {
        private FileInfo file; 

        /// <summary>
        /// Das Objekt, welches den Zugriff auf die Ini-Datei verwaltet.
        /// </summary>
        private IniData data;

        /// <summary>
        /// Liest oder schreibt den XML-Pfad aus/in die Ini-Datei.
        /// </summary>
        public string XmlPath
        {
            get
            {
                return data["xml"]["path"];
            }
            set
            {
                data["xml"]["path"] = value;
            }
        }

        /// <summary>
        /// Liest oder schreibt den Excel-Pfad aus/in die Ini-Datei.
        /// </summary>
        public string ExcelPath
        {
            get
            {
                return data["excel"]["path"];
            }
            set
            {
                data["excel"]["path"] = value;
            }
        }

        /// <summary>
        /// Liest oder schreibt den Excel-Pfad aus/in die Ini-Datei.
        /// </summary>
        public string SheetName
        {
            get
            {
                return data["excel"]["sheetName"];
            }
            set
            {
                data["excel"]["sheetName"] = value;
            }
        }

        /// <summary>
        /// Liest oder schreibt den Excel-Pfad aus/in die Ini-Datei.
        /// </summary>
        public int SheetIdRow
        {
            get
            {
                int id = -1;
                var idRow = data["excel"]["idRow"];
                Int32.TryParse(idRow, out id);
                return id;
            }
            set
            {
                data["excel"]["idRow"] = value.ToString();
            }
        }

        public string ExcelSourceDirectory
        {
            get
            {
                return data["excelSource"]["directory"];
            }
            set
            {
                data["excelSource"]["directory"] = value;
            }
        }

        /// <summary>
        /// Stellt das Ini-Objekt zur Verfügung und erstellt sie neu, falls sie nicht vorhanden ist.
        /// </summary>
        /// <param name="path">Pfad zur INI-Datei.</param>
        public Configuration(string path)
        {
            file = new FileInfo(path);

            if (File.Exists(path))
            {
                using(var reader = file.OpenText())
                {
                    var parser = new FileIniDataParser();
                    data = parser.ReadData(reader);
                }
            } else {
                data = new IniData();
                SetDefault();
            }
        }

        /// <summary>
        /// Setzt die Standardwerte.
        /// </summary>
        public void SetDefault()
        {
            XmlPath = @"http://www.metweb.ch/cgi-local/haus/forecast.pl?lat=47.377455&long=8.536715";
            ExcelPath = @"P:\200_Infrastruktur\07_Energie\Heiz- und Stromzähler_Verbrauch 1.xlsx";
            SheetName = "Heizungsablesung täglich";
            SheetIdRow = 4;
            ExcelSourceDirectory = @"P:\200_Infrastruktur\07_Energie\15 Ablesungen Smart-me";
        }

        /// <summary>
        /// Speichert die Konfigurationen.
        /// </summary>
        public void Save()
        {
            using(var writer = file.CreateText())
            {
                var parser = new FileIniDataParser();
                parser.WriteData(writer, data);
            }
        }
    }
}
