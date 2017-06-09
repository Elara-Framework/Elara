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
                x.GetThreatSituation(l_LocalPlayer) > WoW.WowUnitThreatSituation.None).ToList();
        }

        public static WowUnit GetPartyMemberWithLowestHealth(Game p_Game, bool p_IncludeSelf = true)
        {
            var l_Units = new List<WowUnit>();
            var l_LocalPlayer = p_Game.ObjectManager.LocalPlayer;
            var l_PartyMembers = p_Game.PartyInfo.GetActiveParty()?.PartyMembers;

            if (p_IncludeSelf && l_LocalPlayer?.IsAlive == true)
            {
                l_Units.Add(l_LocalPlayer);
            }

            if (l_PartyMembers != null)
            {
                l_Units.AddRange(l_PartyMembers.Where(x => x.Unit?.IsAlive == true).Select(x => x.Unit));
            }

            return l_Units.OrderBy(x => x.HealthPercent).FirstOrDefault();
        }

    }
}
