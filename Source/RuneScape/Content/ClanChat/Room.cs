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

using RuneScape.Model.Characters;

namespace RuneScape.Content.ClanChat
{
    /// <summary>
    /// Represents a single clan chat room.
    /// </summary>
    public class Room
    {
        #region Properties
        /// <summary>
        /// The room's name.
        /// </summary>
        public long Name { get; private set; }
        /// <summary>
        /// The owner of the room.
        /// </summary>
        public long Owner { get; private set; }

        /// <summary>
        /// Rank requirement to kick.
        /// </summary>
        public Rank KickReq { get; set; }
        /// <summary>
        /// Rank requirement to join.
        /// </summary>
        public Rank JoinReq { get; set; }
        /// <summary>
        /// Rank requirement to talk.
        /// </summary>
        public Rank TalkReq { get; set; }

        /// <summary>
        /// Gets a list of current users in the room.
        /// </summary>
        public List<long> Users { get; private set; }
        /// <summary>
        /// Gets a list of ranks specified to users.
        /// </summary>
        public Dictionary<long, Rank> Ranks { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new clan chat room.
        /// </summary>
        /// <param name="name">The name of the clan chat.</param>
        /// <param name="owner">The owner of the room.</param>
        public Room(long name, long owner)
        {
            this.Name = name;
            this.Owner = owner;

            this.KickReq = Rank.Owner;
            this.JoinReq = Rank.Regular;
            this.TalkReq = Rank.Regular;
        }
        #endregion Constructors
    }
}
