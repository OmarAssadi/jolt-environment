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

using JoltEnvironment.Debug;

namespace JoltEnvironment
{
    /// <summary>
    /// Jolt Environment main class. 
    /// </summary>
    public static class JEnvironment
    {
        #region Properties
        /// <summary>
        /// Gets the local environment logger.
        /// </summary>
        public static Logger Logger { get; private set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Sets up the logger with specified conditions.
        /// </summary>
        /// <param name="logger"></param>
        public static void SetupLogger(Logger logger)
        {
            Logger = logger;
        }
        #endregion Methods
    }
}
