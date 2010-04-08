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
    /// Composes a packet that creates a access mask.
    /// </summary>
    public class AccessMaskPacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs the packet composer.
        /// </summary>
        /// <param name="set">The access mask set.</param>
        /// <param name="window">The access mask window.</param>
        /// <param name="interfaceId">The access mask interface.</param>
        /// <param name="offset">The access mask offset.</param>
        /// <param name="length">The access mask length.</param>
        public AccessMaskPacketComposer(short set, short window, short interfaceId, short offset, short length)
        {
            SetOpcode(223);
            AppendShort(length);
            AppendLEShortA(offset);
		    AppendLEShort(window);
            AppendLEShort(interfaceId);
            AppendLEShort(set);
            AppendLEShort(0);
        }
        #endregion Constructors
    }
}
