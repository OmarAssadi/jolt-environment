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
    /// Composes a packet that recieves a private message.
    /// 
    /// <example>From Johndoe: Hey there!</example>
    /// </summary>
    public class RecievePMPacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs a recieve private message packet.
        /// </summary>
        /// <param name="name">The name of the character, the message is being recieved from.</param>
        /// <param name="rights">The rights of the character, the message is being recieved from.</param>
        /// <param name="message">The message recieved.</param>
        public RecievePMPacketComposer(long name, byte rights, string message, int count)
        {
            byte[] bytes = new byte[message.Length + 1];
            bytes[0] = (byte)message.Length;
            ChatUtilities.EncryptChat(bytes, 0, 1, message.Length, Encoding.ASCII.GetBytes(message));

            SetOpcode(178);
            SetType(PacketType.Byte);
            AppendLong(name);
            AppendShort(1);
            AppendBytes(new byte[] { (byte)((count << 16) & 0xFF), (byte)((count << 8) & 0xFF), (byte)(count & 0xFF) });
            AppendByte((byte)rights);
            AppendBytes(bytes);
        }
        #endregion Constructors
    }
}
