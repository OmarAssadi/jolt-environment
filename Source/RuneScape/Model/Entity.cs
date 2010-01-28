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

namespace RuneScape.Model
{
    /// <summary>
    /// Represents an entity (visible object) in the game world.
    /// </summary>
    public abstract class Entity : RuneObject
    {
        #region Properties
        /// <summary>
        /// The entity's location in the game world.
        /// </summary>
        public Location Location { get; set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new entity.
        /// </summary>
        public Entity()
        {
            this.Location = GameEngine.World.SpawnPoint;
        }

        /// <summary>
        /// Constructs a new entity with a specified location.
        /// </summary>
        /// <param name="location">The location of the entity.</param>
        public Entity(Location location)
        {
            this.Location = location;
        }
        #endregion Constructors
    }
}
