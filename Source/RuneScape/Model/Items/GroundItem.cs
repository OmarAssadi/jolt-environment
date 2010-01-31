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

using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Characters;

namespace RuneScape.Model.Items
{
    /// <summary>
    /// Represents a single item that is visible on the ground.
    /// </summary>
    public class GroundItem : Item, IEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the location at which the item is located on the ground.
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// The character who had dropped the item (if there is no owner, returns null).
        /// </summary>
        public Character Character { get; private set; }

        /// <summary>
        /// Gets or sets the time at which the item was dropped.
        /// </summary>
        public DateTime TimeCreated { get; private set; }
        /// <summary>
        /// Whether to destroy the item after the self-show timer is done.
        /// </summary>
        public bool Destroyed { get; private set; }
        /// <summary>
        /// Whether the item was spawned.
        /// </summary>
        public bool Spawned { get; private set; }

        /// <summary>
        /// The timespawn between now and the time the item was created.
        /// </summary>
        public TimeSpan Age
        {
            get
            {
                TimeSpan span = DateTime.Now - this.TimeCreated;
                if (span <= TimeSpan.Zero)
                    return TimeSpan.Zero;
                return span;
            }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new ground item.
        /// </summary>
        /// <param name="location">The location at which the item will be visible.</param>
        /// <param name="character">Any character that may own the item.</param>
        /// <param name="itemId">The id of the item to spawn.</param>
        /// <param name="itemCount">The amount of the item to spawn.</param>
        /// <param name="destroy">Whether to destroy the item after self-show timer is done.</param>
        public GroundItem(Location location, Character character, short itemId, int itemCount) 
            : base(itemId, itemCount)
        {
            this.Location = location;
            this.Character = character;
            this.Spawned = (this.Character == null) ? true : false;
        }

        /// <summary>
        /// Constructs a spawned ground item.
        /// </summary>
        /// <param name="location">The location at which the item will be visible.</param>
        /// <param name="itemId">The id of the item to spawn.</param>
        /// <param name="itemCount">The amount of the item to spawn.</param>
        public GroundItem(Location location, short itemId, int itemCount) : this(location, null, itemId, itemCount) { }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Spawns the ground item.
        /// </summary>
        public void Spawn()
        {
            if (this.Character != null)
            {
                this.Character.Session.SendData(new CoordinatePacketComposer(this.Character, this.Location).Serialize());
                this.Character.Session.SendData(new SpawnGroundItemPacketComposer(this.Id, this.Count).Serialize());
            }
            else
            {
                List<Character> characters = new List<Character>(GameEngine.World.CharacterManager.Characters.Values);
                characters.ForEach((c) =>
                {
                    c.Session.SendData(new CoordinatePacketComposer(this.Character, this.Location).Serialize());
                    c.Session.SendData(new SpawnGroundItemPacketComposer(this.Id, this.Count).Serialize());
                });
            }
        }

        /// <summary>
        /// Despawns the item.
        /// </summary>
        public void Despawn()
        {
            if (this.Character != null)
            {
                this.Character.Session.SendData(new CoordinatePacketComposer(this.Character, this.Location).Serialize());
                this.Character.Session.SendData(new DespawnGroundItemPacketComposer(this.Id).Serialize());
            }
            else
            {
                List<Character> characters = new List<Character>(GameEngine.World.CharacterManager.Characters.Values);
                characters.ForEach((c) =>
                {
                    c.Session.SendData(new CoordinatePacketComposer(this.Character, this.Location).Serialize());
                    c.Session.SendData(new DespawnGroundItemPacketComposer(this.Id).Serialize());
                });
            }
        }
        #endregion Methods
    }
}
