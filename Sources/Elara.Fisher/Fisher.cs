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
    public class Fisher : Extensions.Module
    {
        public Fisher(Elara p_Elara)
            : base(p_Elara) { }

        private Thread m_PulseThread;
        private UserControlFisher m_Interface;
        
        public FisherEngine Engine { get; private set; } = null;
        public bool Running { get; private set; } = false;
        public int TickInterval { get; private set; } = 200;
        public int LastTick { get; private set; } = 0;

        public override string Name => "Fisher";
        public override string Category => "Bots";
        public override string Author => "Elara";
        public override UserControl Interface => m_Interface;

        public override bool OnLoad()
        {
            m_Interface = new UserControlFisher(this);

            return base.OnLoad();
        }

        public override bool OnUnload()
        {
            if (Running)
                Stop();

            m_Interface?.Dispose();
            m_Interface = null;

            return base.OnUnload();
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
            if (Engine != null && !Engine.GameOwner.BackgroundMode)
            {
                Engine.GameOwner.BringWindowForeground();
            }

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
