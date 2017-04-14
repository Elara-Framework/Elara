using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.AI.Controllers
{
    public class PlayerController
    {
        public readonly Game Game;
        public readonly PlayerSpellController SpellController;

        public PlayerController(Game p_Game)
        {
            this.Game = p_Game;
            this.SpellController = new PlayerSpellController(this);
        }


    }
}
