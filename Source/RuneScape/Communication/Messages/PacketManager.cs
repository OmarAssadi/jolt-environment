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
using System.Data;
using System.Linq;
using System.Text;

using JoltEnvironment.Storage;
using RuneScape.Communication.Messages.Incoming;
using RuneScape.Model.Characters;
using RuneScape.Network;

namespace RuneScape.Communication.Messages
{
    /// <summary>
    /// Provides management for message-packet related content.
    /// </summary>
    public class PacketManager
    {
        #region Fields
        /// <summary>
        /// Container holding all loaded packet handlers.
        /// </summary>
        private Dictionary<int, IPacketHandler> packetHandlers = new Dictionary<int, IPacketHandler>();

        /// <summary>
        /// Packet lengths from opcodes sent to server.
        /// </summary>
        private static int[] packetLengths = 
        {                                        		
            //1  2  3   4    5   6   7  8    9   10
		    -3,	-3,	8,	8,	-3,	-3,	-3,	2,	-3,	-3,	// 0
		    -3,	-3,	-3,	10,	-3,	-3,	-3,	-3,	-3,	-3,	// 1
		    -3,	6,	4,	-3,	-3,	-3,	-3,	-3,	-3,	-3,	// 2
		    8,	-3,	-3,	-3,	-3,	-3,	-3,	2,	2,	-3,	// 3
		    16,	-3,	-3,	-3,	-3,	-3,	-3,	0,	-3,	-1,	// 4
		    -3,	-3,	2,	-3,	-3,	-3,	-3,	-3,	-3,	6,	// 5
		    0,	8,	-3,	6,	-3,	-3,	-3,	-3,	-3,	-3,	// 6
		    -3,	-3,	-3,	-3,	-3,	-3,	-3,	-3,	-3,	-3,	// 7
		    -3,	-3,	-3,	-3,	2,	-3,	-3,	-3,	2,	-3,	// 8
		    6,	-3,	-3,	-3,	-3,	-3,	-3,	-3,	-3,	4,	// 9
		    -3,	-3,	-3,	-3,	-3,	-3,	-3,	-1,	0,	-3,	// 10
		    -3,	-3, -3,	4,	-3,	0,	-3,	-1,	-3,	-1,	// 11
		    -3,	-3,	-3,	2,	-3,	-3,	-3,	-3,	-3,	6,	// 12
		    -3,	10,	8,	6,	-3,	-3,	-3,	-3,	-1,	-3,	// 13
		    -3,	-3,	-3,	-3,	-3,	-3,	-3,	-3,	-3,	-3,	// 14
		    -3,	-3,	-3,	-3,	-3,	-3,	-3,	-3,	6,	-3,	// 15
		    2,	-3,	-3,	-3,	-3,	4,	-3,	9,	-3,	6,	// 16
		    -3,	-3,	-3,	6,	-3,	-3,	-3,	-3,	-1,	12,	// 17
		    -3,	-3,	-3,	-3,	-3,	-3,	8,	-3,	-3,	-3,	// 18
		    -3,	-3,	-3,	-3,	-3,	-3,	-3,	-3,	-3,	2,	// 19
		    -3,	6,	-3,	8,	-3,	-3,	-3,	-3,	-3,	-3,	// 20
		    -3,	8,	-3,	-3,	6,	-3,	-3,	-3,	-3,	-3,	// 21
		    8,	-3,	-1,	-3,	14,	-3,	-3,	2,	6,	-3,	// 22
		    -3,	-3,	6,	6,	-3,	-3,	-3,	-3,	-3,	-3,	// 23
		    -3,	-3,	-3,	-3,	-3,	-3,	-3,	4,	1,	-3,	// 24
		    4,	-3,	-3,	2,	-3,		                // 25
        };
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a new packet manager and loads all the packet handlers from the database.
        /// </summary>
        public PacketManager()
        {
            LoadHandlers(); // Load the packet handlers.
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Handles a packet for the given session.
        /// </summary>
        /// <param name="character">The session to handle packet for.</param>
        /// <param name="packet">The packet data.</param>
        public void Handle(Character character, Packet packet)
        {
            if (this.packetHandlers.ContainsKey(packet.Opcode))
            {
                this.packetHandlers[packet.Opcode].Handle(character, packet);
            }
            else if (character.ServerRights >= ServerRights.SystemAdministrator)
            {
                Program.Logger.WriteDebug("Unhandled packet: " + packet.Opcode + ".");
            }
        }

        /// <summary>
        /// Loads the packet handlers.
        /// </summary>
        private void LoadHandlers()
        {
            AddHandler(0, new NullPacketHandler()); // Handles the null packets (usually sent at login).
            AddHandler(3, new EquipItemPacketHandler()); // Handles item equiping.
            AddHandler(21, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(22, new QuietPacketHandler()); // Recieved when update is required.
            AddHandler(49, new WalkingPacketHandler()); // Handles walking.
            AddHandler(59, new QuietPacketHandler()); // Character's mouse click.
            AddHandler(63, new RemoveInterfacePacketHandler()); // Removes a chatbox interface.
            AddHandler(90, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(99, new QuietPacketHandler()); // Unknown.
            AddHandler(107, new CommandPacketHandler()); // Handles commands.
            AddHandler(108, new RemoveInterfacePacketHandler()); // Removes an interface.
            AddHandler(113, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(115, new PingPacketHandler()); // Sent every second.
            AddHandler(117, new QuietPacketHandler()); // Unkwown.
            AddHandler(119, new WalkingPacketHandler()); // Handles walking.
            AddHandler(129, new HdNotificationPacketHandler()); // Handles hd clients.
            AddHandler(133, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(138, new WalkingPacketHandler()); // Handles walking.
            AddHandler(159, new ReportAbusePacketHandler()); // Handles reported abuses.
            AddHandler(167, new SwapItemPacketHandler()); // Handles item movements.
            AddHandler(169, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(173, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(179, new SwapInterfaceItemPacketHandler()); // Handles item movements between interfaces.
            AddHandler(203, new ItemOptionsPacketHandler()); // Handles item options.
            AddHandler(214, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(222, new ChatPacketHandler()); // Handles chat messages.
            AddHandler(232, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(233, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(248, new QuietPacketHandler()); // Unknown.
        }

        /// <summary>
        /// Clears the current packet handlers.
        /// </summary>
        private void ClearHandlers()
        {
            this.packetHandlers.Clear();
        }

        /// <summary>
        /// Adds a handler to the collection of handlers.
        /// </summary>
        /// <param name="opcode">The opcode of the packet.</param>
        /// <param name="handler">The handler for the packet.</param>
        private void AddHandler(int opcode, IPacketHandler handler)
        { // I did this cause i was too lazy to type :P
            this.packetHandlers.Add(opcode, handler);
        }

        /// <summary>
        /// Gets the packet length for the given opcode.
        /// </summary>
        /// <param name="opcode">The opcode.</param>
        /// <returns>Returns an integer with the value of the opcode length.</returns>
        public static int GetPacketLength(int opcode)
        {
            return packetLengths[opcode];
        }
        #endregion Methods
    }
}
