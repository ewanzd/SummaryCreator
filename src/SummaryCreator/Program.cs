using SummaryCreator.Services;
using SummaryCreator.View;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SummaryCreator
{
    internal static class Program
    {
        /// <summary>
        /// Der Pfad zu der Ini-Datei, wo alle weitere Konfigurationen gespeichert werden.
        /// </summary>
        private static string IniPath = Path.Combine(Application.StartupPath, "SummaryCreator.ini");

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            // Prüfen, ob ein neuer Ini-Pfad mitgegeben wurde
            string path = args.Where(x => x.Split(':')[0] == "IniPath").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(path))
            {
                IniPath = path;
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Objekte zusammensetzen
            var dataService = new DataService();
            var configView = new ConfigForm();
            var configuration = new Configuration(IniPath);
            var configPresenter = new ConfigPresenter(configView, dataService, configuration);

            // Fenster öffnen
            configView.Show();

            // Applikation laufen lassen bis Exit aufgerufen wird
            Application.Run();
        }
    }
}