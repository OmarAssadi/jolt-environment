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

using RuneScape.Communication;
using RuneScape.Network;

namespace RuneScape.Model.Characters
{
    /// <summary>
    /// Defines a character's input details.
    /// 
    ///     <para>When a session requests a login, the data inputed 
    ///     from the login screen will be stamped onto this object
    ///     and processed.</para>
    /// </summary>
    public struct Details
    {
        #region Properties
        /// <summary>
        /// Gets the game session for this character.
        /// </summary>
        public GameSession Session { get; private set; }

        /// <summary>
        /// Gets the character's username.
        /// </summary>
        public string Username { get; private set; }
        /// <summary>
        /// Gets the character's password.
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// Gets whether the character is using hd or not.
        /// </summary>
        public bool Hd { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new character details.
        /// </summary>
        /// <param name="username">The character's username.</param>
        /// <param name="password">The character's password.</param>
        /// <param name="hd">Whether the character is using an HD client or not.</param>
        /// <param name="clientKey">The character's client session key.</param>
        /// <param name="serverKey">The character's server session key.</param>
        public Details(Node node, string username, string password, bool hd, long clientKey, long serverKey) : this()
        {
            this.Username = username;
            this.Password = password;
            this.Hd = hd;
            this.Session = new GameSession(node, clientKey, serverKey);
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Prints out the character's username and password.
        /// </summary>
        /// <returns>The character's username and password.</returns>
        public override string ToString()
        {
            return "username=" + this.Username + ", password=" + this.Password;
        }
        #endregion Methods
    }
}
