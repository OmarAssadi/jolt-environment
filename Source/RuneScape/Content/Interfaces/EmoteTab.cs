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

using RuneScape.Model;
using RuneScape.Model.Characters;

namespace RuneScape.Content.Interfaces
{
    /// <summary>
    /// Represents the emote tab located on the sidebar.
    /// </summary>
    public class EmoteTab : IInterfaceHandler
    {
        #region Fields
        /// <summary>
        /// The id of this interface.
        /// </summary>
        public const int InterfaceId = 464;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the emote buttons.
        /// </summary>
        /// <param name="character">The character to handle for.</param>
        /// <param name="packetId">The packet id of the button.</param>
        /// <param name="buttonId">The button to handle.</param>
        /// <param name="buttonId2">The secondary button to handle.</param>
        public void HandleButton(Character character, int packetId, int buttonId, int buttonId2)
        {
            switch (buttonId)
            {
                case 2:
                    character.PlayAnimation(Animation.Create(855));
                    break;
                case 3:
                    character.PlayAnimation(Animation.Create(856));
                    break;
                case 4:
                    character.PlayAnimation(Animation.Create(858));
                    break;
                case 5:
                    character.PlayAnimation(Animation.Create(859));
                    break;
                case 6:
                    character.PlayAnimation(Animation.Create(857));
                    break;
                case 7:
                    character.PlayAnimation(Animation.Create(863));
                    break;
                case 8:
                    character.PlayAnimation(Animation.Create(2113));
                    break;
                case 9:
                    character.PlayAnimation(Animation.Create(862));
                    break;
                case 10:
                    character.PlayAnimation(Animation.Create(864));
                    break;
                case 11:
                    character.PlayAnimation(Animation.Create(861));
                    break;
                case 12:
                    character.PlayAnimation(Animation.Create(2109));
                    break;
                case 13:
                    character.PlayAnimation(Animation.Create(2111));
                    break;
                case 14:
                    character.PlayAnimation(Animation.Create(866));
                    break;
                case 15:
                    character.PlayAnimation(Animation.Create(2106));
                    break;
                case 16:
                    character.PlayAnimation(Animation.Create(2107));
                    break;
                case 17:
                    character.PlayAnimation(Animation.Create(2108));
                    break;
                case 18:
                    character.PlayAnimation(Animation.Create(860));
                    break;
                case 19:
                    character.PlayAnimation(Animation.Create(0x558));
                    character.PlayGraphics(Graphic.Create(574));
                    break;
                case 20:
                    character.PlayAnimation(Animation.Create(2105));
                    break;
                case 21:
                    character.PlayAnimation(Animation.Create(2110));
                    break;
                case 22:
                    character.PlayAnimation(Animation.Create(865));
                    break;
                case 23:
                    character.PlayAnimation(Animation.Create(2112));
                    break;
                case 24:
                    character.PlayAnimation(Animation.Create(0x84F));
                    break;
                case 25:
                    character.PlayAnimation(Animation.Create(0x850));
                    break;
                case 26:
                    character.PlayAnimation(Animation.Create(1131));
                    break;
                case 27:
                    character.PlayAnimation(Animation.Create(1130));
                    break;
                case 28:
                    character.PlayAnimation(Animation.Create(1129));
                    break;
                case 29:
                    character.PlayAnimation(Animation.Create(1128));
                    break;
                case 30:
                    character.PlayAnimation(Animation.Create(4275));
                    break;
                case 31:
                    character.PlayAnimation(Animation.Create(1745));
                    break;
                case 32:
                    character.PlayAnimation(Animation.Create(4280));
                    break;
                case 33:
                    character.PlayAnimation(Animation.Create(4276));
                    break;
                case 34:
                    character.PlayAnimation(Animation.Create(3544));
                    break;
                case 35:
                    character.PlayAnimation(Animation.Create(3543));
                    break;
                case 36:
                    character.PlayAnimation(Animation.Create(7272));
                    character.PlayGraphics(Graphic.Create(1244));
                    break;
                case 37:
                    character.PlayAnimation(Animation.Create(2836));
                    break;
                case 38:
                    character.PlayAnimation(Animation.Create(6111));
                    break;
                case 39:
                    //Skill cape animation.
                    break;
                case 40:
                    character.PlayAnimation(Animation.Create(7531));
                    break;
                case 41:
                    character.PlayAnimation(Animation.Create(2414));
                    character.PlayGraphics(Graphic.Create(1537));
                    break;
                case 42:
                    character.PlayAnimation(Animation.Create(8770));
                    character.PlayGraphics(Graphic.Create(1553));
                    break;
                case 43:
                    character.PlayAnimation(Animation.Create(9990));
                    character.PlayGraphics(Graphic.Create(1734));
                    break;
                default:
                    if (character.ServerRights >= ServerRights.SystemAdministrator)
                    {
                        Program.Logger.WriteDebug("Unhandled emote button: " + buttonId);
                    }
                    return;
            }
        }
        #endregion Methods
    }
}
