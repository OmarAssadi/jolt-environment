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

using RuneScape.Events;
using RuneScape.Model.Characters;

namespace RuneScape.Model.Items
{
    /// <summary>
    /// A coordinated event that takes an item from the ground.
    /// </summary>
    public class TakeItemEvent : CoordinateEvent
    {
        #region Fields
        /// <summary>
        /// The id of the item.
        /// </summary>
        private short id;
        #endregion Fields

        #region Constructor
        /// <summary>
        /// Constructs a new take item (coordinated) event.
        /// </summary>
        /// <param name="character">The character to execute event for.</param>
        /// <param name="location">The location to be coordianted to.</param>
        /// <param name="id">The item id to take.</param>
        public TakeItemEvent(Character character, Location location, short id)
            : base(character, location)
        {
            this.id = id;
        }
        #endregion Constructor

        #region Methods
        /// <summary>
        /// Executes the trigger.
        /// </summary>
        public override void Execute()
        {
            if (this.Character.Location.WithinInteractionDistance(this.Location))
            {
                int amount = GameEngine.World.ItemManager.GroundItems.GetAmount(this.Location, this.id);
                if (amount != -1)
                {
                    Item item = new Item(this.id, amount);
                    if (this.Character.Inventory.HasSpace(item))
                    {
                        GameEngine.World.ItemManager.GroundItems.Destroy(this.Location, this.id);
                        this.Character.Inventory.AddItem(item);
                    }
                }
            }
        }
        #endregion Methods
    }
}
