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
    /// Represents a character's head icon.
    /// </summary>
    public class HeadIcon
    {
        #region Properties
        /// <summary>
        /// Gets or sets the character's death icon.
        /// </summary>
        public DeathIcons DeathIcon { get; set; }
        /// <summary>
        /// Gets or sets the character's prayer icon.
        /// </summary>
        public PrayerIcons PrayerIcon { get; set; }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Constructs a character's head icon.
        /// </summary>
        public HeadIcon()
        {
            this.DeathIcon = DeathIcons.None;
            this.PrayerIcon = PrayerIcons.None;
        }
        #endregion Constructor
    }
}
