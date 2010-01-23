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

namespace JoltEnvironment.Utilities
{
    /// <summary>
    /// Provides a variety of tools for working with bytes.
    /// </summary>
    public static class ByteUtilities
    {
        /// <summary>
        /// Trims a given array of 8-bit signed integers.
        /// </summary>
        /// <param name="bytes">The array giving bytes to trim.</param>
        /// <param name="offset">Start of trimming.</param>
        /// <param name="length">How much to trim from offset.</param>
        /// <returns>Returns a net array containing a solid byte array with no nulls blanks.</returns>
        public static byte[] Trim(this byte[] bytes, int offset, int length)
        {
            byte[] trimmedBytes = new byte[length - offset];
            for (int i = 0, j = offset; j < length; i++, j++)
            {
                trimmedBytes[i] = bytes[j];
            }
            return trimmedBytes;
        }
    }
}