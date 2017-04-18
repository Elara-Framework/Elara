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
            this.SuspendLayout();
            // 
            // metroLabelUseFrozenOrb
            // 
            this.metroLabelUseFrozenOrb.BackColor = System.Drawing.Color.Transparent;
            this.metroLabelUseFrozenOrb.ForeColor = System.Drawing.Color.Black;
            this.metroLabelUseFrozenOrb.Location = new System.Drawing.Point(86, 20);
            this.metroLabelUseFrozenOrb.Name = "metroLabelUseFrozenOrb";
            this.metroLabelUseFrozenOrb.Size = new System.Drawing.Size(379, 20);
            this.metroLabelUseFrozenOrb.TabIndex = 5;
            this.metroLabelUseFrozenOrb.Text = "Use Frozen Orb";
            this.metroLabelUseFrozenOrb.UseCustomBackColor = true;
            this.metroLabelUseFrozenOrb.UseCustomForeColor = true;
            // 
            // metroToggleUseFrozenOrb
            // 
            this.metroToggleUseFrozenOrb.BackColor = System.Drawing.Color.Transparent;
            this.metroToggleUseFrozenOrb.DisplayStatus = false;
            this.metroToggleUseFrozenOrb.Location = new System.Drawing.Point(20, 20);
            this.metroToggleUseFrozenOrb.Name = "metroToggleUseFrozenOrb";
            this.metroToggleUseFrozenOrb.Size = new System.Drawing.Size(60, 20);
            this.metroToggleUseFrozenOrb.TabIndex = 4;
            this.metroToggleUseFrozenOrb.Text = "Off";
            this.metroToggleUseFrozenOrb.UseCustomBackColor = true;
            this.metroToggleUseFrozenOrb.UseSelectable = true;
            this.metroToggleUseFrozenOrb.CheckedChanged += new System.EventHandler(this.metroToggleUseFrozenOrb_CheckedChanged);
            // 
            // UserControlMage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
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
    }
}
