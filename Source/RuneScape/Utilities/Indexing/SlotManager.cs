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

using RuneScape.Model.Characters;

namespace RuneScape.Utilities.Indexing
{
    /// <summary>
    /// Defines management of character client slots.
    /// </summary>
    public class SlotManager
    {
        #region Fields
        /// <summary>
        /// Represents slots open or taken by characters. This is also the index id used to send to the client.
        /// </summary>
        ClientSlot[] slots = null;
        /// <summary>
        /// Provides reservation of reserving and releasing client slots.
        /// </summary>
        private object lockPad = new object();
        /// <summary>
        /// The slot capacity.
        /// </summary>
        private int capacity;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructws a slot manager.
        /// </summary>
        public SlotManager(int capacity)
        {
            this.capacity = capacity;
            this.slots = new ClientSlot[capacity];

            /*
             * Reset all slots.
             */
            for (int i = 0; i < this.slots.Length; i++)
            {
                this.slots[i].Release();
            }
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Gets a free slot in the client slots array.
        /// </summary>
        /// <returns>Returns the id of the free slot.</returns>
        private short GetFreeSlotId()
        {
            for (short i = 1; i < GameServer.TcpConnection.MaxConnections; i++)
            {
                if (!slots[i].Reserved)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Releases a slot in the client slots array.
        /// </summary>
        /// <param name="slotId">The slot to release.</param>
        public void ReleaseSlot(int slotId)
        {
            lock (this.lockPad)
            {
                if (slotId > -1)
                {
                    this.slots[slotId].Release();
                }
            }
        }

        /// <summary>
        /// Releases the slot in the client's slot array.
        /// </summary>
        /// <param name="character">The character holding the slot.</param>
        public void ReleaseSlot(Character character)
        {
            ReleaseSlot(character.Index);
        }

        /// <summary>
        /// Reserves a slot specified to the character.
        /// </summary>
        /// <param name="character">The character to reserve slot for.</param>
        /// <returns>Returns true if the reservation was successful; False if not.</returns>
        public bool ReserveSlot(Character character)
        {
            if (character == null) return false;

            short slotId = -1;
            lock (this.lockPad)
            {
                if ((slotId = GetFreeSlotId()) != -1)
                {
                    this.slots[slotId].Reserve(character.SessionId, character.MasterId);
                    character.Index = slotId;
                    return true;
                }
            }
            return false;
        }
        #endregion Methods
    }
}
