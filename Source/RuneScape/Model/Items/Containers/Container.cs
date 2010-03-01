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

namespace RuneScape.Model.Items.Containers
{
    /// <summary>
    /// Represents an abstract container for items.
    /// </summary>
    public abstract class Container
    {
        #region Fields
        /// <summary>
        /// The storage for items this container holds.
        /// </summary>
        private Item[] items;
        /// <summary>
        /// The storage's final capacity.
        /// </summary>
        private readonly short capacity;
        /// <summary>
        /// The container type. 
        /// 
        ///     <para>Determines whether the container will 
        ///     act with a certain condition.</para>
        /// </summary>
        private ContainerType type;
        /// <summary>
        /// Provides a lock.
        /// </summary>
        private object obj = new object();
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the character that owns this container.
        /// </summary>
        public Character Character { get; private set; }

        /// <summary>
        /// Gets the item specified by the index.
        /// </summary>
        /// <param name="index">The index within the container.</param>
        /// <returns>Returns a RuneScape.Models.Items.Item object 
        /// containing data of the specified item.</returns>
        public Item this[int index]
        {
            get { lock (this.obj) { return this.items[index]; } }
            set { lock (this.obj) { this.items[index] = value; } }
        }

        /// <summary>
        /// Gets the containers capacity.
        /// </summary>
        public short Capacity
        {
            get { return this.capacity; }
        }

        /// <summary>
        /// Gets the amount of free slots.
        /// </summary>
        public int FreeSlots
        {
            get
            {
                lock (this.obj)
                {
                    int slots = 0;
                    for (int i = 0; i < this.capacity; i++)
                    {
                        if (this.items[i] == null)
                        {
                            slots++;
                        }
                    }
                    return slots;
                }
            }
        }

