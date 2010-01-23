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
    /// A query executed upon the connected sql database.
    /// </summary>
    public interface ISqlDataQuery
    {
        /// <summary>
        /// Executes the ISqlDataQuery upon the database client, which returns a value.
        /// </summary>
        /// <param name="client">The client to execute query on.</param>
        /// <returns>Returns any values from the query.</returns>
        object Execute(SqlDatabaseClient client);
        /// <summary>
        /// Executes an update ISqlDataQuery upon the database client.
        /// </summary>
        /// <param name="client">The client to execute query on.</param>
        void ExecuteUpdate(SqlDatabaseClient client);
    }
}
