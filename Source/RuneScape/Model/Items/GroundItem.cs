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
        #region Fields
        /// <summary>
        /// The character who dropped the item (null if none).
        /// </summary>
        private Character character;
        /// <summary>
        /// Synchronized access for the character.
        /// </summary>
        private object charObj = new object();
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets or sets the location at which the item is located on the ground.
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// Gets the character.
        /// </summary>
        public Character Character 
        {
            get
            {
                lock (this.charObj)
                {
                    return this.character;
                }
            }
            set
            {
                lock (this.charObj)
                {
                    this.character = value;
                }
            } 
        }
        /// <summary>
        /// Gets or sets the time at which the item was dropped.
        /// </summary>
        public DateTime TimeCreated { get; set; }
        /// <summary>
        /// Whether to destroy the item after the self-show timer is done.
        /// </summary>
        public bool Destroyed { get; set; }
        /// <summary>
        /// Whether the item was spawned.
        /// </summary>
        public bool Spawned { get; set; }

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
            this.TimeCreated = DateTime.Now;
        }

        /// <summary>
        /// Constructs a spawned ground item.
        /// </summary>
        /// <param name="location">The location at which the item will be visible.</param>
        /// <param name="itemId">The id of the item to spawn.</param>
        /// <param name="itemCount">The amount of the item to spawn.</param>
        public GroundItem(Location location, short itemId, int itemCount) 
            : this(location, null, itemId, itemCount) { }
        /// <summary>
        /// Constructs a character's dropped item.
        /// </summary>
        /// <param name="character">Any character that may own the item.</param>
        /// <param name="itemId">The id of the item to spawn.</param>
        /// <param name="itemCount">The amount of the item to spawn.</param>
        public GroundItem(Character character, short itemId, int itemCount) 
            : this(character.Location, character, itemId, itemCount) { }
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
        }

        /// <summary>
        /// Spawns a ground item for the specified character.
        /// </summary>
        /// <param name="character">The character to spawn item for.</param>
        public void Spawn(Character character)
        {
            character.Session.SendData(new CoordinatePacketComposer(character, this.Location).Serialize());
            character.Session.SendData(new SpawnGroundItemPacketComposer(this.Id, this.Count).Serialize());
        }

        /// <summary>
        /// Spawns the ground item for all characters within distance.
        /// </summary>
        public void GlobalSpawn()
        {
            List<Character> characters = new List<Character>(
                GameEngine.World.CharacterManager.Characters.Values);

            characters.ForEach((c) =>
            {
                if (c.Location.WithinDistance(this.Location))
                {
                    Spawn(c);
                }
            });
        }

        /// <summary>
        /// Despawns the item.
        /// </summary>
        public void Despawn()
        {
            if (this.Character != null && !this.Character.Session.Disconnected)
            {
                this.Character.Session.SendData(new CoordinatePacketComposer(this.Character, this.Location).Serialize());
                this.Character.Session.SendData(new DespawnGroundItemPacketComposer(this.Id).Serialize());
            }
        }

        /// <summary>
        /// Despawns the item for the specified character.
        /// </summary>
        /// <param name="character">The character to despawn item for.</param>
        public void Despawn(Character character)
        {
            character.Session.SendData(new CoordinatePacketComposer(character, this.Location).Serialize());
            character.Session.SendData(new DespawnGroundItemPacketComposer(this.Id).Serialize());
        }

        /// <summary>
        /// Despawns the item for all characters within distance.
        /// </summary>
        public void GlobalDespawn()
        {
            List<Character> characters = new List<Character>(
                GameEngine.World.CharacterManager.Characters.Values);

            characters.ForEach((c) =>
            {
                if (c.Location.WithinDistance(this.Location))
                {
                    Despawn(c);
                }
            });
        }
        #endregion Methods
    }
}