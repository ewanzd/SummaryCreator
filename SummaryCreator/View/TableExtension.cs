using SummaryCreator.Basic;
using SummaryCreator.Export;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SummaryCreator
{
    public delegate bool CheckNameHandler(string name, ExcelTableSetting setting);

    public partial class TableExtension : Form
    {
        public event EventHandler<ObjectEventArgs<ExcelTableSetting>> Completed;
        public event CheckNameHandler CheckName;

        ExcelTableSetting setting = null;

        public TableExtension()
        {
            InitializeComponent();
        }

        public TableExtension(ExcelTableSetting setting) : this()
        {
            this.setting = setting;

            txbNameMyTable.Text = this.setting.Name;
            txbIntervalHourMyTable.Text = Math.Floor(this.setting.Interval.TotalHours).ToString();
            txbIntervalMinuteMyTable.Text = this.setting.Interval.Minutes.ToString();
            chbDifferenceMyTable.Checked = this.setting.DifferenceActive;

            if(this.setting.OutputUnit == Unit.None)
                this.rbMyTable.Checked = true;
            else if(this.setting.OutputUnit == Unit.Kilo)
                this.rbKiloMyTable.Checked = true;
            else if(this.setting.OutputUnit == Unit.Mega)
                this.rbMegaMyTable.Checked = true;
            else if(this.setting.OutputUnit == Unit.Giga)
                this.rbGigaMyTable.Checked = true;
            else
                this.rbMyTable.Checked = true;
        }

        private void butCancelMyTable_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butAddMyTable_Click(object sender, EventArgs e)
        {
            string name = String.Empty;
            Unit unit = Unit.None;
            int hour = 0;
            int min = 0;
            bool difference = false;

            bool error = false;

            // Name muss einen Wert enthalten und prüft, ob der Name bereits vorhanden ist
            if(String.IsNullOrWhiteSpace(txbNameMyTable.Text) || CheckName != null && CheckName(txbNameMyTable.Text, setting))
            {
                txbNameMyTable.BackColor = Color.RosyBrown;
                error = true;
            }
            else
            {
                name = txbNameMyTable.Text;
                txbNameMyTable.BackColor = default(Color);
            }

            // Ausgabetyp
            if(rbMyTable.Checked)
                unit = Unit.None;
            else if(rbKiloMyTable.Checked)
                unit = Unit.Kilo;
            else if(rbMegaMyTable.Checked)
                unit = Unit.Mega;
            else if(rbGigaMyTable.Checked)
                unit = Unit.Giga;

            // Stunden
            if(!Int32.TryParse(txbIntervalHourMyTable.Text, out hour) || hour < 0)
            {
                txbIntervalHourMyTable.BackColor = Color.RosyBrown;
                error = true;
            }
            else
            {
                txbIntervalHourMyTable.BackColor = default(Color);
            }

            // Minuten
            if(!Int32.TryParse(txbIntervalMinuteMyTable.Text, out min) || min >= 60 || min < 0)
            {
                txbIntervalMinuteMyTable.BackColor = Color.RosyBrown;
                error = true;
            }
            else
            {
                txbIntervalMinuteMyTable.BackColor = default(Color);
            }

            // Differenz
            difference = chbDifferenceMyTable.Checked;

            if(!error)
            {
                // Beide Werte dürfen nicht gleichzeitig 0 sein
                if(hour == 0 && min == 0)
                {
                    txbIntervalHourMyTable.BackColor = Color.RosyBrown;
                    txbIntervalMinuteMyTable.BackColor = Color.RosyBrown;
                    error = true;
                }
                else
                {
                    // Werte stimmen und können an das Aufrufobjekt zurückgegeben werden
                    if(setting == null)
                        setting = new ExcelTableSetting(name, new TimeSpan(hour, min, 0), unit, difference);
                    else
                    {
                        setting.Name = name;
                        setting.Interval = new TimeSpan(hour, min, 0);
                        setting.OutputUnit = unit;
                        setting.DifferenceActive = difference;
                    }
                    if(Completed != null)
                        Completed(this, new ObjectEventArgs<ExcelTableSetting>(setting));
                    this.Close();
                }
            }
        }
    }
}
