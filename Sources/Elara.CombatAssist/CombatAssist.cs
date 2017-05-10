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
    public class CombatAssist : Extensions.Module
    {
        [Serializable]
        public class CombatAssistSettings
        {
            public int TickInterval = 100;
            public bool AllowPullTarget = false;
            public bool AutoAcceptLFGInvite = false;
        }

        public CombatAssist(Elara p_Elara)
            : base(p_Elara) { }

        private Thread m_PulseThread;
        private SettingsManager m_SettingsManager;
        private UserControlCombatAssist m_Interface;
        
        public CombatAssistSettings Settings { get; private set; } = new CombatAssistSettings();
        public CombatAssistEngine Engine { get; private set; } = null;
        public bool Running { get; private set; } = false;
        public int LastTick { get; private set; } = 0;

        public override string Name => "Combat Assist";
        public override string Category => "Tools";
        public override string Author => "Elara";
        public override UserControl Interface => m_Interface;

        public override bool OnLoad()
        {
            m_SettingsManager = new SettingsManager(Elara.Game.ObjectManager.LocalPlayer);
            m_Interface = new UserControlCombatAssist(this);

            Elara.Game.OnChangeActivePlayer += Game_OnChangeActivePlayer;
            LoadSettings();

            return base.OnLoad();
        }

        public override bool OnUnload()
        {
            if (Running)
                Stop();

            Elara.Game.OnChangeActivePlayer -= Game_OnChangeActivePlayer;
            SaveSettings();

            if (m_Interface != null)
            {
                m_Interface.Dispose();
                m_Interface = null;
            }

            return base.OnUnload();
        }

        private void Game_OnChangeActivePlayer(Game p_Game, WowLocalPlayer p_LocalPlayer)
        {
            SaveSettings();
            m_SettingsManager = new SettingsManager(p_LocalPlayer);
            LoadSettings();
        }

        public void Start()
        {
            if (!Running)
            {
                if (Elara.CombatScript == null)
                {
                    MetroFramework.MetroMessageBox.Show(m_Interface, "Please select a combat script before starting Combat Assist !",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Engine = new CombatAssistEngine(this);

                Running = true;
                m_PulseThread = new Thread(Thread_Pulsator);
                m_PulseThread.Start();
                Elara.Logger.WriteLine("Combat Assist", "Combat assist started !");
            }
        }

        public void Stop()
        {
            if (Running)
            {
                Running = false;

                if (m_PulseThread != null)
                {
                    m_PulseThread.Join();
                    m_PulseThread = null;
                }

                Engine?.Dispose();
                Engine = null;
                Running = false;
                Elara.Logger.WriteLine("Combat Assist", "Combat assist stopped !");
            }
        }

        private void LoadSettings()
        {
            var l_Settings = new CombatAssistSettings();
            m_SettingsManager.LoadSettingsXml("CombatAssist", ref l_Settings, true);

            this.Settings = l_Settings;
            m_Interface?.OnChangeSettings(this.Settings);
        }

        private void SaveSettings()
        {
            m_SettingsManager.SaveSettingsXml("CombatAssist", this.Settings, true);
        }

        private void Thread_Pulsator()
        {
            while (Running)
            {
                if (Environment.TickCount - LastTick > Settings.TickInterval)
                {
                    Engine?.Tick();
                    LastTick = Environment.TickCount;
                }
                Thread.Sleep(1);
            }
        }
    }
}
