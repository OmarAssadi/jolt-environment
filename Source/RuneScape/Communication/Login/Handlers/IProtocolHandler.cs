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

namespace RuneScape.Communication.Login.Handlers
{
    /// <summary>
    /// Defines a interface for login protocol handlers.
    /// </summary>
    public interface IProtocolHandler
    {
        /// <summary>
        /// Handles the specified login request.
        /// 
        ///     <para>The login request contains information on how the handler will act.</para>
        /// </summary>
        /// <param name="request">The login request instance.</param>
        void Handle(LoginRequest request);
    }
}
