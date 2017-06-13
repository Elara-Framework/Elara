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
using System.IO;

namespace Elara.CombatAssist
{
    public class CombatAssist : Extensions.IExtension
    {
        [Serializable]
        public class CombatAssistSettings
        {
            public string CombatScript = string.Empty;
            public bool Enabled = false;
            public bool AllowPullTarget = false;
            public bool AutoAcceptLFGInvite = false;
        }
        
        private UserControlCombatAssist m_Interface;
        public Elara Elara { get; private set; }
        public CombatAssistSettings Settings { get; private set; } = new CombatAssistSettings();
        public CombatAssistEngine Engine { get; private set; } = null;
        public bool Running { get; private set; } = false;

        public override bool OnEnable(Elara p_Elara)
        {
            Elara = p_Elara;
            Settings = Utils.Serialization.DeserializeFromJson<CombatAssistSettings>(Path.Combine(this.MetaData.Directory.FullName, "Settings.json")) ?? new CombatAssistSettings();
            Engine = new CombatAssistEngine(this);
            m_Interface = new UserControlCombatAssist(this);

            Elara.AddTabPage("Combat Assist", m_Interface);
            
            Elara.Game.OnUpdate += Game_OnUpdate;
            return true;
        }

        public override bool OnDisable(Elara p_Elara)
        {
            Elara.Game.OnUpdate -= Game_OnUpdate;

            Utils.Serialization.SerializeToJson(Path.Combine(this.MetaData.Directory.FullName, "Settings.json"), this.Settings);

            Engine.Dispose();

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
            if (Engine != null && Settings.Enabled)
            {
                Engine.Pulse();
            }
        }
    }
}
