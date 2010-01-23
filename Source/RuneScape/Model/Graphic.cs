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
    /// Represents a single graphic display request.
    /// </summary>
    public struct Graphic
    {
        #region Properties
        /// <summary>
        /// Gets the graphic id (id from client).
        /// </summary>
        public short Id { get; private set; }
        /// <summary>
        /// Gets the graphic delay.
        /// 
        ///     <para>The length to display the graphic.</para>
        /// </summary>
        public int Delay { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new graphic.
        /// </summary>
        /// <param name="id">The graphic id.</param>
        /// <param name="delay">The delay of the graphic display.</param>
        private Graphic(short id, int delay) : this()
        {
            this.Id = id;
            this.Delay = delay;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Creates a new graphic display.
        /// </summary>
        /// <param name="id">The graphic id.</param>
        /// <returns>Returns a new instance holding the graphic data.</returns>
        public static Graphic Create(short id)
        {
            return new Graphic(id, 0);
        }

        /// <summary>
        /// Creates a new graphic display.
        /// </summary>
        /// <param name="id">The graphic id.</param>
        /// <param name="delay">The delay of the graphic display.</param>
        /// <returns>Returns a new instance holding the graphic data.</returns>
        public static Graphic Create(short id, int delay)
        {
            return new Graphic(id, delay);
        }
        #endregion Methods
    }
}
