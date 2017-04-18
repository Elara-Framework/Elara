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

        private readonly DB.SpellRecord SpellRecord;
        private readonly DB.SpellCategoriesRecord SpellCategoriesRecord;
        private readonly DB.SpellMiscRecord SpellMiscRecord;
        private readonly DB.SpellRangeRecord SpellRangeRecord;
        private readonly DB.SpellCategoryRecord SpellChargeCategory;
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
        }

        public override string ToString() => string.Format("[{0}] {1}", SpellId, Name);

        public string Name => SpellRecord?.Name ?? string.Empty;

        public bool IsOnCooldown => GameOwner.SpellHistory.IsSpellOnCooldown(SpellId);

        public bool IsOnActionBar =>
            GameOwner.ActionBar.Slots.Any(x => x.Type == ActionBar.ActionBarSlot.SlotFlags.Spell && x.ActionId == SpellId);

        public bool CheckSpellAttribute(SpellMiscRecord.SpellAttributes p_Attribute)
        {
            return SpellMiscRecord?.CheckSpellAttribute(p_Attribute, GameOwner.ObjectManager.LocalPlayer?.InstanceDifficultyId ?? 0) ?? false;
        }

        public bool RequireTargetFacingCaster => CheckSpellAttribute(SpellMiscRecord.SpellAttributes.REQ_TARGET_FACING_CASTER);

        public bool RequireCasterBehingTarget => CheckSpellAttribute(SpellMiscRecord.SpellAttributes.REQ_CASTER_BEHIND_TARGET);

        public bool IsTalent => CheckSpellAttribute(SpellMiscRecord.SpellAttributes.IS_TALENT);

        public bool IsCharge => CheckSpellAttribute(SpellMiscRecord.SpellAttributes.CHARGE);

        public bool DontBreakStealth => CheckSpellAttribute(SpellMiscRecord.SpellAttributes.DONT_BREAK_STEALTH);

        public float MinRangeFriendly => SpellRangeRecord?.MinRangeFriendly ?? INVALID_RANGE;

        public float MinRangeHostile => SpellRangeRecord?.MinRangeHostile ?? INVALID_RANGE;

        public float MaxRangeFriendly => SpellRangeRecord?.MaxRangeFriendly ?? INVALID_RANGE;

        public float MaxRangeHostile => SpellRangeRecord?.MaxRangeHostile ?? INVALID_RANGE;

        public int ChargesConsumed => GameOwner.SpellHistory.GetConsumedCharges(SpellId);

        public int ChargesMax => SpellChargeCategory?.MaxCharges ?? 0;

        public int ChargesAvailable => ChargesMax - ChargesConsumed;
    }
}
