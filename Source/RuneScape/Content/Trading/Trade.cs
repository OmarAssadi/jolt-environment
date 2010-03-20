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

using RuneScape.Communication.Messages;
using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Characters;
using RuneScape.Model.Items.Containers;
using RuneScape.Model.Items;

namespace RuneScape.Content.Trading
{
    /// <summary>
    /// Represents a single trade between 2 characters.
    /// </summary>
    public class Trade
    {
        #region Properties
        /// <summary>
        /// Gets the first character.
        /// </summary>
        public Character Character1 { get; private set; }
        /// <summary>
        /// Gets the second character.
        /// </summary>
        public Character Character2 { get; private set; }
        /// <summary>
        /// Gets the first character's offer.
        /// </summary>
        public Container Container1 { get; private set; }
        /// <summary>
        /// Gets the second character's offer.
        /// </summary>
        public Container Container2 { get; private set; }

        /// <summary>
        /// Gets the current trade state.
        /// </summary>
        public TradeState State { get; private set; }

        /// <summary>
        /// Gets whether the trade has been exchanged.
        /// </summary>
        public bool Exchanged { get; private set; }

        /// <summary>
        /// Gets whether the first character has accepted the trade.
        /// </summary>
        public bool Accept1 { get; private set; }
        /// <summary>
        /// Gets whether the second cahracter has accepted the trade.
        /// </summary>
        public bool Accept2 { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new trade.
        /// </summary>
        /// <param name="character1">The first character trading.</param>
        /// <param name="character2">The second character trading.</param>
        public Trade(Character character1, Character character2)
        {
            this.Character1 = character1;
            this.Character2 = character2;

            OpenFirstInterface(this.Character1);
            OpenFirstInterface(this.Character2);
            RefreshInventories();

            this.State = TradeState.Trade;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Opens the first trading interface.
        /// </summary>
        /// <param name="character">The character to open interface for.</param>
        private void OpenFirstInterface(Character character)
        {
            Frames.SendTradeOptions(character);
            Frames.SendInterface(character, 335, true);
            Frames.SendInventoryInterface(character, 336);
            character.Session.SendData(new StringPacketComposer("Trading with " 
                + GetOther(character).PrettyName, 335, 15).Serialize());
            character.Session.SendData(new StringPacketComposer("", 335, 36).Serialize());
        }

        /// <summary>
        /// Opens the second trading interface.
        /// </summary>
        /// <param name="character">The character to open interface for.</param>
        private void OpenSecondInterface(Character character)
        {
            Frames.SendInterface(character, 334, true);
            character.Session.SendData(new StringPacketComposer(BuildString(character == this.Character1 
                ? this.Container1 : this.Container2), 334, 37).Serialize());
            character.Session.SendData(new StringPacketComposer(BuildString(character == this.Character1 
                ? this.Container2 : this.Container1), 334, 41).Serialize());
            character.Session.SendData(new StringPacketComposer("<col=00FFFF>Trading with " 
                + GetOther(character).PrettyName, 334, 46).Serialize());
            character.Session.SendData(new InterfaceConfigPacketComposer(334, 37, false).Serialize());
            character.Session.SendData(new InterfaceConfigPacketComposer(334, 41, false).Serialize());
            character.Session.SendData(new InterfaceConfigPacketComposer(334, 45, false).Serialize());
            character.Session.SendData(new InterfaceConfigPacketComposer(334, 46, false).Serialize());
        }

        /// <summary>
        /// Builds a string for the second interface, showing all items within the given container.
        /// </summary>
        /// <param name="container">The container to build string from.</param>
        /// <returns>Returns a string containing information from the container.</returns>
        private string BuildString(Container container)
        {
            if (container.TakenSlots == 0)
            {
                return "<col=FFFFFF>Absolutely nothing!</col>";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < container.Capacity)
                {
                    Item item = container[i];
                    if (item != null)
                    {
                        sb.Append("<col=FF9040>").Append(item.Name).Append("</col>");
                        if (item.Count > 1)
                        {
                            sb.Append("<col=FFFFFF> x").Append(item.Count).Append("</col>");
                        }
                        sb.Append("<br>");
                    }
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the other character dependant on the specified character.
        /// </summary>
        /// <param name="character">The character to compare.</param>
        /// <returns>Returns the character opposite to the one specified.</returns>
        public Character GetOther(Character character)
        {
            return character == this.Character1 ? this.Character2 : this.Character1;
        }

        /// <summary>
        /// Refreshes both character's inventories.
        /// </summary>
        public void RefreshInventories()
        {
            this.Character1.Session.SendData(new InterfaceItemsPacketComposer(-1, 2, 90, this.Container1).Serialize());
            this.Character1.Session.SendData(new InterfaceItemsPacketComposer(-2, unchecked((short)60981), 90, this.Container2).Serialize());
            this.Character2.Session.SendData(new InterfaceItemsPacketComposer(-1, 2, 90, this.Container2).Serialize());
            this.Character2.Session.SendData(new InterfaceItemsPacketComposer(-2, unchecked((short)60981), 90, this.Container1).Serialize());
            this.Character1.Inventory.Refresh();
            this.Character2.Inventory.Refresh();
            this.Character1.Session.SendData(new InterfaceItemsPacketComposer(-1, 1, 93, this.Character1.Inventory).Serialize());
            this.Character2.Session.SendData(new InterfaceItemsPacketComposer(-1, 1, 93, this.Character2.Inventory).Serialize());
        }
        #endregion Methods
    }
}
