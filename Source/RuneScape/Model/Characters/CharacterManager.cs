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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

using JoltEnvironment.Storage.Sql;
using RuneScape.Communication.Login;
using RuneScape.Communication.Messages;
using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Database.Account;
using RuneScape.Database.Config;
using RuneScape.Network;
using RuneScape.Utilities;
using RuneScape.Utilities.Indexing;

namespace RuneScape.Model.Characters
{
    /// <summary>
    /// Provides management for character related content.
    /// </summary>
    public class CharacterManager
    {
        #region Fields
        /// <summary>
        /// A RuneScape.Database.Account.IAccountLoader inheritant  that loads accounts from a mysql database.
        /// </summary>
        private IAccountLoader accountLoader = new SqlAccountLoader();

        private IAccountSaver accountSaver = new SqlAccountSaver();

        /// <summary>
        /// A System.Collections.Generic.Dictionary collection holding all characters currently online.
        /// </summary>
        private ConcurrentDictionary<Node, Character> characters = new ConcurrentDictionary<Node, Character>();

        /// <summary>
        /// Manages client slots.
        /// </summary>
        private SlotManager slotManager = new SlotManager(GameServer.TcpConnection.MaxConnections);

        /// <summary>
        /// Whether to log login attemps.
        /// </summary>
        private bool logAttempts = false;
        /// <summary>
        /// Whether to log login sessions.
        /// </summary>
        private bool logSessions = false;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the number of connected characters.
        /// </summary>
        public int CharacterCount { get { return this.characters.Count; } }
        /// <summary>
        /// Gets the current characters online.
        /// </summary>
        public ConcurrentDictionary<Node, Character> Characters { get { return this.characters; } }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new character manager.
        /// </summary>
        public CharacterManager()
        {
            try
            {
                // Try to loading the "Login" configuration section.
                var loginVars = Configuration.MySql.LoadFromSection("Login");

                this.logAttempts = (bool)loginVars["LogAttempts"];
                this.logSessions = (bool)loginVars["LogSessions"];
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
        }
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Registers a character into the game.
        /// </summary>
        /// <param name="character">The character to add.</param>
        /// <returns>Returns true if the registration is successful; false if collection is full.</returns>
        public bool Register(Character character)
        {
            try
            {
                if (this.slotManager.ReserveSlot(character))
                {
                    if (this.characters.Count <= GameServer.TcpConnection.MaxConnections)
                    {
                        if (this.characters.TryAdd(character.Session.Connection, character))
                        {
                            UpdateOnlineStatus(character.MasterId, true);
                            Program.Logger.WriteInfo("Registered character (ID:" + character.SessionId + ").");
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.slotManager.ReleaseSlot(character.Index);
                Program.Logger.WriteException(ex);
                return false;
            }
        }

        /// <summary>
        /// Unregisters a character from the game.
        /// </summary>
        /// <param name="character">The character to remove.</param>
        public void Unregister(Character character)
        {
            Unregister(character.Session.Connection);
        }

        /// <summary>
        /// Unregisters a character specified by it's node connection.
        /// </summary>
        /// <param name="node">The connection node associated with the character.</param>
        public void Unregister(Node node)
        {
            try
            {
                Character character = null;
                if (this.characters.TryRemove(node, out character))
                {
                    // TODO: save.
                    GameEngine.AccountWorker.Add(character);
                    this.slotManager.ReleaseSlot(character.Index);
                    UpdateOnlineStatus(character.MasterId, false);
                    Program.Logger.WriteInfo("Unregistered character (ID:" + character.SessionId + ").");
                }
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
        }

        /// <summary>
        /// Gets whether the collection contains the given node connection.
        /// </summary>
        /// <param name="node">The node connection to look for.</param>
        /// <returns>Returns true if contained; false if not.</returns>
        public bool Contains(Node node)
        {
            if (node != null)
            {
                return this.characters.ContainsKey(node);
            }
            return false;
        }

        /// <summary>
        /// Gets whether the collections contains the given character.
        /// </summary>
        /// <param name="character">The character to look for.</param>
        /// <returns>Returns true if contained; false if not.</returns>
        public bool Contains(Character character)
        {
            if (character != null)
            {
                return this.characters.Values.Contains(character);
            }
            return false;
        }

        /// <summary>
        /// Gets a character instance by it's master id.
        /// </summary>
        /// <param name="id">The master id of the character.</param>
        /// <returns>Returns a RuneScape.Model.Characters.Character object if the character exists; null otherwise.</returns>
        public Character Get(Node node)
        {
            if (node != null)
            {
                Character character = null;
                if (this.characters.TryGetValue(node, out character))
                {
                    return character;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks whether a character is online from the server's character collection.
        /// </summary>
        /// <param name="username">The username to check for.</param>
        /// <returns>Returns true if the character is online; false if offline.</returns>
        public bool OnlineByServer(string username)
        {
            foreach (Character c in this.characters.Values)
            {
                if (c != null && c.Name.Equals(username))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks whether a character is online from the server's 
        /// character collection specified by it's instance.
        /// </summary>
        /// <param name="character">The character to check for.</param>
        /// <returns>Returns true if the character is online; false if offline.</returns>
        public bool OnlineByServer(Character character)
        {
            return Contains(character);
        }

        /// <summary>
        /// Checks whether a character is online from the database.
        /// </summary>
        /// <param name="username">The username to check for.</param>
        /// <returns>Returns true if the character is online; false if offline.</returns>
        public bool OnlineByDatabase(string username)
        {
            DataRow result = null;
            using (SqlDatabaseClient client = GameServer.Database.GetClient())
            {
                client.AddParameter("username", username);
                result = client.ReadDataRow("SELECT online FROM characters WHERE username = @username LIMIT 1;");
            }

            if (result != null)
            {
                return (bool)result[0];
            }
            return false;
        }

        /// <summary>
        /// Updates a character's online status on the database.
        /// </summary>
        /// <param name="masterId">The character's master id.</param>
        /// <param name="online">The character's online status.
        ///     <para>true equals online; false equals offline.</para></param>
        public void UpdateOnlineStatus(uint masterId, bool online)
        {
            using (SqlDatabaseClient client = GameServer.Database.GetClient())
            {
                client.AddParameter("masterId", masterId);
                client.AddParameter("status", online ? 1 : 0);
                client.ExecuteUpdate("UPDATE characters SET online = @status WHERE id = @masterId;");
            }
        }

        /// <summary>
        /// Loads an account from the database.
        /// </summary>
        /// <param name="details">The character details to look at when loading.</param>
        public void LoadAccount(Details details, LoginConnectionType loginType)
        {
            string query = string.Empty;
            AccountLoadResult result = this.accountLoader.Load(details, loginType); // Try to load the account.
            GenericPacketComposer composer = new GenericPacketComposer(); // The packet going to be sent.

            // Try registering the user if return code is successful so far.
            if (result.ReturnCode == LoginReturnCode.Successful)
            {
                // The world is full.
                if (!Register(result.Character))
                {
                    result.ReturnCode = LoginReturnCode.WorldFull;
                }
            }

            composer.AppendByte((byte)result.ReturnCode);

            // We only need to send this if the login was successful.
            if (result.ReturnCode == LoginReturnCode.Successful)
            {
                composer.AppendByte((byte)result.Character.ClientRights);
                composer.AppendByte((byte)0);
                composer.AppendByte((byte)0);
                composer.AppendByte((byte)0);
                composer.AppendByte((byte)1);
                composer.AppendShort((short)result.Character.Index);
                composer.AppendByte((byte)1);

                if (this.logSessions)
                {
                    query += "UPDATE characters SET last_ip=@ip, last_signin=NOW() WHERE id = @id;";
                }
            }

            if (this.logAttempts)
            {
                query += "INSERT INTO login_attempts (username,date,ip,attempt) VALUES (@name, NOW(), @ip, @attempt);";
            }
            if (!result.Active)
            {
                query += "UPDATE characters SET active = '1' WHERE id = @id;";
            }
            if (query != string.Empty)
            {
                // Log the user's login attempt. This is useful for tracking hacking, ddos, etc.
                using (SqlDatabaseClient client = GameServer.Database.GetClient())
                {
                    client.AddParameter("id", result.Character.MasterId);
                    client.AddParameter("name", details.Username);
                    client.AddParameter("ip", details.Session.Connection.IPAddress);
                    client.AddParameter("attempt", result.ReturnCode.ToString());
                    client.ExecuteUpdate(query);
                }
            }

            // Send results to the client.
            result.Character.Session.SendData(composer.SerializeBuffer());

            // We can now welcome the player and send nessesary packets.
            if (result.ReturnCode == LoginReturnCode.Successful)
            {
                result.Character.Session.StartConnection();
                if (!result.Active)
                {
                    result.Character.Preferences.Add("just_started", true);
                }
                Frames.SendLoginWelcome(result.Character);
            }

            Program.Logger.WriteDebug(result.Character.Name + " returned " + result.ReturnCode + " at login attempt.");
        }
        #endregion Methods
    }
}
