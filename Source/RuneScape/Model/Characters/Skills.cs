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

namespace RuneScape.Model.Characters
{
    /// <summary>
    /// Represents the character's skills.
    /// </summary>
    public class Skills
    {
        #region Internal Classes
        /// <summary>
        /// Represents a single skill.
        /// </summary>
        public class Skill
        {
            /// <summary>
            /// The skill's level.
            /// </summary>
            public byte Level { get; set; }
            /// <summary>
            /// The skill's experience.
            /// </summary>
            public double Experience { get; set; }
        }
        #endregion Internal Classes

        #region Fields
        /// <summary>
        /// The number of skills in runescape as of revision 508.
        /// </summary>
        public const int Count = 24;
        /// <summary>
        /// The maxmimum amount of experience a character can have in a skill.
        /// </summary>
        public const int MaximumExperience = 200000000;
        /// <summary>
        /// Id's of the skills.
        /// </summary>
        public const int Attack = 0, Defence = 1, Strength = 2, Hitpoints = 3, Ranged = 4, Prayer = 5,
        Magic = 6, Cooking = 7, Woodcutting = 8, Fletching = 9, Fishing = 10, Firemaking = 11,
        Crafting = 12, Smithing = 13, Mining = 14, Herblore = 15, Agility = 16, Theiving = 17, Slayer = 18,
        Farming = 19, Runecrafting = 20, Constructing = 21, Hunter = 22, Summoning = 23;

        /// <summary>
        /// Instance of the character that owns the skill data.
        /// </summary>
        private Character character;

        /// <summary>
        /// The array of skills holding character's skill data.
        /// </summary>
        private Skill[] skill = new Skill[Skills.Count];
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the skills.
        /// </summary>
        public Skill this[int skill]
        {
            get { return this.skill[skill]; }
        }

        /// <summary>
        /// Gets the character's combat level.
        /// </summary>
        public int CombatLevel
        {
            get
            {
                int attack = LevelForExperience(Skills.Attack);
                int defence = LevelForExperience(Skills.Defence);
                int strength = LevelForExperience(Skills.Strength);
                int hp = LevelForExperience(Skills.Hitpoints);
                int prayer = LevelForExperience(Skills.Prayer);
                int ranged = LevelForExperience(Skills.Ranged);
                int magic = LevelForExperience(Skills.Magic);
                double melee = (attack + strength) * 0.325;
                double ranger = Math.Floor(ranged * 1.5) * 0.325;
                double mage = Math.Floor(magic * 1.5) * 0.325;
                double combatLevel = ((defence + hp + Math.Floor((double)(prayer / 2))) * 0.25) + 1;

                if (melee >= ranger && melee >= mage)
                {
                    combatLevel += melee;
                }
                else if (ranger >= melee && ranger >= mage)
                {
                    combatLevel += ranger;
                }
                else if (mage >= melee && mage >= ranger)
                {
                    combatLevel += mage;
                }

                return (int)(combatLevel + (LevelForExperience(Skills.Summoning) / 8));
            }
        }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Constructs the character's skills.
        /// </summary>
        public Skills(Character character)
        {
            this.character = character;

            for (int i = 0; i < Skills.Count; i++)
            {
                this.skill[i] = new Skill { Experience = 0, Level = 1 };
            }

            this.skill[Skills.Hitpoints].Level = 10;
            this.skill[Skills.Hitpoints].Experience = 1184;
        }
        #endregion Constructor

        #region Methods
        /// <summary>
        /// Gets the level for the amount of exp for the specified skill.
        /// </summary>
        /// <param name="skill">The skill</param>
        public byte LevelForExperience(int skill)
        {
            double points = 0;
            int output = 0;

            for (byte lvl = 1; lvl < 100; lvl++)
            {

                points += Math.Floor((double)lvl + 300.0 * Math.Pow(2.0, (double)lvl / 7.0));
                output = (int)Math.Floor(points / 4);
                if ((output - 1) >= this.skill[skill].Experience)
                {
                    return lvl;
                }
            }
            return 99;
        }

        /// <summary>
        /// Gets the experience minimum fr the specified level.
        /// </summary>
        /// <param name="level">The level to get experience minimum for.</param>
        /// <returns>Returns the experience minimum.</returns>
        public int ExperienceForLevel(int level)
        {
            double points = 0;
            double output = 0;

            for (int lvl = 1; lvl <= level; lvl++)
            {
                points += Math.Floor(lvl + 300.0 * Math.Pow(2.0, lvl / 7.0));
                if (lvl >= level)
                {
                    return (int)output;
                }
                output = Math.Floor((double)(points / 4));
            }
            return 0;
        }


