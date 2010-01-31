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

namespace RuneScape.Model
{
    /// <summary>
    /// Represents a signle creature inside the game world.
    /// </summary>
    public abstract class Creature : IEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets The unique client slot id given at creature entry.
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Gets the name of the creature.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the location of the character.
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// Gets the current chat message.
        /// </summary>
        public ChatMessage CurrentChatMessage { get; set; }
        /// <summary>
        /// Gets the current graphic display.
        /// </summary>
        public Graphic CurrentGraphic { get; protected set; }
        /// <summary>
        /// Gets the current animation display.
        /// </summary>
        public Animation CurrentAnimation { get; protected set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new creature.
        /// </summary>
        public Creature() { }

        /// <summary>
        /// Constructs a new create with specified location.
        /// </summary>
        /// <param name="location">The location the creature will be placed ingame.</param>
        public Creature(Location location)
        {
            this.Location = location;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Plays a graphical display.
        /// </summary>
        /// <param name="graphic">The graphic to display.</param>
        public abstract void PlayGraphics(Graphic graphic);
        /// <summary>
        /// Plays a animation display.
        /// </summary>
        /// <param name="animation">The animation to display.</param>
        public abstract void PlayAnimation(Animation animation);
        /// <summary>
        /// The creature will speak this message.
        /// </summary>
        /// <param name="message"></param>
        public abstract void Speak(ChatMessage message);
        /// <summary>
        /// Executes routine procedures.
        /// </summary>
        public abstract void Tick();
        #endregion Methods
    }
}
