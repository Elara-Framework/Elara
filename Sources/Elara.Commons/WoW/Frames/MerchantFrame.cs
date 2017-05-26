using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.WoW.Frames
{
    public static class MerchantFrameExtensions
    {
        public static MerchantFrame GetMerchantFrame(this Game p_Game)
        {
            var l_Frame = p_Game.GetFrameByName("MerchantFrame");

            return l_Frame != null ? new MerchantFrame(l_Frame) : null;
        }
    }

    public class MerchantFrame : FrameScript.SimpleFrame
    {
        public Objects.SmartGuid CurrentMerchantNpcGuid => m_Game.MerchantInfo.CurrentMerchantNpcGuid;

        public Objects.WowUnit CurrentMerchantNpc => m_Game.MerchantInfo.CurrentMerchantNpc;

        public bool CanMerchantRepair => m_Game.GetFrameByName("MerchantRepairAllButton")?.IsVisible == true;

        internal MerchantFrame(FrameScript.SimpleFrame p_Frame)
            : base(p_Frame.GameOwner, p_Frame.Pointer) { }

        public bool RepairAll()
        {
            var l_Button = m_Game.GetFrameByName("MerchantRepairAllButton") as FrameScript.SimpleButton;

            if (l_Button != null &&
                l_Button.IsVisible &&
                l_Button.State == FrameScript.SimpleButton.ButtonState.Normal)
            {
                return l_Button.Click(System.Windows.Forms.MouseButtons.Left);
            }

            return false;
        }

        public bool Close()
        {
            var l_MerchantFrameCloseButton = m_Game.GetFrameByName("MerchantFrameCloseButton") as FrameScript.SimpleButton;
            if (l_MerchantFrameCloseButton?.IsVisible == true && l_MerchantFrameCloseButton?.State == FrameScript.SimpleButton.ButtonState.Normal)
            {
                while (l_MerchantFrameCloseButton.IsVisible && l_MerchantFrameCloseButton.State != FrameScript.SimpleButton.ButtonState.Disabled)
                {
                    if (!l_MerchantFrameCloseButton.Click(System.Windows.Forms.MouseButtons.Left))
                    {
                        m_Game.Logger.WriteLine("MerchantFrame", "Close failed ! Unable to click close button !");
                        return false;
                    }
                    System.Threading.Thread.Sleep(100);
                }
                return true;
            }

            return false;
        }
    }
}
