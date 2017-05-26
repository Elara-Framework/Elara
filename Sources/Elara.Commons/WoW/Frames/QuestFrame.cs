using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.WoW.Frames
{
    public static class QuestFrameExtensions
    {
        public static QuestFrame GetQuestFrame(this Game p_Game)
        {
            var l_Frame = p_Game.GetFrameByName("QuestFrame");

            return l_Frame != null ? new QuestFrame(l_Frame) : null;
        }
    }

    public class QuestFrame : FrameScript.SimpleFrame
    {
        public int CurrentQuestId => m_Game.QuestInfo.CurrentQuestId;

        public Objects.SmartGuid CurrentQuestNpcGuid => m_Game.QuestInfo.CurrentQuestNpcGuid;

        public Objects.WowUnit CurrentQuestNpc => m_Game.QuestInfo.CurrentQuestNpc;

        internal QuestFrame(FrameScript.SimpleFrame p_Frame) 
            : base(p_Frame.GameOwner, p_Frame.Pointer) { }

        public bool Close()
        {
            var l_QuestFrameCloseButton = m_Game.GetFrameByName("QuestFrameCloseButton") as FrameScript.SimpleButton;
            if (l_QuestFrameCloseButton?.IsVisible == true && l_QuestFrameCloseButton?.State == FrameScript.SimpleButton.ButtonState.Normal)
            {
                while (l_QuestFrameCloseButton.IsVisible && l_QuestFrameCloseButton.State != FrameScript.SimpleButton.ButtonState.Disabled)
                {
                    if (!l_QuestFrameCloseButton.Click(System.Windows.Forms.MouseButtons.Left))
                    {
                        m_Game.Logger.WriteLine("QuestFrame", "Close failed ! Unable to click close button !");
                        return false;
                    }
                    System.Threading.Thread.Sleep(100);
                }
                return true;
            }

            return DeclineQuest();
        }

        public bool CompleteQuest(int p_RewardItemId = 0)
        {
            var l_QuestFrameCompleteButton = m_Game.GetFrameByName("QuestFrameCompleteButton") as FrameScript.SimpleButton;
            if (l_QuestFrameCompleteButton?.IsVisible == true && l_QuestFrameCompleteButton?.State == FrameScript.SimpleButton.ButtonState.Normal)
            {
                while (l_QuestFrameCompleteButton.IsVisible && l_QuestFrameCompleteButton.State != FrameScript.SimpleButton.ButtonState.Disabled)
                {
                    if (!l_QuestFrameCompleteButton.Click(System.Windows.Forms.MouseButtons.Left))
                    {
                        m_Game.Logger.WriteLine("QuestFrame", "CompleteQuest failed ! Unable to click continue button !");
                        return false;
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }

            var l_QuestFrameGoodbyeButton = m_Game.GetFrameByName("QuestFrameGoodbyeButton") as FrameScript.SimpleButton;
            if (l_QuestFrameGoodbyeButton?.IsVisible == true && l_QuestFrameGoodbyeButton?.State == FrameScript.SimpleButton.ButtonState.Normal)
            {
                while (l_QuestFrameGoodbyeButton.IsVisible && l_QuestFrameGoodbyeButton.State != FrameScript.SimpleButton.ButtonState.Disabled)
                {
                    if (!l_QuestFrameGoodbyeButton.Click(System.Windows.Forms.MouseButtons.Left))
                    {
                        m_Game.Logger.WriteLine("QuestFrame", "CompleteQuest failed ! Unable to click cancel button !");
                        return false;
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }

            var l_QuestFrameCompleteQuestButton = m_Game.GetFrameByName("QuestFrameCompleteQuestButton") as FrameScript.SimpleButton;
            if (l_QuestFrameCompleteQuestButton?.IsVisible == true && l_QuestFrameCompleteQuestButton?.State == FrameScript.SimpleButton.ButtonState.Normal)
            {
                while (l_QuestFrameCompleteQuestButton.IsVisible && l_QuestFrameCompleteQuestButton.State != FrameScript.SimpleButton.ButtonState.Disabled)
                {
                    if (!l_QuestFrameCompleteQuestButton.Click(System.Windows.Forms.MouseButtons.Left))
                    {
                        m_Game.Logger.WriteLine("QuestFrame", "CompleteQuest failed ! Unable to click complete button !");
                        return false;
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }

            return true;
        }

        public bool AcceptQuest()
        {
            var l_QuestFrameAcceptButton = m_Game.GetFrameByName("QuestFrameAcceptButton") as FrameScript.SimpleButton;
            if (l_QuestFrameAcceptButton?.IsVisible == true && l_QuestFrameAcceptButton?.State == FrameScript.SimpleButton.ButtonState.Normal)
            {
                while (l_QuestFrameAcceptButton.IsVisible && l_QuestFrameAcceptButton.State != FrameScript.SimpleButton.ButtonState.Disabled)
                {
                    if (!l_QuestFrameAcceptButton.Click(System.Windows.Forms.MouseButtons.Left))
                    {
                        m_Game.Logger.WriteLine("QuestFrame", "CompleteQuest failed ! Unable to click accept button !");
                        return false;
                    }
                    System.Threading.Thread.Sleep(100);
                }

                return true;
            }

            return false;
        }

        public bool DeclineQuest()
        {
            var l_QuestFrameDeclineButton = m_Game.GetFrameByName("QuestFrameDeclineButton") as FrameScript.SimpleButton;
            if (l_QuestFrameDeclineButton?.IsVisible == true && l_QuestFrameDeclineButton?.State == FrameScript.SimpleButton.ButtonState.Normal)
            {
                while (l_QuestFrameDeclineButton.IsVisible && l_QuestFrameDeclineButton.State != FrameScript.SimpleButton.ButtonState.Disabled)
                {
                    if (!l_QuestFrameDeclineButton.Click(System.Windows.Forms.MouseButtons.Left))
                    {
                        m_Game.Logger.WriteLine("QuestFrame", "CompleteQuest failed ! Unable to click decline button !");
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
