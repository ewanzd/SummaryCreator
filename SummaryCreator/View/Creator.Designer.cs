namespace SummaryCreator
{
    partial class Creator
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Creator));
            this.butCreate = new System.Windows.Forms.Button();
            this.butLoad = new System.Windows.Forms.Button();
            this.txbStatus = new System.Windows.Forms.TextBox();
            this.butNew = new System.Windows.Forms.Button();
            this.lvSettings = new System.Windows.Forms.ListView();
            this.lvTables = new System.Windows.Forms.ListView();
            this.lblTables = new System.Windows.Forms.Label();
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tpStandard = new System.Windows.Forms.TabPage();
            this.picElektroplan = new System.Windows.Forms.PictureBox();
            this.lblTitel = new System.Windows.Forms.Label();
            this.picSmartme = new System.Windows.Forms.PictureBox();
            this.gbStandardHelp = new System.Windows.Forms.GroupBox();
            this.lblStandardHelp = new System.Windows.Forms.Label();
            this.tpTables = new System.Windows.Forms.TabPage();
            this.butToDown = new System.Windows.Forms.Button();
            this.butToTop = new System.Windows.Forms.Button();
            this.gpTableHelp = new System.Windows.Forms.GroupBox();
            this.lblTableHelp = new System.Windows.Forms.Label();
            this.butTableRemove = new System.Windows.Forms.Button();
            this.butTableGroup = new System.Windows.Forms.Button();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.butSettingSave = new System.Windows.Forms.Button();
            this.butSettingEdit = new System.Windows.Forms.Button();
            this.gpSettingHelp = new System.Windows.Forms.GroupBox();
            this.lblSettingHelp = new System.Windows.Forms.Label();
            this.lblSettings = new System.Windows.Forms.Label();
            this.butSettingRemove = new System.Windows.Forms.Button();
            this.butSettingAdd = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sTARTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smartmeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.überSummaryCreatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbMain.SuspendLayout();
            this.tpStandard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picElektroplan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSmartme)).BeginInit();
            this.gbStandardHelp.SuspendLayout();
            this.tpTables.SuspendLayout();
            this.gpTableHelp.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.gpSettingHelp.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // butCreate
            // 
            this.butCreate.Enabled = false;
            this.butCreate.Location = new System.Drawing.Point(12, 449);
            this.butCreate.Name = "butCreate";
            this.butCreate.Size = new System.Drawing.Size(323, 29);
            this.butCreate.TabIndex = 0;
            this.butCreate.Text = "Neue Excel-Datei erstellen";
            this.butCreate.UseVisualStyleBackColor = true;
            this.butCreate.Click += new System.EventHandler(this.butCreate_Click);
            // 
            // butLoad
            // 
            this.butLoad.Enabled = false;
            this.butLoad.Location = new System.Drawing.Point(175, 414);
            this.butLoad.Name = "butLoad";
            this.butLoad.Size = new System.Drawing.Size(160, 29);
            this.butLoad.TabIndex = 1;
            this.butLoad.Text = "Dateien hinzufügen";
            this.butLoad.UseVisualStyleBackColor = true;
            this.butLoad.Click += new System.EventHandler(this.butLoad_Click);
            // 
            // txbStatus
            // 
            this.txbStatus.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.txbStatus.Location = new System.Drawing.Point(12, 388);
            this.txbStatus.Name = "txbStatus";
            this.txbStatus.ReadOnly = true;
            this.txbStatus.Size = new System.Drawing.Size(323, 20);
            this.txbStatus.TabIndex = 50;
            // 
            // butNew
            // 
            this.butNew.Location = new System.Drawing.Point(12, 414);
            this.butNew.Name = "butNew";
            this.butNew.Size = new System.Drawing.Size(160, 29);
            this.butNew.TabIndex = 6;
            this.butNew.Text = "Dateien auswählen (Neu)";
            this.butNew.UseVisualStyleBackColor = true;
            this.butNew.Click += new System.EventHandler(this.butNew_Click);
            // 
            // lvSettings
            // 
            this.lvSettings.FullRowSelect = true;
            this.lvSettings.Location = new System.Drawing.Point(8, 144);
            this.lvSettings.Name = "lvSettings";
            this.lvSettings.Size = new System.Drawing.Size(300, 150);
            this.lvSettings.TabIndex = 9;
            this.lvSettings.UseCompatibleStateImageBehavior = false;
            this.lvSettings.View = System.Windows.Forms.View.Details;
            this.lvSettings.SelectedIndexChanged += new System.EventHandler(this.lvSettings_SelectedIndexChanged);
            this.lvSettings.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvSettings_KeyDown);
            this.lvSettings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvSettings_MouseDoubleClick);
            // 
            // lvTables
            // 
            this.lvTables.FullRowSelect = true;
            this.lvTables.Location = new System.Drawing.Point(8, 144);
            this.lvTables.Name = "lvTables";
            this.lvTables.Size = new System.Drawing.Size(300, 150);
            this.lvTables.TabIndex = 10;
            this.lvTables.UseCompatibleStateImageBehavior = false;
            this.lvTables.View = System.Windows.Forms.View.Details;
            this.lvTables.SelectedIndexChanged += new System.EventHandler(this.lvTables_SelectedIndexChanged);
            this.lvTables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvTables_KeyDown);
            this.lvTables.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvTables_MouseDoubleClick);
            // 
            // lblTables
            // 
            this.lblTables.AutoSize = true;
            this.lblTables.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTables.Location = new System.Drawing.Point(6, 128);
            this.lblTables.Name = "lblTables";
            this.lblTables.Size = new System.Drawing.Size(56, 13);
            this.lblTables.TabIndex = 11;
            this.lblTables.Text = "Tabellen";
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tpStandard);
            this.tbMain.Controls.Add(this.tpTables);
            this.tbMain.Controls.Add(this.tpSettings);
            this.tbMain.Location = new System.Drawing.Point(12, 27);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(323, 355);
            this.tbMain.TabIndex = 12;
            // 
            // tpStandard
            // 
            this.tpStandard.Controls.Add(this.picElektroplan);
            this.tpStandard.Controls.Add(this.lblTitel);
            this.tpStandard.Controls.Add(this.picSmartme);
            this.tpStandard.Controls.Add(this.gbStandardHelp);
            this.tpStandard.Location = new System.Drawing.Point(4, 22);
            this.tpStandard.Name = "tpStandard";
            this.tpStandard.Padding = new System.Windows.Forms.Padding(3);
            this.tpStandard.Size = new System.Drawing.Size(315, 329);
            this.tpStandard.TabIndex = 0;
            this.tpStandard.Text = "Standard";
            this.tpStandard.UseVisualStyleBackColor = true;
            // 
            // picElektroplan
            // 
            this.picElektroplan.Image = global::SummaryCreator.Properties.Resources.Logo;
            this.picElektroplan.Location = new System.Drawing.Point(280, 6);
            this.picElektroplan.Name = "picElektroplan";
            this.picElektroplan.Size = new System.Drawing.Size(32, 87);
            this.picElektroplan.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picElektroplan.TabIndex = 21;
            this.picElektroplan.TabStop = false;
            // 
            // lblTitel
            // 
            this.lblTitel.AutoSize = true;
            this.lblTitel.Font = new System.Drawing.Font("Arial Narrow", 25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitel.ForeColor = System.Drawing.Color.Black;
            this.lblTitel.Location = new System.Drawing.Point(16, 43);
            this.lblTitel.Name = "lblTitel";
            this.lblTitel.Size = new System.Drawing.Size(246, 40);
            this.lblTitel.TabIndex = 20;
            this.lblTitel.Text = "SummaryCreator";
            // 
            // picSmartme
            // 
            this.picSmartme.Image = global::SummaryCreator.Properties.Resources.logoBlueSmall;
            this.picSmartme.Location = new System.Drawing.Point(135, 86);
            this.picSmartme.Name = "picSmartme";
            this.picSmartme.Size = new System.Drawing.Size(148, 38);
            this.picSmartme.TabIndex = 19;
            this.picSmartme.TabStop = false;
            // 
            // gbStandardHelp
            // 
            this.gbStandardHelp.Controls.Add(this.lblStandardHelp);
            this.gbStandardHelp.Location = new System.Drawing.Point(6, 223);
            this.gbStandardHelp.Name = "gbStandardHelp";
            this.gbStandardHelp.Size = new System.Drawing.Size(302, 100);
            this.gbStandardHelp.TabIndex = 18;
            this.gbStandardHelp.TabStop = false;
            this.gbStandardHelp.Text = "Info";
            // 
            // lblStandardHelp
            // 
            this.lblStandardHelp.Location = new System.Drawing.Point(7, 20);
            this.lblStandardHelp.Name = "lblStandardHelp";
            this.lblStandardHelp.Size = new System.Drawing.Size(289, 77);
            this.lblStandardHelp.TabIndex = 0;
            this.lblStandardHelp.Text = resources.GetString("lblStandardHelp.Text");
            // 
            // tpTables
            // 
            this.tpTables.Controls.Add(this.butToDown);
            this.tpTables.Controls.Add(this.butToTop);
            this.tpTables.Controls.Add(this.gpTableHelp);
            this.tpTables.Controls.Add(this.butTableRemove);
            this.tpTables.Controls.Add(this.butTableGroup);
            this.tpTables.Controls.Add(this.lvTables);
            this.tpTables.Controls.Add(this.lblTables);
            this.tpTables.Location = new System.Drawing.Point(4, 22);
            this.tpTables.Name = "tpTables";
            this.tpTables.Padding = new System.Windows.Forms.Padding(3);
            this.tpTables.Size = new System.Drawing.Size(315, 329);
            this.tpTables.TabIndex = 1;
            this.tpTables.Text = "Tabellen";
            this.tpTables.UseVisualStyleBackColor = true;
            // 
            // butToDown
            // 
            this.butToDown.Enabled = false;
            this.butToDown.Location = new System.Drawing.Point(45, 300);
            this.butToDown.Name = "butToDown";
            this.butToDown.Size = new System.Drawing.Size(31, 23);
            this.butToDown.TabIndex = 19;
            this.butToDown.Text = "\\/";
            this.butToDown.UseVisualStyleBackColor = true;
            this.butToDown.Click += new System.EventHandler(this.butToDown_Click);
            // 
            // butToTop
            // 
            this.butToTop.Enabled = false;
            this.butToTop.Location = new System.Drawing.Point(8, 300);
            this.butToTop.Name = "butToTop";
            this.butToTop.Size = new System.Drawing.Size(31, 23);
            this.butToTop.TabIndex = 18;
            this.butToTop.Text = "/\\";
            this.butToTop.UseVisualStyleBackColor = true;
            this.butToTop.Click += new System.EventHandler(this.butToTop_Click);
            // 
            // gpTableHelp
            // 
            this.gpTableHelp.Controls.Add(this.lblTableHelp);
            this.gpTableHelp.Location = new System.Drawing.Point(6, 3);
            this.gpTableHelp.Name = "gpTableHelp";
            this.gpTableHelp.Size = new System.Drawing.Size(302, 100);
            this.gpTableHelp.TabIndex = 17;
            this.gpTableHelp.TabStop = false;
            this.gpTableHelp.Text = "Info";
            // 
            // lblTableHelp
            // 
            this.lblTableHelp.Location = new System.Drawing.Point(7, 20);
            this.lblTableHelp.Name = "lblTableHelp";
            this.lblTableHelp.Size = new System.Drawing.Size(289, 77);
            this.lblTableHelp.TabIndex = 0;
            this.lblTableHelp.Text = resources.GetString("lblTableHelp.Text");
            // 
            // butTableRemove
            // 
            this.butTableRemove.Enabled = false;
            this.butTableRemove.Location = new System.Drawing.Point(128, 300);
            this.butTableRemove.Name = "butTableRemove";
            this.butTableRemove.Size = new System.Drawing.Size(87, 23);
            this.butTableRemove.TabIndex = 13;
            this.butTableRemove.Text = "Löschen";
            this.butTableRemove.UseVisualStyleBackColor = true;
            this.butTableRemove.Click += new System.EventHandler(this.butTableRemove_Click);
            // 
            // butTableGroup
            // 
            this.butTableGroup.Enabled = false;
            this.butTableGroup.Location = new System.Drawing.Point(221, 300);
            this.butTableGroup.Name = "butTableGroup";
            this.butTableGroup.Size = new System.Drawing.Size(88, 23);
            this.butTableGroup.TabIndex = 12;
            this.butTableGroup.Text = "Gruppieren";
            this.butTableGroup.UseVisualStyleBackColor = true;
            this.butTableGroup.Click += new System.EventHandler(this.butTableGroup_Click);
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.butSettingSave);
            this.tpSettings.Controls.Add(this.butSettingEdit);
            this.tpSettings.Controls.Add(this.gpSettingHelp);
            this.tpSettings.Controls.Add(this.lblSettings);
            this.tpSettings.Controls.Add(this.butSettingRemove);
            this.tpSettings.Controls.Add(this.butSettingAdd);
            this.tpSettings.Controls.Add(this.lvSettings);
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Size = new System.Drawing.Size(315, 329);
            this.tpSettings.TabIndex = 2;
            this.tpSettings.Text = "Ausgabe";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // butSettingSave
            // 
            this.butSettingSave.Location = new System.Drawing.Point(8, 300);
            this.butSettingSave.Name = "butSettingSave";
            this.butSettingSave.Size = new System.Drawing.Size(70, 23);
            this.butSettingSave.TabIndex = 18;
            this.butSettingSave.Text = "Speichern";
            this.butSettingSave.UseVisualStyleBackColor = true;
            this.butSettingSave.Click += new System.EventHandler(this.butSettingSave_Click);
            // 
            // butSettingEdit
            // 
            this.butSettingEdit.Enabled = false;
            this.butSettingEdit.Location = new System.Drawing.Point(85, 300);
            this.butSettingEdit.Name = "butSettingEdit";
            this.butSettingEdit.Size = new System.Drawing.Size(70, 23);
            this.butSettingEdit.TabIndex = 17;
            this.butSettingEdit.Text = "Bearbeiten";
            this.butSettingEdit.UseVisualStyleBackColor = true;
            this.butSettingEdit.Click += new System.EventHandler(this.butSettingEdit_Click);
            // 
            // gpSettingHelp
            // 
            this.gpSettingHelp.Controls.Add(this.lblSettingHelp);
            this.gpSettingHelp.Location = new System.Drawing.Point(6, 3);
            this.gpSettingHelp.Name = "gpSettingHelp";
            this.gpSettingHelp.Size = new System.Drawing.Size(302, 100);
            this.gpSettingHelp.TabIndex = 16;
            this.gpSettingHelp.TabStop = false;
            this.gpSettingHelp.Text = "Info";
            // 
            // lblSettingHelp
            // 
            this.lblSettingHelp.Location = new System.Drawing.Point(7, 20);
            this.lblSettingHelp.Name = "lblSettingHelp";
            this.lblSettingHelp.Size = new System.Drawing.Size(289, 77);
            this.lblSettingHelp.TabIndex = 0;
            this.lblSettingHelp.Text = resources.GetString("lblSettingHelp.Text");
            // 
            // lblSettings
            // 
            this.lblSettings.AutoSize = true;
            this.lblSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettings.Location = new System.Drawing.Point(8, 128);
            this.lblSettings.Name = "lblSettings";
            this.lblSettings.Size = new System.Drawing.Size(83, 13);
            this.lblSettings.TabIndex = 15;
            this.lblSettings.Text = "Einstellungen";
            // 
            // butSettingRemove
            // 
            this.butSettingRemove.Enabled = false;
            this.butSettingRemove.Location = new System.Drawing.Point(161, 300);
            this.butSettingRemove.Name = "butSettingRemove";
            this.butSettingRemove.Size = new System.Drawing.Size(70, 23);
            this.butSettingRemove.TabIndex = 14;
            this.butSettingRemove.Text = "Löschen";
            this.butSettingRemove.UseVisualStyleBackColor = true;
            this.butSettingRemove.Click += new System.EventHandler(this.butSettingRemove_Click);
            // 
            // butSettingAdd
            // 
            this.butSettingAdd.Location = new System.Drawing.Point(238, 300);
            this.butSettingAdd.Name = "butSettingAdd";
            this.butSettingAdd.Size = new System.Drawing.Size(71, 23);
            this.butSettingAdd.TabIndex = 13;
            this.butSettingAdd.Text = "Erstellen";
            this.butSettingAdd.UseVisualStyleBackColor = true;
            this.butSettingAdd.Click += new System.EventHandler(this.butSettingAdd_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sTARTToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(348, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sTARTToolStripMenuItem
            // 
            this.sTARTToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smartmeToolStripMenuItem,
            this.überSummaryCreatorToolStripMenuItem,
            this.beendenToolStripMenuItem});
            this.sTARTToolStripMenuItem.Name = "sTARTToolStripMenuItem";
            this.sTARTToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.sTARTToolStripMenuItem.Text = "Menü";
            // 
            // smartmeToolStripMenuItem
            // 
            this.smartmeToolStripMenuItem.Name = "smartmeToolStripMenuItem";
            this.smartmeToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.smartmeToolStripMenuItem.Text = "smart-me öffnen";
            this.smartmeToolStripMenuItem.Click += new System.EventHandler(this.smartmeToolStripMenuItem_Click);
            // 
            // überSummaryCreatorToolStripMenuItem
            // 
            this.überSummaryCreatorToolStripMenuItem.Name = "überSummaryCreatorToolStripMenuItem";
            this.überSummaryCreatorToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.überSummaryCreatorToolStripMenuItem.Text = "Über SummaryCreator";
            this.überSummaryCreatorToolStripMenuItem.Click += new System.EventHandler(this.überSummaryCreatorToolStripMenuItem_Click);
            // 
            // beendenToolStripMenuItem
            // 
            this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            this.beendenToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.beendenToolStripMenuItem.Text = "Beenden";
            this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
            // 
            // Creator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 487);
            this.Controls.Add(this.tbMain);
            this.Controls.Add(this.butNew);
            this.Controls.Add(this.txbStatus);
            this.Controls.Add(this.butLoad);
            this.Controls.Add(this.butCreate);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Creator";
            this.Text = "SummaryCreator";
            this.tbMain.ResumeLayout(false);
            this.tpStandard.ResumeLayout(false);
            this.tpStandard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picElektroplan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSmartme)).EndInit();
            this.gbStandardHelp.ResumeLayout(false);
            this.tpTables.ResumeLayout(false);
            this.tpTables.PerformLayout();
            this.gpTableHelp.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.tpSettings.PerformLayout();
            this.gpSettingHelp.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butCreate;
        private System.Windows.Forms.Button butLoad;
        private System.Windows.Forms.TextBox txbStatus;
        private System.Windows.Forms.Button butNew;
        private System.Windows.Forms.ListView lvSettings;
        private System.Windows.Forms.ListView lvTables;
        private System.Windows.Forms.Label lblTables;
        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tpStandard;
        private System.Windows.Forms.TabPage tpTables;
        private System.Windows.Forms.Button butTableRemove;
        private System.Windows.Forms.Button butTableGroup;
        private System.Windows.Forms.TabPage tpSettings;
        private System.Windows.Forms.Button butSettingAdd;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sTARTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private System.Windows.Forms.Label lblSettings;
        private System.Windows.Forms.Button butSettingRemove;
        private System.Windows.Forms.GroupBox gpSettingHelp;
        private System.Windows.Forms.Label lblSettingHelp;
        private System.Windows.Forms.GroupBox gpTableHelp;
        private System.Windows.Forms.Label lblTableHelp;
        private System.Windows.Forms.GroupBox gbStandardHelp;
        private System.Windows.Forms.Label lblStandardHelp;
        private System.Windows.Forms.Button butSettingEdit;
        private System.Windows.Forms.ToolStripMenuItem smartmeToolStripMenuItem;
        private System.Windows.Forms.PictureBox picSmartme;
        private System.Windows.Forms.Label lblTitel;
        private System.Windows.Forms.PictureBox picElektroplan;
        private System.Windows.Forms.Button butToDown;
        private System.Windows.Forms.Button butToTop;
        private System.Windows.Forms.Button butSettingSave;
        private System.Windows.Forms.ToolStripMenuItem überSummaryCreatorToolStripMenuItem;
    }
}

