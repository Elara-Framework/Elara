using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace Elara.Navigation
{
    /// <summary>
    /// Navigation world class
    /// </summary>
    public class NavWorld : Game.ChildObject
    {
        /// <summary>
        /// Current map id
        /// </summary>
        int m_CurrentMapId = -1;
        /// <summary>
        /// Current tile id
        /// </summary>
        int m_CurrentTileId = -1;
        /// <summary>
        /// Navigation directory
        /// </summary>
        string m_NavigationDirectory;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Loaded authors
        /// </summary>
        List<string> m_Authors;
        /// <summary>
        /// Authors priority
        /// </summary>
        Dictionary<string, int> m_AuthorsPriorities;
        /// <summary>
        /// Sorted authors by priority
        /// </summary>
        List<string> m_SortedAuthors;

        /// <summary>
        /// Current editing author
        /// </summary>
        string m_EditingAuthor = null;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Navigation tiles for authors
        /// </summary>
        Dictionary<int, List<NavTile>> m_Tiles = new Dictionary<int, List<NavTile>>();
        /// <summary>
        /// Navigations tile for editing author
        /// </summary>
        Dictionary<int, NavTile> m_EditingAuthorTiles = new Dictionary<int, NavTile>();

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Is auto mapper enabled
        /// </summary>
        public bool AutoMapperEnabled { get; set; } = false;
        /// <summary>
        /// Last played position
        /// </summary>
        Utils.Vector3 m_LastPlayerPosition = new Utils.Vector3(-99999, -99999, -99999);

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Navigation graph instance
        /// </summary>
        private NavGraph m_NavGraph = null;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_Game">Game instance</param>
        public NavWorld(Game p_Game)
            : base(p_Game)
        {
            m_NavigationDirectory = Path.Combine(Application.StartupPath, "Navigation");

            /// Create navigation directory if not exist
            if (!Directory.Exists(m_NavigationDirectory))
                Directory.CreateDirectory(m_NavigationDirectory);

            /// Init containers
            m_Authors           = new List<string>();
            m_AuthorsPriorities = new Dictionary<string, int>();
            m_SortedAuthors     = new List<string>();
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Auto correct author name
        /// </summary>
        /// <param name="p_Name">Author name</param>
        /// <returns></returns>
        public string AutoCorrectAuthorName(string p_Name)
        {
            if (p_Name == null)
                return null;

            if (HasAuthor(p_Name))
                return m_Authors.FirstOrDefault(x => x.ToLower() == p_Name.ToLower());
            else
            {
                var l_List = GetAvailableAuthors();

                return l_List.FirstOrDefault(x => x.ToLower() == p_Name.ToLower());
            }
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Has author
        /// </summary>
        /// <param name="p_Name">Author name</param>
        /// <returns></returns>
        public bool HasAuthor(string p_Name)
        {
            return m_Authors.Any(x => x.ToLower() == p_Name.ToLower());
        }
        /// <summary>
        /// Author exist
        /// </summary>
        /// <param name="p_Name">Author name</param>
        /// <returns></returns>
        public bool AuthorExist(string p_Name)
        {
            var l_List = GetAvailableAuthors();

            if (l_List.Any(x => x.ToLower() == p_Name.ToLower()))
                return true;

            return false;
        }
        /// <summary>
        /// Create an author
        /// </summary>
        /// <param name="p_Name">Author name</param>
        /// <returns></returns>
        public bool CreateNewAuthor(string p_Name)
        {
            var l_List = GetAvailableAuthors();

            if (l_List.Any(x => x.ToLower() == p_Name.ToLower()))
                return false;

            var l_Path = Path.Combine(m_NavigationDirectory, p_Name);

            if (!Directory.Exists(l_Path))
                Directory.CreateDirectory(l_Path);

            l_Path = Path.Combine(l_Path, "Version.txt");

            File.WriteAllText(l_Path, DateTime.Now.ToBinary().ToString());

            return true;
        }
        /// <summary>
        /// Clone author
        /// </summary>
        /// <param name="p_From">Author to clone</param>
        /// <param name="p_To">New author name</param>
        /// <returns></returns>
        public bool CloneAuthor(string p_From, string p_To)
        {
            if (!AuthorExist(p_From))
                return false;

            if (AuthorExist(p_To))
                return false;

            var l_From = Path.Combine(m_NavigationDirectory, p_From);
            var l_To   = Path.Combine(m_NavigationDirectory, p_To);

            var l_DestFolder = new DirectoryInfo(l_To);
            l_DestFolder.Create();

            foreach (var l_SourceSubDirPath in Directory.EnumerateDirectories(l_From, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(l_SourceSubDirPath.Replace(l_From, l_To));

            foreach (var l_File in Directory.EnumerateFiles(l_From, "*", SearchOption.AllDirectories))
                File.Copy(l_File, l_File.Replace(l_From, l_To), true);

            l_To = Path.Combine(l_To, "Version.txt");

            File.WriteAllText(l_To, DateTime.Now.ToBinary().ToString());

            return true;
        }
        /// <summary>
        /// Add a new author
        /// </summary>
        /// <param name="p_Name">Author name</param>
        /// <param name="p_Priority">Author priority</param>
        /// <returns>Success or not</returns>
        public bool AddAuthor(string p_Name, int p_Priority)
        {
            /// Editing author is already an navigation provider
            if (m_EditingAuthor == p_Name)
                return false;

            /// Check duplicates
            if (HasAuthor(p_Name))
                return false;

            p_Name = AutoCorrectAuthorName(p_Name);

            /// Skip if the folder doesn't exist
            if (!Directory.Exists(Path.Combine(m_NavigationDirectory, p_Name)))
                return false;

            m_Authors.Add(p_Name);
            m_AuthorsPriorities.Add(p_Name, p_Priority);

            /// Rebuild sorted authors list
            RebuildAuthors();
            /// Trigger map change (rebuild Graph and load new tiles)
            OnMapChange(m_CurrentMapId, true);

            return true;
        }
        /// <summary>
        /// Remove author
        /// </summary>
        /// <param name="p_Name">Author name</param>
        /// <returns></returns>
        public bool RemoveAuthor(string p_Name)
        {
            p_Name = AutoCorrectAuthorName(p_Name);

            if (!HasAuthor(p_Name))
                return false;

            m_Authors.Remove(p_Name);
            m_AuthorsPriorities.Remove(p_Name);
            RebuildAuthors();
            OnMapChange(m_CurrentMapId, true);

            return true;
        }
        /// <summary>
        /// Set editing author name
        /// </summary>
        /// <param name="p_Name">Author name</param>
        public bool SetEditingAuthor(string p_Name)
        {
            p_Name = AutoCorrectAuthorName(p_Name);

            /// Don't change if it's the same
            if (m_EditingAuthor == p_Name)
                return false;

            m_EditingAuthor = p_Name;
            m_EditingAuthorTiles.Clear();

            if (m_EditingAuthor == null)
            {
                OnMapChange(m_CurrentMapId, true);
                return true;
            }

            /// Remove from authors if exist in it
            if (m_Authors.Contains(p_Name))
            {
                m_Authors.Remove(p_Name);
                m_AuthorsPriorities.Remove(p_Name);
                RebuildAuthors();
            }

            var l_Path = Path.Combine(m_NavigationDirectory, p_Name);

            /// Create author directory
            if (!Directory.Exists(l_Path))
                Directory.CreateDirectory(l_Path);

            OnMapChange(m_CurrentMapId, true);

            return true;
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Reset all authors priority
        /// </summary>
        public void ResetAuthorsPriorities()
        {
            m_AuthorsPriorities.Clear();

            foreach (var l_Current in m_Authors)
                m_AuthorsPriorities.Add(l_Current, 0);

            RebuildAuthors();
            OnMapChange(m_CurrentMapId, true);
        }
        /// <summary>
        /// Set author priority
        /// </summary>
        /// <param name="p_Name">Author name</param>
        /// <param name="p_Priority">Author priority</param>
        /// <returns></returns>
        public bool SetAuthorPriority(string p_Name, int p_Priority)
        {
            p_Name = AutoCorrectAuthorName(p_Name);

            if (!HasAuthor(p_Name))
                return false;

            m_AuthorsPriorities[p_Name] = p_Priority;

            RebuildAuthors();
            OnMapChange(m_CurrentMapId, true);

            return true;
        }
        /// <summary>
        /// Rebuild sorted authors by priority list
        /// </summary>
        private void RebuildAuthors()
        {
            m_SortedAuthors = new List<string>(m_Authors.Count);

            foreach (var l_Pair in m_AuthorsPriorities.OrderBy(x => x.Value))
                m_SortedAuthors.Add(l_Pair.Key);
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Get all available authors
        /// </summary>
        /// <returns></returns>
        public string[] GetAvailableAuthors()
        {
            return Directory.GetDirectories(m_NavigationDirectory).Select(x => new DirectoryInfo(x).Name).ToArray();
        }
        /// <summary>
        /// Get editing author
        /// </summary>
        /// <returns></returns>
        public string GetEditingAuthor()
        {
            return m_EditingAuthor != null ? m_EditingAuthor : "";
        }
        /// <summary>
        /// Get author version
        /// </summary>
        /// <param name="p_Name">Author name</param>
        /// <returns></returns>
        public DateTime GetAuthorVersion(string p_Name)
        {
            var l_List = GetAvailableAuthors();

            if (!l_List.Any(x => x.ToLower() == p_Name.ToLower()))
                return DateTime.MinValue;

            var l_Path = Path.Combine(m_NavigationDirectory, p_Name, "Version.txt");

            if (!File.Exists(l_Path))
                return DateTime.MinValue;

            var l_Text = File.ReadAllText(l_Path);
            long l_Date = 0;

            if (!long.TryParse(l_Text, out l_Date))
                return DateTime.MinValue;

            return DateTime.FromBinary(l_Date);
        }
        /// <summary>
        /// Get author priority
        /// </summary>
        /// <param name="p_Name">Author name</param>
        /// <returns></returns>
        public int GetAuthorPriority(string p_Name)
        {
            p_Name = AutoCorrectAuthorName(p_Name);

            if (!HasAuthor(p_Name))
                return -1;

            return m_AuthorsPriorities[p_Name];
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Build packed tile id
        /// </summary>
        /// <param name="p_X">Tile X</param>
        /// <param name="p_Y">Tile Y</param>
        /// <returns>Packed tile id</returns>
        public int BuildTileId(int p_X, int p_Y)
        {
            return ((p_X << 16) | p_Y);
        }
        /// <summary>
        /// Get tileX & tileY from packet tile Id
        /// </summary>
        /// <param name="p_TileId">[IN]Packet tile id</param>
        /// <param name="p_TileX">[OUT]Tile x</param>
        /// <param name="p_TileY">[OUT]Tile y</param>
        public void TileIdToXY(int p_TileId, out int p_TileX, out int p_TileY)
        {
            p_TileX = (short)((p_TileId >> 16) & 0xFFFF);
            p_TileY = (short)(p_TileId & 0xFFFF);
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Get tile by TileX, TileY
        /// </summary>
        /// <param name="p_X">Tile X</param>
        /// <param name="p_Y">Tile Y</param>
        /// <returns>Found tile or null</returns>
        public NavTile GetTile(int p_X, int p_Y)
        {
            int l_TileId = BuildTileId(p_X, p_Y);

            if (m_EditingAuthor != null)
            {
                if (m_EditingAuthorTiles == null)
                    return null;

                NavTile l_Result = null;

                lock (m_EditingAuthorTiles) m_EditingAuthorTiles.TryGetValue(l_TileId, out l_Result);

                return l_Result;
            }
            else
            {
                lock (m_Tiles)
                {
                    if (m_Tiles.ContainsKey(l_TileId) && m_Tiles[l_TileId].Count > 0)
                    {
                        return m_Tiles[l_TileId].First();
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// Get node by full Id
        /// </summary>
        /// <param name="p_NodeID">Full node id</param>
        /// <returns>Found node or null</returns>
        public NavNode GetNode(UInt64 p_NodeID)
        {
            int l_TileId = (int)((p_NodeID >> 32) & 0xFFFFFFFF);

            /// Extract node tile id
            int l_TileX, l_TileY;
            TileIdToXY(l_TileId, out l_TileX, out l_TileY);

            var l_Tile = GetTile(l_TileX, l_TileY);

            if (l_Tile == null)
                return null;

            /// Find node in the tile
            return l_Tile.GetNode(p_NodeID);
        }
        /// <summary>
        /// Find nearest node to a position with a maximum distance
        /// </summary>
        /// <param name="p_Position">Search position</param>
        /// <param name="p_MaxRange">Max search range</param>
        /// <param name="p_IncludeMask">Inclusion mask</param>
        /// <returns>Found node or null</returns>
        public NavNode FindNearestNode(Utils.Vector3 p_PlayerPosition, NavNode.NavNodeFlags p_IncludeMask = NavNode.NavNodeFlags.Walkable, float p_MaxRange = 100.0f)
        {
            int l_TileX = p_PlayerPosition.TileX;
            int l_TileY = p_PlayerPosition.TileY;

            List<NavNode> l_Nodes = new List<NavNode>();

            /// Lookup in 8 near tiles
            for (int l_X = (l_TileX - 1); l_X < (l_TileX + 2); ++l_X)
            {
                for (int l_Y = (l_TileY - 1); l_Y < (l_TileY + 2); ++l_Y)
                {
                    NavTile l_Tile = GetTile(l_X, l_Y);

                    if (l_Tile != null)
                    {
                        var l_Node = l_Tile.FindNearestNode(p_PlayerPosition, p_IncludeMask, p_MaxRange);

                        if (l_Node != null)
                            l_Nodes.Add(l_Node);
                    }
                }
            }

            var l_FinalNode = l_Nodes.OrderBy(x => x.Position.Distance3D(p_PlayerPosition)).FirstOrDefault();

            return l_FinalNode?.Position.Distance3D(p_PlayerPosition) <= p_MaxRange ? l_FinalNode : null;
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Get navigation graph
        /// </summary>
        /// <returns>NavGraph or null</returns>
        public NavGraph GetNavGraph()
        {
            return m_NavGraph;
        }
        /// <summary>
        /// Build a navigation query for path finding
        /// </summary>
        /// <returns>NavQuery or null</returns>
        public NavQuery BuildNavQuery()
        {
            if (m_NavGraph == null)
                return null;

            return new NavQuery(m_NavGraph);
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Save all dirty editing author's tiles
        /// </summary>
        public void Save()
        {
            if (m_EditingAuthor != null)
            {
                bool l_SomethingSaved = false;
                foreach (var l_Tile in m_EditingAuthorTiles.Values)
                {
                    if (l_Tile.Save())
                        l_SomethingSaved = true;
                }

                if (l_SomethingSaved)
                {
                    var l_FileName = Path.Combine(m_NavigationDirectory, m_EditingAuthor, "Version.txt");

                    File.WriteAllText(l_FileName, DateTime.Now.ToBinary().ToString());
                }
            }
        }
        /// <summary>
        /// Draw tiles
        /// </summary>
        /// <param name="p_Renderer">Rendered instance</param>
        /// <param name="p_RenderNodes">Draw nodes</param>
        /// <param name="p_RenderTilesBox">Draw tiles boxs</param>
        /// <param name="p_RenderConnections">Draw nodes connections</param>
        public void Draw(Game.Overlay.Renderer p_Renderer, bool p_RenderNodes, bool p_RenderTilesBox, bool p_RenderConnections)
        {
            if (m_EditingAuthorTiles != null && m_EditingAuthorTiles.Count != 0)
            {
                foreach (var l_Tile in m_EditingAuthorTiles.Values)
                    l_Tile.Draw(p_Renderer, p_RenderNodes, p_RenderTilesBox, p_RenderConnections);
            }
            else
            {
                foreach (var l_Tiles in m_Tiles.Values)
                    l_Tiles.FirstOrDefault()?.Draw(p_Renderer, p_RenderNodes, p_RenderTilesBox, p_RenderConnections);
            }
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// When the map change
        /// </summary>
        /// <param name="p_MapId">New map Id</param>
        /// <param name="p_Force">Force change</param>
        public void OnMapChange(int p_MapId, bool p_Force = false)
        {
            if (!p_Force && p_MapId == m_CurrentMapId)
                return;

            m_CurrentMapId = p_MapId;

            lock (m_Tiles) m_Tiles.Clear();
            lock (m_EditingAuthorTiles) m_EditingAuthorTiles.Clear();

            m_NavGraph = new NavGraph(this);

            if (p_MapId == -1)
                return;

            /// Load all authors tiles
            foreach (var l_Author in m_SortedAuthors)
            {
                var l_MapDirectory = Path.Combine(m_NavigationDirectory, l_Author, p_MapId.ToString());

                if (!Directory.Exists(l_MapDirectory))
                    continue;

                foreach (var l_File in new DirectoryInfo(l_MapDirectory).EnumerateFiles("*_*.dat"))
                {
                    var l_Parts = l_File.Name.Replace(l_File.Extension, "").Split('_');
                    if (l_Parts.Length != 2)
                        continue;

                    int l_X, l_Y;
                    if (int.TryParse(l_Parts[0], out l_X) &&
                        int.TryParse(l_Parts[1], out l_Y))
                    {
                        int l_TileId = BuildTileId(l_X, l_Y);

                        lock (m_Tiles)
                        {
                            if (!m_Tiles.ContainsKey(l_TileId))
                                m_Tiles.Add(l_TileId, new List<NavTile>());

                            bool l_IsMain = false;
                            if (m_EditingAuthor == null && m_Tiles[l_TileId].Count == 0)
                                l_IsMain = true;

                            m_Tiles[l_TileId].Add(new NavTile(this, l_File.FullName, l_X, l_Y, l_IsMain));
                        }
                    }
                }
            }

            /// Load all editing author tiles
            if (m_EditingAuthor != null)
            {
                var l_MapPath = Path.Combine(m_NavigationDirectory, m_EditingAuthor, p_MapId.ToString());
                if (!Directory.Exists(l_MapPath))
                    Directory.CreateDirectory(l_MapPath);
                else
                {
                    var l_MapDirectory = Path.Combine(m_NavigationDirectory, m_EditingAuthor, p_MapId.ToString());

                    foreach (var l_File in new DirectoryInfo(l_MapDirectory).EnumerateFiles("*_*.dat"))
                    {
                        var l_Parts = l_File.Name.Replace(l_File.Extension, "").Split('_');
                        if (l_Parts.Length != 2)
                            continue;

                        int l_X, l_Y;
                        if (int.TryParse(l_Parts[0], out l_X) &&
                            int.TryParse(l_Parts[1], out l_Y))
                        {
                            int l_TileId = BuildTileId(l_X, l_Y);

                            lock (m_EditingAuthorTiles)
                            {
                                if (!m_EditingAuthorTiles.ContainsKey(l_TileId))
                                    m_EditingAuthorTiles[l_TileId] = new NavTile(this, l_File.FullName, l_X, l_Y, true);
                            }
                        }
                    }
                }
            }

            /// Extract tile X & Y
            int l_TileX, l_TileY;
            TileIdToXY(m_CurrentTileId, out l_TileX, out l_TileY);

            /// Force tile change
            OnTileChange(l_TileX, l_TileY, true);
        }

        /// <summary>
        /// When the current tile change
        /// </summary>
        /// <param name="p_X">Tile X</param>
        /// <param name="p_Y">Tile Y</param>
        /// <param name="p_Force">Force change</param>
        public void OnTileChange(int p_X, int p_Y, bool p_Force = false)
        {
            /// Default tile ID on map change, skip
            if (p_X == -1 && p_Y == -1)
            {
                m_CurrentTileId = -1;
                return;
            }

            /// Build tile id
            var l_TileId = BuildTileId(p_X, p_Y);

            /// If we need to change something
            if (p_Force || l_TileId != m_CurrentTileId)
            {
                m_CurrentTileId = l_TileId;

                if (m_EditingAuthor != null)
                {
                    lock (m_EditingAuthorTiles)
                    {
                        /// Create the new tile if the editing author doesn't have one
                        if (!m_EditingAuthorTiles.ContainsKey(l_TileId))
                        {
                            var l_FileName = string.Format("{0}_{1}.dat", p_X, p_Y);
                            var l_TilePath = Path.Combine(m_NavigationDirectory, m_EditingAuthor, m_CurrentMapId.ToString(), l_FileName);

                            m_EditingAuthorTiles.Add(l_TileId, new NavTile(this, l_TilePath, p_X, p_Y, true));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// When the player move
        /// </summary>
        /// <param name="p_Position">New player position</param>
        public void OnPlayerMove(Utils.Vector3 p_Position)
        {
            if (m_EditingAuthor == null)
                return;

            /// Check if the player is far enough to the last position
            if (m_LastPlayerPosition.Distance3D(p_Position) < (NavTile.MIN_NODE_DISTANCE / 4.0f))
                return;

            m_LastPlayerPosition = p_Position;

            /// Add node is auto mapping is enabled
            if (AutoMapperEnabled && m_EditingAuthor != null)
            {
                int l_TileX = m_LastPlayerPosition.TileX;
                int l_TileY = m_LastPlayerPosition.TileY;
                var l_Tile = GetTile(l_TileX, l_TileY);

                if (l_Tile != null)
                    l_Tile.AddNodeFromUnit(GameOwner.ObjectManager.LocalPlayer);
            }
        }


    }
}
