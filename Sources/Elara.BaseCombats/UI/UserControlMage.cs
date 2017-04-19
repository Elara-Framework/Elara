using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Elara.BaseCombats;

namespace Elara.BaseCombats.UI
{
    public partial class UserControlMage : MetroFramework.Controls.MetroUserControl
    {
        private readonly Mage m_Combat;

        public UserControlMage(Mage p_Combat)
        {
            m_Combat = p_Combat;
            InitializeComponent();
        }

        public void UpdateSettings(Mage.MageSettings p_Settings)
        {
            metroToggleUseFrozenOrb.Checked = p_Settings.UseFrozenOrb;
            metroToggleUseBlizzard.Checked = p_Settings.UseBlizzard;
        }

        private void metroToggleUseFrozenOrb_CheckedChanged(object sender, EventArgs e)
        {
            m_Combat.CurrentSetting.UseFrozenOrb = metroToggleUseFrozenOrb.Checked;
        }

        private void metroToggleUseBlizzard_CheckedChanged(object sender, EventArgs e)
        {
            m_Combat.CurrentSetting.UseBlizzard = metroToggleUseBlizzard.Checked;
        }
    }
}
