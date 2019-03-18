using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomaticSummaryCreator.GUI
{
    /// <summary>
    /// Startet einen Timer, der nach der angegeben Zeit einen Vorgang startet.
    /// </summary>
    public partial class ConfigTimer : Form
    {
        /// <summary>
        /// Wichtige Konfigurationsdaten für den Vorgang.
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// Verfügbare Zeit, bis der Vorgang gestartet wird.
        /// </summary>
        private int sec;

        /// <summary>
        /// Restliche Zeit, bis der Vorgang gestartet wird.
        /// </summary>
        private TimeSpan restTime;

        /// <summary>
        /// Intervall, in dem der Tick ausgeführt wird.
        /// </summary>
        private TimeSpan interval = new TimeSpan(0, 0, 0, 0, 100);

        /// <summary>
        /// Den Intervall abrufen oder anpassen.
        /// </summary>
        public int Interval
        {
            get
            {
                return Convert.ToInt32(interval.TotalMilliseconds);
            }
            protected set
            {
                interval = new TimeSpan(0, 0, 0, 0, value);
            }
        }

        /// <summary>
        /// Startet über dieses Event den Vorgang.
        /// </summary>
        public event EventHandler Start;

        /// <summary>
        /// Die Form wurde geschlossen.
        /// </summary>
        public event EventHandler Exit;

        /// <summary>
        /// Startet die Form mit seinem Timer.
        /// </summary>
        /// <param name="sec">Verfügbare Zeit, bis der Vorgang gestartet wird.</param>
        public ConfigTimer(int sec, Configuration ini)
        {
            if(ini == null)
                throw new ArgumentNullException("ini");

            // Verfügbare Zeit kann nicht unter null sein
            if(sec < 0)
                throw new ArgumentOutOfRangeException("sec");

            // Komponente initialisieren
            InitializeComponent();

            // Ini-Datei setzen
            this.configuration = ini;

            // Timerdaten setzen
            this.sec = sec;
            restTime = new TimeSpan(0, 0, sec);

            // Bestehende Konfigurationen einfügen
            txbExcelPath.Text = ini.ExcelPath;
            txbXMLPath.Text = ini.XmlPath;
            txbTableName.Text = ini.SheetName;
            txbIdRow.Text = ini.SheetIdRow.ToString();
            txbCounterDirectory.Text = ini.ExcelSourceDirectory;

            // Timer starten
            timer.Start();
        }

        /// <summary>
        /// Die Daten werden überprüft und gespeichert, wenn sie gültig sind.
        /// </summary>
        private void butSave_Click(object sender, EventArgs e)
        {
            // Timer stoppen
            Stop();

            // Eingegebene Daten abrufen
            string excel = txbExcelPath.Text;
            string xml = txbXMLPath.Text;
            string table = txbTableName.Text;
            string id = txbIdRow.Text;
            string counter = txbCounterDirectory.Text;

            // Prüfen, ob Daten eingegeben wurde
            if(String.IsNullOrWhiteSpace(excel) || String.IsNullOrWhiteSpace(xml))
            {
                txbExcelPath.BackColor = Color.RosyBrown;
                return;
            }
            else
            {
                txbExcelPath.BackColor = default(Color);
            }

            // Prüfen, ob die Excel-Datei die richtige Endung besitzt
            if(Path.GetExtension(excel) != ".xlsx")
            {
                txbExcelPath.BackColor = Color.RosyBrown;
                return;
            }
            else
            {
                txbExcelPath.BackColor = default(Color);
            }

            int idInt;
            if(!Int32.TryParse(id, out idInt))
            {
                txbIdRow.BackColor = Color.RosyBrown;
                return;
            }
            else
            {
                txbIdRow.BackColor = default(Color);
            }

            // Daten übernehmen
            configuration.ExcelPath = excel;
            configuration.XmlPath = xml;
            configuration.SheetName = table;
            configuration.SheetIdRow = idInt;
            configuration.ExcelSourceDirectory = counter;

            // Daten abspeichern
            configuration.Save();
        }

        /// <summary>
        /// Timer stoppen/Vorgang starten.
        /// </summary>
        private void butTimeStatus_Click(object sender, EventArgs e)
        {
            if(timer.Enabled)
            {
                Stop();
            }
            else
            {
                // Vorgang starten
                if(Start != null)
                {
                    lblStatus.Text = "Wird ausgewertet....";
                    Start(this, EventArgs.Empty);
                }
            }
        }

        public void Stop()
        {
            // Timer stoppen
            timer.Stop();
            butTimeStatus.Text = "Ausführen";
        }

        /// <summary>
        /// Der nächste Tick wurde erreicht.
        /// </summary>
        private void restTime_Tick(object sender, EventArgs e)
        {
            if(restTime.TotalMilliseconds <= 0)
            {
                // Vorgang starten
                timer.Stop();
                if(Start != null)
                    Start(this, EventArgs.Empty);
            }
            else
            {
                // Restzeit anpassen
                restTime = restTime.Subtract(interval);
                txbRestTime.Text = String.Format("{0}", restTime.ToString(@"ss\:ff"));
            }
        }

        /// <summary>
        /// Fenster wurde geschlossen.
        /// </summary>
        private void Meteo_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Bescheid geben
            if(Exit != null)
                Exit(this, EventArgs.Empty);
        }

        private void txbExcelPath_Click(object sender, EventArgs e)
        {
            // Timer stoppen
            Stop();

            // Datei auswählen
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            ofd.Filter = "Excel-Dateien (*.xlsx)|*.xlsx";
            ofd.FilterIndex = 1;
            if(ofd.ShowDialog() == DialogResult.OK)
                txbExcelPath.Text = ofd.FileName;
        }

        private void txbXMLPath_Click(object sender, EventArgs e)
        {
            // Timer stoppen
            Stop();
        }

        private void txbCounterDirectory_Click(object sender, EventArgs e)
        {
            // Timer stoppen
            Stop();
        }

        private void txbTableName_Click(object sender, EventArgs e)
        {
            // Timer stoppen
            Stop();
        }

        private void txbIdRow_Click(object sender, EventArgs e)
        {
            // Timer stoppen
            Stop();
        }
    }
}
