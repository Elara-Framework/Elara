using System;
using System.Collections.Generic;
using System.Linq;

namespace Elara.Navigation
{
    /// <summary>
    /// Navigation query node
    /// </summary>
    public class NavQueryNode
    {
        /// <summary>
        /// Distance from start
        /// </summary>
        public double DistanceFromStart = double.PositiveInfinity;
        /// <summary>
        /// Previous node
        /// </summary>
        public NavQueryNode Prev;
        /// <summary>
        /// Current node
        /// </summary>
        public NavNode Current;
        /// <summary>
        /// Is checked
        /// </summary>
        public bool Checked;
    }

    ////////////////////////////////////////////////////////

    /// <summary>
    /// Navigation query settings
    /// </summary>
    public class NavQuerySettings
    {
        /// <summary>
        /// Nodes include mask
        /// </summary>
        public NavNode.NavNodeFlags IncludeMask = NavNode.NavNodeFlags.Walkable;

        /// <summary>
        /// Walk weight
        /// </summary>
        public float WalkWeight = 1.0f;
        /// <summary>
        /// Fly weight
        /// </summary>
        public float FlyWeight = 0.3f;
        /// <summary>
        /// Swim weight
        /// </summary>
        public float SwimWeight = 3.0f;
    }
    

    ////////////////////////////////////////////////////////

    /// <summary>
    /// Navigation query class
    /// </summary>
    public class NavQuery
    {
        /// <summary>
        /// Search nodes
        /// </summary>
        private List<NavQueryNode> m_SearchNodes = new List<NavQueryNode>();
        /// <summary>
        /// Lookup table for NavNode to NavQueryNode
        /// </summary>
        private Dictionary<UInt64, NavQueryNode> m_LookupQueryNavNode = new Dictionary<UInt64, NavQueryNode>();
        /// <summary>
        /// Navigation graph
        /// </summary>
        private readonly NavGraph m_NavGraph;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_NavGraph">Navigation graph</param>
        public NavQuery(NavGraph p_NavGraph)
        {
            this.m_NavGraph = p_NavGraph;
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Search for path
        /// </summary>
        /// <param name="p_StartNode">Start node</param>
        /// <param name="p_EndNode">End node</param>
        /// <param name="p_Path">[OUT]Output path</param>
        /// <param name="p_Settings">Settings</param>
        /// <param name="p_BreakOnEndNode">Break when the end is reached</param>
        /// <returns>Success or not</returns>
        public bool SearchForPath(NavNode p_StartNode, NavNode p_EndNode, out List<NavNode> p_Path, NavQuerySettings p_Settings, bool p_BreakOnEndNode = true)
        {
            if (p_StartNode == null || (p_StartNode.Flags & p_Settings.IncludeMask) == 0 ||
                p_EndNode == null || (p_EndNode.Flags & p_Settings.IncludeMask) == 0)
            {
                p_Path = null;
                return false;
            }

            this.Initialize(p_StartNode, p_Settings.IncludeMask);

            var l_EndNode = m_LookupQueryNavNode[p_EndNode.NodeId];

            /// Init path
            p_Path = new List<NavNode>();

            /// Iterate on all search nodes
            while (this.m_SearchNodes.Count > 0)
            {
                NavQueryNode l_Nearest = this.m_SearchNodes.OrderBy<NavQueryNode, double>((Func<NavQueryNode, double>)(emp => emp.DistanceFromStart)).First<NavQueryNode>();

                this.m_SearchNodes.Remove(l_Nearest);
                l_Nearest.Checked = true;

                if (!(l_Nearest.Current.NodeId == p_EndNode.NodeId & p_BreakOnEndNode))
                {
                    foreach (NavNodeConnection l_Connection in this.m_NavGraph.GetConnections(l_Nearest.Current).Where<NavNodeConnection>((Func<NavNodeConnection, bool>)(t => !FindNode(t.ToNode)?.Checked == true)))
                    {
                        NavQueryNode l_ToNode = FindNode(l_Connection.ToNode);
                        float l_Weight = p_Settings.WalkWeight;

                        if ((l_ToNode.Current.Flags & NavNode.NavNodeFlags.Flying) != 0)
                            l_Weight = p_Settings.FlyWeight;

                        if ((l_ToNode.Current.Flags & NavNode.NavNodeFlags.Swimming) != 0)
                            l_Weight = p_Settings.SwimWeight;

                        double l_Value = l_Nearest.DistanceFromStart + (l_Weight * l_Connection.Distance);

                        if (l_Value < l_ToNode.DistanceFromStart)
                        {
                            l_ToNode.DistanceFromStart  = l_Value;
                            l_ToNode.Prev               = l_Nearest;
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            /// Inset all node from the end to the begin
            for (NavQueryNode l_Prev = l_EndNode.Prev; l_Prev != null; l_Prev = l_Prev.Prev)
                p_Path.Insert(0, l_Prev.Current);

            p_Path.Add(p_EndNode);

            return p_Path.Count > 1;
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Find a NavQueryNode
        /// </summary>
        /// <param name="p_NodeId">Node Id</param>
        /// <returns>Found node or null</returns>
        private NavQueryNode FindNode(UInt64 p_NodeId)
        {
            NavQueryNode l_Res = null;
            m_LookupQueryNavNode.TryGetValue(p_NodeId, out l_Res);
            return l_Res;
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Initialize from start node
        /// </summary>
        /// <param name="p_StartingNode">Start node</param>
        /// <param name="p_IncludeMask">Inclusion mask</param>
        private void Initialize(NavNode p_StartingNode, NavNode.NavNodeFlags p_IncludeMask)
        {
            this.m_SearchNodes.Clear();

            foreach (UInt64 l_CurrentNodeId in this.m_NavGraph.NodeConnections.Keys)
            {
                var l_NavNode = m_NavGraph.FindNode(l_CurrentNodeId);

                if (l_NavNode == null || (l_NavNode.Flags & p_IncludeMask) == 0)
                    continue;

                var l_QueryNavNode = new NavQueryNode()
                {
                    Current = l_NavNode
                };

                l_QueryNavNode.Prev     = null;
                l_QueryNavNode.Checked  = false;

                if (l_QueryNavNode.Current.NodeId == p_StartingNode.NodeId)
                {
                    l_QueryNavNode.DistanceFromStart = 0.0;
                    this.m_SearchNodes.Insert(0, l_QueryNavNode);
                }
                else
                {
                    l_QueryNavNode.DistanceFromStart = double.PositiveInfinity;
                    this.m_SearchNodes.Add(l_QueryNavNode);
                }

                m_LookupQueryNavNode.Add(l_NavNode.NodeId, l_QueryNavNode);
            }
        }
    }
}
