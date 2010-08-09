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
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the packet lenghts.
        /// </summary>
        public static PacketLengths Lengths { get; set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new packet manager and loads all the packet handlers from the database.
        /// </summary>
        public PacketManager()
        {
            Lengths = new PacketLengths();
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
            try
            {
                if (this.packetHandlers.ContainsKey(packet.Opcode))
                {
                    this.packetHandlers[packet.Opcode].Handle(character, packet);
                }
                else if (character.ServerRights >= ServerRights.SystemAdministrator)
                {
                    Program.Logger.WriteDebug("Unhandled packet " + packet.ToString() + " by " + character.Name + ".");
                }
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
        }

        /// <summary>
        /// Loads the packet handlers.
        /// </summary>
        private void LoadHandlers()
        {
            AddHandler(0, new NullPacketHandler()); // Handles the null packets (Ssent at login).
            AddHandler(2, new RemoveIgnorePacketHandler()); // Handles removed ignores.
            AddHandler(3, new EquipItemPacketHandler()); // Handles item equiping.
            AddHandler(21, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(22, new QuietPacketHandler()); // Recieved when update is required.
            AddHandler(30, new AddFriendPacketHandler()); // Handles added friends.
            AddHandler(37, new CharacterOptionsPacketHandler()); // Handles character options.
            AddHandler(38, new ItemExaminePacketHandler()); // Handles item examine option.
            AddHandler(42, new JoinClanChatPacketHandler()); // Handles clan chat joining.
            AddHandler(43, new InputPacketHandler()); // Handles input from interfaces.
            AddHandler(47, new IdlePacketHandler()); // Handle client idling.
            AddHandler(49, new WalkingPacketHandler()); // Handles walking.
            AddHandler(59, new QuietPacketHandler()); // Character's mouse click.
            AddHandler(60, new NewMapRegionPacketHandler()); // Handles region changing.
            AddHandler(61, new AddIgnorePacketHandler()); // Handles added ignores.
            AddHandler(63, new RemoveInterfacePacketHandler()); // Removes a chatbox interface.
            AddHandler(88, new NpcExaminePacketHandler()); // Handles npc examine option.
            AddHandler(90, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(99, new QuietPacketHandler()); // Client rotation position.
            AddHandler(102, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(107, new CommandPacketHandler()); // Handles commands.
            AddHandler(108, new RemoveInterfacePacketHandler()); // Removes an interface.
            AddHandler(111, new ChangeRankPacketHandler()); // Handles changing of clan member ranks.
            AddHandler(113, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(115, new PingPacketHandler()); // Sent every second.
            AddHandler(117, new QuietPacketHandler()); // Unkwown.
            AddHandler(119, new WalkingPacketHandler()); // Handles walking.
            AddHandler(129, new HdNotificationPacketHandler()); // Handles hd clients.
            AddHandler(132, new RemoveFriendPacketHandler()); // Handles removed friends.
            AddHandler(133, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(138, new WalkingPacketHandler()); // Handles walking.
            AddHandler(158, new ObjectOptionsPacketHandler()); // Handles object options.
            AddHandler(159, new ReportAbusePacketHandler()); // Handles reported abuses.
            AddHandler(167, new SwapItemPacketHandler()); // Handles item movements.
            AddHandler(169, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(173, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(178, new PrivateMessagePacketHandler()); // Handles private messaging.
            AddHandler(179, new SwapInterfaceItemPacketHandler()); // Handles item movements between interfaces.
            AddHandler(189, new CreateClanPacketHandler()); // Handles clan creation.
            AddHandler(200, new KickUserPacketHandler()); // Handles kicking of clan members.
            AddHandler(201, new TakeItemPacketHandler()); // Handles taking items.
            AddHandler(203, new ItemOptionsPacketHandler()); // Handles item options.
            AddHandler(211, new DropItemPacketHandler()); // Handles dropped items.
            AddHandler(214, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(222, new ChatPacketHandler()); // Handles chat messages.
            AddHandler(226, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(232, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(233, new ButtonPacketHandler()); // Handles buttons.
            AddHandler(248, new ActiveWindowPacketHandler()); // Handles client window activity.
            AddHandler(253, new AcceptTradePacketHandler()); // Handles accepting of trade requests.
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
        {
            this.packetHandlers.Add(opcode, handler);
        }
        #endregion Methods
    }
}
