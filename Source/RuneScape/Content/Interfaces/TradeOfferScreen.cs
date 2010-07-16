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
    /// Represents the first trading screen, where offering of items is done.
    /// </summary>
    public class TradeOfferScreen : IInterfaceHandler
    {
        #region Fields
        /// <summary>
        /// The id of this interface.
        /// </summary>
        public const int InterfaceId = 335;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the character trading offer screen.
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
                case 12:
                case 18:
                    character.Request.Trade.Close(true);
                    break;
                case 16:
                    character.Request.Trade.Accept(character);
                    break;
                case 30:
                    if (packetId == 233)
                    {
                        character.Request.Trade.RemoveItem(character, buttonId2, 1);
                        character.Request.Trade.FlashIcon(character, buttonId2);
                        character.Request.Trade.TradeModified = true;
                    }
                    else if (packetId == 90)
                    {
                        character.Request.Trade.ExamineMy(character, buttonId2);
                    }
                    break;
                case 32:
                    character.Request.Trade.ExamineOther(character, buttonId2);
                    break;
                default:
                    Console.WriteLine("Unhandled: " + buttonId);
                    break;
            }
        }
        #endregion Methods
    }
}
