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
using System.Data;

using JoltEnvironment.Storage.Sql;
using RuneScape.Communication.Login;
using RuneScape.Database.Offence;
using RuneScape.Model;
using RuneScape.Model.Characters;
using RuneScape.Model.Items;

namespace RuneScape.Database.Account
{
    /// <summary>
    /// Represents a loader that loads accounts from a mysql database.
    /// </summary>
    public class SqlAccountLoader : IAccountLoader
    {
        #region Methods
        /// <summary>
        /// Loads an account from the mysql database using the given character details.
        /// </summary>
        /// <param name="details">The details used to load the account.</param>
        /// <returns>Returns the account load result.</returns> 
        public AccountLoadResult Load(Details details, LoginConnectionType loginType)
        {
            AccountLoadResult result = new AccountLoadResult();
            if (GameEngine.World.SystemUpdate)
            {
                result.Character = new Character(details, 0);
                result.ReturnCode = LoginReturnCode.SystemUpdate;
            }
            try
            {
                DataRow data = null;
                using (SqlDatabaseClient client = GameServer.Database.GetClient())
                {
                    /* 
                     * Checks if the character exists in the database. It also 
                     * checks if the given password matches the stored password.
                     */
                    client.AddParameter("username", details.Username);
                    client.AddParameter("password", details.Password);
                    data = client.ReadDataRow("SELECT * FROM characters LEFT JOIN (character_preferences) ON (characters.id = character_preferences.master_id) WHERE username = @username AND password = @password LIMIT 1;");
                    //LEFT JOIN (character_appearance) ON (characters.id = character_appearance.master_id)
                }

                if (data != null) // Meaning the character exists, and the password is correct.
                {
                    result.Character = new Character(details, (uint)data[0]);

                    // If a character is offensive, set the proper penalties.
                    OffenceType offence = (OffenceType)GameEngine.World.OffenseManager.GetOffence(result.Character.MasterId);
                    if (offence == OffenceType.Banned) // If the character is banned, flag and end this request.
                    {
                        result.ReturnCode = LoginReturnCode.AccountDisabled;
                    }
                    if (offence == OffenceType.Muted) // If the character is muted, we will mute this character.
                    {
                        result.Character.Muted = true;
                    }

                    /*
                     * Only check if it's a new connection, as reconnections try
                     * connnecting to the server before the older session is removed,
                     * so we must ignore whether or not the character is online.
                     */
                    if (loginType != LoginConnectionType.Reconnection && (bool)data[5])
                    {
                        result.ReturnCode = LoginReturnCode.AlreadyOnline;
                    }

                    /*
                     * We only want to assign the character details loaded from 
                     * the database if the player has passed though security.
                     */
                    if (result.ReturnCode == LoginReturnCode.Successful)
                    {
                        // Core info.
                        result.Character.ClientRights = (ClientRights)data[3];
                        result.Character.ServerRights = (ServerRights)data[4];
                        result.Active = (bool)data[6];

                        // Appearance.
                        result.Character.Appearance.Gender = (Gender)data[14];
                        result.Character.Appearance.Head = (short)data[15];
                        result.Character.Appearance.Torso = (short)data[16];
                        result.Character.Appearance.Arms = (short)data[17];
                        result.Character.Appearance.Wrist = (short)data[18];
                        result.Character.Appearance.Legs = (short)data[19];
                        result.Character.Appearance.Feet = (short)data[20];
                        result.Character.Appearance.Beard = (short)data[21];
                        result.Character.Appearance.HairColor = (byte)data[22];
                        result.Character.Appearance.TorsoColor = (byte)data[23];
                        result.Character.Appearance.LegColor = (byte)data[24];
                        result.Character.Appearance.FeetColor = (byte)data[25];
                        result.Character.Appearance.SkinColor = (byte)data[26];
                        
                        // Location.
                        result.Character.Location = Location.Create((short)data[27], (short)data[28], (byte)data[29]);

                        // Energy.
                        result.Character.WalkingQueue.RunEnergy = (byte)data[30];

                        // Containers.
                        if (data[31] is string)
                            result.Character.Inventory.Deserialize((string)data[31]);
                        if (data[32] is string)
                            result.Character.Equipment.Deserialize((string)data[32]);
                        if (data[33] is string)
                            result.Character.Bank.Deserialize((string)data[33]);

                        // Friends and ignores
                        if (data[34] is string)
                            result.Character.Contacts.DeserializeFriends((string)data[34]);
                        if (data[35] is string)
                            result.Character.Contacts.DeserializeIgnores((string)data[35]);

                        // Preferences.
                        result.Character.Preferences.SingleMouse = (bool)data[37];
                        result.Character.Preferences.DisableChatEffects = (bool)data[38];
                        result.Character.Preferences.SplitChat = (bool)data[39];
                        result.Character.Preferences.AcceptAid = (bool)data[40];
                    }
                }
                else // User doesn't exist or password is wrong.
                {
                    result.Character = new Character(details, 0);
                    result.ReturnCode = LoginReturnCode.WrongPassword;
                }
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
                result.Character = new Character(details, 0);
                result.ReturnCode = LoginReturnCode.BadSession;
            }
            return result;
        }
        #endregion Methods
    }
}
