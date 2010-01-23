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

using JoltEnvironment.Storage.Sql;
using JoltEnvironment.Utilities;
using RuneScape.Utilities.Filtering;


namespace RuneScape.Communication.Login.Handlers
{
    /// <summary>
    /// Handles the account name input.
    /// </summary>
    public class AccountNameProtocolHandler : IProtocolHandler
    {
        #region Fields
        /// <summary>
        /// The protocol id of this handle.
        /// </summary>
        public const int InterfaceId = 118;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the protocol request.
        /// </summary>
        /// <param name="request">The instance requesting the protcol handle.</param>
        public void Handle(LoginRequest request)
        {
            request.Buffer.Skip(1);
            if (request.Buffer.RemainingAmount >= 4)
            {
                long longName = request.Buffer.ReadLong();
                string name = StringUtilities.LongToString(longName);

                bool availible = false;

                // Check if the entered name is availible.
                using (SqlDatabaseClient client = GameServer.Database.GetClient())
                {
                    client.AddParameter("name", name);
                    DataRow row = client.ReadDataRow("SELECT id FROM characters WHERE username = @name LIMIT 1");

                    if (row == null)
                    {
                        availible = true;
                    }
                }

                // Check if the world allows account creation.
                if (GameEngine.World.AccountCreationEnabled)
                {
                    // Name lengths must be valid.
                    if (name != null && name.Length > 0 && name.Length < 13 && !name.ContainsBadWord())
                    {
                        if (availible)
                        {
                            request.Connection.SendData((byte)AccountCreationReturnCode.Good);
                        }
                        else
                        {
                            request.Connection.SendData((byte)AccountCreationReturnCode.AlreadyTaken);
                        }
                    }
                    else
                    {
                        request.Connection.SendData((byte)AccountCreationReturnCode.InvalidUsername);
                    }
                }
                else
                {
                    request.Connection.SendData((byte)AccountCreationReturnCode.Disabled);
                }
            }
        }
        #endregion Methods
    }
}
