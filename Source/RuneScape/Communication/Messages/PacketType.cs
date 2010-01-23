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

namespace RuneScape.Communication.Messages
{
    /// <summary>
    /// Defines packet types that are noted by the client.
    /// </summary>
    public enum PacketType
    {
        /// <summary>
        /// A fixed size packet where the size never changes.
        /// </summary>
        Fixed,
        /// <summary>
        /// A variable packet where the size is described by a byte.
        /// </summary>
        Byte,
        /// <summary>
        /// A variable packet where the size is described by a word.
        /// </summary>
        Short
    }
}
