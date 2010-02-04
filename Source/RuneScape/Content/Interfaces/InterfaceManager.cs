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
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JoltEnvironment.Storage;
using RuneScape.Model.Characters;

namespace RuneScape.Content.Interfaces
{
    /// <summary>
    /// Provides management for interface related content.
    /// </summary>
    public class InterfaceManager
    {
        #region Fields
        /// <summary>
        /// A collection holding runescape interfaces (ingame).
        /// </summary>
        private Dictionary<int, IInterfaceHandler> interfacesHandlers = new Dictionary<int, IInterfaceHandler>();
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a interface manager.
        /// </summary>
        public InterfaceManager()
        {
            LoadHandlers();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Handles a specified interface.
        /// </summary>
        /// <param name="character">The character to handle the interface for.</param>
        /// <param name="packetId">The packet id of the button.</param>
        /// <param name="buttonId">The button to handle.</param>
        /// <param name="buttonId2">The second button to handle.</param>
        /// <param name="interfaceId">The interface id of the button's location.</param>
        public void Handle(Character character, int opcode, int buttonId, int buttonId2, int interfaceId)
        {
            if (this.interfacesHandlers.ContainsKey(interfaceId))
            {
                this.interfacesHandlers[interfaceId].HandleButton(character, opcode, buttonId, buttonId2);
            }
            else if (character.ServerRights >= ServerRights.SystemAdministrator)
            {
                Program.Logger.WriteDebug("Unhandled interface: " + interfaceId + " [" + buttonId + "," + buttonId2 + "]");
            }
        }

        /// <summary>
        /// Loads all interface handlers.
        /// </summary>
        public void LoadHandlers()
        {
            AddHandler(182, new LogoutTab()); // The logout tab located on the side bar.
            AddHandler(261, new OptionsTab()); // The options tab located on the side bar.
            AddHandler(378, new WelcomeScreen()); // The welcome screen displayed on login.
            AddHandler(387, new EquipmentTab()); // The equipment tab lcoated on the side bar.
            AddHandler(464, new EmoteTab()); // The emotes tab located on the side bar.
            AddHandler(548, new SideBar()); // The side bar.
            AddHandler(750, new RuneOrbs()); // The orbs to the right of the mini map.
            AddHandler(751, new ChatOptionsBar()); // The options displayed under the chatbox.
            AddHandler(755, new WorldMap()); // The world map interface.
            AddHandler(771, new CharacterSetup()); // The setup interface (the one shown on first login).
        }

        /// <summary>
        /// Clears the handlers.
        /// </summary>
        public void ClearHandlers()
        {
            this.interfacesHandlers.Clear();
        }

        /// <summary>
        /// Adds a handler to the collection of handlers.
        /// </summary>
        /// <param name="interfaceId">The id of the interface.</param>
        /// <param name="handler">The handler for theinterface.</param>
        public void AddHandler(int interfaceId, IInterfaceHandler handler)
        {
            this.interfacesHandlers.Add(interfaceId, handler);
        }
        #endregion Methods
    }
}
