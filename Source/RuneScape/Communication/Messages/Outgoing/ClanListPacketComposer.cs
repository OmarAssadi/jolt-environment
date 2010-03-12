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

using RuneScape.Content.ClanChat;
using RuneScape.Model.Characters;

namespace RuneScape.Communication.Messages.Outgoing
{
    /// <summary>
    /// Composes a packet that sends the character's clan mates.
    /// </summary>
    public class ClanListPacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs the packet.
        /// </summary>
        /// <param name="room">The room of clan mates.</param>
        public ClanListPacketComposer(Room room)
        {
            SetOpcode(82);
            SetType(PacketType.Short);

            AppendLong(room.Owner);
            AppendLong(room.Name);
            AppendByte((byte)(room.KickReq - 1));
            AppendByte(room.UserCount);

            lock (room.Users)
            {
                room.Users.ForEach((l) =>
                {
                    AppendLong(l);
                    AppendShort((short)GameEngine.World.Id);
                    AppendByte((byte)room.GetRank(l));
                    AppendString("World " + GameEngine.World.Id);
                });
            }
        }

        /// <summary>
        /// Constructs the packet.
        /// </summary>
        public ClanListPacketComposer()
        {
            SetOpcode(82);
            SetType(PacketType.Short);
            AppendLong(0);
        }
        #endregion Constructors
    }
}
