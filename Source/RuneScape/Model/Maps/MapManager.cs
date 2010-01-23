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
using System.IO;

namespace RuneScape.Model.Maps
{
    /// <summary>
    /// Provides management for map related content.
    /// </summary>
    public class MapManager
    {
        #region Fields
        /// <summary>
        /// The map regions loaded from a RuneScape.Model.Maps.MapDataLoader object.
        /// </summary>
        private Dictionary<int, int[]> regions = new Dictionary<int, int[]>();
        #endregion Fields

        #region Constructor
        /// <summary>
        /// Constructs a new map manager.
        /// </summary>
        public MapManager()
        {      
            // And finally, load the map data regions.
            MapDataLoader.Load(this.regions);
        }
        #endregion Constructor

        #region Methods
        /// <summary>
        /// Gets a map region from the given id.
        /// </summary>
        /// <param name="id">The region's id.</param>
        /// <returns>Returns 4 ints which contain the region data.</returns>
        public int[] GetMapData(int id)
        {
            return regions[id];
        }
        #endregion Methods
    }
}
