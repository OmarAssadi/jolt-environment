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

namespace RuneScape.Events
{
    /// <summary>
    /// Represents a task triggered by ticks.
    /// </summary>
    public abstract class Event
    {
        #region Properties
        /// <summary>
        /// Gets or sets the event delay between each execution.
        /// </summary>
        public int Delay { get; protected set; }
        /// <summary>
        /// Gets or sets whether the event is running.
        /// </summary>
        public bool Running { get; protected set; }
        /// <summary>
        /// Gets the last run time.
        /// </summary>
        public DateTime LastRun { get; set; }

        /// <summary>
        /// Gets whether the event is ready to be executed.
        /// </summary>
        public bool Ready
        {
            get
            {
                if (!this.Running)
                {
                    return false;
                }
                return (DateTime.Now - LastRun).TotalMilliseconds >= this.Delay;
            }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new event.
        /// </summary>
        /// <param name="delay">The delay betwen each execution.</param>
        public Event(int delay)
        {
            this.Delay = delay;
            this.LastRun = DateTime.Now;
            this.Running = true;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Stops the event.
        /// <remarks>Making this call will drop the event;
        /// you cannot start this instance again.</remarks>
        /// </summary>
        public void Stop()
        {
            this.Running = false;
        }

        /// <summary>
        /// Executes the event.
        /// </summary>
        public abstract void Execute();
        #endregion Methods
    }
}
