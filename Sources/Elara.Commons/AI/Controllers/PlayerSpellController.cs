using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Elara.WoW;
using Elara.Utils;
using Elara.WoW.Objects;

namespace Elara.AI.Controllers
{
    public class PlayerSpellController
    {
        public readonly PlayerController Owner;

        public PlayerSpellController(PlayerController p_Owner)
        {
            Owner = p_Owner;
        }

        public bool CanUseSpell(WoW.Helpers.SpellInfo p_Spell, 
            WoW.Objects.WowUnit Target = null,
            bool CheckCooldown = true,
            bool CheckGlobalCooldown = true,
            bool CheckRange = true,
            bool CheckCharges = true,
            bool CheckTargetFacing = true,
            bool CheckCasting = true)
        {
            var l_LocalPlayer = Owner.LocalPlayer;

            if (l_LocalPlayer == null)
                return false;

            if (l_LocalPlayer.CastingInfo != null)
                return false;

            if (CheckCooldown && p_Spell.IsOnCooldown)
                return false;

            if (CheckGlobalCooldown && Owner.GameOwner.SpellHistory.IsGlobalCooldownActive)
                return false;

            if (CheckCharges && p_Spell.ChargesMax > 0 && p_Spell.ChargesAvailable == 0)
                return false;

            if (Target != null && !(Target.Guid == l_LocalPlayer.Guid) && p_Spell.RequireFacingTarget &&
                !l_LocalPlayer.IsFacingHeading(Target.Position, 1.5f))
                return false;

            if (CheckRange && !p_Spell.IsInRange(Target))
                return false;

            var l_UsableAction = GetKeyBindBySpell(p_Spell)?.IsUsableAction == true;

            if (!l_UsableAction)
                return false;

            return true;
        }

        public bool UseSpell(WoW.Helpers.SpellInfo p_Spell, WowUnit p_Unit = null)
        {
            var l_LocalPlayer = Owner.LocalPlayer;

            if (l_LocalPlayer == null)
                return false;

            var l_KeyBind = GetKeyBindBySpell(p_Spell);
            var l_Bindings = Owner.GameOwner.Bindings;
            var l_CurrentTargetGuid = l_LocalPlayer.TargetGuid;
            var l_Target = p_Unit ?? l_LocalPlayer.Target;

            if (l_Target != null && 
                !(l_Target.Guid == l_CurrentTargetGuid) && 
                !p_Spell.IsSelfOnlySpell &&
                !Owner.TargetUnitWithHotkeys(l_Target))
            {
                Owner.GameOwner.Logger.WriteLine("PlayerSpellController", "UseSpell - Failed to target unit: " + l_Target.Name);
                return false;
            }

            Owner.GameOwner.Logger.WriteLine("Debug", "[PlayerSpellController] Use spell : " + p_Spell + (l_Target != null ? " on : " + l_Target.Name : string.Empty));
            var l_Result = l_KeyBind?.Press() == true;

            if (!l_Result)
            {
                Owner.GameOwner.Logger.WriteLine("PlayerSpellController", "UseSpell - Keybind press failed !");
            }

            if (!(l_LocalPlayer.TargetGuid == l_CurrentTargetGuid))
            {
                if (l_LocalPlayer.LastEnemyGuid == l_CurrentTargetGuid)
                {
                    Owner.GameOwner.Logger.WriteLine("PlayerSpellController", "UseSpell - Target previous enemy");
                    l_Bindings["TARGETPREVIOUSENEMY"]?.Press();
                }
                else if (l_LocalPlayer.LastFriendGuid == l_CurrentTargetGuid)
                {
                    Owner.GameOwner.Logger.WriteLine("PlayerSpellController", "UseSpell - Target previous friend");
                    l_Bindings["TARGETPREVIOUSFRIEND"]?.Press();
                }
            }

            return l_Result;
        }

        public bool UseSpell(WoW.Helpers.SpellInfo p_Spell, Vector3 p_Position)
        {
            var l_KeyBind = GetKeyBindBySpell(p_Spell);

            Owner.GameOwner.Logger.WriteLine("Debug", "[PlayerSpellController] Use aoe spell : " + p_Spell);

            Point l_ScreenPosition = new Point();
            if (Owner.GameOwner.WorldFrame?.ActiveCamera?.WorldToScreen(p_Position, ref l_ScreenPosition) == true &&
                l_KeyBind?.Press() == true)
            {
                using (var l_LockedCursor = Owner.GameOwner.ActiveMouseController.LockCursor(l_ScreenPosition))
                {
                    l_LockedCursor.Click(MouseButtons.Left);
                }
            }

            return false;
        }

        private ActionBar.ActionBarSlot GetKeyBindBySpell(WoW.Helpers.SpellInfo p_Spell)
        {
            return Owner.GameOwner.ActionBar.GetActiveSlots().FirstOrDefault(x =>
                x.Filled && 
                x.Type == WowActionButtonType.Spell && 
                x.SpellId == p_Spell.SpellId && 
                x.CanPress);
        }
    }
}
