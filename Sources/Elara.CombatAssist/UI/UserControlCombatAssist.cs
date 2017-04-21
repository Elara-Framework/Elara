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

        private void metroToggleAttackInCombatOnly_CheckedChanged(object sender, EventArgs e)
        {
            m_CombatAssist.Settings.AllowPullTarget = metroToggleAllowPullTarget.Checked;
        }

        private void metroToggleAutoAcceptLFGInvite_CheckedChanged(object sender, EventArgs e)
        {
            m_CombatAssist.Settings.AutoAcceptLFGInvite = metroToggleAutoAcceptLFGInvite.Checked;
        }
    }
}
