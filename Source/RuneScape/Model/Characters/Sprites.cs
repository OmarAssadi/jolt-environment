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
    /// Handles client sprites.
    /// </summary>
    public class Sprites
    {
        #region Properties
        /// <summary>
        /// Gets the primary sprite.
        /// </summary>
        public int PrimarySprite { get; set; }
        /// <summary>
        /// Gets the secondary sprite.
        /// </summary>
        public int SecondarySprite { get; set; }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Constructs a new sprites class.
        /// </summary>
        public Sprites()
        {
            this.PrimarySprite = -1;
            this.SecondarySprite = -1;
        }
        #endregion Constructor

        #region Methods
        /// <summary>
        /// Resets the sprites.
        /// </summary>
        public void Reset()
        {
            this.PrimarySprite = -1;
            this.SecondarySprite = -1;
        }
        #endregion Methods
    }
}