        /// <summary>
        /// Adds experience to the character's current amount.
        /// </summary>
        /// <param name="skill">The skill to add experience to.</param>
        /// <param name="experience">The experience to add.</param>
        public void AddExperience(byte skill, double experience)
        {
            experience *= GameEngine.World.ExperienceRate;
            int oldLevel = LevelForExperience(skill);
            this[skill].Experience += experience;
            if (this[skill].Experience > Skills.MaximumExperience)
            {
                this[skill].Experience = Skills.MaximumExperience;
            }
            int newLevel = LevelForExperience(skill);
            int levelDiff = newLevel - oldLevel;
            if (newLevel > oldLevel)
            {
                this[skill].Level += (byte)levelDiff;
                // TODO: levelup
                //LevelUp.levelUp(player, skill);
            }
            character.Session.SendData(new StatsPacketComposer(skill, this[skill].Level, this[skill].Experience).Serialize());
            character.UpdateFlags.AppearanceUpdateRequired = true;
        }

        /// <summary>
        /// Increases the specified skill temporarily by one.
        /// </summary>
        /// <param name="skill">The skill to incease by one.</param>
        public void IncrementLevel(byte skill)
        {
            this.skill[skill].Level--;
            character.Session.SendData(new StatsPacketComposer(skill, this[skill].Level, this[skill].Experience).Serialize());
        }

        /// <summary>
        /// Decreases the specified skill temporarily by one.
        /// </summary>
        /// <param name="skill">The skill to decease by one.</param>
        public void DecrementLevel(byte skill)
        {
            this.skill[skill].Level++;
            character.Session.SendData(new StatsPacketComposer(skill, this[skill].Level, this[skill].Experience).Serialize());
        }

        /// <summary>
        /// Normalises the specified skill.
        /// </summary>
        /// <param name="skill">The skill to normalize.</param>
        public void NormaliseLevel(byte skill)
        {
            int norm = LevelForExperience(skill);
            if (this.skill[skill].Level > norm)
            {
                this.skill[skill].Level--;
                character.Session.SendData(new StatsPacketComposer(skill, this[skill].Level, this[skill].Experience).Serialize());
            }
            else if (this.skill[skill].Level < norm)
            {
                this.skill[skill].Level++;
                character.Session.SendData(new StatsPacketComposer(skill, this[skill].Level, this[skill].Experience).Serialize());
            }
        }

        /// <summary>
        /// Decreases the hitpoints by the specified decreasement.
        /// </summary>
        /// <param name="decreasement">The amount to decrease hitpoints by.</param>
        public void DamageHitpoints(byte decreasement)
        {
            this.skill[Skills.Hitpoints].Level -= decreasement;
            if (this.skill[Skills.Hitpoints].Level < 0)
            {
                this.skill[Skills.Hitpoints].Level = 0;
            }

            character.Session.SendData(new StatsPacketComposer(Skills.Hitpoints, 
                this.skill[Skills.Hitpoints].Level, 
                this.skill[Skills.Hitpoints].Experience).Serialize());
        }

        /// <summary>
        /// Increases the hitpoints by the specified increasement.
        /// </summary>
        /// <param name="decreasement">The amount to increase hitpoints by.</param>
        public void HealHitpoints(byte increasement)
        {
            this.skill[Skills.Hitpoints].Level += increasement;
            byte max = LevelForExperience(Skills.Hitpoints);
            if (this.skill[Skills.Hitpoints].Level > max)
            {
                this.skill[Skills.Hitpoints].Level = max;
            }

            character.Session.SendData(new StatsPacketComposer(Skills.Hitpoints,
                this.skill[Skills.Hitpoints].Level,
                this.skill[Skills.Hitpoints].Experience).Serialize());
        }

        /// <summary>
        /// Sets the specified skill's level.
        /// </summary>
        /// <param name="skill">The skill to change level for.</param>
        /// <param name="value">The value of the new level.</param>
        public void UpdateSkill(byte skill, byte value)
        {
            this.skill[skill].Level = value;
            character.Session.SendData(new StatsPacketComposer(skill, value, this[skill].Experience).Serialize());
            character.UpdateFlags.AppearanceUpdateRequired = true;
        }
        #endregion Methods
    }
}
