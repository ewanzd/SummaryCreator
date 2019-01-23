using SummaryCreator.Basic;
using SummaryCreator.Export;
using SummaryCreator.Model;
using SummaryCreator.Source;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace SummaryCreator
{
    /// <summary>
    /// Form for create smart.me summary.
    /// </summary>
    public partial class Creator : Form
    {
        #region variable and property

        // ==============================================
        // Variablen und Eigenschaften

        /// <summary>
        /// All data.
        /// </summary>
        private Summary summary = new Summary();

        /// <summary>
        /// Every setting create own table.
        /// </summary>
        private List<ExcelTableSetting> settings = new List<ExcelTableSetting>();

        /// <summary>
        /// Load files complete.
        /// </summary>
        private event EventHandler<ObjectEventArgs<string>> completed;

        /// <summary>
        /// Path to config data.
        /// </summary>
        private string path
        {
            get
            {
                // Pfad abrufen
                string getPath = Properties.Settings.Default.SettingsPath;

                // Wenn der Pfad bisher nicht gesetzt wurde oder es das Verzeichnis nicht gibt, wird der Standardwert genommen
                if(String.IsNullOrWhiteSpace(getPath) || !Directory.Exists(getPath))
                {
                    // Standardpfad erstellen
                    getPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Elektroplan", "SummaryCreator");
                    
                    // Speichert den Standardpfad in die Konfigurationsdatei
                    Properties.Settings.Default.SettingsPath = getPath;
                    Properties.Settings.Default.Save();
                }

                // Gibt den Pfad zurück
                return getPath;
            }
        }

        /// <summary>
        /// Path to directory of last call directory.
        /// </summary>
        private string lastPath
        {
            get
            {
                // Pfad abrufen
                string getLastPath = Properties.Settings.Default.LastPath;

                // Wenn der Pfad bisher nicht gesetzt wurde oder es das Verzeichnis nicht gibt, wird der Standardwert genommen
                if(String.IsNullOrWhiteSpace(getLastPath) || !Directory.Exists(getLastPath))
                {
                    // Standardpfad erstellen
                    getLastPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

                    // Speichert den Standardpfad in die Konfigurationsdatei
                    lastPath = getLastPath;
                }

                // Gibt den Pfad zurück
                return getLastPath;
            }
            set
            {
                // Pfad speichern
                Properties.Settings.Default.LastPath = value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Change status text.
        /// </summary>
        private string status
        {
            set
            {
                // Prüft, ob der Status aus dem Aufrufthread aktualisiert werden kann
                if(this.InvokeRequired)
                    this.Invoke(new Action(() => status = value));
                else
                    // Aktualisiert den Statustext
                    txbStatus.Text = value;
            }
        }

        #endregion

        #region constructor and load start data

        // ==============================================
        // Konstruktor

        /// <summary>
        /// Constructor: Standard settings for this form.
        /// </summary>
        public Creator()
        {
            // Fenster erstellen und Konfigurationen vornehmen
            InitializeComponent();
            status = "Bitte Dateien auswählen";
            completed += Creator_Completed;

            // ListView Tabellen Spalten
            lvTables.Columns.Add("Name", 100, HorizontalAlignment.Left);
            lvTables.Columns.Add("Art", 100, HorizontalAlignment.Left);

            // ListView Einstellungen Spalten
            lvSettings.Columns.Add("Name", 200, HorizontalAlignment.Left);
            
            // Einstellungen laden
            loadSettings(path);
        }

        /// <summary>
        /// Load settings or create default value.
        /// </summary>
        private void loadSettings(string path)
        {
            // Pfad zur Konfigurationsdatei erstellen
            string settingPath = Path.Combine(path, "settings.xml");

            // Prüfen, ob Datei existiert
            if(File.Exists(settingPath))
            {
                // Wenn ja werden die Einstellungen geladen
                settings = XMLPort.Load<List<ExcelTableSetting>>(settingPath);
                foreach(var setting in settings)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = setting.Name;
                    item.Tag = setting;
                    lvSettings.Items.Add(item);
                }
            }
            else
            {
                // Tabelleneinstellung Tage
                ExcelTableSetting settingDays = new ExcelTableSetting("Zusammenfassung Tage", new TimeSpan(24, 0, 0), Unit.None, true);
                ListViewItem itemDays = new ListViewItem();
                itemDays.Text = settingDays.Name;
                itemDays.Tag = settingDays;
                
                // Einstellungen hinzufügen
                settings.Add(settingDays);

                // Einstellungen anzeigen
                lvSettings.Items.Add(itemDays);

                // Standardeinstellungen speichern
                XMLPort.Save(settings, Path.Combine(path, "settings.xml"));
            }
        }

        #endregion

        #region load data

        // ==============================================
        // Daten laden

        /// <summary>
        /// Add new files.
        /// </summary>
        private async void butLoad_Click(object sender, EventArgs e)
        {
            // Zusätzliche Dateien mit Daten hinzufügen
            OpenFileDialog ofd = createFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                butNew.Enabled = false;
                butCreate.Enabled = false;
                status = "Wertet Daten aus....";
                lastPath = Path.GetDirectoryName(ofd.FileName);
                await Task.Run(() => loadDataFrom(ofd.FileNames));
            }
        }

        /// <summary>
        /// Replace all files with new files.
        /// </summary>
        private async void butNew_Click(object sender, EventArgs e)
        {
            // Aktuelle Daten ersetzen durch den Inhalt neuer Dateien
            OpenFileDialog ofd = createFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                summary.Clear();
                butCreate.Enabled = false;
                butLoad.Enabled = false;
                butNew.Enabled = false;
                status = "Wertet Daten aus....";
                lastPath = Path.GetDirectoryName(ofd.FileName);
                await Task.Run(() => loadDataFrom(ofd.FileNames));
            }
        }

        /// <summary>
        /// Create dialog for select files.
        /// </summary>
        private OpenFileDialog createFileDialog()
        {
            // Dialogkonfigurationen für Dateien auswählen
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = lastPath;
            ofd.Filter = "CSV-Dateien (*.csv)|*.csv|Alle Dateien (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Multiselect = true;

            return ofd;
        }

        /// <summary>
        /// Load all data from every file to summary.
        /// </summary>
        /// <param name="fileNames">The paths from all files.</param>
        private void loadDataFrom(string[] fileNames)
        {
            try
            {
                // Alle ausgewählte Dateien werden importiert
                foreach(string loadPath in fileNames)
                    CSVSource.ImportFromCSV(new CSVFile(loadPath), summary);

                // Tabellennamen laden und Einträge ordnen
                string namesPath = Path.Combine(path, "names.xml");
                if(File.Exists(namesPath))
                {
                    // Namen abrufen
                    var names = XMLPort.Load<List<IndexName>>(namesPath);
                    foreach(var table in summary)
                    {
                        var find = names.Where(x => x.ID == table.ID).FirstOrDefault();
                        if(find != null && table.ID == table.HeaderText)
                            table.HeaderText = find.Displayname;
                    }

                    // Einträge ordnen
                    summary.SortContainerSequence(names.Select(item => item.ID).ToList());
                }

                // Tabelle aktualisieren
                tableListReset();

                // Status anzeigen
                completed(this, new ObjectEventArgs<string>("Daten geladen"));
            }
            catch(IOException)
            {
                completed(this, new ObjectEventArgs<string>("error"));
                MessageBox.Show("Bitte schliessen Sie zuerst die Dateien");
            }
            catch(Exception ex)
            {
                completed(this, new ObjectEventArgs<string>("error"));
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        #endregion

        #region save data

        // ==============================================
        // Daten speichern

        /// <summary>
        /// Create excel file.
        /// </summary>
        private async void butCreate_Click(object sender, EventArgs e)
        {
            // Status anzeige
            status = "Excel wird erstellt....";

            // Tabellen erstellen Async
            await Task.Run(() => saveData(summary, settings));
        }

        /// <summary>
        /// All data will be save with the selected settings.
        /// </summary>
        /// <param name="summary">The data object with all tables.</param>
        /// <param name="settings">Every setting create one table.</param>
        private void saveData(Summary summary, List<ExcelTableSetting> settings)
        {
            // Excel-Export Klasse erstellen
            ExcelExport excelExport = new ExcelExport();

            try
            {
                // Nach Datum sortieren
                summary.Sort();

                // Mit jeder Einstellung eine Tabelle aller Daten erstellen
                foreach(ExcelTableSetting setting in settings)
                    excelExport.AddTable(summary, setting);

                // Namen speichern
                string namesPath = Path.Combine(path, "names.xml");
                var indexName = summary.Select(x => new IndexName(x.HeaderText, x.ID)).ToList();
                XMLPort.Save(indexName, namesPath);

                // Status anzeigen
                completed(this, new ObjectEventArgs<string>("Excel erstellt"));
            }
            catch(Exception ex)
            {
                // Meistens tritt ein Fehler auf, wenn vor dem Abschluss des Prozesses auf die Exceltabelle geklickt wird
                completed(this, new ObjectEventArgs<string>("error"));
                MessageBox.Show("Sie dürfen das Excel, bis der Vorgang erfolgreich abgeschlossen wurde, nicht bearbeiten. Falls dies nicht der Fall sein soll, " +
                    "werden fehlerhafte Daten vorhanden sein.\nError: " + ex.Message);
            }
            finally
            {
                // Die COM-Objekte werden freigegeben
                excelExport.Dispose();
            }
        }

        #endregion

        #region help methods for gui

        // ==============================================
        // Hilfsmethoden für den Zugriff auf die GUI

        /// <summary>
        /// If load files successful enabled buttons for create Excel files.
        /// </summary>
        /// <param name="e">Output text in status TextBox.</param>
        private void Creator_Completed(object sender, ObjectEventArgs<string> e)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new EventHandler<ObjectEventArgs<string>>(Creator_Completed), sender, e );
            }
            else
            {
                // Button wieder aktivieren
                butNew.Enabled = true;

                // Prüft, ob ein Fehler aufgetreten ist
                if(e.Obj != "error")
                {
                    // Aktiviert die Knöpfe, mit denen die Excl-Datei erstellt werden kann
                    butCreate.Enabled = true;
                    butLoad.Enabled = true;
                    status = e.Obj;
                }
                else
                {
                    // Es ist ein Fehler aufgetreten
                    status = "Fehlgeschlagen";
                }
            }
        }

        /// <summary>
        /// Reset ListView for tables.
        /// </summary>
        private void tableListReset()
        {
            // Prüft, ob auf die Komponente der Form zugegriffen werden können
            if(this.InvokeRequired)
                this.Invoke(new Action(() => tableListReset()));
            else
            {
                // Tabelleneinträge löschen
                lvTables.Items.Clear();

                // Tabelleneinträge neu erstellen
                foreach(var table in summary)
                {
                    ListViewItem lvi = new ListViewItem(new[] { table.HeaderText, table.TypeName });
                    lvi.Tag = table;
                    lvTables.Items.Add(lvi);
                }
            }
        }

        #endregion

        #region ListView tables

        // ==============================================
        // ListView -> Tabellen

        /// <summary>
        /// Remove a table from summary.
        /// </summary>
        private void butTableRemove_Click(object sender, EventArgs e)
        {
            int indexSelected = 0;

            // Löscht jede ausgewählte Tabelle von Summary und der ListView
            foreach(ListViewItem item in lvTables.SelectedItems)
            {
                indexSelected = lvTables.Items.IndexOf(item);
                summary.Remove((TableContainer)item.Tag);
                lvTables.Items.Remove(item);
            }

            // Prüfen, ob noch Items vorhanden sind
            CheckTablesItemsAvailable();

            // Nächster Eintrag auswählen
            if(lvTables.Items.Count > 0)
            {
                if(indexSelected >= lvTables.Items.Count)
                    indexSelected -= 1;
                lvTables.Items[indexSelected].Selected = true;
                lvTables.Select();
            }

            // Status aktualisieren
            status = "Auswahl gelöscht";
        }

        private void CheckTablesItemsAvailable()
        {
            if(summary.Count == 0)
            {
                // Falls alle Tabellen gelöscht wurden werden die Buttons, um ein Excel zu erstellen, deaktiviert
                butTableRemove.Enabled = false;
                butTableGroup.Enabled = false;
                butToDown.Enabled = false;
                butToTop.Enabled = false;
            }
            else
            {
                // Alle Einträge aktivieren und SelectedIndex auswählen
                butTableRemove.Enabled = true;
                butTableGroup.Enabled = true;
                butToDown.Enabled = true;
                butToTop.Enabled = true;
            }
        }

        /// <summary>
        /// Erstellt eine Gruppe aus den ausgewählten Tabellen
        /// </summary>
        private void butTableGroup_Click(object sender, EventArgs e)
        {
            // Erstellt die Gruppe
            Group group = new Group();

            // Fügt die ausgewählten Tabellen und Gruppen der Gruppe hinzu
            foreach(ListViewItem lvi in lvTables.SelectedItems)
            {
                TableContainer table = summary.Find(x => x.ID == lvi.Text);
                table.Sort();
                group.Add(table);
            }

            // Gruppenname angeben und Gruppe dem Datenobjekt hinzufügen
            string value = String.Empty;
            if(InputBox("Name geben", "Gib der Gruppe einen Namen:", ref value) == DialogResult.OK)
            {
                // Name muss einmalig und darf nicht leer sein
                if(String.IsNullOrWhiteSpace(value) || summary.Find(x => x.ID == value) != null)
                {
                    status = "Gültiger Name eingeben";
                    return;
                }
                group.ID = value;
                summary.Add(group);

                // Die ausgewählten Tabellen löschen
                butTableRemove_Click(sender, e);

                // Tabelle aktualisieren
                tableListReset();

                // Status ausgeben
                status = "Gruppe erstellt";
            }
        }

        /// <summary>
        /// Analyse the key.
        /// </summary>
        private void lvTables_KeyDown(object sender, KeyEventArgs e)
        {
            // Tabelle löschen
            if(e.KeyCode == Keys.Delete)
                butTableRemove_Click(sender, e);
        }

        /// <summary>
        /// Buttons enable and disable.
        /// </summary>
        private void lvTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckTablesItemsAvailable();
        }

        /// <summary>
        /// Move every selected item one step to top.
        /// </summary>
        private void butToTop_Click(object sender, EventArgs e)
        {
            // Prüft, ob der erste Eintrag der ListView ausgewählt wurde
            var query = (from list in lvTables.SelectedItems.Cast<ListViewItem>()
                        where lvTables.Items[0] == list
                        select list).Any();

            if(!query)
            {
                // Verschiebt jedes ausgewählte Item um eine Position nach oben
                foreach(ListViewItem item in lvTables.SelectedItems)
                {
                    if(summary.MoveTop((TableContainer)item.Tag))
                    {
                        int idx = lvTables.Items.IndexOf(item);
                        lvTables.Items.Remove(item);
                        lvTables.Items.Insert(idx - 1, item);
                        item.Selected = true;
                    }
                }
            }
            // Die vorher ausgewählte Items wieder selektieren
            lvTables.Select();
        }

        /// <summary>
        /// Move every selected item one step to down.
        /// </summary>
        private void butToDown_Click(object sender, EventArgs e)
        {
            // Prüft, ob der letzte Eintrag der ListView ausgewählt wurde
            var query = (from list in lvTables.SelectedItems.Cast<ListViewItem>()
                        where lvTables.Items[lvTables.Items.Count - 1] == list
                        select list).Any();

            if(!query)
            {
                // Verschiebt jedes ausgewählte Item um eine Position nach unten
                foreach(ListViewItem item in lvTables.SelectedItems)
                {
                    if(summary.MoveDown((TableContainer)item.Tag))
                    {
                        int idx = lvTables.Items.IndexOf(item);
                        lvTables.Items.Remove(item);
                        lvTables.Items.Insert(idx + 1, item);
                        item.Selected = true;
                    }
                }
            }
            // Die vorher ausgewählte Items wieder selektieren
            lvTables.Select();
        }

        /// <summary>
        /// Give a item a new display name.
        /// </summary>
        private void lvTables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Geklicktes Item abrufen
            ListViewHitTestInfo info = lvTables.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            // Das Item muss mit einem TableContainer verbunden sein
            if(item != null && item.Tag != null)
            {
                // TableContainer abrufen
                TableContainer container = (TableContainer)item.Tag;

                // Aktueller Name abrufen
                string text = container.HeaderText;

                // Eingabefenster öffnen
                InputBox("Spaltenname", "Gib dem Item einen neuen Namen(" + container.ID + "):", ref text);

                // Leerer Namen nicht gültig
                if(String.IsNullOrWhiteSpace(text))
                {
                    status = "Leerer Namen eingegeben";
                    return;
                }

                // Abfragen, ob bereits ein Item mit diesem Namen vorhanden ist
                TableContainer find = summary.Find(x => x.HeaderText == text);

                // Der Name muss einmalig sein
                if(find == null || find == container)
                {
                    // Text aktualisieren
                    container.HeaderText = text;
                    item.Text = text;
                    status = "Ausgabename wurde angepasst";
                }
                else
                    status = "Name bereits vorhanden";
            }
        }

        #endregion

        #region ListView settings

        // ==============================================
        // ListView -> Einstellungen

        /// <summary>
        /// Show form for create setting.
        /// </summary>
        private void butSettingAdd_Click(object sender, EventArgs e)
        {
            // Neue Einstellung erstellen
            TableExtension tableExtension = new TableExtension();
            tableExtension.Completed += tableExtension_Complete;
            tableExtension.CheckName += tableExtension_CheckName;
            tableExtension.Show();
        }

        /// <summary>
        /// Edit a existing setting.
        /// </summary>
        private void butSettingEdit_Click(object sender, EventArgs e)
        {
            // Öffnet alle ausgewählte Einstellungen in je einem eigenen Fenster
            foreach(ListViewItem item in lvSettings.SelectedItems)
            {
                ExcelTableSetting setting = settings.Find(x => x.Name == item.Text);
                if(setting != null)
                {
                    TableExtension tableExtension = new TableExtension(setting);
                    tableExtension.Completed += tableExtension_Complete;
                    tableExtension.CheckName += tableExtension_CheckName;
                    tableExtension.Show();
                }
            }
        }

        /// <summary>
        /// Check the name.
        /// </summary>
        /// <param name="name">The new name of the setting.</param>
        /// <param name="setting">Current open setting.</param>
        /// <returns>true if existing, false if not.</returns>
        private bool tableExtension_CheckName(string name, ExcelTableSetting setting)
        {
            // Prüft alle Einstellungen, ob der Namen bereits vorhanden ist ausser die aktuell geöffnete EInstellung
            foreach(var item in settings)
                if(item.Name == name)
                    if(setting != null && !settings.Contains(setting) || setting.Name != name)
                        return true;

            // Name wurde nicht gefunden
            return false;
        }

        /// <summary>
        /// Add new setting.
        /// </summary>
        /// <param name="e">new setting.</param>
        private void tableExtension_Complete(object sender, ObjectEventArgs<ExcelTableSetting> e)
        {
            // Prüft, ob die Einstellung auf eine bestehende Einstellung eine Referenz besitzt
            if(settings.Contains(e.Obj))
            {
                // Aktualisiert alle EInträge der ListView
                foreach(ListViewItem item in lvSettings.Items)
                {
                    ExcelTableSetting setting = (ExcelTableSetting)item.Tag;
                    item.Text = setting.Name;
                }
            }
            else
            {
                // Fügt die Einstellung den anderen Einstellungen hinzu
                settings.Add(e.Obj);

                // Neuer ListView EIntrag erstellen
                ListViewItem item = new ListViewItem();
                item.Text = e.Obj.Name;
                item.Tag = e.Obj;

                // Fügt die Einstellung der ListView hinzu
                lvSettings.Items.Add(item);
            }

            // Status anpassen
            status = "Einstellung wurde gemerkt";
        }

        /// <summary>
        /// Remove the selected settings.
        /// </summary>
        private void butSettingRemove_Click(object sender, EventArgs e)
        {
            // Löscht alle ausgewählte Einstellungen
            foreach(ListViewItem item in lvSettings.SelectedItems)
            {
                settings.RemoveAll(x => x.Name == item.Text);
                lvSettings.Items.Remove(item);
            }

            // Status anpassen
            status = "Einstellungen gelöscht";
        }

        /// <summary>
        /// Analyse the key.
        /// </summary>
        private void lvSettings_KeyDown(object sender, KeyEventArgs e)
        {
            // Einstellung löschen
            if(e.KeyCode == Keys.Delete)
                butSettingRemove_Click(sender, e);
        }

        /// <summary>
        /// Buttons enable and disable.
        /// </summary>
        private void lvSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lvSettings.SelectedItems.Count == 0)
            {
                butSettingEdit.Enabled = false;
                butSettingRemove.Enabled = false;
            }
            else
            {
                butSettingEdit.Enabled = true;
                butSettingRemove.Enabled = true;
            }
        }

        /// <summary>
        /// Open config from setting.
        /// </summary>
        private void lvSettings_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Geklicktes Item abrufen
            ListViewHitTestInfo info = lvSettings.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            // Das Item muss mit einem ExcelTableSetting verbunden sein
            if(item != null && item.Tag != null)
            {
                ExcelTableSetting setting = (ExcelTableSetting)item.Tag;
                if(setting != null)
                {
                    TableExtension tableExtension = new TableExtension(setting);
                    tableExtension.Completed += tableExtension_Complete;
                    tableExtension.CheckName += tableExtension_CheckName;
                    tableExtension.Show();
                }
            }
        }

        /// <summary>
        /// Save all settings.
        /// </summary>
        private void butSettingSave_Click(object sender, EventArgs e)
        {
            string settingPath = Path.Combine(path, "settings.xml");
            XMLPort.Save(settings, settingPath);
            status = "Einstellungen wurden gespeichert";
        }

        #endregion

        #region input form

        // ==============================================
        // Eingabefenster erstellen

        /// <summary>
        /// Create a form for input name of setting.
        /// </summary>
        /// <param name="title">The title from the form.</param>
        /// <param name="promptText">Help text from the form.</param>
        /// <param name="value">The input text return about this variable.</param>
        /// <returns>Return the result OK or Cancel.</returns>
        private static DialogResult InputBox(string title, string promptText, ref string value)
        {
            // Von http://www.csharp-examples.net/inputbox/ kopiert

            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Abbrechen";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        #endregion

        #region menue

        // ==============================================
        // Menü

        /// <summary>
        /// Exit the application.
        /// </summary>
        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Beendet die Applikation
            Environment.Exit(0);
        }

        /// <summary>
        /// Open smart-me homepage.
        /// </summary>
        private void smartmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Öffnet die smart-me.com Website
            System.Diagnostics.Process.Start(@"http://smart-me.com/");
        }

        /// <summary>
        /// Show about box
        /// </summary>
        private void überSummaryCreatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Öffnet eine Infobox über den SummaryCreator
            AboutBox about = new AboutBox();
            about.Show();
        }

        #endregion
    }
}
