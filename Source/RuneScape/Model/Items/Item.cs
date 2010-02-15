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

namespace RuneScape.Model.Items
{
    /// <summary>
    /// Represents a single runescape item.
    /// </summary>
    public class Item : IRuneObject
    {
        #region Properties
        /// <summary>
        /// Gets the item's id.
        /// </summary>
        public short Id { get; private set; }
        /// <summary>
        /// Gets the item's count.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the item's index.
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Gets the item's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the item's definition.
        /// </summary>
        public ItemDefinition Definition { get { 
            return GameEngine.World.ItemManager.GetDefinition(this.Id); } }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs single item.
        /// </summary>
        /// <param name="id">The id of the item to create.</param>
        public Item(short id)
            : this(id, 1)
        {
        }

        /// <summary>
        /// Constructs a stacked item.
        /// </summary>
        /// <param name="id">The id of the item to create.</param>
        /// <param name="count">The amount of items for the stack.</param>
        public Item(short id, int count)
        {
            this.Id = id;
            this.Name = Definition.Name;
            this.Count = count;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Summerizes this item.
        /// </summary>
        /// <returns>Returns a string that summerizes this item.</returns>
        public override string ToString()
        {
            return "Item[id=" + this.Id + ", count=" + this.Count + "]";
        }
        #endregion Methods
    }
}
