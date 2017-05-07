using Elara.Utils;
using System;

namespace Elara.Navigation
{
    /// <summary>
    /// Navigation node
    /// </summary>
    public class NavNode
    {
        /// <summary>
        /// File size
        /// </summary>
        public const int SIZE = 4 + (3 * 4) + 4;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Node flags
        /// </summary>
        [Flags]
        public enum NavNodeFlags : int
        {
            None            = 0x0,
            Walkable        = 0x1,
            Swimming        = 0x2,
            Flying          = 0x4,
            Transport       = 0x8
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Node Id
        /// </summary>
        public UInt64 NodeId { get; set; } = 0;
        /// <summary>
        /// Position
        /// </summary>
        public Vector3 Position = Vector3.Zero;
        /// <summary>
        /// Flags
        /// </summary>
        public NavNodeFlags Flags { get; set; } = NavNodeFlags.Walkable;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Get low node id for storage
        /// </summary>
        /// <returns>Low node id</returns>
        public int GetNodeIdLow()
        {
            return (int)(NodeId & 0xFFFFFFFF);
        }
        /// <summary>
        /// Get hight node id
        /// </summary>
        /// <returns>Hight node id</returns>
        public int GetNodeIdHight()
        {
            return (int)((NodeId >> 32) & 0xFFFFFFFF);
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Is this node equal to a another one
        /// </summary>
        /// <param name="p_Other">Other node</param>
        /// <returns>Result</returns>
        public override bool Equals(object p_Other)
        {
            NavNode l_OtherNode = p_Other as NavNode;

            return l_OtherNode != null && l_OtherNode.Position == this.Position && l_OtherNode.Flags == this.Flags;
        }
        /// <summary>
        /// Get object hash code
        /// </summary>
        /// <returns>This object hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int l_Hash = 13;
                l_Hash = (l_Hash * 7) + Position.X.GetHashCode();
                l_Hash = (l_Hash * 7) + Position.Y.GetHashCode();
                l_Hash = (l_Hash * 7) + Position.Z.GetHashCode();
                l_Hash = (l_Hash * 7) + Flags.GetHashCode();
                l_Hash = (l_Hash * 7) + NodeId.GetHashCode();
                return l_Hash;
            }
        }
        /// <summary>
        /// To string operator override
        /// </summary>
        /// <returns>This node serialized</returns>
        public override string ToString()
        {
            return String.Format("Position: {0}, Flags: {1}", Position, Flags);
        }
        
    }
}
