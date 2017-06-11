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
        }

        public void OnChangeSettings(CombatAssist.CombatAssistSettings p_Settings)
        {
            var l_Action = (Action)(() =>
            {
                metroToggleAllowPullTarget.Checked = p_Settings.AllowPullTarget;
                metroToggleAutoAcceptLFGInvite.Checked = p_Settings.AutoAcceptLFGInvite;
            });

            if (InvokeRequired)
                BeginInvoke(l_Action);
            else
                l_Action();
        }

        private void timerRefreshUI_Tick(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            var l_CombatScriptName = m_CombatAssist.Elara.Settings.GetCharacterValue("ELARA_COMBAT_ASSIST", "COMBAT_SCRIPT_NAME", string.Empty);

            if (!string.IsNullOrEmpty(l_CombatScriptName) &&
                metroComboBoxCombatScript.SelectedItem == null &&
                m_CombatAssist.Elara.CombatScripts.Any(x => x.Name == l_CombatScriptName))
            {
                if (!metroComboBoxCombatScript.Items.Contains(l_CombatScriptName))
                    metroComboBoxCombatScript.Items.Add(l_CombatScriptName);

                metroComboBoxCombatScript.SelectedItem = l_CombatScriptName;
            }
            else if (metroComboBoxCombatScript.SelectedItem != null && !string.IsNullOrEmpty(metroComboBoxCombatScript.SelectedItem.ToString()) && !m_CombatAssist.Elara.CombatScripts.Any(x => x.Name == metroComboBoxCombatScript.Text))
            {
                metroComboBoxCombatScript.SelectedItem = null;
            }

            metroToggleEnabled.Checked = this.m_CombatAssist.Elara.Settings.GetCharacterValue<bool>("ELARA_COMBAT_ASSIST", "ENABLED", false);
            metroToggleAutoAcceptLFGInvite.Checked = this.m_CombatAssist.Elara.Settings.GetCharacterValue<bool>("ELARA_COMBAT_ASSIST", "AUTO_ACCEPT_LFG", false);
            metroToggleAllowPullTarget.Checked = this.m_CombatAssist.Elara.Settings.GetCharacterValue<bool>("ELARA_COMBAT_ASSIST", "PULL_TARGET", false);
        }

        private void metroComboBoxCombatScript_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(metroComboBoxCombatScript.Text))
                m_CombatAssist.Elara.Settings.SetCharacterValue("ELARA_COMBAT_ASSIST", "COMBAT_SCRIPT_NAME", metroComboBoxCombatScript.Text);
        }

        private void metroComboBoxCombatScript_DropDown(object sender, EventArgs e)
        {
            metroComboBoxCombatScript.Items.Clear();
            metroComboBoxCombatScript.Items.AddRange(m_CombatAssist.Elara.CombatScripts.Select(x => x.Name).ToArray());
        }

        private void metroToggleEnabled_Click(object sender, EventArgs e)
        {
            m_CombatAssist.Elara.Settings.SetCharacterValue("ELARA_COMBAT_ASSIST", "ENABLED", !metroToggleEnabled.Checked);
            UpdateControls();
        }

        private void metroToggleAutoAcceptLFGInvite_Click(object sender, EventArgs e)
        {
            m_CombatAssist.Elara.Settings.SetCharacterValue("ELARA_COMBAT_ASSIST", "AUTO_ACCEPT_LFG", !metroToggleAutoAcceptLFGInvite.Checked);
            UpdateControls();
        }

        private void metroToggleAllowPullTarget_Click(object sender, EventArgs e)
        {
            m_CombatAssist.Elara.Settings.SetCharacterValue("ELARA_COMBAT_ASSIST", "PULL_TARGET", !metroToggleAllowPullTarget.Checked);
            UpdateControls();
        }
    }
}
