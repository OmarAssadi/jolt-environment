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
    /// Handles the deposit interface placed over the character's inventory.
    /// </summary>
    public class DepositInterface : IInterfaceHandler
    {
        #region Fields
        /// <summary>
        /// The id of this interface.
        /// </summary>
        public const int InterfaceId = 763;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the banking interface buttons.
        /// </summary>
        /// <param name="character">The character to handle for.</param>
        /// <param name="packetId">The packet id of the button.</param>
        /// <param name="buttonId">The button to handle.</param>
        /// <param name="buttonId2">The secondary button to handle.</param>
        public void HandleButton(Character character, int packetId, int buttonId, int buttonId2)
        {
            switch (buttonId)
            {
                case 0:
                    switch (packetId)
                    {
                        case 233:
                            character.Bank.Deposit((byte)buttonId2, 1);
                            break;
                        case 21:
                            character.Bank.Deposit((byte)buttonId2, 5);
                            break;
                        case 169:
                            character.Bank.Deposit((byte)buttonId2, 10);
                            break;
                        case 173:
                            character.Preferences.Add("deposit", (byte)buttonId2);
                            character.Session.SendData(new RunScriptPacketComposer(108, "s", new object[] { "Please enter the amount to deposit:" }).Serialize());
                            break;
                        case 214:
                            character.Bank.Deposit((byte)buttonId2, character.Preferences.BankX);
                            break;
                        case 232:
                            character.Bank.Deposit((byte)buttonId2,
                                character.Inventory.GetAmount(character.Inventory[buttonId2]));
                            break;
                        case 133:
                            int count = character.Inventory.GetAmount(character.Inventory[buttonId2]);
                            character.Bank.Deposit((byte)buttonId2,
                                count - 1 != 0 ? count - 1 : 1);
                            break;
                        case 90:
                            character.Inventory.Examine((byte)buttonId2);
                            break;
                    }
                    break;
                default:
                    {
                        if (character.ServerRights >= ServerRights.SystemAdministrator)
                        {
                            Program.Logger.WriteDebug("Unhandled inventory button: " + buttonId);
                        }
                        break;
                    }
            }
        }
        #endregion Methods
    }
}
