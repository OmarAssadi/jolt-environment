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
using RuneScape.Communication.Messages;
using RuneScape.Communication.Messages.Outgoing;

namespace RuneScape.Content.Interfaces
{
    /// <summary>
    /// Represents the options tab on the sidebar.
    /// </summary>
    public class OptionsTab : IInterfaceHandler
    {
        #region Fields
        /// <summary>
        /// The id of this interface.
        /// </summary>
        public const int InterfaceId = 261;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the option tab and buttons.
        /// </summary>
        /// <param name="character">The character to handle for.</param>
        /// <param name="packetId">The packet id of the button.</param>
        /// <param name="buttonId">The button to handle.</param>
        /// <param name="buttonId2">The secondary button to handle.</param>
        public void HandleButton(Character character, int packetId, int buttonId, int buttonId2)
        {
            switch (buttonId)
            {
                case 1:
                    {
                        if (!character.WalkingQueue.RunToggled)
                        {
                            character.WalkingQueue.RunToggled = true;
                            character.Session.SendData(new ConfigPacketComposer(173, 1).Serialize());
                        }
                        else
                        {
                            character.WalkingQueue.RunToggled = false;
                            character.Session.SendData(new ConfigPacketComposer(173, 0).Serialize());
                        }
                        break;
                    }
                case 2:
                    {
                        if (character.Preferences.DisableChatEffects)
                        {
                            character.Preferences.DisableChatEffects = true;
                            character.Session.SendData(new ConfigPacketComposer(171, 0).Serialize());
                        }
                        else
                        {
                            character.Preferences.DisableChatEffects = false;
                            character.Session.SendData(new ConfigPacketComposer(171, 1).Serialize());
                        }
                        break;
                    }
                case 3:
                    {
                        if (!character.Preferences.SplitChat)
                        {
                            character.Preferences.SplitChat = true;
                            character.Session.SendData(new ConfigPacketComposer(287, 1).Serialize());
                        }
                        else
                        {
                            character.Preferences.SplitChat = false;
                            character.Session.SendData(new ConfigPacketComposer(287, 0).Serialize());
                        }
                        break;
                    }
                case 4:
                    {
                        if (!character.Preferences.SingleMouse)
                        {
                            character.Preferences.SingleMouse = true;
                            character.Session.SendData(new ConfigPacketComposer(170, 1).Serialize());
                        }
                        else
                        {
                            character.Preferences.SingleMouse = false;
                            character.Session.SendData(new ConfigPacketComposer(170, 0).Serialize());
                        }
                        break;
                    }
                case 5:
                    {
                        if (!character.Preferences.AcceptAid)
                        {
                            character.Preferences.SingleMouse = true;
                            character.Session.SendData(new ConfigPacketComposer(170, 1).Serialize());
                        }
                        else
                        {
                            character.Preferences.SingleMouse = false;
                            character.Session.SendData(new ConfigPacketComposer(170, 0).Serialize());
                        }
                        break;
                    }
                case 14:
                    {
                        Frames.SendInterface(character, 742, false);
                        break;
                    }
                case 16:
                    {
                        Frames.SendInterface(character, 743, false);
                        break;
                    }
                default:
                    {
                        Console.WriteLine(buttonId);
                        break;
                    }
            }
        }
        #endregion Methods
    }
}
