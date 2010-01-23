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

namespace RuneScape.Database.Config
{
    /// <summary>
    /// Represents the the loading point of a configuration set.
    /// </summary>
    public static class Configuration
    {
        #region Fields
        /// <summary>
        /// Configurations loading from MySQL.
        /// </summary>
        private static MySqlConfigLoader mysqlConfigLoader = new MySqlConfigLoader();
        /// <summary>
        /// Configurations loading from INI.
        /// </summary>
        private static IniConfigLoader iniConfigLoader = new IniConfigLoader();
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the MySQL configuration loader.
        /// </summary>
        public static MySqlConfigLoader MySql { get { return mysqlConfigLoader; } }
        /// <summary>
        /// Gets the INI configuration loader.
        /// </summary>
        public static IniConfigLoader Ini { get { return iniConfigLoader; } }
        #endregion Properties
    }
}
