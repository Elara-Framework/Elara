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

namespace Elara.Fisher
{
    public class Fisher : Extensions.IExtension
    {
        private Thread m_PulseThread;
        private UserControlFisher m_Interface;
        private Tuple<string, System.Action>[] m_Options = null;

        public Elara Elara { get; private set; }
        public FisherEngine Engine { get; private set; } = null;
        public bool Running { get; private set; } = false;
        public int TickInterval { get; private set; } = 200;
        public int LastTick { get; private set; } = 0;

        public Tuple<string, System.Action>[] Options => m_Options;

        public override bool OnEnable(Elara p_Elara)
        {
            Elara = p_Elara;
            m_Interface = new UserControlFisher(this);
            
            Elara.AddTabPage("Fisher", m_Interface);

            return true;
        }

        public override bool OnDisable(Elara p_Elara)
        {
            if (Running)
                Stop();

            if (m_Interface != null)
            {
                Elara.RemoveTabPage(m_Interface);

                m_Interface.Dispose();
                m_Interface = null;
            }

            return true;
        }

        public void Start()
        {
            if (!Running)
            {
                Engine = new FisherEngine(this);
                Elara.Logger.WriteLine("Fisher", "Start bot");

                Running = true;
                m_PulseThread = new Thread(Thread_Pulsator);
                m_PulseThread.Start();
            }
        }

        public void Stop()
        {
            if (Running)
            {
                Elara.Logger.WriteLine("Fisher", "Stopping bot ...");
                Running = false;

                if (m_PulseThread != null)
                {
                    m_PulseThread.Join();
                    m_PulseThread = null;
                }

                Engine?.Dispose();
                Engine = null;
                Running = false;
            }
        }

        private void Thread_Pulsator()
        {
            if (Engine != null && !Engine.GameOwner.BackgroundModeEnabled)
                Engine.GameOwner.BringWindowForeground();

            while (Running)
            {
                if (Environment.TickCount - LastTick > TickInterval)
                {
                    Engine?.Tick();
                    LastTick = Environment.TickCount;
                }
                Thread.Sleep(1);
            }
        }
    }
}
