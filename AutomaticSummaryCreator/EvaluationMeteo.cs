using AutomaticSummaryCreator.Data;
using AutomaticSummaryCreator.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticSummaryCreator
{
    /// <summary>
    /// Verwaltet die Meteo-Daten.
    /// </summary>
    public class EvaluationMeteo : Evaluation
    {
        /// <summary>
        /// Datencontainer.
        /// </summary>
        public MeteoData Data
        {
            get;
            set;
        }

        /// <summary>
        /// Ladet alle Daten der angegebenen XML-Datei.
        /// </summary>
        /// <param name="path">Pfad zu der XML-Datei.</param>
        public override void LoadData(string path)
        {
            // Lädt die XML-Datei herunter
            XmlLoad xml = new XmlLoad(path);

            // Lädt die Daten in das MeteoData Objekt
            Data = xml.Evaluation();
        }

        /// <summary>
        /// Daten speichern.
        /// </summary>
        /// <param name="insert">Daten über den angegebenen Insert speichern.</param>
        /// <param name="targetRow">ID der Zeile, in der die Werte gespeichert werden soll.</param>
        public override void SaveData(SheetDataInsert insert, string targetRow)
        {
            if(Data == null)
                throw new Exception("Keine Daten verfügbar");

            insert.Insert(insert_GetData, targetRow);
        }

        /// <summary>
        /// Wertet den Wert für das entsprechende Feld aus.
        /// </summary>
        /// <param name="colId">Bezeichnung für die angesprochene Spalte.</param>
        /// <param name="rowId">Bezeichnung für die angesprochene Zeile.</param>
        /// <returns>Ausgewerteter Wert.</returns>
        protected virtual string insert_GetData(string colId, string rowId)
        {
            // id teilen in name und tag
            string[] splitId = colId.Split('-');
            string name = splitId[0];

            // Heute, Morgen oder Übermorgen herausfinden
            int day = 0;
            if(splitId.Length > 1)
                Int32.TryParse(splitId[1], out day);

            // Alle Prognosen mit der gesuchten ID suchen
            var forecasts = Data.Where(forecast => forecast.ContainsName(name));

            // Wenn kein Eintrag gefunden wurde leerer String zurück geben
            if(forecasts.Count() <= 0)
                return String.Empty;

            // Globalstrahlung
            if(name == "irradience")
            {
                // Globalstrahlung um 12 Uhr ermitteln
                Forecast forecastGlobal = Data.Where(x => x.Day == 0 && x.DateOne.ToUniversalTime().Hour == 12).FirstOrDefault();

                // Falls der Wert gefunden wurde, dieser zurückgeben
                return (forecastGlobal != null) ? forecastGlobal["irradience"].Data.ToString() : String.Empty;
            }

            // Wert an einem bestimmten Tag suchen
            Forecast find = forecasts.Where(forecast => forecast.Day == day).FirstOrDefault();

            // Prüfen, ob zum dazugehörigen Tag ein Eintrag vorhanden ist
            if(find == null)
                return String.Empty;

            // Daten zurückgeben
            return find[name].Data;
        }
    }
}
