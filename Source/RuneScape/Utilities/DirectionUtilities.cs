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

namespace RuneScape.Utilities
{
    /// <summary>
    /// Provides direction related utilities.
    /// </summary>
    public static class DirectionUtilities
    {
        #region Fields
        /// <summary>
        /// Delta X direction.
        /// </summary>
        public static readonly int[] DeltaX = { -1, 0, 1, -1, 1, -1, 0, 1 };
        /// <summary>
        /// Delta Y direction.
        /// </summary>
        public static readonly int[] DeltaY = { 1, 1, 1, 0, 0, -1, -1, -1 };
        #endregion Fields

        #region Methods
        /// <summary>
        /// Gets the direction specified by the two differences.
        /// </summary>
        /// <param name="dx">X difference.</param>
        /// <param name="dy">Y difference.</param>
        /// <returns>Returns the direction.</returns>
        public static int CalculateDirection(int dx, int dy)
        {
            if (dx < 0)
            {
                if (dy < 0)
                    return 5;
                else if (dy > 0)
                    return 0;
                else
                    return 3;
            }
            else if (dx > 0)
            {
                if (dy < 0)
                    return 7;
                else if (dy > 0)
                    return 2;
                else
                    return 4;
            }
            else
            {
                if (dy < 0)
                    return 6;
                else if (dy > 0)
                    return 1;
                else
                    return -1;
            }
        }
        #endregion Methods
    }
}
