using SummaryCreator.Basic;
using SummaryCreator.Model;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel; 

namespace SummaryCreator.Export
{
    /// <summary>
    /// Can export the data from summery.
    /// </summary>
    public class ExcelExport : IDisposable
    {
        int tableCount = 0;
        Excel.Application app;
        Excel.Workbook wkb;

        public ExcelExport()
        {
            // Excel erstellen
            app = new Excel.Application();
            app.Visible = true;
            app.ScreenUpdating = true;

            // Container für Arbeitsblätter erstellen
            wkb = (Excel.Workbook)app.Workbooks.Add();
        }

        /// <summary>
        /// Export table to excel.
        /// </summary>
        /// <param name="summary">All data from Summary include in excel.</param>
        /// <param name="settings">Settings for create excel.</param>
        public void AddTable(Summary summary, ExcelTableSetting settings)
        {
            if(summary == null)
                throw new ArgumentNullException("summary");

            if(settings == null)
                throw new ArgumentNullException("settings");

            // Die Anzahl der Tabelle wird um 1 erhöht
            tableCount++;

            // Falls nicht genügend Tabellen vorhanden sind wird eine hinzugefügt
            if(tableCount > wkb.Worksheets.Count)
                wkb.Sheets.Add(After: wkb.Sheets[wkb.Sheets.Count]);

            // Die Tabelle wird ausgewählt
            Excel.Worksheet wks = (Excel.Worksheet)wkb.Worksheets[tableCount];

            // Tabelle wird angezeigt
            wks.Select();

            // Tabelle einen Namen geben
            if(String.IsNullOrWhiteSpace(settings.Name))
                wks.Name = "Unbekannt" + tableCount;
            else
                wks.Name = settings.Name;

            // Inhalt einfügen
            createSheet(wks, summary, settings.Interval, settings.DifferenceActive, settings.OutputUnit);

            // Titel fett
            Excel.Range myRangeHeadline = wks.get_Range("1:1", Type.Missing);
            myRangeHeadline.Font.Bold = true;
        }

        /// <summary>
        /// Create content for worksheet with summary from interval time.
        /// </summary>
        /// <param name="wks">Save the data in that worksheet.</param>
        /// <param name="interval">The difference between the rows.</param>
        /// <param name="differentActive">Add a column with difference between the rows.</param>
        static void createSheet(Excel.Worksheet wks, Summary summary, TimeSpan interval, bool differentActive, Unit output)
        {
            // Erstellt eine Hilfsklasse für den WorkSheet
            SheetHelper sheet = new SheetHelper(wks);

            // Start und Endzeit
            DateTime start = summary.StartTime;
            DateTime end = summary.EndTime;

            // Startwerte
            int colCount = 1;

            // Datumswerte schreiben
            sheet.AddTitel(colCount, "Datum");
            int totalRow = sheet.CreateDateTimeCol(start, end, interval, 2, colCount);
            colCount++;

            // Inhaltswerte aller Tabellen schreiben
            foreach(var table in summary)
            {
                // Erste Zeile
                Row firstRow = table.FirstRow(table.FirstTime, table.FirstTime + interval);

                // Titel erstellen
                int colTitel = colCount;
                for(int i = 0; i < firstRow.Count; i++)
                {
                    string text = String.Format("{0} Total ({1}Wh)", table.ToString(), output.GetUnitValue());
                    sheet.AddTitel(colTitel, text);
                    if(differentActive)
                        sheet.AddTitel(++colTitel, "Differenz");
                    colTitel++;
                }

                // Tabelle auffüllen
                Row previousValue = firstRow;
                int currentRow = 2;

                // Jede erste Zeile im Intervall
                foreach(var row in table.NextRow(start, interval))
                {
                    // Falls es in der aktuellen Zeile keinen Wert gibt wird diese übersprungen
                    if(row == null)
                    {
                        currentRow++;
                        continue;
                    }

                    // Prüft, ob die Zeile mit dem Datum übereinstimmt
                    int rowCountData = sheet.SearchDateTime(row.DateTime, currentRow, totalRow, 1);
                    if(!(rowCountData < 1))
                        currentRow = rowCountData;
                    else
                        continue;

                    // Jede Spalte der Zeile
                    int i = 0;
                    int currentCol = colCount;
                    foreach(var value in row)
                    {
                        sheet[currentRow, currentCol] = value.ToString(output);
                        if(differentActive)
                        {
                            currentCol++;
                            if(previousValue != null && previousValue.Count > 0)
                            {
                                sheet[currentRow, currentCol] = (value - previousValue[i]).ToString(output);
                                i++;
                            }
                        }
                        currentCol++;
                    }
                    // Werte für den nächsten Durchlauf anpassen
                    previousValue = row;
                    currentRow++;
                }
                // Zur nächsten freien Spalte springen
                if(differentActive)
                    colCount += firstRow.Count * 2;
                else
                    colCount += firstRow.Count;
            }
        }

        /// <summary>
        /// Quit all connection to excel.
        /// </summary>
        public void Dispose()
        {
            try
            {
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
            }
        }
    }
}
