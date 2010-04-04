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

using RuneScape.Content.Trading;
using RuneScape.Events;
using RuneScape.Model.Characters;

namespace RuneScape.Communication.Messages.Incoming
{
    /// <summary>
    /// Handler for character options (shown when right click player).
    /// </summary>
    public class CharacterOptionsPacketHandler : IPacketHandler
    {
        #region Methods
        /// <summary>
        /// Handles character options.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            switch (packet.Opcode)
            {
                case 37:
                    TradeOption(character, packet);
                    break;
            }
        }

        /// <summary>
        /// Handles the trade option.
        /// </summary>
        /// <param name="character">The character wishing to trade.</param>
        /// <param name="packet">The packet containing handle data.</param>
        private void TradeOption(Character character, Packet packet)
        {
            short id = (short)(packet.ReadShort() & 0xFFFF);
            Character other = GameEngine.World.CharacterManager.Get(id);

            if (other != null)
            {
                GameEngine.Events.RegisterCoordinateEvent(new RequestTradeEvent(character, other));
            }
        }
        #endregion Methods
    }
}
