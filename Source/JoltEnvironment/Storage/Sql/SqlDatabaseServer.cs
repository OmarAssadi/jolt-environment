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
using System.Net;
using System.Text;

namespace JoltEnvironment.Storage.Sql
{
    /// <summary>
    /// Represents credenitials for a SQL database access.
    /// </summary>
    public class SqlDatabaseServer
    {
        #region Properties
        /// <summary>
        /// Gets the database server's network host (IP address).
        /// </summary>
        public string Host { get; private set; }
        /// <summary>
        /// Gets the database server's tcp network port.
        /// </summary>
        public ushort Port { get; private set; }

        /// <summary>
        /// Gets the user's name which is used to gain access to the database.
        /// </summary>
        public string User { get; private set; }
        /// <summary>
        /// Gets the password corresponding with the connection's user.
        /// </summary>
        public string Password { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a SqlDatabaseServer object specified with connection details.
        /// </summary>
        /// <param name="host">The address of the which the database server is hosted.</param>
        /// <param name="port">The port at which the database server is listening from.</param>
        /// <param name="user">The username credition to gain access to database(s).</param>
        /// <param name="password">The username's corresponding password.</param>
        /// <exception cref="System.ArgumentException">Triggered when host address or user credinitial is empty.</exception>
        public SqlDatabaseServer(string host, ushort port, string user, string password)
        {
            if (host == null || host.Length < 1)
            {
                throw new ArgumentException("The host address cannot be empty.");
            }
            if (user == null || user.Length < 1)
            {
                throw new ArgumentException("The user credenitial cannot be empty.");
            }

            this.Host = host;
            this.Port = port;
            this.User = user;
            this.Password = password;
        }
        #endregion Constructors
    }
}
