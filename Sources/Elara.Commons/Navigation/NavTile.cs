using Elara.WoW.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Elara.Navigation
{
    /// <summary>
    /// Navigation tile
    /// </summary>
    public class NavTile
    {
        /// <summary>
        /// File version
        /// </summary>
        public const int NAV_TILE_VERSION = 8;
        /// <summary>
        /// Minimum distance between two nodes
        /// </summary>
        public const double MIN_NODE_DISTANCE = 3;
        /// <summary>
        /// Maximum connection distance
        /// </summary>
        public const double CONNECTION_DIST = 6;
        /// <summary>
        /// Max slope angle
        /// </summary>
        public const float MAX_SLOPE_ANGLE = 0;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// File header
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct NavTileHeader
        {
            public int Version;
            public int TileX, TileY;
            public int NodeIdIncrement;
            public int NodesCount;
            public int NodesConnectionsCount;
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// World instance
        /// </summary>
        public NavWorld World;
        /// <summary>
        /// Full tile file name
        /// </summary>
        public string TilePath;
        /// <summary>
        /// Packed tile id (cache)
        /// </summary>
        public int TileId;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Tile header
        /// </summary>
        public NavTileHeader Header;
        /// <summary>
        /// Nodes
        /// </summary>
        public Dictionary<UInt64, NavNode> Nodes { get; private set; } = new Dictionary<UInt64, NavNode>();
        /// <summary>
        /// Connections
        /// </summary>
        public List<NavNodeConnection> Connections { get; private set; } = new List<NavNodeConnection>();
        /// <summary>
        /// Need to be saved
        /// </summary>
        private bool m_Dirty = false;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Is main tile for navigation
        /// </summary>
        private bool m_IsMain = false;
        /// <summary>
        /// Draw buffer need to be rebuilt
        /// </summary>
        private bool m_GraphicDirty = false;
        /// <summary>
        /// Boxes vertices
        /// </summary>
        List<List<Game.Overlay.Renderer.ColoredVertex>> m_Boxes = new List<List<Game.Overlay.Renderer.ColoredVertex>>();
        /// <summary>
        /// Connections vertices
        /// </summary>
        List<Game.Overlay.Renderer.ColoredVertex> m_ConnectionsLineVertices = null;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_World">NavWorld instance</param>
        /// <param name="p_TileFileName">Tile file name</param>
        /// <param name="p_TileX">Tile grid X</param>
        /// <param name="p_TileY">Tile grid Y</param>
        public NavTile(NavWorld p_World, string p_TileFileName, int p_TileX, int p_TileY, bool p_IsMain)
        {
            World       = p_World;
            TilePath    = p_TileFileName;
            TileId      = World.BuildTileId(p_TileX, p_TileY);

            m_IsMain = p_IsMain;

            bool l_NeedInit = false;

            /// Check if we have an existing tile
            if (File.Exists(p_TileFileName))
            {
                var l_FileInfo = new FileInfo(p_TileFileName);

                /// Check if we can read the header
                if (l_FileInfo.Length >= Utils.MarshalCache<NavTileHeader>.Size)
                {
                    bool l_NeedMove = false;
                    using (var l_StreamReader = new BinaryReader(new FileStream(p_TileFileName, FileMode.Open)))
                    {
                        if (l_StreamReader.BaseStream.CanRead)
                        {
                            /// Read tile header
                            Header = new NavTileHeader();
                            Header.Version                  = l_StreamReader.ReadInt32();
                            Header.TileX                    = l_StreamReader.ReadInt32();
                            Header.TileY                    = l_StreamReader.ReadInt32();
                            Header.NodeIdIncrement          = l_StreamReader.ReadInt32();
                            Header.NodesCount               = l_StreamReader.ReadInt32();
                            Header.NodesConnectionsCount    = l_StreamReader.ReadInt32();

                            /// Compute file size
                            long l_ComputedSize = Utils.MarshalCache<NavTileHeader>.Size;
                            l_ComputedSize += Header.NodesCount * NavNode.SIZE;
                            l_ComputedSize += Header.NodesConnectionsCount * NavNodeConnection.SIZE;

                            /// Check if the file is valid
                            if (Header.Version == NAV_TILE_VERSION &&
                                l_ComputedSize == l_FileInfo.Length &&
                                ((Header.TileX >= 0) && (Header.TileX < 64)) &&
                                ((Header.TileY >= 0) && (Header.TileY < 64)))
                            {
                                /// Read all nodes
                                for (int l_I = 0; l_I < Header.NodesCount; l_I++)
                                {
                                    var l_Node = new NavNode();
                                    l_Node.NodeId = BuildNodeId(l_StreamReader.ReadInt32());
                                    l_Node.Position = new Utils.Vector3(
                                        l_StreamReader.ReadSingle(),
                                        l_StreamReader.ReadSingle(),
                                        l_StreamReader.ReadSingle());
                                    l_Node.Flags = (NavNode.NavNodeFlags)l_StreamReader.ReadInt32();

                                    Nodes.Add(l_Node.NodeId, l_Node);
                                }

                                /// Read all nodes connections
                                for (int l_I = 0; l_I < Header.NodesConnectionsCount; l_I++)
                                {
                                    var l_Connection = new NavNodeConnection();
                                    l_Connection.FromNode   = (UInt64)l_StreamReader.ReadInt64();
                                    l_Connection.ToNode     = (UInt64)l_StreamReader.ReadInt64();
                                    l_Connection.Distance   = l_StreamReader.ReadSingle();

                                    Connections.Add(l_Connection);

                                    if (m_IsMain)
                                        World.GetNavGraph()?.RegisterConnection(l_Connection);
                                }

                                SetDirty(true);
                            }
                            else
                            {
                                l_NeedMove = true;
                                l_NeedInit = true;
                            }
                        }
                        else
                        {
                            l_NeedMove = true;
                            l_NeedInit = true;
                        }
                    }

                    if (l_NeedMove)
                        MoveOldTile();
                }
                else
                {
                    l_NeedInit = true;
                }
            }
            else
            {
                l_NeedInit = true;
            }

            /// Check if we need to init a new tile
            if (l_NeedInit)
            {
                Header = new NavTileHeader();
                Header.Version                  = NAV_TILE_VERSION;
                Header.TileX                    = p_TileX;
                Header.TileY                    = p_TileY;
                Header.NodeIdIncrement          = 0;
                Header.NodesCount               = 0;
                Header.NodesConnectionsCount    = 0;

                /// Drop old invalid? file if exist
                if (File.Exists(TilePath))
                    File.Delete(TilePath);
            }
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Save the tile
        /// </summary>
        public bool Save()
        {
            if (!m_Dirty)
                return false;

            if (File.Exists(TilePath))
                File.Delete(TilePath);

            lock (Nodes)
            {
                if (Nodes.Count == 0)
                {
                    m_Dirty = false;
                    return false;
                }

                lock (Connections)
                {
                    using (var l_StreamWriter = new BinaryWriter(new FileStream(TilePath, FileMode.Create)))
                    {
                        l_StreamWriter.Write((int)Header.Version);
                        l_StreamWriter.Write((int)Header.TileX);
                        l_StreamWriter.Write((int)Header.TileY);
                        l_StreamWriter.Write((int)Header.NodeIdIncrement);
                        l_StreamWriter.Write((int)Header.NodesCount);
                        l_StreamWriter.Write((int)Header.NodesConnectionsCount);

                        foreach (var l_Node in Nodes.Values)
                        {
                            l_StreamWriter.Write((int)l_Node.GetNodeIdLow());
                            l_StreamWriter.Write((float)l_Node.Position.X);
                            l_StreamWriter.Write((float)l_Node.Position.Y);
                            l_StreamWriter.Write((float)l_Node.Position.Z);
                            l_StreamWriter.Write((int)l_Node.Flags);
                        }

                        foreach (var l_Connection in Connections)
                        {
                            l_StreamWriter.Write((UInt64)l_Connection.FromNode);
                            l_StreamWriter.Write((UInt64)l_Connection.ToNode);
                            l_StreamWriter.Write((float)l_Connection.Distance);
                        }

                        l_StreamWriter.Flush();
                    }
                }
            }

            m_Dirty = false;

            return true;
        }
        /// <summary>
        /// Set this tile as dirty, need to be saved
        /// </summary>
        /// <param name="p_OnlyGraphics">Only graphics</param>
        public void SetDirty(bool p_OnlyGraphics = false)
        {
            m_GraphicDirty = true;

            if (p_OnlyGraphics)
                return;

            m_Dirty = true;
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Generate a new node Id
        /// </summary>
        /// <returns>The generated node Id</returns>
        private UInt64 GenerateNodeId()
        {
            /// Increment counter
            Header.NodeIdIncrement++;

            UInt64 l_HighPart   = (UInt32)TileId;
            UInt32 l_LowPart    = (UInt32)(Header.NodeIdIncrement);

            return (UInt64)((l_HighPart << 32) | l_LowPart);
        }
        /// <summary>
        /// Build full node id from low node id part
        /// </summary>
        /// <param name="p_Increment">Low node id part</param>
        /// <returns>The full node Id</returns>
        public UInt64 BuildNodeId(int p_Increment)
        {
            UInt64 l_HighPart   = (UInt32)TileId;
            UInt32 l_LowPart    = (UInt32)p_Increment;

            return (UInt64)((l_HighPart << 32) | l_LowPart);
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Add a new node from unit instance
        /// </summary>
        /// <param name="p_Unit">Unit instance</param>
        public void AddNodeFromUnit(WowUnit p_Unit)
        {
            bool l_ShouldAdd = false;

            lock (Nodes)
            {
                /// Check if we should add the node
                if (!Nodes.Any(x => x.Value.Position.Distance3D(p_Unit.Position) < MIN_NODE_DISTANCE))
                    l_ShouldAdd = true;
            }

            if (l_ShouldAdd)
            {
                var l_Node = new NavNode();

                l_Node.NodeId   = GenerateNodeId();
                l_Node.Position = p_Unit.Position;
                l_Node.Flags    = NavNode.NavNodeFlags.Walkable;

                if (p_Unit.IsFlying)
                    l_Node.Flags = NavNode.NavNodeFlags.Flying;

                if (p_Unit.IsSwimming)
                    l_Node.Flags = NavNode.NavNodeFlags.Swimming;

                lock (Nodes)
                {
                    Nodes.Add(l_Node.NodeId, l_Node);

                    Header.NodesCount = Nodes.Count;
                }

                /// Connect node to all possible neighborhood
                SearchForConnections(l_Node);
                /// Notify that we need to save changes
                SetDirty();
            }
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Search all possible connections for a node
        /// </summary>
        /// <param name="p_Node">Node</param>
        private void SearchForConnections(NavNode p_Node)
        {
            /// Search connection in our current tile
            SearchConnectionsForTile(this, p_Node);

            /// Lookup in 8 near tiles
            for (int l_X = (Header.TileX - 1); l_X < (Header.TileX + 2); ++l_X)
            {
                for (int l_Y = (Header.TileY - 1); l_Y < (Header.TileY + 2); ++l_Y)
                {
                    /// Skip this tile, already proceeded
                    if (l_X == 0 && l_Y == 0)
                        continue;

                    var l_Tile = World.GetTile(l_X, l_Y);

                    if (l_Tile != null)
                        SearchConnectionsForTile(l_Tile, p_Node);
                }
            }
        }
        /// <summary>
        /// Search all possible connections for a node in a tile
        /// </summary>
        /// <param name="p_Tile">Lookup tile</param>
        /// <param name="p_Node">Node</param>
        private void SearchConnectionsForTile(NavTile p_Tile, NavNode p_Node)
        {
            lock (p_Tile.Nodes)
            {
                foreach (var l_Node in p_Tile.Nodes.Values)
                {
                    if (l_Node.NodeId == p_Node.NodeId)
                        continue;

                    double l_Dist = l_Node.Position.Distance3D(p_Node.Position);
                
                    if (l_Dist <= CONNECTION_DIST)
                    {
                        p_Tile.ConnectNode(p_Node, l_Node);
                    }
                }
            }
        }
        /// <summary>
        /// Make a node connection
        /// </summary>
        /// <param name="p_Left">Left node</param>
        /// <param name="p_Right">Right node</param>
        private void ConnectNode(NavNode p_Left, NavNode p_Right)
        {
            /// From left node
            {
                var l_Connection = new NavNodeConnection();
                l_Connection.FromNode   = p_Left.NodeId;
                l_Connection.ToNode     = p_Right.NodeId;
                l_Connection.Distance   = (float)p_Left.Position.Distance3D(p_Right.Position);

                if (p_Left.GetNodeIdHight() == TileId)
                {
                    lock (Connections) Connections.Add(l_Connection);
                    Header.NodesConnectionsCount++;
                }
                else
                {
                    int l_TileX, l_TileY;
                    World.TileIdToXY(p_Left.GetNodeIdHight(), out l_TileX, out l_TileY);

                    var l_Tile = World.GetTile(l_TileX, l_TileY);

                    if (l_Tile != null)
                    {
                        lock (l_Tile.Connections) l_Tile.Connections.Add(l_Connection);
                        l_Tile.Header.NodesConnectionsCount++;
                        l_Tile.SetDirty();
                    }
                }

                if (m_IsMain)
                    World.GetNavGraph()?.RegisterConnection(l_Connection);
            }
            /// From right node
            {
                var l_Connection = new NavNodeConnection();
                l_Connection.FromNode   = p_Right.NodeId;
                l_Connection.ToNode     = p_Left.NodeId;
                l_Connection.Distance   = (float)p_Left.Position.Distance3D(p_Right.Position);

                if (p_Right.GetNodeIdHight() == TileId)
                {
                    lock (Connections) Connections.Add(l_Connection);
                    Header.NodesConnectionsCount++;
                }
                else
                {
                    int l_TileX, l_TileY;
                    World.TileIdToXY(p_Right.GetNodeIdHight(), out l_TileX, out l_TileY);

                    var l_Tile = World.GetTile(l_TileX, l_TileY);

                    if (l_Tile != null)
                    {
                        lock (l_Tile.Connections) l_Tile.Connections.Add(l_Connection);
                        l_Tile.Header.NodesConnectionsCount++;
                        l_Tile.SetDirty();
                    }
                }

                if (m_IsMain)
                    World.GetNavGraph()?.RegisterConnection(l_Connection);
            }
        }

        /// <summary>
        /// Find nearest node to a position with a maximum distance
        /// </summary>
        /// <param name="p_Position">Search position</param>
        /// <param name="p_IncludeMask">Inclusion mask</param>
        /// <param name="p_MaxRange">Max search range</param>
        /// <returns>Found node or null</returns>
        public NavNode FindNearestNode(Utils.Vector3 p_Position, NavNode.NavNodeFlags p_IncludeMask = NavNode.NavNodeFlags.Walkable, float p_MaxRange = 100.0f)
        {
            lock (Nodes)
            {
                var l_Node = Nodes.Values.Where(x => (x.Flags & p_IncludeMask) != 0).OrderBy(x => x.Position.Distance3D(p_Position)).FirstOrDefault();

                return l_Node?.Position.Distance3D(p_Position) <= p_MaxRange ? l_Node : null;
            }
        }

        /// <summary>
        /// Get node by ID
        /// </summary>
        /// <param name="p_NodeID">Node id</param>
        /// <returns>Found node or null</returns>
        public NavNode GetNode(UInt64 p_NodeID)
        {
            NavNode l_Res = null;
            lock (Nodes) Nodes.TryGetValue(p_NodeID, out l_Res);

            return l_Res;
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Rename bad/old format tile file
        /// </summary>
        private void MoveOldTile()
        {
            var l_Path = TilePath + string.Format(".backup_{0}", DateTime.Now.ToFileTime());

            File.Move(TilePath, l_Path);
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Draw this tile
        /// </summary>
        /// <param name="p_Renderer">Renderer instance</param>
        /// <param name="p_RenderNodes">Draw nodes</param>
        /// <param name="p_RenderTilesBox">Draw tiles boxs</param>
        /// <param name="p_RenderConnections">Draw nodes connections</param>
        public void Draw(Game.Overlay.Renderer p_Renderer, bool p_RenderNodes, bool p_RenderTilesBox, bool p_RenderConnections)
        {
            /// Rebuild draw cache if needed
            if (m_GraphicDirty)
            {
                m_GraphicDirty = false;

                lock (Nodes)
                {
                    /// Nothing to draw if no nodes present
                    if (Nodes.Count == 0)
                        return;

                    m_Boxes.Clear();

                    foreach (var l_Node in Nodes.Values)
                    {
                        var l_Color = Color.Black;
                        
                        if (l_Node.Flags.HasFlag(NavNode.NavNodeFlags.Walkable))
                            l_Color = Color.White;
                        else if (l_Node.Flags.HasFlag(NavNode.NavNodeFlags.Flying))
                            l_Color = Color.Yellow;
                        else if (l_Node.Flags.HasFlag(NavNode.NavNodeFlags.Swimming))
                            l_Color = Color.Blue;
                        else if (l_Node.Flags.HasFlag(NavNode.NavNodeFlags.Transport))
                            l_Color = Color.Violet;

                        m_Boxes.Add(BuildBox(new Utils.Vector3(l_Node.Position.X - 0.25f, l_Node.Position.Y - 0.25f, l_Node.Position.Z),
                            0.5f, 0.5f, l_Color));
                    }
                
                    if (Connections.Count != 0)
                    {
                        lock (Connections)
                        {
                            m_ConnectionsLineVertices = new List<Game.Overlay.Renderer.ColoredVertex>(Connections.Count);

                            foreach (var l_Connection in Connections)
                            {
                                NavNode l_From = null;
                                NavNode l_To = null;

                                if (l_Connection.GetFromNodeTileId() == TileId)
                                    l_From = Nodes[l_Connection.FromNode];
                                else
                                {
                                    int l_TileX, l_TileY;
                                    World.TileIdToXY(l_Connection.GetFromNodeTileId(), out l_TileX, out l_TileY);

                                    var l_Tile = World.GetTile(l_TileX, l_TileY);

                                    if (l_Tile != null)
                                    {
                                        lock (l_Tile.Nodes) l_From = l_Tile.Nodes[l_Connection.FromNode];
                                    }
                                }

                                if (l_Connection.GetToNodeTileId() == TileId)
                                    l_To = Nodes[l_Connection.ToNode];
                                else
                                {
                                    int l_TileX, l_TileY;
                                    World.TileIdToXY(l_Connection.GetToNodeTileId(), out l_TileX, out l_TileY);

                                    var l_Tile = World.GetTile(l_TileX, l_TileY);

                                    if (l_Tile != null)
                                    {
                                        lock (l_Tile.Nodes) l_To = l_Tile.Nodes[l_Connection.ToNode];
                                    }
                                }

                                if (l_From == null || l_To == null)
                                    continue;

                                m_ConnectionsLineVertices.AddRange(new[]
                                {
                                    new Game.Overlay.Renderer.ColoredVertex(l_From.Position, Color.Red),
                                    new Game.Overlay.Renderer.ColoredVertex(l_To.Position, Color.Red)
                                });
                            }
                        }
                    }
                }
            }

            if (p_RenderTilesBox)
            {
                float l_PositionX = (32 * Utils.MathUtil.GridSize) - (((float)Header.TileX + 1) * Utils.MathUtil.GridSize);
                float l_PositionY = (32 * Utils.MathUtil.GridSize) - (((float)Header.TileY + 1) * Utils.MathUtil.GridSize);

                /// Draw tile AABBOX
                p_Renderer.DrawWorldBox(new Utils.Vector3(l_PositionX, l_PositionY, -200f), Utils.MathUtil.GridSize, 400f, Color.White);
            }

            if (p_RenderNodes && m_Boxes.Any())
            {
                p_Renderer.SetDrawModeWorld();
                
                foreach (var l_Box in m_Boxes)
                    p_Renderer.DrawUserPrimitives(Game.Overlay.Renderer.PrimitiveTopology.LineStrip, l_Box.ToArray());
            }

            if (p_RenderConnections && m_ConnectionsLineVertices != null)
            {
                p_Renderer.SetDrawModeWorld();
                p_Renderer.DrawUserPrimitives(Game.Overlay.Renderer.PrimitiveTopology.LineList, m_ConnectionsLineVertices.ToArray());
            }
        }

        /// <summary>
        /// Build box vertices
        /// </summary>
        /// <param name="p_Position">Box position</param>
        /// <param name="p_Width">Side size</param>
        /// <param name="p_Height">Height</param>
        /// <param name="p_Color">Box color</param>
        /// <returns>Box vertices</returns>
        private List<Game.Overlay.Renderer.ColoredVertex> BuildBox(Utils.Vector3 p_Position, float p_Width, float p_Height, Color p_Color)
        {
            var l_Buffer = new List<Game.Overlay.Renderer.ColoredVertex>(16);

            l_Buffer.AddRange(new[]
            {
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X, p_Position.Y, p_Position.Z, p_Color),                                           // 1
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X + p_Width, p_Position.Y, p_Position.Z, p_Color),                                 // 2
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X + p_Width, p_Position.Y + p_Width, p_Position.Z, p_Color),                       // 3
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X, p_Position.Y + p_Width, p_Position.Z, p_Color),                                 // 4
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X, p_Position.Y, p_Position.Z, p_Color),                                           // 5
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X, p_Position.Y, p_Position.Z + p_Height, p_Color),                                // 6
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X + p_Width, p_Position.Y, p_Position.Z + p_Height, p_Color),                      // 7
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X + p_Width, p_Position.Y, p_Position.Z, p_Color),                                 // 8
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X + p_Width, p_Position.Y, p_Position.Z + p_Height, 0xFFFFFFFF),                   // 9
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X + p_Width, p_Position.Y + p_Width, p_Position.Z + p_Height, p_Color),            // 10
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X + p_Width, p_Position.Y + p_Width, p_Position.Z, p_Color),                       // 11
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X + p_Width, p_Position.Y + p_Width, p_Position.Z + p_Height, 0xFFFFFFFF),         // 12
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X, p_Position.Y + p_Width, p_Position.Z + p_Height, p_Color),                      // 13
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X, p_Position.Y + p_Width, p_Position.Z, p_Color),                                 // 14
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X, p_Position.Y + p_Width, p_Position.Z + p_Height, 0xFFFFFFFF),                   // 15
                new Game.Overlay.Renderer.ColoredVertex(p_Position.X, p_Position.Y, p_Position.Z + p_Height, p_Color)                                 // 16
            });

            return l_Buffer;
        }
    }
}
