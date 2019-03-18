using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using OfficeExcel = Microsoft.Office.Interop.Excel;

namespace BasicLibrary.Excel
{
    /// <summary>
    /// Stellt die Arbeitsblätter eines Arbeitsbuches zur Verfügung.
    /// </summary>
    public class Book : IDisposable
    {
        /// <summary>
        /// Anzahl der Tabellen
        /// </summary>
        int sheetCount = 0;

        /// <summary>
        /// Excel-Anwendung.
        /// </summary>
        OfficeExcel.Application app;

        /// <summary>
        /// Aktuelles Arbeitsbuch.
        /// </summary>
        OfficeExcel.Workbook wkb;

        /// <summary>
        /// Gibt der aktuelle Status an, ob die Excel-Datei bereits geschlossen wurde.
        /// </summary>
        bool close = false;

        /// <summary>
        /// Gibt der aktuelle Status an, ob die Excel-Datei bereits freigegeben wurde.
        /// </summary>
        bool dispose = false;

        /// <summary>
        /// Anzahl der Tabellen.
        /// </summary>
        public int Count
        {
            get
            {
                return sheetCount;
            }
        }

        /// <summary>
        /// Gibt auskunft, ob die Excel-Datei bereits geschlossen wurde.
        /// </summary>
        public bool IsClose
        {
            get
            {
                return close;
            }
        }

        /// <summary>
        /// Gibt auskunft, ob die Excel-Datei bereits freigegeben wurde.
        /// </summary>
        public bool IsDispose
        {
            get
            {
                return dispose;
            }
        }

        /// <summary>
        /// Startet ein neues Arbeitsbuch.
        /// </summary>
        public Book()
        {
            // Excel erstellen
            initialize();

            // Container für Arbeitsblätter erstellen
            wkb = (OfficeExcel.Workbook)app.Workbooks.Add();
        }

        /// <summary>
        /// Erstellt das Objekt mit einem bestehenden Arbeitsbuch.
        /// </summary>
        /// <param name="path">Der Pfad zum Arbeitsbuch.</param>
        public Book(string path)
        {
            // Excel erstellen
            initialize();

            // Öffnet das Arbeitsbuch
            Open(path);
        }

        /// <summary>
        /// Excel Applikation initialisieren.
        /// </summary>
        protected void initialize()
        {
            // Excel erstellen
            app = new OfficeExcel.Application()
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
            if(!File.Exists(path))
                throw new ArgumentException("Datei nicht vorhanden.");

            // Prüft, ob es sich um eine Excel-Datei handelt
            if(Path.GetExtension(path) != ".xlsx")
                throw new ArgumentException("Ungültiges Format.");

            // Öffnet das Arbeitsbuch
            wkb = app.Workbooks.Open(path);
        }

        /// <summary>
        /// Erstellt ein neues Arbeitsblatt.
        /// </summary>
        public void NewSheet()
        {
            // Die Anzahl der Tabelle wird um 1 erhöht
            sheetCount++;

            // Falls nicht genügend Tabellen vorhanden sind wird eine hinzugefügt
            if(sheetCount > wkb.Worksheets.Count)
                wkb.Sheets.Add(After: wkb.Sheets[wkb.Sheets.Count]);
        }

        /// <summary>
        /// Erstellt ein neues Arbeitsblatt und gibt dieses zurück.
        /// </summary>
        /// <typeparam name="sheet">Type des Arbeitsblattes.</typeparam>
        /// <returns>Das neue Arbeitsblatt.</returns>
        public sheet NewSheet<sheet>() where sheet : Sheet, new()
        {
            // Die Anzahl der Tabelle wird um 1 erhöht
            sheetCount++;

            // Falls nicht genügend Tabellen vorhanden sind wird eine hinzugefügt
            if(sheetCount > wkb.Worksheets.Count)
                wkb.Sheets.Add(After: wkb.Sheets[wkb.Sheets.Count]);

            // Tabelle holen
            return GetSheet<sheet>(sheetCount);
        }

        /// <summary>
        /// Prüft, ob ein Arbeitsblatt mit diesem Namen vorhanden ist.
        /// </summary>
        /// <param name="name">Name des Arbeitsblattes.</param>
        /// <returns>Gibt true zurück wenn vorhanden und false wenn nicht.</returns>
        public bool ContainsSheet(string name)
        {
            foreach(OfficeExcel.Worksheet sheet in wkb.Worksheets)
                if(sheet.Name == name)
                    return true;
            return false;
        }

        /// <summary>
        /// Holt ein vorhandenes Arbeitsblatt mit dem angegebenen Index.
        /// </summary>
        /// <typeparam name="sheet">Type des Arbeitsblattes.</typeparam>
        /// <param name="index">Der Index, der das Arbeitsblatt im Arbeitsbuch besitzt.</param>
        /// <returns>Gibt das Arbeitsblatt mit dem angegebenen Type zurück.</returns>
        public sheet GetSheet<sheet>(int index) where sheet : Sheet, new()
        {
            sheet she = new sheet();
            she.SetSheet((OfficeExcel.Worksheet)wkb.Worksheets[index]);
            return she;
        }

        /// <summary>
        /// Holt ein vorhandenes Arbeitsblatt mit diesem Namen.
        /// </summary>
        /// <typeparam name="sheet">Type des Arbeitsblattes.</typeparam>
        /// <param name="name">Name des Arbeitsblattes, welches geholt werden sollte.</param>
        /// <returns>Gibt das Arbeitsblatt mit dem angegebenen Type zurück.</returns>
        public sheet GetSheet<sheet>(string name) where sheet : Sheet, new()
        {
            foreach(OfficeExcel.Worksheet workSheet in wkb.Worksheets)
                if(workSheet.Name == name)
                {
                    sheet she = new sheet();
                    she.SetSheet(workSheet);
                    return she;
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
            this.Close();
        }

        /// <summary>
        /// Schliesst die Excel-Datei ohne zu speichern. Das Objekt kann danach nicht mehr verwendet werden.
        /// </summary>
        public void Close()
        {
            if(app == null)
                return;

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
                if(!IsClose)
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
        ~Book()
        {
            Dispose();
        }
    }
}
