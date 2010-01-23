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

namespace JoltEnvironment.Storage.Sql
{
    /// <summary>
    /// A DataObject stored in a SQL database.
    /// </summary>
    public interface ISqlDataObject
    {
        /// <summary>
        /// Inserts the ISqlDataObject into the database via the client.
        /// </summary>
        /// <param name="client">The client to execute query on.</param>
        /// <returns>Returns true if the client has successfully executed the query.</returns>
        bool Insert(SqlDatabaseClient client);
        /// <summary>
        /// Updaets the ISqlDataObject into the database via the client.
        /// </summary>
        /// <param name="client">The client to execute query on.</param>
        /// <returns>Returns true if the client has successfully executed the query.</returns>
        bool Update(SqlDatabaseClient client);
        /// <summary>
        /// Deletes the ISqlDataObject into the database via the client.
        /// </summary>
        /// <param name="client">The client to execute query on.</param>
        /// <returns>Returns true if the client has successfully executed the query.</returns>
        bool Delete(SqlDatabaseClient client);
        /// <summary>
        /// Loads the ISqlDataObject into the database via the client.
        /// </summary>
        /// <param name="client">The client to execute query on.</param>
        /// <returns>Returns true if the client has successfully executed the query.</returns>
        bool Load(SqlDatabaseClient client);
    }
}
