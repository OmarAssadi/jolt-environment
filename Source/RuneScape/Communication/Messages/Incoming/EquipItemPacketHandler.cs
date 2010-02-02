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
using RuneScape.Model.Items.Containers;
using RuneScape.Model.Items;

namespace RuneScape.Communication.Messages.Incoming
{
    /// <summary>
    /// Handler for item equiping.
    /// </summary>
    public class EquipItemPacketHandler : IPacketHandler
    {
        #region Methods
        /// <summary>
        /// Handles the equiping of items.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            packet.Skip(4);
            short wearId = packet.ReadLEShort();
            short index = packet.ReadLEShort();

            if (index < 0 || index >= InventoryContainer.Size)
            {
                return;
            }

            Item item = character.Inventory[index];

            if (item == null)
            {
                return;
            }

            if (item.Definition.Id == wearId)
            {
                /*sbyte targetSlot = EquipmentContainer.GetItemType(wearId);
                if (targetSlot == -1)
                {
                    return;
                }
                character.Inventory[index] = null;
                character.Inventory.Refresh();
                if (targetSlot == 3)
                {
                    if (EquipmentContainer.TwoHanded(item.Definition) &&
                        character.Equipment[5] != null)
                    {
                        if (!character.Inventory.AddItem(new Item(character.Equipment[5].Id,
                            character.Equipment[5].Count)))
                        {
                            character.Inventory.AddItem(new Item(wearId, item.Count));
                            return;
                        }
                        character.Equipment[5] = null;
                        character.Equipment.Refresh();
                    }
                }
                else if (targetSlot == 5)
                {
                    if (EquipmentContainer.TwoHanded(item.Definition) &&
                        character.Equipment[3] != null)
                    {
                        if (!character.Inventory.AddItem(new Item(character.Equipment[3].Id,
                            character.Equipment[3].Count)))
                        {
                            character.Inventory.AddItem(new Item(wearId, item.Count));
                            return;
                        }
                        character.Equipment[3] = null;
                        character.Equipment.Refresh();
                    }
                }
                if (character.Equipment[targetSlot] != null &&
                    (wearId != character.Equipment[targetSlot].Definition.Id
                    || !item.Definition.Stackable))
                {
                    character.Inventory.AddItem(new Item(character.Equipment[targetSlot].Definition.Id,
                        character.Equipment[targetSlot].Count));
                    character.Equipment[targetSlot] = null;
                    character.Equipment.Refresh();
                }
                int oldAmount = 0;
                if (character.Equipment[targetSlot] != null)
                {
                    oldAmount = character.Equipment[targetSlot].Count;
                }
                Item equipment = new Item(wearId, oldAmount + item.Count);
                character.Equipment[targetSlot] = equipment;
                character.Equipment.Refresh();*/
            }
        }
        #endregion Methods
    }
}
