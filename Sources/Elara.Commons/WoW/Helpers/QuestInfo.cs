using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.WoW.Helpers
{
    public class QuestInfo : Game.ChildObject
    {
        private static Dictionary<int, QuestCacheEntry> m_QuestCacheEntries = new Dictionary<int, QuestCacheEntry>();

        private QuestCacheEntry m_QuestCacheEntry = null;

        public QuestCacheEntry QuestCacheEntry => m_QuestCacheEntry;
        public int QuestId { get; private set; }

        public QuestInfo(Game p_Game, int p_QuestId)
            : base(p_Game)
        {
            lock (m_QuestCacheEntries)
            {
                if (!m_QuestCacheEntries.TryGetValue(p_QuestId, out m_QuestCacheEntry))
                    if (p_Game.DBCache.Quest.Update().TryGetValue(p_QuestId, out m_QuestCacheEntry) && m_QuestCacheEntry != null)
                        m_QuestCacheEntries.Add(p_QuestId, m_QuestCacheEntry);
            }

            QuestId = p_QuestId;
        }

        public virtual bool IsComplete
        {
            get
            {
                foreach (var l_Objective in Objectives)
                    if (!IsObjectiveComplete(l_Objective))
                        return false;

                return true;
            }
        }

        public virtual bool IsObjectiveComplete(QuestObjectiveInfo p_Objective)
        {
            var l_LocalPlayer = m_Game.ObjectManager.LocalPlayer;

            if (l_LocalPlayer == null)
                return false;

            var l_Quest = m_Game.QuestLog.Quests.FirstOrDefault(x => x.QuestId == this.QuestId);

            if (l_Quest == null)
                return false;

            switch (p_Objective.ObjectiveType)
            {
                default:
                    break;
            }

            if (p_Objective.QuestLogObjectiveIndex >= 0 && p_Objective.QuestLogObjectiveIndex < l_Quest.ObjectivesCounter.Length)
            {
                return l_Quest.ObjectivesCounter[p_Objective.QuestLogObjectiveIndex] >= p_Objective.RequiredCount;
            }

            return false;
        }

        public List<QuestObjectiveInfo> Objectives => m_QuestCacheEntry?.Objectives ?? new List<QuestObjectiveInfo>();

        public string Name => m_QuestCacheEntry?.Name ?? string.Empty;

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}", QuestId, Name);
        }

    }
}
