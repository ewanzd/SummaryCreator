using System;
using System.IO;

namespace AutomaticSummaryCreator
{
    /// <summary>
    /// Stellt die Daten der Ini-Datei zur Verfügung.
    /// </summary>
    public class Ini
    {
        /// <summary>
        /// Pfad zur Ini-Datei.
        /// </summary>
        public string IniPath
        {
            get;
            protected set;
        }

        /// <summary>
        /// Liest oder schreibt den XML-Pfad aus/in die Ini-Datei.
        /// </summary>
        public string XmlPath
        {
            get
            {
                return ini.WertLesen("xml", "path");
            }
            set
            {
                ini.WertSchreiben("xml", "path", value);
            }
        }

        /// <summary>
        /// Liest oder schreibt den Excel-Pfad aus/in die Ini-Datei.
        /// </summary>
        public string ExcelPath
        {
            get
            {
                return ini.WertLesen("excel", "path");
            }
            set
            {
                ini.WertSchreiben("excel", "path", value);
            }
        }

        /// <summary>
        /// Liest oder schreibt den Excel-Pfad aus/in die Ini-Datei.
        /// </summary>
        public string SheetName
        {
            get
            {
                return ini.WertLesen("excel", "sheetName");
            }
            set
            {
                ini.WertSchreiben("excel", "sheetName", value);
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
                Int32.TryParse(ini.WertLesen("excel", "idRow"), out id);
                return id;
            }
            set
            {
                ini.WertSchreiben("excel", "idRow", value.ToString());
            }
        }

        public string ExcelSourceDirectory
        {
            get
            {
                return ini.WertLesen("excelSource", "directory");
            }
            set
            {
                ini.WertSchreiben("excelSource", "directory", value);
            }
        }

        /// <summary>
        /// Das Objekt, welches den Zugriff auf die Ini-Datei verwaltet.
        /// </summary>
        protected INI.IINIDatei ini;

        /// <summary>
        /// Stellt das Ini-Objekt zur Verfügung und erstellt sie neu, falls sie nicht vorhanden ist.
        /// </summary>
        /// <param name="path">Pfad zur INI-Datei.</param>
        public Ini(string path)
        {
            this.IniPath = path;

            if(!File.Exists(IniPath))
            {
                using(File.Create(IniPath)) { }
                ini = new INI.INIDatei(IniPath);
                SetDefault();
            }
            else
                ini = new INI.INIDatei(IniPath);
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
    }
}
