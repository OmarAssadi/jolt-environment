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
using System.Timers;

namespace JoltEnvironment.Service.Events
{
    /// <summary>
    /// Represents a single event.
    /// 
    ///     <para>Events are used for timed / looped executions that can be stopped if needed.</para>
    /// </summary>
    public abstract class Event
    {
        #region Properties
        /// <summary>
        /// Interval between each execution.
        /// </summary>
        public long Interval { get; set; }
        /// <summary>
        /// Amount of executions that will be produced.
        /// </summary>
        public int Iterations { get; set; }
        /// <summary>
        /// Whether the event is running or not.
        /// </summary>
        public bool IsRunning { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new event.
        /// </summary>
        /// <param name="interval">The interval between each execution.</param>
        public Event(long interval)
        {
            this.Interval = interval;
            this.Iterations = 0;
            this.IsRunning = true;
        }

        /// <summary>
        /// Constructs a new event.
        /// </summary>
        /// <param name="interval">The interval between each execution.</param>
        /// <param name="iterations">Amount of executions to produce.</param>
        public Event(long interval, int iterations)
        {
            this.Interval = interval;
            this.Iterations = iterations;
            this.IsRunning = true;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// The execution method. When a event is made, this method is called.
        /// </summary>
        /// <param name="context">Any data that may have to be passed on.</param>
        /// <param name="e"></param>
        public abstract void Execute(Object context, ElapsedEventArgs e);

        /// <summary>
        /// Incase the event needs to be stopped, this method can 
        /// be called to stop the thread at the next interation.
        /// </summary>
        public void Stop()
        {
            this.IsRunning = false;
        }
        #endregion Methods
    }
}
