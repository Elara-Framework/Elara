using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elara.WoW.DB;

namespace Elara.WoW
{
    public class SpellInfo
    {
        public const float INVALID_RANGE = float.MaxValue;

        private readonly SpellRecord SpellRecord;
        private readonly SpellCategoriesRecord SpellCategoriesRecord;
        private readonly SpellMiscRecord SpellMiscRecord;
        private readonly SpellRangeRecord SpellRangeRecord;
        private readonly SpellCategoryRecord SpellChargeCategory;
        private readonly List<SpellCastingRequirementsRecord> SpellCastingRequirements = new List<SpellCastingRequirementsRecord>();
        public readonly Game GameOwner;
        public readonly int SpellId;

        public SpellInfo(Game p_Game, int p_SpellId)
        {
            this.GameOwner = p_Game;
            this.SpellId = p_SpellId;
            this.SpellRecord = p_Game.Database.Spell.GetRecord(p_SpellId);
            this.SpellCategoriesRecord = p_Game.Database.SpellCategories.GetRecordBySpellId(p_SpellId);
            this.SpellChargeCategory = this.SpellCategoriesRecord?.ChargeCategory;
            this.SpellMiscRecord = this.SpellRecord?.SpellMisc;
            this.SpellRangeRecord = this.SpellMiscRecord?.SpellRange;
            this.SpellCastingRequirements = p_Game.Database.SpellCastingRequirements.Records.Values.Where(x => x.SpellId == p_SpellId).ToList();
        }

        public override string ToString() => string.Format("[{0}] {1}", SpellId, Name);

        public string Name => SpellRecord?.Name ?? string.Empty;

        public TimeSpan CooldownRemaining
        {
            get
            {
                var l_Cooldowns = GameOwner.SpellHistory.GetCooldowns();
                var l_CategoryId = this.SpellCategoriesRecord?.CategoryId ?? 0;

                foreach (var l_Cooldown in l_Cooldowns)
                {
                    if (l_Cooldown.SpellId == this.SpellId || (l_CategoryId != 0 && l_Cooldown.CategoryId == l_CategoryId))
                    {
                        if (l_Cooldown.TimeRemaing > TimeSpan.Zero)
                            return l_Cooldown.TimeRemaing;
                        else if (l_Cooldown.CategoryRemaining > TimeSpan.Zero)
                            return l_Cooldown.CategoryRemaining;
                        else if (l_Cooldown.GcdRemaining > TimeSpan.Zero)
                            return l_Cooldown.GcdRemaining;
                    }
                }

                return TimeSpan.Zero;
            }
        }

        public bool IsOnCooldown => CooldownRemaining > TimeSpan.Zero;

        public bool IsKnown => GameOwner.SpellBook.IsSpellKnown(SpellId);

        public bool IsOnActionBar =>
            GameOwner.ActionBar.GetActiveSlots().Any(x => x.Type == ActionBar.ActionBarSlot.SlotFlags.Spell && x.SpellId == SpellId);

        public bool CheckSpellAttribute(SpellMiscRecord.SpellAttributes p_Attribute)
        {
            return SpellMiscRecord?.CheckSpellAttribute(p_Attribute, GameOwner.ObjectManager.LocalPlayer?.InstanceDifficultyId ?? 0) ?? false;
        }

        public bool IsInRange(Objects.WowUnit p_Target)
        {
            if (!HasRange)
                return true;

            var l_LocalPlayer = GameOwner.ObjectManager.LocalPlayer;

            if (l_LocalPlayer == null)
                return false;

            if (p_Target == null || p_Target.Guid == l_LocalPlayer.Guid)
                return true;

            return p_Target.CombatDistanceTo(l_LocalPlayer) <= MaxRangeHostile;
        }

        public bool RequireTargetFacingCaster => CheckSpellAttribute(SpellMiscRecord.SpellAttributes.REQ_TARGET_FACING_CASTER);

        public bool RequireCasterBehingTarget => CheckSpellAttribute(SpellMiscRecord.SpellAttributes.REQ_CASTER_BEHIND_TARGET);

        public bool RequireFacingTarget => SpellCastingRequirements.Any(x => x.FacingCasterFlags != 0);

        public bool IsTalent => CheckSpellAttribute(SpellMiscRecord.SpellAttributes.IS_TALENT);

        public bool DontBreakStealth => CheckSpellAttribute(SpellMiscRecord.SpellAttributes.DONT_BREAK_STEALTH);

        public bool IsSelfOnlySpell => SpellMiscRecord?.SpellRangeId == 1;

        public bool IsMeleeSpell => SpellMiscRecord?.SpellRangeId == 2;

        public bool HasRange => SpellRangeRecord != null && !IsMeleeSpell && !IsSelfOnlySpell;

        public float MinRangeFriendly => SpellRangeRecord?.MinRangeFriendly ?? INVALID_RANGE;

        public float MinRangeHostile => SpellRangeRecord?.MinRangeHostile ?? INVALID_RANGE;

        public float MaxRangeFriendly => SpellRangeRecord?.MaxRangeFriendly ?? INVALID_RANGE;

        public float MaxRangeHostile => SpellRangeRecord?.MaxRangeHostile ?? INVALID_RANGE;

        public int ChargesConsumed => GameOwner.SpellHistory.GetConsumedCharges(SpellId);

        public int ChargesMax => SpellChargeCategory?.MaxCharges ?? 0;

        public int ChargesAvailable => ChargesMax - ChargesConsumed;
    }
}
