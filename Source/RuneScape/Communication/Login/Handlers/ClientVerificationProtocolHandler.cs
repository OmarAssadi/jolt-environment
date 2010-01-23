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

namespace RuneScape.Communication.Login.Handlers
{
    /// <summary>
    /// Handles client verification.
    /// 
    ///     <para>When a character attempts to first connect to the server, 
    ///     he/she must go through a verification to check if the client 
    ///     used to connect to the server is valid and will work properly 
    ///     when attempting to login. If the user has passed the 
    ///     validation, he/she can now be sent update keys.</para>
    /// </summary>
    public class ClientVerificationProtocolHandler : IProtocolHandler
    {
        #region Fields
        /// <summary>
        /// The protocol id of this handle.
        /// </summary>
        public const int ProtocolId = 15;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the protocol request.
        /// </summary>
        /// <param name="request">The instance requesting the protcol handle.</param>
        public void Handle(LoginRequest request)
        {
            if (request.Buffer.RemainingAmount >= 5)
            {
                // This information does not require any actions, so we will skip them.
                request.Buffer.Skip(3);

                // Verify that the client is revision 508.
                if (request.Buffer.ReadShort() == 508)
                {
                    request.Connection.SendData(0);
                    request.Buffer = null;
                }
            }
        }
        #endregion Methods
    }
}
