using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elara.WoW;

namespace Elara.AI.Controllers
{
    public class PlayerSpellController
    {
        public readonly PlayerController Owner;

        public PlayerSpellController(PlayerController p_Owner)
        {
            Owner = p_Owner;
        }

        public bool CanUseSpellById(int p_SpellId)
        {
            var l_KeyBind = GetKeyBindBySpellId(p_SpellId);

            return l_KeyBind?.IsUsableAction == true;
        }

        public bool UseSpellById(int p_SpellId)
        {
            var l_KeyBind = GetKeyBindBySpellId(p_SpellId);

            return l_KeyBind?.Press() == true;
        }

        private ActionBar.ActionBarSlot GetKeyBindBySpellId(int p_SpellId)
        {
            return Owner.Game.ActionBar.Slots.FirstOrDefault(x =>
                x.Filled && x.Type == ActionBar.ActionBarSlot.SlotFlags.Spell && x.ActionId == p_SpellId && x.CanPress);
        }
    }
}
