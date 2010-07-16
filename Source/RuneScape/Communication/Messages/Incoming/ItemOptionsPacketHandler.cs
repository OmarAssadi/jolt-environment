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

using RuneScape.Content.Interfaces;
using RuneScape.Model.Characters;

namespace RuneScape.Communication.Messages.Incoming
{
    /// <summary>
    /// Handler for the hd notification packet.
    /// </summary>
    public class ItemOptionsPacketHandler : IPacketHandler
    {
        #region Methods
        /// <summary>
        /// Handles the item options.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            switch (packet.Opcode)
            {
                case 203:
                    {
                        HandleOption1(character, packet);
                        break;
                    }
            }
        }

        /// <summary>
        /// Handles the first option.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        private void HandleOption1(Character character, Packet packet)
        {
            short slot = packet.ReadLEShortA();
            short interfaceId = packet.ReadShort();
            packet.Skip(2);
            short itemId = packet.ReadShort();

            if (slot < 0 || itemId < 0)
            {
                return;
            }

            switch (interfaceId)
            {
                /*
                 * Unequip item (Equipment tab).
                 */
                case EquipmentTab.InterfaceId:
                    {
                        if (character.Equipment[slot] != null)
                        {
                            if (character.Inventory.AddItem(character.Equipment[slot]))
                            {
                                character.Equipment.Set(slot, null);
                            }
                        }
                        break;
                    }
                default:
                    {
                        Program.Logger.WriteDebug("Unhandled interface: " + interfaceId);
                        break;
                    }
            }
        }
        #endregion Methods
    }
}
