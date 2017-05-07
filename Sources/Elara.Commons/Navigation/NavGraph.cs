using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace Elara.Navigation
{
    /// <summary>
    /// Navigation graph
    /// </summary>
    public class NavGraph
    {
        /// <summary>
        /// Navigation world instance
        /// </summary>
        public NavWorld World;
        /// <summary>
        /// Connections cache
        /// </summary>
        public Dictionary<UInt64, ConcurrentBag<NavNodeConnection>> NodeConnections = new Dictionary<UInt64, ConcurrentBag<NavNodeConnection>>();

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_World">Navigation world instance</param>
        public NavGraph(NavWorld p_World)
        {
            World = p_World;
        }
        
        ////////////////////////////////////////////////////////

        /// <summary>
        /// Register a node connection
        /// </summary>
        /// <param name="p_Connection">Connection to register</param>
        public void RegisterConnection(NavNodeConnection p_Connection)
        {
            if (!NodeConnections.ContainsKey(p_Connection.FromNode))
                NodeConnections.Add(p_Connection.FromNode, new ConcurrentBag<NavNodeConnection>());

            if (!NodeConnections[p_Connection.FromNode].Contains(p_Connection))
                NodeConnections[p_Connection.FromNode].Add(p_Connection);
        }
        /// <summary>
        /// Get all connections for a specific node
        /// </summary>
        /// <param name="p_Node">Lookup node</param>
        /// <returns>All node connections</returns>
        public ConcurrentBag<NavNodeConnection> GetConnections(NavNode p_Node)
        {
            if (this.NodeConnections.ContainsKey(p_Node.NodeId))
                return this.NodeConnections[p_Node.NodeId];

            return new ConcurrentBag<NavNodeConnection>();
        }
        /// <summary>
        /// Is a connection between two node bi-directional
        /// </summary>
        /// <param name="p_Left">Left node</param>
        /// <param name="p_Right">Right node</param>
        /// <returns>True or false</returns>
        public bool IsBiDirectionalConnection(NavNode p_Left, NavNode p_Right)
        {
            return this.NodeConnections.ContainsKey(p_Left.NodeId) && 
                this.NodeConnections.ContainsKey(p_Right.NodeId) && 
                (this.NodeConnections[p_Left.NodeId].Any<NavNodeConnection>((Func<NavNodeConnection, bool>)(t => t.ToNode == p_Right.NodeId)) && 
                this.NodeConnections[p_Right.NodeId].Any<NavNodeConnection>((Func<NavNodeConnection, bool>)(t => t.ToNode == p_Left.NodeId)));
        }
        /// <summary>
        /// Find a node by is full Id
        /// </summary>
        /// <param name="p_NodeID">Full node id</param>
        /// <returns>Found node or null</returns>
        public NavNode FindNode(UInt64 p_NodeID)
        {
            return World.GetNode(p_NodeID);
        }
    }
}
