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
    /// Represents a model's location in the game world.
    /// </summary>
    public class Location
    {
        #region Properties
        /// <summary>
        /// Gets or sets the model's X coordinate.
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Gets or sets the model's Y coordinate.
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Gets or sets the model's Z coordinate.
        /// </summary>
        public int Z { get; set; }

        /// <summary>
        /// Gets the model's regional X coordinate.
        /// </summary>
        public int RegionX { get { return (this.X >> 3); } }
        /// <summary>
        /// Gets the model's regional Y coordinate.
        /// </summary>
        public int RegionY { get { return (this.Y >> 3); } }

        /// <summary>
        /// Gets the model's local X coordinate.
        /// </summary>
        public int LocalX { get { return this.X - 8 * (this.RegionX - 6); } }
        /// <summary>
        /// Gets the model's local Y coordinate.
        /// </summary>
        public int LocalY { get { return this.Y - 8 * (this.RegionY - 6); } }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new location instance.
        /// </summary>
        /// <param name="x">The instance's X coordinate.</param>
        /// <param name="y">The instance's Y coordinate.</param>
        /// <param name="z">The instance's Z coordinate.</param>
        private Location(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        #endregion Contructors

        #region Methods
        /// <summary>
        /// Creates a location instance.
        /// </summary>
        /// <param name="x">The instance's X coordinate.</param>
        /// <param name="y">The instance's Y coordinate.</param>
        /// <param name="z">The instance's Z coordinate.</param>
        /// <returns>Returns a RuneScape.Model.Location object.</returns>
        public static Location Create(int x, int y, int z)
        {
            return new Location(x, y, z);
        }
        
        /// <summary>
        /// Gets the local x range dependant on the given location.
        /// </summary>
        /// <param name="location">The location used to find range.</param>
        /// <returns>Returns an int.</returns>
        public int GetLocalX(Location location)
        {
            return this.X - 8 * (location.RegionX - 6);
        }

        /// <summary>
        /// Gets the local y range dependant on the given location.
        /// </summary>
        /// <param name="location">The location used to find range.</param>
        /// <returns>Returns an int.</returns>
        public int GetLocalY(Location location)
        {
            return this.Y - 8 * (location.RegionY - 6);
        }

        /// <summary>
        /// Copies the current location instance.
        /// </summary>
        /// <returns>Returns a RuneScape.Model.Local object.</returns>
        public Location Copy()
        {
            return new Location(this.X, this.Y, this.Z);
        }

        /// <summary>
        /// Gets the location's hash code.
        /// </summary>
        /// <returns>Returns an integer.</returns>
        public override int GetHashCode()
        {
            return this.Z << 30 | this.X << 15 | this.Y;
        }

        /// <summary>
        /// Compares two RuneScape.Model.Location objects to see if their coordinates match.
        /// </summary>
        /// <param name="obj">The location object.</param>
        /// <returns>Returns true if both objects have the same coordiantes; false if they dont.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Location))
                return false;
            Location location = (Location)obj;
            return location.X == this.X 
                && location.Y == this.Y 
                && location.Z == this.Z;
        }

        /// <summary>
        /// Provides a string that shows the location's coordinates.
        /// </summary>
        /// <returns>Returns a string containing locations coordinates.</returns>
        public override string ToString()
        {
            return "x=" + this.X + ",y=" + this.Y + ",z=" + this.Z;
        }

        /// <summary>
        /// Measures the distance between two locations.
        /// </summary>
        /// <param name="other">The location to compare with.</param>
        /// <param name="distance">The instance that the distance has to be within</param>
        /// <returns>Returns true if the given location is within distance of the given distance range; false if not.</returns>
        public bool WithinDistance(Location other, int distance)
        {
            if (other.Z != this.Z)
            {
                return false;
            }

            int deltaX = other.X - this.X, deltaY = other.Y - this.Y;
            return (deltaX <= (distance) 
                && deltaX >= (0 - distance - 1) 
                && deltaY <= (distance) 
                && deltaY >= (0 - distance - 1));
        }

        /// <summary>
        /// Measures the distance between two locations.
        /// </summary>
        /// <param name="other">The location to compare with.</param>
        /// <returns>Returns true if the given location is within distance of the 16 distance range; false if not.</returns>
        public bool WithinDistance(Location other)
        {
            if (other.Z != this.Z)
            {
                return false;
            }
            int deltaX = other.X - this.X, deltaY = other.Y - this.Y;
            return deltaX <= 14 && deltaX >= -15 && deltaY <= 14 && deltaY >= -15;
        }

        /// <summary>
        /// Measures the distance between two locations.
        /// </summary>
        /// <param name="other">The location to compare with.</param>
        /// <returns>Returns true if the given location is within distance of the 3 distance range; false if not.</returns>
        public bool WithinInteractionDistance(Location other)
        {
            return WithinDistance(other, 3);
        }

        /// <summary>
        /// Gets the absolute distance between the specified location.
        /// </summary>
        /// <param name="other">The location to compare with.</param>
        /// <returns>Returns a double holding the value of the distance.</returns>
        public double GetDistance(Location other)
        {
            int xDiff = this.X - other.X;
            int yDiff = this.Y - other.Y;
            return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        }
        #endregion Methods
    }
}
