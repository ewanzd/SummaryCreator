using SummaryCreator.Resources;

namespace SummaryCreator.View
{
    partial class ConfigForm
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
            if (disposing && (components != null))
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
            this.lblRestTimeText = new System.Windows.Forms.Label();
            this.lblRestTime = new System.Windows.Forms.Label();
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
            // lblRestTimeText
            // 
            this.lblRestTimeText.AutoSize = true;
            this.lblRestTimeText.Location = new System.Drawing.Point(90, 294);
            this.lblRestTimeText.Name = "lblRestTimeText";
            this.lblRestTimeText.Size = new System.Drawing.Size(50, 20);
            this.lblRestTimeText.TabIndex = 0;
            this.lblRestTimeText.Text = Strings.ConfigForm_TimeLeft;
            // 
            // lblRestTime
            // 
            this.lblRestTime.Location = new System.Drawing.Point(20, 291);
            this.lblRestTime.Name = "lblRestTime";
            this.lblRestTime.Size = new System.Drawing.Size(60, 20);
            this.lblRestTime.TabIndex = 1;
            this.lblRestTime.BackColor = System.Drawing.Color.PaleVioletRed;
            this.lblRestTime.Padding = new System.Windows.Forms.Padding(3);
            this.lblRestTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.RestTime_Tick);
            // 
            // butTimeStatus
            // 
            this.butTimeStatus.Location = new System.Drawing.Point(495, 280);
            this.butTimeStatus.Name = "butTimeStatus";
            this.butTimeStatus.Size = new System.Drawing.Size(100, 35);
            this.butTimeStatus.TabIndex = 2;
            this.butTimeStatus.Text = Strings.ConfigForm_Stop;
            this.butTimeStatus.UseVisualStyleBackColor = true;
            this.butTimeStatus.Click += new System.EventHandler(this.ButTimeStatus_Click);
            // 
            // butSave
            // 
            this.butSave.Location = new System.Drawing.Point(500, 210);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(75, 25);
            this.butSave.TabIndex = 4;
            this.butSave.Text = Strings.ConfigForm_Save;
            this.butSave.UseVisualStyleBackColor = true;
            this.butSave.Click += new System.EventHandler(this.ButSave_Click);
            // 
            // gbData
            // 
            this.gbData.Controls.Add(this.lblStatus);
            this.gbData.Controls.Add(this.lblInfo);
            this.gbData.Controls.Add(this.lblIdRow);
            this.gbData.Controls.Add(this.lblTableName);
            this.gbData.Controls.Add(this.lblCounterDirectory);
            this.gbData.Controls.Add(this.lblXMLPath);
            this.gbData.Controls.Add(this.lblExcelPath);
            this.gbData.Controls.Add(this.txbCounterDirectory);
            this.gbData.Controls.Add(this.txbTableName);
            this.gbData.Controls.Add(this.txbIdRow);
            this.gbData.Controls.Add(this.txbXMLPath);
            this.gbData.Controls.Add(this.txbExcelPath);
            this.gbData.Controls.Add(this.butSave);
            this.gbData.Location = new System.Drawing.Point(13, 13);
            this.gbData.Margin = new System.Windows.Forms.Padding(5);
            this.gbData.Name = "gbData";
            this.gbData.AutoSize = true;
            this.gbData.TabIndex = 3;
            this.gbData.TabStop = false;
            this.gbData.Text = Strings.ConfigForm_Data;
            // 
            // txbIdRow
            // 
            this.txbIdRow.Location = new System.Drawing.Point(125, 179);
            this.txbIdRow.Name = "txbIdRow";
            this.txbIdRow.Size = new System.Drawing.Size(450, 20);
            this.txbIdRow.TabIndex = 9;
            this.txbIdRow.Click += new System.EventHandler(this.TxbField_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(22, 25);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(150, 13);
            this.lblInfo.TabIndex = 5;
            this.lblInfo.Text = Strings.ConfigForm_AdjustConfigurations;
            // 
            // lblIdRow
            // 
            this.lblIdRow.AutoSize = true;
            this.lblIdRow.Location = new System.Drawing.Point(7, 182);
            this.lblIdRow.Name = "lblIdRow";
            this.lblIdRow.Size = new System.Drawing.Size(65, 13);
            this.lblIdRow.TabIndex = 8;
            this.lblIdRow.Text = Strings.ConfigForm_ResultRowWithIds;
            // 
            // txbCounterDirectory
            // 
            this.txbCounterDirectory.Location = new System.Drawing.Point(125, 80);
            this.txbCounterDirectory.Name = "txbCounterDirectory";
            this.txbCounterDirectory.Size = new System.Drawing.Size(450, 20);
            this.txbCounterDirectory.TabIndex = 11;
            this.txbCounterDirectory.Click += new System.EventHandler(this.TxbField_Click);
            // 
            // txbTableName
            // 
            this.txbTableName.Location = new System.Drawing.Point(125, 153);
            this.txbTableName.Name = "txbTableName";
            this.txbTableName.Size = new System.Drawing.Size(450, 20);
            this.txbTableName.TabIndex = 7;
            this.txbTableName.Click += new System.EventHandler(this.TxbField_Click);
            // 
            // lblTableName
            // 
            this.lblTableName.AutoSize = true;
            this.lblTableName.Location = new System.Drawing.Point(7, 156);
            this.lblTableName.Name = "lblTableName";
            this.lblTableName.Size = new System.Drawing.Size(74, 13);
            this.lblTableName.TabIndex = 6;
            this.lblTableName.Text = Strings.ConfigForm_ResultTableName;
            // 
            // lblCounterDirectory
            // 
            this.lblCounterDirectory.AutoSize = true;
            this.lblCounterDirectory.Location = new System.Drawing.Point(7, 83);
            this.lblCounterDirectory.Name = "lblCounterDirectory";
            this.lblCounterDirectory.Size = new System.Drawing.Size(90, 13);
            this.lblCounterDirectory.TabIndex = 10;
            this.lblCounterDirectory.Text = Strings.ConfigForm_SensorSourceDirectory;
            // 
            // txbXMLPath
            // 
            this.txbXMLPath.Location = new System.Drawing.Point(125, 54);
            this.txbXMLPath.Name = "txbXMLPath";
            this.txbXMLPath.Size = new System.Drawing.Size(450, 20);
            this.txbXMLPath.TabIndex = 1;
            this.txbXMLPath.Click += new System.EventHandler(this.TxbField_Click);
            // 
            // lblXMLPath
            // 
            this.lblXMLPath.AutoSize = true;
            this.lblXMLPath.Location = new System.Drawing.Point(6, 57);
            this.lblXMLPath.Name = "lblXMLPath";
            this.lblXMLPath.Size = new System.Drawing.Size(78, 13);
            this.lblXMLPath.TabIndex = 0;
            this.lblXMLPath.Text = Strings.ConfigForm_MeteoSourceFile;
            // 
            // lblExcelPath
            // 
            this.lblExcelPath.AutoSize = true;
            this.lblExcelPath.Location = new System.Drawing.Point(7, 128);
            this.lblExcelPath.Name = "lblExcelPath";
            this.lblExcelPath.Size = new System.Drawing.Size(76, 13);
            this.lblExcelPath.TabIndex = 2;
            this.lblExcelPath.Text = Strings.ConfigForm_ResultFile;
            // 
            // txbExcelPath
            // 
            this.txbExcelPath.Location = new System.Drawing.Point(125, 125);
            this.txbExcelPath.Name = "txbExcelPath";
            this.txbExcelPath.Size = new System.Drawing.Size(450, 20);
            this.txbExcelPath.TabIndex = 3;
            this.txbExcelPath.Click += new System.EventHandler(this.TxbExcelPath_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Maroon;
            this.lblStatus.Location = new System.Drawing.Point(125, 214);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 25);
            this.lblStatus.TabIndex = 12;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Controls.Add(this.gbData);
            this.Controls.Add(this.butTimeStatus);
            this.Controls.Add(this.lblRestTime);
            this.Controls.Add(this.lblRestTimeText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConfigForm";
            this.Text = Strings.ConfigForm_Title;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Meteo_FormClosed);
            this.gbData.ResumeLayout(false);
            this.gbData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRestTimeText;
        private System.Windows.Forms.Label lblRestTime;
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