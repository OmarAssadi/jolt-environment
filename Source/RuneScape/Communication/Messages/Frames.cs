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
using RuneScape.Content.Interfaces;
using RuneScape.Model;
using RuneScape.Model.Characters;

namespace RuneScape.Communication.Messages
{
    /// <summary>
    /// Defines a set of packets (composed) to create commonly used frames.
    /// </summary>
    public static class Frames
    {
        #region Methods
        /// <summary>
        /// Sends a few packets at login to prepare the character for gameplay.
        /// </summary>
        /// <param name="character">The character to produce frame for.</param>
        public static void SendLoginWelcome(Character character)
        {
            SendMapRegion(character);
            WelcomeScreen.Show(character);
            SendCloseInventoryInterface(character);
            SendTab(character, 6, 745);
            SendTab(character, 7, 754);
            SendTab(character, 11, 751); // Chat options
            SendTab(character, 68, 752); // Chatbox
            SendTab(character, 67, 747);
            character.Session.SendData(new ConfigPacketComposer(1160, -1).Serialize());
            SendTab(character, 8, 137); // Playername on chat
            SendTab(character, 73, 92); // Attack tab
            SendTab(character, 74, 320); // Skill tab
            SendTab(character, 75, 274); //  Quest tab
            SendTab(character, 76, 149); // Inventory tab
            SendTab(character, 77, 387); // Equipment tab
            SendTab(character, 78, 271); // Prayer tab
            SendTab(character, 79, 192); // Magic tab
            SendTab(character, 81, 550); // Friend tab
            SendTab(character, 82, 551); // Ignore tab
            SendTab(character, 83, 589); // Clan tab
            SendTab(character, 84, 261); // Setting tab
            SendTab(character, 85, 464); // Emote tab
            SendTab(character, 86, 187); // Music tab
            SendTab(character, 87, 182); // Logout tab

            SendTab(character, 64, 748); // HP orb
            SendTab(character, 65, 749); // Prayer orb
            SendTab(character, 66, 750); // Energy orb

            // Summoning orb
            for (byte i = 0; i < 6; i++)
            {
                character.Session.SendData(new InterfaceConfigPacketComposer(747, i, false).Serialize());
            }

            /* 
             * Default configs must be sent, otherwise if the character has logged off, 
             * but hasn't closed client and tries logging in again, he/she will see the 
             * same configs from last session.
             */
            character.Session.SendData(new ConfigPacketComposer(173, 0).Serialize()); // Running config.
            character.Session.SendData(new ConfigPacketComposer(313, -1).Serialize());
            character.Session.SendData(new ConfigPacketComposer(465, -1).Serialize());
            character.Session.SendData(new ConfigPacketComposer(802, -1).Serialize());
            character.Session.SendData(new ConfigPacketComposer(1085, 249852).Serialize());

            // Send settings.
            SendSettingsConfig(character);

            //Refresh objects.
            character.Inventory.Refresh();

            // The character's skill data must be sent.
            SendSkills(character);
            // The character's run energy must be sent.
            character.Session.SendData(new RunEnergyPacketComposer(character.WalkingQueue.RunEnergy).Serialize());
            // Send warm welcome!
            character.Session.SendData(new MessagePacketComposer(GameEngine.World.WelcomeMessage).Serialize());
        }

        /// <summary>
        /// Sends the required hd interfaces.
        /// </summary>
        /// <param name="character">The character to produce frame for.</param>
        public static void SendHdInterface(Character character)
        {
            character.Session.SendData(new InterfacePacketComposer(1, 549, 0, 746).Serialize());
            character.Session.SendData(new InterfacePacketComposer(1, 746, 13, 748).Serialize()); //energy orb
            character.Session.SendData(new InterfacePacketComposer(1, 746, 14, 749).Serialize()); //energy orb
            character.Session.SendData(new InterfacePacketComposer(1, 746, 15, 750).Serialize()); //energy orb
            //character.Session.SendData(new InterfacePacketComposer(1, 746, 16, 747); //summing orb
            character.Session.SendData(new InterfacePacketComposer(1, 746, 18, 751).Serialize()); //things below chatbox 
            character.Session.SendData(new InterfacePacketComposer(1, 752, 8, 137).Serialize()); //chatbox
            character.Session.SendData(new InterfacePacketComposer(1, 746, 65, 752).Serialize()); //chatbox 752
            character.Session.SendData(new InterfacePacketComposer(1, 549, 0, 746).Serialize()); // Main interface
            character.Session.SendData(new InterfacePacketComposer(1, 746, 87, 92).Serialize()); // Attack tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 88, 320).Serialize()); // Skill tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 89, 274).Serialize()); // Quest tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 90, 149).Serialize()); // Inventory tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 91, 387).Serialize()); // Equipment tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 92, 271).Serialize()); // Prayer tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 93, 193).Serialize()); // Magic tab
            //character.Session.SendData(new InterfacePacketComposer(1, 746, 94, 662).Serialize()); // Summoning tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 95, 550).Serialize()); // Friend tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 96, 551).Serialize()); // Ignore tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 97, 589).Serialize()); // Clan tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 98, 261).Serialize()); // Setting tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 99, 464).Serialize()); // Emote tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 100, 187).Serialize()); // Music tab
            character.Session.SendData(new InterfacePacketComposer(1, 746, 101, 182).Serialize()); // Logout tab
            character.Session.SendData(new InterfacePacketComposer(1, 752, 8, 137).Serialize()); // Chatbox 
            character.Session.SendData(new InterfacePacketComposer(1, 746, 65, 752).Serialize()); // Chatbox 752
            character.Session.SendData(new InterfacePacketComposer(1, 746, 18, 751).Serialize()); // Settings below chatbox
            character.Session.SendData(new InterfacePacketComposer(1, 746, 13, 748).Serialize()); // HP orb
            character.Session.SendData(new InterfacePacketComposer(1, 746, 14, 749).Serialize()); // Prayer orb
            character.Session.SendData(new InterfacePacketComposer(1, 746, 15, 750).Serialize()); // Energy orb
            character.Session.SendData(new InterfacePacketComposer(1, 746, 12, 747).Serialize()); // Summoning orb
            //player.getEquipment().refresh();
            SendLoginWelcome(character);
        }

