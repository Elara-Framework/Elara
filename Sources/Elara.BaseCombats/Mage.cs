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
        }

        private SpellInfo RuneOfPower;
        private SpellInfo RayOfFrost;
        private SpellInfo Flurry;
        private SpellInfo IceLance;
        private SpellInfo FrozenOrb;
        private SpellInfo FrostBolt;

        private const int AURA_FINGERS_OF_FROST = 44544;
        private const int AURA_BRAIN_FREEZE = 190446;

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

            m_Interface     = new UI.UserControlMage(this);
            LoadSettings(Elara.SettingsManager);

            RuneOfPower     = new SpellInfo(Game, 116011);
            RayOfFrost      = new SpellInfo(Game, 205021);
            IceLance        = new SpellInfo(Game, 30455);
            Flurry          = new SpellInfo(Game, 44614);
            FrozenOrb       = new SpellInfo(Game, 84714);
            FrostBolt       = new SpellInfo(Game, 116);
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
            var l_SpellController = p_PlayerController.SpellController;
            var l_LocalPlayer = p_PlayerController.LocalPlayer;
            var l_Target = l_LocalPlayer?.Target;

            if (l_LocalPlayer != null & l_Target != null && l_Target.IsAlive)
            {
                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    RuneOfPower.ChargesAvailable > 0 &&                                 // Do we have a charge
                    l_SpellController.CanUseSpell(RuneOfPower, l_Target))               // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(RuneOfPower);
                    return;
                }

                /* TODO : Detect if we face the target */
                if (CurrentSetting.UseFrozenOrb &&                                     // Check user setting
                    l_LocalPlayer.CastingInfo == null &&                               // Not casting
                    l_SpellController.CanUseSpell(FrozenOrb, l_Target))                // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(FrozenOrb);
                    return;
                }

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_SpellController.CanUseSpell(RayOfFrost, l_Target))                // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(RayOfFrost);
                    return;
                }

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.GetAuraById(AURA_BRAIN_FREEZE) != null &&             // Check for proc
                    l_SpellController.CanUseSpell(Flurry, l_Target))                    // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(Flurry);
                    return;
                }

                if (l_LocalPlayer.CastingInfo == null &&                                // Not casting
                    l_LocalPlayer.GetAuraById(AURA_FINGERS_OF_FROST) != null &&         // Check for proc
                    l_SpellController.CanUseSpell(IceLance, l_Target))                  // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(IceLance);
                    return;
                }

                if (l_LocalPlayer.CastingInfo == null &&                                 // Not casting
                    l_SpellController.CanUseSpell(FrostBolt, l_Target))                  // Use SpellController generic conditions
                {
                    l_SpellController.UseSpell(FrostBolt);
                    return;
                }
            }
        }
    }
}
