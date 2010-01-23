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
    /// Priority of the log being printed.
    /// </summary>
    public enum LogPriority
    {
        /// <summary>
        /// Used for debugging purposes.
        /// 
        ///     Developers, debuggers, and/or testers who require 
        ///     all the information possible should keep the priority 
        ///     at this. It is encouraged to use this type if the log 
        ///     does not need to be within a release. 
        /// </summary>
        Debug,
        /// <summary>
        /// Used for informative purposes. 
        /// 
        ///     A typical information log that is used to give information
        ///     of no real importance to the user. Although, it is not
        ///     recommended that you keep it at this as it does show
        ///     connection attempts, database connectivity, etc which may
        ///     show unual readings.
        /// </summary>
        Info,
        /// <summary>
        /// Used for warning purposes.
        ///
        ///     A warning log that may be worth looking at, but usually not
        ///     harmful to the environment and/or runtime. Having a priority
        ///     above this is HIGHLY not recommended, warnings usually tell
        ///     you that something wrong could happen and informs you that
        ///     you may need to take action.
        /// </summary>
        Warn,
        /// <summary>
        /// Used for error purposes.
        /// 
        ///     A error log is something that be at threat to your
        ///     environment and/or runtime. A stacktrace is always printed
        ///     with an error log which should help you debug and fix the
        ///     problem.
        /// </summary>
        Error
    }
}
