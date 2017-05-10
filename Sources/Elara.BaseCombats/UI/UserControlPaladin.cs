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
    public partial class UserControlPaladin : MetroFramework.Controls.MetroUserControl
    {
        private readonly Paladin m_Combat;

        public UserControlPaladin(Paladin p_Combat)
        {
            m_Combat = p_Combat;
            InitializeComponent();
        }

        public void UpdateSettings(Paladin.PaladinSettings p_Settings)
        {
            var l_Action = (Action)(() =>
            {
                // Update UI controls
            });

            if (InvokeRequired)
                BeginInvoke(l_Action);
            else
                l_Action();
        }
    }
}
