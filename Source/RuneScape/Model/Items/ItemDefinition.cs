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
    /// Represents a item's definition.
    /// </summary>
    public partial class ItemDefinition
    {
        #region Properties
        /// <summary>
        /// Gets the item's id.
        /// </summary>
        public short Id { get; private set; }
        /// <summary>
        /// Gets the item's name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the item's examination quote.
        /// </summary>
        public string Examine { get; private set; }

        /// <summary>
        /// Gets the equipment id of this item.
        /// 
        ///     <remarks>-1 if not equipable.</remarks>
        /// </summary>
        public short EquipId { get; private set; }
        /// <summary>
        /// Gets whether the item is noted.
        /// </summary>
        public bool Noted { get; private set; }
        /// <summary>
        /// Gets the note id (if availible).
        /// </summary>
        public short NoteId { get; private set; }
        /// <summary>
        /// Gets whether the item is stackable.
        /// </summary>
        public bool Stackable { get; private set; }
        /// <summary>
        /// Gets whether the item is tradable.
        /// </summary>
        public bool Tradable { get; private set; }

        /// <summary>
        /// Gets the item's bonuses.
        /// </summary>
        public ItemDefinitionBonuses Bonuses { get; private set; }
        /// <summary>
        /// Gets the item's stock prices.
        /// </summary>
        public ItemDefinitionPrices Prices { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a definition with specified details.
        /// </summary>
        /// <param name="id">The item's id.</param>
        /// <param name="name">The item's name.</param>
        /// <param name="examine">The item's examination qoute.</param>
        /// <param name="equipId">The item's equip id.</param>
        /// <param name="noted">Whether the item is noted.</param>
        /// <param name="stackable">Whether the item is stackable.</param>
        /// <param name="prices">Array of definition prices.</param>
        /// <param name="bonuses">Array of definition bonuses.</param>
        public ItemDefinition(short id, string name, string examine, short equipId, bool noted, short noteId, bool stackable, bool tradable, int[] prices, short[] bonuses)
        {
            this.Id = id;
            this.Name = name;
            this.Examine = examine;
            this.EquipId = equipId;
            this.Noted = noted;
            this.NoteId = noteId;
            this.Stackable = stackable;
            this.Tradable = tradable;
            this.Bonuses = new ItemDefinitionBonuses(bonuses);
            this.Prices = new ItemDefinitionPrices(prices);
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Summerizes the definition.
        /// </summary>
        /// <returns>Returns the summerized version of this definition.</returns>
        public override string ToString()
        {
            return "ItemDefinition[id=" + this.Id + ", name=" + this.Name + ", examine=" + this.Examine + "]";
        }
        #endregion Methods
    }
}
