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
using RuneScape.Utilities;

namespace RuneScape.Model.Npcs
{
    /// <summary>
    /// Handles npc walking.
    /// </summary>
    public class WalkingQueue
    {
        #region Fields
        /// <summary>
        /// The size of the points.
        /// </summary>
        private const int Capacity = 50;

        /// <summary>
        /// The entity this queue belongs to.
        /// </summary>
        private Npc npc = null;

        /// <summary>
        /// The queue of movements.
        /// </summary>
        private MovementPoint[] points = new MovementPoint[WalkingQueue.Capacity];

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
        /// Whether the queue is empty.
        /// </summary>
        public bool Empty
        {
            get
            {
                return this.readPosition == this.writePosition;
            }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new walking queue class.
        /// </summary>
        public WalkingQueue(Npc npc)
        {
            this.npc = npc;
            for (int i = 0; i < WalkingQueue.Capacity; i++)
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
                this.npc.Location.X,
                this.npc.Location.Y, -1);
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
                AddStepInternal((short)(x - diffX), (short)(y - diffY));
            }
        }

        /// <summary>
        /// Adds a step to the queue internally.
        /// </summary>
        /// <param name="x">X coordinate of step.</param>
        /// <param name="y">Y coordinate of step.</param>
        private void AddStepInternal(short x, short y)
        {
            // Check to see if the current count of queues overflows the capacity limit.
            if (this.writePosition >= WalkingQueue.Capacity)
            {
                /*
                 * The character most likely is a bot, cause 
                 * normal characters cannot reach this limit.
                 */
                return;
            }

            // Caclulate direction from differences.
            sbyte direction = DirectionUtilities.CalculateDirection(this.npc.Location.X, this.npc.Location.Y, x, y);

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
            this.npc.Sprite = GetNextPoint();
        }

        /// <summary>
        /// Gets the next point.
        /// </summary>
        /// <returns>Returns the point's direction.</returns>
        private sbyte GetNextPoint()
        {
            // Nothing to read.
            if (this.readPosition == this.writePosition)
            {
                return -1;
            }

            MovementPoint mp = points[this.readPosition++];
            sbyte dir = mp.Direction;

            /*
             * You cannot search though an array with a negative number, 
             * so we must check if the direction is higher than -1.
             */
            if (dir != -1)
            {
                dir >>= 1;
                this.npc.Location = Location.Create(mp.X, mp.Y, this.npc.Location.Z);
            }
            return dir;
        }
        #endregion Methods
    }
}
