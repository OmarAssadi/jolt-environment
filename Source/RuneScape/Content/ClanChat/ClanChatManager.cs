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

using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Characters;
using JoltEnvironment.Utilities;

namespace RuneScape.Content.ClanChat
{
    /// <summary>
    /// Defines management for clan chats.
    /// </summary>
    public class ClanChatManager
    {
        #region Fields
        /// <summary>
        /// A container of clan rooms.
        /// </summary>
        private Dictionary<long, Room> rooms = new Dictionary<long, Room>();
        #endregion Fields

        #region Methods
        /// <summary>
        /// Creates a new clan chat room.
        /// </summary>
        /// <param name="character">The character that owns the clanchat.</param>
        /// <param name="name">The name of the clan chat.</param>
        public void CreateRoom(Character character, long name)
        {
            Room room = new Room(name, character.LongName);
            character.Contacts.Friends.ForEach((l) => room.AddRank(l, Rank.Friend));
            character.Session.SendData(new StringPacketComposer(room.Name.LongToString(), 590, 22).Serialize());

            lock (rooms)
            {
                rooms.Add(character.LongName, room);
            }
        }

        /// <summary>
        /// Attempts to join the specified room.
        /// </summary>
        /// <param name="name">The name of the clan to join.</param>
        public void Join(Character character, long name)
        {
            Room r = null;
            lock (rooms)
            {
                if (this.rooms.ContainsKey(name))
                {
                    r = this.rooms[name];
                }
            }

            if (rooms != null)
            {
                if (r.CanJoin(name))
                {
                    r.AddUser(name);
                    character.Session.SendData(new MessagePacketComposer(
                        "Now talking in clan channel " + r.Name + ".").Serialize());
                    character.Session.SendData(new MessagePacketComposer(
                        "To talk, start each line of the chat with the / symbol.").Serialize());
                    r.Refresh();
                    character.ClanRoom = name;
                }
                else
                {
                    character.Session.SendData(new MessagePacketComposer(
                        "You do not have a high enough rank to join this clan channel.").Serialize());
                }
            }
            else
            {
                character.Session.SendData(new MessagePacketComposer(
                        "This channel does not exist.").Serialize());
            }
        }

        /// <summary>
        /// Attempts to leave the character's current room.
        /// </summary>
        /// <param name="character">The character to leave.</param>
        public void Leave(Character character)
        {
            if (character.ClanRoom == null)
            {
                return;
            }

            Room r = null;
            lock (rooms)
            {
                if (this.rooms.ContainsKey((long)character.ClanRoom))
                {
                    r = this.rooms[(long)character.ClanRoom];
                }
            }

            if (r != null)
            {
                r.RemoveUser(character.LongName);
                r.Refresh();
                character.Session.SendData(new ClanListPacketComposer().Serialize());
                character.ClanRoom = null;
            }
        }
        #endregion Methods
    }
}
