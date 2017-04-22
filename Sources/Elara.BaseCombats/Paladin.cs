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
    public class Paladin : Extensions.CombatScript
    {
        [Serializable]
        public class PaladinSettings
        {
            
        }

        private SpellInfo Judgment;
        private SpellInfo DivineStorm;
        private SpellInfo TemplarsVerdict;
        private SpellInfo BladeOfJustice;
        private SpellInfo CrusaderStrike;

        private UI.UserControlPaladin m_Interface;
        public PaladinSettings CurrentSetting { get; private set; } = new PaladinSettings();

        public Paladin(Elara p_Elara)
            : base(p_Elara)
        {
        }

        public override string Name => "Paladin";

        public override string Author => "Elara";

        public override UserControl Interface => m_Interface;

        public override void OnLoad()
        {
            Elara.SettingsManager.OnLoadSettings += SettingsManager_OnLoadSettings;
            Elara.SettingsManager.OnSaveSettings += SettingsManager_OnSaveSettings;

            m_Interface = new UI.UserControlPaladin(this);
            LoadSettings(Elara.SettingsManager);

            Judgment = new SpellInfo(Game, 20271);
            DivineStorm = new SpellInfo(Game, 53385);
            TemplarsVerdict = new SpellInfo(Game, 85256);
            BladeOfJustice = new SpellInfo(Game, 184575);
            CrusaderStrike = new SpellInfo(Game, 35395);
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
            p_SettingsManager.SaveSettingXml<PaladinSettings>("Paladin", CurrentSetting);
        }

        private void LoadSettings(SettingsManager p_SettingsManager)
        {
            var l_Settings = new PaladinSettings();
            p_SettingsManager.LoadSettingXml<PaladinSettings>("Paladin", ref l_Settings);

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
                case WowPlayer.PlayerSpecializations.PaladinHoly:
                    Combat_Holy(p_PlayerController);
                    break;
                case WowPlayer.PlayerSpecializations.PaladinProtection:
                    Combat_Protection(p_PlayerController);
                    break;
                case WowPlayer.PlayerSpecializations.PaladinRetribution:
                    Combat_Retribution(p_PlayerController);
                    break;
                default:
                    Combat_NoSpec(p_PlayerController);
                    break;
            }
        }

        private void Combat_Holy(PlayerController p_PlayerController)
        {
            Elara.Logger.WriteLine("Paladin", "Error : Holy specialization is not supported (yet) !");
        }

        private void Combat_Protection(PlayerController p_PlayerController)
        {
            Elara.Logger.WriteLine("Paladin", "Error : Protection specialization is not supported (yet) !");
        }

        private void Combat_Retribution(PlayerController p_PlayerController)
        {
            var l_SpellController = p_PlayerController.SpellController;
            var l_LocalPlayer = p_PlayerController.LocalPlayer;
            var l_Target = l_LocalPlayer?.Target;

            if (l_LocalPlayer != null & l_Target != null && l_Target.IsAlive)
            {
                var l_TargetScreenPosition = new Point();
                var l_TargetVisibleOnScreen = this.Game.WorldFrame?.ActiveCamera?.WorldToScreen(l_Target.Position, ref l_TargetScreenPosition) == true;
                var l_HostilesAroundTarget = CombatUtils.GetAttackersAroundPosition(this.Game, l_Target.Position, p_MaxRange: 8.0f);
                var l_HostilesAroundPlayer = CombatUtils.GetAttackersAroundPosition(this.Game, l_LocalPlayer.Position, p_MaxRange: 8.0f);

                /* AOE */
                if (l_HostilesAroundPlayer.Count >= 2)
                {
                    if (l_LocalPlayer.CastingInfo == null &&                            // Not casting
                        l_LocalPlayer.GetPower(WowUnit.UnitPower.HolyPower) >= 3 &&     // Check holy power
                        l_SpellController.CanUseSpell(DivineStorm))                     // Use SpellController generic conditions
                    {
                        l_SpellController.UseSpell(DivineStorm);
                        return;
                    }
                }
                /* Single Target */
                else
                {
                    if (l_LocalPlayer.CastingInfo == null &&                            // Not casting
                        l_LocalPlayer.GetPower(WowUnit.UnitPower.HolyPower) >= 3 &&     // Check holy power
                        l_SpellController.CanUseSpell(TemplarsVerdict, l_Target))       // Use SpellController generic conditions
                    {
                        l_SpellController.UseSpell(TemplarsVerdict);
                        return;
                    }
                }

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                    l_SpellController.CanUseSpell(Judgment, l_Target))                  // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(Judgment);
                    return;
                }

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                    l_LocalPlayer.GetPower(WowUnit.UnitPower.HolyPower) <= 3 &&         // Check holy power
                    l_SpellController.CanUseSpell(BladeOfJustice, l_Target))            // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(BladeOfJustice);
                    return;
                }

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.IsFacingHeading(l_Target, 1.5f) &&                    // Check target facing
                    l_SpellController.CanUseSpell(CrusaderStrike, l_Target))            // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(CrusaderStrike);
                    return;
                }
            }
        }

        private void Combat_NoSpec(PlayerController p_PlayerController)
        {
            Elara.Logger.WriteLine("Paladin", "Error : No specialization is not supported (yet) !");
        }
    }
}
