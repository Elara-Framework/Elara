using System;

namespace Elara.Navigation
{
    /// <summary>
    /// Navigation node connection
    /// </summary>
    public class NavNodeConnection
    {
        /// <summary>
        /// Size in file
        /// </summary>
        public const int SIZE = 8 + 8 + 4;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Source node
        /// </summary>
        public UInt64 FromNode { get; set; }
        /// <summary>
        /// Destination node
        /// </summary>
        public UInt64 ToNode { get; set; }
        /// <summary>
        /// Distance between the 2 nodes
        /// </summary>
        public float Distance { get; set; }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Constructor
        /// </summary>
        public NavNodeConnection()
        {
            FromNode    = 0;
            ToNode      = 0;
            Distance    = 0.0f;
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Get from node owner tile id
        /// </summary>
        /// <returns></returns>
        public int GetFromNodeTileId()
        {
            return (int)((FromNode >> 32) & 0xFFFFFFFF);
        }
        /// <summary>
        /// Get to node owner tile id
        /// </summary>
        /// <returns></returns>
        public int GetToNodeTileId()
        {
            return (int)((ToNode >> 32) & 0xFFFFFFFF);
        }
    }
}
