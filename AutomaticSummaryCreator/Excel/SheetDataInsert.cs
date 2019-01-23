using BasicLibrary.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticSummaryCreator.Excel
{
    /// <summary>
    /// Verwaltet die Tabelle und fügt die Daten ein. 
    /// </summary>
    public class SheetDataInsert
    {
        /// <summary>
        /// Verwaltet das Arbeitsbuch und die Excel-Applikation.
        /// </summary>
        protected Book book = null;

        /// <summary>
        /// Hilft beim Zugriff auf die Tabelle.
        /// </summary>
        protected SheetTarget sheet = null;

        /// <summary>
        /// Zeile, wo die IDs der Spalten gespeichert wurde, damit die Werte zugeordnet werden können.
        /// </summary>
        public int IdRow
        {
            get;
            set;
        }

        /// <summary>
        /// Pfad zu der Excel-Quelldatei. Sie muss bereits das gegebene Format einhalten.
        /// </summary>
        protected string targetPath;

        /// <summary>
        /// Öffnet die Tabelle, damit sie bearbeitet werden kann.
        /// </summary>
        /// <param name="targetPath">Pfad zur Zieldatei.</param>
        /// <param name="sheetName">Name der Tabelle in der Zieldatei.</param>
        /// <param name="idRow">Nummer der Zeile, in der die IDs gespeichert wurden.</param>
        public SheetDataInsert(string targetPath, string sheetName, int idRow = 1)
        {
            this.targetPath = targetPath;
            book = new Book(targetPath);
            sheet = book.GetSheet<SheetTarget>(sheetName);
            this.IdRow = idRow;
        }

        int lastRow = 1;

        /// <summary>
        /// Neue Daten in die Tabelle einfüllen.
        /// </summary>
        /// <param name="getData">Funktion übergeben, in der die ID ausgewertet und der fertige Wert zurückgibt, der in die Tabelle eingefügt wird(SpaltenID, ZeilenID).</param>
        /// <param name="targetRowValue">Die Bezeichnung der Spalte, in der die Daten eingefügt werden sollen.</param>
        /// <param name="targetColSearch">Sucht in dieser Spalte nach der Bezeichnung.</param>
        public virtual int Insert(Func<string, string, string> getData, string targetRowValue, int targetColSearch = 1)
        {
            // Die Funktion muss mitgegeben werden
            if(getData == null)
                throw new ArgumentNullException("getData");

            // ID-Zeile muss mindestens 1 sein
            if(IdRow < 1 || IdRow > sheet.CountRow)
                throw new IndexOutOfRangeException("Ungültige ID-Zeile: " + IdRow);

            // Zeile ermitteln
            int row = sheet.IndexOfRow(targetRowValue, lastRow, sheet.CountRow, targetColSearch);

            // Falls nicht gefunden bei Zeile 1 suchen beginnen
            if(row < 1)
                row = sheet.IndexOfRow(targetRowValue, 1, sheet.CountRow, targetColSearch);

            // Wenn die Zeile nicht gefunden werden konnte wird eine neue Zeile hinten angefügt
            if(row < 1)
            {
                row = sheet.CountRow + 1;
                sheet[row, targetColSearch] = targetRowValue;
            }
            else
                lastRow = row;

            // Alle IDs in der im Konstruktor übergebenen Zeile durchgehen
            for(int col = 1; col <= sheet.CountCol; col++)
            {
                // Wert lesen
                string id = sheet[IdRow, col];

                // Prüfen, ob das Feld einen Wert besitzt
                if(String.IsNullOrWhiteSpace(id))
                    continue;

                // Holt den Wert ab
                string newValue = getData(id, targetRowValue);

                // Falls ein Wert zurückgegeben wurde wird er in das entsprechende Feld eingefügt
                if(!String.IsNullOrEmpty(newValue))
                    sheet[row, col] = newValue;
            }

            return row;
        }

        /// <summary>
        /// Speichert und schliesst die Excel-Applikation.
        /// </summary>
        public virtual void Close(bool save = true)
        {
            // Prüfen ob das Arbeitsbuch erstellt wurde
            if(book == null)
                return;

            // Schliesst das Arbeitsbuch und speichert es falls gewünscht
            if(!book.IsClose && save)
                book.SaveAndClose(targetPath);
            else if(!book.IsClose)
                book.Close();

            // Gibt die Excel-Applikation frei
            if(!book.IsDispose)
                book.Dispose();
        }
    }
}
