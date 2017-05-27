using Elara.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elara.AI.Controllers
{
    public class PlayerMoveController
    {
        public readonly PlayerController Owner;

        public PlayerMoveController(PlayerController p_Owner)
        {
            Owner = p_Owner;
        }

        public bool AscentStart()
        {
            return Owner.GameOwner.Bindings["JUMP"]?.Hold() == true;
        }

        public bool AscendStop()
        {
            return Owner.GameOwner.Bindings["JUMP"]?.Release() == true;
        }

        public bool Jump()
        {
            var l_Result = AscentStart();

            if (!l_Result)
                return false;

            System.Threading.Thread.Sleep(100);

            return AscendStop();
        }

        public bool TurnLeftStart()
        {
            return Owner.GameOwner.Bindings["TURNLEFT"]?.Hold() == true;
        }

        public bool TurnLeftStop()
        {
            return Owner.GameOwner.Bindings["TURNLEFT"]?.Release() == true;
        }

        public bool TurnRightStart()
        {
            return Owner.GameOwner.Bindings["TURNRIGHT"]?.Hold() == true;
        }

        public bool TurnRightStop()
        {
            return Owner.GameOwner.Bindings["TURNRIGHT"]?.Release() == true;
        }

        public bool MoveForwardStart()
        {
            return Owner.GameOwner.Bindings["MOVEFORWARD"]?.Hold() == true;
        }

        public bool MoveForwardStop()
        {
            return Owner.GameOwner.Bindings["MOVEFORWARD"]?.Release() == true;
        }

        public bool MovebackwardStart()
        {
            return Owner.GameOwner.Bindings["MOVEBACKWARD"]?.Hold() == true;
        }

        public bool MoveBackwardStop()
        {
            return Owner.GameOwner.Bindings["MOVEBACKWARD"]?.Release() == true;
        }

        public bool StrafeLeftStart()
        {
            return Owner.GameOwner.Bindings["STRAFELEFT"]?.Hold() == true;
        }

        public bool StrafeLeftStop()
        {
            return Owner.GameOwner.Bindings["STRAFELEFT"]?.Release() == true;
        }

        public bool StrafeRightStart()
        {
            return Owner.GameOwner.Bindings["STRAFERIGHT"]?.Hold() == true;
        }

        public bool StrafeRightStop()
        {
            return Owner.GameOwner.Bindings["STRAFERIGHT"]?.Release() == true;
        }

        public bool PitchUpStart()
        {
            return Owner.GameOwner.Bindings["PITCHUP"]?.Hold() == true;
        }

        public bool PitchUpStop()
        {
            return Owner.GameOwner.Bindings["PITCHUP"]?.Release() == true;
        }

        public bool PitchDownStart()
        {
            return Owner.GameOwner.Bindings["PITCHDOWN"]?.Hold() == true;
        }

        public bool PitchDownStop()
        {
            return Owner.GameOwner.Bindings["PITCHDOWN"]?.Release() == true;
        }

        public void StopMove()
        {
            var l_KeyBinds = Owner.GameOwner.Bindings;

            var l_LocalPlayer = Owner.ObjectManager.LocalPlayer;
            
            l_KeyBinds["JUMP"]?.Release();
            l_KeyBinds["TURNLEFT"]?.Release();
            l_KeyBinds["TURNRIGHT"]?.Release();
            l_KeyBinds["MOVEFORWARD"]?.Release();
            l_KeyBinds["MOVEBACKWARD"]?.Release();
            l_KeyBinds["STRAFELEFT"]?.Release();
            l_KeyBinds["STRAFERIGHT"]?.Release();
            l_KeyBinds["PITCHUP"]?.Release();
            l_KeyBinds["PITCHDOWN"]?.Release();

            if (l_LocalPlayer != null)
            {
                var l_MovementFlags = l_LocalPlayer.Movement.Flags;

                if ((l_MovementFlags & WoW.WowMovementFlags.MOVEMENTFLAG_BACKWARD) != 0)
                {
                    MovebackwardStart();
                    MoveBackwardStop();
                }

                if ((l_MovementFlags & WoW.WowMovementFlags.MOVEMENTFLAG_FORWARD) != 0)
                {
                    MoveForwardStart();
                    MoveForwardStop();
                }
            }
        }

        public void Face(Vector3 p_Position, float p_Tolerance = 0.10f)
        {
            var l_LocalPlayer = Owner.LocalPlayer;

            if (l_LocalPlayer == null)
                return;

            bool l_LeftPressed = false;
            bool l_RightPressed = false;
            bool l_PitchUpPressed = false;
            bool l_PitchDownPressed = false;

            var l_TimeOut = DateTime.Now.AddSeconds(3);
            while (DateTime.Now < l_TimeOut)
            {
                bool l_HandlePitch = l_LocalPlayer.IsSwimming || l_LocalPlayer.IsFlying;
                var l_Directions = MathUtil.GetFacingDirections(l_LocalPlayer.Position, l_LocalPlayer.Heading, l_LocalPlayer.Pitch, p_Position);
                bool l_FacingCorrectly = true;

                float l_LeftAngle = 0.0f;
                float l_RightAngle = 0.0f;
                float l_PitchUpAngle = 0.0f;
                float l_PitchDownAngle = 0.0f;

                l_Directions.TryGetValue(MathUtil.Directions.Left, out l_LeftAngle);
                l_Directions.TryGetValue(MathUtil.Directions.Right, out l_RightAngle);
                l_Directions.TryGetValue(MathUtil.Directions.PitchUp, out l_PitchUpAngle);
                l_Directions.TryGetValue(MathUtil.Directions.PitchDown, out l_PitchDownAngle);

                if ((l_LocalPlayer.Movement.Flags & WoW.WowMovementFlags.MOVEMENTFLAG_FORWARD) != 0 &&
                    l_LocalPlayer.Position.Distance3D(p_Position) <= 3.0f ||
                    (l_LeftAngle > 1.0f || l_RightAngle > 1.0f) ||
                    (l_HandlePitch && (l_PitchDownAngle > 1.0f || l_PitchUpAngle > 1.0f)))
                {
                    MoveForwardStop();
                }

                if (l_LeftAngle > p_Tolerance)
                {
                    TurnLeftStart();
                    l_LeftPressed = true;
                    l_FacingCorrectly = false;
                }
                else if (l_LeftPressed)
                {
                    TurnLeftStop();
                    l_LeftPressed = false;
                }

                if (l_RightAngle > p_Tolerance)
                {
                    TurnRightStart();
                    l_RightPressed = true;
                    l_FacingCorrectly = false;
                }
                else if (l_LeftPressed)
                {
                    TurnRightStop();
                    l_RightPressed = false;
                }

                if (l_HandlePitch)
                {
                    if (l_PitchUpAngle > p_Tolerance)
                    {
                        PitchUpStart();
                        l_PitchUpPressed = true;
                        l_FacingCorrectly = false;
                    }
                    else if (l_PitchUpPressed)
                    {
                        PitchUpStop();
                        l_PitchUpPressed = false;
                    }

                    if (l_PitchDownAngle > p_Tolerance)
                    {
                        PitchDownStart();
                        l_PitchDownPressed = true;
                        l_FacingCorrectly = false;
                    }
                    else if (l_PitchDownPressed)
                    {
                        PitchDownStop();
                        l_PitchDownPressed = false;
                    }
                }

                if (l_FacingCorrectly)
                    break;

                Thread.Sleep(1);
            }

            if (l_LeftPressed)      TurnLeftStop();
            if (l_RightPressed)     TurnRightStop();
            if (l_PitchUpPressed)   PitchUpStop();
            if (l_PitchDownPressed) PitchDownStop();
        }

        public delegate void OnStuckCallback(PlayerMoveController p_MoveController, Vector3 p_Destination);

        public TreeSharp.RunStatus ApproachPosition(Vector3 p_Position, float p_StopDistance = 1.5f, OnStuckCallback p_OnStuckCallback = null)
        {
            var l_LocalPlayer = Owner.LocalPlayer;

            if (l_LocalPlayer != null)
            {
                var l_Distance = p_Position.Distance3D(l_LocalPlayer.Position);
                if (l_Distance < p_StopDistance)
                {
                    StopMove();
                    return TreeSharp.RunStatus.Success;
                }

                var l_HeightDiff = p_Position.Z - l_LocalPlayer.Position.Z;

                // Take of
                if (l_LocalPlayer.CanFly &&
                    l_HeightDiff > 2.0f &&
                    p_Position.Distance2D(l_LocalPlayer.Position) < 1.5f)
                {
                    StopMove();
                    Jump();
                }

                bool l_HandlePitch = l_LocalPlayer.IsSwimming || l_LocalPlayer.IsFlying;
                if (!l_LocalPlayer.IsFacingHeading(p_Position, 0.15f) || (l_HandlePitch && !l_LocalPlayer.IsFacingPitch(p_Position, 0.15f)))
                    Face(p_Position, 0.05f);

                MoveForwardStart();
                return TreeSharp.RunStatus.Running;
            }

            return TreeSharp.RunStatus.Failure;
        }
    }
}
