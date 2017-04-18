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
    }
}
