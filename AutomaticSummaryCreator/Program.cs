using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AutomaticSummaryCreator
{
    static class Program
    {
        /// <summary>
        /// Der Pfad zu der Ini-Datei, wo alle weitere Konfigurationen gespeichert werden.
        /// </summary>
        private static string IniPath = Path.Combine(Application.StartupPath, "SummaryCreator.ini");

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Prüfen, ob ein neuer Ini-Pfad mitgegeben wurde
            string path = args.Where(x => x.Split(':')[0] == "IniPath").FirstOrDefault();
            if(!string.IsNullOrWhiteSpace(path))
            {
                IniPath = path;
            }
                
            // Prüfen, ob die Sekunden mitgegeben wurde, die der Benutzer Zeit hat, um die Konfigurationen zu bearbeiten
            int startSec;
            if(!int.TryParse(args.Where(x => x.Split(':')[0] == "RestTime").FirstOrDefault(), out startSec))
            {
                startSec = 10;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new SummaryCreatorApplication(IniPath, startSec);
            Application.Run();
        }
    }
}
