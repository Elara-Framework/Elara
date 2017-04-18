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
            public bool InCombatOnly = true;
        }

        public CombatAssist(Elara p_Elara)
            : base(p_Elara) { }

        private Thread m_PulseThread;
        private UserControlCombatAssist m_Interface;
        
        public CombatAssistSettings Settings { get; private set; } = new CombatAssistSettings();
        public CombatAssistEngine Engine { get; private set; } = null;
        public bool Running { get; private set; } = false;
        public int LastTick { get; private set; } = 0;

        public override string Name => "Combat Assist";
        public override string Author => "Elara";
        public override UserControl Interface => m_Interface;

        public override void OnLoad()
        {
            m_Interface = new UserControlCombatAssist(this);

            Elara.SettingsManager.OnLoadSettings += SettingsManager_OnLoadSettings;
            LoadSettings();
        }

        public override void OnUnload()
        {
            SaveSettings();
            Elara.SettingsManager.OnLoadSettings -= SettingsManager_OnLoadSettings;

            if (m_Interface != null)
            {
                m_Interface.Dispose();
                m_Interface = null;
            }
        }

        public override bool IsRunning => Running;

        public override void Start()
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

        public override void Stop()
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

        private void SettingsManager_OnLoadSettings(SettingsManager p_SettingsManager)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            var l_Settings = new CombatAssistSettings();
            Elara.SettingsManager.LoadSettingXml("CombatAssist", ref l_Settings);

            this.Settings = l_Settings;
            m_Interface?.OnChangeSettings(this.Settings);
        }

        private void SaveSettings()
        {
            Elara.SettingsManager.SaveSettingXml("CombatAssist", this.Settings);
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
