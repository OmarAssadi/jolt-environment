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
    /// Management for event based services.
    /// </summary>
    public class EventManager
    {
        /// <summary>
        /// Submits an event with a interval.
        /// </summary>
        /// <param name="interval">The interval between each execution.</param>
        /// <param name="handler">The method to execute each interval/interation.</param>
        /// <returns>Returns the crated timer instance.</returns>
        public Timer Submit(long interval, ElapsedEventHandler handler)
        {
            return Submit(interval, 0, handler);
        }

        /// <summary>
        /// Submit an event with a interval and a iteration.
        /// </summary>
        /// <param name="interval">The interval between each execution.</param>
        /// <param name="iterations">Amount of executions to produce.</param>
        /// <param name="handler">The method to execute each interval/interation.</param>
        /// <returns>Returns the crated timer instance.</returns>
        public Timer Submit(long interval, int iterations, ElapsedEventHandler handler)
        {
            var timer = new Timer(interval);
            timer.Elapsed += handler;
            if (iterations > 0)
                timer.Elapsed += (s, e) =>
                {
                    if (--iterations <= 0)
                        timer.Dispose();
                };
            return timer;
        }

        /// <summary>
        /// Submits an event.
        /// </summary>
        /// <param name="evnt">The event to execute.</param>
        /// <returns>Returns the timer instance.</returns>
        public void Submit(Event e)
        {
            var timer = new Timer(e.Interval);
            timer.Elapsed += e.Execute;
            if (e.Iterations > 0)
                timer.Elapsed += (s, eArgs) =>
                {
                    if (--e.Iterations <= 0 || !e.IsRunning)
                        timer.Dispose();
                };
        }
    }
}
