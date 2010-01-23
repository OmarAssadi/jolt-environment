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

namespace RuneScape.Utilities.Filtering
{
    /// <summary>
    /// Provides methods to filter bad words in game.
    /// </summary>
    public static class BadWords
    {
        #region Fields
        /// <summary>
        /// List of bad words.
        /// </summary>
        public static List<string> badWords = new List<string>();
        #endregion Fields

        #region Methods
        /// <summary>
        /// Loads bad words from database.
        /// </summary>
        /// <returns></returns>
        public static void Load()
        {
            DataRow[] rows = null;
            using (SqlDatabaseClient client = GameServer.Database.GetClient())
            {
                DataTable table = client.ReadDataTable("SELECT word FROM bad_words");
                rows = table.Select();
            }

            foreach (DataRow row in rows)
            {
                badWords.Add(Convert.ToString(row[0]));
            }
        }

        /// <summary>
        /// Extenstion for a string to check bad words.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsBadWord(this string value)
        {
            foreach (string s in badWords)
            {
                if (value.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion Methods
    }
}
