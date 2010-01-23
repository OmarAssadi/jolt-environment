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
using RuneScape.Model.Items;
using RuneScape.Model.Items.Containers;

namespace RuneScape.Communication.Messages.Incoming
{
    /// <summary>
    /// Handler for items moved on an interface.
    /// </summary>
    public class SwapItemPacketHandler : IPacketHandler
    {
        #region Methods
        /// <summary>
        /// Handles the items moved on an interface.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            short toSlot = packet.ReadLEShortA();
            packet.Skip(1);
            short fromSlot = packet.ReadLEShortA();
            packet.Skip(2);
            byte interfaceId = packet.ReadByte();
            packet.Skip(1);

            switch (interfaceId)
            {
                /*
                 * Item moved on inventory.
                 */
                case 149:
                    {
                        if (fromSlot < 0 || fromSlot >= InventoryContainer.Size 
                            || toSlot < 0 || toSlot >= InventoryContainer.Size)
                        {
                            break;
                        }

                        Item tmpItem = character.Inventory[fromSlot];
                        Item tmpItem2 = character.Inventory[toSlot];
                        character.Inventory[fromSlot] = tmpItem2;
                        character.Inventory[toSlot] = tmpItem;
                        break;
                    }
            }
        }
        #endregion Methods
    }
}
