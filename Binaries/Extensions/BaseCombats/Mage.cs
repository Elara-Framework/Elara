using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Elara.AI.Controllers;
using Elara.WoW;
using Elara.WoW.Objects;
using Elara.TreeSharp;

namespace Elara.BaseCombats
{
    public class Mage : Extensions.CombatScript
    {
        
        private WoW.Helpers.SpellInfo RuneOfPower;
        private WoW.Helpers.SpellInfo RayOfFrost;
        private WoW.Helpers.SpellInfo Flurry;
        private WoW.Helpers.SpellInfo IceLance;
        private WoW.Helpers.SpellInfo FrozenOrb;
        private WoW.Helpers.SpellInfo FrostBolt;
        private WoW.Helpers.SpellInfo FrostBomb;
        private WoW.Helpers.SpellInfo GlacialSpike;
        private WoW.Helpers.SpellInfo Blizzard;

        private const int AURA_FINGERS_OF_FROST         = 44544;
        private const int AURA_FROSTBOMB                = 112948;
        private const int AURA_BRAIN_FREEZE             = 190446;
        private const int AURA_RUNE_OF_POWER            = 116014;
        private const int AURA_ICICLES                  = 205473;

        public Mage(Elara p_Elara)
            : base(p_Elara)
        {
            RuneOfPower = new WoW.Helpers.SpellInfo(Game, 116011);
            RayOfFrost = new WoW.Helpers.SpellInfo(Game, 205021);
            IceLance = new WoW.Helpers.SpellInfo(Game, 30455);
            Flurry = new WoW.Helpers.SpellInfo(Game, 44614);
            FrozenOrb = new WoW.Helpers.SpellInfo(Game, 84714);
            FrostBolt = new WoW.Helpers.SpellInfo(Game, 116);
            FrostBomb = new WoW.Helpers.SpellInfo(Game, 112948);
            GlacialSpike = new WoW.Helpers.SpellInfo(Game, 199786);
            Blizzard = new WoW.Helpers.SpellInfo(Game, 190356);
        }
        
        public override string Name => "Mage";

        public override float CombatRange => 28.0f;

        public override void Pulse(PlayerController p_PlayerController)
        {

        }

        public override void Combat(PlayerController p_PlayerController)
        {
            switch (p_PlayerController.LocalPlayer.Specialization)
            {
                case WowTalentSpecialization.TALENT_SPEC_MAGE_FROST:
                    Combat_Frost(p_PlayerController);
                    break;
                case WowTalentSpecialization.TALENT_SPEC_MAGE_FIRE:
                    Combat_Fire(p_PlayerController);
                    break;
                case WowTalentSpecialization.TALENT_SPEC_MAGE_ARCANE:
                    Combat_Arcane(p_PlayerController);
                    break;
                default:
                    Combat_NoSpec(p_PlayerController);
                    break;
            }
        }

        private void Combat_Frost(PlayerController p_PlayerController)
        {
            var l_SpellController = p_PlayerController.SpellController;
            var l_LocalPlayer = p_PlayerController.LocalPlayer;
            var l_Target = l_LocalPlayer?.Target;

            if (l_LocalPlayer != null & l_Target != null && l_Target.IsAlive)
            {
                var l_TargetScreenPosition = new Point();
                var l_TargetVisibleOnScreen = this.Game.WorldFrame?.ActiveCamera?.WorldToScreen(l_Target.Position, ref l_TargetScreenPosition) == true;
                var l_HostilesAroundTarget = CombatUtils.GetAttackersAroundPosition(this.Game, l_Target.Position, p_MaxRange: 8.0f);

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.IsMoving == false &&                                  // Not moving
                    RuneOfPower.ChargesAvailable > 0 &&                                 // Do we have a charge
                    l_SpellController.CanUseSpell(RuneOfPower, l_Target))               // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(RuneOfPower);
                    return;
                }
                
                if (l_LocalPlayer.IsMoving == false &&                                  // Not moving
                    l_LocalPlayer.IsFacingHeading(l_Target, 0.3f) &&                    // Check target facing
                    l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_SpellController.CanUseSpell(FrozenOrb, l_Target))                 // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(FrozenOrb);
                    return;
                }

                if (//CurrentSetting.UseBlizzard &&                                     // Check user setting
                    l_LocalPlayer.IsMoving == false &&                                  // Not moving
                    l_HostilesAroundTarget.Count >= 2 &&                                // Check adds around target
                    l_TargetVisibleOnScreen &&                                          // Check that target is visible to click on
                    l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                    l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_SpellController.CanUseSpell(Blizzard, l_Target))                  // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(Blizzard, l_Target.Position);
                    return;
                }

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.IsMoving == false &&                                  // Not moving
                    l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                    l_LocalPlayer.GetAuraById(AURA_RUNE_OF_POWER) == null &&            // Check that we don't have Rune of Power buff
                    l_SpellController.CanUseSpell(RayOfFrost, l_Target))                // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(RayOfFrost);
                    return;
                }

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                    l_LocalPlayer.GetAuraById(AURA_BRAIN_FREEZE) != null &&             // Check for proc
                    l_SpellController.CanUseSpell(Flurry, l_Target))                    // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(Flurry);
                    return;
                }

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                    l_LocalPlayer.GetAuraById(AURA_FINGERS_OF_FROST)?.Stack >= 2 &&     // Check for proc
                    l_SpellController.CanUseSpell(IceLance, l_Target))                  // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(IceLance);
                    return;
                }

                /* TODO : Check adds around target */
                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.IsMoving == false &&                                  // Not moving
                    l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                    l_LocalPlayer.GetAuraById(AURA_FINGERS_OF_FROST)?.Stack >= 1 &&     // Check for proc
                    l_Target.GetAuraById(AURA_FROSTBOMB) == null &&                     // Check if buff is not already applied
                    l_SpellController.CanUseSpell(FrostBomb, l_Target))                 // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(FrostBomb);
                    return;
                }
                

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.IsMoving == false &&                                  // Not moving
                    l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                    l_LocalPlayer.GetAuraById(AURA_ICICLES)?.Stack >= 5 &&              // Check for proc
                    l_SpellController.CanUseSpell(GlacialSpike, l_Target))              // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(GlacialSpike);
                    return;
                }

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.IsMoving == false &&                                  // Not moving
                    l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                    l_SpellController.CanUseSpell(FrostBolt, l_Target))                 // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(FrostBolt);
                    return;
                }
            }
        }

        private void Combat_Fire(PlayerController p_PlayerController)
        {
            Elara.Logger.WriteLine("Mage", "Error : Fire specialization is not supported (yet) !");
        }

        private void Combat_Arcane(PlayerController p_PlayerController)
        {
            Elara.Logger.WriteLine("Mage", "Error : Arcane specialization is not supported (yet) !");
        }

        private void Combat_NoSpec(PlayerController p_PlayerController)
        {
            var l_SpellController = p_PlayerController.SpellController;
            var l_LocalPlayer = p_PlayerController.LocalPlayer;
            var l_Target = l_LocalPlayer?.Target;


            if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                l_LocalPlayer.IsMoving == false &&                                  // Not moving
                l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                l_SpellController.CanUseSpell(FrostBolt, l_Target))                 // Use SpellController generic conditions
            {
                l_SpellController.UseSpell(FrostBolt);
                return;
            }
        }
    }
}
