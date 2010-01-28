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
    /// Represents a npc's update flags.
    /// </summary>
    public class UpdateFlags
    {
        #region Fields
        /// <summary>
        /// Gets or sets whether the npc's animation requires updating.
        /// </summary>
        public bool AnimationUpdateRequired { get; set; }
        /// <summary>
        /// Gets or sets whether the npc's chat requried updating.
        /// </summary>
        public bool SpeakUpdateRequired { get; set; }
        /// <summary>
        /// Gets or sets whether the npc's hit update requires updating.
        /// </summary>
        public bool DamageUpdateRequired { get; set; }
        /// <summary>
        /// Gets or sets whether the npc's hit2 update requires updating.
        /// </summary>
        public bool Damage2UpdateRequired { get; set; }
        /// <summary>
        /// Gets or sets whether the character's faceto requires updating.
        /// </summary>
        public bool FaceToUpdateRequired { get; set; }
        /// <summary>
        /// Gets or sets whether the character's graphics required updating.
        /// </summary>
        public bool GraphicsUpdateRequired { get; set; }

        /// <summary>
        /// Gets or sets the face to direction.
        /// </summary>
        public short FaceTo { get; set; }
        /// <summary>
        /// Gets or sets whether to clear (current) face to direction.
        /// </summary>
        public bool ClearFaceTo { get; set; }

        public bool UpdateRequired
        {
            get
            {
                return this.AnimationUpdateRequired 
                    || this.SpeakUpdateRequired 
                    || this.DamageUpdateRequired 
                    || this.Damage2UpdateRequired 
                    || this.FaceToUpdateRequired 
                    || this.GraphicsUpdateRequired;
            }
        }
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a npc's update flags.
        /// </summary>
        public UpdateFlags()
        {
            this.FaceTo = -1;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Clears any flagged updates.
        /// </summary>
        public void Clear()
        {
            this.AnimationUpdateRequired = false;
            this.SpeakUpdateRequired = false;
            this.DamageUpdateRequired = false;
            this.Damage2UpdateRequired = false;
            this.FaceToUpdateRequired = false;
            this.GraphicsUpdateRequired = false;
            this.FaceTo = -1;
        }
        #endregion Methods
    }
}
