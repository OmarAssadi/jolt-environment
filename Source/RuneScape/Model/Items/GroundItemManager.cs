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

namespace RuneScape.Model.Items
{
    /// <summary>
    /// Defines management for ground items.
    /// </summary>
    public class GroundItemManager
    {
        #region Fields
        /// <summary>
        /// A container holding all existing/spawned ground items.
        /// </summary>
        private List<GroundItem> items = new List<GroundItem>();
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the ground items.
        /// </summary>
        public List<GroundItem> Items { get { return this.items; } }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a ground item.
        /// </summary>
        /// <param name="item">The item to create.</param>
        /// <param name="location">The location of the item.</param>
        public void Create(Item item, Location location)
        {
            lock (this.Items)
            {
                foreach (GroundItem g in this.items)
                {
                    if (g.Character == null && !g.Destroyed && !g.Spawned
                        && g.Location.Equals(location) && g.Id == item.Id
                        && (g.Definition.Noted || g.Definition.Stackable))
                    {
                        g.Count += item.Count;
                        return;
                    }
                };
            }

            GroundItem gItem = new GroundItem(location, item.Id, item.Count);
            this.items.Add(gItem);
            gItem.GlobalSpawn();
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
                foreach (GroundItem g in this.items)
                {
                    if (g.Character == character && !g.Destroyed && !g.Spawned
                        && g.Location.Equals(location) && g.Id == item.Id 
                        && (g.Definition.Noted || g.Definition.Stackable))
                    {
                        g.Count += item.Count;
                        return;
                    }
                };
            }

            GroundItem gItem = new GroundItem(location, character,item.Id, item.Count);
            this.items.Add(gItem);
            gItem.Spawn();
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
                foreach (GroundItem g in this.items)
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
                    }
                }

                if (binItem != null)
                {
                    this.items.Remove(binItem);
                }
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
                this.items.ForEach((g) =>
                {
                    if (character.Location.WithinDistance(g.Location) && !g.Destroyed
                        && (character == g.Character || character.LongName == g.Character.LongName
                        || g.Character == null))
                    {
                        g.Spawn(character);
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
                foreach (GroundItem gItem in this.items)
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
                foreach (GroundItem gItem in this.items)
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
