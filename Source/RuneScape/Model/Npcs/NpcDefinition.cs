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

namespace RuneScape.Model.Npcs
{
    /// <summary>
    /// Represents a npc's definition.
    /// </summary>
    public partial class NpcDefinition
    {
        #region Properties
        /// <summary>
        /// Gets the npc definition's id.
        /// </summary>
        public short Id { get; private set; }
        /// <summary>
        /// Gets the npc definition's name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the npc definition's examination quote.
        /// </summary>
        public string Examine { get; private set; }
        /// <summary>
        /// Gets the npc definition's respawn rate.
        /// </summary>
        public byte RespawnRate { get; private set; }
        /// <summary>
        /// Gets the npc definition's combat level.
        /// </summary>
        public byte CombatLevel { get; private set; }
        /// <summary>
        /// Gets the npc definition's max hit.
        /// </summary>
        public byte MaxHit { get; private set; }
        /// <summary>
        /// Gets the npc definition's attack speed.
        /// </summary>
        public byte AttackSpeed { get; private set; }
        /// <summary>
        /// Gets the npc definition's attack animation.
        /// </summary>
        public short AttackAnimation { get; private set; }
        /// <summary>
        /// Gets the npc definition's defence animation.
        /// </summary>
        public short DefenceAnimation { get; private set; }
        /// <summary>
        /// Gets the npc definition's death animtion.
        /// </summary>
        public short DeathAnimation { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a npc definition.
        /// </summary>
        public NpcDefinition(short id, string name, string examine, 
            byte respawnRate, byte combatLevel, byte maxHit, 
            byte attackSpeed, short attackAnimation, short defenceAnimation, 
            short deathAnimation)
        {
            this.Id = id;
            this.Name = name;
            this.Examine = examine;
            this.RespawnRate = respawnRate;
            this.CombatLevel = combatLevel;
            this.MaxHit = maxHit;
            this.AttackSpeed = attackSpeed;
            this.AttackAnimation = attackAnimation;
            this.DefenceAnimation = defenceAnimation;
            this.DeathAnimation = deathAnimation;
        }
        #endregion Constructors
    }
}
