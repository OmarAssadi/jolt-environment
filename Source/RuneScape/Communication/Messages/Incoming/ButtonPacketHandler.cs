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

namespace RuneScape.Communication.Messages.Incoming
{
    /// <summary>
    /// Handler for the button packets.
    /// </summary>
    public class ButtonPacketHandler : IPacketHandler
    {
        #region Methods
        /// <summary>
        /// Handles the buttons pressed by the character.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            int interfaceId = packet.ReadShort() & 0xFFFF;
            int buttonId = packet.ReadShort() & 0xFFFF;
            int buttonId2 = 0;

            if (packet.Length >= 6)
            {
                buttonId2 = packet.ReadShort() & 0xFFFF;
            }
            if (buttonId2 == 65535)
            {
                buttonId2 = 0;
            }

            GameEngine.Content.InterfaceManager.Handle(character, packet.Opcode, buttonId, buttonId2, interfaceId);
        }
        #endregion Methods
    }
}
