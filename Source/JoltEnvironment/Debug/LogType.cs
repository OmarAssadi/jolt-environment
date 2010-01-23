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

namespace JoltEnvironment.Debug
{
    /// <summary>
    /// Type of log to be shown.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// Used for less 'experienced' users. Displays a limied 
        /// amount of data, but is more user-friendly and readable.
        /// </summary>
        Standard,
        /// <summary>
        /// Used for developers, debuggers, and experienced users.
        /// Displays an advanced debug of each log printed, but is
        /// less user-friendly.
        /// </summary>
        Advanced
    }
}
