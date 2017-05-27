using Elara.WoW.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.WoW.Helpers
{
    public class ItemInfo
    {
        private readonly ItemSparseRecord ItemSparseRecord;
        public readonly Game GameOwner;
        public readonly int ItemId;

        public ItemInfo(Objects.WowItem p_Item)
            : this(p_Item.GameOwner, p_Item.ItemId)
        {

        }

        public ItemInfo(Game p_Game, int p_ItemId)
        {
            this.GameOwner = p_Game;
            this.ItemId = p_ItemId;
            this.ItemSparseRecord = p_Game.Database.ItemSparse.GetRecord(p_ItemId);
        }

        public string Name => ItemSparseRecord?.Name ?? string.Empty;

        public WowItemQuality Quality => ItemSparseRecord?.Rarity ?? WowItemQuality.ITEM_QUALITY_POOR;

        public int BeginQuestId => ItemSparseRecord?.QuestId ?? 0;

        public int BuyPrice => ItemSparseRecord?.BuyPrice ?? 0;

        public int SellPrice =>  NoVendorValue ? 0 : ItemSparseRecord?.SellPrice ?? 0;

        public WowInventoryType EquipSlot => ItemSparseRecord != null ? (WowInventoryType)ItemSparseRecord.EquipSlot : WowInventoryType.INVTYPE_NON_EQUIP;

        public int RequiredLevel => ItemSparseRecord?.RequiredLevel ?? 0;

        public int RequiredSpellId => ItemSparseRecord?.RequiredSpell ?? 0;

        public int RequiredSkillId => ItemSparseRecord?.RequiredSkill ?? 0;

        public int RequiredSkillLevel => ItemSparseRecord?.RequiredSkillLevel ?? 0;

        public bool IsConjured => (ItemSparseRecord?.Flags & WowItemFlags.ITEM_FLAG_CONJURED) != 0;

        public bool IsOpenable => (ItemSparseRecord?.Flags & WowItemFlags.ITEM_FLAG_HAS_LOOT) != 0;

        public bool HasQuestGlow => (ItemSparseRecord?.Flags & WowItemFlags.ITEM_FLAG_HAS_QUEST_GLOW) != 0;

        public bool IsBoundToAccount => (ItemSparseRecord?.Flags & WowItemFlags.ITEM_FLAG_IS_BOUND_TO_ACCOUNT) != 0;

        public bool IsMillable => (ItemSparseRecord?.Flags & WowItemFlags.ITEM_FLAG_IS_MILLABLE) != 0;

        public bool IsProspectable => (ItemSparseRecord?.Flags & WowItemFlags.ITEM_FLAG_IS_PROSPECTABLE) != 0;

        public bool IsNotUseableInArena => (ItemSparseRecord?.Flags & WowItemFlags.ITEM_FLAG_NOT_USEABLE_IN_ARENA) != 0;

        public bool IsUniqueEquippable => (ItemSparseRecord?.Flags & WowItemFlags.ITEM_FLAG_UNIQUE_EQUIPPABLE) != 0;

        public bool IsNoDurabilityLoss => (ItemSparseRecord?.Flags & WowItemFlags.ITEM_FLAG_NO_DURABILITY_LOSS) != 0;

        public bool IsHordeItem => (ItemSparseRecord?.Flags2 & WowItemFlags2.ITEM_FLAG2_FACTION_HORDE) != 0;

        public bool IsAllianceItem => (ItemSparseRecord?.Flags2 & WowItemFlags2.ITEM_FLAG2_FACTION_ALLIANCE) != 0;

        public bool IsCasterItem => (ItemSparseRecord?.Flags2 & WowItemFlags2.ITEM_FLAG2_CLASSIFY_AS_CASTER) != 0;

        public bool IsPhysicalItem => (ItemSparseRecord?.Flags2 & WowItemFlags2.ITEM_FLAG2_CLASSIFY_AS_PHYSICAL) != 0;

        public bool IsCasterWeapon => (ItemSparseRecord?.Flags2 & WowItemFlags2.ITEM_FLAG2_CASTER_WEAPON) != 0;

        public bool NoDurability => (ItemSparseRecord?.Flags2 & WowItemFlags2.ITEM_FLAG2_NO_DURABILITY) != 0;

        public bool ConfirmBeforeUse => (ItemSparseRecord?.Flags2 & WowItemFlags2.ITEM_FLAG2_CONFIRM_BEFORE_USE) != 0;

        public bool NoVendorValue => (ItemSparseRecord?.Flags2 & WowItemFlags2.ITEM_FLAG2_NO_VENDOR_VALUE) != 0;

        public bool IsForDamageRole => (ItemSparseRecord?.Flags2 & WowItemFlags2.ITEM_FLAG2_ROLE_DAMAGE) != 0;

        public bool IsForHealerRole => (ItemSparseRecord?.Flags2 & WowItemFlags2.ITEM_FLAG2_ROLE_HEALER) != 0;

        public bool IsForTankRole => (ItemSparseRecord?.Flags2 & WowItemFlags2.ITEM_FLAG2_ROLE_TANK) != 0;

        public override string ToString() => string.Format("[{0}] {1}", ItemId, Name);

    }
}
