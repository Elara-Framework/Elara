using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elara.WoW;
using Elara.WoW.FrameScript;
using Elara.WoW.Objects;

namespace Elara.AI.Controllers
{
    public class PlayerController : Game.ChildObject
    {
        public readonly PlayerSpellController SpellController;

        public readonly PlayerMoveController MoveController;

        public ObjectManager ObjectManager => this.GameOwner.ObjectManager;

        public WowLocalPlayer LocalPlayer => this.GameOwner.ObjectManager.LocalPlayer;

        public PlayerController(Game p_Game)
            : base(p_Game)
        {
            this.SpellController = new PlayerSpellController(this);
            this.MoveController = new PlayerMoveController(this);
        }

        private SimpleFrame GetBagContainerFrame(WowLocalPlayer.WowBag p_Bag)
        {
            if (p_Bag.ContainerType < WowLocalPlayer.WowBag.ContainerTypes.BackPack ||
                p_Bag.ContainerType > WowLocalPlayer.WowBag.ContainerTypes.EquippedBag4)
                return null;

            for (int l_I = 1; l_I <= 5; l_I++)
            {
                var l_Frame = m_Game.GetFrameByName("ContainerFrame" + l_I);

                if (l_Frame != null &&
                    l_Frame.IsVisible &&
                    l_Frame.Id == (int)p_Bag.ContainerType)
                {
                    return l_Frame;
                }
            }

            return null;
        }

        private SimpleButton GetBagContainerSlotButton(WowLocalPlayer.WowBag p_Bag, int p_SlotIndex)
        {
            var l_ContainerFrame = GetBagContainerFrame(p_Bag);

            if (l_ContainerFrame != null)
            {
                var l_SlotButton = m_Game.GetFrameByName(string.Format("{0}Item{1}", l_ContainerFrame.Name, p_Bag.MaxSlots - p_SlotIndex)) as SimpleButton;

                if (l_SlotButton != null &&
                    l_SlotButton.IsVisible)
                {
                    return l_SlotButton;
                }
            }

            return null;
        }

        public SimpleFrame OpenBagContainer(WowLocalPlayer.WowBag p_Bag)
        {
            var l_BagContainer = GetBagContainerFrame(p_Bag);

            if (l_BagContainer != null)
                return l_BagContainer;

            SimpleButton l_OpenBagButton = null;

            switch (p_Bag.ContainerType)
            {
                case WowLocalPlayer.WowBag.ContainerTypes.BackPack:
                    l_OpenBagButton = m_Game.GetFrameByName("MainMenuBarBackpackButton") as SimpleButton;
                    break;
                case WowLocalPlayer.WowBag.ContainerTypes.EquippedBag1:
                case WowLocalPlayer.WowBag.ContainerTypes.EquippedBag2:
                case WowLocalPlayer.WowBag.ContainerTypes.EquippedBag3:
                case WowLocalPlayer.WowBag.ContainerTypes.EquippedBag4:
                    l_OpenBagButton = m_Game.GetFrameByName(string.Format("CharacterBag{0}Slot", (int)p_Bag.ContainerType - 1)) as SimpleButton;
                    break;
                default:
                    return null;
            }

            if (l_OpenBagButton == null ||
                l_OpenBagButton.IsVisible == false ||
                l_OpenBagButton.State != WowSimpleButtonState.Normal)
                return null;

            if (l_OpenBagButton.Click(System.Windows.Forms.MouseButtons.Left))
                m_Game.WaitNextFrame();
            else
                return null;

            return GetBagContainerFrame(p_Bag);
        }

        public bool UseInventoryItem(WowLocalPlayer.WowBag p_Bag, int p_SlotIndex, System.Windows.Forms.MouseButtons p_MouseButton = System.Windows.Forms.MouseButtons.Right)
        {
            var l_ContainerFrame = GetBagContainerFrame(p_Bag);
            bool l_WasOpen = l_ContainerFrame?.IsVisible == true;
            bool l_Result = false;

            if (l_ContainerFrame == null)
            {
                m_Game.Logger.WriteLine("PlayerController", "UseInventoryItem - Open bag : " + p_Bag.ContainerType);
                l_ContainerFrame = OpenBagContainer(p_Bag);
            }

            if (l_ContainerFrame == null ||
                l_ContainerFrame.IsVisible == false)
            {
                m_Game.Logger.WriteLine("PlayerController", "UseInventoryItem - Unable to open bag : " + p_Bag.ContainerType);
                return false;
            }

            var l_SlotButton = GetBagContainerSlotButton(p_Bag, p_SlotIndex);

            if (l_SlotButton != null)
                l_Result = l_SlotButton.Click(System.Windows.Forms.MouseButtons.Right);

            if (!l_WasOpen && l_ContainerFrame?.IsVisible == true)
            {
                var l_CloseButton = m_Game.GetFrameByName(l_ContainerFrame.Name + "CloseButton") as SimpleButton;

                l_CloseButton?.Click(System.Windows.Forms.MouseButtons.Left);
            }

            return l_Result;
        }

        public bool UseInventoryItem(int p_ItemId, System.Windows.Forms.MouseButtons p_MouseButton = System.Windows.Forms.MouseButtons.Right)
        {
            var l_LocalPlayer = m_Game.ObjectManager.LocalPlayer;

            if (l_LocalPlayer == null)
                return false;
            
            WowLocalPlayer.WowBag l_Bag;
            WowItem l_Item;
            int l_SlotIndex;

            if (!l_LocalPlayer.GetInventoryItem(p_ItemId, out l_Bag, out l_SlotIndex, out l_Item))
                return false;

            return UseInventoryItem(l_Bag, l_SlotIndex, p_MouseButton);
        }

        private Action GetTargetingAction(WowUnit p_Unit)
        {
            var l_LocalPlayer = LocalPlayer;
            var l_KeyBinds = m_Game.Bindings;

            if (l_LocalPlayer == null || p_Unit == null)
                return null;

            if (l_LocalPlayer.TargetGuid == p_Unit.Guid)
                return delegate () { };

            var l_PartyMembers = m_Game.PartyInfo.GetActiveParty()?.Members;

            if (p_Unit.Guid == l_LocalPlayer.Guid)
            {
                return delegate () { l_KeyBinds["TARGETSELF"]?.Press(); };
            }
            else if (l_PartyMembers?.Any(x => x.Guid == p_Unit.Guid) == true)
            {
                for (int l_I = 0; l_I < l_PartyMembers.Count; l_I++)
                {
                    if (l_PartyMembers[l_I].Guid == p_Unit.Guid)
                    {
                        return delegate() { l_KeyBinds["TARGETPARTYMEMBER" + (l_I + 1)]?.Press(); };
                    }
                }
            }

            return null;
        }

        public bool CanTargetUnitWithHotkeys(WowUnit p_Unit)
        {
            return GetTargetingAction(p_Unit) != null;
        }

        public bool TargetUnitWithHotkeys(WowUnit p_Unit)
        {
            var l_LocalPlayer = LocalPlayer;

            if (l_LocalPlayer == null || p_Unit == null)
                return false;

            if (l_LocalPlayer.TargetGuid == p_Unit.Guid)
                return true;

            var l_Action = GetTargetingAction(p_Unit);

            if (l_Action == null)
                return false;

            l_Action();

            return l_LocalPlayer.TargetGuid == p_Unit.Guid;
        }
    }
}
