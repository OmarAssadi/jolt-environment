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

using JoltEnvironment.Storage.Sql;

namespace RuneScape.Utilities
{
    /// <summary>
    /// Provides functions for dealing with database versions.
    /// </summary>
    public class DatabaseVersionCheck : ISqlDataQuery
    {
        #region Methods
        /// <summary>
        /// Attempts to check the database verson via the database to see if it's valid for use.
        /// </summary>
        /// <param name="client">The client providing the connection to database.</param>
        /// <returns>Returns true if valid; false if not.</returns>
        public object Execute(SqlDatabaseClient client)
        {
            client.AddParameter("sver", Program.Version.ToString());
            string dbResult = (string)client.ExecuteQuery(
                        "SELECT database_version FROM versioning; "
                        + "UPDATE versioning SET server_version = @sver;");

            Version required = new Version(1, 0, 3100);
            Version dbVersion = Version.Parse(dbResult);
            if (dbVersion < required)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Stub.
        /// </summary>
        /// <param name="client">The client providing the connection to database.</param>
        public void ExecuteUpdate(SqlDatabaseClient client)
        {
            throw new NotImplementedException();
        }
        #endregion Methods
    }
}
