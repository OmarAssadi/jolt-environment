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

namespace RuneScape.Communication.Messages.Outgoing
{
    /// <summary>
    /// Composes a script that is ran via the client.
    /// </summary>
    public class RunScriptPacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs a client script packet
        /// </summary>
        /// <param name="scriptId">The id of the script to run.</param>
        /// <param name="arguments">The aruguments.</param>
        /// <param name="argumentTypes">Argument types</param>
        /// <exception cref="System.ArgumentException">Thrown when the number 
        /// of arguments does not match the number of argument types.</exception>
        public RunScriptPacketComposer(int scriptId, string argumentTypes, params object[] arguments)
        {
            if (arguments.Length != argumentTypes.Length)
            {
                throw new ArgumentException("Arguments length mismatch.");
            }

            SetOpcode(152);
            SetType(PacketType.Short);
            AppendString(argumentTypes);

            int j = 0;
            for (int i = (argumentTypes.Length - 1); i >= 0; i--, j++)
            {
                if (argumentTypes[i] == 's')
                {
                    AppendString((string)arguments[j]);
                }
                else
                {
                    AppendInt((int)arguments[j]);
                }
            }
            AppendInt(scriptId);
        }
        #endregion Constructors
    }
}
