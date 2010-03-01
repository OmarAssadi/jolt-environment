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

using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Characters;

namespace RuneScape.Model.Items.Containers
{
    /// <summary>
    /// A container for character inventories.
    /// </summary>
    public class BankContainer : Container
    {
        #region Fields
        /// <summary>
        /// The size of the bank container..
        /// 
        ///     <remarks>This value cannot be changed, because 
        ///     this is the maximum amount required for the 
        ///     runescape client.</remarks>
        /// </summary>
        public const int Size = 200;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets or sets whether the bank is currently in inserting/swap mode.
        /// </summary>
        public bool Inserting { get; set; }
        /// <summary>
        /// Gets or sets whether the bank is currently in noting mode.
        /// </summary>
        public bool Noting { get; set; }
        #endregion Proeprties

        #region Constructors
        /// <summary>
        /// Constructs a new bank container.
        /// </summary>
        public BankContainer(Character character)
            : base(BankContainer.Size, ContainerType.AlwaysStack, character)
        {
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Refreshes the bank.
        /// </summary>
        public override void Refresh()
        {
            this.Character.Session.SendData(new InterfaceItemsPacketComposer(-1, unchecked((short)64207), 95, this).Serialize());
            this.Character.Session.SendData(new InterfaceItemsPacketComposer(-1, unchecked((short)64209), 93, this.Character.Inventory).Serialize());
            this.Character.Session.SendData(new StringPacketComposer(this.TakenSlots.ToString(), 762, 97).Serialize());
            this.Character.Session.SendData(new StringPacketComposer(BankContainer.Size.ToString(), 762, 98).Serialize());
        }

        /// <summary>
        /// Deposits an item from the character's inventory.
        /// </summary>
        public bool Deposit(byte slot, int count)
        {
            if (slot < 0 || slot > InventoryContainer.Size || count <= 0)
            {
                return false;
            }

            Item item = this.Character.Inventory[slot];
            if (item == null)
            {
                return false;
            }
            
            if (count < this.Character.Inventory.GetAmount(item))
            {
                item = new Item(item.Id, count);
            }

            if (this.Character.Inventory.Contains(item))
            {
                if (this.Contains(item))
                {
                    for (int i = 0; i < BankContainer.Size; i++)
                    {
                        Item bankItem = this[i];
                        if (bankItem != null && bankItem.Id == item.Id)
                        {
                            this[i] = new Item(item.Definition.Noted ? item.Definition.NoteId : item.Id, bankItem.Count + item.Count);
                            this.Character.Inventory.DeleteItem(item);
                            break;
                        }
                    }
                }
                else
                {
                    if (this.FreeSlots <= 0)
                    {
                        this.Character.Session.SendData(new MessagePacketComposer(
                            "Not enough space in your bank.").Serialize());
                    }
                    else
                    {
                        this.AddInternal(new Item(item.Definition.Noted ? item.Definition.NoteId : item.Id, item.Count));
                        this.Character.Inventory.DeleteItem(item);
                    }
                }
            }
            Refresh();
            return true;
        }

        /// <summary>
        /// Withdraws an item from the character's bank into the inventory container.
        /// </summary>
        /// <param name="slot">The slot in the bank.</param>
        /// <param name="count">The amount of the item in the bank to withdraw.</param>
        /// <returns>Whether or not the item was withdrawn.</returns>
        public bool Withdraw(byte slot, int count)
        {
            if (slot < 0 || slot > InventoryContainer.Size || count <= 0)
            {
                return false;
            }

            Item item = this[slot];
            if (item == null)
            {
                return false;
            }

            if (count < item.Count)
            {
                item = new Item(item.Id, count);
            }

            if (this.Contains(item))
            {
                if (this.Character.Inventory.FreeSlots <= 0)
                {
                    this.Character.Session.SendData(new MessagePacketComposer(
                        "Not enough space in your inventory.").Serialize());
                    return false;
                }
                else
                {
                    if (this.Noting && item.Definition.NoteId != -1)
                    {
                        this.Character.Inventory.AddItem(new Item(item.Definition.NoteId, item.Count));
                    }
                    else
                    {
                        this.Character.Inventory.AddItem(item);
                    }
                    this.RemoveInternal(item);
                    this.Sort();
                }
            }
            Refresh();
            return true;
        }

        /// <summary>
        /// Inserts an item from one slot to another.
        /// </summary>
        /// <param name="fromSlot">The slot to move the item from.</param>
        /// <param name="toSlot">The slot to move the item to.</param>
        public void Insert(int fromSlot, int toSlot)
        {
            Item tmpItem = this.Character.Bank[fromSlot];
            if (toSlot > fromSlot)
            {
                for (int i = fromSlot; i < toSlot; i++)
                {
                    this[i] = this[i + 1];
                }
            }
            else if (fromSlot > toSlot)
            {
                for (int i = fromSlot; i > toSlot; i--)
                {
                    this[i] = this[i - 1];
                }
            }
            this[toSlot] = tmpItem;
            Refresh();
        }

        /// <summary>
        /// Examines an item within the character's bank.
        /// </summary>
        /// <param name="slot">The slot of the bank to examine.</param>
        public void Examine(byte slot)
        {
            if (slot < 0 || slot > BankContainer.Size || this[slot] == null)
            {
                return;
            }
            this.Character.Session.SendData(new MessagePacketComposer(this[slot].Definition.Examine).Serialize());
        }
        #endregion Methods
    }
}
