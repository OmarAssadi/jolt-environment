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

using RuneScape.Services;

namespace RuneScape.Workers
{
    /// <summary>
    /// A worker processing major update tasks.
    /// </summary>
    public class MajorWorker : Worker
    {
        #region Fields
        /// <summary>
        /// The interval between each interation.
        /// </summary>
        public const int Interval = 50;

        /// <summary>
        /// Services providing processing.
        /// </summary>
        public MajorUpdatesService service = new MajorUpdatesService();
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a new major worker.
        /// </summary>
        public MajorWorker() : base("Major Worker", ThreadPriority.Highest) { }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Thread's process.
        /// </summary>
        protected override void Run()
        {
            while (GameServer.IsRunning)
            {
                try
                {
                    /*
                     * Puts the thread to sleep for a few milliseconds.
                     */ 
                    Thread.Sleep(MajorWorker.Interval);

                    /*
                     * Processes the major (player/npc/world) updates.
                     */
                    service.Process();
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }
            }
            this.Abort();
        }
        #endregion Methods
    }
}
