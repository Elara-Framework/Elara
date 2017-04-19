using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Elara.WoW;
using Elara.Utils;

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
            bool CheckRange = true)
        {
            var l_Target = Target ?? Owner.LocalPlayer?.Target;

            if (CheckCooldown && p_Spell.IsOnCooldown)
                return false;

            if (CheckGlobalCooldown && Owner.GameOwner.SpellHistory.IsGlobalCooldownActive)
                return false;

            if (CheckRange && l_Target != null)
            {
                // TODO : Detect hostile / friendly target
                double l_TargetRange = l_Target.DistanceTo(Owner.LocalPlayer);
                float l_MaxRange = Math.Max(p_Spell.MaxRangeFriendly, p_Spell.MaxRangeHostile);
                float l_MinRange = Math.Min(p_Spell.MinRangeFriendly, p_Spell.MinRangeHostile);

                if (l_TargetRange > l_MaxRange || l_TargetRange < l_MinRange)
                    return false;
            }

            return GetKeyBindBySpellId(p_Spell.SpellId)?.IsUsableAction == true;
        }

        public bool UseSpell(SpellInfo p_Spell)
        {
            var l_KeyBind = GetKeyBindBySpellId(p_Spell.SpellId);

            Owner.GameOwner.Logger.WriteLine("Debug", "[PlayerSpellController] Use spell : " + p_Spell);

            return l_KeyBind?.Press() == true;
        }

        public bool UseSpell(SpellInfo p_Spell, Vector3 p_Position)
        {
            var l_KeyBind = GetKeyBindBySpellId(p_Spell.SpellId);

            Owner.GameOwner.Logger.WriteLine("Debug", "[PlayerSpellController] Use aoe spell : " + p_Spell);

            Point l_ScreenPosition = new Point();
            if (Owner.GameOwner.WorldFrame?.ActiveCamera?.WorldToScreen(p_Position, ref l_ScreenPosition) == true &&
                l_KeyBind?.Press() == true)
            {
                using (var l_LockedCursor = Owner.GameOwner.ActiveMouseController.LockCursor(l_ScreenPosition))
                {
                    l_LockedCursor.Click(System.Windows.Forms.MouseButtons.Left);
                }
            }

            return false;
        }

        private ActionBar.ActionBarSlot GetKeyBindBySpellId(int p_SpellId)
        {
            var l_OverridenSpell = Owner.GameOwner.SpellBook.GetOverridenSpell(p_SpellId);

            return Owner.GameOwner.ActionBar.GetActiveSlots().FirstOrDefault(x =>
                x.Filled && 
                x.Type == ActionBar.ActionBarSlot.SlotFlags.Spell && 
                (x.ActionId == p_SpellId || l_OverridenSpell?.BaseSpellId == x.ActionId) && 
                x.CanPress);
        }
    }
}
