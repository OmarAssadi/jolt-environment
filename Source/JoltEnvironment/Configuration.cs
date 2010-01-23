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
using System.IO;
using System.Linq;
using System.Text;

namespace JoltEnvironment
{
    /// <summary>
    /// Represents a set of configurations loaded from a file.
    /// 
    ///     <para>Used to load configurations from a .conf file 
    ///     containing the settings and configurations needed.</para>
    /// </summary>
    public class Configuration
    {
        #region Fields
        /// <summary>
        /// A System.Connections.Generic.Dictionary object for storing configurations.
        /// </summary>
        private Dictionary<string, dynamic> configurations = new Dictionary<string, dynamic>();
        #endregion Fields

        #region Properites
        /// <summary>
        /// Gets the configurations that were loaded from a configuration file.
        /// </summary>
        /// <param name="key">The key of the configuration.</param>
        /// <returns>Returns the value associated with the key.</returns>
        public dynamic this[string key]
        {
            get { return this.configurations[key]; }
        }
        #endregion Properites

        #region Methods
        /// <summary>
        /// Loads a set of configurations that were within the given file.
        /// </summary>
        /// <param name="filePath">The file to load.</param>
        /// <returns>Returns a new JoltEnvironment.Configuration object.</returns>
        public static Configuration Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File at path \"" + filePath + "\" does not exist.");

            Configuration config = new Configuration();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length == 0 || line[0] == '#')
                        continue; // Line is empty/a comment.

                    int indexOfDelimiter = line.IndexOf(" = ");
                    if (indexOfDelimiter != -1)
                    {
                        string key = line.Substring(0, indexOfDelimiter).Split(':')[1];
                        if (!config.configurations.ContainsKey(key))
                        {
                            try
                            {
                                string type = line.Substring(0, indexOfDelimiter).Split(':')[0];
                                string value = line.Substring(indexOfDelimiter + 3).Trim();

                                if (type.Equals("Boolean"))
                                    config.configurations.Add(key, Boolean.Parse(value));
                                else if (type.Equals("Byte"))
                                    config.configurations.Add(key, Byte.Parse(value));
                                else if (type.Equals("Char"))
                                    config.configurations.Add(key, Char.Parse(value));
                                else if (type.Equals("Decimal"))
                                    config.configurations.Add(key, Decimal.Parse(value));
                                else if (type.Equals("Double"))
                                    config.configurations.Add(key, Double.Parse(value));
                                else if (type.Equals("Int16"))
                                    config.configurations.Add(key, Int16.Parse(value));
                                else if (type.Equals("Int32"))
                                    config.configurations.Add(key, Int32.Parse(value));
                                else if (type.Equals("Int64"))
                                    config.configurations.Add(key, Int64.Parse(value));
                                else if (type.Equals("SByte"))
                                    config.configurations.Add(key, SByte.Parse(value));
                                else if (type.Equals("Single"))
                                    config.configurations.Add(key, Single.Parse(value));
                                else if (type.Equals("String"))
                                    config.configurations.Add(key, value);
                                else if (type.Equals("UInt16"))
                                    config.configurations.Add(key, UInt16.Parse(value));
                                else if (type.Equals("UInt32"))
                                    config.configurations.Add(key, UInt32.Parse(value));
                                else if (type.Equals("UInt64"))
                                    config.configurations.Add(key, UInt64.Parse(value));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                    }
                }
                reader.Dispose();
            }
            return config;
        }
        #endregion Methods
    }
}
