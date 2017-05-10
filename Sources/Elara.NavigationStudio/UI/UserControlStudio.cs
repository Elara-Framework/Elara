using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elara.NavigationStudio
{
    /// <summary>
    /// Main control interface
    /// </summary>
    public partial class UserControlStudio : MetroFramework.Controls.MetroUserControl
    {
        /// <summary>
        /// Navigation studio instance
        /// </summary>
        private NavigationStudio m_Studio;

        private Utils.Vector3 m_Dest = Utils.Vector3.Zero;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_Studio">Navigation studio</param>
        public UserControlStudio(NavigationStudio p_Studio)
        {
            m_Studio = p_Studio;

            InitializeComponent();

            /// Enabled refresh timer
            RefreshTimer.Start();

            RefreshSources();

            SourceGridView.CellContentClick += SourceGridView_OnCellContentClick;
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Refresh sources view
        /// </summary>
        private void RefreshSources()
        {
            string l_OldRow = "";

            if (SourceGridView.SelectedRows.Count == -1)
                l_OldRow = SourceGridView.SelectedRows[0].Cells[1].Value.ToString();

            SourceGridView.Rows.Clear();

            var l_Sources = m_Studio.GetNavWorld().GetAvailableAuthors();

            foreach (var l_Current in l_Sources)
            {
                bool l_Enabled      = m_Studio.GetNavWorld().HasAuthor(l_Current);
                string l_Version    = m_Studio.GetNavWorld().GetAuthorVersion(l_Current).ToString();

                bool l_IsMain       = m_Studio.GetNavWorld().GetAuthorPriority(l_Current) == 1000;
                bool l_IsEditing    = m_Studio.GetNavWorld().GetEditingAuthor().ToLower() == l_Current.ToLower();

                var l_Index = SourceGridView.Rows.Add(l_Enabled, l_Current, l_Version, "Clone", "Main");

                if (l_IsMain)
                    SourceGridView.Rows[l_Index].DefaultCellStyle.BackColor = Color.Green;
                else if (l_IsEditing)
                    SourceGridView.Rows[l_Index].DefaultCellStyle.BackColor = Color.Red;
            }

            if (l_OldRow != "")
            {
                DataGridViewRow l_Row = SourceGridView.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => r.Cells["Name"].Value.ToString().Equals(l_OldRow))
                    .First();

                if (l_Row != null)
                {
                    l_Row.Selected = true;
                    SourceGridView.FirstDisplayedScrollingRowIndex = l_Row.Index;
                    SourceGridView.Update();
                }
            }
            else
            {
                SourceGridView.ClearSelection();
            }
        }

        ////////////////////////////////////////////////////////

        private void DrawNodesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            m_Studio.DrawNodes = DrawNodesCheckbox.Checked;
        }

        private void DrawTilesBoxCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            m_Studio.DrawTilesBox = DrawTilesBoxCheckbox.Checked;
        }

        private void DrawConnectionsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            m_Studio.DrawConnections = DrawConnectionsCheckbox.Checked;
        }

        private void AutoSaveCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoSaveCheckbox.Checked)
                SaveTimer.Start();
            else
                SaveTimer.Stop();
        }

        private void AutoMapCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            m_Studio.SetAutoMap(AutoMapCheckbox.Checked);
        }

        private void SaveTimer_Tick(object sender, EventArgs e)
        {
            m_Studio.Save();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            var l_Player = m_Studio.Elara.Game.ObjectManager.LocalPlayer;

            if (l_Player != null)
            {
                var l_Position = l_Player.Position;

                PositionX.Text = l_Position.X.ToString();
                PositionY.Text = l_Position.Y.ToString();
                PositionZ.Text = l_Position.Z.ToString();
                TileX.Text = l_Position.TileX.ToString();
                TileY.Text = l_Position.TileY.ToString();
            }
            else
            {
                PositionX.Text = "0";
                PositionY.Text = "0";
                PositionZ.Text = "0";
                TileX.Text = "-1";
                TileY.Text = "-1";
            }
        }

        private void RefreshSourcesButton_Click(object sender, EventArgs e)
        {
            this.RefreshSources();
        }

        private void SaveNavigationButton_Click(object sender, EventArgs e)
        {
            m_Studio.Save();
        }

        private void NewSourceButton_Click(object sender, EventArgs e)
        {
            string l_SourceName = ShowMetroInputDialog("New source name ?");

            if (l_SourceName == "" || l_SourceName.Length < 3)
            {
                MetroFramework.MetroMessageBox.Show(this, "Name invalid, need 3 characters at least");
                return;
            }

            if (m_Studio.GetNavWorld().AuthorExist(l_SourceName))
            {
                MetroFramework.MetroMessageBox.Show(this, "Name already used");
                return;
            }

            m_Studio.GetNavWorld().CreateNewAuthor(l_SourceName);
            this.RefreshSources();
        }

        private void SourceGridView_OnCellContentClick(object sender, DataGridViewCellEventArgs p_Event)
        {
            var l_SenderGrid = (DataGridView)sender;
            
            if (p_Event.RowIndex >= 0)
                StartEditingButton.Enabled = m_Studio.GetNavWorld().GetEditingAuthor() == "" ? true : false;
            else
                StartEditingButton.Enabled = false;

            if (l_SenderGrid.Columns[p_Event.ColumnIndex] is DataGridViewButtonColumn &&
                p_Event.RowIndex >= 0)
            {
                var l_AuthorName    = l_SenderGrid.Rows[p_Event.RowIndex].Cells[1].Value.ToString();
                bool l_Clone        = p_Event.ColumnIndex == l_SenderGrid.Columns.Count - 2;

                if (l_Clone)
                {
                    string l_NewName = ShowMetroInputDialog("Clone name ?");

                    if (l_NewName == "" || l_NewName.Length < 3)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Name invalid, need 3 characters at least");
                        return;
                    }

                    if (m_Studio.GetNavWorld().AuthorExist(l_NewName))
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Name already used");
                        return;
                    }

                    m_Studio.GetNavWorld().CloneAuthor(l_AuthorName, l_NewName);
                    this.RefreshSources();
                }
                else
                {
                    m_Studio.GetNavWorld().ResetAuthorsPriorities();
                    m_Studio.GetNavWorld().SetAuthorPriority(l_AuthorName, 1000);
                    this.RefreshSources();
                }
            }
            else if (l_SenderGrid.Columns[p_Event.ColumnIndex] is DataGridViewCheckBoxColumn &&
                p_Event.RowIndex >= 0)
            {
                var l_AuthorName = l_SenderGrid.Rows[p_Event.RowIndex].Cells[1].Value.ToString();

                if (l_AuthorName.ToLower() == m_Studio.GetNavWorld().GetEditingAuthor().ToLower())
                {
                    MetroFramework.MetroMessageBox.Show(this, "Source \"" + l_AuthorName + "\" is currently in edition");
                    return;
                }

                if (!m_Studio.GetNavWorld().HasAuthor(l_AuthorName))
                    m_Studio.GetNavWorld().AddAuthor(l_AuthorName, 0);
                else
                    m_Studio.GetNavWorld().RemoveAuthor(l_AuthorName);

                RefreshSources();
            }
        }

        private void StartEditing_Click(object sender, EventArgs e)
        {
            if (SourceGridView.SelectedRows.Count != 1)
                return;

            var l_Row = SourceGridView.SelectedRows[0];

            if (m_Studio.GetNavWorld().SetEditingAuthor(l_Row.Cells[1].Value.ToString()))
            {
                RefreshSources();
                StopEditingButton.Enabled = true;
                StartEditingButton.Enabled = false;
            }
        }

        private void StopEditingButton_Click(object sender, EventArgs e)
        {
            m_Studio.GetNavWorld().SetEditingAuthor(null);
            RefreshSources();
            StopEditingButton.Enabled = false;
        }

        public static string ShowMetroInputDialog(string p_Caption)
        {
            var l_Window = new MetroFramework.Forms.MetroForm()
            {
                Width           = 500,
                Height          = 150,
                Text            = p_Caption,
                StartPosition   = FormStartPosition.CenterScreen,
                Resizable       = false,
                SizeGripStyle   = SizeGripStyle.Hide,
                MaximizeBox     = false,
                MinimizeBox     = false
            };

            var l_TextBox = new MetroFramework.Controls.MetroTextBox()
            {
                Left    = 50,
                Top     = 80,
                Width   = 400
            };

            var l_Confirmation  = new MetroFramework.Controls.MetroButton()
            {
                Text            = "Ok",
                Left            = 350,
                Width           = 100,
                Top             = 110,
                DialogResult    = DialogResult.OK
            };

            l_Confirmation.Click += (sender, e) => { l_Window.Close(); };

            l_Window.AcceptButton = l_Confirmation;

            l_Window.Controls.Add(l_TextBox);
            l_Window.Controls.Add(l_Confirmation);

            return l_Window.ShowDialog() == DialogResult.OK ? l_TextBox.Text : "";
        }

        private void SelectDestButton_Click(object sender, EventArgs e)
        {
            var l_LocalPlayer = m_Studio.Elara.Game.ObjectManager.LocalPlayer;

            if (l_LocalPlayer != null)
                m_Dest = l_LocalPlayer.Position;
        }

        private void NavigateButton_Click(object sender, EventArgs e)
        {
            var l_LocalPlayer = m_Studio.Elara.Game.ObjectManager.LocalPlayer;

            if (l_LocalPlayer != null)
            {
                var l_PlayerController = new AI.Controllers.PlayerController(m_Studio.Elara.Game);
                var l_Navigator = new AI.Controllers.PlayerWorldNavigator(l_PlayerController);

                l_Navigator.SetWorld(m_Studio.GetNavWorld());
                l_Navigator.SetDestination(l_LocalPlayer.CurrentMapId, m_Dest, 3, true);

                while (l_Navigator.Navigate() == TreeSharp.RunStatus.Running)
                    System.Threading.Thread.Sleep(1);
            }
        }

        private void UserControlStudio_Load(object sender, EventArgs e)
        {

        }
    }
}
