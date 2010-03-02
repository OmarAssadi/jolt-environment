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

namespace RuneScape.Communication.Messages.Outgoing
{
    /// <summary>
    /// Composes packet that updates a friends status (online/offline, world).
    /// </summary>
    public class UpdateFriendPacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs a update friend packet.
        /// </summary>
        /// <param name="name">The character's name as a long.</param>
        /// <param name="world">The character's world. 0 if not online.</param>
        public UpdateFriendPacketComposer(long name, short world)
        {
            SetOpcode(154);
            SetType(PacketType.Byte);
            AppendLong(name);
            AppendShort(world);
            AppendByte(1);

            if (world != 0)
            {
                AppendString("Online");
            }
        }
        #endregion Constructors
    }
}
