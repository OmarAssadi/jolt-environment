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
    /// Represents a single chat message.
    /// </summary>
    public struct ChatMessage
    {
        #region Properties
        /// <summary>
        /// Gets the chat's text effects.
        /// </summary>
        public int Effects { get; private set; }
        /// <summary>
        /// Gets the chat's length.
        /// </summary>
        public int Length { get; private set; }
        /// <summary>
        /// Gets the chat's text.
        /// </summary>
        public string Text { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new chat message.
        /// </summary>
        /// <param name="effects">The chat text effects.</param>
        /// <param name="colors">The chat text length.</param>
        /// <param name="text">The chat text.</param>
        private ChatMessage(int effects, int length, string text) : this()
        {
            this.Effects = effects;
            this.Length = length;
            this.Text = text;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Creates a new chat message.
        /// </summary>
        /// <param name="effects">The chat text effects.</param>
        /// <param name="length">The chat text length.</param>
        /// <param name="text">The chat text.</param>
        /// <returns>Returns a RuneScape.Model.ChatMessage object.</returns>
        public static ChatMessage Create(int effects, int length, string text)
        {
            return new ChatMessage(effects, length, text);
        }
        #endregion Methods
    }
}
