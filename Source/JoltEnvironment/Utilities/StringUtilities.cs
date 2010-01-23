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
    /// Provides multiple ways of working with strings.
    /// </summary>
    public static class StringUtilities
    {
        #region Fields
        /// <summary>
        /// Valid characters able to be sent to runescape game.
        /// </summary>
        private static char[] validChars = {
		    '_', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 
		    'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 
		    't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', 
		    '3', '4', '5', '6', '7', '8', '9'
	    };
        #endregion Fields

        #region Methods
        /// <summary>
        /// Converts a given 64-bit integer to a string.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns a string.</returns>
        public static string LongToString(long value)
        {
            if (value <= 0L || value >= 0x5b5b57f8a98a5dd1L)
                return null;
            if (value % 37L == 0L)
                return null;

            int i = 0;
		    char[] ac = new char[12];
		    while (value != 0L) {
			    long l1 = value;
			    value /= 37L;
			    ac[11 - i++] = validChars[(int)(l1 - value * 37L)];
		    }
            return new string(ac, 12 - i, i);
        }

        /// <summary>
        /// Converts a given string to a 64-bit integer.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>Returns a 64-bit integer.</returns>
        public static long StringToLong(string s)
        {
            long l = 0L;
            for (int i = 0; i < s.Length && i < 12; i++)
            {
                char c = s[i];
                l *= 37L;
                if (c >= 'A' && c <= 'Z') l += (1 + c) - 65;
                else if (c >= 'a' && c <= 'z') l += (1 + c) - 97;
                else if (c >= '0' && c <= '9') l += (27 + c) - 48;
            }
            while (l % 37L == 0L && l != 0L) l /= 37L;
            return l;
        }

        /// <summary>
        /// Formats a given name for display.
        /// </summary>
        /// <param name="name">The name to format.</param>
        /// <returns>Returns a formatted string.</returns>
        public static string FormatForDisplay(string name)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name).Replace('_', ' ');
        }

        /// <summary>
        /// Formats a given name for protocol.
        /// </summary>
        /// <param name="name">The name to format.</param>
        /// <returns>Returns a formatted string.</returns>
        public static string FormatForProtocol(string name)
        {
            return name.Replace(' ', '_').ToLower();
        }
        #endregion Methods
    }
}
