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
        /// Gets or sets whether the bank is currently in noting mode.
        /// </summary>
        public bool Noting { get; set; }
        /// <summary>
        /// Gets the current tab id.
        /// </summary>
        public byte CurrentTab { get; set; }
        /// <summary>
        /// Gets the tab index (item id value).
        /// </summary>
        public short[] TabIndex { get; private set; }
        #endregion Proeprties

        #region Constructors
        /// <summary>
        /// Constructs a new bank container.
        /// </summary>
        public BankContainer(Character character)
            : base(BankContainer.Size, ContainerType.AlwaysStack, character)
        {
            this.TabIndex = new short[11];
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Refreshes the bank.
        /// </summary>
        public override void Refresh()
        {
            this.Character.Session.SendData(new InterfaceItemsPacketComposer(-1, 1, 95, this).Serialize());
            this.Character.Session.SendData(new InterfaceItemsPacketComposer(-1, 1, 93, this.Character.Inventory).Serialize());
            this.Character.Session.SendData(new StringPacketComposer(this.TakenSlots.ToString(), 762, 97).Serialize());
            this.Character.Session.SendData(new StringPacketComposer(BankContainer.Size.ToString(), 762, 98).Serialize());
        }

        /// <summary>
        /// Sets up the tabs in the characer's bank.
        /// </summary>
        public void ConfigureTabs()
        {
            int config = 0;
            config += TabIndex[3] - TabIndex[2]; //Sets the starting slot for the second tab
            config += (TabIndex[4] - TabIndex[3]) * 1024; // *1024 allows us to set the 3rd tab
            config += (TabIndex[5] - TabIndex[4]) * 1048576; // *1048576 allows us to set the 4th tab
            this.Character.Session.SendData(new ConfigPacketComposer(1246, config).Serialize());
            config = 0;
            config += TabIndex[6] - TabIndex[5]; //Sets the starting slot for the 5th tab
            config += (TabIndex[7] - TabIndex[6]) * 1024; //*1024 allows us to set the 6th tab
            config += (TabIndex[8] - TabIndex[7]) * 1048576; //*1048576 allows us to set the 7th tab
            this.Character.Session.SendData(new ConfigPacketComposer(1247, config).Serialize());
            config = -2013265920;
            config += (TabIndex[9] - TabIndex[8]); //Sets the starting slot for the 8th tab
            config += (TabIndex[10] - TabIndex[9]) * 1024; //*1024 allows us to set the 9th tab
            this.Character.Session.SendData(new ConfigPacketComposer(1248, config).Serialize());
        }

        /// <summary>
        /// Deposits an item from the character's inventory.
        /// </summary>
        public bool Deposit(byte slot, int count)
        {
            if (slot < 0 || slot > InventoryContainer.Size || count < 1)
            {
                return false;
            }

            Item item = this.Character.Inventory[slot];
            if (slot == null)
            {
                return false;
            }

            int amt = this.Character.Inventory.GetAmount(item);
            if (count > amt)
            {
                item = new Item(item.Id, amt);
            }
            else
            {
                item = new Item(item.Id, count);
            }
            this.Character.Inventory.DeleteItem(item);
            item = new Item(item.Definition.Noted ? item.Definition.NoteId : item.Id, item.Count);

            if (this.ContainsOne(item))
            {
                for (int i = 0; i < BankContainer.Size; i++)
                {
                    Item bankedItem = this[i];
                    if (bankedItem == null)
                    {
                        continue;
                    }
                    if (bankedItem.Id == item.Id)
                    {
                        this[i] = new Item(item.Id, bankedItem.Count + item.Count);
                        break;
                    }
                }
            }
            else
            {
                if (FreeSlots <= 0)
                {
                    this.Character.Session.SendData(new MessagePacketComposer("You do not have enough space in your bank.").Serialize());
                    return false;
                }
                else
                {
                    if (CurrentTab == 10)
                    {
                        this[GetFreeSlot()] = new Item(item.Id, item.Count);
                    }
                    else
                    {
                        int freeSlot = TabIndex[CurrentTab] + (TabIndex[CurrentTab + 1] - TabIndex[CurrentTab]);
                        Insert(GetFreeSlot(), freeSlot);
                        IncrementStartingTabSlot(CurrentTab);
                        ConfigureTabs();
                        CurrentTab = 10;
                        this[GetFreeSlot()] = new Item(item.Id, item.Count);
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
            if (slot < 0 || slot > BankContainer.Size || count <= 0)
            {
                return false;
            }
            Item item = this[slot];
            int tab = GetTabBySlot(slot);
            if (item == null)
            {
                return false;
            }
            item = new Item(item.Id, count > item.Count ? item.Count : count);

            if (Noting)
            {
                if (item.Definition.NoteId == -1)
                {
                    this.Character.Session.SendData(new MessagePacketComposer("You cannot withdraw this item as a note.").Serialize());
                    return false;
                }
            }

            if (count > this.Character.Inventory.FreeSlots && !item.Definition.Stackable &&
                !Noting)
            {
                item = new Item(item.Id, this.Character.Inventory.FreeSlots);
            }

            if (this.Contains(item))
            {
                if (this.Character.Inventory.FreeSlots <= 0)
                {
                    this.Character.Session.SendData(new MessagePacketComposer("Not enough space in inventory.").Serialize());
                    return false;
                }
                else
                {
                    if (Noting)
                    {
                        this.Character.Inventory.AddItem(new Item(item.Definition.NoteId, item.Count));
                        this.RemoveInternal(item);
                    }
                    else
                    {
                        this.Character.Inventory.AddItem(item);
                        this.RemoveInternal(item);
                    }
                    if (this[slot] == null)
                    {
                        DecrementStartingTabSlot(tab);
                        ConfigureTabs();
                    }
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
        /// Increments the starting point where the tab displays an item.
        /// </summary>
        /// <param name="start">The starting slot to increment from.</param>
        public void IncrementStartingTabSlot(int start)
        {
            for (int i = start + 1; i < TabIndex.Length; i++)
            {
                TabIndex[i]++;
            }
        }

        /// <summary>
        /// Decrements the starting point where the tab displays an item.
        /// </summary>
        /// <param name="start">The starting slot to decrement from.</param>
        public void DecrementStartingTabSlot(int start)
        {
            if (start == 10)
            {
                return;
            }
            for (int i = start + 1; i < TabIndex.Length; i++)
            {
                TabIndex[i]--;
            }
            if (TabIndex[start + 1] - TabIndex[start] == 0)
            {
                CollapseTab(start);
            }
        }

        /// <summary>
        /// Collapses the tab and returns the items to the main tab.
        /// </summary>
        /// <param name="tab">The tab index.</param>
        public void CollapseTab(int tab)
        {
            int size = TabIndex[tab + 1] - TabIndex[tab];

            Item[] tmpItems = new Item[size];

            for (int i = 0; i < size; i++)
            {
                tmpItems[i] = this[TabIndex[tab] + i];
                this[TabIndex[tab] + i] = null;
            }
            Sort();
            for (int i = tab; i < TabIndex.Length - 1; i++)
            {
                TabIndex[i] = (short)(TabIndex[i + 1] - size);
            }

            TabIndex[10] = (short)(TabIndex[10] - size);
            ConfigureTabs();
            for (int i = 0; i < size; i++)
            {
                int slot = GetFreeSlot();
                this[slot] = tmpItems[i];
            }
            Refresh();
        }

        /// <summary>
        /// Gets the tab id by the item slot.
        /// </summary>
        /// <param name="slot">The slot of the item.</param>
        /// <returns>Returns the slot id.</returns>
        public int GetTabBySlot(int slot)
        {
            int tab = 0;
            for (int i = 0; i < TabIndex.Length; i++)
            {
                if (slot >= TabIndex[i])
                {
                    tab = i;
                }
            }
            return tab;
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
