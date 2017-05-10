using System.Threading;
using System.Windows.Forms;

namespace Elara.NavigationStudio
{
    /// <summary>
    /// Extension root class
    /// </summary>
    public class NavigationStudio : Extensions.Module
    {
        /// <summary>
        /// Extension name
        /// </summary>
        public override string Name => "Navigation Studio";
        /// <summary>
        /// Extension category
        /// </summary>
        public override string Category => "Creations";
        /// <summary>
        /// Extension author
        /// </summary>
        public override string Author => "Elara";
        /// <summary>
        /// Extension main UI
        /// </summary>
        public override UserControl Interface => m_Interface;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Is running
        /// </summary>
        private bool m_Running = false;
        /// <summary>
        /// Update thread instance
        /// </summary>
        private System.Threading.Thread m_UpdateThread = null;
        /// <summary>
        /// User interface
        /// </summary>
        private UserControlStudio m_Interface;
        /// <summary>
        /// Navigation world
        /// </summary>
        private Navigation.NavWorld m_World = null;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Should draw navigation nodes
        /// </summary>
        public bool DrawNodes = false;
        /// <summary>
        /// Should draw navigation tiles box
        /// </summary>
        public bool DrawTilesBox = false;
        /// <summary>
        /// Should draw navigation nodes connections
        /// </summary>
        public bool DrawConnections = false;

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_Elara">Elara instance</param>
        public NavigationStudio(Elara p_Elara)
            : base(p_Elara)
        {

        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// On extension load
        /// </summary>
        public override bool OnLoad()
        {
            /// Create navigation world instance
            m_World = new Navigation.NavWorld(Elara.Game);
            /// Create main interface
            m_Interface = new UserControlStudio(this);

            /// Create update thread
            m_UpdateThread = new Thread(UpdateThread);
            m_UpdateThread.Start();

            /// Register callback for overlay rendering
            Elara.Game.OnRenderOverlay += Game_OnRenderOverlay;

            return base.OnLoad();
        }
        /// <summary>
        /// On extension unload
        /// </summary>
        public override bool OnUnload()
        {
            /// Unregister callback for overlay rendering
            Elara.Game.OnRenderOverlay -= Game_OnRenderOverlay;

            /// Terminate update thread
            m_Running = false;
            if (m_UpdateThread != null)
            {
                m_UpdateThread.Join();
                m_UpdateThread = null;
            }

            /// Release main interface
            m_Interface?.Dispose();
            m_Interface = null;

            return base.OnUnload();
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// On render overlay
        /// </summary>
        /// <param name="p_Overlay">Overlay instance</param>
        /// <param name="p_Renderer">Renderer instance</param>
        private void Game_OnRenderOverlay(Game.Overlay p_Overlay, Game.Overlay.Renderer p_Renderer)
        {
            /// Don't draw if not running
            if (!m_Running)
                return;

            /// Draw toggled elements
            if (DrawNodes || DrawTilesBox || DrawConnections)
                m_World.Draw(p_Renderer, DrawNodes, DrawTilesBox, DrawConnections);
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Get navigation world instance
        /// </summary>
        /// <returns>Nav world instance</returns>
        public Navigation.NavWorld GetNavWorld()
        {
            return m_World;
        }
        /// <summary>
        /// Set auto mapper enabled or not
        /// </summary>
        /// <param name="p_Enabled">Is enabled</param>
        public void SetAutoMap(bool p_Enabled)
        {
            m_World.AutoMapperEnabled = p_Enabled;
        }
        /// <summary>
        /// Save the navigation world
        /// </summary>
        public void Save()
        {
            m_World.Save();
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Update the navigation world
        /// </summary>
        private void UpdateThread()
        {
            m_Running = true;
            while (m_Running)
            {
                var l_Player = Elara.Game.ObjectManager.LocalPlayer;

                if (l_Player != null)
                {
                    var l_Position = l_Player.Position;

                    m_World.OnMapChange(l_Player.CurrentMapId);
                    m_World.OnTileChange(l_Position.TileX, l_Position.TileY);

                    if (m_World.AutoMapperEnabled)
                        m_World.OnPlayerMove(l_Position);
                }

                Thread.Sleep(10);
            }
        }

    }
}
