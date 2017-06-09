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
using Elara.WoW.FrameScript;

namespace Elara.CombatAssist
{
    public class CombatAssistEngine : IDisposable
    {
        private bool m_Disposed = false;

        public readonly CombatAssist OwnerCombatAssist;
        public readonly Game GameOwner;
        public readonly PlayerController PlayerController;
        public readonly Logger Logger;
        public readonly Composite Root;

        public WowLocalPlayer LocalPlayer { get; private set; } = null;
        public WowUnit Target { get; private set; } = null;

        public CombatAssistEngine(CombatAssist p_CombatAssist)
        {
            this.OwnerCombatAssist = p_CombatAssist;
            this.GameOwner = p_CombatAssist.Elara.Game;
            this.PlayerController = new PlayerController(p_CombatAssist.Elara.Game);
            this.Logger = p_CombatAssist.Elara.Logger;
            this.GameOwner.OnRenderOverlay += GameOwner_OnRenderOverlay;

            this.Root = CreateComposite();
            this.Root.Start(this);
        }

        RunStatus AcceptLFGInviteAction(object ctx)
        {
            var l_Button = GameOwner.GetFrameByName("LFGDungeonReadyDialogEnterDungeonButton") as SimpleButton;
            if (l_Button?.IsVisible == true)
            {
                GameOwner.Logger.WriteLine("Combat Assist", "Accepting LFG invitation ...");
                GameOwner.BringWindowForeground();
                Thread.Sleep(1000); // Just in case you have 90's computer (I'm sorry for you)

                l_Button.Click(MouseButtons.Left);
                return RunStatus.Success;
            }
            return RunStatus.Failure;
        }

        private Composite CreateComposite()
        {
            return new PrioritySelector(
                new Decorator(ret => OwnerCombatAssist.Settings.AutoAcceptLFGInvite &&
                                     GameOwner.GetFrameByName("LFGDungeonReadyDialogEnterDungeonButton")?.IsVisible == true,
                    new Action(AcceptLFGInviteAction)
                ),
                new Decorator(ret => OwnerCombatAssist.Elara.CombatScript != null,
                    new Action(delegate(object context)
                    {
                        OwnerCombatAssist.Elara.CombatScript.Tick(PlayerController);

                        var l_Target = this.Target;
                        if (l_Target != null && l_Target.Health > 0 && !l_Target.IsNotAttackable)
                        {
                            if (this.OwnerCombatAssist.Settings.AllowPullTarget || LocalPlayer.IsIncombat)
                            {
                                OwnerCombatAssist.Elara.CombatScript.Combat(PlayerController);

                                return RunStatus.Success;
                            }
                        }

                        return RunStatus.Failure;
                    })
                )
            );
        }

        private void GameOwner_OnRenderOverlay(Game.Overlay p_Overlay, Game.Overlay.Renderer p_Renderer)
        {
            var l_Target = this.Target;

            if (l_Target != null)
            {
                var l_Position = l_Target.Position;
                var l_CenterPos = new Utils.Vector3(l_Position.X - (l_Target.CombatReach / 2.0f), l_Position.Y - (l_Target.Movement.BoundingBoxHeight / 2.0f), l_Position.Z);

                p_Renderer.DrawWorldBox(l_CenterPos, l_Target.CombatReach, l_Target.Movement.BoundingBoxHeight, Color.Red);
            }
        }

        ~CombatAssistEngine()
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
            this.LocalPlayer = this.GameOwner.ObjectManager.LocalPlayer;
            this.Target = this.GameOwner.ObjectManager.LocalPlayer?.Target;
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
                Logger.WriteLine("Combat Assist", "Unhandled exception in root composite !");
                Logger.WriteLine("Combat Assist", e.ToString());
                this.Root.Stop(this);
                this.Root.Start(this);
            }
        }
    }
}
