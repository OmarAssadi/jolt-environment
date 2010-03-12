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

using RuneScape.Communication.Messages;
using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Content.ClanChat;
using RuneScape.Model.Characters;

namespace RuneScape.Content.Interfaces
{
    /// <summary>
    /// Represents the clan chat tab located on the sidebar.
    /// </summary>
    public class ClanChatTab : IInterfaceHandler
    {
        #region Fields
        /// <summary>
        /// The id of this interface.
        /// </summary>
        public const int InterfaceId = 589;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the clanchat tab buttons.
        /// </summary>
        /// <param name="character">The character to handle for.</param>
        /// <param name="packetId">The packet id of the button.</param>
        /// <param name="buttonId">The button to handle.</param>
        /// <param name="buttonId2">The secondary button to handle.</param>
        public void HandleButton(Character character, int packetId, int buttonId, int buttonId2)
        {
            switch (buttonId)
            {
                case 9:
                    Frames.SendInterface(character, 590, false);
                    Room room = GameEngine.Content.ClanChat.Get(character.LongName);

                    if (room != null)
                    {
                        character.Session.SendData(new StringPacketComposer(room.StringName, 590, 22).Serialize());
                        character.Session.SendData(new StringPacketComposer(Room.RankToString(room.JoinReq), 590, 23).Serialize());
                        character.Session.SendData(new StringPacketComposer(Room.RankToString(room.TalkReq), 590, 24).Serialize());
                        character.Session.SendData(new StringPacketComposer(Room.RankToString(room.KickReq), 590, 25).Serialize());
                    }
                    
                    break;
                default:
                    Program.Logger.WriteDebug("Unhandled clanchat button: " + buttonId);
                    break;
            }
        }
        #endregion Methods
    }
}
