namespace Elara.NavigationStudio
{
    partial class UserControlStudio
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.SaveNavigationButton = new MetroFramework.Controls.MetroButton();
            this.AutoMapCheckbox = new MetroFramework.Controls.MetroCheckBox();
            this.AutoSaveCheckbox = new MetroFramework.Controls.MetroCheckBox();
            this.DrawConnectionsCheckbox = new MetroFramework.Controls.MetroCheckBox();
            this.DrawTilesBoxCheckbox = new MetroFramework.Controls.MetroCheckBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.DrawNodesCheckbox = new MetroFramework.Controls.MetroCheckBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.SaveTimer = new System.Windows.Forms.Timer(this.components);
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.TileY = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.TileX = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.PositionZ = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.PositionY = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.PositionX = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel3 = new MetroFramework.Controls.MetroPanel();
            this.NavigateButton = new MetroFramework.Controls.MetroButton();
            this.SelectDestButton = new MetroFramework.Controls.MetroButton();
            this.StopEditingButton = new MetroFramework.Controls.MetroButton();
            this.StartEditingButton = new MetroFramework.Controls.MetroButton();
            this.metroLabel10 = new MetroFramework.Controls.MetroLabel();
            this.NewSourceButton = new MetroFramework.Controls.MetroButton();
            this.metroLabel9 = new MetroFramework.Controls.MetroLabel();
            this.RefreshSourcesButton = new MetroFramework.Controls.MetroButton();
            this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.metroPanel4 = new MetroFramework.Controls.MetroPanel();
            this.SourceGridView = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.metroPanel1.SuspendLayout();
            this.metroPanel2.SuspendLayout();
            this.metroPanel3.SuspendLayout();
            this.metroPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SourceGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // metroPanel1
            // 
            this.metroPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.metroPanel1.Controls.Add(this.SaveNavigationButton);
            this.metroPanel1.Controls.Add(this.AutoMapCheckbox);
            this.metroPanel1.Controls.Add(this.AutoSaveCheckbox);
            this.metroPanel1.Controls.Add(this.DrawConnectionsCheckbox);
            this.metroPanel1.Controls.Add(this.DrawTilesBoxCheckbox);
            this.metroPanel1.Controls.Add(this.metroLabel1);
            this.metroPanel1.Controls.Add(this.DrawNodesCheckbox);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(3, 42);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(159, 287);
            this.metroPanel1.TabIndex = 0;
            this.metroPanel1.VerticalScrollbar = true;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // SaveNavigationButton
            // 
            this.SaveNavigationButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveNavigationButton.Location = new System.Drawing.Point(3, 244);
            this.SaveNavigationButton.Name = "SaveNavigationButton";
            this.SaveNavigationButton.Size = new System.Drawing.Size(153, 40);
            this.SaveNavigationButton.TabIndex = 8;
            this.SaveNavigationButton.Text = "Save navigation";
            this.SaveNavigationButton.UseSelectable = true;
            this.SaveNavigationButton.Click += new System.EventHandler(this.SaveNavigationButton_Click);
            // 
            // AutoMapCheckbox
            // 
            this.AutoMapCheckbox.AutoSize = true;
            this.AutoMapCheckbox.ForeColor = System.Drawing.SystemColors.Highlight;
            this.AutoMapCheckbox.Location = new System.Drawing.Point(8, 123);
            this.AutoMapCheckbox.Name = "AutoMapCheckbox";
            this.AutoMapCheckbox.Size = new System.Drawing.Size(76, 15);
            this.AutoMapCheckbox.TabIndex = 7;
            this.AutoMapCheckbox.Text = "Auto Map";
            this.AutoMapCheckbox.UseCustomForeColor = true;
            this.AutoMapCheckbox.UseSelectable = true;
            this.AutoMapCheckbox.CheckedChanged += new System.EventHandler(this.AutoMapCheckbox_CheckedChanged);
            // 
            // AutoSaveCheckbox
            // 
            this.AutoSaveCheckbox.AutoSize = true;
            this.AutoSaveCheckbox.Location = new System.Drawing.Point(8, 102);
            this.AutoSaveCheckbox.Name = "AutoSaveCheckbox";
            this.AutoSaveCheckbox.Size = new System.Drawing.Size(103, 15);
            this.AutoSaveCheckbox.TabIndex = 6;
            this.AutoSaveCheckbox.Text = "Auto save (10s)";
            this.AutoSaveCheckbox.UseSelectable = true;
            this.AutoSaveCheckbox.CheckedChanged += new System.EventHandler(this.AutoSaveCheckbox_CheckedChanged);
            // 
            // DrawConnectionsCheckbox
            // 
            this.DrawConnectionsCheckbox.AutoSize = true;
            this.DrawConnectionsCheckbox.Location = new System.Drawing.Point(8, 81);
            this.DrawConnectionsCheckbox.Name = "DrawConnectionsCheckbox";
            this.DrawConnectionsCheckbox.Size = new System.Drawing.Size(118, 15);
            this.DrawConnectionsCheckbox.TabIndex = 5;
            this.DrawConnectionsCheckbox.Text = "Draw connections";
            this.DrawConnectionsCheckbox.UseSelectable = true;
            this.DrawConnectionsCheckbox.CheckedChanged += new System.EventHandler(this.DrawConnectionsCheckbox_CheckedChanged);
            // 
            // DrawTilesBoxCheckbox
            // 
            this.DrawTilesBoxCheckbox.AutoSize = true;
            this.DrawTilesBoxCheckbox.Location = new System.Drawing.Point(8, 60);
            this.DrawTilesBoxCheckbox.Name = "DrawTilesBoxCheckbox";
            this.DrawTilesBoxCheckbox.Size = new System.Drawing.Size(96, 15);
            this.DrawTilesBoxCheckbox.TabIndex = 4;
            this.DrawTilesBoxCheckbox.Text = "Draw tiles box";
            this.DrawTilesBoxCheckbox.UseSelectable = true;
            this.DrawTilesBoxCheckbox.CheckedChanged += new System.EventHandler(this.DrawTilesBoxCheckbox_CheckedChanged);
            // 
            // metroLabel1
            // 
            this.metroLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroLabel1.BackColor = System.Drawing.Color.Transparent;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel1.Location = new System.Drawing.Point(3, 6);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(153, 19);
            this.metroLabel1.TabIndex = 3;
            this.metroLabel1.Text = "Settings";
            this.metroLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DrawNodesCheckbox
            // 
            this.DrawNodesCheckbox.AutoSize = true;
            this.DrawNodesCheckbox.Location = new System.Drawing.Point(8, 39);
            this.DrawNodesCheckbox.Name = "DrawNodesCheckbox";
            this.DrawNodesCheckbox.Size = new System.Drawing.Size(85, 15);
            this.DrawNodesCheckbox.TabIndex = 2;
            this.DrawNodesCheckbox.Text = "Draw nodes";
            this.DrawNodesCheckbox.UseSelectable = true;
            this.DrawNodesCheckbox.CheckedChanged += new System.EventHandler(this.DrawNodesCheckbox_CheckedChanged);
            // 
            // metroLabel2
            // 
            this.metroLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroLabel2.BackColor = System.Drawing.Color.Transparent;
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel2.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel2.Location = new System.Drawing.Point(3, 9);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(695, 28);
            this.metroLabel2.TabIndex = 1;
            this.metroLabel2.Text = "Navigation Studio";
            this.metroLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroLabel2.UseCustomBackColor = true;
            // 
            // SaveTimer
            // 
            this.SaveTimer.Interval = 10000;
            this.SaveTimer.Tick += new System.EventHandler(this.SaveTimer_Tick);
            // 
            // metroPanel2
            // 
            this.metroPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanel2.Controls.Add(this.TileY);
            this.metroPanel2.Controls.Add(this.metroLabel7);
            this.metroPanel2.Controls.Add(this.TileX);
            this.metroPanel2.Controls.Add(this.metroLabel8);
            this.metroPanel2.Controls.Add(this.PositionZ);
            this.metroPanel2.Controls.Add(this.metroLabel6);
            this.metroPanel2.Controls.Add(this.PositionY);
            this.metroPanel2.Controls.Add(this.metroLabel5);
            this.metroPanel2.Controls.Add(this.PositionX);
            this.metroPanel2.Controls.Add(this.metroLabel4);
            this.metroPanel2.Controls.Add(this.metroLabel3);
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(168, 251);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(371, 78);
            this.metroPanel2.TabIndex = 2;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // TileY
            // 
            // 
            // 
            // 
            this.TileY.CustomButton.Image = null;
            this.TileY.CustomButton.Location = new System.Drawing.Point(53, 1);
            this.TileY.CustomButton.Name = "";
            this.TileY.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.TileY.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.TileY.CustomButton.TabIndex = 1;
            this.TileY.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.TileY.CustomButton.UseSelectable = true;
            this.TileY.CustomButton.Visible = false;
            this.TileY.Lines = new string[] {
        "0,00000"};
            this.TileY.Location = new System.Drawing.Point(180, 50);
            this.TileY.MaxLength = 32767;
            this.TileY.Name = "TileY";
            this.TileY.PasswordChar = '\0';
            this.TileY.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TileY.SelectedText = "";
            this.TileY.SelectionLength = 0;
            this.TileY.SelectionStart = 0;
            this.TileY.ShortcutsEnabled = true;
            this.TileY.Size = new System.Drawing.Size(75, 23);
            this.TileY.TabIndex = 14;
            this.TileY.Text = "0,00000";
            this.TileY.UseSelectable = true;
            this.TileY.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.TileY.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel7
            // 
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.Location = new System.Drawing.Point(157, 51);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(17, 19);
            this.metroLabel7.TabIndex = 13;
            this.metroLabel7.Text = "Y";
            // 
            // TileX
            // 
            // 
            // 
            // 
            this.TileX.CustomButton.Image = null;
            this.TileX.CustomButton.Location = new System.Drawing.Point(53, 1);
            this.TileX.CustomButton.Name = "";
            this.TileX.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.TileX.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.TileX.CustomButton.TabIndex = 1;
            this.TileX.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.TileX.CustomButton.UseSelectable = true;
            this.TileX.CustomButton.Visible = false;
            this.TileX.Lines = new string[] {
        "0,00000"};
            this.TileX.Location = new System.Drawing.Point(76, 50);
            this.TileX.MaxLength = 32767;
            this.TileX.Name = "TileX";
            this.TileX.PasswordChar = '\0';
            this.TileX.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TileX.SelectedText = "";
            this.TileX.SelectionLength = 0;
            this.TileX.SelectionStart = 0;
            this.TileX.ShortcutsEnabled = true;
            this.TileX.Size = new System.Drawing.Size(75, 23);
            this.TileX.TabIndex = 12;
            this.TileX.Text = "0,00000";
            this.TileX.UseSelectable = true;
            this.TileX.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.TileX.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel8
            // 
            this.metroLabel8.AutoSize = true;
            this.metroLabel8.Location = new System.Drawing.Point(4, 51);
            this.metroLabel8.Name = "metroLabel8";
            this.metroLabel8.Size = new System.Drawing.Size(65, 19);
            this.metroLabel8.TabIndex = 11;
            this.metroLabel8.Text = "Tile       X";
            // 
            // PositionZ
            // 
            // 
            // 
            // 
            this.PositionZ.CustomButton.Image = null;
            this.PositionZ.CustomButton.Location = new System.Drawing.Point(53, 1);
            this.PositionZ.CustomButton.Name = "";
            this.PositionZ.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.PositionZ.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.PositionZ.CustomButton.TabIndex = 1;
            this.PositionZ.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.PositionZ.CustomButton.UseSelectable = true;
            this.PositionZ.CustomButton.Visible = false;
            this.PositionZ.Lines = new string[] {
        "0,00000"};
            this.PositionZ.Location = new System.Drawing.Point(284, 25);
            this.PositionZ.MaxLength = 32767;
            this.PositionZ.Name = "PositionZ";
            this.PositionZ.PasswordChar = '\0';
            this.PositionZ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.PositionZ.SelectedText = "";
            this.PositionZ.SelectionLength = 0;
            this.PositionZ.SelectionStart = 0;
            this.PositionZ.ShortcutsEnabled = true;
            this.PositionZ.Size = new System.Drawing.Size(75, 23);
            this.PositionZ.TabIndex = 10;
            this.PositionZ.Text = "0,00000";
            this.PositionZ.UseSelectable = true;
            this.PositionZ.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.PositionZ.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(261, 26);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(17, 19);
            this.metroLabel6.TabIndex = 9;
            this.metroLabel6.Text = "Z";
            // 
            // PositionY
            // 
            // 
            // 
            // 
            this.PositionY.CustomButton.Image = null;
            this.PositionY.CustomButton.Location = new System.Drawing.Point(53, 1);
            this.PositionY.CustomButton.Name = "";
            this.PositionY.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.PositionY.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.PositionY.CustomButton.TabIndex = 1;
            this.PositionY.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.PositionY.CustomButton.UseSelectable = true;
            this.PositionY.CustomButton.Visible = false;
            this.PositionY.Lines = new string[] {
        "0,00000"};
            this.PositionY.Location = new System.Drawing.Point(180, 25);
            this.PositionY.MaxLength = 32767;
            this.PositionY.Name = "PositionY";
            this.PositionY.PasswordChar = '\0';
            this.PositionY.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.PositionY.SelectedText = "";
            this.PositionY.SelectionLength = 0;
            this.PositionY.SelectionStart = 0;
            this.PositionY.ShortcutsEnabled = true;
            this.PositionY.Size = new System.Drawing.Size(75, 23);
            this.PositionY.TabIndex = 8;
            this.PositionY.Text = "0,00000";
            this.PositionY.UseSelectable = true;
            this.PositionY.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.PositionY.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(157, 26);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(17, 19);
            this.metroLabel5.TabIndex = 7;
            this.metroLabel5.Text = "Y";
            // 
            // PositionX
            // 
            // 
            // 
            // 
            this.PositionX.CustomButton.Image = null;
            this.PositionX.CustomButton.Location = new System.Drawing.Point(53, 1);
            this.PositionX.CustomButton.Name = "";
            this.PositionX.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.PositionX.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.PositionX.CustomButton.TabIndex = 1;
            this.PositionX.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.PositionX.CustomButton.UseSelectable = true;
            this.PositionX.CustomButton.Visible = false;
            this.PositionX.Lines = new string[] {
        "0,00000"};
            this.PositionX.Location = new System.Drawing.Point(76, 25);
            this.PositionX.MaxLength = 32767;
            this.PositionX.Name = "PositionX";
            this.PositionX.PasswordChar = '\0';
            this.PositionX.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.PositionX.SelectedText = "";
            this.PositionX.SelectionLength = 0;
            this.PositionX.SelectionStart = 0;
            this.PositionX.ShortcutsEnabled = true;
            this.PositionX.Size = new System.Drawing.Size(75, 23);
            this.PositionX.TabIndex = 6;
            this.PositionX.Text = "0,00000";
            this.PositionX.UseSelectable = true;
            this.PositionX.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.PositionX.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(4, 26);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(66, 19);
            this.metroLabel4.TabIndex = 5;
            this.metroLabel4.Text = "Position X";
            // 
            // metroLabel3
            // 
            this.metroLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroLabel3.BackColor = System.Drawing.Color.Transparent;
            this.metroLabel3.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel3.Location = new System.Drawing.Point(3, 3);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(365, 19);
            this.metroLabel3.TabIndex = 4;
            this.metroLabel3.Text = "Informations";
            this.metroLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // metroPanel3
            // 
            this.metroPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanel3.Controls.Add(this.NavigateButton);
            this.metroPanel3.Controls.Add(this.SelectDestButton);
            this.metroPanel3.Controls.Add(this.StopEditingButton);
            this.metroPanel3.Controls.Add(this.StartEditingButton);
            this.metroPanel3.Controls.Add(this.metroLabel10);
            this.metroPanel3.Controls.Add(this.NewSourceButton);
            this.metroPanel3.Controls.Add(this.metroLabel9);
            this.metroPanel3.Controls.Add(this.RefreshSourcesButton);
            this.metroPanel3.HorizontalScrollbarBarColor = true;
            this.metroPanel3.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel3.HorizontalScrollbarSize = 10;
            this.metroPanel3.Location = new System.Drawing.Point(545, 42);
            this.metroPanel3.Name = "metroPanel3";
            this.metroPanel3.Size = new System.Drawing.Size(153, 286);
            this.metroPanel3.TabIndex = 3;
            this.metroPanel3.VerticalScrollbarBarColor = true;
            this.metroPanel3.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel3.VerticalScrollbarSize = 10;
            // 
            // NavigateButton
            // 
            this.NavigateButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NavigateButton.Location = new System.Drawing.Point(4, 222);
            this.NavigateButton.Name = "NavigateButton";
            this.NavigateButton.Size = new System.Drawing.Size(146, 22);
            this.NavigateButton.TabIndex = 16;
            this.NavigateButton.Text = "Navigate";
            this.NavigateButton.UseSelectable = true;
            this.NavigateButton.Click += new System.EventHandler(this.NavigateButton_Click);
            // 
            // SelectDestButton
            // 
            this.SelectDestButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectDestButton.Location = new System.Drawing.Point(4, 194);
            this.SelectDestButton.Name = "SelectDestButton";
            this.SelectDestButton.Size = new System.Drawing.Size(146, 22);
            this.SelectDestButton.TabIndex = 15;
            this.SelectDestButton.Text = "Select dest";
            this.SelectDestButton.UseSelectable = true;
            this.SelectDestButton.Click += new System.EventHandler(this.SelectDestButton_Click);
            // 
            // StopEditingButton
            // 
            this.StopEditingButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StopEditingButton.Enabled = false;
            this.StopEditingButton.Location = new System.Drawing.Point(3, 118);
            this.StopEditingButton.Name = "StopEditingButton";
            this.StopEditingButton.Size = new System.Drawing.Size(146, 23);
            this.StopEditingButton.TabIndex = 11;
            this.StopEditingButton.Text = "Stop editing";
            this.StopEditingButton.UseSelectable = true;
            this.StopEditingButton.Click += new System.EventHandler(this.StopEditingButton_Click);
            // 
            // StartEditingButton
            // 
            this.StartEditingButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartEditingButton.Enabled = false;
            this.StartEditingButton.Location = new System.Drawing.Point(4, 89);
            this.StartEditingButton.Name = "StartEditingButton";
            this.StartEditingButton.Size = new System.Drawing.Size(146, 23);
            this.StartEditingButton.TabIndex = 10;
            this.StartEditingButton.Text = "Start editing";
            this.StartEditingButton.UseSelectable = true;
            this.StartEditingButton.Click += new System.EventHandler(this.StartEditing_Click);
            // 
            // metroLabel10
            // 
            this.metroLabel10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroLabel10.BackColor = System.Drawing.Color.Transparent;
            this.metroLabel10.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel10.Location = new System.Drawing.Point(3, 172);
            this.metroLabel10.Name = "metroLabel10";
            this.metroLabel10.Size = new System.Drawing.Size(146, 19);
            this.metroLabel10.TabIndex = 9;
            this.metroLabel10.Text = "Test";
            this.metroLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NewSourceButton
            // 
            this.NewSourceButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewSourceButton.Location = new System.Drawing.Point(3, 60);
            this.NewSourceButton.Name = "NewSourceButton";
            this.NewSourceButton.Size = new System.Drawing.Size(146, 23);
            this.NewSourceButton.TabIndex = 8;
            this.NewSourceButton.Text = "New source";
            this.NewSourceButton.UseSelectable = true;
            this.NewSourceButton.Click += new System.EventHandler(this.NewSourceButton_Click);
            // 
            // metroLabel9
            // 
            this.metroLabel9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroLabel9.BackColor = System.Drawing.Color.Transparent;
            this.metroLabel9.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel9.Location = new System.Drawing.Point(4, 6);
            this.metroLabel9.Name = "metroLabel9";
            this.metroLabel9.Size = new System.Drawing.Size(146, 19);
            this.metroLabel9.TabIndex = 7;
            this.metroLabel9.Text = "Tools";
            this.metroLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RefreshSourcesButton
            // 
            this.RefreshSourcesButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshSourcesButton.Location = new System.Drawing.Point(3, 31);
            this.RefreshSourcesButton.Name = "RefreshSourcesButton";
            this.RefreshSourcesButton.Size = new System.Drawing.Size(146, 23);
            this.RefreshSourcesButton.TabIndex = 2;
            this.RefreshSourcesButton.Text = "Refresh sources";
            this.RefreshSourcesButton.UseSelectable = true;
            this.RefreshSourcesButton.Click += new System.EventHandler(this.RefreshSourcesButton_Click);
            // 
            // RefreshTimer
            // 
            this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // metroPanel4
            // 
            this.metroPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanel4.Controls.Add(this.SourceGridView);
            this.metroPanel4.HorizontalScrollbarBarColor = true;
            this.metroPanel4.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel4.HorizontalScrollbarSize = 10;
            this.metroPanel4.Location = new System.Drawing.Point(168, 42);
            this.metroPanel4.Name = "metroPanel4";
            this.metroPanel4.Size = new System.Drawing.Size(371, 203);
            this.metroPanel4.TabIndex = 4;
            this.metroPanel4.VerticalScrollbarBarColor = true;
            this.metroPanel4.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel4.VerticalScrollbarSize = 10;
            // 
            // SourceGridView
            // 
            this.SourceGridView.AllowUserToAddRows = false;
            this.SourceGridView.AllowUserToDeleteRows = false;
            this.SourceGridView.AllowUserToResizeRows = false;
            this.SourceGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.SourceGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SourceGridView.CausesValidation = false;
            this.SourceGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.SourceGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.SourceGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SourceGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column4,
            this.Version,
            this.Column1,
            this.Column2});
            this.SourceGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.SourceGridView.EnableHeadersVisualStyles = false;
            this.SourceGridView.GridColor = System.Drawing.SystemColors.WindowFrame;
            this.SourceGridView.Location = new System.Drawing.Point(0, 0);
            this.SourceGridView.MultiSelect = false;
            this.SourceGridView.Name = "SourceGridView";
            this.SourceGridView.RowHeadersVisible = false;
            this.SourceGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SourceGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.SourceGridView.ShowCellErrors = false;
            this.SourceGridView.ShowCellToolTips = false;
            this.SourceGridView.ShowEditingIcon = false;
            this.SourceGridView.ShowRowErrors = false;
            this.SourceGridView.Size = new System.Drawing.Size(371, 203);
            this.SourceGridView.TabIndex = 3;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Column3.FillWeight = 10F;
            this.Column3.HeaderText = "";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column3.Width = 5;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.FillWeight = 40F;
            this.Column4.HeaderText = "Name";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Version
            // 
            this.Version.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Version.FillWeight = 30F;
            this.Version.HeaderText = "Version";
            this.Version.Name = "Version";
            this.Version.ReadOnly = true;
            this.Version.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Version.Width = 5;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Column1.FillWeight = 10F;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.Width = 5;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Column2.FillWeight = 10F;
            this.Column2.HeaderText = "";
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.Width = 5;
            // 
            // UserControlStudio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.metroPanel4);
            this.Controls.Add(this.metroPanel3);
            this.Controls.Add(this.metroPanel2);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroPanel1);
            this.Name = "UserControlStudio";
            this.Size = new System.Drawing.Size(701, 342);
            this.UseCustomBackColor = true;
            this.UseCustomForeColor = true;
            this.Load += new System.EventHandler(this.UserControlStudio_Load);
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            this.metroPanel3.ResumeLayout(false);
            this.metroPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SourceGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroCheckBox DrawNodesCheckbox;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroCheckBox DrawTilesBoxCheckbox;
        private MetroFramework.Controls.MetroCheckBox DrawConnectionsCheckbox;
        private System.Windows.Forms.Timer SaveTimer;
        private MetroFramework.Controls.MetroCheckBox AutoSaveCheckbox;
        private MetroFramework.Controls.MetroCheckBox AutoMapCheckbox;
        private MetroFramework.Controls.MetroButton SaveNavigationButton;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private MetroFramework.Controls.MetroTextBox TileY;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        private MetroFramework.Controls.MetroTextBox TileX;
        private MetroFramework.Controls.MetroLabel metroLabel8;
        private MetroFramework.Controls.MetroTextBox PositionZ;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private MetroFramework.Controls.MetroTextBox PositionY;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroTextBox PositionX;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroPanel metroPanel3;
        private System.Windows.Forms.Timer RefreshTimer;
        private MetroFramework.Controls.MetroPanel metroPanel4;
        private MetroFramework.Controls.MetroButton NewSourceButton;
        private MetroFramework.Controls.MetroLabel metroLabel9;
        private MetroFramework.Controls.MetroButton RefreshSourcesButton;
        private System.Windows.Forms.DataGridView SourceGridView;
        private MetroFramework.Controls.MetroButton StopEditingButton;
        private MetroFramework.Controls.MetroButton StartEditingButton;
        private MetroFramework.Controls.MetroLabel metroLabel10;
        private MetroFramework.Controls.MetroButton NavigateButton;
        private MetroFramework.Controls.MetroButton SelectDestButton;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Version;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
        private System.Windows.Forms.DataGridViewButtonColumn Column2;
    }
}
