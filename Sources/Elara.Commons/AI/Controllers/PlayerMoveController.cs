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
            return AscentStart() && AscendStop();
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
            
            l_KeyBinds["JUMP"]?.Release();
            l_KeyBinds["TURNLEFT"]?.Release();
            l_KeyBinds["TURNRIGHT"]?.Release();
            l_KeyBinds["MOVEFORWARD"]?.Release();
            l_KeyBinds["MOVEBACKWARD"]?.Release();
            l_KeyBinds["STRAFELEFT"]?.Release();
            l_KeyBinds["STRAFERIGHT"]?.Release();
            l_KeyBinds["PITCHUP"]?.Release();
            l_KeyBinds["PITCHDOWN"]?.Release();

            if (Owner.LocalPlayer?.Movement?.Speed > 0.0f)
            {
                MovebackwardStart();
                MoveBackwardStop();
            }
        }

        public void FaceHeading(Vector3 p_Position)
        {
            var l_LocalPlayer = Owner.LocalPlayer;

            if (l_LocalPlayer != null)
            {
                var l_Directions = MathUtil.GetFacingDirections(l_LocalPlayer.Position, l_LocalPlayer.Heading, l_LocalPlayer.Pitch, p_Position);

                float l_HeadingRequired;
                if (l_Directions.TryGetValue(MathUtil.Directions.Left, out l_HeadingRequired))
                {
                    if (l_HeadingRequired > 1.5f && l_LocalPlayer.Movement.Speed > 0.0f)
                        StopMove();

                    var l_TimeOut = DateTime.Now.AddSeconds(3);
                    TurnLeftStart();
                    while (!l_LocalPlayer.IsFacingHeading(p_Position, 0.10f) && l_TimeOut > DateTime.Now)
                        Thread.Sleep(1);
                    TurnLeftStop();
                }
                else if (l_Directions.TryGetValue(MathUtil.Directions.Right, out l_HeadingRequired))
                {
                    if (l_HeadingRequired > 1.5f && l_LocalPlayer.Movement.Speed > 0.0f)
                        StopMove();

                    var l_TimeOut = DateTime.Now.AddSeconds(3);
                    TurnRightStart();
                    while (!l_LocalPlayer.IsFacingHeading(p_Position, 0.10f) && l_TimeOut > DateTime.Now)
                        Thread.Sleep(1);
                    TurnRightStop();
                }
            }
        }

        public delegate void OnStuckCallback(PlayerMoveController p_MoveController, Vector3 p_Destination);

        public TreeSharp.RunStatus ApproachPosition(Vector3 p_Position, float p_StopDistance = 1.0f, OnStuckCallback p_OnStuckCallback = null)
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

                if (!l_LocalPlayer.IsFacingHeading(p_Position, 0.10f))
                {
                    FaceHeading(p_Position);
                }

                MoveForwardStart();
                return TreeSharp.RunStatus.Running;
            }

            return TreeSharp.RunStatus.Failure;
        }
    }
}
