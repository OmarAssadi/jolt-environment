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

using RuneScape.Utilities;

namespace RuneScape.Network
{
    /// <summary>
    /// Handles connections recieved remotely (via HTTP, FTP, TCP, etc).
    /// </summary>
    public class RemoteManager : INetworkManager
    {
        #region Properties
        /// <summary>
        /// Gets the listener providing service to accepting connections.
        /// </summary>
        public ConnectionListener Listener { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new remote manager.
        /// </summary>
        /// <param name="ip">The IP to listen from.</param>
        /// <param name="port">The port to listen from.</param>
        /// <param name="allowExternal">Whether to allow connections 
        /// from locations other than just localhost.</param>
        public RemoteManager(string ip, int port)
        {
            this.Listener = new ConnectionListener(ip, port, this);
            RemoteCommands.LoadCommands();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Handles new connections.
        /// </summary>
        /// <param name="node">The node providing serivce between server and client.</param>
        public void HandleNewConnection(Node node)
        {
            node.Start(
                new RouteDataCallback(OnDataRecieve), 
                new DisconnectCallback(OnDisconnection));
        }

        /// <summary>
        /// Triggered when a node disconnects.
        /// </summary>
        private void OnDisconnection(Node node)
        {
            node.Socket.Dispose();
        }

        /// <summary>
        /// Triggered when a node recieves data.
        /// </summary>
        /// <param name="data">The data recieved.</param>
        public void OnDataRecieve(Node node, byte[] data)
        {
            string[] arguments = Encoding.ASCII.GetString(data).Split('\0');
            string command = arguments[0].ToLower();
            node.SendData(Encoding.ASCII.GetBytes(RemoteCommands.Handle(command, arguments)));
            node.Socket.Dispose(); // We don't need this connection anymore.
        }
        #endregion Methods
    }
}
