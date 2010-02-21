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

using RuneScape.Model.Characters;

namespace RuneScape.Model.Items.Containers
{
    /// <summary>
    /// A container for character inventories.
    /// </summary>
    public class BankContainer : Container
    {
        #region Fields
        /// <summary>
        /// The size of the bank container..
        /// 
        ///     <remarks>This value cannot be changed, because 
        ///     this is the maximum amount required for the 
        ///     runescape client.</remarks>
        /// </summary>
        public const int Size = 200;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a new bank container.
        /// </summary>
        public BankContainer(Character character)
            : base(BankContainer.Size, ContainerType.Standard, character)
        {
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Refreshes the bank.
        /// </summary>
        public override void Refresh()
        {
        }
        #endregion Methods
    }
}
