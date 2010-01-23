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
using RuneScape.Utilities;

namespace RuneScape.Model.Characters
{
    /// <summary>
    /// Handles character walking.
    /// </summary>
    public class WalkingQueue
    {
        #region Fields
        /// <summary>
        /// The size of the points.
        /// </summary>
        private const int QueueCapacity = 50;

        /// <summary>
        /// The entity this queue belongs to.
        /// </summary>
        private Character character = null;

        /// <summary>
        /// The queue of movements.
        /// </summary>
        private MovementPoint[] points = new MovementPoint[WalkingQueue.QueueCapacity];

        /// <summary>
        /// The reading position in the points array.
        /// </summary>
        private byte readPosition = 0;
        /// <summary>
        /// The writing position in the points array.
        /// </summary>
        private byte writePosition = 0;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets or sets whether the character is running.
        /// </summary>
        public bool Running { get; set; }
        /// <summary>
        /// Gets or sets whether the character's running is toggled.
        /// </summary>
        public bool RunToggled { get; set; }
        /// <summary>
        /// Gets or sets the character's running energy.
        /// </summary>
        public byte RunEnergy { get; set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new walking queue class.
        /// </summary>
        /// <param name="character">The character to contruct the component for.</param>
        public WalkingQueue(Character character)
        {
            this.character = character;
            this.RunEnergy = 100;
            this.Running = false;
            this.RunToggled = false;

            for (int i = 0; i < WalkingQueue.QueueCapacity; i++)
            {
                points[i] = new MovementPoint(0, 0, -1);
            }
            Reset();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Reset the queue so no steps to walk.
        /// </summary>
        public void Reset()
        {
            // Set the point to the current location so the character doesn't move.
            points[0] = new MovementPoint(
                this.character.Location.LocalX, 
                this.character.Location.LocalY, -1);
            this.readPosition = writePosition = 1;
        }

        /// <summary>
        /// Adds a step to the walking queue.
        /// </summary>
        /// <param name="x">The local x coordinate to reach.</param>
        /// <param name="y">The local y coordinate to reach.</param>
        public void AddStep(int x, int y)
        {
            // Calculate differences.
            int diffX = x - this.points[this.writePosition - 1].X;
            int diffY = y - this.points[this.writePosition - 1].Y;

            // Calculate steps between differences.
            int max = Math.Max(Math.Abs(diffX), Math.Abs(diffY));
            for (int i = 0; i < max; i++)
            {
                /*
                 * This will keep lowering the differences untill 
                 * it reaches 0 (meaning no more steps to take).
                 */
                if (diffX < 0)
                    diffX++;
                else if (diffX > 0)
                    diffX--;
                if (diffY < 0)
                    diffY++;
                else if (diffY > 0)
                    diffY--;

                // Add the step to the (internal) queue.
                AddStepInternal(x - diffX, y - diffY);
            }
        }

        /// <summary>
        /// Adds a step to the queue internally.
        /// </summary>
        /// <param name="x">X coordinate of step.</param>
        /// <param name="y">Y coordinate of step.</param>
        private void AddStepInternal(int x, int y)
        {
            // Check to see if the current count of queues overflows the capacity limit.
            if (this.writePosition >= WalkingQueue.QueueCapacity)
            {
                /*
                 * The character most like is a bot, cause 
                 * normal characters cannot reach this limit.
                 */ 
                return;
            }

            // Get differences.
            int diffX = x - this.points[this.writePosition -1].X;
            int diffY = y - this.points[this.writePosition - 1].Y;

            // Caclulate direction from differences.
            int direction = DirectionUtilities.CalculateDirection(diffX, diffY);

            // We only need to enqueue the point if the direction is valid.
            if (direction > -1)
            {
                // Enqueue the calculated point.
                this.points[this.writePosition++] = new MovementPoint(x, y, direction);
            }
        }

        /// <summary>
        /// Processes the character's next movement.
        /// </summary>
        public void ProcessNextMovement()
        {
            // Sprites need to be reset incase the character is teleporting.
            this.character.Sprites.PrimarySprite = this.character.Sprites.SecondarySprite = -1;

            // The character is teleporting (not walking).
            if (this.character.UpdateFlags.TeleportLocation != null)
            {
                // Reset the walk queue because it is no longer valid.
                Reset();

                /*
                 * Needed to check if the character's current region 
                 * changed so we can send the required mapdata.
                 */
                Location lastRegion = this.character.Location;
                int diffX = lastRegion.RegionX - this.character.UpdateFlags.TeleportLocation.RegionX;
                int diffY = lastRegion.RegionY - this.character.UpdateFlags.TeleportLocation.RegionY;

                this.character.Location = this.character.UpdateFlags.TeleportLocation;
                this.character.UpdateFlags.TeleportLocation = null;
                this.character.UpdateFlags.Teleporting = true;

                /*
                 * When map data is loaded, the packet loads 4 regions. like so:
                 * ____
                 * |_|_|
                 * |_|_|
                 * 
                 * So when the character walks out or reaches an end of the 
                 * region, it has to be loaded or else the client would crash
                 * which is where in some cases, you see that black strip on
                 * the side(s) of the mini-map. This indicates that the region
                 * isn't loaded, and needs to be loaded.
                 */

                bool regionChanging = false;
                if ((diffX) >= 4 || (diffX) <= -4)
                {
                    regionChanging = true;
                }
                if ((diffY) >= 4 || (diffY) <= -4)
                {
                    regionChanging = true;
                }

                if (regionChanging)
                {
                    this.character.UpdateFlags.MapRegionChanging = true;
                }
            }
            else
            {
                Location oldLocation = this.character.Location;

                int walkDir = GetNextPoint();
                int runDir = -1;

                if (this.Running || this.RunToggled)
                {
                    if (this.RunEnergy > 0)
                    {
                        runDir = GetNextPoint();
                        if (runDir != -1)
                        {
                            this.character.Session.SendData(new RunEnergyPacketComposer(--this.RunEnergy).Serialize());
                        }
                    }
                    else
                    {
                        if (this.RunToggled)
                        {
                            this.RunToggled = false;
                            this.character.Session.SendData(new ConfigPacketComposer(173, 0).Serialize());
                        }
                        this.Running = false;
                    }
                }

                // Get the differences between current location and the last region.
                int diffX = this.character.UpdateFlags.LastRegion.RegionX - this.character.Location.RegionX;
                int diffY = this.character.UpdateFlags.LastRegion.RegionY - this.character.Location.RegionY;

                // Wehther the map is changing.
                bool regionChanging = false;

                // Decide if we need to send new map regions.
                if (diffX >= 4)
                {
                    regionChanging = true;
                }
                else if (diffX <= -4)
                {
                    regionChanging = true;
                }
                if (diffY >= 4)
                {
                    regionChanging = true;
                }
                else if (diffY <= -4)
                {
                    regionChanging = true;
                }

                // Flag region changing if needed.
                if (regionChanging)
                {
                    this.character.UpdateFlags.MapRegionChanging = true;

                    if (walkDir != -1)
                    {
                        this.readPosition--;
                    }
                    if (runDir != -1)
                    {
                        this.readPosition--;
                    }
                    walkDir = -1;
                    runDir = -1;

                    this.character.Location = oldLocation;
                }

                // Update character's sprites.
                this.character.Sprites.PrimarySprite = walkDir;
                this.character.Sprites.SecondarySprite = runDir;
            }
        }

        /// <summary>
        /// Gets the next point.
        /// </summary>
        /// <returns>Returns the point's direction.</returns>
        private int GetNextPoint()
        {
            // Nothing to read.
            if (this.readPosition == this.writePosition)
            {
                return -1;
            }

            int dir = points[this.readPosition++].Direction;

            /*
             * You cannot search though an array with a negative number, 
             * so we must check if the direction is higher than -1.
             */ 
            if (dir > -1)
            {
                int xdiff = DirectionUtilities.DeltaX[dir];
                int ydiff = DirectionUtilities.DeltaY[dir];

                this.character.Location = Location.Create(
                    character.Location.X + xdiff,
                    character.Location.Y + ydiff,
                    character.Location.Z);
            }
            return dir;
        }
        #endregion Methods
    }
}
