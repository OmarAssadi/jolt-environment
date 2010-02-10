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

namespace RuneScape.Model.Characters
{
    /// <summary>
    /// Represents a character's own preferences.
    /// </summary>
    public class Preferences
    {
        #region Fields
        /// <summary>
        /// Collection of temporary attributes.
        /// 
        ///     <para>These attributes are not gauranteed to be there, 
        ///     and should proceed with caution when using.</para>
        /// </summary>
        private Dictionary<string, object> temporaryAttributes = new Dictionary<string, object>();
        /// <summary>
        /// Provides a lock for the attributes.
        /// </summary>
        private object attributesLock = new object();
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets a temporary attribute specified by the key.
        /// </summary>
        /// <param name="key">The attribute to obtain.</param>
        /// <returns>Returns the value of the specified key.</returns>
        public object this[string key]
        {
            get 
            {
                lock (this.attributesLock)
                {
                    if (this.temporaryAttributes.ContainsKey(key))
                    {
                        return this.temporaryAttributes[key];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            set 
            {
                lock (this.attributesLock)
                {
                    if (this.temporaryAttributes.ContainsKey(key))
                    {
                        this.temporaryAttributes[key] = value;
                    }

                }
            }
        }

        /// <summary>
        /// Gets or sets whether the character is using a hd client.
        /// </summary>
        public bool Hd { get; set; }
        /// <summary>
        /// Gets or sets whether the character's connected client is resized.
        /// </summary>
        public bool Resized { get; set; }
        /// <summary>
        /// Gets or sets whether the character is using a singe click mouse.
        /// </summary>
        public bool SingleMouse { get; set; }
        /// <summary>
        /// Gets or sets whether the character allows chat effects.
        /// </summary>
        public bool DisableChatEffects { get; set; }
        /// <summary>
        /// Gets or sets whether the character's private chat messages are split
        /// from the chat box.
        /// </summary>
        public bool SplitChat { get; set; }
        /// <summary>
        /// Gets or sets whether the character accepts aid.
        /// </summary>
        public bool AcceptAid { get; set; }

        /// <summary>
        /// Gets or sets the character's attack style.
        /// </summary>
        public byte AttackStyle { get; set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs the preferences with default values.
        /// </summary>
        public Preferences()
        {
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Attempts to add the specified key and value.
        /// </summary>
        /// <param name="key">The key to be added.</param>
        /// <param name="value">The key's value.</param>
        /// <returns>True if successfully added; false if not.</returns>
        public bool Add(string key, object value)
        {
            lock (this.attributesLock)
            {
                if (!this.temporaryAttributes.ContainsKey(key))
                {
                    this.temporaryAttributes.Add(key, value);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Attempts to remove the specified key.
        /// </summary>
        /// <param name="key">The key to be removed.</param>
        /// <returns>True if successfully removed; false if not.</returns>
        public bool Remove(string key)
        {
            lock (this.attributesLock)
            {
                return this.temporaryAttributes.Remove(key);
            }
        }
        #endregion Methods
    }
}
