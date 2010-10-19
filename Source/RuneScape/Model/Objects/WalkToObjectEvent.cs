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

namespace RuneScape.Model.Objects
{
    /// <summary>
    /// A coordinate even that walks to the object specified.
    /// </summary>
    public class WalkToObjectEvent : CoordinateEvent
    {
        #region Fields
        /// <summary>
        /// The id of the object to walk to.
        /// </summary>
        private ushort objectId;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a new trade (coordinated) event.
        /// </summary>
        /// <param name="character">The character being coordinated.</param>
        /// <param name="location">The location to reach.</param>
        public WalkToObjectEvent(Character character, Location location, ushort objectId)
            : base(character, location)
        {
            this.Distance = 1;
            this.objectId = objectId;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Executes the trigger.
        /// </summary>
        public override void Execute()
        {
            dynamic script = GameServer.Scripting[@"\ObjectOption1Handlers\Object" + this.objectId + ".cs"];
            if (script != null)
            {
                script.Handle(this.Character, this.Location);
            }
        }
        #endregion Methods
    }
}
