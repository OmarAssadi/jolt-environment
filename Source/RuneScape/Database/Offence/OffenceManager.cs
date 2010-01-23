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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JoltEnvironment.Storage.Sql;

namespace RuneScape.Database.Offence
{
    /// <summary>
    /// Provides management for offence related content.
    /// </summary>
    public class OffenceManager
    {
        #region Methods
        /// <summary>
        /// Gets any offences that the character may have caused from the database.
        /// </summary>
        /// <param name="username">The character's username.</param>
        /// <returns>The offense type.</returns>
        public OffenceType GetOffence(uint id)
        {
            DataRow row;
            using (SqlDatabaseClient client = GameServer.Database.GetClient())
            {
                client.AddParameter("id", id);
                row = client.ReadDataRow("SELECT type,expire_date FROM offences WHERE userid = @id AND expired = '0' LIMIT 1;");
            }

            try
            {
                if (row != null)
                {
                    if (DateTime.Now >= (DateTime)row[1])
                    {
                        RemoveOffence(id);
                        return OffenceType.None;
                    }
                    Console.WriteLine(row[0]);
                    return (OffenceType)row[0];
                }
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
            return OffenceType.None;
        }

        /// <summary>
        /// Removes an offence off a character.
        /// </summary>
        /// <param name="username">The character's username.</param>
        private void RemoveOffence(uint id)
        {
            using (SqlDatabaseClient client = GameServer.Database.GetClient())
            {
                // Changes the expired value from 0 to 1 so that when offences are 
                // checked, the checker will know that this offence has expired.
                client.AddParameter("id", id);
                client.ExecuteUpdate("UPDATE offences SET expired = '1' WHERE userid = @id AND expired = '0';");
            }
        }
        #endregion Methods
    }
}
