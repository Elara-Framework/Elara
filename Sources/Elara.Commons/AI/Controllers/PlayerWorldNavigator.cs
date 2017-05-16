using Elara.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.AI.Controllers
{
    public class PlayerWorldNavigator : IDisposable
    {
        private bool m_IsDisposed = false;
        public readonly PlayerController Owner;

        public PlayerMoveController MoveController => Owner.MoveController;
        public NavWorld World { get; private set; } = null;
        public NavQuery Query { get; private set; } = null;
        public float OutOfWorldTolerance { get; private set; } = 10.0f;
        public float StopDistance { get; private set; } = 0.0f;
        public int DestinationMapId { get; private set; } = -1;
        public Utils.Vector3 DestinationPosition { get; private set; } = Utils.Vector3.Zero;
        public bool Running { get; private set; } = false;
        public List<NavNode> RunningPath { get; private set; } = null;
        public NavNode DestinationNode { get; private set; } = null;
        public WoW.Companions.MountCompanion Mount { get; private set; } = null;
        public bool Arrived { get; private set; } = false;
        public bool AllowFlying { get; set; } = true;
        public bool AllowSwimming { get; set; } = true;
        public bool DisplayPath { get; set; } = true;
        public bool UseMount { get; set; } = true;

        public PlayerWorldNavigator(PlayerController p_Owner)
        {
            Owner = p_Owner;

            p_Owner.GameOwner.OnRenderOverlay += GameOwner_OnRenderOverlay;
            p_Owner.GameOwner.OnChangeActivePlayer += GameOwner_OnChangeActivePlayer;

            UpdateMount();
        }

        ~PlayerWorldNavigator()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!m_IsDisposed)
            {
                Owner.GameOwner.OnChangeActivePlayer -= GameOwner_OnChangeActivePlayer;
                Owner.GameOwner.OnRenderOverlay -= GameOwner_OnRenderOverlay;

                m_IsDisposed = true;
            }
        }

        private void UpdateMount()
        {
            Mount = null;

            var l_Mounts = Owner.GameOwner.Companions.GetMounts().Where(x => x.ActionBarSlot != null)
                .OrderBy(x => !x.IsGroundMount)
                .OrderBy(x => !x.IsFlyingMount)
                .OrderBy(x => !x.IsScalingMount).ToList();

            Mount = l_Mounts.FirstOrDefault(x => x.IsGroundMount || x.IsFlyingMount || x.IsScalingMount);

            if (Mount != null)
            {
                Owner.GameOwner.Logger.WriteLine("PlayerWorldNavigator", "Using mount : " + Mount.Name);
            }
            else
            {
                Owner.GameOwner.Logger.WriteLine("PlayerWorldNavigator", "Warning : No valid mount found on action bar !");
            }
        }

        private void GameOwner_OnChangeActivePlayer(Game p_Game, WoW.Objects.WowLocalPlayer p_LocalPlayer)
        {
            UpdateMount();
        }

        private void GameOwner_OnRenderOverlay(Game.Overlay p_Overlay, Game.Overlay.Renderer p_Renderer)
        {
            var l_LocalPlayer = Owner.ObjectManager.LocalPlayer;

            if (l_LocalPlayer != null && DisplayPath)
            {
                var l_RunningPath = RunningPath;

                if (l_RunningPath != null && l_RunningPath.Any())
                {
                    lock (l_RunningPath)
                    {
                        foreach (var l_Node in l_RunningPath)
                        {
                            var l_BoxPos = new Utils.Vector3(l_Node.Position.X - 0.50f, l_Node.Position.Y - 0.50f, l_Node.Position.Z);
                            p_Renderer.DrawWorldBox(l_BoxPos, 1.0f, 1.0f, System.Drawing.Color.Green);
                        }
                    }
                }

                if (l_LocalPlayer.CurrentMapId == DestinationMapId)
                {
                    var l_BoxPos = new Utils.Vector3(DestinationPosition.X - 0.50f, DestinationPosition.Y - 0.50f, DestinationPosition.Z);
                    p_Renderer.DrawWorldBox(l_BoxPos, 1.0f, 1.0f, System.Drawing.Color.Green);
                }
            }
        }

        private void UpdateWorld()
        {
            var l_LocalPlayer = Owner.ObjectManager.LocalPlayer;

            if (l_LocalPlayer != null && World != null)
            {
                var l_Position = l_LocalPlayer.Position;

                World.OnMapChange(l_LocalPlayer.CurrentMapId);
                World.OnTileChange(l_Position.TileX, l_Position.TileY);
            }
        }

        public void SetWorld(NavWorld p_World)
        {
            if (p_World != null)
            {
                World = p_World;
            }
            else
            {
                World = null;
            }
        }

        public bool SetDestination(int p_MapId, Utils.Vector3 p_Position, float p_StopDistance = 3.0f, bool p_Force = false)
        {
            var l_LocalPlayer = Owner.ObjectManager.LocalPlayer;

            if (p_StopDistance < 2.0f)
                p_StopDistance = 2.0f;

            if (l_LocalPlayer?.Position.Distance3D(p_Position) <= p_StopDistance)
            {
                DestinationPosition = p_Position;
                DestinationMapId = p_MapId;
                Arrived = true;
                Running = false;
                return true;
            }

            StopDistance = p_StopDistance;
            RunningPath = GetPath(p_Position);

            if (RunningPath != null || p_Force)
            {
                DestinationPosition = p_Position;
                DestinationMapId = p_MapId;
                Arrived = false;
                Running = true;
                return true;
            }
            else
            {
                DestinationPosition = Utils.Vector3.Zero;
                DestinationMapId = -1;
                return false;
            }
        }

        public void Stop()
        {
            if (Running)
            {
                MoveController?.StopMove();
                Running = false;
            }
        }

        public NavNode.NavNodeFlags GetIncludeMask()
        {
            NavNode.NavNodeFlags l_Result = NavNode.NavNodeFlags.None;
            var l_LocalPlayer = Owner.ObjectManager.LocalPlayer;

            if (l_LocalPlayer != null)
                l_Result |= NavNode.NavNodeFlags.Walkable;
            else
                return l_Result;

            if (AllowFlying && l_LocalPlayer.CanFly)
                l_Result |= NavNode.NavNodeFlags.Flying;

            if (AllowSwimming)
                l_Result |= NavNode.NavNodeFlags.Swimming;

            return l_Result;
        }

        public List<NavNode> GetPath(Utils.Vector3 p_Position)
        {
            var l_LocalPlayer = Owner.ObjectManager.LocalPlayer;
            if (World == null || l_LocalPlayer == null)
                return null;

            var l_IncludeMAsk = GetIncludeMask();
            var l_PlayerPosition = l_LocalPlayer.Position;

            var l_StartNode = World.FindNearestNode(l_PlayerPosition, l_IncludeMAsk, OutOfWorldTolerance);
            if (l_StartNode == null)
                return null;

            var l_EndNode = World.FindNearestNode(p_Position, l_IncludeMAsk, OutOfWorldTolerance);
            if (l_EndNode == null)
                return null;

            var l_Settings = new Navigation.NavQuerySettings();
            l_Settings.IncludeMask = l_IncludeMAsk;

            var l_Query = new NavQuery(World.GetNavGraph());

            List<NavNode> l_Path;
            if (l_Query.SearchForPath(l_StartNode, l_EndNode, out l_Path, l_Settings))
            {
               
                return l_Path;
            }

            return null;
        }

        public bool CanMoveTo(Utils.Vector3 p_Position)
        {
            return GetPath(p_Position)?.Any() == true;
        }

        public TreeSharp.RunStatus Navigate()
        {
            if (Arrived)
                return TreeSharp.RunStatus.Success;

            if (DestinationMapId == -1 || !Running)
                return TreeSharp.RunStatus.Failure;

            var l_LocalPlayer = Owner.ObjectManager.LocalPlayer;
            var l_MoveController = MoveController;
            var l_RunningPath = RunningPath;

            if (l_MoveController == null || World == null || l_LocalPlayer == null)
                return TreeSharp.RunStatus.Failure;

            var l_PlayerPosition = l_LocalPlayer.Position;
            float l_PlayerSpeed = l_LocalPlayer.Movement.Speed;
            float l_DistanceCheck = l_PlayerSpeed > 3.0f ? l_PlayerSpeed * 0.4f : 2.0f;

            if (DestinationMapId == l_LocalPlayer.CurrentMapId && 
                DestinationPosition.Distance3D(l_PlayerPosition) <= StopDistance)
            {
                if ((l_LocalPlayer.Movement.Flags & WoW.Objects.MovementShared.MovementFlags.Forward) != 0)
                    l_MoveController.MoveForwardStop();

                if ((l_LocalPlayer.Movement.Flags & WoW.Objects.MovementShared.MovementFlags.Backward) != 0)
                    l_MoveController.MoveBackwardStop();

                Arrived = true;
                return TreeSharp.RunStatus.Success;
            }

            if (l_LocalPlayer.CastingInfo != null)
                return TreeSharp.RunStatus.Running;
            
            if (UseMount && !l_LocalPlayer.IsMounted && !l_LocalPlayer.IsIncombat && Mount?.CanUseNow == true)
            {
                l_MoveController.StopMove();
                Owner.GameOwner.Logger.WriteLine("PlayerWorldNavigator", "Use mount : " + Mount.Name);
                Mount?.Use();
                return TreeSharp.RunStatus.Running;
            }

            NavNode l_NextNode = null;
            Utils.Vector3 l_NextPosition = DestinationPosition;

            if (l_RunningPath != null && l_RunningPath.Any())
            {
                lock (l_RunningPath)
                {
                    while (true)
                    {
                        l_NextNode = l_RunningPath.FirstOrDefault();

                        if (l_NextNode == null)
                            break;

                        if (l_NextNode.Position.Distance3D(l_PlayerPosition) > l_DistanceCheck)
                        {
                            l_NextPosition = l_NextNode.Position;
                            break;
                        }
                        else
                            l_RunningPath.RemoveAt(0);
                    }
                }
            }
            
            return l_MoveController.ApproachPosition(l_NextPosition, 0.5f) 
                == TreeSharp.RunStatus.Failure ? TreeSharp.RunStatus.Failure : TreeSharp.RunStatus.Running;
        }
    }
}
