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

using RuneScape.Communication.Messages.Outgoing;

namespace RuneScape.Communication.Login.Handlers
{
    /// <summary>
    /// Handles the session request protocol.
    /// 
    ///     <para>When a character enters his/her login information 
    ///     in the client and clicks login, the client will request 
    ///     a randomly generated session key that the server and 
    ///     client will be indexed with.</para>
    /// </summary>
    public class SessionRequestProtocolHandler : IProtocolHandler
    {
        #region Fields
        /// <summary>
        /// The protocol id of this handle.
        /// </summary>
        public const int ProtocolId = 14;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the protocol request.
        /// </summary>
        /// <param name="request">The instance requesting the protcol handle.</param>
        public void Handle(LoginRequest request)
        {
            request.Buffer.Skip(1);
            int nameHash = request.Buffer.ReadByte();
            long key = ((long)(new Random().NextDouble() * 99999999D) << 32) + (long)(new Random().NextDouble() * 99999999D);
            GenericPacketComposer session = new GenericPacketComposer();
            session.AppendByte(0);
            session.AppendLong(key);
            request.Buffer = null; // Dump current buffer.
            request.NameHash = nameHash; // Save name hash.
            request.Connection.SendData(session.Serialize());
        }
        #endregion Methods
    }
}
