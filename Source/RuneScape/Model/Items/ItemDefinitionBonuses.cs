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
    /// Represents an item definition's bonuses.
    /// </summary>
    public class ItemDefinitionBonuses
    {
        #region Properties
        /// <summary>
        /// Gets the item's attack-stab bonus.
        /// </summary>
        public short AttackStab { get; private set; }
        /// <summary>
        /// Gets the item's attack-slash bonus.
        /// </summary>
        public short AttackSlash { get; private set; }
        /// <summary>
        /// Gets the item's attack-crush bonus.
        /// </summary>
        public short AttackCrush { get; private set; }
        /// <summary>
        /// Gets the item's attack-magic bonus.
        /// </summary>
        public short AttackMagic { get; private set; }
        /// <summary>
        /// Gets the item's attack ranged-bonus.
        /// </summary>
        public short AttackRanged { get; private set; }
        /// <summary>
        /// Gets the item's defence-stab bonus.
        /// </summary>
        public short DefenceStab { get; private set; }
        /// <summary>
        /// Gets the item's defence-slash bonus.
        /// </summary>
        public short DefenceSlash { get; private set; }
        /// <summary>
        /// Gets the item's defence-crush bonus.
        /// </summary>
        public short DefenceCrush { get; private set; }
        /// <summary>
        /// Gets the item's defence-magic bonus.
        /// </summary>
        public short DefenceMagic { get; private set; }
        /// <summary>
        /// Gets the item's defence-ragned bonus.
        /// </summary>
        public short DefenceRanged { get; private set; }
        /// <summary>
        /// Gets the item's defence-summoning bonus.
        /// </summary>
        public short DefenceSummoning { get; private set; }
        /// <summary>
        /// Gets the item's stregnth bonus.
        /// </summary>
        public short Strength { get; private set; }
        /// <summary>
        /// Gets the item's prayer bonus.
        /// </summary>
        public short Prayer { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs an item definition's bonuses.
        /// </summary>
        /// <param name="bonuses">The bonuses.</param>
        public ItemDefinitionBonuses(short[] bonuses)
        {
            this.AttackStab = bonuses[0];
            this.AttackSlash = bonuses[1];
            this.AttackCrush = bonuses[2];
            this.AttackMagic = bonuses[3];
            this.AttackRanged = bonuses[4];
            this.DefenceStab = bonuses[5];
            this.DefenceSlash = bonuses[6];
            this.DefenceCrush = bonuses[7];
            this.DefenceMagic = bonuses[8];
            this.DefenceRanged = bonuses[9];
            this.DefenceSummoning = bonuses[10];
            this.Strength = bonuses[11];
            this.Prayer = bonuses[12];
        }
        #endregion Constructors
    }
}
