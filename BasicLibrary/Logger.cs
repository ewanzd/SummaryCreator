using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.CompilerServices;

namespace BasicLibrary
{
    /// <summary>
    /// Log a message to a log file.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// The path to the log file.
        /// </summary>
        public static String LogFilePath = String.Empty;

        /// <summary>
        /// Write a text in a log file.
        /// </summary>
        /// <param name="message">The message, that will be write in the log file.</param>
        /// <param name="extensionInfo">Add the caller informationen.</param>
        /// <param name="memberName">The member from caller.</param>
        /// <param name="sourceFilePath">The source path from caller.</param>
        /// <param name="sourceLineNumber">The line number from caller.</param>
        public static bool Trace(string message, bool extensionInfo = false, [CallerMemberName]string memberName = "", [CallerFilePath]string sourceFilePath = "", [CallerLineNumber]int sourceLineNumber = 0)
        {
            // Speichert den Pfad, da ansonsten die Gefahr besteht, dass er währenddessen die Methode ausgeführt geändert wird
            string path = Logger.LogFilePath;

            // Prüft, ob ein Pfad angegeben wurde
            if(String.IsNullOrEmpty(path))
                return false;

            try
            {
                // Behälter für alle bestehende Zeile in der Logdatei
                StringCollection changedEntriesCol = new StringCollection();

                // Prüft, ob die Datei existiert. Falls ja werden alle Zeile gespeichert
                if(File.Exists(path))
                {
                    string line = "";
                    int count = 1;

                    // Stream zur Datei öffnen
                    using(StreamReader sr = new StreamReader(path))
                    {
                        // Alle Zeilen lesen
                        while((line = sr.ReadLine()) != null)
                        {
                            // Maximal 999 Einträge lesen 
                            if(count == 999)
                                break;

                            // Zeile hinzufügen
                            changedEntriesCol.Add(line);

                            // Anzahl Zeilen um 1 erhöhen
                            count++;
                        }
                    }
                }

                // Neuer Einträg erstellen
                string text = String.Format("Zeit: {0}; Nachricht: {1};", DateTime.Now.ToString(), message);

                // Aufruferinformationen hinzufügen
                if(extensionInfo)
                    text += String.Format("{0}Attributname: {1}{0}Pfad: {2}{0}Zeilennummer: {3};", Environment.NewLine, memberName, sourceFilePath, sourceLineNumber);

                // Eintrag hinzufügen
                changedEntriesCol.Insert(0, text);

                // Schreibestream öffnen und alle Einträge schreiben
                FileStream fs = new FileStream(path, FileMode.Create);
                using(StreamWriter sw = new StreamWriter(fs))
                    foreach(string str in changedEntriesCol)
                        sw.WriteLine(str);

                // Erfolgreich abgeschlossen
                return true;
            }
            catch
            {
                // Ein Fehler ist aufgetreten, der Eintrag konnte nicht erfolgreich hinzugefügt werden
                return false;
            }
        }

        /// <summary>
        /// Write a text in a log file.
        /// </summary>
        /// <param name="message">The exception, that will be write in the log file.</param>
        /// <param name="extensionInfo">Add the caller informationen.</param>
        /// <param name="memberName">The member from caller.</param>
        /// <param name="sourceFilePath">The source path from caller.</param>
        /// <param name="sourceLineNumber">The line number from caller.</param>
        public static bool Trace(Exception exception, bool extensionInfo = true, [CallerMemberName]string memberName = "", [CallerFilePath]string sourceFilePath = "", [CallerLineNumber]int sourceLineNumber = 0)
        {
            // Fehlertext erstellen
            string exceptionMessage = "Es ist ein Fehler aufgetreten;";

            // Fehlermeldung hinzufügen
            while(exception != null)
            {
                exceptionMessage += String.Format("{0}Exception: {1}{0}Stack: {2}", Environment.NewLine, exception.Message, exception.StackTrace);
                exception = exception.InnerException;
            }

            // Fehlermeldung übergeben
            return Trace(exceptionMessage, extensionInfo, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}
