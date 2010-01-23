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

using RuneScape.Model.Characters;

namespace RuneScape.Communication.Messages.Outgoing
{
    /// <summary>
    /// Composes a string.
    /// </summary>
    public class StringPacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs a text shown on an interface.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="interfaceId">The id of interface to show text on.</param>
        /// <param name="childId">The id of the interface's child to show text on.</param>
        public StringPacketComposer(string text, short interfaceId, short childId)
        {
            int stringSize = text.Length + 5;
            SetOpcode(179);
            AppendByte((byte)(stringSize / 256));
            AppendByte((byte)(stringSize % 256));
            AppendString(text);
            AppendShort(childId);
            AppendShort(interfaceId);
        }
        #endregion Constructors
    }
}
