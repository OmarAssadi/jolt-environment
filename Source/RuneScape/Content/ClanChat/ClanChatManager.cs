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
using RuneScape.Utilities;

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
        /// Gets whether there is a room the specified character owns.
        /// </summary>
        /// <param name="name">The name of the character.</param>
        /// <returns>Returns true if contained; false if not.</returns>
        public bool Contains(long name)
        {
            lock (this.rooms)
            {
                return this.rooms.ContainsKey(name);
            }
        }

        /// <summary>
        /// Gets a room with the specified name.
        /// </summary>
        /// <param name="name">The name to get room from.</param>
        /// <returns>Returns the room if contained; false if not.</returns>
        public Room Get(long name)
        {
            Room r = null;
            lock (this.rooms)
            {
                if (this.rooms.ContainsKey(name))
                {
                    r = this.rooms[name];
                }
            }
            return r;
        }

        /// <summary>
        /// Creates a new clanchat room.
        /// </summary>
        /// <param name="character">The character that owns the clanchat.</param>
        /// <param name="name">The name of the clan chat.</param>
        public void CreateRoom(Character character, long name)
        {
            if (!rooms.ContainsKey(character.LongName))
            {
                Room room = new Room(name, character.LongName, character.MasterId);
                character.Contacts.Friends.ForEach((l) => room.AddRank(l, Rank.Friend));
                room.AddRank(character.LongName, Rank.Owner);
                character.Session.SendData(new StringPacketComposer(room.StringName, 590, 22).Serialize());

                lock (this.rooms)
                {
                    this.rooms.Add(character.LongName, room);
                }
            }
            else
            {
                Room r = null;
                lock (this.rooms)
                {
                    r = this.rooms[character.LongName];
                }

                r.SetName(name);
                character.Session.SendData(new StringPacketComposer(r.StringName, 590, 22).Serialize());
                Program.Logger.WriteInfo(r.StringName);
            }
        }

        /// <summary>
        /// Removes a clanchat room.
        /// </summary>
        /// <param name="character">The character to remove channel from.</param>
        public void RemoveRoom(Character character)
        {
            character.Session.SendData(new StringPacketComposer("Chat disabled", 590, 22).Serialize());

            Room r = Get(character.LongName);
            if (r != null)
            {
                lock (r.Users)
                {
                    r.Users.ForEach((l) =>
                    {
                        Character c = GameEngine.World.CharacterManager.Get(l);
                        c.Session.SendData(new MessagePacketComposer("The channel you were in has been disabled").Serialize());
                        c.Session.SendData(new ClanListPacketComposer().Serialize());
                    });
                }
                character.Session.SendData(new ClanListPacketComposer().Serialize());
            }
        }

        /// <summary>
        /// Attempts to join the specified room.
        /// </summary>
        /// <param name="name">The name of the clan to join.</param>
        public void Join(Character character, long name)
        {
            Room r = Get(name);
            if (r != null)
            {
                if (r.CanJoin(name))
                {
                    r.AddUser(character.LongName);
                    character.Session.SendData(new MessagePacketComposer(
                        "Now talking in clan channel " + r.StringName).Serialize());
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

            Room r = Get((long)character.ClanRoom);
            if (r != null)
            {
                r.RemoveUser(character.LongName);
                r.Refresh();
                character.Session.SendData(new ClanListPacketComposer().Serialize());
                character.ClanRoom = null;
            }
        }

        /// <summary>
        /// Sends a message to everyone inside the clan room.
        /// </summary>
        /// <param name="character">The character who is sending the message.</param>
        /// <param name="message">The message to send.</param>
        public void Message(Character character, string message)
        {
            Room r = Get((long)character.ClanRoom);
            if (r != null)
            {
                if (r.CanTalk(character.LongName))
                {
                    lock (r.Users)
                    {
                        r.Users.ForEach((l) =>
                        {
                            GameEngine.World.CharacterManager.Get(l).Session.SendData(
                                new ClanMessagePacketComposer(message, character.LongName, r.Name,
                                    r.NextUniqueId, (byte)character.ClientRights).Serialize());
                            ChatUtilities.LogChat(character.Name, ChatType.Clanchat, 
                                StringUtilities.LongToString(r.Owner), message);
                        });
                    }
                }
                else
                {
                    character.Session.SendData(new MessagePacketComposer(
                        "You do not have a high enough rank to talk.").Serialize());
                }
            }
        }
        #endregion Methods
    }
}
