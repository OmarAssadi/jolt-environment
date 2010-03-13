/* 
    Jolt Environment
    Copyright (C) 2010 Jolt Environment Team

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RuneScape.Communication.Login;

namespace RuneScape.Network
{
    /// <summary>
    /// Provides management for tcp connections.
    /// </summary>
    public class ConnectionManager : INetworkManager
    {
        #region Fields
        /// <summary>
        /// The maximum amount of simultaneous connections on the server.
        /// </summary>
        private readonly int maxConnections;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the connection listener.
        /// </summary>
        public ConnectionListener Listener { get; private set; }
        /// <summary>
        /// Gets the list of current connections.
        /// </summary>
        public List<Node> CurrentConnections { get; private set; }

        /// <summary>
        /// Gets the maximum amount of simultaneous connections.
        /// </summary>
        public int MaxConnections { get { return this.maxConnections; } }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new connection management.
        /// </summary>
        /// <param name="ip">The local ip for the listener to listen from.</param>
        /// <param name="port">The local port for the listener to listen from.</param>
        /// <param name="maxConnections">The maximum amount of simultaneous 
        /// connections though this management.</param>
        public ConnectionManager(string ip, int port, int maxConnections)
        {
            int initialCapacity = maxConnections;
            if (maxConnections > 4)
                initialCapacity /= 4;

            this.CurrentConnections = new List<Node>(initialCapacity);
            this.maxConnections = maxConnections;
            this.Listener = new ConnectionListener(ip, port, this);
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Destroys the connection management.
        /// </summary>
        public void Destroy()
        {
            lock (this.CurrentConnections)
            {
                int count = 0;
                // Safely disconnect all the users.
                this.CurrentConnections.ForEach((n) =>
                {
                    n.OnDisconnect();
                    count++;
                });
                this.CurrentConnections.Clear();
                Console.WriteLine(" -> Removed and saved " + count + " online characters.");
            }
        }

        /// <summary>
        /// Handles a newly created connection.
        /// </summary>
        /// <param name="node">The node processed though the connection factory.</param>
        public void HandleNewConnection(Node node)
        {
            Program.Logger.WriteDebug("New connection [id=" + node.Id + "] created for " + node.EndPoint + ".");
            lock (this.CurrentConnections)
            {
                this.CurrentConnections.Add(node);
            }
            GameEngine.LoginWorker.AddRequest(new LoginRequest(node));
        }

        /// <summary>
        /// Drops a specified connection.
        /// </summary>
        /// <param name="node">The connection to drop.</param>
        public void DropConnection(Node node)
        {
            lock (this.CurrentConnections)
            {
                if (node != null && this.CurrentConnections.Contains(node))
                {
                    node.Socket.Close();
                    this.CurrentConnections.Remove(node);
                    Program.Logger.WriteDebug("Connection [id=" + node.Id + "] dropped.");
                }
            }
        }
        #endregion Methods
    }
}
