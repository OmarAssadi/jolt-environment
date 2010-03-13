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
    /// Composes a packet that sends character options that a 
    /// user can choose when right clicking another character.
    /// </summary>
    public class CharacterOptionPacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs the packet composer.
        /// </summary>
        /// <param name="text">The text shown for the option.</param>
        /// <param name="slot">The option slot (the order the option will be shown).</param>
        /// <param name="top">Whether the option is above all other options.</param>
        public CharacterOptionPacketComposer(string text, byte slot, bool top)
        {
            SetOpcode(252);
            SetType(PacketType.Byte);
            AppendByteC((byte)(top ? 1 : 0));
            AppendString(text);
            AppendByteC(slot);
        }
        #endregion Constructors
    }
}
