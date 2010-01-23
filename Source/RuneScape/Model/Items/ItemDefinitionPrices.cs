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

namespace RuneScape.Model.Items
{
    /// <summary>
    /// Represents an item definition's stock prices.
    /// </summary>
    public class ItemDefinitionPrices
    {
        #region Properties
        /// <summary>
        /// The price's minimum amount.
        /// </summary>
        public int Minimum { get; private set; }
        /// <summary>
        /// The price's normal amount.
        /// </summary>
        public int Normal { get; private set; }
        /// <summary>
        /// The price's maximum amount.
        /// </summary>
        public int Maximum { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs an item definition's stock prices.
        /// </summary>
        /// <param name="prices">The stock prices.</param>
        public ItemDefinitionPrices(int[] prices)
        {
            this.Minimum = prices[0];
            this.Normal = prices[1];
            this.Maximum = prices[2];
        }
        #endregion Constructors
    }
}
