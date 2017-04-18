using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Elara.AI.Controllers;
using Elara.WoW;
using Elara.WoW.Objects;
using Elara.TreeSharp;
using Action = Elara.TreeSharp.Action;

namespace Elara.Fisher
{
    public class FisherEngine : IDisposable
    {
        private const int FISHING_SPELL_ID = 131474;
        private bool m_Disposed = false;

        public readonly Fisher OwnerFisher;
        public readonly Game GameOwner;
        public readonly PlayerController PlayerController;
        public readonly Logger Logger;
        public readonly ObjectManager ObjectManager;
        public readonly ActionBar ActionBar;
        public readonly Composite Root;

        public WowLocalPlayer LocalPlayer { get; private set; } = null;
        public WowGameObject FishingBobber { get; private set; } = null;

        public FisherEngine(Fisher p_Fisher)
        {
            this.OwnerFisher = p_Fisher;
            this.GameOwner = p_Fisher.Elara.Game;
            this.PlayerController = new PlayerController(p_Fisher.Elara.Game);
            this.Logger = p_Fisher.Elara.Logger;
            this.ObjectManager = this.GameOwner.ObjectManager;
            this.ActionBar = this.GameOwner.ActionBar;
            this.GameOwner.OnRenderOverlay += GameOwner_OnRenderOverlay;

            this.Root = CreateComposite();
            this.Root.Start(this);
        }

        public class InteractFishingBobberAction : Action
        {
            private readonly FisherEngine m_Engine;

            public InteractFishingBobberAction(FisherEngine p_Engine)
            {
                m_Engine = p_Engine;
            }

            protected override RunStatus Run(object context)
            {
                if (m_Engine.FishingBobber == null)
                    return RunStatus.Failure;

                var l_ScreenPos = new Point();
                if (!m_Engine.FishingBobber.GetScreenPosition(ref l_ScreenPos))
                    return RunStatus.Failure;

                using (var l_LockedCursor = m_Engine.GameOwner.ActiveMouseController.LockCursor(l_ScreenPos))
                {
                    l_LockedCursor.Click(MouseButtons.Right);
                }

                Thread.Sleep(1000);
                return RunStatus.Success;
            }
        }

        public class CastFishingAction : Action
        {
            private readonly FisherEngine m_Engine;

            public CastFishingAction(FisherEngine p_Engine)
            {
                m_Engine = p_Engine;
            }

            protected override RunStatus Run(object context)
            {
                if (m_Engine.PlayerController.SpellController.UseSpell(new SpellInfo(m_Engine.GameOwner, FISHING_SPELL_ID)))
                {
                    Thread.Sleep(1000);
                    return RunStatus.Success;
                }
                return RunStatus.Failure;
            }
        }

        private Composite CreateComposite()
        {
            return new PrioritySelector(
                new Decorator(ret => FishingBobber?.IsObjectLocked == true,
                    new InteractFishingBobberAction(this)
                ),
                new Decorator(ret => LocalPlayer.CastingInfo == null,
                    new CastFishingAction(this)
                )
            );
        }

        private void GameOwner_OnRenderOverlay(Game.Overlay p_Overlay, Game.Overlay.Renderer p_Renderer)
        {
            var l_FishingBobber = this.FishingBobber;

            if (l_FishingBobber != null)
            {
                var l_Position = l_FishingBobber.Position;
                var l_CenterPos = new Utils.Vector3(l_Position.X - (1.0f / 2.0f), l_Position.Y - (1.0f / 2.0f), l_Position.Z);

                p_Renderer.DrawWorldBox(l_CenterPos, 1.0f, 1.0f, Color.DarkRed);
            }
        }

        ~FisherEngine()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!m_Disposed)
            {
                this.GameOwner.OnRenderOverlay -= GameOwner_OnRenderOverlay;
                m_Disposed = true;
            }
        }

        public void Tick()
        {
            this.LocalPlayer = this.ObjectManager.LocalPlayer;
            this.FishingBobber = this.ObjectManager.GetObjectsOfType<WowGameObject>(false).FirstOrDefault(x => x.CreatedByGuid == LocalPlayer.Guid);

            if (this.LocalPlayer == null)
                return;

            try
            {
                this.Root.Tick(this);
                if (this.Root.LastStatus != RunStatus.Running)
                {
                    this.Root.Stop(this);
                    this.Root.Start(this);
                }
            }
            catch (Exception e)
            {
                Logger.WriteLine("Fisher", "Unhandled exception in root composite !");
                Logger.WriteLine("Fisher", e.ToString());
                this.Root.Stop(this);
                this.Root.Start(this);
            }
        }
    }
}
