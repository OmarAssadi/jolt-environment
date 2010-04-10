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
using System.Data;
using System.Net;
using System.Net.Sockets;

using JoltEnvironment.Storage.Sql;

namespace RuneScape.Network
{
    /// <summary>
    /// Provides a listener for new connections.
    /// </summary>
    public class ConnectionListener
    {
        #region Fields
        /// <summary>
        /// The maximum amount of queued connections to the listener.
        /// </summary>
        private const int ListenQueueSize = 5;

        /// <summary>
        /// A socket for listening for new connections.
        /// </summary>
        private TcpListener listener;
        /// <summary>
        /// Called when a connection is asynchronously recieved.
        /// </summary>
        private AsyncCallback connectionRequest;

        /// <summary>
        /// The connection manager.
        /// </summary>
        private INetworkManager manager;
        /// <summary>
        /// The connection factory.
        /// </summary>
        private ConnectionFactory factory;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets whether the listener is listening.
        /// </summary>
        public bool Listening { get; private set; }

        /// <summary>
        /// Gets whether the connections requests should be logged.
        /// </summary>
        public bool LogConnections { get; private set; }
        /// <summary>
        /// Gets whether the connections requested should be checked in blacklist.
        /// </summary>
        public bool CheckBlacklist { get; private set; }
        /// <summary>
        /// Gets the connection factory.
        /// </summary>
        public ConnectionFactory Factory { get { return this.factory; } }
        #endregion Proeprties

        #region Constructors
        /// <summary>
        /// Constructs a new connection listener.
        /// </summary>
        /// <param name="ip">The local ip to listen from (usually 127.0.0.1 or LAN ip).</param>
        /// <param name="port">The port to listen from.</param>
        /// <param name="manager">The connection manager.</param>
        public ConnectionListener(string ip, int port, INetworkManager manager)
        {
            IPAddress boundIP = null;

            if (ip == "any")
            {
                boundIP = IPAddress.Any;
            }
            else if (!IPAddress.TryParse(ip, out boundIP))
            {
                boundIP = IPAddress.Loopback;
                Program.Logger.WriteWarn("Unable to parse given local ip address \"" + ip + "\"."
                    + "\nBinding listener to " + boundIP + " instead.");
            }

            this.listener = new TcpListener(boundIP, port);
            this.connectionRequest = new AsyncCallback(OnConnectionRequest);
            this.factory = new ConnectionFactory();
            this.manager = manager;
            Program.Logger.WriteInfo("Listening for new connections on " + boundIP + ":" + port + ".");
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Starts listening for new connections.
        /// </summary>
        public void Start(bool checkBlacklist)
        {
            // Check black list option.
            this.CheckBlacklist = checkBlacklist;

            // We have to check if the listener is already listening so we don't disrupt it.
            if (this.Listening)
            {
                return;
            }

            this.listener.Start(ConnectionListener.ListenQueueSize);
            this.Listening = true;

            StandBy();
        }

        /// <summary>
        /// Stops listening for new connections.
        /// </summary>
        public void Stop()
        {
            // There's no point trying to stop the listener if it's already stopped!
            if (!this.Listening)
            {
                return;
            }

            this.Listening = false;
            this.listener.Stop();
        }

        /// <summary>
        /// Stands by for new connections.
        /// </summary>
        public void StandBy()
        {
            try
            {
                this.listener.BeginAcceptSocket(this.connectionRequest, null);
            }
            catch (ObjectDisposedException ode)
            {
                Program.Logger.WriteException(ode);
            }
            catch (SocketException se)
            {
                Program.Logger.WriteException(se);
            }
        }

        /// <summary>
        /// Handles connection requests.
        /// </summary>
        /// <param name="async">The asynchronous handle result.</param>
        public void OnConnectionRequest(IAsyncResult result)
        {
            try
            {
                Socket socket = this.listener.EndAcceptSocket(result);
                StandBy();

                if (this.CheckBlacklist)
                {
                    using (SqlDatabaseClient client = GameServer.Database.GetClient())
                    {
                        client.AddParameter("ip", socket.RemoteEndPoint.ToString().Split(':')[0]);
                        DataRow row = client.ReadDataRow("SELECT * FROM blacklist WHERE ip_address = @ip LIMIT 1;");

                        if (row != null)
                        {
                            Program.Logger.WriteDebug("Connection attempt from " + socket.RemoteEndPoint + " denied.");
                            socket.Close();
                            socket = null;
                        }
                    }
                }

                Node node = this.factory.Create(socket);
                if (node != null)
                {
                    this.manager.HandleNewConnection(node);
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
        }
        #endregion Methods
    }
}
