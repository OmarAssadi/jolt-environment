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

using RuneScape.Communication.Messages;
using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Characters;

namespace RuneScape.Model.Objects
{
    /// <summary>
    /// Represents a single object visible in the game.
    /// </summary>
    public class MapObject : Entity
    {
        #region Properties
        /// <summary>
        /// Gets the object model type.
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Gets the object face direction.
        /// </summary>
        public int Face { get; set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new world object.
        /// </summary>
        /// <param name="objectId">The object's cache id.</param>
        /// <param name="type">The object's model type.</param>
        /// <param name="face">The object's face direction.</param>
        /// <param name="location">The object's coordinates.</param>
        public MapObject(short objectId, int type, int face, Location location) 
            : base(location)
        {
            this.Index = objectId;
            this.Name = "object" + objectId;
            this.Type = type;
            this.Face = face;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Spawns the object for the specified character.
        /// </summary>
        /// <param name="character">The character to spawn object for.</param>
        public void Spawn(Character character)
        {
            character.Session.SendData(new CoordinatePacketComposer(character, this.Location).Serialize());
            character.Session.SendData(new SpawnObjectPacketComposer((short)this.Index, this.Type, this.Face).Serialize());
        }

        /// <summary>
        /// Spawns the object for all characters online.
        /// </summary>
        public void SpawnGlobal()
        {
            List<Character> characters = new List<Character>(GameEngine.World.CharacterManager.Characters.Values);
            characters.ForEach((c) =>
            {
                if (c.Location.WithinDistance(this.Location))
                {
                    Spawn(c);
                }
            });
        }

        /// <summary>
        /// Despawns the object for the specified character.
        /// </summary>
        /// <param name="character">The character to depspawn object for.</param>
        public void Despawn(Character character)
        {
            character.Session.SendData(new CoordinatePacketComposer(character, this.Location).Serialize());
            character.Session.SendData(new DespawnObjectPacketComposer().Serialize());
        }

        /// <summary>
        /// Despawns the object for all characters online.
        /// </summary>
        public void DespawnGlobal()
        {
            List<Character> characters = new List<Character>(GameEngine.World.CharacterManager.Characters.Values);
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
