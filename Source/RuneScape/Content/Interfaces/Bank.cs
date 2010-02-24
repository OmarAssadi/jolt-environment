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
        public void HandleButton(Model.Characters.Character character, int packetId, int buttonId, int buttonId2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shows the bank interface.
        /// </summary>
        /// <param name="character">The character to show interface for.</param>
        public static void Show(Character character)
        {
            character.Session.SendData(new StringPacketComposer("Bank of " + GameEngine.World.Name, 762, 24).Serialize());
            character.Session.SendData(new ConfigPacketComposer(563, 4194304).Serialize());
            character.Bank.ConfigureTabs();
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
            character.Bank.ConfigureTabs();
            character.Bank.Refresh();
            Frames.SendInterface(character, 762, true);
            Frames.SendInventoryInterface(character, 763);
            character.Bank.CurrentTab = 10;
        }
        #endregion Methods
    }
}
