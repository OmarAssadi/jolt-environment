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

namespace RuneScape.Model.Npcs
{
    /// <summary>
    /// Represents a single non-player character in the world.
    /// </summary>
    public class Npc : Creature
    {
        #region Properties
        /// <summary>
        /// Gets the npc's name.
        /// </summary>
        public override string Name
        {
            get; protected set;
        }

        /// <summary>
        /// Gets the npc's update flags.
        /// </summary>
        public UpdateFlags UpdateFlags { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new non-player character.
        /// </summary>
        /// <param name="indexId">The cache index id of the npc.</param>
        public Npc(int indexId)
        {
            this.Index = indexId;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Plays a graphical display.
        /// </summary>
        /// <param name="graphic">The graphic to display.</param>
        public override void PlayGraphics(Graphic graphic)
        {
            this.CurrentGraphic = graphic;
        }

        /// <summary>
        /// Plays a animation display.
        /// </summary>
        /// <param name="animation">The animation to display.</param>
        public override void PlayAnimation(Animation animation)
        {
            this.CurrentAnimation = animation;
        }

        /// <summary>
        /// Executes routine procedures.
        /// </summary>
        public override void Tick()
        {
        }
        #endregion Methods
    }
}
