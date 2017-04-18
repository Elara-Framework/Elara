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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroToggleAttackInCombatOnly = new MetroFramework.Controls.MetroToggle();
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
            // metroLabel1
            // 
            this.metroLabel1.BackColor = System.Drawing.Color.Transparent;
            this.metroLabel1.ForeColor = System.Drawing.Color.Black;
            this.metroLabel1.Location = new System.Drawing.Point(71, 116);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(447, 20);
            this.metroLabel1.TabIndex = 12;
            this.metroLabel1.Text = "Use combat assist only in combat (will attempt to pull otherwise)";
            this.metroLabel1.UseCustomBackColor = true;
            this.metroLabel1.UseCustomForeColor = true;
            // 
            // metroToggleAttackInCombatOnly
            // 
            this.metroToggleAttackInCombatOnly.BackColor = System.Drawing.Color.Transparent;
            this.metroToggleAttackInCombatOnly.DisplayStatus = false;
            this.metroToggleAttackInCombatOnly.Location = new System.Drawing.Point(5, 116);
            this.metroToggleAttackInCombatOnly.Name = "metroToggleAttackInCombatOnly";
            this.metroToggleAttackInCombatOnly.Size = new System.Drawing.Size(60, 20);
            this.metroToggleAttackInCombatOnly.TabIndex = 11;
            this.metroToggleAttackInCombatOnly.Text = "Off";
            this.metroToggleAttackInCombatOnly.UseCustomBackColor = true;
            this.metroToggleAttackInCombatOnly.UseSelectable = true;
            this.metroToggleAttackInCombatOnly.CheckedChanged += new System.EventHandler(this.metroToggleAttackInCombatOnly_CheckedChanged);
            // 
            // UserControlCombatAssist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.metroToggleAttackInCombatOnly);
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
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroToggle metroToggleAttackInCombatOnly;
    }
}
