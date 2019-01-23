namespace SummaryCreator
{
    partial class TableExtension
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbMyTable = new System.Windows.Forms.GroupBox();
            this.txbNameMyTable = new System.Windows.Forms.TextBox();
            this.lblNameMyTable = new System.Windows.Forms.Label();
            this.chbDifferenceMyTable = new System.Windows.Forms.CheckBox();
            this.lblDifferenceMyTable = new System.Windows.Forms.Label();
            this.txbIntervalMinuteMyTable = new System.Windows.Forms.TextBox();
            this.lblIntervalMinuteMyTable = new System.Windows.Forms.Label();
            this.txbIntervalHourMyTable = new System.Windows.Forms.TextBox();
            this.lblIntervalHourMyTable = new System.Windows.Forms.Label();
            this.lblIntervalMyTable = new System.Windows.Forms.Label();
            this.rbGigaMyTable = new System.Windows.Forms.RadioButton();
            this.lblUnitMyTable = new System.Windows.Forms.Label();
            this.rbMegaMyTable = new System.Windows.Forms.RadioButton();
            this.rbKiloMyTable = new System.Windows.Forms.RadioButton();
            this.rbMyTable = new System.Windows.Forms.RadioButton();
            this.butCancelMyTable = new System.Windows.Forms.Button();
            this.butAddMyTable = new System.Windows.Forms.Button();
            this.gbMyTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMyTable
            // 
            this.gbMyTable.Controls.Add(this.txbNameMyTable);
            this.gbMyTable.Controls.Add(this.lblNameMyTable);
            this.gbMyTable.Controls.Add(this.chbDifferenceMyTable);
            this.gbMyTable.Controls.Add(this.lblDifferenceMyTable);
            this.gbMyTable.Controls.Add(this.txbIntervalMinuteMyTable);
            this.gbMyTable.Controls.Add(this.lblIntervalMinuteMyTable);
            this.gbMyTable.Controls.Add(this.txbIntervalHourMyTable);
            this.gbMyTable.Controls.Add(this.lblIntervalHourMyTable);
            this.gbMyTable.Controls.Add(this.lblIntervalMyTable);
            this.gbMyTable.Controls.Add(this.rbGigaMyTable);
            this.gbMyTable.Controls.Add(this.lblUnitMyTable);
            this.gbMyTable.Controls.Add(this.rbMegaMyTable);
            this.gbMyTable.Controls.Add(this.rbKiloMyTable);
            this.gbMyTable.Controls.Add(this.rbMyTable);
            this.gbMyTable.Location = new System.Drawing.Point(12, 12);
            this.gbMyTable.Name = "gbMyTable";
            this.gbMyTable.Size = new System.Drawing.Size(224, 238);
            this.gbMyTable.TabIndex = 8;
            this.gbMyTable.TabStop = false;
            this.gbMyTable.Text = "Konfiguration";
            // 
            // txbNameMyTable
            // 
            this.txbNameMyTable.Location = new System.Drawing.Point(6, 37);
            this.txbNameMyTable.Name = "txbNameMyTable";
            this.txbNameMyTable.Size = new System.Drawing.Size(206, 20);
            this.txbNameMyTable.TabIndex = 20;
            // 
            // lblNameMyTable
            // 
            this.lblNameMyTable.Location = new System.Drawing.Point(22, 18);
            this.lblNameMyTable.Name = "lblNameMyTable";
            this.lblNameMyTable.Size = new System.Drawing.Size(295, 18);
            this.lblNameMyTable.TabIndex = 19;
            this.lblNameMyTable.Text = "Name der Tabelle (muss einmalig sein)";
            // 
            // chbDifferenceMyTable
            // 
            this.chbDifferenceMyTable.AutoSize = true;
            this.chbDifferenceMyTable.Location = new System.Drawing.Point(6, 210);
            this.chbDifferenceMyTable.Name = "chbDifferenceMyTable";
            this.chbDifferenceMyTable.Size = new System.Drawing.Size(68, 17);
            this.chbDifferenceMyTable.TabIndex = 18;
            this.chbDifferenceMyTable.Text = "Differenz";
            this.chbDifferenceMyTable.UseVisualStyleBackColor = true;
            // 
            // lblDifferenceMyTable
            // 
            this.lblDifferenceMyTable.Location = new System.Drawing.Point(28, 188);
            this.lblDifferenceMyTable.Name = "lblDifferenceMyTable";
            this.lblDifferenceMyTable.Size = new System.Drawing.Size(184, 19);
            this.lblDifferenceMyTable.TabIndex = 17;
            this.lblDifferenceMyTable.Text = "Differenz anzeigen";
            // 
            // txbIntervalMinuteMyTable
            // 
            this.txbIntervalMinuteMyTable.Location = new System.Drawing.Point(181, 150);
            this.txbIntervalMinuteMyTable.Name = "txbIntervalMinuteMyTable";
            this.txbIntervalMinuteMyTable.Size = new System.Drawing.Size(31, 20);
            this.txbIntervalMinuteMyTable.TabIndex = 16;
            this.txbIntervalMinuteMyTable.Text = "0";
            this.txbIntervalMinuteMyTable.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblIntervalMinuteMyTable
            // 
            this.lblIntervalMinuteMyTable.AutoSize = true;
            this.lblIntervalMinuteMyTable.Location = new System.Drawing.Point(119, 153);
            this.lblIntervalMinuteMyTable.Name = "lblIntervalMinuteMyTable";
            this.lblIntervalMinuteMyTable.Size = new System.Drawing.Size(48, 13);
            this.lblIntervalMinuteMyTable.TabIndex = 15;
            this.lblIntervalMinuteMyTable.Text = "Minuten:";
            // 
            // txbIntervalHourMyTable
            // 
            this.txbIntervalHourMyTable.Location = new System.Drawing.Point(71, 150);
            this.txbIntervalHourMyTable.Name = "txbIntervalHourMyTable";
            this.txbIntervalHourMyTable.Size = new System.Drawing.Size(31, 20);
            this.txbIntervalHourMyTable.TabIndex = 14;
            this.txbIntervalHourMyTable.Text = "1";
            this.txbIntervalHourMyTable.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblIntervalHourMyTable
            // 
            this.lblIntervalHourMyTable.AutoSize = true;
            this.lblIntervalHourMyTable.Location = new System.Drawing.Point(3, 153);
            this.lblIntervalHourMyTable.Name = "lblIntervalHourMyTable";
            this.lblIntervalHourMyTable.Size = new System.Drawing.Size(50, 13);
            this.lblIntervalHourMyTable.TabIndex = 13;
            this.lblIntervalHourMyTable.Text = "Stunden:";
            // 
            // lblIntervalMyTable
            // 
            this.lblIntervalMyTable.Location = new System.Drawing.Point(25, 126);
            this.lblIntervalMyTable.Name = "lblIntervalMyTable";
            this.lblIntervalMyTable.Size = new System.Drawing.Size(187, 19);
            this.lblIntervalMyTable.TabIndex = 12;
            this.lblIntervalMyTable.Text = "Intervall festlegen";
            // 
            // rbGigaMyTable
            // 
            this.rbGigaMyTable.AutoSize = true;
            this.rbGigaMyTable.Location = new System.Drawing.Point(165, 92);
            this.rbGigaMyTable.Name = "rbGigaMyTable";
            this.rbGigaMyTable.Size = new System.Drawing.Size(50, 17);
            this.rbGigaMyTable.TabIndex = 11;
            this.rbGigaMyTable.Text = "GWh";
            this.rbGigaMyTable.UseVisualStyleBackColor = true;
            // 
            // lblUnitMyTable
            // 
            this.lblUnitMyTable.Location = new System.Drawing.Point(25, 72);
            this.lblUnitMyTable.Name = "lblUnitMyTable";
            this.lblUnitMyTable.Size = new System.Drawing.Size(187, 17);
            this.lblUnitMyTable.TabIndex = 9;
            this.lblUnitMyTable.Text = "Einheit der Ausgabe bestimmen";
            // 
            // rbMegaMyTable
            // 
            this.rbMegaMyTable.AutoSize = true;
            this.rbMegaMyTable.Location = new System.Drawing.Point(108, 92);
            this.rbMegaMyTable.Name = "rbMegaMyTable";
            this.rbMegaMyTable.Size = new System.Drawing.Size(51, 17);
            this.rbMegaMyTable.TabIndex = 10;
            this.rbMegaMyTable.Text = "MWh";
            this.rbMegaMyTable.UseVisualStyleBackColor = true;
            // 
            // rbKiloMyTable
            // 
            this.rbKiloMyTable.AutoSize = true;
            this.rbKiloMyTable.Location = new System.Drawing.Point(54, 92);
            this.rbKiloMyTable.Name = "rbKiloMyTable";
            this.rbKiloMyTable.Size = new System.Drawing.Size(48, 17);
            this.rbKiloMyTable.TabIndex = 8;
            this.rbKiloMyTable.Text = "kWh";
            this.rbKiloMyTable.UseVisualStyleBackColor = true;
            // 
            // rbMyTable
            // 
            this.rbMyTable.AutoSize = true;
            this.rbMyTable.Checked = true;
            this.rbMyTable.Location = new System.Drawing.Point(6, 92);
            this.rbMyTable.Name = "rbMyTable";
            this.rbMyTable.Size = new System.Drawing.Size(42, 17);
            this.rbMyTable.TabIndex = 7;
            this.rbMyTable.TabStop = true;
            this.rbMyTable.Text = "Wh";
            this.rbMyTable.UseVisualStyleBackColor = true;
            // 
            // butCancelMyTable
            // 
            this.butCancelMyTable.Location = new System.Drawing.Point(60, 256);
            this.butCancelMyTable.Name = "butCancelMyTable";
            this.butCancelMyTable.Size = new System.Drawing.Size(85, 23);
            this.butCancelMyTable.TabIndex = 22;
            this.butCancelMyTable.Text = "Abbrechen";
            this.butCancelMyTable.UseVisualStyleBackColor = true;
            this.butCancelMyTable.Click += new System.EventHandler(this.butCancelMyTable_Click);
            // 
            // butAddMyTable
            // 
            this.butAddMyTable.Location = new System.Drawing.Point(151, 256);
            this.butAddMyTable.Name = "butAddMyTable";
            this.butAddMyTable.Size = new System.Drawing.Size(85, 23);
            this.butAddMyTable.TabIndex = 21;
            this.butAddMyTable.Text = "Speichern";
            this.butAddMyTable.UseVisualStyleBackColor = true;
            this.butAddMyTable.Click += new System.EventHandler(this.butAddMyTable_Click);
            // 
            // TableExtension
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 284);
            this.Controls.Add(this.butCancelMyTable);
            this.Controls.Add(this.gbMyTable);
            this.Controls.Add(this.butAddMyTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TableExtension";
            this.Text = "Tabellen-Konfiguration";
            this.gbMyTable.ResumeLayout(false);
            this.gbMyTable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMyTable;
        private System.Windows.Forms.TextBox txbNameMyTable;
        private System.Windows.Forms.Label lblNameMyTable;
        private System.Windows.Forms.CheckBox chbDifferenceMyTable;
        private System.Windows.Forms.Label lblDifferenceMyTable;
        private System.Windows.Forms.TextBox txbIntervalMinuteMyTable;
        private System.Windows.Forms.Label lblIntervalMinuteMyTable;
        private System.Windows.Forms.TextBox txbIntervalHourMyTable;
        private System.Windows.Forms.Label lblIntervalHourMyTable;
        private System.Windows.Forms.Label lblIntervalMyTable;
        private System.Windows.Forms.RadioButton rbGigaMyTable;
        private System.Windows.Forms.Label lblUnitMyTable;
        private System.Windows.Forms.RadioButton rbMegaMyTable;
        private System.Windows.Forms.RadioButton rbKiloMyTable;
        private System.Windows.Forms.RadioButton rbMyTable;
        private System.Windows.Forms.Button butCancelMyTable;
        private System.Windows.Forms.Button butAddMyTable;
    }
}