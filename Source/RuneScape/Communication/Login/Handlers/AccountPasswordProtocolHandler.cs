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
using System.Data;
using System.Linq;
using System.Text;

using JoltEnvironment.Security;
using JoltEnvironment.Storage.Sql;
using JoltEnvironment.Utilities;
using RuneScape.Communication.Messages;

namespace RuneScape.Communication.Login.Handlers
{
    /// <summary>
    /// Handles the account password / confirmation part of account creation.
    /// </summary>
    public class AccountPasswordProtocolHandler : IProtocolHandler
    {
        #region Fields
        /// <summary>
        /// The protocol id of this handle.
        /// </summary>
        public const int InterfaceId = 48;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the protocol request.
        /// </summary>
        /// <param name="request">The instance requesting the protcol handle.</param>
        public void Handle(LoginRequest request)
        {
            request.Buffer.Skip(1);
            byte packetSize = 0;
            if (request.Buffer.RemainingAmount >= 1)
            {
                packetSize = request.Buffer.ReadByte();
            }

            if (request.Buffer.RemainingAmount >= packetSize)
            {
                Packet p = new Packet(request.Buffer.RemainingData);
                p.Skip(4); // RSA Encrpytion.

                // Check if client revision is valid.
                int clientVersion = p.ReadShort();
                if (clientVersion != 508)
                {
                    request.Remove = true;
                    return;
                }

                long longUser = p.ReadLong();
                string username = StringUtilities.LongToString(longUser);
                p.Skip(4); // PADDING
                string password = p.ReadString();
                if (password.Contains(username))
                {
                    request.Connection.SendData((byte)AccountCreationReturnCode.TooSimilar);
                    return;
                }
                if (password.Length < 5 || password.Length > 20)
                {
                    request.Connection.SendData((byte)AccountCreationReturnCode.InvalidLength);
                    return;
                }

                /*
                 * Security is very important when coming to dealing with passwords, 
                 * hence why jolt environment hashes the username and password.
                 */
                string hash = Hash.GetHash(username + Hash.GetHash(password, HashType.SHA1), HashType.SHA1);
                p.Skip(6); // Padding(?)
                byte birthDay = p.ReadByte();
                byte birthMonth = (byte)(p.ReadByte() + 1);
                p.Skip(4); // Padding(?)
                short birthYear = p.ReadShort();
                short country = p.ReadShort();
                p.Skip(4); // Unknown.

                /*
                 * We now attempt to finalize the creation, by rechecking if username 
                 * is availible and inserting the given information to the database.
                 */
                using (SqlDatabaseClient client = GameServer.Database.GetClient())
                {
                    client.AddParameter("username", username);
                    DataRow row = client.ReadDataRow("SELECT id FROM characters WHERE username = @username LIMIT 1");

                    /*
                     * If the row isn't null, that means that the database 
                     * has found a character with the same username.
                     */
                    if (row != null)
                    {
                        request.Connection.SendData((byte)AccountCreationReturnCode.AlreadyTaken);
                        return;
                    }
                    else
                    {
                        client.AddParameter("password", hash); // Insert the hashed password for security.
                        client.AddParameter("dob", birthDay + "-" + birthMonth + "-" + birthYear);
                        client.AddParameter("country", country);
                        client.AddParameter("ip", request.Connection.IPAddress);
                        client.ExecuteUpdate("INSERT INTO characters (username,password,dob,country,register_ip,register_date) VALUES (@username, @password, @dob, @country, @ip, NOW());");
                        
                        // Now that the character is now registered to the core table, we can now grab the auto incremented id.
                        //dbClient.AddParamWithValue("id", dbClient.ReadUInt32("SELECT id FROM characters WHERE username = @username"));
                        request.Connection.SendData((byte)AccountCreationReturnCode.Good);
                        return;
                    }
                }
            }
        }
        #endregion Methods
    }
}
