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

using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Items;
using RuneScape.Model.Items.Containers;

namespace RuneScape.Model.Characters
{
    /// <summary>
    /// Represents a character's bonuses.
    /// </summary>
    public class Bonuses
    {
        #region Fields
        /// <summary>
        /// The character whom owns the bonuses.
        /// </summary>
        private Character character;

        /// <summary>
        /// Get amount of bonuses.
        /// </summary>
        public const int Size = 13;
        #endregion Fields

        #region Properties
        /// <summary>
        /// The total attack stab bonus calculated from the equipted items.
        /// </summary>
        public short AttackStab { get; private set; }
        /// <summary>
        /// The total attack slash bonus calculated from the equipted items.
        /// </summary>
        public short AttackSlash { get; private set; }
        /// <summary>
        /// The total attack crush bonus calculated from the equipted items.
        /// </summary>
        public short AttackCrush { get; private set; }
        /// <summary>
        /// The total attack magic bonus calculated from the equipted items.
        /// </summary>
        public short AttackMagic { get; private set; }
        /// <summary>
        /// The total attack range bonus calculated from the equipted items.
        /// </summary>
        public short AttackRanged { get; private set; }
        /// <summary>
        /// The total defence stab bonus calculated from the equipted items.
        /// </summary>
        public short DefenceStab { get; private set; }
        /// <summary>
        /// The total defence slash bonus calculated from the equipted items.
        /// </summary>
        public short DefenceSlash { get; private set; }
        /// <summary>
        /// The total defence crush bonus calculated from the equipted items.
        /// </summary>
        public short DefenceCrush { get; private set; }
        /// <summary>
        /// The total defence magic bonus calculated from the equipted items.
        /// </summary>
        public short DefenceMagic { get; private set; }
        /// <summary>
        /// The total defence range bonus calculated from the equipted items.
        /// </summary>
        public short DefenceRanged { get; private set; }
        /// <summary>
        /// The total defence summoning bonus calculated from the equipted items.
        /// </summary>
        public short DefenceSummoning { get; private set; }
        /// <summary>
        /// The total strength bonus calculated from the equipted items.
        /// </summary>
        public short Strength { get; private set; }
        /// <summary>
        /// The total prayer stab bonus calculated from the equipted items.
        /// </summary>
        public short Prayer { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new bonuses component.
        /// </summary>
        /// <param name="character">The character reference.</param>
        public Bonuses(Character character)
        {
            this.character = character;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Refreshes all the bonuses.
        /// </summary>
        public void Refresh(bool updateInterface)
        {
            Reset();

            for (int i = 0; i < EquipmentContainer.Size; i++)
            {
                Item item = this.character.Equipment[i];
                if (item != null)
                {
                    ItemDefinitionBonuses def = item.Definition.Bonuses;
                    this.AttackStab += def.AttackStab;
                    this.AttackSlash += def.AttackSlash;
                    this.AttackCrush += def.AttackCrush;
                    this.AttackMagic += def.AttackMagic;
                    this.AttackRanged += def.AttackRanged;
                    this.DefenceStab += def.DefenceStab;
                    this.DefenceSlash += def.DefenceSlash;
                    this.DefenceCrush += def.DefenceCrush;
                    this.DefenceMagic += def.DefenceMagic;
                    this.DefenceRanged += def.DefenceRanged;
                    this.DefenceSummoning += def.DefenceSummoning;
                    this.Strength += def.Strength;
                    this.Prayer += def.Prayer;
                }
            }

            if (updateInterface)
            {
                for (int i = 0; i < 12; i++)
                {
                    character.Session.SendData(new StringPacketComposer("Stab: " + (this.AttackStab >= 0 ? "+" : "-") + this.AttackStab, 667, 35).Serialize());
                    character.Session.SendData(new StringPacketComposer("Slash: " + (this.AttackSlash >= 0 ? "+" : "-") + this.AttackSlash, 667, 36).Serialize());
                    character.Session.SendData(new StringPacketComposer("Crush: " + (this.AttackCrush >= 0 ? "+" : "-") + this.AttackCrush, 667, 37).Serialize());
                    character.Session.SendData(new StringPacketComposer("Magic: " + (this.AttackMagic >= 0 ? "+" : "-") + this.AttackMagic, 667, 38).Serialize());
                    character.Session.SendData(new StringPacketComposer("Range: " + (this.AttackRanged >= 0 ? "+" : "-") + this.AttackRanged, 667, 39).Serialize());
                    character.Session.SendData(new StringPacketComposer("Stab: " + (this.DefenceStab >= 0 ? "+" : "-") + this.DefenceStab, 667, 40).Serialize());
                    character.Session.SendData(new StringPacketComposer("Slash: " + (this.DefenceSlash >= 0 ? "+" : "-") + this.DefenceSlash, 667, 41).Serialize());
                    character.Session.SendData(new StringPacketComposer("Crush: " + (this.DefenceCrush >= 0 ? "+" : "-") + this.DefenceCrush, 667, 42).Serialize());
                    character.Session.SendData(new StringPacketComposer("Magic: " + (this.DefenceMagic >= 0 ? "+" : "-") + this.DefenceMagic, 667, 43).Serialize());
                    character.Session.SendData(new StringPacketComposer("Range: " + (this.DefenceRanged >= 0 ? "+" : "-") + this.DefenceRanged, 667, 44).Serialize());
                    character.Session.SendData(new StringPacketComposer("Summoning: " + (this.DefenceSummoning >= 0 ? "+" : "-") + this.DefenceSummoning, 667, 45).Serialize());
                    character.Session.SendData(new StringPacketComposer("Strength: " + (this.Strength >= 0 ? "+" : "-") + this.Strength, 667, 47).Serialize());
                    character.Session.SendData(new StringPacketComposer("Prayer: " + (this.Prayer >= 0 ? "+" : "-") + this.Prayer, 667, 48).Serialize());
                }
            }
        }

        /// <summary>
        /// Resets all the bonuses to 0.
        /// </summary>
        public void Reset()
        {
            this.AttackStab = 0;
            this.AttackSlash = 0;
            this.AttackCrush = 0;
            this.AttackMagic = 0;
            this.AttackRanged = 0;
            this.DefenceStab = 0;
            this.DefenceSlash = 0;
            this.DefenceCrush = 0;
            this.DefenceMagic = 0;
            this.DefenceRanged = 0;
            this.DefenceSummoning = 0;
            this.Strength = 0;
            this.Prayer = 0;
        }
        #endregion Methods
    }
}
