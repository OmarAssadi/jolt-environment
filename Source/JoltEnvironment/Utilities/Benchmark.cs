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
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoltEnvironment.Utilities
{
    /// <summary>
    /// Represents a benchmark for a piece of code.
    /// </summary>
    public class Benchmark
    {
        #region Fields
        /// <summary>
        /// The watch.
        /// </summary>
        private Stopwatch watch = new Stopwatch();
        /// <summary>
        /// The code to benchmark.
        /// </summary>
        private Action action;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a new benchmark.
        /// </summary>
        /// <param name="action">The code to benchmark.</param>
        public Benchmark(Action action)
        {
            this.action = action;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Processes the code.
        /// </summary>
        public void Process()
        {
            watch.Reset();
            watch.Start();
            action.Invoke();
            watch.Stop();
        }

        /// <summary>
        /// Gets the elapsed time in milliseconds.
        /// </summary>
        /// <returns>Returns the time elapsed.</returns>
        public double GetTime()
        {
            return watch.Elapsed.TotalMilliseconds;
        }

        /// <summary>
        /// Gets the elapsed time in a formated milliseconds count.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>Returns a string containing the time elapsed.</returns>
        public string GetTime(string format)
        {
            return watch.Elapsed.TotalMilliseconds.ToString(format);
        }

        /// <summary>
        /// Gets the elapsed time in milliseconds count.
        /// </summary>
        /// <returns>Returns a string containing the time elapsed.</returns>
        public override string ToString()
        {
            return watch.Elapsed.TotalMilliseconds.ToString();
        }
        #endregion Methods
    }
}
