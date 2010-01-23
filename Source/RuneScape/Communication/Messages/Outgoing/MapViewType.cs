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

namespace RuneScape.Communication.Messages.Outgoing
{
    /// <summary>
    /// Defines the types of map views.
    /// </summary>
    public enum MapViewType : byte
    {
        /// <summary>
        /// The map will be shown normally.
        /// </summary>
        Normal,
        /// <summary>
        /// The map will be shown without the compass.
        /// </summary>
        BlackCompass,
        /// <summary>
        /// The map will be shown without the minimap.
        /// </summary>
        BlackMinimap
    }
}
