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
using RuneScape.Content.ClanChat;
using RuneScape.Model.Characters;

namespace RuneScape.Content.Interfaces
{
    /// <summary>
    /// Represents the clan setup interface.
    /// </summary>
    public class ClanSetup : IInterfaceHandler
    {
        #region Fields
        /// <summary>
        /// The id of this interface.
        /// </summary>
        public const int InterfaceId = 590;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the clan setup buttons.
        /// </summary>
        /// <param name="character">The character to handle for.</param>
        /// <param name="packetId">The packet id of the button.</param>
        /// <param name="buttonId">The button to handle.</param>
        /// <param name="buttonId2">The secondary button to handle.</param>
        public void HandleButton(Character character, int packetId, int buttonId, int buttonId2)
        {
            switch (buttonId)
            {
                case 22:
                    if (packetId == 233)
                    {
                        character.Session.SendData(new RunScriptPacketComposer(109, "s", new object[] { 
                                "Enter your clan prefix:" }).Serialize());
                    }
                    else if (packetId == 22)
                    {
                        GameEngine.Content.ClanChat.RemoveRoom(character);
                    }
                    break;
                case 23:
                    Room r = GameEngine.Content.ClanChat.Get(character.LongName);
                    switch (packetId)
                    {
                        case 223:
                            r.JoinReq = Rank.Regular;
                            character.Session.SendData(new StringPacketComposer("Anyone", 590, 23).Serialize());
                            break;
                        case 21:
                            r.JoinReq = Rank.Friend;
                            character.Session.SendData(new StringPacketComposer("Any Friends", 590, 23).Serialize());
                            break;
                        case 169:
                            r.JoinReq = Rank.Recruit;
                            character.Session.SendData(new StringPacketComposer("Recruit+", 590, 23).Serialize());
                            break;
                        case 214:
                            r.JoinReq = Rank.Corporal;
                            character.Session.SendData(new StringPacketComposer("Corporal+", 590, 23).Serialize());
                            break;
                        case 173:
                            r.JoinReq = Rank.Sergeant;
                            character.Session.SendData(new StringPacketComposer("Sergeant+", 590, 23).Serialize());
                            break;
                        case 232:
                            r.JoinReq = Rank.Lieutenant;
                            character.Session.SendData(new StringPacketComposer("Lientenant+", 590, 23).Serialize());
                            break;
                        case 133:
                            r.JoinReq = Rank.Captain;
                            character.Session.SendData(new StringPacketComposer("Captain+", 590, 23).Serialize());
                            break;
                        case 102:
                            r.JoinReq = Rank.General;
                            character.Session.SendData(new StringPacketComposer("General+", 590, 23).Serialize());
                            break;
                        case 226:
                            r.JoinReq = Rank.Owner;
                            character.Session.SendData(new StringPacketComposer("Only me", 590, 23).Serialize());
                            break;
                    }
                    break;
                case 24:
                    Room r2 = GameEngine.Content.ClanChat.Get(character.LongName);
                    switch (packetId)
                    {
                        case 223:
                            r2.TalkReq = Rank.Regular;
                            character.Session.SendData(new StringPacketComposer("Anyone", 590, 24).Serialize());
                            break;
                        case 21:
                            r2.TalkReq = Rank.Friend;
                            character.Session.SendData(new StringPacketComposer("Any Friends", 590, 24).Serialize());
                            break;
                        case 169:
                            r2.TalkReq = Rank.Recruit;
                            character.Session.SendData(new StringPacketComposer("Recruit+", 590, 24).Serialize());
                            break;
                        case 214:
                            r2.TalkReq = Rank.Corporal;
                            character.Session.SendData(new StringPacketComposer("Corporal+", 590, 24).Serialize());
                            break;
                        case 173:
                            r2.TalkReq = Rank.Sergeant;
                            character.Session.SendData(new StringPacketComposer("Sergeant+", 590, 24).Serialize());
                            break;
                        case 232:
                            r2.TalkReq = Rank.Lieutenant;
                            character.Session.SendData(new StringPacketComposer("Lientenant+", 590, 24).Serialize());
                            break;
                        case 133:
                            r2.TalkReq = Rank.Captain;
                            character.Session.SendData(new StringPacketComposer("Captain+", 590, 24).Serialize());
                            break;
                        case 102:
                            r2.TalkReq = Rank.General;
                            character.Session.SendData(new StringPacketComposer("General+", 590, 24).Serialize());
                            break;
                        case 226:
                            r2.TalkReq = Rank.Owner;
                            character.Session.SendData(new StringPacketComposer("Only me", 590, 24).Serialize());
                            break;
                    }
                    break;
                case 25:
                    Room r3 = GameEngine.Content.ClanChat.Get(character.LongName);
                    switch (packetId)
                    {
                        case 223:
                            r3.KickReq = Rank.Regular;
                            character.Session.SendData(new StringPacketComposer("Anyone", 590, 25).Serialize());
                            break;
                        case 21:
                            r3.KickReq = Rank.Friend;
                            character.Session.SendData(new StringPacketComposer("Any Friends", 590, 25).Serialize());
                            break;
                        case 169:
                            r3.KickReq = Rank.Recruit;
                            character.Session.SendData(new StringPacketComposer("Recruit+", 590, 25).Serialize());
                            break;
                        case 214:
                            r3.KickReq = Rank.Corporal;
                            character.Session.SendData(new StringPacketComposer("Corporal+", 590, 25).Serialize());
                            break;
                        case 173:
                            r3.KickReq = Rank.Sergeant;
                            character.Session.SendData(new StringPacketComposer("Sergeant+", 590, 25).Serialize());
                            break;
                        case 232:
                            r3.KickReq = Rank.Lieutenant;
                            character.Session.SendData(new StringPacketComposer("Lientenant+", 590, 25).Serialize());
                            break;
                        case 133:
                            r3.KickReq = Rank.Captain;
                            character.Session.SendData(new StringPacketComposer("Captain+", 590, 25).Serialize());
                            break;
                        case 102:
                            r3.KickReq = Rank.General;
                            character.Session.SendData(new StringPacketComposer("General+", 590, 25).Serialize());
                            break;
                        case 226:
                            r3.KickReq = Rank.Owner;
                            character.Session.SendData(new StringPacketComposer("Only me", 590, 25).Serialize());
                            break;
                    }
                    r3.Refresh();
                    break;
                default:
                    Program.Logger.WriteDebug("Unhandled clansetup button: " + buttonId);
                    break;
            }
        }
        #endregion Methods
    }
}
