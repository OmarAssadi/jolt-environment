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

namespace RuneScape.Content.ClanChat
{
    /// <summary>
    /// Defines the ranks availible in a clan chat room.
    /// </summary>
    public enum Rank
    {
        /// <summary>
        /// A regular player within the chat. This player has no rights.
        /// </summary>
        Regular = -1,
        /// <summary>
        /// A player who is friend of the clan chat owner.
        /// </summary>
        Friend = 0, 
        /// <summary>
        /// A recruit of the clan chat.
        /// </summary>
        Recruit = 1,
        /// <summary>
        /// A corperal of the clan chat.
        /// </summary>
        Corporal = 2,
        /// <summary>
        /// A sergeant of the clan chat.
        /// </summary>
        Sergeant = 3,
        /// <summary>
        /// A lieutenant of the clan chat.
        /// </summary>
        Lieutenant = 4,
        /// <summary>
        /// A captain of the clan chat.
        /// </summary>
        Captain = 5,
        /// <summary>
        /// A general of the clan chat.
        /// </summary>
        General = 6,
        /// <summary>
        /// The owner of the clan chat.
        /// </summary>
        Owner = 7
    }
}
