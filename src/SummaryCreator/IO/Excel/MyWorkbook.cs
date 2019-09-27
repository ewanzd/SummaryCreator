using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace SummaryCreator.IO.Excel
{
    /// <summary>
    /// Stellt die Arbeitsblätter eines Arbeitsbuches zur Verfügung.
    /// </summary>
    public sealed class MyWorkbook : IDisposable
    {
        /// <summary>
        /// Anzahl der Tabellen
        /// </summary>
        private int sheetCount = 0;

        /// <summary>
        /// Excel-Anwendung.
        /// </summary>
        private Application app;

        /// <summary>
        /// Aktuelles Arbeitsbuch.
        /// </summary>
        private Workbook wkb;

        /// <summary>
        /// Gibt der aktuelle Status an, ob die Excel-Datei bereits geschlossen wurde.
        /// </summary>
        private bool close = false;

        /// <summary>
        /// Gibt der aktuelle Status an, ob die Excel-Datei bereits freigegeben wurde.
        /// </summary>
        private bool dispose = false;

        /// <summary>
        /// Anzahl der Tabellen.
        /// </summary>
        public int Count {
            get {
                return sheetCount;
            }
        }

        /// <summary>
        /// Gibt auskunft, ob die Excel-Datei bereits geschlossen wurde.
        /// </summary>
        public bool IsClose {
            get {
                return close;
            }
        }

        /// <summary>
        /// Gibt auskunft, ob die Excel-Datei bereits freigegeben wurde.
        /// </summary>
        public bool IsDispose {
            get {
                return dispose;
            }
        }

        /// <summary>
        /// Startet ein neues Arbeitsbuch.
        /// </summary>
        public MyWorkbook()
        {
            // Excel erstellen
            Initialize();

            // Container für Arbeitsblätter erstellen
            wkb = app.Workbooks.Add();
        }

        /// <summary>
        /// Erstellt das Objekt mit einem bestehenden Arbeitsbuch.
        /// </summary>
        /// <param name="path">Der Pfad zum Arbeitsbuch.</param>
        public MyWorkbook(string path)
        {
            // Excel erstellen
            Initialize();

            // Öffnet das Arbeitsbuch
            Open(path);
        }

        /// <summary>
        /// Excel Applikation initialisieren.
        /// </summary>
        private void Initialize()
        {
            // Excel erstellen
            app = new Application()
            {
                Visible = true,
                ScreenUpdating = true,
                DisplayAlerts = false
            };
        }

        /// <summary>
        /// Öffnet ein bestehendes Arbeitsbuch.
        /// </summary>
        /// <param name="path">Der Pfad zum Arbeitsbuch.</param>
        public void Open(string path)
        {
            // Prüft, ob die Datei existiert
            if (!File.Exists(path))
                throw new ArgumentException("Datei nicht vorhanden.");

            // Prüft, ob es sich um eine Excel-Datei handelt
            if (!Path.GetExtension(path).Equals(".xlsx"))
                throw new ArgumentException("Ungültiges Format.");

            // Öffnet das Arbeitsbuch
            wkb = app.Workbooks.Open(path);
        }

        /// <summary>
        /// Erstellt ein neues Arbeitsblatt und gibt dieses zurück.
        /// </summary>
        /// <returns>Das neue Arbeitsblatt.</returns>
        public MyWorksheet NewSheet()
        {
            // Die Anzahl der Tabelle wird um 1 erhöht
            sheetCount++;

            // Falls nicht genügend Tabellen vorhanden sind wird eine hinzugefügt
            if (sheetCount > wkb.Worksheets.Count)
            {
                wkb.Sheets.Add(After: wkb.Sheets[wkb.Sheets.Count]);
            }

            // Tabelle holen
            return GetSheet(sheetCount);
        }

        /// <summary>
        /// Prüft, ob ein Arbeitsblatt mit diesem Namen vorhanden ist.
        /// </summary>
        /// <param name="name">Name des Arbeitsblattes.</param>
        /// <returns>Gibt true zurück wenn vorhanden und false wenn nicht.</returns>
        public bool ContainsSheet(string name)
        {
            if (name == null)
            {
                return false;
            }

            foreach (Worksheet sheet in wkb.Worksheets)
            {
                if (name.Equals(sheet.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Holt ein vorhandenes Arbeitsblatt mit dem angegebenen Index.
        /// </summary>
        /// <param name="index">Der Index, der das Arbeitsblatt im Arbeitsbuch besitzt.</param>
        /// <returns>Gibt das Arbeitsblatt mit dem angegebenen Type zurück.</returns>
        public MyWorksheet GetSheet(int index)
        {
            var mySheet = new MyWorksheet();
            mySheet.SetSheet((Worksheet)wkb.Worksheets[index]);
            return mySheet;
        }

        /// <summary>
        /// Holt ein vorhandenes Arbeitsblatt mit diesem Namen.
        /// </summary>
        /// <param name="name">Name des Arbeitsblattes, welches geholt werden sollte.</param>
        /// <returns>Gibt das Arbeitsblatt mit dem angegebenen Type zurück.</returns>
        public MyWorksheet GetSheet(string name)
        {
            foreach (Worksheet workSheet in wkb.Worksheets)
                if (name.Equals(workSheet.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    var mySheet = new MyWorksheet();
                    mySheet.SetSheet(workSheet);
                    return mySheet;
                }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Speichert und schliesst die Excel-Datei. Das Objekt kann danach nicht mehr verwendet werden.
        /// </summary>
        /// <param name="path">Pfad, wo die Datei gespeichert werden soll.</param>
        public void SaveAndClose(string path)
        {
            wkb.Close(true, path);
            Close();
        }

        /// <summary>
        /// Schliesst die Excel-Datei ohne zu speichern. Das Objekt kann danach nicht mehr verwendet werden.
        /// </summary>
        public void Close()
        {
            if (app == null)
            {
                return;
            }

            app.Workbooks.Close();
            app.Quit();
            close = true;
        }

        /// <summary>
        /// Gibt alle COM-Ressourcen frei, sollte dringend ausgeführt werden.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (!IsClose)
                    Close();

                Marshal.ReleaseComObject(wkb.Worksheets);
                Marshal.ReleaseComObject(wkb);
                Marshal.ReleaseComObject(app);
            }
            catch
            {
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                dispose = true;
            }
        }

        /// <summary>
        /// Beendet das Objekt und gibt die COM-Ressourcen frei.
        /// </summary>
        ~MyWorkbook()
        {
            Dispose();
        }
    }
}