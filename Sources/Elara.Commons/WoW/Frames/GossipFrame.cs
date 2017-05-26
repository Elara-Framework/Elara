using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.WoW.Frames
{
    public static class GossipFrameExtensions
    {
        public static GossipFrame GetGossipFrame(this Game p_Game)
        {
            var l_Frame = p_Game.GetFrameByName("GossipFrame");

            return l_Frame != null ? new GossipFrame(l_Frame) : null;
        }
    }

    public class GossipFrame : FrameScript.SimpleFrame
    {
        public List<GossipInfo.GossipQuest> Quests => m_Game.GossipInfo.Quests;

        public List<GossipInfo.GossipOption> Options => m_Game.GossipInfo.Options;

        internal GossipFrame(FrameScript.SimpleFrame p_Frame)
            : base(p_Frame.GameOwner, p_Frame.Pointer) { }

        public bool SelectQuestById(int p_QuestId)
        {
            var l_Quest = Quests.FirstOrDefault(x => x.QuestId == p_QuestId);

            if (l_Quest == null)
                return false;

            for (int l_I = 1; l_I <= 40; l_I++)
            {
                var l_Frame = m_Game.GetFrameByName("GossipTitleButton" + l_I) as FrameScript.SimpleButton;

                if (l_Frame != null &&
                    l_Frame.IsVisible == true &&
                    l_Frame.Text.Contains(l_Quest.Name) == true)
                {
                    return l_Frame.Click(System.Windows.Forms.MouseButtons.Left);
                }
            }

            return false;
        }

        public bool SelectOptionByType(GossipInfo.GossipOption.GossipOptionTypes p_Type)
        {
            var l_Option = Options.FirstOrDefault(x => x.Type == p_Type);

            if (l_Option == null)
                return false;

            for (int l_I = 1; l_I <= 40; l_I++)
            {
                var l_Frame = m_Game.GetFrameByName("GossipTitleButton" + l_I) as FrameScript.SimpleButton;

                if (l_Frame != null &&
                    l_Frame.IsVisible == true &&
                    l_Frame.Text.Contains(l_Option.Text) == true)
                {
                    return l_Frame.Click(System.Windows.Forms.MouseButtons.Left);
                }
            }

            return false;
        }
    }
}
