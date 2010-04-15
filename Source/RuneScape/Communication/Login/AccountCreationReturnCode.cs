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

namespace RuneScape.Communication.Login
{
    /// <summary>
    /// Defines the possible return codes sent during account creation.
    /// </summary>
    public enum AccountCreationReturnCode
    {
        /// <summary>
        /// No return code can describe problem.
        /// </summary>
        None = 0,
        /// <summary>
        /// The input is valid.
        /// </summary>
        Good = 2,
        /// <summary>
        /// Server is too busy to repsond.
        /// </summary>
        ServerBusy = 7,
        /// <summary>
        /// Account creation is disabled for this world.
        /// </summary>
        Disabled = 9,
        /// <summary>
        /// The name is already taken.
        /// </summary>
        AlreadyTaken = 20,
        /// <summary>
        /// Invaid username.
        /// </summary>
        InvalidUsername = 22,
        /// <summary>
        /// Invalid username/password length.
        /// </summary>
        InvalidLength = 30,
        /// <summary>
        /// The password is too similar to the username.
        /// </summary>
        TooSimilar = 34
    }
}
