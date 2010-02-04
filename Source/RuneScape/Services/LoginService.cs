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

using RuneScape.Communication.Login;

namespace RuneScape.Services
{
    /// <summary>
    /// Serves as a login service.
    /// </summary>
    public class LoginService
    {
        #region Fields
        /// <summary>
        /// A container for login requests.
        /// </summary>
        //private HashSet<LoginRequest> requests = new HashSet<LoginRequest>();
        private List<LoginRequest> requests = new List<LoginRequest>();
        /// <summary>
        /// Provides locking of adding and removing 
        /// </summary>
        private object requestLock = new object();
        /// <summary>
        /// Manages the protocol handlers.
        /// </summary>
        private ProtocolManager protocolManager = new ProtocolManager();
        #endregion Fields

        #region Methods
        /// <summary>
        /// Processes the login requests.
        /// </summary>
        public void Process()
        {
            if (this.requests.Count > 0)
            {
                List<LoginRequest> toProcess = null;
                lock (this.requests)
                {
                    toProcess = new List<LoginRequest>(this.requests);
                }

                toProcess.ForEach((request) =>
                {
                    if (request.Connection.Age >= TimeSpan.FromSeconds(15)
                        || request.Finished
                        || request.Remove)
                    {
                        // If the request wasn't finished, it no longer requires processing.
                        if (!request.Finished)
                        {
                            request.OnRequestDisconnect();
                        }

                        // A condition has been met, so the request has to be removed.
                        RemoveRequest(request);
                    }

                    // Make sure the buffer is availible, otherwise there's no point in processing.
                    else if (request.Buffer != null)
                    {
                        if (request.Buffer.RemainingAmount >= 1)
                        {
                            int protocolId = request.Buffer.Peek();
                            this.protocolManager.Handle(protocolId, request);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Adds a request to the request container.
        /// </summary>
        /// <param name="request">The request object which needs to be added.</param>
        public void AddRequest(LoginRequest request)
        {
            lock (this.requestLock)
            {
                this.requests.Add(request);
            }
        }

        /// <summary>
        /// Removes a request from the request container.
        /// </summary>
        /// <param name="request">The request object which needs to be removed.</param>
        public void RemoveRequest(LoginRequest request)
        {
            lock (this.requestLock)
            {
                this.requests.Remove(request);
            }
        }
        #endregion Methods
    }
}
