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

namespace RuneScape.Communication.Messages.Incoming
{
    /// <summary>
    /// Handler for the walking packet(s).
    /// </summary>
    public class WalkingPacketHandler : IPacketHandler
    {
        #region Methods
        /// <summary>
        /// Handles the walking requested by the character.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            try
            {
                // This will close the open interface.
                // TODO: remove skill menu
                // TODO: remove trade screen
                if (character.Preferences["interface"] != null)
                {
                    short interfaceId = (short)character.Preferences["interface"];

                    // Close interface first.
                    //Frames.SendCloseInterface(character);


                    // Call any extra requirements.
                    switch (interfaceId)
                    {
                        // Bank interface
                        case 762:
                            Frames.SendRestoreInventory(character);
                            Frames.SendTabs(character);
                            //Frames.SendCloseInventoryInterface(character);
                            break;
                        case 667:
                            Frames.SendRestoreInventory(character);
                            Frames.SendTabs(character);
                            //Frames.SendCloseInventoryInterface(character);
                            break;
                    }
                    Frames.SendCloseInterface(character);
                }

                // Walking via minimap.
                int length = packet.Length;
                if (packet.Opcode == 119)
                {
                    length -= 14;
                }

                /* 
                 * The character's walking queue needs to be reset, otherwise 
                 * the character will have to finish the older request (if any) 
                 * before it continues with the new request.
                 */
                character.WalkingQueue.Reset();

                int steps = (length - 5) / 2;
                int[,] path = new int[steps, 2];

                int firstX = packet.ReadLEShortA() - (character.Location.RegionX - 6) * 8;
                int firstY = packet.ReadShortA() - (character.Location.RegionY - 6) * 8;
                bool running = packet.ReadByteC() == 1;

                character.WalkingQueue.Running = running;
                character.WalkingQueue.AddStep(firstX, firstY);

                for (int i = 0; i < steps; i++)
                {
                    path[i, 0] = unchecked((sbyte)packet.ReadByte()) + firstX;
                    path[i, 1] = unchecked((sbyte)packet.ReadByteS()) + firstY;
                    character.WalkingQueue.AddStep(path[i, 0], path[i, 1]);
                }
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
        }
        #endregion Methods
    }
}
