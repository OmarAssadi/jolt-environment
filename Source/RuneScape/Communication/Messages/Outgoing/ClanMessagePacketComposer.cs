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

namespace RuneScape.Communication.Messages.Outgoing
{
    /// <summary>
    /// Composes a packet that sends a clan message to the client.
    /// </summary>
    public class ClanMessagePacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs the packet.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public ClanMessagePacketComposer(string message, long playerName, long clanName, int messageCount, byte rights)
        {
            SetOpcode(229);
            SetType(PacketType.Byte);
            AppendLong(playerName);
            AppendByte(1);
            AppendLong(clanName);
            AppendShort(1);

            byte[] bytes = new byte[message.Length + 1];
            bytes[0] = (byte)message.Length;
            ChatUtilities.EncryptChat(bytes, 0, 1, message.Length, Encoding.ASCII.GetBytes(message));

            AppendByte((byte)((messageCount << 16) & 0xFF));
            AppendByte((byte)((messageCount << 8) & 0xFF));
            AppendByte((byte)(messageCount & 0xFF));
            AppendByte(rights);
            AppendBytes(bytes);
        }
        #endregion Constructors
    }
}
