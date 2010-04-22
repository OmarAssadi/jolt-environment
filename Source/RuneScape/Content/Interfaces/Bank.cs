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
using RuneScape.Communication.Messages;

namespace RuneScape.Content.Interfaces
{
    /// <summary>
    /// Represents the game bank.
    /// </summary>
    public class Bank : IInterfaceHandler
    {
        #region Fields
        /// <summary>
        /// The id of this interface.
        /// </summary>
        public const int InterfaceId = 762;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the character setup options.
        /// </summary>
        /// <param name="character">The character to handle for.</param>
        /// <param name="packetId">The packet id of the button.</param>
        /// <param name="buttonId">The button to handle.</param>
        /// <param name="buttonId2">The secondary button to handle.</param>
        public void HandleButton(Character character, int packetId, int buttonId, int buttonId2)
        {
            switch (buttonId)
            {
                case 73:
                    switch (packetId)
                    {
                        case 233:
                            character.Bank.Withdraw((byte)buttonId2, 1);
                            break;
                        case 21:
                            character.Bank.Withdraw((byte)buttonId2, 5);
                            break;
                        case 169:
                            character.Bank.Withdraw((byte)buttonId2, 10);
                            break;
                        case 173:
                            character.Preferences.Add("withdraw", (byte)buttonId2);
                            character.Session.SendData(new RunScriptPacketComposer(108, "s", new object[] { "Please enter the amount to withdraw:" }).Serialize());
                            break;
                        case 232:
                            character.Bank.Withdraw((byte)buttonId2,
                                character.Bank.GetAmount(character.Bank[buttonId2]));
                            break;
                        case 133:
                            int count = character.Bank.GetAmount(character.Bank[buttonId2]);
                            character.Bank.Withdraw((byte)buttonId2,
                                count - 1 != 0 ? count - 1 : 1);
                            break;
                        case 90:
                            character.Bank.Examine((byte)buttonId2);
                            break;
                    }
                    break;
                case 14:
                    character.Bank.Inserting = !character.Bank.Inserting;
                    break;
                case 16:
                    character.Bank.Noting = !character.Bank.Noting;
                    break;
                case 22:
                    break;
                default:
                    {
                        if (character.ServerRights >= ServerRights.SystemAdministrator)
                        {
                            Program.Logger.WriteDebug("Unhandled banking button: " + buttonId);
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Shows the bank interface.
        /// </summary>
        /// <param name="character">The character to show interface for.</param>
        public static void Show(Character character)
        {
            character.Session.SendData(new StringPacketComposer("Bank of " + GameEngine.World.Name, 762, 24).Serialize());
            character.Session.SendData(new ConfigPacketComposer(563, 4194304).Serialize());
            character.Session.SendData(new ConfigPacketComposer(1248, -2013265920).Serialize());
            character.Session.SendData(new ConfigPacketComposer(115, character.Bank.Noting ? 1 : 0).Serialize());
            character.Session.SendData(new ConfigPacketComposer(305, character.Bank.Inserting ? 1 : 0).Serialize());
            GenericPacketComposer gpc = new GenericPacketComposer();
            gpc.SetOpcode(223);
            gpc.AppendShort(496);
            gpc.AppendLEShortA(0);
            gpc.AppendLEShort(73);
            gpc.AppendLEShort(762);
            gpc.AppendLEShort(1278);
            gpc.AppendLEShort(20);
            character.Session.SendData(gpc.Serialize());
            gpc = new GenericPacketComposer();
            gpc.SetOpcode(223);
            gpc.AppendShort(27);
            gpc.AppendLEShortA(0);
            gpc.AppendLEShort(0);
            gpc.AppendLEShort(763);
            gpc.AppendLEShort(1150);
            gpc.AppendLEShort(18);
            character.Session.SendData(gpc.Serialize());
            character.Bank.Refresh();
            Frames.SendInterface(character, 762, true);
            Frames.SendInventoryInterface(character, 763);
        }
        #endregion Methods
    }
}
