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

using RuneScape.Model.Characters;
using RuneScape.Network;

namespace RuneScape.Communication.Messages.Incoming
{
    /// <summary>
    /// Defines a handlement of runescape packets.
    /// </summary>
    public interface IPacketHandler
    {
        /// <summary>
        /// Handles the given packet.
        /// </summary>
        /// <param name="session">The session to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        void Handle(Character character, Packet packet);
    }
}
