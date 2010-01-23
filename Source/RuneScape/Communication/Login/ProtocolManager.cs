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

using RuneScape.Communication.Login.Handlers;

namespace RuneScape.Communication.Login
{
    /// <summary>
    /// Manages the login protocols.
    /// </summary>
    public class ProtocolManager
    {
        #region Fields
        /// <summary>
        /// A container holding all protcol handlers availible.
        /// </summary>
        private Dictionary<int, IProtocolHandler> handlers = new Dictionary<int, IProtocolHandler>();
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a new protocol manager by loading all required handlers.
        /// </summary>
        public ProtocolManager()
        {
            LoadHandlers();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Handles the given request.
        /// </summary>
        /// <param name="id">The id of the protocol to handle.</param>
        /// <param name="request">The login request to handle for.</param>
        public void Handle(int id, LoginRequest request)
        {
            if (this.handlers.ContainsKey(id))
            {
                try
                {
                    this.handlers[id].Handle(request);
                }
                catch (Exception ex)
                {
                    Program.Logger.WriteException(ex);
                }
            }
            else
            {
                request.Remove = true;
                Program.Logger.WriteDebug("Unhandled protocol: " + id);
            }
        }

        /// <summary>
        /// Loads the handlers into the handlers container.
        /// </summary>
        private void LoadHandlers()
        {
            AddHandler(2, new ClientUpdateProtocolHandler());
            AddHandler(14, new SessionRequestProtocolHandler());
            AddHandler(15, new ClientVerificationProtocolHandler());
            AddHandler(16, new LoginVerificationProtocolHandler());
            AddHandler(18, new ReconnectionProtocolHandler());
            AddHandler(48, new AccountPasswordProtocolHandler());
            AddHandler(85, new AccountInfoProtocolHandler());
            AddHandler(118, new AccountNameProtocolHandler());
        }

        /// <summary>
        /// Adds a single handler into the handlers container.
        /// </summary>
        /// <param name="id">The protocol id/handle id.</param>
        /// <param name="handler">The handler instance.</param>
        private void AddHandler(int id, IProtocolHandler handler)
        {
            this.handlers.Add(id, handler);
        }
        #endregion Methods
    }
}
