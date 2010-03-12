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

using RuneScape.Content.ClanChat;
using RuneScape.Model.Characters;
using RuneScape.Communication.Messages.Outgoing;

namespace RuneScape.Communication.Messages.Incoming
{
    /// <summary>
    /// Handler for kicking clan members.
    /// </summary>
    public class KickUserPacketHandler : IPacketHandler
    {
        #region Methods
        /// <summary>
        /// Handles kicking of clan members.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            long name = packet.ReadLong();
            GameEngine.Content.ClanChat.Leave(GameEngine.World.CharacterManager.Get(name));
            character.Session.SendData(new MessagePacketComposer(
                "You have been kicked out of the channel.").Serialize()); 
        }
        #endregion Methods
    }
}
