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
using System.IO;
using System.Collections.Generic;

namespace RuneScape.Model.Maps
{
    /// <summary>
    /// Provides services for loading map data.
    /// </summary>
    public static class MapDataLoader
    {
        #region Methods
        /// <summary>
        /// Loads map data and stores them in a specified 
        /// </summary>
        /// <param name="regionsCollection">The collection to add region data to.</param>
        public static void Load(Dictionary<int, int[]> regionsCollection)
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(
                    File.OpenRead(@"..\data\mapdata\regions.dat")))
                {
                    while (reader.PeekChar() >= 0)
                    {
                        int mapId = reader.ReadInt32(); // The map id.
                        int[] mapData = new int[4]; // The map data.

                        for (int i = 0; i < 4; i++)
                            mapData[i] = reader.ReadInt32();

                        regionsCollection.Add(mapId, mapData); // Add the data to collection.
                    }
                }
                //Program.Logger.WriteInfo("Loaded " + regionsCollection.Count + " map regions.");
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
        }
        #endregion Methods
    }
}
