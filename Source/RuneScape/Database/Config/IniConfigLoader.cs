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

namespace RuneScape.Database.Config
{
    /// <summary>
    /// Allows loading of configurations from a ini file.
    /// </summary>
    public class IniConfigLoader : IConfigLoader
    {
        #region Methods
        /// <summary>
        /// Loads a set of configurations from the specified file.
        /// </summary>
        /// <param name="location">The file location.</param>
        /// <returns>Returns a System.Collections.Generic.Dictionary object
        /// containing all configurations loadaed from the given location.</returns>
        public Dictionary<string, dynamic> Load(string location)
        {
            if (!File.Exists(location))
                throw new FileNotFoundException("File at path \"" + location + "\" does not exist.");

            Dictionary<string, dynamic> configs = new Dictionary<string, dynamic>();

            using (StreamReader reader = new StreamReader(location))
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
                        if (!configs.ContainsKey(key))
                        {
                            try
                            {
                                string type = line.Substring(0, indexOfDelimiter).Split(':')[0];
                                string value = line.Substring(indexOfDelimiter + 3).Trim();

                                if (type.Equals("Boolean"))
                                    configs.Add(key, Boolean.Parse(value));
                                else if (type.Equals("Byte"))
                                    configs.Add(key, Byte.Parse(value));
                                else if (type.Equals("Char"))
                                    configs.Add(key, Char.Parse(value));
                                else if (type.Equals("Decimal"))
                                    configs.Add(key, Decimal.Parse(value));
                                else if (type.Equals("Double"))
                                    configs.Add(key, Double.Parse(value));
                                else if (type.Equals("Int16"))
                                    configs.Add(key, Int16.Parse(value));
                                else if (type.Equals("Int32"))
                                    configs.Add(key, Int32.Parse(value));
                                else if (type.Equals("Int64"))
                                    configs.Add(key, Int64.Parse(value));
                                else if (type.Equals("SByte"))
                                    configs.Add(key, SByte.Parse(value));
                                else if (type.Equals("Single"))
                                    configs.Add(key, Single.Parse(value));
                                else if (type.Equals("String"))
                                    configs.Add(key, value);
                                else if (type.Equals("UInt16"))
                                    configs.Add(key, UInt16.Parse(value));
                                else if (type.Equals("UInt32"))
                                    configs.Add(key, UInt32.Parse(value));
                                else if (type.Equals("UInt64"))
                                    configs.Add(key, UInt64.Parse(value));
                            }
                            catch (Exception ex)
                            {
                                Program.Logger.WriteException(ex);
                            }
                        }
                    }
                }
            }
            return configs;
        }
        #endregion Methods
    }
}
