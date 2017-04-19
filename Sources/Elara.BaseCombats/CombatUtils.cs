using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elara.Utils;
using Elara.WoW.Objects;

namespace Elara.BaseCombats
{
    public static class CombatUtils
    {

        public static List<WowUnit> GetAttackersAroundPosition(Game p_Game, Vector3 p_Position, float p_MaxRange = 8.0f)
        {
            var l_LocalPlayer = p_Game.ObjectManager.LocalPlayer;

            if (l_LocalPlayer == null)
                return new List<WowUnit>();

            return p_Game.ObjectManager.GetObjectsOfType<WowUnit>(false).Where(x =>
                x.IsAlive &&
                x.Position.Distance3D(p_Position) <= p_MaxRange &&
                x.GetThreatSituation(l_LocalPlayer) > WowUnit.UnitThreatSituation.None).ToList();
        }

    }
}
