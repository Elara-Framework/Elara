using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elara.WoW;
using Elara.WoW.Objects;

namespace Elara.AI.Controllers
{
    public class PlayerController : Game.ChildObject
    {
        public readonly PlayerSpellController SpellController;

        public ObjectManager ObjectManager => this.GameOwner.ObjectManager;

        public WowLocalPlayer LocalPlayer => this.GameOwner.ObjectManager.LocalPlayer;

        public PlayerController(Game p_Game)
            : base(p_Game)
        {
            this.SpellController = new PlayerSpellController(this);
        }

        private Action GetTargetingAction(WowUnit p_Unit)
        {
            var l_LocalPlayer = LocalPlayer;
            var l_KeyBinds = m_Game.Bindings;

            if (l_LocalPlayer == null || p_Unit == null)
                return null;

            if (l_LocalPlayer.TargetGuid == p_Unit.Guid)
                return delegate () { };

            var l_PartyMembers = m_Game.PartyInfo.GetActiveParty()?.Members;

            if (p_Unit.Guid == l_LocalPlayer.Guid)
            {
                return delegate () { l_KeyBinds["TARGETSELF"]?.Press(); };
            }
            else if (l_PartyMembers?.Any(x => x.Guid == p_Unit.Guid) == true)
            {
                for (int l_I = 0; l_I < l_PartyMembers.Count; l_I++)
                {
                    if (l_PartyMembers[l_I].Guid == p_Unit.Guid)
                    {
                        return delegate() { l_KeyBinds["TARGETPARTYMEMBER" + (l_I + 1)]?.Press(); };
                    }
                }
            }

            return null;
        }

        public bool CanTargetUnitWithHotkeys(WowUnit p_Unit)
        {
            return GetTargetingAction(p_Unit) != null;
        }

        public bool TargetUnitWithHotkeys(WowUnit p_Unit)
        {
            var l_LocalPlayer = LocalPlayer;

            if (l_LocalPlayer == null || p_Unit == null)
                return false;

            var l_Action = GetTargetingAction(p_Unit);

            if (l_Action == null)
                return false;

            l_Action();

            return l_LocalPlayer.TargetGuid == p_Unit.Guid;
        }
    }
}
