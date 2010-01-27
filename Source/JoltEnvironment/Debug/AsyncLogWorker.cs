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
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;

namespace JoltEnvironment.Debug
{
    /// <summary>
    /// An asynchronous log worker which processes all logs via a dedicated thread.
    /// </summary>
    public class AsyncLogWorker
    {
        #region Fields
        /// <summary>
        /// A thread which processes logs.
        /// </summary>
        private Thread thread;
        /// <summary>
        /// A blocking queue of log messages.
        /// </summary>
        private BlockingCollection<Action> queue;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs the asychronous log worker.
        /// </summary>
        public AsyncLogWorker()
        {
            this.queue = new BlockingCollection<Action>();

            this.thread = new Thread(Process);
            this.thread.Name = "Log Worker";
            this.thread.Priority = ThreadPriority.Lowest;
            this.thread.Start();
	    }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Processes the logs.
        /// </summary>
        private void Process()
        {
            while (true)
            {
                try
                {
                    queue.Take().Invoke();

                    /* The thread needs to sleep for atleast 1 millisecond, 
                     * otherwise it could very likely take up alot of cpu 
                     * when there's lots of logs queued.
                     */
                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Queues a log.
        /// </summary>
        /// <param name="action">The log action to queue.</param>
        public void QueueLog(Action action)
        {
            this.queue.Add(action);
        }
        #endregion Methods
    }
}
