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
            GroundItem gItem = new GroundItem(location, item.Id, item.Count);
            this.items.Add(gItem);
            gItem.GlobalSpawn();
        }

        /// <summary>
        /// Refreshes all the ground items so it is visible to the character.
        /// </summary>
        /// <param name="character">The character to refresh ground items for.</param>
        public void Refresh(Character character)
        {
            this.items.ForEach((g) =>
            {
                if (character.Location.WithinDistance(g.Location) && !g.Destroyed 
                    && (character == g.Character || g.Character == null))
                {
                    g.Spawn(character);
                }
            });
        }
        #endregion Methods
    }
}
