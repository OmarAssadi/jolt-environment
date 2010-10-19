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

using RuneScape.Events;
using RuneScape.Model;
using RuneScape.Model.Characters;
using RuneScape.Model.Objects;

namespace RuneScape.Communication.Messages.Incoming
{    
    /// <summary>
    /// Handler for object options packets.
    /// </summary>
    public class ObjectOptionsPacketHandler : IPacketHandler
    {
        #region Methods
        /// <summary>
        /// Handles the item options.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            switch (packet.Opcode)
            {
                case 158:
                    {
                        HandleOption1(character, packet);
                        break;
                    }
            }
        }

        /// <summary>
        /// Handles the first option.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        private void HandleOption1(Character character, Packet packet)
        {
            short x = (short)(packet.ReadLEShort() & 0xFFFF);
            ushort id = (ushort)(packet.ReadShort() & 0xFFFF);
            short y = (short)(packet.ReadLEShortA() & 0xFFFF);
            Location location = Location.Create(x, y, character.Location.Z);
            GameEngine.Events.RegisterCoordinateEvent(new WalkToObjectEvent(character, location, id));
        }
        #endregion Methods
    }
}
