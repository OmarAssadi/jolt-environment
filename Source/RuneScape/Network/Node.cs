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
using System.Net.Sockets;
using System.Threading.Tasks;

using JoltEnvironment.Utilities;

namespace RuneScape.Network
{
    ////////////////////////////////////////////////////////
    /// <summary>
    /// Routes the received data.
    /// </summary>
    /// <param name="data">The data to be routed.</param>
    public delegate void RouteDataCallback(Node node, byte[] data);
    /// <summary>
    /// Disconnects the node.
    /// </summary>
    public delegate void DisconnectCallback(Node node);
    ////////////////////////////////////////////////////////


    /// <summary>
    /// Represents a single node connection to the server.
    /// </summary>
    public class Node
    {
        #region Fields
        /// <summary>
        /// The default buffer size for receiving data.
        /// </summary>
        private const int ReceiveBufferSize = 512;

        /// <summary>
        /// The storage when data is recieved.
        /// </summary>
        private byte[] receiveBuffer;

        /// <summary>
        /// Called when data is asynchronously received from the connection.
        /// </summary>
        private AsyncCallback dataReceived;
        /// <summary>
        /// Called when the data is ready to be routed to a handler.
        /// </summary>
        private RouteDataCallback routeData = new RouteDataCallback((n, data) => { });
        /// <summary>
        /// Called when the node disconnects.
        /// </summary>
        private DisconnectCallback disconnect = new DisconnectCallback((n) => { });
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the node's connection id.
        /// </summary>
        public uint Id { get; private set; }
        /// <summary>
        /// Gets the node's socket connection to the server.
        /// </summary>
        public Socket Socket { get; private set; }
        /// <summary>
        /// Gets the time that the node was created.
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// Gets the age of the connection.
        /// </summary>
        public TimeSpan Age
        {
            get
            {
                TimeSpan span = DateTime.Now - this.CreateTime;
                if (span <= TimeSpan.Zero)
                    return TimeSpan.Zero;
                return span;
            }
        }

        /// <summary>
        /// Gets the user's ip address.
        /// </summary>
        public string IPAddress
        {
            get
            {
                if (this.Socket == null)
                {
                    return "";
                }

                try
                {
                    return this.Socket.RemoteEndPoint.ToString().Split(':')[0];
                }
                catch (Exception ex)
                {
                    Program.Logger.WriteException(ex);
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets the user's full endpoint.
        /// </summary>
        public string EndPoint
        {
            get
            {
                if (this.Socket == null)
                    return "";
                return this.Socket.RemoteEndPoint.ToString();
            }
        }

        /// <summary>
        /// Gets whether the socket is connected.
        /// </summary>
        public bool Connected
        {
            get { return this.Socket.Connected; }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new connection node.
        /// </summary>
        /// <param name="id">The connection id.</param>
        /// <param name="socket">The socket providing connection 
        /// between server and client.</param>
        public Node(uint id, Socket socket)
        {
            this.Id = id;
            this.Socket = socket;
            this.CreateTime = DateTime.Now;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Starts listening for new data.
        /// </summary>
        /// <param name="router">Routes information recieved.</param>
        /// <param name="disconnecter">Action tooken on disconnection.</param>
        public void Start(RouteDataCallback router, DisconnectCallback disconnecter)
        {
            this.receiveBuffer = new byte[Node.ReceiveBufferSize];
            this.dataReceived = new AsyncCallback(OnDataRecieved);
            this.disconnect = disconnecter;
            this.routeData = router;
            StandBy();
        }

        /// <summary>
        /// Stops listening for new data and closes the connection.
        /// </summary>
        public void OnDisconnect()
        {
            this.disconnect(this);
        }

        /// <summary>
        /// Stands by for new incoming data.
        /// </summary>
        private void StandBy()
        {
            try
            {
                this.Socket.BeginReceive(this.receiveBuffer, 0, Node.ReceiveBufferSize, SocketFlags.None, this.dataReceived, null);
            }
            catch (ObjectDisposedException)
            {
                OnDisconnect();
            }
            catch (SocketException se)
            {
                HandleSocketException(se);
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
                OnDisconnect();
            }
        }

        /// <summary>
        /// Handles the data received asynchronously from the socket.
        /// </summary>
        /// <param name="async">The result recieved.</param>
        private void OnDataRecieved(IAsyncResult result)
        {
            try
            {
                int bytesRecieved = this.Socket.EndReceive(result);
                if (bytesRecieved == 0) // Winsock error.
                {
                    throw new ObjectDisposedException("Socket");
                }
                else
                {
                    // Trim and handle data.
                    byte[] data = new byte[bytesRecieved];
                    Buffer.BlockCopy(this.receiveBuffer, 0, data, 0, bytesRecieved);
                    RouteData(this, data);
                    StandBy(); // Wait for new data.
                }
            }
            catch (ObjectDisposedException)
            {
                OnDisconnect();
            }
            catch (SocketException se)
            {
                HandleSocketException(se);
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
                OnDisconnect();
            }
        }

        /// <summary>
        /// Routes the data to an external object.
        /// </summary>
        /// <param name="data">The data to be handled.</param>
        private void RouteData(Node node, byte[] data)
        {
            this.routeData(node, data);
        }

        /// <summary>
        /// Sends a signle byte to the node.
        /// </summary>
        /// <param name="b">The byte to be sent.</param>
        public void SendData(byte b)
        {
            SendData(new byte[] { b }, 0 , 1);
        }

        /// <summary>
        /// Sends a whole array to the node.
        /// </summary>
        /// <param name="data">The data to be sent.</param>
        public void SendData(byte[] data)
        {
            SendData(data, 0, data.Length);
        }

        /// <summary>
        /// Sends a specified amount of thie array to the node.
        /// </summary>
        /// <param name="data">The data to be sent.</param>
        /// <param name="offset">The index of which to start sending from the array.</param>
        /// <param name="length">The amount of send from the offset.</param>
        public void SendData(byte[] data, int offset, int length)
        {
            try
            {
                this.Socket.BeginSend(data, offset, length, SocketFlags.None, new AsyncCallback(DataSent), data);
            }
            catch (ObjectDisposedException)
            {
                OnDisconnect();
            }
            catch (SocketException se)
            {
                HandleSocketException(se);
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
                OnDisconnect();
            }
        }

        /// <summary>
        /// Handles the data sent asynchronously.
        /// </summary>
        /// <param name="result">The results.</param>
        private void DataSent(IAsyncResult result)
        {
            /*try
            {
                int dataSend = this.Socket.EndSend(result);
                Console.WriteLine("SENT: " + dataSend);
            }
            catch (ObjectDisposedException)
            {
                OnDisconnect();
            }
            catch (SocketException se)
            {
                HandleSocketException(se);
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
                OnDisconnect();
            }*/
        }

        /// <summary>
        /// Handles a socket exception.
        /// </summary>
        /// <param name="se">The socket exception to handle.</param>
        private void HandleSocketException(SocketException se)
        {
            if (se.SocketErrorCode == SocketError.ConnectionReset)
            {
            }
            else
            {
                Program.Logger.WriteException(se);
            }
            OnDisconnect();
        }
        #endregion Methods
    }
}
