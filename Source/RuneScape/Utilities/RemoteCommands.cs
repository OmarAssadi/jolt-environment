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

namespace RuneScape.Utilities
{
    /// <summary>
    /// Commands that can be triggered via remote connections.
    /// </summary>
    public static class RemoteCommands
    {
        #region Methods
        /// <summary>
        /// Handles remote commands.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="arguments">Arguments following the command.</param>
        public static string Handle(string command, string[] arguments)
        {
            return "";
        }
        #endregion Methods
    }
}
