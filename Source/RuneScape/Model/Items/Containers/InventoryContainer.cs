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

using RuneScape.Communication.Messages;
using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Characters;

namespace RuneScape.Model.Items.Containers
{
    /// <summary>
    /// A container for character inventories.
    /// </summary>
    public class InventoryContainer : Container
    {
        #region Fields
        /// <summary>
        /// The size of the inventory container..
        /// 
        ///     <remarks>This value cannot be changed, because 
        ///     this is the maximum amount required for the 
        ///     runescape client.</remarks>
        /// </summary>
        public const byte Size = 28;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a new inventory container.
        /// </summary>
        public InventoryContainer(Character character)
            : base(InventoryContainer.Size, ContainerType.Standard, character)
        {
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Refreshes the inventory items.
        /// </summary>
        public override void Refresh()
        {
            this.character.Session.SendData(new InterfaceItemsPacketComposer(149, 0, 93, this).Serialize());
        }

        /// <summary>
        /// Attempts to add an item to the container.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <returns>Returns true if added successfully; false if not.</returns>
        public bool AddItem(Item item)
        {
            if (!AddInternal(item))
            {
                character.Session.SendData(new MessagePacketComposer("Not enough space in your inventory.").Serialize());
                return false;
            }
            Refresh();
            return true;
        }

        /// <summary>
        /// Deletes the specified item from the container.
        /// </summary>
        /// <param name="item">The item to delete.</param>
        public void DeleteItem(Item item)
        {
            RemoveInternal(item);
            Refresh();
        }

        /// <summary>
        /// Deletes all items that matches the id of the specified item object.
        /// </summary>
        /// <param name="item">The item to delete.</param>
        public void DeleteAllItems(Item item)
        {
            RemoveAllInternal(item);
            Refresh();
        }

        /// <summary>
        /// Resets the inventory container. Shows nothing after this call is made.
        /// </summary>
        public void Reset()
        {
            ResetInternal();
            Refresh();
        }
        #endregion Methods
    }
}
