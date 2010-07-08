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
using RuneScape.Model.Items;
using RuneScape.Model.Items.Containers;

namespace RuneScape.Content.Interfaces
{
    /// <summary>
    /// The inventory shown with the first trading screen.
    /// </summary>
    public class TradeOfferInventory : IInterfaceHandler
    {
        #region Fields
        /// <summary>
        /// The id of this interface.
        /// </summary>
        public const int InterfaceId = 336;
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
            if (!character.Request.Trading || buttonId2 < 0 
                || buttonId2 >= InventoryContainer.Size)
            {
                return;
            }

            switch (packetId)
            {
                // Offer 1
                case 233:
                    character.Request.Trade.OfferItem(character, buttonId2, 1);
                    break;
                // Offer 5
                case 21:
                    character.Request.Trade.OfferItem(character, buttonId2, 5);
                    break;
                // Offer 10
                case 169:
                    character.Request.Trade.OfferItem(character, buttonId2, 10);
                    break;
                // Offer all
                case 214:
                    Item item = character.Inventory[buttonId2];
                    if (item != null)
                    {
                        int amt = character.Inventory.GetAmount(item);
                        character.Request.Trade.OfferItem(character, buttonId2, amt);
                    }
                    break;
                // Offer X
                case 173:
                    character.Preferences.Add("trade_offer", buttonId2);
                    character.Session.SendData(new RunScriptPacketComposer(108, "s", new object[] { "Please enter the amount to offer:" }).Serialize());
                    break;
                default:
                    Program.Logger.WriteDebug("Unhandled button. [buttonId=" + buttonId + ",buttonId2=" + buttonId2 + "]");
                    break;
            }
        }
        #endregion Methods
    }
}
