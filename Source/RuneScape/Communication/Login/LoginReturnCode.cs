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

namespace RuneScape.Communication.Login
{
    /// <summary>
    /// Defines the types of return codes send to the runescape client at login.
    /// </summary>
    public enum LoginReturnCode
    {
        /// <summary>
        /// The login was successful.
        /// </summary>
        Successful = 2,
        /// <summary>
        /// The password was invalid / incorrect.
        /// </summary>
        WrongPassword = 3,
        /// <summary>
        /// The character was disabled.
        /// </summary>
        AccountDisabled = 4,
        /// <summary>
        /// The character is already online.
        /// </summary>
        AlreadyOnline = 5,
        /// <summary>
        /// The world is full.
        /// </summary>
        WorldFull = 7,
        /// <summary>
        /// The connection limit has exeeded for this ip.
        /// </summary>
        LimitExeeded = 9,
        /// <summary>
        /// An error happend during loading of account.
        /// </summary>
        BadSession = 10,
        /// <summary>
        /// The session was rejected.
        /// </summary>
        Rejected = 11
    }
}
