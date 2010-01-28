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

using JoltEnvironment.Utilities;
using RuneScape.Communication;
using RuneScape.Model.Items.Containers;
using RuneScape.Model.Npcs;

namespace RuneScape.Model.Characters
{
    /// <summary>
    /// Represents a single player in the world.
    /// </summary>
    public class Character : Creature
    {
        #region Properties
        /// <summary>
        /// A temporarily given id registering the 
        /// player's session to the server.
        /// </summary>
        public uint SessionId { get; private set; }
        /// <summary>
        /// A permenant id given uniquely to this user 
        /// to define the master id for the database.
        /// </summary>
        public uint MasterId { get; private set; }
        /// <summary>
        /// Gets the character's password.
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// Gets the character's session control.
        /// </summary>
        public GameSession Session { get; private set; }

        /// <summary>
        /// Gets the character's username converted to a Int64 bit integer.
        /// </summary>
        public long LongName { get; private set; }

        /// <summary>
        /// The character's client-side rights to the server.
        /// </summary>
        public ClientRights ClientRights { get; set; }
        /// <summary>
        /// The character's server-side rights to the server.
        /// </summary>
        public ServerRights ServerRights { get; set; }

        /// <summary>
        /// Gets or sets whether the character is muted or not.
        /// </summary>
        public bool Muted { get; set; }

        /// <summary>
        /// Gets the local characters surrounding this character.
        /// </summary>
        public List<Character> LocalCharacters { get; set; }
        /// <summary>
        /// Gets the local npcs surrouding this character.
        /// </summary>
        public List<Npc> LocalNpcs { get; set; }

        /// <summary>
        /// Gets character preferences.
        /// </summary>
        public Preferences Preferences { get; private set; }
        /// <summary>
        /// Gets the character's update flags.
        /// </summary>
        public UpdateFlags UpdateFlags { get; private set; }
        /// <summary>
        /// Gets the character's sprites.
        /// </summary>
        public Sprites Sprites { get; private set; }
        /// <summary>
        /// Gets the character's appearance.
        /// </summary>
        public Appearance Appearance { get; private set; }
        /// <summary>
        /// Gets the character's head icon.
        /// </summary>
        public HeadIcon HeadIcon { get; private set; }
        /// <summary>
        /// Gets the character's walking queue.
        /// </summary>
        public WalkingQueue WalkingQueue { get; private set; }
        /// <summary>
        /// Gets the character's skill.
        /// </summary>
        public Skills Skills { get; private set; }
        /// <summary>
        /// Gets the character's inventory.
        /// </summary>
        public InventoryContainer Inventory { get; private set; }
        /// <summary>
        /// Gets the character's equipment.
        /// </summary>
        public EquipmentContainer Equipment { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new character.
        /// </summary>
        /// <param name="details">The details passed on from the account loader.</param>
        public Character(Details details, uint masterId)
        {
            // Core details of the character.
            this.SessionId = details.Session.Connection.Id;
            this.MasterId = masterId;
            this.Name = details.Username;
            this.Password = details.Password;
            this.Session = details.Session;
            this.Session.Character = this;
            this.LongName = StringUtilities.StringToLong(details.Username);

            // Entity bases.
            this.LocalCharacters = new List<Character>();
            this.LocalNpcs = new List<Npc>();

            // Character sub-components.
            this.Preferences = new Preferences();
            this.Preferences.Hd = details.Hd;
            this.UpdateFlags = new UpdateFlags();
            this.UpdateFlags.LastRegion = this.Location;
            this.Sprites = new Sprites();
            this.Appearance = new Appearance();
            this.HeadIcon = new HeadIcon();
            this.WalkingQueue = new WalkingQueue(this);
            this.Skills = new Skills(this);
            this.Inventory = new InventoryContainer(this);
            this.Equipment = new EquipmentContainer(this);
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
            this.UpdateFlags.GraphicsUpdateRequired = true;
        }

        /// <summary>
        /// Plays a animation display.
        /// </summary>
        /// <param name="animation">The animation to display.</param>
        public override void PlayAnimation(Animation animation)
        {
            this.CurrentAnimation = animation;
            this.UpdateFlags.AnimationUpdateRequired = true;
        }

        /// <summary>
        /// Speaks a message.
        /// </summary>
        /// <param name="message">The message to speak.</param>
        public override void Speak(ChatMessage message)
        {
            this.CurrentChatMessage = message;
            this.UpdateFlags.ChatUpdateRequired = true;
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
