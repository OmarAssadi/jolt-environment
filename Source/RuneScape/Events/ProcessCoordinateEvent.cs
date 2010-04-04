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
    /// An event that processes a coordinate event.
    /// </summary>
    public class ProcessCoordinateEvent : Event
    {
        #region Fields
        /// <summary>
        /// The coordinate event to process.
        /// </summary>
        private CoordinateEvent ce;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a coordiante processor event..
        /// </summary>
        public ProcessCoordinateEvent(CoordinateEvent e) 
            : base(0)
        {
            this.ce = e;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Executes the event.
        /// </summary>
        public override void Execute()
        {
            if (this.ce.Character.UpdateFlags.Teleporting)
            {
                Stop();
                return;
            }

            bool atTarget = this.ce.Character.Location.WithinDistance(this.ce.Location, this.ce.Distance);

            // Most likely the character had clicked somewhere else.
            if (ce.FailedAttempts >= 2)
            {
                Stop();
                ce.Cancel();
            }
            // The character is at the required targeted location, execute event.
            else if (atTarget)
            {
                ce.Reached = true;
                ce.Execute();
                Stop();
            }
            // The character hasn't reached the targeted located yet.
            else
            {
                if (!ce.Character.Location.Equals(ce.OldLocation))
                {
                    ce.OldLocation = ce.Character.Location;

                    // NOTE: reference(s) may screw things up.
                    //ce.OldLocation = Model.Location.Create(ce.Character.Location.X, ce.Character.Location.Y, ce.Character.Location.Z);
                }
                else
                {
                    ce.FailedAttempts++;
                }
            }
            this.Delay = 600;
        }
        #endregion Methods
    }
}
