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

using RuneScape.Model.Characters;

namespace RuneScape.Communication.Messages.Outgoing
{
    /// <summary>
    /// Composes a new map region.
    /// </summary>
    public class NewMapRegionPacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs a new map region packet.
        /// </summary>
        /// <param name="character">The character to compose packet for.</param>
        public NewMapRegionPacketComposer(Character character)
            : base()
        {
            if (!ValidateRegion(character))
            {
                character.Session.SendData(new MessagePacketComposer(
                    "This area is not availible. Teleporting to spawn point.").Serialize());
                character.Location = GameEngine.World.SpawnPoint;
            }

            SetOpcode(142);
            SetType(PacketType.Short);
            bool forceSend = true;
            if ((((character.Location.RegionX / 8) == 48) || ((character.Location.RegionX / 8) == 49)) && ((character.Location.RegionY / 8) == 48))
                forceSend = false;
            if (((character.Location.RegionX / 8) == 48) && ((character.Location.RegionY / 8) == 148))
                forceSend = false;
            AppendShortA(character.Location.RegionX);
            AppendLEShortA(character.Location.LocalY);
            AppendShortA(character.Location.LocalX);

            for (int xCalc = (character.Location.RegionX - 6) / 8; xCalc <= ((character.Location.RegionX + 6) / 8); xCalc++)
            {
                for (int yCalc = (character.Location.RegionY - 6) / 8; yCalc <= ((character.Location.RegionY + 6) / 8); yCalc++)
                {
                    int region = yCalc + (xCalc << 8); // 1786653352
                    if (forceSend || ((yCalc != 49) && (yCalc != 149) && (yCalc != 147) && (xCalc != 50) && ((xCalc != 49) || (yCalc != 47))))
                    {
                        int[] mapData = GameEngine.World.MapManager.GetMapData(region);
                        if (mapData == null)
                        {
                            mapData = new int[4];
                            for (int i = 0; i < 4; i++)
                                mapData[i] = 0;
                        }
                        AppendInt(mapData[0]);
                        AppendInt(mapData[1]);
                        AppendInt(mapData[2]);
                        AppendInt(mapData[3]);
                    }
                }
            }
            AppendByteC(character.Location.Z);
            AppendShort(character.Location.RegionY);
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Validates the character's current map region.
        /// </summary>
        /// <param name="character">The character to validate.</param>
        /// <returns>True is the map region if valid; false if the region is not valid.</returns>
        private bool ValidateRegion(Character character)
        {
            bool hasMapData = true;
            bool forceSend = true;
            if ((((character.Location.RegionX / 8) == 48) || ((character.Location.RegionX / 8) == 49)) && ((character.Location.RegionY / 8) == 48))
            {
                forceSend = false;
            }
            if (((character.Location.RegionX / 8) == 48) && ((character.Location.RegionY / 8) == 148))
            {
                forceSend = false;
            }
            for (int xCalc = (character.Location.RegionX - 6) / 8; xCalc <= ((character.Location.RegionX + 6) / 8); xCalc++)
            {
                for (int yCalc = (character.Location.RegionY - 6) / 8; yCalc <= ((character.Location.RegionY + 6) / 8); yCalc++)
                {
                    int region = yCalc + (xCalc << 8); // 1786653352
                    if (forceSend || ((yCalc != 49) && (yCalc != 149) && (yCalc != 147) && (xCalc != 50) && ((xCalc != 49) || (yCalc != 47))))
                    {
                        int[] mapData = GameEngine.World.MapManager.GetMapData(region);
                        if (mapData == null)
                        {
                            hasMapData = false;
                        }
                    }
                }
            }
            return hasMapData;
        }
        #endregion Methods
    }
}