        /// <summary>
        /// Sends a set of setting configs.
        /// </summary>
        /// <param name="character"></param>
        public static void SendSettingsConfig(Character character)
        {
            character.Session.SendData(new ConfigPacketComposer(170, character.Preferences.SingleMouse.GetHashCode()).Serialize());
            character.Session.SendData(new ConfigPacketComposer(171, character.Preferences.DisableChatEffects.GetHashCode()).Serialize());
            // TODO: send auto retaliate.
            character.Session.SendData(new ConfigPacketComposer(427, character.Preferences.AcceptAid.GetHashCode()).Serialize());
            /*if (!character.Preferences.SplitChat)
            {
                GenericPacketComposer gpc = new GenericPacketComposer(152);
                gpc.SetType(PacketType.Short);
                gpc.AppendString("s");
                gpc.AppendInt(83);
                character.Session.SendData(gpc.Serialize());
                character.Preferences.SplitChat = true;
            }
            else
            {
                character.Preferences.SplitChat = false;
            }*/
            character.Session.SendData(new ConfigPacketComposer(287, character.Preferences.SplitChat.GetHashCode()).Serialize());
        }

        /// <summary>
        /// Closes the inventory interface.
        /// </summary>
        /// <param name="character">The character to produce frame for.</param>
        public static void SendCloseInventoryInterface(Character character)
        {
            character.Preferences.Remove("interface");
            character.Session.SendData(new InterfaceConfigPacketComposer(
                (short)(character.Preferences.Hd ? 746 : 548), 71, true).Serialize());
        }

        /// <summary>
        /// Sends a tab interface.
        /// </summary>
        /// <param name="character">The character to produce frame for.</param>
        /// <param name="tabId">The id of the tab.</param>
        /// <param name="childId">The child if of the tab.</param>
        public static void SendTab(Character character, short tabId, short childId)
        {
            character.Session.SendData(new InterfacePacketComposer(1, 
                (short)(childId == 137 ? 752 : (character.Preferences.Hd ? 746 : 548)), tabId, childId).Serialize());
        }

        /// <summary>
        /// Opens an interface.
        /// </summary>
        /// <param name="character">The character to produce frame for.</param>
        /// <param name="id">The id of the interface to open.</param>
        /// <param name="inventoryInterface">Whether or not the interface is an intventory interface.</param>
        public static void SendInterface(Character character, short id, bool inventoryInterface)
        {
            SendCloseInterface(character);
            character.Preferences.Add("interface", id);
            if (character.Preferences.Hd)
            {
                character.Session.SendData(new InterfacePacketComposer(0, 746, (short)(inventoryInterface ? 4 : 6), id).Serialize());
                character.Session.SendData(new InterfacePacketComposer(0, 746, 8, id).Serialize());
            }
            else
            {
                character.Session.SendData(new InterfacePacketComposer(0, 548, 8, id).Serialize());
            }
        }

        /// <summary>
        /// Closes an open interface.
        /// </summary>
        /// <param name="character">The character to produce frame for.</param>
        public static void SendCloseInterface(Character character)
        {
            character.Preferences.Remove("interface");
            if (character.Preferences.Hd)
            {
                character.Session.SendData(new CloseInterfacePacketComposer(746, 3).Serialize());
                character.Session.SendData(new CloseInterfacePacketComposer(746, 4).Serialize());
                character.Session.SendData(new CloseInterfacePacketComposer(746, 6).Serialize());
                character.Session.SendData(new CloseInterfacePacketComposer(746, 8).Serialize());
            }
            else
            {
                character.Session.SendData(new CloseInterfacePacketComposer(548, 8).Serialize());
            }
        }

        /// <summary>
        /// Opens an interface on the chatbox.
        /// </summary>
        /// <param name="character">The character to produce frame for.</param>
        /// <param name="childId">The child id of the interface.</param>
        public static void SendChatboxInterface(Character character, short childId)
        {
            character.Session.SendData(new InterfacePacketComposer(1, 752, 8, childId).Serialize());
        }

        /// <summary>
        /// Closes any open interfaces on the chatbox.
        /// </summary>
        /// <param name="character">The character to produce the frame for.</param>
        public static void SendCloseChatboxInterface(Character character)
        {
            character.Session.SendData(new InterfacePacketComposer(1, 752, 8, 137).Serialize());
        }

        /// <summary>
        /// Sends new map regions needed to the specified character, aswell 
        /// as updating the items and objects around the character.
        /// </summary>
        /// <param name="character">The character to produce frame for.</param>
        public static void SendMapRegion(Character character)
        {
            character.Session.SendData(new NewMapRegionPacketComposer(character).Serialize());
            character.UpdateFlags.LastRegion = character.Location;
            // TODO: Item update.
            // TODO: Object update.
        }

        /// <summary>
        /// Updates all the character's skills.
        /// </summary>
        /// <param name="character">The character to update skills for.</param>
        public static void SendSkills(Character character)
        {
            for (byte i = 0; i < Skills.Count; i++)
            {
                byte level = character.Skills[i].Level;
                double experience = character.Skills[i].Experience;

                character.Session.SendData(new StatsPacketComposer(i, level, experience).Serialize());
            }
        }
        #endregion Methods
    }
}
