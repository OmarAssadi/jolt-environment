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

using RuneScape.Model;
using RuneScape.Model.Characters;

namespace RuneScape.Utilities
{
    /// <summary>
    /// Represents an event that is executed one the character has reached to a specified location.
    /// </summary>
    public class CoordinateEvent
    {
        #region Fields
        /// <summary>
        /// The action triggered when reached specified location.
        /// </summary>
        //private Action action;
        #endregion Fields

        #region Properties
        public Action Action { get; set; }
        /// <summary>
        /// Gets the character.
        /// </summary>
        public Character Character { get; private set; }
        /// <summary>
        /// Gets or sets the target location.
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// Gets or sets the character's old location.
        /// </summary>
        public Location OldLocation { get; set; }

        /// <summary>
        /// Gets or sets the number of failed attempts.
        /// </summary>
        public int FailedAttempts { get; set; }
        /// <summary>
        /// Gets or sets the distance away from the location.
        /// </summary>
        public int Distance { get; set; }
        /// <summary>
        /// Gets or sets whether the location has been reached.
        /// </summary>
        public int Reached { get; set; }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Constructs a new coordinate event.
        /// </summary>
        /// <param name="character">The character to coordinate.</param>
        /// <param name="location">The location the character is attempting to reach.</param>
        /// <param name="action">The action trigger when reached location.</param>
        public CoordinateEvent(Character character, Location location)
        {
            this.Character = character;
            this.Location = location;
            //this.action = action;
        }
        #endregion Constructor

        #region Methods
        /// <summary>
        /// Processes a coordinate event.
        /// </summary>
        public static bool Process(CoordinateEvent ce)
        {
            if (ce.Character.UpdateFlags.Teleporting)
            {
                return false;
            }
            bool isAtTarget = ce.Character.Location.WithinDistance(ce.Location, ce.Distance);

            if (isAtTarget && ce.Character.Location.Equals(ce.Location) || ce.FailedAttempts >= 2)
            {
            }
            return true;
        }
        #endregion Methods
    }
}
