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
using RuneScape.Content.ClanChat;
using RuneScape.Utilities;

namespace RuneScape.Model.Characters
{
    /// <summary>
    /// Prepresents a character's contacts.
    /// </summary>
    public class Contacts
    {
        #region Fields
        /// <summary>
        /// The character whom owns the contacts.
        /// </summary>
        private Character character;
        /// <summary>
        /// The amount of messages sent since login.
        /// </summary>
        private int counter = 0;
        /// <summary>
        /// A lock providing access to the contacts (friends/ignored).
        /// </summary>
        private object lockObj = new object();
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets a list of contacts that are considered as friends.
        /// </summary>
        public List<long> Friends { get; private set; }
        /// <summary>
        /// Gets a list of contacts that are ignored.
        /// </summary>
        public List<long> Ignored { get; private set; }

        /// <summary>
        /// Gets the next unique counter id.
        /// </summary>
        public int NextUniqueId
        {
            get
            {
                return ++this.counter;
            }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs the contacts.
        /// </summary>
        /// <param name="character">The character who owns the contacts.</param>
        public Contacts(Character character)
        {
            this.character = character;
            this.Friends = new List<long>(100);
            this.Ignored = new List<long>(100);
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Refreshes the character's contact list.
        /// </summary>
        public void Refresh()
        {
            lock (this.lockObj)
            {
                Friends.ForEach((l) => this.character.Session.SendData(
                    new UpdateFriendPacketComposer(l, GetWorld(l)).Serialize()));
                this.character.Session.SendData(
                    new UpdateIgnorePacketComposer(Ignored.ToArray()).Serialize());
            }
        }

        /// <summary>
        /// Gets the specified name's world id.
        /// </summary>
        /// <param name="name">The name of the friend.</param>
        /// <returns>Returns the world id.</returns>
        public static short GetWorld(long name)
        {
            List<Character> chars = new List<Character>(GameEngine.World.CharacterManager.Characters.Values);
            foreach (Character c in chars)
            {
                if (c.LongName == name)
                {
                    return (short)GameEngine.World.Id;
                }
            }
            return 0;
        }

        /// <summary>
        /// Adds a friend to the contact list.
        /// </summary>
        /// <param name="name">The name of the friend.</param>
        public void AddFriend(long name)
        {
            lock (this.lockObj)
            {
                if (this.Friends.Count >= 100)
                {
                    this.character.Session.SendData(new MessagePacketComposer(
                        "Your friends list is full.").Serialize());
                    return;
                }
                if (this.Friends.Contains(name))
                {
                    this.character.Session.SendData(new MessagePacketComposer(
                        "This person is already on your friends list.").Serialize());
                    return;
                }

                this.Friends.Add(name);
                this.character.Session.SendData(new UpdateFriendPacketComposer(name, GetWorld(name)).Serialize());

                Room room = GameEngine.Content.ClanChat.Get(this.character.LongName);
                if (room != null)
                {
                    room.AddRank(name, Rank.Friend);
                }
            }
        }

        /// <summary>
        /// Adds a ignored person to the list.
        /// </summary>
        /// <param name="name">The name of the ignored.</param>
        public void AddIgnore(long name)
        {
            lock (this.lockObj)
            {
                if (this.Ignored.Count >= 100)
                {
                    this.character.Session.SendData(new MessagePacketComposer(
                        "Your ignored list is full.").Serialize());
                    return;
                }
                if (this.Ignored.Contains(name))
                {
                    this.character.Session.SendData(new MessagePacketComposer(
                        "This person is already on your ignored list.").Serialize());
                    return;
                }
                this.Ignored.Add(name);

                Room room = GameEngine.Content.ClanChat.Get(this.character.LongName);
                if (room != null)
                {
                    room.RemoveRank(name);
                }
            }
        }

        /// <summary>
        /// Removes the specified friend from the contact list.
        /// </summary>
        /// <param name="name">the name of the friend to remove.</param>
        public void RemoveFriend(long name)
        {
            lock (this.lockObj)
            {
                this.Friends.Remove(name);
            }
        }

        /// <summary>
        /// Removes the specified ignore from the contact list.
        /// </summary>
        /// <param name="name">the name of the ignore to remove.</param>
        public void RemoveIgnore(long name)
        {
            lock (this.lockObj)
            {
                this.Ignored.Remove(name);
            }
        }

        /// <summary>
        /// Submits a login trigger to everyone online.
        /// </summary>
        public void OnLogin()
        {
            new List<Character>(GameEngine.World.CharacterManager
                .Characters.Values).ForEach((c) => c.Contacts.OnLogin(this.character));
        }

        /// <summary>
        /// Submits a logout trigger to everyone online.
        /// </summary>
        public void OnLogout()
        {
            new List<Character>(GameEngine.World.CharacterManager
                .Characters.Values).ForEach((c) => c.Contacts.OnLogout(this.character));
        }

        /// <summary>
        /// Triggers a login event for the specified character.
        /// </summary>
        /// <param name="c">The character to trigger event for.</param>
        private void OnLogin(Character c)
        {
            if (this.Friends.Contains(c.LongName))
            {
                this.character.Session.SendData(new UpdateFriendPacketComposer(
                    c.LongName, (short)GameEngine.World.Id).Serialize());
            }
        }

        /// <summary>
        /// Triggers a logout event for the specified character.
        /// </summary>
        /// <param name="c">The character to trigger event for.</param>
        private void OnLogout(Character c)
        {
            if (this.Friends.Contains(c.LongName))
            {
                this.character.Session.SendData(new UpdateFriendPacketComposer(
                    c.LongName, 0).Serialize());
            }
        }

        /// <summary>
        /// Sends a message to the the specified friend.
        /// </summary>
        /// <param name="name">The name of the friend.</param>
        /// <param name="message">The message to send.</param>
        public void SendMessage(long name, string message)
        {
            List<Character> chars = new List<Character>(GameEngine.World.CharacterManager.Characters.Values);
            foreach (Character c in chars)
            {
                if (c.LongName == name)
                {
                    c.Session.SendData(new RecievePMPacketComposer(this.character.LongName,
                        (byte)this.character.ClientRights, message, this.character.Contacts.NextUniqueId).Serialize());
                    this.character.Session.SendData(new SendPMPacketComposer(name, message).Serialize());
                    ChatUtilities.LogChat(this.character.Name, ChatType.Private, c.Name, message);
                    return;
                }
            }
            this.character.Session.SendData(new MessagePacketComposer("Your friend is currently unavailible.").Serialize());
        }

        /// <summary>
        /// Serializes the character's friends.
        /// </summary>
        /// <returns>Returns a string representing the serialized query.</returns>
        public string SerializeFriends()
        {
            lock (this.lockObj)
            {
                if (this.Friends.Count > 0)
                {
                    StringBuilder query = new StringBuilder();
                    this.Friends.ForEach((l) =>
                    {
                        if (query.Length > 0)
                        {
                            query.Append(",");
                        }
                        query.Append(l);
                    });
                    return query.ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Serializes the character's ignores.
        /// </summary>
        /// <returns>Returns a string representing the serialized query.</returns>
        public string SerializeIgnores()
        {
            lock (this.lockObj)
            {
                if (this.Ignored.Count > 0)
                {
                    StringBuilder query = new StringBuilder();
                    this.Ignored.ForEach((l) =>
                    {
                        if (query.Length > 0)
                        {
                            query.Append(",");
                        }
                        query.Append(l);
                    });
                    return query.ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Deserializes the given data and parses the friends data from it.
        /// </summary>
        /// <param name="data">The data to parse information from.</param>
        public void DeserializeFriends(string data)
        {
            lock (this.lockObj)
            {
                string[] names = data.Split(',');
                foreach (string name in names)
                {
                    this.Friends.Add(long.Parse(name));
                }
            }
        }

        /// <summary>
        /// Deserializes the given data and parses the ignores data from it.
        /// </summary>
        /// <param name="data">The data to parse information from.</param>
        public void DeserializeIgnores(string data)
        {
            lock (this.lockObj)
            {
                string[] names = data.Split(',');
                foreach (string name in names)
                {
                    this.Ignored.Add(long.Parse(name));
                }
            }
        }
        #endregion Methods
    }
}
