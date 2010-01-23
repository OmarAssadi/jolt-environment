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
        public short AttackStabBonus { get; private set; }
        /// <summary>
        /// Gets the item's attack-slash bonus.
        /// </summary>
        public short AttackSlashBonus { get; private set; }
        /// <summary>
        /// Gets the item's attack-crush bonus.
        /// </summary>
        public short AttackCrushBonus { get; private set; }
        /// <summary>
        /// Gets the item's attack-magic bonus.
        /// </summary>
        public short AttackMagicBonus { get; private set; }
        /// <summary>
        /// Gets the item's attack ranged-bonus.
        /// </summary>
        public short AttackRangedBonus { get; private set; }
        /// <summary>
        /// Gets the item's defence-stab bonus.
        /// </summary>
        public short DefenceStabBonus { get; private set; }
        /// <summary>
        /// Gets the item's defence-slash bonus.
        /// </summary>
        public short DefenceSlashBonus { get; private set; }
        /// <summary>
        /// Gets the item's defence-crush bonus.
        /// </summary>
        public short DefenceCrushBonus { get; private set; }
        /// <summary>
        /// Gets the item's defence-magic bonus.
        /// </summary>
        public short DefenceMagicBonus { get; private set; }
        /// <summary>
        /// Gets the item's defence-ragned bonus.
        /// </summary>
        public short DefenceRangedBonus { get; private set; }
        /// <summary>
        /// Gets the item's defence-summoning bonus.
        /// </summary>
        public short DefenceSummoningBonus { get; private set; }
        /// <summary>
        /// Gets the item's stregnth bonus.
        /// </summary>
        public short StrengthBonus { get; private set; }
        /// <summary>
        /// Gets the item's prayer bonus.
        /// </summary>
        public short PrayerBonus { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs an item definition's bonuses.
        /// </summary>
        /// <param name="bonuses">The bonuses.</param>
        public ItemDefinitionBonuses(short[] bonuses)
        {
            this.AttackStabBonus = bonuses[0];
            this.AttackSlashBonus = bonuses[1];
            this.AttackCrushBonus = bonuses[2];
            this.AttackMagicBonus = bonuses[3];
            this.AttackRangedBonus = bonuses[4];
            this.DefenceStabBonus = bonuses[5];
            this.DefenceSlashBonus = bonuses[6];
            this.DefenceCrushBonus = bonuses[7];
            this.DefenceMagicBonus = bonuses[8];
            this.DefenceRangedBonus = bonuses[9];
            this.DefenceSummoningBonus = bonuses[10];
            this.StrengthBonus = bonuses[11];
            this.PrayerBonus = bonuses[12];
        }
        #endregion Constructors
    }
}
