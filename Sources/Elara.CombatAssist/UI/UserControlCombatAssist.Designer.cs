namespace Elara.CombatAssist
{
    partial class UserControlCombatAssist
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.metroLabelAllowPullTarget = new MetroFramework.Controls.MetroLabel();
            this.metroToggleAllowPullTarget = new MetroFramework.Controls.MetroToggle();
            this.metroLabelAutoAcceptLFGInvite = new MetroFramework.Controls.MetroLabel();
            this.metroToggleAutoAcceptLFGInvite = new MetroFramework.Controls.MetroToggle();
            this.metroComboBoxCombatScript = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroToggleEnabled = new MetroFramework.Controls.MetroToggle();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Elara.CombatAssist.Properties.Resources.Sword_Filled_100;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(700, 110);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // metroLabelAllowPullTarget
            // 
            this.metroLabelAllowPullTarget.BackColor = System.Drawing.Color.Transparent;
            this.metroLabelAllowPullTarget.ForeColor = System.Drawing.Color.Black;
            this.metroLabelAllowPullTarget.Location = new System.Drawing.Point(5, 177);
            this.metroLabelAllowPullTarget.Name = "metroLabelAllowPullTarget";
            this.metroLabelAllowPullTarget.Size = new System.Drawing.Size(263, 20);
            this.metroLabelAllowPullTarget.TabIndex = 12;
            this.metroLabelAllowPullTarget.Text = "Allow pulling out of combat target";
            this.metroLabelAllowPullTarget.UseCustomBackColor = true;
            this.metroLabelAllowPullTarget.UseCustomForeColor = true;
            // 
            // metroToggleAllowPullTarget
            // 
            this.metroToggleAllowPullTarget.BackColor = System.Drawing.Color.Transparent;
            this.metroToggleAllowPullTarget.DisplayStatus = false;
            this.metroToggleAllowPullTarget.Location = new System.Drawing.Point(274, 177);
            this.metroToggleAllowPullTarget.Name = "metroToggleAllowPullTarget";
            this.metroToggleAllowPullTarget.Size = new System.Drawing.Size(60, 20);
            this.metroToggleAllowPullTarget.TabIndex = 11;
            this.metroToggleAllowPullTarget.Text = "Off";
            this.metroToggleAllowPullTarget.UseCustomBackColor = true;
            this.metroToggleAllowPullTarget.UseSelectable = true;
            this.metroToggleAllowPullTarget.Click += new System.EventHandler(this.metroToggleAllowPullTarget_Click);
            // 
            // metroLabelAutoAcceptLFGInvite
            // 
            this.metroLabelAutoAcceptLFGInvite.BackColor = System.Drawing.Color.Transparent;
            this.metroLabelAutoAcceptLFGInvite.ForeColor = System.Drawing.Color.Black;
            this.metroLabelAutoAcceptLFGInvite.Location = new System.Drawing.Point(5, 203);
            this.metroLabelAutoAcceptLFGInvite.Name = "metroLabelAutoAcceptLFGInvite";
            this.metroLabelAutoAcceptLFGInvite.Size = new System.Drawing.Size(263, 20);
            this.metroLabelAutoAcceptLFGInvite.TabIndex = 14;
            this.metroLabelAutoAcceptLFGInvite.Text = "Auto accept LFG invite";
            this.metroLabelAutoAcceptLFGInvite.UseCustomBackColor = true;
            this.metroLabelAutoAcceptLFGInvite.UseCustomForeColor = true;
            // 
            // metroToggleAutoAcceptLFGInvite
            // 
            this.metroToggleAutoAcceptLFGInvite.BackColor = System.Drawing.Color.Transparent;
            this.metroToggleAutoAcceptLFGInvite.DisplayStatus = false;
            this.metroToggleAutoAcceptLFGInvite.Location = new System.Drawing.Point(274, 203);
            this.metroToggleAutoAcceptLFGInvite.Name = "metroToggleAutoAcceptLFGInvite";
            this.metroToggleAutoAcceptLFGInvite.Size = new System.Drawing.Size(60, 20);
            this.metroToggleAutoAcceptLFGInvite.TabIndex = 13;
            this.metroToggleAutoAcceptLFGInvite.Text = "Off";
            this.metroToggleAutoAcceptLFGInvite.UseCustomBackColor = true;
            this.metroToggleAutoAcceptLFGInvite.UseSelectable = true;
            this.metroToggleAutoAcceptLFGInvite.Click += new System.EventHandler(this.metroToggleAutoAcceptLFGInvite_Click);
            // 
            // metroComboBoxCombatScript
            // 
            this.metroComboBoxCombatScript.FormattingEnabled = true;
            this.metroComboBoxCombatScript.ItemHeight = 23;
            this.metroComboBoxCombatScript.Location = new System.Drawing.Point(274, 116);
            this.metroComboBoxCombatScript.Name = "metroComboBoxCombatScript";
            this.metroComboBoxCombatScript.PromptText = "Combat script ...";
            this.metroComboBoxCombatScript.Size = new System.Drawing.Size(208, 29);
            this.metroComboBoxCombatScript.Sorted = true;
            this.metroComboBoxCombatScript.TabIndex = 17;
            this.metroComboBoxCombatScript.UseSelectable = true;
            this.metroComboBoxCombatScript.DropDown += new System.EventHandler(this.metroComboBoxCombatScript_DropDown);
            this.metroComboBoxCombatScript.SelectedValueChanged += new System.EventHandler(this.metroComboBoxCombatScript_SelectedValueChanged);
            // 
            // metroLabel1
            // 
            this.metroLabel1.BackColor = System.Drawing.Color.Transparent;
            this.metroLabel1.ForeColor = System.Drawing.Color.Black;
            this.metroLabel1.Location = new System.Drawing.Point(5, 122);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(263, 20);
            this.metroLabel1.TabIndex = 18;
            this.metroLabel1.Text = "Combat script";
            this.metroLabel1.UseCustomBackColor = true;
            this.metroLabel1.UseCustomForeColor = true;
            // 
            // metroLabel2
            // 
            this.metroLabel2.BackColor = System.Drawing.Color.Transparent;
            this.metroLabel2.ForeColor = System.Drawing.Color.Black;
            this.metroLabel2.Location = new System.Drawing.Point(5, 151);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(263, 20);
            this.metroLabel2.TabIndex = 20;
            this.metroLabel2.Text = "Enable combat assist";
            this.metroLabel2.UseCustomBackColor = true;
            this.metroLabel2.UseCustomForeColor = true;
            // 
            // metroToggleEnabled
            // 
            this.metroToggleEnabled.BackColor = System.Drawing.Color.Transparent;
            this.metroToggleEnabled.DisplayStatus = false;
            this.metroToggleEnabled.Location = new System.Drawing.Point(274, 151);
            this.metroToggleEnabled.Name = "metroToggleEnabled";
            this.metroToggleEnabled.Size = new System.Drawing.Size(60, 20);
            this.metroToggleEnabled.TabIndex = 19;
            this.metroToggleEnabled.Text = "Off";
            this.metroToggleEnabled.UseCustomBackColor = true;
            this.metroToggleEnabled.UseSelectable = true;
            this.metroToggleEnabled.Click += new System.EventHandler(this.metroToggleEnabled_Click);
            // 
            // UserControlCombatAssist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroToggleEnabled);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.metroComboBoxCombatScript);
            this.Controls.Add(this.metroLabelAutoAcceptLFGInvite);
            this.Controls.Add(this.metroToggleAutoAcceptLFGInvite);
            this.Controls.Add(this.metroLabelAllowPullTarget);
            this.Controls.Add(this.metroToggleAllowPullTarget);
            this.Controls.Add(this.pictureBox1);
            this.Name = "UserControlCombatAssist";
            this.Size = new System.Drawing.Size(700, 350);
            this.UseCustomBackColor = true;
            this.UseCustomForeColor = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private MetroFramework.Controls.MetroLabel metroLabelAllowPullTarget;
        private MetroFramework.Controls.MetroToggle metroToggleAllowPullTarget;
        private MetroFramework.Controls.MetroLabel metroLabelAutoAcceptLFGInvite;
        private MetroFramework.Controls.MetroToggle metroToggleAutoAcceptLFGInvite;
        private MetroFramework.Controls.MetroComboBox metroComboBoxCombatScript;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroToggle metroToggleEnabled;
    }
}
