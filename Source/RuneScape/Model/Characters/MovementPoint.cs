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

namespace RuneScape.Model.Characters
{
    /// <summary>
    /// Represents a single movement point for queued walking.
    /// </summary>
    public class MovementPoint
    {
        #region Properties
        /// <summary>
        /// The point's current x coordinate.
        /// </summary>
        public short X { get; set; }
        /// <summary>
        /// The point's current y coordinate.
        /// </summary>
        public short Y { get; set; }
        /// <summary>
        /// The point's current direction.
        /// </summary>
        public sbyte Direction { get; set; }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Constructs a new point.
        /// </summary>
        /// <param name="x">The point's X coordinate.</param>
        /// <param name="y">The point's Y coordinate.</param>
        /// <param name="direction">The point's direction.</param>
        public MovementPoint(short x, short y, sbyte direction)
        {
            this.X = x;
            this.Y = y;
            this.Direction = direction;
        }
        #endregion Constructor
    }
}