        /// <summary>
        /// Gets the amount of taken slots.
        /// </summary>
        public int TakenSlots
        {
            get
            {
                lock (this.obj)
                {
                    int slots = 0;
                    for (int i = 0; i < this.capacity; i++)
                    {
                        if (this.items[i] != null)
                        {
                            slots++;
                        }
                    }
                    return slots;
                }
            }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a container with specified coniditions and capacity.
        /// </summary>
        /// <param name="type">The container's condition type.</param>
        /// <param name="capacity">The capacity of the container. 
        ///     <remarks>This value cannot be changed.</remarks></param>
        protected Container(short capacity, ContainerType type, Character character)
        {
            this.Character = character;
            this.items = new Item[capacity];
            this.capacity = capacity;
            this.type = type;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Sorts the items in container.
        /// </summary>
        protected void Sort()
        {
            lock (this.obj)
            {
                Item[] oldItems = this.items;
                Item[] newItems = new Item[this.capacity];
                int index = 0;

                // Loop though old items to check for items, then sort them into new array.
                for (int i = 0; i < this.capacity; i++)
                {
                    if (oldItems[i] != null)
                    {
                        newItems[index++] = oldItems[i];
                    }
                }
                this.items = newItems;
            }
        }

        /// <summary>
        /// Adds an item to the container.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>Returns true if successfully added; false if not.</returns>
        protected bool AddInternal(Item item)
        {
            lock (this.obj)
            {
                // The item is stack / noted.
                if (this.type == ContainerType.AlwaysStack || item.Definition.Stackable || item.Definition.Noted)
                {
                    for (int i = 0; i < this.capacity; i++)
                    {
                        if (this[i] != null && this[i].Id == item.Id)
                        {
                            uint total = (uint)(this[i].Count + item.Count);
                            if (total > int.MaxValue)
                            {
                                return false;
                            }
                            this[i] = new Item(item.Definition.Id, (int)total);
                            return true;
                        }
                    }
                }

                // The item is not stacked / noted.
                else
                {
                    if (item.Count > 1)
                    {
                        if (FreeSlots >= item.Count)
                        {
                            for (int i = 0; i < item.Count; i++)
                            {
                                this[GetFreeSlot()] = new Item(item.Id);
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                int slot = GetFreeSlot();
                if (slot == -1)
                {
                    return false;
                }
                else
                {
                    this[slot] = item;
                    return true;
                }
            }
        }

        /// <summary>
        /// Adds a whole container to this container.
        /// </summary>
        /// <param name="container">The container to add.</param>
        protected void AddAllInternal(Container container)
        {
            lock (this.obj)
            {
                for (int i = 0; i < container.capacity; i++)
                {
                    Item item = container[i];
                    if (item != null)
                    {
                        AddInternal(item);
                    }
                }
            }
        }

        /// <summary>
        /// Removess an item from the container.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>Returns the number of items removed.</returns>
        protected int RemoveInternal(Item item)
        {
            lock (this.obj)
            {
                int removed = 0;
                int toRemove = item.Count;

                for (int i = 0; i < this.capacity; i++)
                {
                    if (this[i] != null && this[i].Id == item.Id)
                    {
                        int count = this[i].Count;
                        if (count > toRemove)
                        {
                            removed += toRemove;
                            count -= toRemove;
                            toRemove = 0;
                            this[i] = new Item(item.Id, count);
                            return removed;
                        }
                        else
                        {
                            removed += count;
                            toRemove -= count;
                            this[i] = null;
                        }
                    }
                }
                return removed;
            }
        }

        /// <summary>
        /// Removes all items of the specified type.
        /// </summary>
        /// <param name="item">The item that should be completely removed from the container.</param>
        /// <returns>Returns the number of items removed.</returns>
        protected int RemoveAllInternal(Item item)
        {
            lock (this.obj)
            {
                int deleted = 0;
                for (int i = 0; i < this.capacity; i++)
                {
                    if (this[i] != null)
                    {
                        if (this[i].Id == item.Id)
                        {
                            deleted += this[i].Count;
                            this[i] = null;
                        }
                    }
                }
                return deleted;
            }
        }

        /// <summary>
        /// Removed an item specified with a preferred slot.
        /// </summary>
        /// <param name="preferredSlot">The preferred slot of item removal.</param>
        /// <param name="item">The item to remove.</param>
        /// <returns>Returns the number of items removed.</returns>
        protected int RemoveInternal(int preferredSlot, Item item)
        {
            lock (this.obj)
            {
                int removed = 0;
                int toRemove = item.Count;

                if (this[preferredSlot] != null)
                {
                    if (this[preferredSlot].Id == item.Id)
                    {
                        int count = this[preferredSlot].Count;

                        if (count > toRemove)
                        {
                            removed += toRemove;
                            count -= toRemove;
                            toRemove = 0;
                            this[preferredSlot] = new Item(item.Id, count);
                            return removed;
                        }
                        else
                        {
                            removed += count;
                            toRemove -= count;
                            this[preferredSlot] = null;
                        }
                    }
                }
                for (int i = 0; i < this.capacity; i++)
                {
                    if (this[i] != null)
                    {
                        if (this[i].Id == item.Id)
                        {
                            int count = this[i].Count;

                            if (count > toRemove)
                            {
                                removed += toRemove;
                                count -= toRemove;
                                toRemove = 0;
                                this[i] = new Item(item.Id, count);
                                return removed;
                            }
                            else
                            {
                                removed += count;
                                toRemove -= count;
                                this[i] = null;
                            }
                        }
                    }
                }
                return removed;
            }
        }

        /// <summary>
        /// Checks to see if the container contains atleast one count of the specified item.
        /// </summary>
        /// <param name="item">The item to check for.</param>
        /// <returns>Returns true if contained; false if not.</returns>
        public bool ContainsOne(Item item)
        {
            lock (this.obj)
            {
                for (int i = 0; i < this.capacity; i++)
                {
                    if (this[i] != null)
                    {
                        if (this[i].Id == item.Id)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Checks to see if the container contains the amount of items specified.
        /// </summary>
        /// <param name="item">The item to check for.</param>
        /// <returns>Returns true if contained; false if not.</returns>
        public bool Contains(Item item)
        {
            lock (this.obj)
            {
                int amount = 0;
                for (int i = 0; i < this.capacity; i++)
                {
                    if (this[i] != null)
                    {
                        if (this[i].Id == item.Id)
                        {
                            amount += this[i].Count;
                        }
                    }
                }
                return amount >= item.Count;
            }
        }

        /// <summary>
        /// Checks the container and counts all the items matching the one specified.
        /// </summary>
        /// <param name="item">The item to check in container.</param>
        /// <returns>Returns the amount of this item in the container.</returns>
        public int GetAmount(Item item)
        {
            lock (this.obj)
            {
                int count = 0;
                for (int i = 0; i < this.capacity; i++)
                {
                    if (this[i] != null)
                    {
                        if (this[i].Id == item.Id)
                        {
                            count += this[i].Count;
                        }
                    }
                }
                return count;
            }
        }

        /// <summary>
        /// Gets an item from the container, specified by the id.
        /// </summary>
        /// <param name="id">The id of the item to look for.</param>
        /// <returns>Returns a RuneScape.Models.Items.Item object 
        /// containing data of the specified item id.</returns>
        public Item GetById(int id)
        {
            lock (this.obj)
            {
                for (int i = 0; i < this.capacity; i++)
                {
                    if (this[i] == null)
                    {
                        continue;
                    }
                    if (this[i].Id == id)
                    {
                        return this[i];
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Gets an item from the container, specified by the id.
        /// </summary>
        /// <param name="id">The id of the item to look for.</param>
        /// <returns>The slot id at which the item is located.</returns>
        public int GetBySlot(int id)
        {
            lock (this.obj)
            {
                for (int i = 0; i < this.capacity; i++)
                {
                    if (this[i] == null)
                    {
                        continue;
                    }
                    if (this[i].Id == id)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// Attemppts to find a free slot in the container.
        /// </summary>
        /// <returns>Returns the availible slot id; -1 if none.</returns>
        public int GetFreeSlot()
        {
            lock (this.obj)
            {
                for (int i = 0; i < this.capacity; i++)
                {
                    if (this.items[i] == null)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// Checks if this container has space to add in the specified container.
        /// </summary>
        /// <param name="container">The container to check.</param>
        /// <returns>Returns true if there's space; false if not.</returns>
        public bool HasSpace(Container container)
        {
            lock (this.obj)
            {
                for (int i = 0; i < container.capacity; i++)
                {
                    Item item = container[i];
                    if (item != null)
                    {
                        if (!HasSpace(item))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Checks if this container has space to add in the specified item.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>Returns true if there's space; false if not.</returns>
        public bool HasSpace(Item item)
        {
            if (this.type == ContainerType.AlwaysStack || item.Definition.Stackable || item.Definition.Noted)
            {
                lock (this.obj)
                {
                    for (int i = 0; i < this.capacity; i++)
                    {
                        if (this[i] != null)
                        {
                            if (this[i].Id == item.Id)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (item.Count > 1)
                {
                    if (this.FreeSlots >= item.Count)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            int index = GetFreeSlot();
            if (index == -1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Serializes the current container.
        /// </summary>
        /// <returns>Returns a string representing the serialized query.</returns>
        public string Serialize()
        {
            if (this.TakenSlots > 0)
            {
                string query = string.Empty;

                for (int i = 0; i < this.Capacity; i++)
                {
                    if (this[i] != null && this[i].Count > 0)
                    {
                        if (query != string.Empty)
                        {
                            query += ",";
                        }
                        query += (i + "=" + this[i].Id + ":" + this[i].Count);
                    }
                }
                return query;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Deserializes a string of data and generates specified container items.
        /// </summary>
        /// <param name="data">The data to parse.</param>
        public void Deserialize(string data)
        {
            string[] items = data.Split(',');

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != string.Empty)
                {
                    string[] subData = items[i].Split(':');
                    string[] subSubData = subData[0].Split('=');

                    int slot = int.Parse(subSubData[0]);
                    short itemId = short.Parse(subSubData[1]);
                    int amount = int.Parse(subData[1]);

                    if (amount > 0)
                    {
                        this[slot] = new Item(itemId, amount);
                    }
                }
            }
        }

        /// <summary>
        /// Produces an array holding items from this container.
        /// </summary>
        /// <param name="sort">Whether to sort the items.</param>
        /// <returns>Returns an array holding this contain's items.</returns>
        public Item[] ToArray(bool sort)
        {
            if (sort)
            {
                Sort();
            }
            return this.items;
        }

        /// <summary>
        /// Resets the item container.
        /// </summary>
        protected void ResetInternal()
        {
            this.items = new Item[this.capacity];
        }

        /// <summary>
        /// Refreshes the game player to display contents in this container.
        /// </summary>
        public abstract void Refresh();
        #endregion Methods
    }
}
