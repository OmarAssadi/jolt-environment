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

using JoltEnvironment.Utilities;
using RuneScape.Model.Characters;
using RuneScape.Communication.Messages.Outgoing;

namespace RuneScape.Content.ClanChat
{
    /// <summary>
    /// Represents a single clan chat room.
    /// </summary>
    public class Room
    {
        #region Fields
        /// <summary>
        /// The message counter.
        /// </summary>
        private int counter = 0;
        #endregion Fields

        #region Properties
        /// <summary>
        /// The room's name.
        /// </summary>
        public long Name { get; private set; }
        /// <summary>
        /// Gets the room's name as a readable string.
        /// </summary>
        public string StringName { get; private set; }
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
        private Dictionary<long, Rank> Ranks { get; set; }

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

        /// <summary>
        /// Gets the next unique id.
        /// </summary>
        public int NextUniqueId
        {
            get { return ++this.counter; }
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
            this.StringName = name.LongToString();

            this.Users = new List<long>();
            this.Ranks = new Dictionary<long, Rank>();

            this.KickReq = Rank.Owner;
            this.JoinReq = Rank.Regular;
            this.TalkReq = Rank.Regular;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Sets the room's name.
        /// </summary>
        /// <param name="name">The name to set</param>
        public void SetName(long name)
        {
            this.Name = name;
            this.StringName = name.LongToString();
        }

        /// <summary>
        /// Adds a rank to the ranks container.
        /// </summary>
        /// <param name="name">The name of the character.</param>
        /// <param name="rank">The rank for the character.</param>
        public void AddRank(long name, Rank rank)
        {
            lock (this.Ranks)
            {
                if (this.Ranks.ContainsKey(name))
                {
                    this.Ranks[name] = rank;
                    return;
                }
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
            lock (this.Users)
            {
                this.Users.ForEach((l) =>
                {
                    GameEngine.World.CharacterManager.Get(l).Session.SendData(
                        new ClanListPacketComposer(this).Serialize());
                });
            }
        }

        /// <summary>
        /// Gets whether the user is able to join the room.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>Returns true if able to join; false if not.</returns>
        public bool CanJoin(long name)
        {
            if (this.JoinReq <= GetRank(name))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets whether the user is able to talk in the room.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>Returns true if able to talk; false if not.</returns>
        public bool CanTalk(long name)
        {
            if (this.TalkReq <= GetRank(name))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Transforms a given rank to a user-friendly string.
        /// </summary>
        /// <param name="rank">The rank to transform.</param>
        /// <returns>Returns the rank as a (modified) string.</returns>
        public static string RankToString(Rank rank)
        {
            switch (rank)
            {
                case Rank.Regular:
                    return "Anyone";
                case Rank.Friend:
                    return "Any Friends";
                case Rank.Recruit:
                    return "Recruit+";
                case Rank.Corporal:
                    return "Corporal+";
                case Rank.Sergeant:
                    return "Sergeant+";
                case Rank.Lieutenant:
                    return "Lieutenant+";
                case Rank.Captain:
                    return "Captain+";
                case Rank.General:
                    return "General+";
                case Rank.Owner:
                    return "Only me";
            }
            return "";
        }
        #endregion Methods
    }
}
