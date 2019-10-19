using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace SummaryCreator.View
{
    /// <summary>
    /// Startet einen Timer, der nach der angegeben Zeit einen Vorgang startet.
    /// </summary>
    public partial class ConfigForm : Form, IConfigView
    {
        /// <summary>
        /// Restliche Zeit, bis der Vorgang gestartet wird.
        /// </summary>
        private TimeSpan restTime;

        /// <summary>
        /// Intervall, in dem der Tick ausgeführt wird.
        /// </summary>
        private TimeSpan interval = new TimeSpan(0, 0, 0, 0, 100);

        /// <summary>
        /// Startet die Form mit seinem Timer.
        /// </summary>
        public ConfigForm()
        {
            // Komponente initialisieren
            InitializeComponent();
        }

        /// <summary>
        /// Den Intervall abrufen oder anpassen.
        /// </summary>
        public int TimerInterval {
            get {
                return Convert.ToInt32(interval.TotalMilliseconds);
            }
            protected set {
                interval = new TimeSpan(0, 0, 0, 0, value);
            }
        }

        public ConfigPresenter Presenter { get; set; }

        public string ExcelPath { get => txbExcelPath.Text; set => txbExcelPath.Text = value; }
        public string MeteoPath { get => txbXMLPath.Text; set => txbXMLPath.Text = value; }
        public string SensorDirectoryPath { get => txbCounterDirectory.Text; set => txbCounterDirectory.Text = value; }
        public string TableName { get => txbTableName.Text; set => txbTableName.Text = value; }
        public int IdRow { get => int.Parse(txbIdRow.Text); set => txbIdRow.Text = value.ToString(); }

        public bool TimerIsEnabled { get => timer.Enabled; set => timer.Enabled = value; }
        public string Status { set => lblStatus.Text = value; }
        public double RemainingTime { get => restTime.TotalSeconds; set => restTime = new TimeSpan(0, 0, 0, (int)value); }
        public bool ActionButtonEnabled { get => butTimeStatus.Enabled; set => butTimeStatus.Enabled = value; }
        public string ActionButtonText { get => butTimeStatus.Text; set => butTimeStatus.Text = value; }

        /// <summary>
        /// Die Daten werden überprüft und gespeichert, wenn sie gültig sind.
        /// </summary>
        private void ButSave_Click(object sender, EventArgs e)
        {
            // Timer stoppen
            Presenter.OnStop();

            // Eingegebene Daten abrufen
            string excel = txbExcelPath.Text;
            string xml = txbXMLPath.Text;
            string table = txbTableName.Text;
            string id = txbIdRow.Text;
            string counter = txbCounterDirectory.Text;

            // Prüfen, ob Daten eingegeben wurde
            if (string.IsNullOrWhiteSpace(excel) || string.IsNullOrWhiteSpace(xml))
            {
                txbExcelPath.BackColor = Color.RosyBrown;
                return;
            }
            else
            {
                txbExcelPath.BackColor = default(Color);
            }

            // Prüfen, ob die Excel-Datei die richtige Endung besitzt
            if (Path.GetExtension(excel) != ".xlsx")
            {
                txbExcelPath.BackColor = Color.RosyBrown;
                return;
            }
            else
            {
                txbExcelPath.BackColor = default(Color);
            }

            int idInt;
            if (!int.TryParse(id, out idInt))
            {
                txbIdRow.BackColor = Color.RosyBrown;
                return;
            }
            else
            {
                txbIdRow.BackColor = default(Color);
            }

            Presenter.OnSave();
        }

        /// <summary>
        /// Timer stoppen/Vorgang starten.
        /// </summary>
        private void ButTimeStatus_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                Presenter.OnStop();
            }
            else
            {
                // Vorgang starten
                Presenter.OnRun();
            }
        }

        /// <summary>
        /// Der nächste Tick wurde erreicht.
        /// </summary>
        private void RestTime_Tick(object sender, EventArgs e)
        {
            if (restTime.TotalMilliseconds <= 0)
            {
                // Vorgang starten
                Presenter.OnRun();
            }
            else
            {
                // Restzeit anpassen
                restTime = restTime.Subtract(interval);
                lblRestTime.Text = restTime.ToString(@"ss\:ff", CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Fenster wurde geschlossen.
        /// </summary>
        private void Meteo_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Bescheid geben
            Presenter.OnExit();
        }

        private void TxbExcelPath_Click(object sender, EventArgs e)
        {
            // Timer stoppen
            Presenter.OnStop();

            // Datei auswählen
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            ofd.Filter = "Excel-Dateien (*.xlsx)|*.xlsx";
            ofd.FilterIndex = 1;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txbExcelPath.Text = ofd.FileName;
            }
        }

        private void TxbField_Click(object sender, EventArgs e)
        {
            // Timer stoppen
            Presenter.OnStop();
        }
    }
}