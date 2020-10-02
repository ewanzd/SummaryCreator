using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace SummaryCreator.View
{
    /// <summary>
    /// Manage 'Summary Creator'. You can set important settings and start the process.
    /// </summary>
    public partial class ConfigForm : Form, IConfigView
    {
        /// <summary>
        /// Time left until process is started.
        /// </summary>
        private TimeSpan restTime;

        /// <summary>
        /// Tick interval.
        /// </summary>
        private TimeSpan interval = new TimeSpan(0, 0, 0, 0, 100);

        /// <summary>
        /// Create form and start timer.
        /// </summary>
        public ConfigForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Tick interval.
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
        /// Read input data, check whether they are valid and save them.
        /// </summary>
        private void ButSave_Click(object sender, EventArgs e)
        {
            Presenter.OnStop();

            string excel = txbExcelPath.Text;
            string xml = txbXMLPath.Text;
            string id = txbIdRow.Text;

            if (string.IsNullOrWhiteSpace(excel) || string.IsNullOrWhiteSpace(xml))
            {
                txbExcelPath.BackColor = Color.RosyBrown;
                return;
            }
            else
            {
                txbExcelPath.BackColor = default;
            }

            if (Path.GetExtension(excel) != ".xlsx")
            {
                txbExcelPath.BackColor = Color.RosyBrown;
                return;
            }
            else
            {
                txbExcelPath.BackColor = default;
            }

            if (!int.TryParse(id, out _))
            {
                txbIdRow.BackColor = Color.RosyBrown;
                return;
            }
            else
            {
                txbIdRow.BackColor = default;
            }

            Presenter.OnSave();
        }

        /// <summary>
        /// Stop timer and start process.
        /// </summary>
        private async void ButTimeStatus_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                Presenter.OnStop();
            }
            else
            {
                await Presenter.OnRunAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Next tick was reached.
        /// </summary>
        private async void RestTime_Tick(object sender, EventArgs e)
        {
            if (restTime.TotalMilliseconds <= 0)
            {
                await Presenter.OnRunAsync().ConfigureAwait(false);
                Presenter.OnStop();
            }
            else
            {
                restTime = restTime.Subtract(interval);
                lblRestTime.Text = restTime.ToString(@"ss\:ff", CultureInfo.InvariantCulture);
            }
        }

        private void Meteo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Presenter.OnExit();
        }

        private void TxbField_Click(object sender, EventArgs e)
        {
            Presenter.OnStop();
        }
    }
}