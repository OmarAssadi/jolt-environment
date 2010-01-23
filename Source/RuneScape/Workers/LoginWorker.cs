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
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using RuneScape.Communication.Login;
using RuneScape.Services;

namespace RuneScape.Workers
{
    /// <summary>
    /// A worker processing "login" tasks.
    /// </summary>
    public class LoginWorker : Worker
    {
        #region Fields
        /// <summary>
        /// The interval between each thread iteration.
        /// </summary>
        public const int Interval = 500;

        /// <summary>
        /// Services providing processing.
        /// </summary>
        public LoginService service = new LoginService();
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a new login worker on the highest priority.
        /// </summary>
        public LoginWorker() : base("Login Worker", ThreadPriority.AboveNormal) { }

        /// <summary>
        /// Constructs a new login worker with a specific priority.
        /// </summary>
        /// <param name="priority">The priority of the worker thread.</param>
        public LoginWorker(ThreadPriority priority) : base("Login Worker", priority) { }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Thread's proccess.
        /// </summary>
        protected override void Run()
        {
            while (GameServer.IsRunning)
            {
                try
                {
                    /*
                     * Allow some rest before processing again so the remote 
                     * endpoint is able to send the amount of data it needs to.
                     * Process any requests.
                     */ 
                    Thread.Sleep(LoginWorker.Interval);
                }
                catch (Exception ex)
                {
                    Program.Logger.WriteException(ex);
                }

                this.service.Process();
            }
            this.Abort();
        }

        /// <summary>
        /// Adds a request for the service.
        /// </summary>
        /// <param name="request">The request to add.</param>
        public void AddRequest(LoginRequest request)
        {
            this.service.AddRequest(request);
        }
        #endregion Methods
    }
}
