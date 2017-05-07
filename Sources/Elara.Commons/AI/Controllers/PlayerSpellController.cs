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

        public bool CanUseSpell(SpellInfo p_Spell, 
            WoW.Objects.WowUnit Target = null,
            bool CheckCooldown = true,
            bool CheckGlobalCooldown = true,
            bool CheckRange = true,
            bool CheckCharges = true)
        {
            if (CheckCooldown && p_Spell.IsOnCooldown)
                return false;

            if (CheckGlobalCooldown && Owner.GameOwner.SpellHistory.IsGlobalCooldownActive)
                return false;

            if (CheckCharges && p_Spell.ChargesMax > 0 && p_Spell.ChargesAvailable == 0)
                return false;

            if (CheckRange && Target != null && p_Spell.HasRange)
            {
                // TODO : Detect hostile / friendly target
                double l_TargetRange = Target.DistanceTo(Owner.LocalPlayer);
                float l_MaxRange = Math.Max(p_Spell.MaxRangeFriendly, p_Spell.MaxRangeHostile);
                float l_MinRange = Math.Min(p_Spell.MinRangeFriendly, p_Spell.MinRangeHostile);

                if (l_TargetRange > l_MaxRange || l_TargetRange < l_MinRange)
                    return false;
            }

            return GetKeyBindBySpell(p_Spell)?.IsUsableAction == true;
        }

        public bool UseSpell(SpellInfo p_Spell, WowUnit p_Unit = null)
        {
            var l_KeyBind = GetKeyBindBySpell(p_Spell);
            var l_Bindings = Owner.GameOwner.Bindings;
            var l_CurrentTargetGuid = Owner.LocalPlayer.TargetGuid;

            Owner.TargetUnitWithHotkeys(p_Unit);

            Owner.GameOwner.Logger.WriteLine("Debug", "[PlayerSpellController] Use spell : " + p_Spell);

            var l_Result = l_KeyBind?.Press() == true;

            if (!(Owner.LocalPlayer.TargetGuid == l_CurrentTargetGuid))
            {
                if (Owner.LocalPlayer.LastEnemyGuid == l_CurrentTargetGuid)
                {
                    l_Bindings["TARGETPREVIOUSENEMY"]?.Press();
                }
                else if (Owner.LocalPlayer.LastFriendGuid == l_CurrentTargetGuid)
                {
                    l_Bindings["TARGETPREVIOUSFRIEND"]?.Press();
                }
            }

            return l_Result;
        }

        public bool UseSpell(SpellInfo p_Spell, Vector3 p_Position)
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

        private ActionBar.ActionBarSlot GetKeyBindBySpell(SpellInfo p_Spell)
        {
            return Owner.GameOwner.ActionBar.GetActiveSlots().FirstOrDefault(x =>
                x.Filled && 
                x.Type == ActionBar.ActionBarSlot.SlotFlags.Spell && 
                x.SpellId == p_Spell.SpellId && 
                x.CanPress);
        }
    }
}
