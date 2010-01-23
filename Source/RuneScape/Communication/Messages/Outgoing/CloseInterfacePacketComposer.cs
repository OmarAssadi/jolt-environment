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
    /// Composes a close interface packet.
    /// </summary>
    public class CloseInterfacePacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs a new close interface packet.
        /// </summary>
        /// <param name="windowId">The window id of the interface.</param>
        /// <param name="positionId">The position of the interface.</param>
        public CloseInterfacePacketComposer(int windowId, int position)
        {
            SetOpcode(246);
            AppendInt(windowId << 16 | position);
        }
        #endregion Constructors
    }
}
