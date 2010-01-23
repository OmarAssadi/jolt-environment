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

using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Characters;

namespace RuneScape.Content.Interfaces
{
    /// <summary>
    /// Represents the whole side bar.
    /// </summary>
    public class SideBar : IInterfaceHandler
    {
        #region Fields
        /// <summary>
        /// The id of this interface.
        /// </summary>
        public const int InterfaceId = 548;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the sidebar tabs and buttons.
        /// </summary>
        /// <param name="character">The character to handle for.</param>
        /// <param name="packetId">The packet id of the button.</param>
        /// <param name="buttonId">The button to handle.</param>
        /// <param name="buttonId2">The secondary button to handle.</param>
        public void HandleButton(Character character, int packetId, int buttonId, int buttonId2)
        {
            switch (buttonId)
            {
                case 60:
                    {
                        character.Session.SendData(new WindowPanePacketComposer(755).Serialize());
                        break;
                    }
            }
        }
        #endregion Methods
    }
}
