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

namespace RuneScape.Content.Interfaces
{
    /// <summary>
    /// Represents the second trading screen, where 
    /// characters finalize and confirm their tradings.
    /// </summary>
    public class TradeConfirmScreen : IInterfaceHandler
    {
        #region Fields
        /// <summary>
        /// The id of this interface.
        /// </summary>
        public const int InterfaceId = 334;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the character trading confirm screen.
        /// </summary>
        /// <param name="character">The character to handle for.</param>
        /// <param name="packetId">The packet id of the button.</param>
        /// <param name="buttonId">The button to handle.</param>
        /// <param name="buttonId2">The secondary button to handle.</param>
        public void HandleButton(Character character, int packetId, int buttonId, int buttonId2)
        {
            if (!character.Request.Trading)
            {
                return;
            }

            switch (buttonId)
            {
                case 20:
                    character.Request.Trade.Accept(character);
                    break;
                case 8:
                case 21:
                    character.Request.Trade.Close(true);
                    break;
                default:
                    Console.WriteLine("Unhandled: " + buttonId);
                    break;
            }
        }
        #endregion Methods
    }
}
