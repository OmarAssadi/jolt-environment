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
    /// Defines management for item related content.
    /// </summary>
    public class ItemManager
    {
        #region Fields
        /// <summary>
        /// Contains all definitions loaded from database.
        /// </summary>
        private ItemDefinition[] definitions;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a new item manager.
        /// </summary>
        public ItemManager()
        {
            // Load the definitions.
            this.definitions = ItemDefinition.Load();

            // Fill specific equipment information.
            EquipmentItems.Fill();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Get an item definition from the array specified by the id.
        /// </summary>
        /// <param name="itemId">The item id of the item definitions wanted.</param>
        /// <returns>Returns a RuneScape.Model.Items.ItemDefinition instance 
        /// containing specified item definitions.</returns>
        public ItemDefinition GetDefinition(int itemId)
        {
            return this.definitions[itemId];
        }
        #endregion Methods
    }
}
