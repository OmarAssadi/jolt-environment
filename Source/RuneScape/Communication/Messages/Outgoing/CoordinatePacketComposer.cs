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

using RuneScape.Model;
using RuneScape.Model.Characters;

namespace RuneScape.Communication.Messages.Outgoing
{
    /// <summary>
    /// Composes a packet which indicates the client where to place the following packet.
    /// </summary>
    public class CoordinatePacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs a new coordinate packet.
        /// </summary>
        /// <param name="location">The location of the coordinate.</param>
        public CoordinatePacketComposer(Character character, Location location)
        {
            SetOpcode(177);

            int regionX = character.UpdateFlags.LastRegion.RegionX;
            int regionY = character.UpdateFlags.LastRegion.RegionY;

            AppendByte((byte)(location.Y - ((regionY - 6) * 8)));
            AppendByteS((byte)(location.X - ((regionX - 6) * 8)));
        }

        /// <summary>
        /// Constructs a new coordinate packet specified offsets.
        /// </summary>
        /// <param name="location">The location of the coordinate.</param>
        public CoordinatePacketComposer(Character character, Location location, int xOffset, int yOffset)
        {
            SetOpcode(177);

            int regionX = character.UpdateFlags.LastRegion.RegionX;
            int regionY = character.UpdateFlags.LastRegion.RegionY;

            AppendByte((byte)(location.Y - ((regionY - 6) * 8) + yOffset));
            AppendByteS((byte)(location.X - ((regionX - 6) * 8) + xOffset));
        }
        #endregion Constructors
    }
}
