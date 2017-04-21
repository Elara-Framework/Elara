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
        [Serializable]
        public class MageSettings
        {
            public bool UseFrozenOrb = true;
            public bool UseBlizzard = false;
			public bool UseCoolDowns = false;
        }

		private SpellInfo RuneOfPower;
		private SpellInfo RayOfFrost;
		private SpellInfo Flurry;
		private SpellInfo IceLance;
		private SpellInfo FrozenOrb;
		private SpellInfo FrostBolt;
		private SpellInfo FrostBomb;
		private SpellInfo GlacialSpike;
		private SpellInfo Blizzard;
		private SpellInfo Ebonbolt;
		private SpellInfo IcyVeins;
		private SpellInfo MirrorImage;
		private SpellInfo WaterJet;
		private SpellInfo Blink;
		private SpellInfo Shimmer;


		private const int AURA_FINGERS_OF_FROST = 44544;
		private const int AURA_FROSTBOMB = 112948;
		private const int AURA_BRAIN_FREEZE = 190446;
		private const int AURA_RUNE_OF_POWER = 116014;
		private const int AURA_ICICLES = 205473;
		private const int AURA_INCANTERS_FLOW = 116267;

		private UI.UserControlMage m_Interface;
		public MageSettings CurrentSetting { get; private set; } = new MageSettings();

		public Mage(Elara p_Elara)
			: base(p_Elara)
		{
		}

		public override string Name => "Mage";

		public override string Author => "Elara";

		public override UserControl Interface => m_Interface;

		public override void OnLoad()
		{
			Elara.SettingsManager.OnLoadSettings += SettingsManager_OnLoadSettings;
			Elara.SettingsManager.OnSaveSettings += SettingsManager_OnSaveSettings;

			m_Interface = new UI.UserControlMage(this);
			LoadSettings(Elara.SettingsManager);

			RuneOfPower = new SpellInfo(Game, 116011);
			RayOfFrost = new SpellInfo(Game, 205021);
			IceLance = new SpellInfo(Game, 30455);
			Flurry = new SpellInfo(Game, 44614);
			FrozenOrb = new SpellInfo(Game, 84714);
			FrostBolt = new SpellInfo(Game, 116);
			FrostBomb = new SpellInfo(Game, 112948);
			GlacialSpike = new SpellInfo(Game, 199786);
			Blizzard = new SpellInfo(Game, 190356);
			Ebonbolt = new SpellInfo(Game, 214634);
			IcyVeins = new SpellInfo(Game, 12472);
			MirrorImage = new SpellInfo(Game, 55342);
			WaterJet = new SpellInfo(Game, 135029);
			Blink = new SpellInfo(Game, 1953);
			Shimmer = new SpellInfo(Game, 212653);


        }

        public override void OnUnload()
        {
            Elara.SettingsManager.OnLoadSettings -= SettingsManager_OnLoadSettings;
            Elara.SettingsManager.OnSaveSettings -= SettingsManager_OnSaveSettings;
            SaveSettings(Elara.SettingsManager);

            m_Interface?.Dispose();
            m_Interface = null;
        }

        private void SettingsManager_OnSaveSettings(SettingsManager p_SettingsManager)
        {
            SaveSettings(p_SettingsManager);
        }

        private void SettingsManager_OnLoadSettings(SettingsManager p_SettingsManager)
        {
            LoadSettings(p_SettingsManager);
        }

        private void SaveSettings(SettingsManager p_SettingsManager)
        {
            p_SettingsManager.SaveSettingXml<MageSettings>("Mage", CurrentSetting);
        }

        private void LoadSettings(SettingsManager p_SettingsManager)
        {
            var l_Settings = new MageSettings();
            p_SettingsManager.LoadSettingXml<MageSettings>("Mage", ref l_Settings);

            CurrentSetting = l_Settings;
            m_Interface?.UpdateSettings(CurrentSetting);
        }

        public override void Pull(PlayerController p_PlayerController)
        {
            Combat(p_PlayerController);
        }

        public override void Combat(PlayerController p_PlayerController)
        {
            switch (p_PlayerController.LocalPlayer.Specialization)
            {
                case WowPlayer.PlayerSpecializations.MageFrost:
                    Combat_Frost(p_PlayerController);
                    break;
                case WowPlayer.PlayerSpecializations.MageFire:
                    Combat_Fire(p_PlayerController);
                    break;
                case WowPlayer.PlayerSpecializations.MageArcane:
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
                    RuneOfPower.ChargesAvailable > 0 &&                                 // Do we have a charge
                    l_SpellController.CanUseSpell(RuneOfPower, l_Target))               // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(RuneOfPower);
                    return;
                }
		if (CurrentSetting.UseBlizzard &&                                       // Check user setting
		  l_HostilesAroundTarget.Count >= 1 &&                                // Check adds around target
		  l_TargetVisibleOnScreen &&                                          // Check that target is visible to click on
		  l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
		  l_LocalPlayer.CastingInfo == null &&                                // Not casting
		  l_SpellController.CanUseSpell(Blizzard, l_Target))                  // Use SpellController generic conditions
		{
			l_SpellController.UseSpell(Blizzard, l_Target.Position);
			return;
		}
                


		if (CurrentSetting.UseFrozenOrb &&                                      // Check user setting
                   l_LocalPlayer.IsFacingHeading(l_Target, 0.3f) &&                    // Check target facing
                   l_LocalPlayer.CastingInfo == null &&                                // Not casting
                   l_SpellController.CanUseSpell(FrozenOrb, l_Target))                 // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(FrozenOrb);
                    return;
                }


		if (CurrentSetting.UseCoolDowns &&                                       // Check user setting
					Shimmer.ChargesAvailable > 1) 
		{
					l_SpellController.UseSpell(Shimmer);
			return;
		}
              

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
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
                    l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                    l_LocalPlayer.GetAuraById(AURA_FINGERS_OF_FROST)?.Stack >= 1 &&     // Check for proc
                    l_Target.GetAuraById(AURA_FROSTBOMB) == null &&                     // Check if buff is not already applied
                    l_SpellController.CanUseSpell(FrostBomb, l_Target))                 // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(FrostBomb);
                    return;
                }

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                    l_LocalPlayer.GetAuraById(AURA_ICICLES)?.Stack >= 5 &&              // Check for proc
                    l_SpellController.CanUseSpell(GlacialSpike, l_Target))              // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(GlacialSpike);
                    return;
                }

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
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
            Elara.Logger.WriteLine("Mage", "Error : No specialization is not supported (yet) !");
        }
    }
}
