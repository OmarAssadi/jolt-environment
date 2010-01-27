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

using JoltEnvironment.Storage.Sql;

namespace RuneScape.Database.Config
{
    /// <summary>
    /// Allows loading of configurations from a mysql database.
    /// </summary>
    public class MySqlConfigLoader : IConfigLoader
    {
        #region Methods
        /// <summary>
        /// Gets a set of configurations specified by the SQL query.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <returns>Returns a System.Collections.Generic.Dictionary object
        /// containing all configurations loadaed from the given location.</returns>
        public Dictionary<string, object> Load(string query)
        {
            DataTable data = null;
            Dictionary<string, dynamic> configs = new Dictionary<string, dynamic>();

            try
            {
                
                // Get the table from the database.
                using (SqlDatabaseClient client = GameServer.Database.GetClient())
                {
                    data = client.ReadDataTable(query);
                }

                // Only process if there's data.
                if (data != null && data.Rows.Count > 0)
                {
                    DataRow[] allRows = data.Select();

                    // Get the configurations.
                    foreach (DataRow row in allRows)
                    {
                        string key = row[1].ToString();
                        string type = row[2].ToString();
                        object value = row[3];

                        if (type.Equals("Boolean"))
                            configs.Add(key, Convert.ToBoolean(value));
                        else if (type.Equals("Byte"))
                            configs.Add(key, Convert.ToByte(value));
                        else if (type.Equals("Char"))
                            configs.Add(key, Convert.ToChar(value));
                        else if (type.Equals("Decimal"))
                            configs.Add(key, Convert.ToDecimal(value));
                        else if (type.Equals("Double"))
                            configs.Add(key, Convert.ToDouble(value));
                        else if (type.Equals("Int16"))
                            configs.Add(key, Convert.ToInt16(value));
                        else if (type.Equals("Int32"))
                            configs.Add(key, Convert.ToInt32(value));
                        else if (type.Equals("Int64"))
                            configs.Add(key, Convert.ToInt64(value));
                        else if (type.Equals("SByte"))
                            configs.Add(key, Convert.ToSByte(value));
                        else if (type.Equals("Single"))
                            configs.Add(key, Convert.ToSingle(value));
                        else if (type.Equals("String"))
                            configs.Add(key, Convert.ToString(value));
                        else if (type.Equals("UInt16"))
                            configs.Add(key, Convert.ToUInt16(value));
                        else if (type.Equals("UInt32"))
                            configs.Add(key, Convert.ToUInt32(value));
                        else if (type.Equals("UInt64"))
                            configs.Add(key, Convert.ToUInt64(value));
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
            return configs;
        }

        /// <summary>
        /// Loads a set of configurations from the specified table.
        /// </summary>
        /// <param name="table">The table to load configurations from.</param>
        /// <returns>Returns a System.Collections.Generic.Dictionary object
        /// containing all configurations loadaed from the given location.</returns>
        public Dictionary<string, object> LoadFromTable(string table)
        {
            return Load("SELECT * FROM " + table + "");
        }

        /// <summary>
        /// Loads a set of configurations form a specified table.
        /// </summary>
        /// <param name="section">The section to load configurations from.</param>
        /// <returns>Returns a System.Collections.Generic.Dictionary object
        /// containing all configurations loadaed from the given location.</returns>
        public Dictionary<string, object> LoadFromSection(string section)
        {
            return Load("SELECT * FROM configurations WHERE section = '" + section + "';");
        }
        #endregion Methods
    }
}
