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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JoltEnvironment.Storage.Sql;

namespace RuneScape.Database.Config
{
    /// <summary>
    /// Represents a collection of sql configurations.
    /// </summary>
    public class SqlConfig
    {
        #region Fields
        /// <summary>
        /// A dictionary containing configurations.
        /// </summary>
        private Dictionary<string, object> configs;
        /// <summary>
        /// An object used to sychronization.
        /// </summary>
        private object syncObj = new object();

        /// <summary>
        /// Loader for configurations.
        /// </summary>
        private static SqlConfigLoader loader = new SqlConfigLoader();
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets or sets a specified key/value pair.
        /// </summary>
        /// <param name="key">The key to get/set.</param>
        /// <returns>Returns the specified key's value.</returns>
        public object this[string key]
        {
            get
            {
                if (configs.ContainsKey(key))
                {
                    return configs[key];
                }
                return null;
            }
            set
            {
                if (configs.ContainsKey(key))
                {
                    using (SqlDatabaseClient client = GameServer.Database.GetClient())
                    {
                        client.AddParameter("k", key);
                        client.AddParameter("v", value);
                        client.ExecuteUpdate("UPDATE configurations SET configurations.value = @v WHERE configurations.key = @k;");
                    }
                    this.configs[key] = value;
                }
                else
                {
                    Console.WriteLine(0);
                }
            }
        }


        public int Count
        {
            get
            {
                return this.configs.Count;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this.syncObj;
            }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new SqlConfig collection.
        /// </summary>
        /// <param name="configs">Collection of configs to </param>
        public SqlConfig(Dictionary<string, object> configs)
        {
            this.configs = configs;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Loads a set of configurations from the specified table.
        /// </summary>
        /// <param name="table">The table to load configurations from.</param>
        /// <returns>Returns a System.Collections.Generic.Dictionary object
        /// containing all configurations loadaed from the given location.</returns>
        public static SqlConfig LoadFromTable(string table)
        {
            return loader.Load("SELECT * FROM " + table + "");
        }

        /// <summary>
        /// Loads a set of configurations form a specified table.
        /// </summary>
        /// <param name="section">The section to load configurations from.</param>
        /// <returns>Returns a System.Collections.Generic.Dictionary object
        /// containing all configurations loadaed from the given location.</returns>
        public static SqlConfig LoadFromSection(string section)
        {
            return loader.Load("SELECT * FROM configurations WHERE section = '" + section + "';");
        }
        #endregion Methods
    }
}
