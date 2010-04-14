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
using System.Threading.Tasks;

using RuneScape.Communication.Messages;
using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Characters;
using RuneScape.Network;

namespace RuneScape.Communication
{
    /// <summary>
    /// Represents a single session on the server.
    /// </summary>
    public class GameSession : IDisposable
    {
        #region Fields
        /// <summary>
        /// A asynchronous task handling recieved data.
        /// </summary>
        private Task handleTask = new Task(new Action(delegate() { }));
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the session's connection instance.
        /// </summary>
        public Node Connection { get; private set; }
        /// <summary>
        /// Gets the session's character.
        /// </summary>
        public Character Character { get; set; }

        /// <summary>
        /// The session's client key.
        /// </summary>
        public long ClientKey { get; private set; }
        /// <summary>
        /// The session's server key.
        /// </summary>
        public long ServerKey { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new game session.
        /// </summary>
        /// <param name="id">The session id created by RuneScape.Network.Tcp.TcpConnectionFactory instance.</param>
        public GameSession(Node node, long clientKey, long serverKey)
        {
            this.Connection = node;
            this.ClientKey = clientKey;
            this.ServerKey = serverKey;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Starts the tcp connection. Routes all data recieved from 
        /// the connection to <code>this.HandleConnection(byte[] data);</code>
        /// </summary>
        public void StartConnection()
        {
            if (this.handleTask != null)
            {
                this.handleTask.Start();
            }
            if (this.Connection != null)
            {
                this.Connection.Start(
                    new RouteDataCallback(HandlePacket),
                    new DisconnectCallback(OnSessionDisconnect));
            }
        }

        /// <summary>
        /// Handles the session disconnection.
        /// </summary>
        public void OnSessionDisconnect(Node node)
        {
            if (this.Connection != null)
            {
                GameEngine.World.CharacterManager.Unregister((short)this.Character.Index);
                GameServer.TcpConnection.DropConnection(this.Connection);
            }
        }

        /// <summary>
        /// Sends a single byte to the node connection.
        /// </summary>
        /// <param name="data">The byte to send.</param>
        public void SendData(byte data)
        {
            Connection.SendData(data);
        }

        /// <summary>
        /// Sends an array of bytes to the node connection.
        /// </summary>
        /// <param name="data">The byte array to send.</param>
        public void SendData(byte[] data)
        {
            Connection.SendData(data);
        }

        /// <summary>
        /// Handles a given amount of data from the connection.
        /// </summary>
        /// <param name="data">A byte array holding data received 
        /// asychronously from the tcp connection.</param>
        public void HandlePacket(Node node, byte[] data)
        {
            try
            {
                Packet buffer = new Packet(-1, data);

                // Sometimes more than 1 packet is sent, so we must try reading them all.
                while (buffer.RemainingAmount > 0)
                {
                    // The packet headers.
                    int opcode = buffer.ReadByte();
                    int length = PacketManager.GetPacketLength(opcode);

                    // The packet length is determined from the packet.
                    if (length == -1)
                    {
                        if (buffer.RemainingAmount > 0)
                        {
                            length = buffer.ReadByte();
                        }
                        else
                        {
                            return;
                        }
                    }

                    // The length is unknown, guess the length by reading the remaining amount.
                    if (length < 0)
                    {
                        length = buffer.RemainingAmount;
                    }

                    // Packet is valid, and can be handled.
                    if (buffer.RemainingAmount >= length)
                    {
                        // Copy the required data for this packet.
                        byte[] payload = new byte[length];
                        buffer.Read(payload);
                        Packet packet = new Packet(opcode, payload);

                        // Handle the packets via a task, so processing is much quicker and more efficient.
                        this.handleTask.ContinueWith((t) => GameEngine.World.PacketManager.Handle(this.Character, packet));
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
        }

        #region IDispose Members
        /// <summary>
        /// Attempts to dispose the session.
        /// </summary>
        public void Dispose()
        {
            this.handleTask.Dispose();
        }
        #endregion IDispose Memebrs
        #endregion Methods
    }
}
