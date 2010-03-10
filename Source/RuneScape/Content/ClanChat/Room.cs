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
using RuneScape.Communication.Messages.Outgoing;

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
        private List<long> Users { get; private set; }
        /// <summary>
        /// Gets a list of ranks specified to users.
        /// </summary>
        private Dictionary<long, Rank> Ranks { get; private set; }

        /// <summary>
        /// The amount of users in the room.
        /// </summary>
        public byte UserCount
        {
            get
            {
                lock (this.Users)
                {
                    return (byte)this.Users.Count;
                }
            }
        }
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

            this.Users = new List<long>();
            this.Ranks = new Dictionary<long, Rank>();

            this.KickReq = Rank.Owner;
            this.JoinReq = Rank.Regular;
            this.TalkReq = Rank.Regular;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Adds a rank to the ranks container.
        /// </summary>
        /// <param name="name">The name of the character.</param>
        /// <param name="rank">The rank for the character.</param>
        public void AddRank(long name, Rank rank)
        {
            lock (this.Ranks)
            {
                Ranks.Add(name, rank);
            }
        }

        /// <summary>
        /// Removes a rank for the specified character.
        /// </summary>
        /// <param name="name">The character to remove rank from.</param>
        public void RemoveRank(long name)
        {
            lock (this.Ranks)
            {
                Ranks.Remove(name);
            }
        }

        /// <summary>
        /// Checks whether the room contains a rank for the specified name.
        /// </summary>
        /// <param name="name">The name to check for.</param>
        /// <returns>Returns the character's rank if existant; null if not.</returns>
        public Rank GetRank(long name)
        {
            lock (this.Ranks)
            {
                if (this.Ranks.ContainsKey(name))
                {
                    return this.Ranks[name];
                }
            }
            return Rank.Regular;
        }

        /// <summary>
        /// Adds a user to the room.
        /// </summary>
        /// <param name="name">The name of the user to add.</param>
        public void AddUser(long name)
        {
            lock (this.Users)
            {
                this.Users.Add(name);
            }
        }

        /// <summary>
        /// Removes a user from the room.
        /// </summary>
        /// <param name="name">The name of the user to remove.</param>
        public void RemoveUser(long name)
        {
            lock (this.Users)
            {
                this.Users.Remove(name);
            }
        }

        /// <summary>
        /// Refreshes the list for the room.
        /// </summary>
        public void Refresh()
        {
            GetUsers().ForEach((l) =>
            {
                GameEngine.World.CharacterManager.Get(l).Session.SendData(
                    new ClanListPacketComposer(this).Serialize());
            });
        }

        /// <summary>
        /// Gets the current list of users.
        /// </summary>
        /// <returns>A list with the users in this room.</returns>
        public List<long> GetUsers()
        {
            lock (this.Users)
            {
                return new List<long>(this.Users);
            }
        }

        /// <summary>
        /// Gets whether the user is able to join the room.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>Returns true if able to join; false if not.</returns>
        public bool CanJoin(long name)
        {
            if (this.JoinReq >= GetRank(name))
            {
                return true;
            }
            return false;
        }
        #endregion Methods
    }
}
