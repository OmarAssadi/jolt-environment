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
using System.Threading;

using RuneScape.Communication.Messages;
using RuneScape.Network;

namespace RuneScape.Communication.Login
{
    /// <summary>
    /// Represents a single login request from a new connection.
    /// </summary>
    public class LoginRequest
    {
        #region Properties
        /// <summary>
        /// Gets the RuneScape.Network.Node object associated with this login request.
        /// </summary>
        public Node Connection { get; private set; }
        /// <summary>
        /// Gets the RuneScape.Utilities.Buffer.ReadBuffer object used to read sent data.
        /// </summary>
        public Packet Buffer { get; set; }

        /// <summary>
        /// Gets or sets whether the request was successful was finished.
        /// </summary>
        public bool Finished { get; set; }
        /// <summary>
        /// Gets or sets whether the request should be removed.
        /// </summary>
        public bool Remove { get; set; }
        /// <summary>
        /// Gets the request's login stage.
        /// </summary>
        public int LoginStage { get; set; }
        /// <summary>
        /// Gets the request's hashed name (for detecting floods, bot attacks, etc).
        /// </summary>
        public int NameHash { get; set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new login request.
        /// </summary>
        /// <param name="connection">The connection object.</param>
        public LoginRequest(Node connection)
        {
            this.Connection = connection;
            StartConnection();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Starts the request's connection.
        /// </summary>
        public void StartConnection()
        {
            if (this.Connection != null)
            {
                this.Connection.Start(
                    new RouteDataCallback(DataRecieved),
                    new DisconnectCallback(OnRequestDisconnect));
            }
        }

        /// <summary>
        /// Disconnects a user.
        /// </summary>
        public void OnRequestDisconnect(Node node)
        {
            GameServer.TcpConnection.DropConnection(this.Connection);
            this.Remove = true;
        }

        /// <summary>
        /// When data is recieved, it is processed here.
        /// </summary>
        /// <param name="data">The data to be processed.</param>
        private void DataRecieved(Node node, byte[] data)
        {
            if (this.Buffer != null) // More data needs to be recieved.
            {
                this.Buffer.AddBytes(data);
            }
            else this.Buffer = new Packet(data); // There is no data, so we create a new buffer.
        }
        #endregion Methods
    }
}
