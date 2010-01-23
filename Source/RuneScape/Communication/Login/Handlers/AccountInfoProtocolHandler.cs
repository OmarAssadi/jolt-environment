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
    /// Handles the personal information entered to create a new account.
    /// </summary>
    public class AccountInfoProtocolHandler : IProtocolHandler
    {
        #region Fields
        /// <summary>
        /// The protocol id of this handle.
        /// </summary>
        public const int InterfaceId = 85;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the protocol request.
        /// </summary>
        /// <param name="request">The instance requesting the protcol handle.</param>
        public void Handle(LoginRequest request)
        {
            request.Buffer.Skip(1);
            if (request.Buffer.RemainingAmount >= 6)
            {
                byte day = request.Buffer.ReadByte();
                byte month = (byte)(request.Buffer.ReadByte() + 1);
                short year = request.Buffer.ReadShort();
                short country = request.Buffer.ReadShort();

                // Check if world allows account creation.
                if (GameEngine.World.AccountCreationEnabled)
                {
                    request.Connection.SendData((byte)AccountCreationReturnCode.Good);
                }
                else
                {
                    request.Connection.SendData((byte)AccountCreationReturnCode.Disabled);
                }
            }
        }
        #endregion Methods
    }
}
