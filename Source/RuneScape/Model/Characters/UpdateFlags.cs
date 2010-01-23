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

namespace RuneScape.Model.Characters
{
    /// <summary>
    /// Represents a character's update flags.
    /// </summary>
    public class UpdateFlags
    {
        #region Properties
        /// <summary>
        /// Gets or sets the character's last region.
        /// </summary>
        public Location LastRegion { get; set; }
        /// <summary>
        /// Gets or sets the character's teleport location on the game world.
        /// </summary>
        public Location TeleportLocation { get; set; }

        /// <summary>
        /// Gets or sets whether the character has teleported.
        /// </summary>
        public bool Teleporting { get; set; }
        /// <summary>
        /// Gets or sets whether the character's map region has changed.
        /// </summary>
        public bool MapRegionChanging { get; set; }

        /// <summary>
        /// Gets or sets whether the character's animation requires updating.
        /// </summary>
        public bool AnimationUpdateRequired { get; set; }
        /// <summary>
        /// Gets or sets whether the character's appearance requires updating.
        /// </summary>
        public bool AppearanceUpdateRequired { get; set; }
        /// <summary>
        /// Gets or sets whether the character's chat text requires updating.
        /// </summary>
        public bool ChatUpdateRequired { get; set; }
        /// <summary>
        /// Gets or sets whether the character's face to requires updating.
        /// </summary>
        public bool FaceToUpdateRequired { get; set; }
        /// <summary>
        /// Gets or sets whether the character's graphics requires updating.
        /// </summary>
        public bool GraphicsUpdateRequired { get; set; }
        /// <summary>
        /// Gets or sets whether the character's hit update requires updating.
        /// </summary>
        public bool DamageUpdateRequired { get; set; }
        /// <summary>
        /// Gets or sets whether the character's hit2 update requires updating.
        /// </summary>
        public bool Damage2UpdateRequired { get; set; }

        /// <summary>
        /// Gets or sets the face to direction.
        /// </summary>
        public short FaceTo { get; set; }

        /// <summary>
        /// Gets or sets whether the character's face to should be reset.
        /// </summary>
        public bool ClearFaceTo { get; set; }

        /// <summary>
        /// Gets whether the character requires some sort of update.
        /// </summary>
        public bool UpdateRequired 
        { 
            get { return 
                this.AnimationUpdateRequired 
                || this.AppearanceUpdateRequired 
                || this.ChatUpdateRequired 
                || this.FaceToUpdateRequired 
                || this.GraphicsUpdateRequired 
                || this.Damage2UpdateRequired 
                || this.DamageUpdateRequired; 
            } 
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new character update flags.
        /// </summary>
        public UpdateFlags()
        {
            Clear();
            ClearFaceTo = false;
            AppearanceUpdateRequired = true;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Clears all update requests.
        /// </summary>
        public void Clear()
        {
            this.AnimationUpdateRequired = false;
            this.AppearanceUpdateRequired = false;
            this.ChatUpdateRequired = false;
            this.Teleporting = false;
            this.FaceTo = -1;
            this.FaceToUpdateRequired = false;
            this.GraphicsUpdateRequired = false;
            this.Damage2UpdateRequired = false;
            this.DamageUpdateRequired = false;
            this.MapRegionChanging = false;
        }
        #endregion Methods
    }
}
