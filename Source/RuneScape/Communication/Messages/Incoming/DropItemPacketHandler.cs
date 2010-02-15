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
    /// handler for the drop item packet.
    /// </summary>
    public class DropItemPacketHandler : IPacketHandler
    {
        #region Methods
        /// <summary>
        /// Handles the items dropped by the character.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            packet.Skip(4);
            short slot = packet.ReadLEShortA();
            short itemId = packet.ReadShort();

            if (slot < 0 || slot > InventoryContainer.Size 
                || character.Inventory[slot] == null 
                || character.Inventory[slot].Id != itemId)
            {
                return;
            }

            Item item = character.Inventory[slot];
            character.Inventory[slot] = null;
            character.Inventory.Refresh();
            GameEngine.World.ItemManager.GroundItems.Create(item, character.Location, character);
        }
        #endregion Methods
    }
}
