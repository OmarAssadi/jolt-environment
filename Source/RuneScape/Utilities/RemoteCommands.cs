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
    /// Represents a single command function.
    /// </summary>
    /// <param name="arguments">Arguments of the command.</param>
    public delegate string CommandFunc(string[] arguments);

    /// <summary>
    /// Commands that can be triggered via remote connections.
    /// </summary>
    public static class RemoteCommands
    {
        #region Fields
        /// <summary>
        /// A Dictionary that contains all the commands availible.
        /// </summary>
        private static Dictionary<string, CommandFunc> commands = new Dictionary<string, CommandFunc>();
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles remote commands.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="arguments">Arguments following the command.</param>
        public static string Handle(string command, string[] arguments)
        {
            try
            {
                if (commands.ContainsKey(command))
                {
                    return commands[command](arguments);
                }
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
            return "0";
        }

        /// <summary>
        /// Loads all commands.
        /// </summary>
        public static void LoadCommands()
        {
            commands.Add("test", Test);
        }

        /// <summary>
        /// Test command.
        /// </summary>
        /// <param name="arguments">Arguments passed through for the command.</param>
        private static string Test(string[] arguments)
        {
            return new Random().NextDouble().ToString();
        }
        #endregion Methods
    }
}
