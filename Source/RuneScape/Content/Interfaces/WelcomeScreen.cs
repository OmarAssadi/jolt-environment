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
using RuneScape.Communication.Messages.Outgoing;

namespace RuneScape.Content.Interfaces
{
    /// <summary>
    /// Represents the welcome screen shown at login.
    /// </summary>
    public class WelcomeScreen : IInterfaceHandler
    {
        #region Fields
        /// <summary>
        /// The id of this interface.
        /// </summary>
        public const int InterfaceId = 378;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the welcome screen buttons.
        /// </summary>
        /// <param name="character">The character to handle for.</param>
        /// <param name="packetId">The packet id of the button.</param>
        /// <param name="buttonId">The button to handle.</param>
        /// <param name="buttonId2">The secondary button to handle.</param>
        public void HandleButton(Character character, int packetId, int buttonId, int buttonId2)
        {
            switch (buttonId)
            {
                case 140:
                    {
                        if (character.Preferences["just_started"] != null)
                        {
                            CharacterSetup.Show(character);
                        }
                        character.Session.SendData(new WindowPanePacketComposer(548).Serialize());
                        break;
                    }
                default:
                    {
                        if (character.ServerRights >= ServerRights.SystemAdministrator)
                        {
                            Program.Logger.WriteDebug("Unhandled welcome screen button: " + buttonId);
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Shows the welcome interface.
        /// </summary>
        public static void Show(Character character)
        {
            character.Session.SendData(new WindowPanePacketComposer(549).Serialize());
            character.Session.SendData(new InterfacePacketComposer(1, 549, 2, 378).Serialize());
            character.Session.SendData(new InterfacePacketComposer(1, 549, 3, 17).Serialize());
            character.Session.SendData(new StringPacketComposer("", 378, 116).Serialize());
            character.Session.SendData(new StringPacketComposer("0", 378, 39).Serialize());
            character.Session.SendData(new StringPacketComposer("0", 378, 96).Serialize());
            character.Session.SendData(new StringPacketComposer("0 unread messages", 378, 37).Serialize());
            character.Session.SendData(new StringPacketComposer("We will never email you. We use the Message Centre on the website instead.", 378, 38).Serialize());
            character.Session.SendData(new StringPacketComposer("0 days of member credit", 378, 94).Serialize());
            character.Session.SendData(new StringPacketComposer("You have 0 days of member credit remaining. Please click here to extend your credit.", 378, 93).Serialize());
            character.Session.SendData(new StringPacketComposer("Remember to keep your account secure, never give out your password.", 378, 62).Serialize());
            character.Session.SendData(new StringPacketComposer("All your account information is safe with us, don't worry!", 378, 56).Serialize());
            character.Session.SendData(new StringPacketComposer(GameEngine.World.Motw, 17, 3).Serialize());
        }
        #endregion Methods
    }
}
