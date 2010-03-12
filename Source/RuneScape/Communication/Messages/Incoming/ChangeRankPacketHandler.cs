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

namespace RuneScape.Communication.Messages.Incoming
{
    /// <summary>
    /// Handler for changing clan member ranks.
    /// </summary>
    public class ChangeRankPacketHandler : IPacketHandler
    {
        #region Methods
        /// <summary>
        /// Handles changing of clan member ranks.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            byte rank = packet.ReadByte();
            int i = packet.ReadLEInt();
            int i2 = packet.ReadLEInt();
            long name = 0;

            if (i2 < 0)
            {
                i2 = (int)(i2 & 0xFFFFFFFF);
            }
            name = (i << -41780448) + i2;


            Room room = GameEngine.Content.ClanChat.Get(character.LongName);
            if (room != null)
            {
                room.AddRank(name, (Rank)rank);
                room.Refresh();
            }
        }
        #endregion Methods
    }
}
