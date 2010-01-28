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
        public static readonly sbyte[] DeltaX = { -1, 0, 1, -1, 1, -1, 0, 1 };
        /// <summary>
        /// Delta Y direction.
        /// </summary>
        public static readonly sbyte[] DeltaY = { 1, 1, 1, 0, 0, -1, -1, -1 };
        /// <summary>
        /// Server to client direction translation.
        /// </summary>
        public static readonly sbyte[] SCDirectionTranslation = { 1, 2, 4, 7, 6, 5, 3, 0 };
        #endregion Fields

        #region Methods
        /// <summary>
        /// Gets the direction specified by the two differences.
        /// </summary>
        /// <param name="dx">X difference.</param>
        /// <param name="dy">Y difference.</param>
        /// <returns>Returns the direction.</returns>
        public static sbyte CalculateDirection(short dx, short dy)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcX"></param>
        /// <param name="srcY"></param>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <returns></returns>
        public static sbyte CalculateDirection(int srcX, int srcY, int destX, int destY)
        {
            int dx = destX - srcX, dy = destY - srcY;
            if (dx < 0)
            {
                if (dy < 0)
                {
                    if (dx < dy)
                        return 11;
                    else if (dx > dy)
                        return 9;
                    else
                        return 10; // dx == dy
                }
                else if (dy > 0)
                {
                    if (-dx < dy)
                        return 15;
                    else if (-dx > dy)
                        return 13;
                    else
                        return 14; // -dx == dy
                }
                else
                { // dy == 0
                    return 12;
                }
            }
            else if (dx > 0)
            {
                if (dy < 0)
                {
                    if (dx < -dy)
                        return 7;
                    else if (dx > -dy)
                        return 5;
                    else
                        return 6; // dx == -dy
                }
                else if (dy > 0)
                {
                    if (dx < dy)
                        return 1;
                    else if (dx > dy)
                        return 3;
                    else
                        return 2; // dx == dy
                }
                else
                { // dy == 0
                    return 4;
                }
            }
            else
            { // dx == 0
                if (dy < 0)
                {
                    return 8;
                }
                else if (dy > 0)
                {
                    return 0;
                }
                else
                { // dy == 0
                    return -1; // src and dest are the same
                }
            }
        }
        #endregion Methods
    }
}
