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
using RuneScape.Model.Items;
using RuneScape.Utilities;

namespace RuneScape.Communication.Messages.Incoming
{
    /// <summary>
    /// Handler for the take item packet.
    /// </summary>
    public class TakeItemPacketHandler : IPacketHandler
    {        
        #region Methods
        /// <summary>
        /// Handles the items dropped by the character.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            short y = packet.ReadShortA();
            short x = packet.ReadShort();
            short id = packet.ReadLEShortA();
            Location location = Location.Create(x, y, character.Location.Z);

            CoordinateEvent ce = new CoordinateEvent(character, location)
            {
                Action = new Action(() =>
                {
                    if (character.Location.WithinInteractionDistance(location))
                    {
                        if (GameEngine.World.ItemManager.GroundItems.Exists(location, id))
                        {
                            int amount = GameEngine.World.ItemManager.GroundItems.GetAmount(location, id);
                            if (character.Inventory.HasSpace(new Item(id, amount)))
                            {
                                GameEngine.World.ItemManager.GroundItems.Destroy(location, id);
                                character.Inventory.AddItem(new Item(id, amount));
                            }
                        }
                    }
                })
            };
            GameEngine.Content.RegisterCE(ce);
        }
        #endregion Methods
    }
}
