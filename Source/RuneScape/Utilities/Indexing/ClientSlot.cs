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

namespace RuneScape.Utilities.Indexing
{
    /// <summary>
    /// Represents a slot in the client/server communication.
    /// </summary>
    public struct ClientSlot
    {
        #region Fields
        /// <summary>
        /// Whether the slot is currently reserved.
        /// </summary>
        private bool reserved;

        /// <summary>
        /// The character who has reserved this slot's session id.
        /// </summary>
        private uint characterSessionId;
        /// <summary>
        /// The character who has reserved this slot's master id.
        /// </summary>
        private uint characterMasterId;
        #endregion Fields 

        #region Properties
        /// <summary>
        /// Gets whether the slot is reserved or not.
        /// </summary>
        public bool Reserved { get { return this.reserved; } }
        /// <summary>
        /// Gets the character's session id.
        /// </summary>
        public uint SessionId { get { return this.characterMasterId; } }
        /// <summary>
        /// Gets the character's master id.
        /// </summary>
        public uint MasterId { get { return this.characterSessionId; } }
        #endregion Proeprties

        #region Methods
        /// <summary>
        /// Reserves the slot.
        /// </summary>
        /// <param name="sessionId">The character's session id.</param>
        /// <param name="masterId">The character's master id.</param>
        public void Reserve(uint sessionId, uint masterId)
        {
            this.reserved = true;
            this.characterSessionId = sessionId;
            this.characterMasterId = masterId;
        }

        /// <summary>
        /// Releases the slot.
        /// </summary>
        public void Release()
        {
            this.reserved = false;
            this.characterSessionId = 0;
            this.characterMasterId = 0;
        }
        #endregion Methods
    }
}
