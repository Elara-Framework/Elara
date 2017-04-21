namespace Elara.BaseCombats.UI
{
    partial class UserControlMage
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
            this.metroLabelUseFrozenOrb = new MetroFramework.Controls.MetroLabel();
            this.metroToggleUseFrozenOrb = new MetroFramework.Controls.MetroToggle();
            this.metroLabelUseBlizzard = new MetroFramework.Controls.MetroLabel();
            this.metroToggleUseBlizzard = new MetroFramework.Controls.MetroToggle();
            this.metroToggleUseCDs = new MetroFramework.Controls.MetroToggle();
            this.metroLabelUseCDs = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroLabelUseFrozenOrb
            // 
            this.metroLabelUseFrozenOrb.BackColor = System.Drawing.Color.Transparent;
            this.metroLabelUseFrozenOrb.ForeColor = System.Drawing.Color.Black;
            this.metroLabelUseFrozenOrb.Location = new System.Drawing.Point(86, 24);
            this.metroLabelUseFrozenOrb.Name = "metroLabelUseFrozenOrb";
            this.metroLabelUseFrozenOrb.Size = new System.Drawing.Size(151, 20);
            this.metroLabelUseFrozenOrb.TabIndex = 5;
            this.metroLabelUseFrozenOrb.Text = "Use Frozen Orb";
            this.metroLabelUseFrozenOrb.UseCustomBackColor = true;
            this.metroLabelUseFrozenOrb.UseCustomForeColor = true;
            // 
            // metroToggleUseFrozenOrb
            // 
            this.metroToggleUseFrozenOrb.BackColor = System.Drawing.Color.Transparent;
            this.metroToggleUseFrozenOrb.DisplayStatus = false;
            this.metroToggleUseFrozenOrb.Location = new System.Drawing.Point(20, 24);
            this.metroToggleUseFrozenOrb.Name = "metroToggleUseFrozenOrb";
            this.metroToggleUseFrozenOrb.Size = new System.Drawing.Size(60, 20);
            this.metroToggleUseFrozenOrb.TabIndex = 4;
            this.metroToggleUseFrozenOrb.Text = "Off";
            this.metroToggleUseFrozenOrb.UseCustomBackColor = true;
            this.metroToggleUseFrozenOrb.UseSelectable = true;
            this.metroToggleUseFrozenOrb.CheckedChanged += new System.EventHandler(this.metroToggleUseFrozenOrb_CheckedChanged);
            // 
            // metroLabelUseBlizzard
            // 
            this.metroLabelUseBlizzard.BackColor = System.Drawing.Color.Transparent;
            this.metroLabelUseBlizzard.ForeColor = System.Drawing.Color.Black;
            this.metroLabelUseBlizzard.Location = new System.Drawing.Point(86, 50);
            this.metroLabelUseBlizzard.Name = "metroLabelUseBlizzard";
            this.metroLabelUseBlizzard.Size = new System.Drawing.Size(151, 20);
            this.metroLabelUseBlizzard.TabIndex = 7;
            this.metroLabelUseBlizzard.Text = "Use Blizzard";
            this.metroLabelUseBlizzard.UseCustomBackColor = true;
            this.metroLabelUseBlizzard.UseCustomForeColor = true;
            // 
            // metroToggleUseBlizzard
            // 
            this.metroToggleUseBlizzard.BackColor = System.Drawing.Color.Transparent;
            this.metroToggleUseBlizzard.DisplayStatus = false;
            this.metroToggleUseBlizzard.Location = new System.Drawing.Point(20, 50);
            this.metroToggleUseBlizzard.Name = "metroToggleUseBlizzard";
            this.metroToggleUseBlizzard.Size = new System.Drawing.Size(60, 20);
            this.metroToggleUseBlizzard.TabIndex = 6;
            this.metroToggleUseBlizzard.Text = "Off";
            this.metroToggleUseBlizzard.UseCustomBackColor = true;
            this.metroToggleUseBlizzard.UseSelectable = true;
            this.metroToggleUseBlizzard.CheckedChanged += new System.EventHandler(this.metroToggleUseBlizzard_CheckedChanged);
            // 
            // metroToggleUseCDs
            // 
            this.metroToggleUseCDs.BackColor = System.Drawing.Color.Transparent;
            this.metroToggleUseCDs.DisplayStatus = false;
            this.metroToggleUseCDs.Location = new System.Drawing.Point(20, 76);
            this.metroToggleUseCDs.Name = "metroToggleUseCDs";
            this.metroToggleUseCDs.Size = new System.Drawing.Size(60, 20);
            this.metroToggleUseCDs.TabIndex = 8;
            this.metroToggleUseCDs.Text = "Off";
            this.metroToggleUseCDs.UseCustomBackColor = true;
            this.metroToggleUseCDs.UseSelectable = true;
            // 
            // metroLabelUseCDs
            // 
            this.metroLabelUseCDs.BackColor = System.Drawing.Color.Transparent;
            this.metroLabelUseCDs.ForeColor = System.Drawing.Color.Black;
            this.metroLabelUseCDs.Location = new System.Drawing.Point(86, 76);
            this.metroLabelUseCDs.Name = "metroLabelUseCDs";
            this.metroLabelUseCDs.Size = new System.Drawing.Size(151, 20);
            this.metroLabelUseCDs.TabIndex = 9;
            this.metroLabelUseCDs.Text = "Use Cooldowns";
            this.metroLabelUseCDs.UseCustomBackColor = true;
            this.metroLabelUseCDs.UseCustomForeColor = true;
            // 
            // UserControlMage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.metroLabelUseCDs);
            this.Controls.Add(this.metroToggleUseCDs);
            this.Controls.Add(this.metroLabelUseBlizzard);
            this.Controls.Add(this.metroToggleUseBlizzard);
            this.Controls.Add(this.metroLabelUseFrozenOrb);
            this.Controls.Add(this.metroToggleUseFrozenOrb);
            this.Name = "UserControlMage";
            this.Size = new System.Drawing.Size(700, 350);
            this.UseCustomBackColor = true;
            this.UseCustomForeColor = true;
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabelUseFrozenOrb;
        private MetroFramework.Controls.MetroToggle metroToggleUseFrozenOrb;
        private MetroFramework.Controls.MetroLabel metroLabelUseBlizzard;
        private MetroFramework.Controls.MetroToggle metroToggleUseBlizzard;
        private MetroFramework.Controls.MetroToggle metroToggleUseCDs;
        private MetroFramework.Controls.MetroLabel metroLabelUseCDs;
    }
}
