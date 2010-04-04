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
using RuneScape.Model.Characters;

namespace RuneScape.Content.Trading
{
    /// <summary>
    /// A coordinated event that requests a trade between the specified characters.
    /// </summary>
    public class RequestTradeEvent : CoordinateEvent
    {
        #region Fields
        /// <summary>
        /// The character to request.
        /// </summary>
        private Character other;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a new trade (coordinated) event.
        /// </summary>
        /// <param name="character">The character being coordinated.</param>
        /// <param name="other">The character to reach.</param>
        public RequestTradeEvent(Character character, Character other)
            : base(character, other.Location)
        {
            this.other = other;
            this.Distance = 1;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Executes the trigger.
        /// </summary>
        public override void Execute()
        {
            if (this.Character.Location.WithinInteractionDistance(this.Location))
            {
                this.Character.Request.RequestTrade(other);
            }
        }
        #endregion Methods
    }
}
