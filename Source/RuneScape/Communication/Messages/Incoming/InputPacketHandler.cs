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

namespace RuneScape.Communication.Messages.Incoming
{
    /// <summary>
    /// Handler for user input from interfaces.
    /// </summary>
    public class InputPacketHandler : IPacketHandler
    {
        #region Methods
        /// <summary>
        /// Handles user input from interfaces.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            switch (packet.Opcode)
            {
                case 43:
                    int count = packet.ReadInt();

                    #region Bank/Deposit
                    /**
                     * Checks if the character requested a deposit.
                     */
                    if (character.Preferences["deposit"] != null)
                    {
                        character.Bank.Deposit((byte)character.Preferences["deposit"], count);
                        character.Preferences.Remove("deposit");
                        character.Preferences.BankX = count;
                        character.Session.SendData(new ConfigPacketComposer(1249, count).Serialize());
                    }
                    #endregion Bank/Deposit

                    #region Bank/Withdraw
                    /**
                     * Checks if the character requested a deposit.
                     */
                    if (character.Preferences["withdraw"] != null)
                    {
                        character.Bank.Withdraw((byte)character.Preferences["withdraw"], count);
                        character.Preferences.Remove("withdraw");
                        character.Preferences.BankX = count;
                        character.Session.SendData(new ConfigPacketComposer(1249, count).Serialize());
                    }
                    #endregion Bank/Deposit

                    #region Trade/Offer
                    /*
                     * Checks if the character inputed a trade offer value (Offer X).
                     */
                    if (character.Preferences["trade_offer"] != null)
                    {
                        int slot = (int)character.Preferences["trade_offer"];
                        character.Preferences.Remove("trade_offer");
                        character.Request.Trade.OfferItem(character, slot, count);
                    }
                    #endregion Trade/Offer
                    break;
            }
        }
        #endregion Methods
    }
}