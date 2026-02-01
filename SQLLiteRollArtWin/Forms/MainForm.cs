using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace SQLLiteRollArtWin.Forms
{
    public class MainForm : Form
    {
        private PictureBox pictureBox;
        private Button buttonCsvToSQLite;
        private Button button2;
        private Button button3;
        private Button buttonClose;
        private OpenFileDialog openFileDialog;
        private Button buttonOpenSQLite;
        private TableLayoutPanel tableLayoutPanel;

        public MainForm()
        {
            InitializeComponent();
            Text = "SQLLite RollArt";
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonCsvToSQLite = new System.Windows.Forms.Button();
            this.buttonOpenSQLite = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 301F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel.Controls.Add(this.pictureBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.button2, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.button3, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonClose, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.buttonCsvToSQLite, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonOpenSQLite, 2, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(705, 302);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Image = global::SQLLiteRollArtWin.Properties.Resources.Artistique;
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.tableLayoutPanel.SetRowSpan(this.pictureBox, 4);
            this.pictureBox.Size = new System.Drawing.Size(295, 296);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // button2
            // 
            this.tableLayoutPanel.SetColumnSpan(this.button2, 2);
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Enabled = false;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.button2.Location = new System.Drawing.Point(304, 78);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(398, 69);
            this.button2.TabIndex = 2;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.tableLayoutPanel.SetColumnSpan(this.button3, 2);
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Enabled = false;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.button3.Location = new System.Drawing.Point(304, 153);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(398, 69);
            this.button3.TabIndex = 3;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableLayoutPanel.SetColumnSpan(this.buttonClose, 2);
            this.buttonClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonClose.Image = global::SQLLiteRollArtWin.Properties.Resources.fermer;
            this.buttonClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonClose.Location = new System.Drawing.Point(304, 228);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(398, 71);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "&Fermer";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonCsvToSQLite
            // 
            this.buttonCsvToSQLite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(26)))), ((int)(((byte)(111)))));
            this.buttonCsvToSQLite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCsvToSQLite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCsvToSQLite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCsvToSQLite.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCsvToSQLite.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonCsvToSQLite.Image = global::SQLLiteRollArtWin.Properties.Resources.Roller64;
            this.buttonCsvToSQLite.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCsvToSQLite.Location = new System.Drawing.Point(304, 3);
            this.buttonCsvToSQLite.Name = "buttonCsvToSQLite";
            this.buttonCsvToSQLite.Size = new System.Drawing.Size(316, 69);
            this.buttonCsvToSQLite.TabIndex = 1;
            this.buttonCsvToSQLite.Text = "Création compétition";
            this.buttonCsvToSQLite.UseVisualStyleBackColor = false;
            this.buttonCsvToSQLite.Click += new System.EventHandler(this.buttonCsvToSQLite_Click);
            // 
            // buttonOpenSQLite
            // 
            this.buttonOpenSQLite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(26)))), ((int)(((byte)(111)))));
            this.buttonOpenSQLite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonOpenSQLite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOpenSQLite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOpenSQLite.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOpenSQLite.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonOpenSQLite.Image = global::SQLLiteRollArtWin.Properties.Resources.Folder64x64;
            this.buttonOpenSQLite.Location = new System.Drawing.Point(626, 3);
            this.buttonOpenSQLite.Name = "buttonOpenSQLite";
            this.buttonOpenSQLite.Size = new System.Drawing.Size(76, 69);
            this.buttonOpenSQLite.TabIndex = 4;
            this.buttonOpenSQLite.UseVisualStyleBackColor = false;
            this.buttonOpenSQLite.Click += new System.EventHandler(this.buttonOpenSQLite_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Fichiers CSV (*.csv)|*.csv";
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(26)))), ((int)(((byte)(111)))));
            this.ClientSize = new System.Drawing.Size(705, 302);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Outils SQLLite RollArt";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }/// <summary>
         /// Bouton : conversion d’un fichier CSV en base SQLite prête à l’emploi.
         /// - Sélection d’un CSV
         /// - Conversion CSV → JSON
         /// - Injection des données dans une base SQLite basée sur un template
         /// - Exécution d’un script SQL d’initialisation
         /// - Renommage final de la base selon la manifestation
         /// - Ouverture de l’explorateur sur le fichier généré
         /// </summary>
        private async void buttonCsvToSQLite_Click(object sender, EventArgs e)
        {
            // ------------------------------------------------------------
            // 1. Sélection du fichier CSV
            // ------------------------------------------------------------

            // Ouverture de la boîte de dialogue de sélection de fichier
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            // Sécurité : aucun fichier sélectionné
            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return;

            var formJudge = new FormJudgeQty();
            formJudge.ShowDialog(this);
            if(formJudge.DialogResult != DialogResult.OK)
                return;

            var judgeQty = formJudge.Qty;

            try
            {
                // Curseur "chargement"
                Cursor.Current = Cursors.WaitCursor;
                this.UseWaitCursor = true;
                this.Enabled = false;

                // ------------------------------------------------------------
                // 2. Lecture complète du fichier CSV (UTF-8, BOM détecté)
                // ------------------------------------------------------------

                using (var reader = new StreamReader(
                           openFileDialog.FileName,
                           Encoding.UTF8,
                           detectEncodingFromByteOrderMarks: true))
                {
                    var content = await reader.ReadToEndAsync();

                    // Vérification : fichier vide
                    if (string.IsNullOrEmpty(content))
                    {
                        MessageBox.Show(@"Fichier séléctionné vide.");
                        return;
                    }

                    // ------------------------------------------------------------
                    // 3. Conversion CSV → JSON
                    // ------------------------------------------------------------

                    var jsonContent = Helpers.NewtonSoft.CsvToJsonHelper(content);

                    // ------------------------------------------------------------
                    // 4. Création de la base SQLite de travail à partir du template
                    // ------------------------------------------------------------
                    var templateDbPath = Path.Combine(AppPaths.TemplatesDir, "20252026.s3db");

                    var workingDbPath = Path.Combine(
                        AppPaths.SqliteDir,
                        $"20252026_{Guid.NewGuid():N}.s3db"
                    );


                    // Copie du template vers la base de travail
                    File.Copy(templateDbPath, workingDbPath, overwrite: false);

                    // ------------------------------------------------------------
                    // 5. Insertion des données JSON dans la table SQLite
                    // ------------------------------------------------------------

                    var array = JArray.Parse(jsonContent);

                    // Création de la table rolskanet
                    await Task.Run(() => Helpers.SqliteHelper.ExecuteRaw(
                        workingDbPath,
                        @"
CREATE TABLE Rolskanet (
    [Id] INTEGER PRIMARY KEY AUTOINCREMENT,
    GaraParams TEXT,
    ""#"" TEXT,
    ""Manifestation"" TEXT,
    ""Type"" TEXT,
    ""Filière"" TEXT,
    ""Groupe d'épreuve"" TEXT,
    ""Épreuve"" TEXT,
    ""Groupe"" TEXT,
    ""Numéro de licence"" TEXT,
    ""Nom"" TEXT,
    ""Numéro de dossard"" TEXT,
    ""Numéro de transpondeur"" TEXT,
    ""Prenom"" TEXT,
    ""Civilité"" TEXT,
    ""Nationalité"" TEXT,
    ""Date de naissance"" TEXT,
    ""Sexe"" TEXT,
    ""Adresse"" TEXT,
    ""Code postal"" TEXT,
    ""Ville"" TEXT,
    ""Adresse mail"" TEXT,
    ""Téléphone"" TEXT,
    ""Portable"" TEXT,
    ""N°Ligue - Nom Ligue"" TEXT,
    ""N° Département - Nom Département"" TEXT,
    ""N° Club"" TEXT,
    ""Nom Club"" TEXT,
    ""Licence"" TEXT,
    ""Discipline"" TEXT,
    ""Sportif catégorie âge"" TEXT,
    ""Date de début"" TEXT,
    ""Date de fin"" TEXT,
    ""N° - Nom Structure organisatrice"" TEXT,
    ""Code postal manifestation"" TEXT,
    ""Commune manifestation"" TEXT,
    ""Date d'inscription"" TEXT,
    ""État"" TEXT,
    ""Commentaire"" TEXT
);
"
                    ));


                    // Insertion JSON → table SQLite
                    await Task.Run(() => Helpers.SqliteHelper.InsertJsonIntoTable(
                        workingDbPath,
                        "Rolskanet",
                        array
                    ));

                    // ------------------------------------------------------------
                    // 6. Exécution du script SQL d’initialisation
                    // ------------------------------------------------------------
                    var sqlFilePath = Path.Combine(
                        AppPaths.TemplatesDir,
                        "20252026Init.sql"
                    );


                    var sqlScript = await Task.Run(() => File.ReadAllText(
                        sqlFilePath,
                        new UTF8Encoding()
                    ));

                    sqlScript = sqlScript.Replace("$JUDGEQTY$", judgeQty.ToString());

                    await Task.Run(() => Helpers.SqliteHelper.ExecuteSqlScript(
                        workingDbPath,
                        sqlScript
                    ));

                    // ------------------------------------------------------------
                    // 7. Récupération du nom de la manifestation
                    // ------------------------------------------------------------

                    // Le nom est extrait du premier élément du JSON
                    var manifestation = array[0]?["Manifestation"]?.ToString();

                    // Nettoyage du nom pour qu’il soit valide en tant que nom de fichier
                    var manifestationDbPath = Path.Combine(
                        AppPaths.SqliteDir,
                        $"{MakeSafeFileName(manifestation)}.s3db"
                    );

                    // ------------------------------------------------------------
                    // 8. Renommage final de la base SQLite
                    // ------------------------------------------------------------

                    // Renommage de la base de travail vers son nom final
                    if (File.Exists(manifestationDbPath))
                        File.Delete(manifestationDbPath);

                    File.Move(workingDbPath, manifestationDbPath);

                    // ------------------------------------------------------------
                    // 9. Ouverture de l’explorateur Windows sur le fichier généré
                    // ------------------------------------------------------------

                    System.Diagnostics.Process.Start(
                        "explorer.exe",
                        "/select,\"" + manifestationDbPath + "\""
                    );
                }
            }
            catch (Exception ex)
            {
                // ------------------------------------------------------------
                // Gestion centralisée des erreurs
                // ------------------------------------------------------------

                MessageBox.Show(
                    ex.Message,
                    "Une erreur est survenue!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                // Curseur normal (toujours remis, même en cas d'erreur)
                this.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
                this.Enabled = true;
            }
        }

        private static string MakeSafeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');

            return name.Trim();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var templateDb = Path.Combine(AppPaths.TemplatesDir, "20252026.s3db");
            var templateSql = Path.Combine(AppPaths.TemplatesDir, "20252026Init.sql");

            if (!File.Exists(templateDb) || !File.Exists(templateSql))
            {
                MessageBox.Show("Templates manquants.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            Directory.CreateDirectory(AppPaths.SqliteDir);

            var userDb = Path.Combine(AppPaths.SqliteDir, "20252026.s3db");

            if (!File.Exists(userDb))
                File.Copy(templateDb, userDb);
        }

        private void buttonOpenSQLite_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe",AppPaths.SqliteDir);
        }
    }
    public static class AppPaths
    {
        public static string AppDir => AppDomain.CurrentDomain.BaseDirectory;

        public static string TemplatesDir => Path.Combine(AppDir, "Templates");

        public static string AppDataRoot =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SQLLiteRollArtWin"
            );

        public static string SqliteDir => Path.Combine(AppDataRoot, "Sqlite");
    }
}
