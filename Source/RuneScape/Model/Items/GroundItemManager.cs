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
using System.Data;
using System.Linq;
using System.Text;

using JoltEnvironment.Storage.Sql;
using RuneScape.Model.Characters;

namespace RuneScape.Model.Items
{
    /// <summary>
    /// Defines management for ground items.
    /// </summary>
    public class GroundItemManager
    {
        #region Properties
        /// <summary>
        /// Gets a container holding all existing/spawned ground items.
        /// </summary>
        private List<GroundItem> Items { get; set; }
        /// <summary>
        /// Gets a container holding all spawned ground items waiting to be respawned.
        /// </summary>
        private List<GroundItem> WaitList { get; set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new ground item manager.
        /// </summary>
        public GroundItemManager()
        {
            this.Items = new List<GroundItem>();
            this.WaitList = new List<GroundItem>();

            DataRow[] rows = null;
            using (SqlDatabaseClient client = GameServer.Database.GetClient())
            {
                rows = client.ReadDataTable("SELECT * FROM item_spawns").Select();
            }

            // Add in all (server-)spawned items.
            foreach (DataRow row in rows)
            {
                short id = (short)row[1];
                int count = (int)row[2];
                short x = (short)row[3];
                short y = (short)row[4];

                GroundItem gItem = new GroundItem(Location.Create(x, y, 0), id, count);
                gItem.Spawned = true;
                Add(gItem);
            }
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Adds a precreated ground item into the container.
        /// </summary>
        /// <param name="item"></param>
        public void Add(GroundItem item)
        {
            lock (this.Items)
            {
                this.Items.Add(item);
            }
        }

        /// <summary>
        /// Creates a ground item.
        /// </summary>
        /// <param name="item">The item to create.</param>
        /// <param name="location">The location of the item.</param>
        public void Create(Item item, Location location)
        {
            lock (this.Items)
            {
                foreach (GroundItem g in this.Items)
                {
                    if (g.Character == null && !g.Destroyed && !g.Spawned
                        && g.Location.Equals(location) && g.Id == item.Id
                        && (g.Definition.Noted || g.Definition.Stackable))
                    {
                        g.Count += item.Count;
                        return;
                    }
                };

                GroundItem gItem = new GroundItem(location, item.Id, item.Count);
                this.Items.Add(gItem);
                gItem.GlobalSpawn();
            }
        }

        /// <summary>
        /// Creates a character's ground item.
        /// </summary>
        /// <param name="item">The item to create.</param>
        /// <param name="location">The location of the item.</param>
        /// <param name="character">The owner of the item.</param>
        public void Create(Item item, Location location, Character character)
        {
            lock (this.Items)
            {
                foreach (GroundItem g in this.Items)
                {
                    if (g.Character == character && !g.Destroyed && !g.Spawned
                        && g.Location.Equals(location) && g.Id == item.Id 
                        && (g.Definition.Noted || g.Definition.Stackable))
                    {
                        g.Count += item.Count;
                        return;
                    }
                };

                GroundItem gItem = new GroundItem(location, character, item.Id, item.Count);
                this.Items.Add(gItem);
                gItem.Spawn();
            }
        }

        /// <summary>
        /// Destroys the specified item at the specified location.
        /// </summary>
        /// <param name="location">The location the item is expected to be at.</param>
        /// <param name="id">The id of the item to destroy.</param>
        public void Destroy(Location location, int id)
        {
            GroundItem binItem = null;
            lock (this.Items)
            {
                foreach (GroundItem g in this.Items)
                {
                    if (g.Location.Equals(location) && g.Id == id)
                    {
                        binItem = g;
                        g.Destroyed = true;

                        if (g.Character == null)
                        {
                            g.GlobalDespawn();
                        }
                        else
                        {
                            g.Despawn();
                        }
                        break;
                    }
                }

                if (binItem != null)
                {
                    if (binItem.Spawned)
                    {
                        lock (this.WaitList)
                        {
                            this.WaitList.Add(binItem);
                        }
                        binItem.TimeCreated = DateTime.Now;
                    }
                    this.Items.Remove(binItem);
                }
            }
        }

        /// <summary>
        /// Processes all the items that have been dropped/spawned.
        /// </summary>
        public void Process()
        {
            // Loop though all ground items synchronously.
            lock (this.Items)
            {
                var groundItems = new List<GroundItem>(this.Items);
                groundItems.ForEach((gItem) =>
                {
                    if (!gItem.Spawned && (DateTime.Now - gItem.TimeCreated).TotalSeconds >= 90)
                    {
                        if (gItem.Character == null)
                        {
                            gItem.Destroyed = true;
                            gItem.GlobalDespawn();
                            this.Items.Remove(gItem);
                        }
                        else
                        {
                            if (!gItem.Definition.Tradable)
                            {
                                gItem.Destroyed = true;
                                gItem.Despawn();
                                this.Items.Remove(gItem);
                            }
                            else
                            {
                                gItem.Despawn();
                                gItem.Character = null;
                                gItem.TimeCreated = DateTime.Now;
                                gItem.GlobalSpawn();
                            }
                        }
                    }
                });
            }

            // Loop though all wait list items synchronously.
            lock (this.WaitList)
            {
                var waitList = new List<GroundItem>(this.WaitList);
                waitList.ForEach((gItem) =>
                {
                    if ((DateTime.Now - gItem.TimeCreated).TotalSeconds >= 60)
                    {
                        gItem.Destroyed = false;
                        gItem.GlobalSpawn();
                        gItem.TimeCreated = DateTime.Now;
                        Add(gItem);
                        this.WaitList.Remove(gItem);
                    }
                });
            }
        }

        /// <summary>
        /// Refreshes all the ground items so it is visible to the character.
        /// </summary>
        /// <param name="character">The character to refresh ground items for.</param>
        public void Refresh(Character character)
        {
            lock (this.Items)
            {
                this.Items.ForEach((g) =>
                {
                    if (character.Location.WithinDistance(g.Location) && !g.Destroyed)
                    {
                        if (g.Character != null && character != g.Character
                            && character.MasterId == g.Character.MasterId)
                        {
                            g.Character = character;
                            g.Spawn();
                        }
                        else
                        {
                            g.Spawn(character);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Loops through all existing ground items to check if a 
        /// specified item exists on the specified location.
        /// </summary>
        /// <param name="location">The location the item is expected to be at.</param>
        /// <param name="id">The id of the item to look for.</param>
        /// <returns>Returns true if located, false if not.</returns>
        public bool Exists(Location location, int id)
        {
            lock (this.Items)
            {
                foreach (GroundItem gItem in this.Items)
                {
                    if (gItem.Location.Equals(location) && gItem.Id == id && !gItem.Destroyed)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Loops through all existing ground items to get the amount of items at the specified location.
        /// </summary>
        /// <param name="location">The location the item is expected to be at.</param>
        /// <param name="id">The id of the item to look for.</param>
        /// <returns>Returns the amount; -1 if not found.</returns>
        public int GetAmount(Location location, int id)
        {
            lock (this.Items)
            {
                foreach (GroundItem gItem in this.Items)
                {
                    if (gItem.Location.Equals(location) && gItem.Id == id && !gItem.Destroyed)
                    {
                        return gItem.Count;
                    }
                }
                return -1;
            }
        }
        #endregion Methods
    }
}
