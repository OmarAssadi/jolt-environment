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
using System.Data;
using System.Linq;
using System.Text;

using JoltEnvironment.Storage.Sql;
using RuneScape.Utilities;

namespace RuneScape.Model.Npcs
{
    /// <summary>
    /// Represents a single non-player character in the world.
    /// </summary>
    public class Npc : Creature
    {
        #region Properties
        /// <summary>
        /// Gets the npc's cache index.
        /// </summary>
        public int CacheIndex { get; private set; }

        /// <summary>
        /// Gets or sets the npc's walking type.
        /// </summary>
        public WalkType WalkType { get; set; }
        /// <summary>
        /// The npc's original spawn location. This is the spot the npc will spawn every time.
        /// </summary>
        public Location OriginalLocation { get; private set; }
        /// <summary>
        /// The minimum range of walking this npc can go to.
        /// </summary>
        public Location MinimumRange { get; set; }
        /// <summary>
        /// The maximum range of walking this npc can go to.
        /// </summary>
        public Location MaximumRange { get; set; }
        /// <summary>
        /// Gets the npc's speakable messages.
        /// </summary>
        public string[] SpeakMessages { get; private set; }

        /// <summary>
        /// Gets the npc's current hitpoints.
        /// </summary>
        public byte HitPoints { get; set; }
        /// <summary>
        /// Gets the npc's current sprite.
        /// </summary>
        public sbyte Sprite { get; set; }

        /// <summary>
        /// Gets the definition(s) for this npc.
        /// </summary>
        public NpcDefinition Definition { get; set; }
        /// <summary>
        /// Gets the npc's update flags.
        /// </summary>
        public UpdateFlags UpdateFlags { get; private set; }

        /// <summary>
        /// A random number provider.
        /// </summary>
        private static Random random = new Random();
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new npc.
        /// </summary>
        /// <param name="indexId">The npc's client index id.</param>
        /// <param name="cacheIndexId">The npc's cache index id.</param>
        /// <param name="originalCoords">Original location at which the npc will always spawn at.</param>
        /// <param name="minRange">The min range at which the npc can walk to.</param>
        /// <param name="maxRange">The max range at which the npc can walk to.</param>
        /// <param name="type">The type of walking the npc does.</param>
        public Npc(int cacheIndexId, Location originalCoords, Location minRange, 
            Location maxRange, WalkType type, NpcDefinition definition, string[] messages) : base(originalCoords)
        {
            this.Sprite = -1;
            this.CacheIndex = cacheIndexId;
            this.OriginalLocation = originalCoords;
            this.MinimumRange = minRange;
            this.MaximumRange = maxRange;
            this.WalkType = type;

            this.Definition = definition;
            this.Name = this.Definition.Name;
            this.HitPoints = this.Definition.HitPoints;
            this.SpeakMessages = messages;

            this.UpdateFlags = new UpdateFlags();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Regenerates the random provider.
        /// </summary>
        public static void RegenerateRandom()
        {
            random = new Random();
        }

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
            this.UpdateFlags.SpeakUpdateRequired = true;
        }

        public void Speak(string message)
        {
            Speak(ChatMessage.Create(0, message.Length, message));
        }

        /// <summary>
        /// Executes routine procedures.
        /// </summary>
        public override void Tick()
        {
            this.Sprite = -1;

            if (this.WalkType == WalkType.Range && random.NextDouble() > 0.8)
            {
                int moveX = (int)(Math.Floor((random.NextDouble() * 3)) - 1);
                int moveY = (int)(Math.Floor((random.NextDouble() * 3)) - 1);
                short tgtX = (short)(this.Location.X + moveX);
                short tgtY = (short)(this.Location.Y + moveY);
                this.Sprite = DirectionUtilities.CalculateDirection(this.Location.X, this.Location.Y, tgtX, tgtY);

                if (tgtX > this.MaximumRange.X || tgtX < this.MinimumRange.X 
                    || tgtY > this.MaximumRange.Y || tgtY < this.MinimumRange.Y)
                {
                    this.Sprite = -1;
                }
                if (this.Sprite != -1)
                {
                    this.Sprite >>= 1;
                    this.Location = Location.Create(tgtX, tgtY, this.Location.Z);
                }
            }

            if (this.SpeakMessages != null && random.NextDouble() > 0.95)
            {
                int index = random.Next(0, this.SpeakMessages.Length);
                Speak(this.SpeakMessages[index]);
            }
        }
        #endregion Methods
    }
}
