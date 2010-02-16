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

using RuneScape.Events;

namespace RuneScape.Services
{
    /// <summary>
    /// Serves as a event service.
    /// </summary>
    public class EventService
    {
        #region Fields
        /// <summary>
        /// A container of currently waiting/executing events.
        /// </summary>
        private List<Event> events = new List<Event>();
        /// <summary>
        /// A container that temporarily holds evnts that are queed for removal.
        /// </summary>
        private List<Event> removalBin = new List<Event>();

        /// <summary>
        /// An object providing synchronization.
        /// </summary>
        private object lockObj = new object();
        #endregion Fields

        #region Methods
        /// <summary>
        /// Processes all the events.
        /// </summary>
        public void Process()
        {
            lock (this.lockObj)
            {
                // Process the events.
                this.events.ForEach((e) =>
                {
                    if (!e.Running)
                    {
                        this.removalBin.Add(e);
                    }
                    else if (e.Ready)
                    {
                        e.LastRun = DateTime.Now;
                        e.Execute();
                    }
                });

                // Removal of stopped events.
                this.removalBin.ForEach((e) =>
                {
                    events.Remove(e);
                });
                this.removalBin.Clear();
            }
        }

        /// <summary>
        /// Registers an event.
        /// </summary>
        /// <param name="e">The event to register.</param>
        public void Register(Event e)
        {
            lock (this.lockObj)
            {
                this.events.Add(e);
            }
        }

        /// <summary>
        /// Registers a coordinate event.
        /// </summary>
        /// <param name="e">The event to register.</param>
        public void RegisterCoordinateEvent(CoordinateEvent e)
        {
            Register(new ProcessCoordinateEvent(e));
        }
        #endregion Methods
    }
}
