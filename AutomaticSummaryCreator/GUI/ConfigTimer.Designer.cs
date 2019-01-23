namespace AutomaticSummaryCreator.GUI
{
    partial class ConfigTimer
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
            this.components = new System.ComponentModel.Container();
            this.lblRestTime = new System.Windows.Forms.Label();
            this.txbRestTime = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.butTimeStatus = new System.Windows.Forms.Button();
            this.gbData = new System.Windows.Forms.GroupBox();
            this.txbIdRow = new System.Windows.Forms.TextBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblIdRow = new System.Windows.Forms.Label();
            this.txbCounterDirectory = new System.Windows.Forms.TextBox();
            this.txbTableName = new System.Windows.Forms.TextBox();
            this.butSave = new System.Windows.Forms.Button();
            this.lblTableName = new System.Windows.Forms.Label();
            this.lblCounterDirectory = new System.Windows.Forms.Label();
            this.txbXMLPath = new System.Windows.Forms.TextBox();
            this.lblXMLPath = new System.Windows.Forms.Label();
            this.lblExcelPath = new System.Windows.Forms.Label();
            this.txbExcelPath = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.gbData.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRestTime
            // 
            this.lblRestTime.AutoSize = true;
            this.lblRestTime.Location = new System.Drawing.Point(20, 265);
            this.lblRestTime.Name = "lblRestTime";
            this.lblRestTime.Size = new System.Drawing.Size(48, 13);
            this.lblRestTime.TabIndex = 0;
            this.lblRestTime.Text = "Restzeit:";
            // 
            // txbRestTime
            // 
            this.txbRestTime.Location = new System.Drawing.Point(72, 262);
            this.txbRestTime.Name = "txbRestTime";
            this.txbRestTime.ReadOnly = true;
            this.txbRestTime.Size = new System.Drawing.Size(50, 20);
            this.txbRestTime.TabIndex = 1;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.restTime_Tick);
            // 
            // butTimeStatus
            // 
            this.butTimeStatus.Location = new System.Drawing.Point(278, 259);
            this.butTimeStatus.Name = "butTimeStatus";
            this.butTimeStatus.Size = new System.Drawing.Size(75, 23);
            this.butTimeStatus.TabIndex = 2;
            this.butTimeStatus.Text = "Stop";
            this.butTimeStatus.UseVisualStyleBackColor = true;
            this.butTimeStatus.Click += new System.EventHandler(this.butTimeStatus_Click);
            // 
            // gbData
            // 
            this.gbData.Controls.Add(this.lblStatus);
            this.gbData.Controls.Add(this.txbIdRow);
            this.gbData.Controls.Add(this.lblInfo);
            this.gbData.Controls.Add(this.lblIdRow);
            this.gbData.Controls.Add(this.txbCounterDirectory);
            this.gbData.Controls.Add(this.txbTableName);
            this.gbData.Controls.Add(this.butSave);
            this.gbData.Controls.Add(this.lblTableName);
            this.gbData.Controls.Add(this.lblCounterDirectory);
            this.gbData.Controls.Add(this.txbXMLPath);
            this.gbData.Controls.Add(this.lblXMLPath);
            this.gbData.Controls.Add(this.lblExcelPath);
            this.gbData.Controls.Add(this.txbExcelPath);
            this.gbData.Location = new System.Drawing.Point(13, 13);
            this.gbData.Name = "gbData";
            this.gbData.Size = new System.Drawing.Size(346, 240);
            this.gbData.TabIndex = 3;
            this.gbData.TabStop = false;
            this.gbData.Text = "Daten";
            // 
            // txbIdRow
            // 
            this.txbIdRow.Location = new System.Drawing.Point(98, 179);
            this.txbIdRow.Name = "txbIdRow";
            this.txbIdRow.Size = new System.Drawing.Size(242, 20);
            this.txbIdRow.TabIndex = 9;
            this.txbIdRow.Click += new System.EventHandler(this.txbIdRow_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(22, 25);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(150, 13);
            this.lblInfo.TabIndex = 5;
            this.lblInfo.Text = "Konfigurationsdaten anpassen";
            // 
            // lblIdRow
            // 
            this.lblIdRow.AutoSize = true;
            this.lblIdRow.Location = new System.Drawing.Point(7, 182);
            this.lblIdRow.Name = "lblIdRow";
            this.lblIdRow.Size = new System.Drawing.Size(65, 13);
            this.lblIdRow.TabIndex = 8;
            this.lblIdRow.Text = "Zeile mit IDs";
            // 
            // txbCounterDirectory
            // 
            this.txbCounterDirectory.Location = new System.Drawing.Point(98, 80);
            this.txbCounterDirectory.Name = "txbCounterDirectory";
            this.txbCounterDirectory.Size = new System.Drawing.Size(242, 20);
            this.txbCounterDirectory.TabIndex = 11;
            this.txbCounterDirectory.Click += new System.EventHandler(this.txbCounterDirectory_Click);
            // 
            // txbTableName
            // 
            this.txbTableName.Location = new System.Drawing.Point(98, 153);
            this.txbTableName.Name = "txbTableName";
            this.txbTableName.Size = new System.Drawing.Size(242, 20);
            this.txbTableName.TabIndex = 7;
            this.txbTableName.Click += new System.EventHandler(this.txbTableName_Click);
            // 
            // butSave
            // 
            this.butSave.Location = new System.Drawing.Point(265, 205);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(75, 23);
            this.butSave.TabIndex = 4;
            this.butSave.Text = "Speichern";
            this.butSave.UseVisualStyleBackColor = true;
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // lblTableName
            // 
            this.lblTableName.AutoSize = true;
            this.lblTableName.Location = new System.Drawing.Point(7, 156);
            this.lblTableName.Name = "lblTableName";
            this.lblTableName.Size = new System.Drawing.Size(74, 13);
            this.lblTableName.TabIndex = 6;
            this.lblTableName.Text = "Tabellenname";
            // 
            // lblCounterDirectory
            // 
            this.lblCounterDirectory.AutoSize = true;
            this.lblCounterDirectory.Location = new System.Drawing.Point(7, 83);
            this.lblCounterDirectory.Name = "lblCounterDirectory";
            this.lblCounterDirectory.Size = new System.Drawing.Size(90, 13);
            this.lblCounterDirectory.TabIndex = 10;
            this.lblCounterDirectory.Text = "Quellordner (.csv)";
            // 
            // txbXMLPath
            // 
            this.txbXMLPath.Location = new System.Drawing.Point(98, 54);
            this.txbXMLPath.Name = "txbXMLPath";
            this.txbXMLPath.Size = new System.Drawing.Size(242, 20);
            this.txbXMLPath.TabIndex = 1;
            this.txbXMLPath.Click += new System.EventHandler(this.txbXMLPath_Click);
            // 
            // lblXMLPath
            // 
            this.lblXMLPath.AutoSize = true;
            this.lblXMLPath.Location = new System.Drawing.Point(6, 57);
            this.lblXMLPath.Name = "lblXMLPath";
            this.lblXMLPath.Size = new System.Drawing.Size(78, 13);
            this.lblXMLPath.TabIndex = 0;
            this.lblXMLPath.Text = "Quelldatei (xml)";
            // 
            // lblExcelPath
            // 
            this.lblExcelPath.AutoSize = true;
            this.lblExcelPath.Location = new System.Drawing.Point(7, 128);
            this.lblExcelPath.Name = "lblExcelPath";
            this.lblExcelPath.Size = new System.Drawing.Size(76, 13);
            this.lblExcelPath.TabIndex = 2;
            this.lblExcelPath.Text = "Zieldatei (.xlsx)";
            // 
            // txbExcelPath
            // 
            this.txbExcelPath.Location = new System.Drawing.Point(98, 125);
            this.txbExcelPath.Name = "txbExcelPath";
            this.txbExcelPath.Size = new System.Drawing.Size(242, 20);
            this.txbExcelPath.TabIndex = 3;
            this.txbExcelPath.Click += new System.EventHandler(this.txbExcelPath_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Maroon;
            this.lblStatus.Location = new System.Drawing.Point(22, 210);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 12;
            // 
            // ConfigTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 292);
            this.Controls.Add(this.gbData);
            this.Controls.Add(this.butTimeStatus);
            this.Controls.Add(this.txbRestTime);
            this.Controls.Add(this.lblRestTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConfigTimer";
            this.Text = "Meteodaten abrufen";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Meteo_FormClosed);
            this.gbData.ResumeLayout(false);
            this.gbData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRestTime;
        private System.Windows.Forms.TextBox txbRestTime;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button butTimeStatus;
        private System.Windows.Forms.GroupBox gbData;
        private System.Windows.Forms.Label lblXMLPath;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.TextBox txbExcelPath;
        private System.Windows.Forms.Label lblExcelPath;
        private System.Windows.Forms.TextBox txbXMLPath;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txbCounterDirectory;
        private System.Windows.Forms.Label lblCounterDirectory;
        private System.Windows.Forms.TextBox txbIdRow;
        private System.Windows.Forms.Label lblIdRow;
        private System.Windows.Forms.TextBox txbTableName;
        private System.Windows.Forms.Label lblTableName;
        private System.Windows.Forms.Label lblStatus;
    }
}