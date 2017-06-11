using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Elara.WoW;
using Elara.WoW.Objects;
using Elara.TreeSharp;

namespace Elara.CombatAssist
{
    public class CombatAssist : Extensions.IExtension
    {
        [Serializable]
        public class CombatAssistSettings
        {
            public bool AllowPullTarget = false;
            public bool AutoAcceptLFGInvite = false;
        }
        
        private UserControlCombatAssist m_Interface;
        private Tuple<string, System.Action>[] m_Options = null;
        public Elara Elara { get; private set; }
        public CombatAssistSettings Settings { get; private set; } = new CombatAssistSettings();
        public CombatAssistEngine Engine { get; private set; } = null;
        public bool Running { get; private set; } = false;

        public Tuple<string, System.Action>[] Options => m_Options;

        public bool OnEnable(Elara p_Elara)
        {
            Elara = p_Elara;

            m_Interface = new UserControlCombatAssist(this);
            Elara.AddTabPage("Combat Assist", m_Interface);
            
            Elara.Game.OnUpdate += Game_OnUpdate;
            Engine = new CombatAssistEngine(this);

            return true;
        }

        public bool OnDisable(Elara p_Elara)
        {
            Engine.Dispose();
            Elara.Game.OnUpdate -= Game_OnUpdate;

            if (m_Interface != null)
            {
                Elara.RemoveTabPage(m_Interface);

                m_Interface.Dispose();
                m_Interface = null;
            }

            return true;
        }

        private void Game_OnUpdate(Game p_Game, TimeSpan p_Delta)
        {
            if (Engine != null && Elara.Settings.GetCharacterValue<bool>("ELARA_COMBAT_ASSIST", "ENABLED") == true)
            {
                Engine.Pulse();
            }
        }
    }
}
