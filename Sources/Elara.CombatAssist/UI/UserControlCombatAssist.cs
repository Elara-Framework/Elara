using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elara.CombatAssist
{
    public partial class UserControlCombatAssist : MetroFramework.Controls.MetroUserControl
    {
        private readonly CombatAssist m_CombatAssist;

        public UserControlCombatAssist(CombatAssist p_CombatAssist)
        {
            m_CombatAssist = p_CombatAssist;

            InitializeComponent();

            OnReloadSettings(p_CombatAssist.Settings);
        }

        public void OnReloadSettings(CombatAssist.CombatAssistSettings p_Settings)
        {
            var l_Action = (Action)(() =>
            {
                metroComboBoxCombatScript.Items.Clear();
                metroComboBoxCombatScript.Items.AddRange(m_CombatAssist.Elara.CombatScripts.Select(x => x.Name).ToArray());
                metroComboBoxCombatScript.SelectedItem = p_Settings.CombatScript;

                metroToggleEnabled.Checked                  = p_Settings.Enabled;
                metroToggleAllowPullTarget.Checked          = p_Settings.AllowPullTarget;
                metroToggleAutoAcceptLFGInvite.Checked      = p_Settings.AutoAcceptLFGInvite;
            });

            if (this.InvokeRequired)
                this.Invoke(l_Action);
            else
                l_Action();
        }

        private void metroComboBoxCombatScript_SelectedValueChanged(object sender, EventArgs e)
        {
            m_CombatAssist.Settings.CombatScript = metroComboBoxCombatScript.Text ?? string.Empty;
        }

        private void metroComboBoxCombatScript_DropDown(object sender, EventArgs e)
        {
            metroComboBoxCombatScript.Items.Clear();
            metroComboBoxCombatScript.Items.AddRange(m_CombatAssist.Elara.CombatScripts.Select(x => x.Name).ToArray());
        }

        private void metroToggleEnabled_Click(object sender, EventArgs e)
        {
            m_CombatAssist.Settings.Enabled = metroToggleEnabled.Checked;
        }

        private void metroToggleAutoAcceptLFGInvite_Click(object sender, EventArgs e)
        {
            m_CombatAssist.Settings.AutoAcceptLFGInvite = metroToggleAutoAcceptLFGInvite.Checked;
        }

        private void metroToggleAllowPullTarget_Click(object sender, EventArgs e)
        {
            m_CombatAssist.Settings.AllowPullTarget = metroToggleAllowPullTarget.Checked;
        }
    }
}
