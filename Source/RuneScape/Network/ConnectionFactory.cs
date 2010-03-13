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

using System.Net.Sockets;

namespace RuneScape.Network
{
    /// <summary>
    /// Represents a factory providing creations of connection nodes.
    /// </summary>
    public class ConnectionFactory
    {
        #region Properties
        /// <summary>
        /// Gets the current connection count. 
        /// 
        ///     <para>This value is imcremented every time a connection is recieved.</para>
        /// </summary>
        public uint Count { get; private set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a new node connection instance specified by the socket.
        /// </summary>
        /// <param name="socket">The System.Net.Sockets.Socket object containing
        /// the connection between the server and endpoint.</param>
        /// <returns>Returns a news RuneScape.Network.Node object.</returns>
        public Node Create(Socket socket)
        {
            if (socket == null)
                return null;

            Node node = new Node(++this.Count, socket);
            Program.Logger.WriteDebug("New connection [id=" + node.Id + "] created for " + node.EndPoint + ".");
            return node;
        }
        #endregion Methods
    }
}
