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
using System.Threading;

namespace RuneScape.Workers
{
    /// <summary>
    /// Represents a managed worker. This is a abstract class.
    /// </summary>
    public abstract class Worker
    {
        #region Fields
        /// <summary>
        /// The worker's base thread.
        /// </summary>
        protected Thread workerThread;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a new worker.
        /// </summary>
        /// <param name="name">The worker thread's name.</param>
        /// <param name="priority">The worker thread's priority.</param>
        /// <param name="threadMethod">The method that provides for the thread.</param>
        public Worker(string name, ThreadPriority priority)
        {
            this.workerThread = new Thread(new ThreadStart(Run));
            this.workerThread.Name = name;
            this.workerThread.Priority = priority;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Starts the worker's thread.
        /// </summary>
        public void Start()
        {
            this.workerThread.Start();
        }

        /// <summary>
        /// Aborts the worker's thread.
        /// </summary>
        public void Abort()
        {
            this.workerThread.Abort();
        }

        /// <summary>
        /// The thread's process.
        /// </summary>
        protected abstract void Run();
        #endregion Methods
    }
}
