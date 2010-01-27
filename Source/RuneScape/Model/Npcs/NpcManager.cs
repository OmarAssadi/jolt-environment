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

namespace RuneScape.Model.Npcs
{
    /// <summary>
    /// Defines management for npcs.
    /// </summary>
    public class NpcManager
    {
        #region Fields
        /// <summary>
        /// A collection of npc definitions.
        /// </summary>
        Dictionary<short, NpcDefinition> definitions;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs the npc manager.
        /// </summary>
        public NpcManager()
        {
            // Load definitions.
            this.definitions = NpcDefinition.Load();
        }
        #endregion Cosntructors

        #region Methods
        /// <summary>
        /// Gets the definition of the npc by the specified id.
        /// </summary>
        /// <param name="id">The cache index id of the npc.</param>
        /// <returns>Returns the npc definition set.</returns>
        public NpcDefinition GetDefinition(short id)
        {
            return definitions[id];
        }
        #endregion Methods
    }
}
