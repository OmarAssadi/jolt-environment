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
    /// Composes the a head on interface packet.
    /// 
    ///     <para>The head is shown on an interface.</para>
    /// </summary>
    public class HeadOnInterfacePacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs a packet that shows the character's head on the specified interface/child.
        /// </summary>
        /// <param name="interfaceId">The interface to show character's head on.</param>
        /// <param name="childId">Specific spot within the interface where the head should be shown.</param>
        public HeadOnInterfacePacketComposer(short interfaceId, short childId)
        {
            SetOpcode(101);
            AppendShort(interfaceId);
            AppendShort(childId);
        }
        #endregion Constructors
    }
}
