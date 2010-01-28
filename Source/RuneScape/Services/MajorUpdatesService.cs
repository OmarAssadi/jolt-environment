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
using System.Threading.Tasks;

using RuneScape.Model;
using RuneScape.Model.Characters;
using RuneScape.Model.Npcs;

namespace RuneScape.Services
{
    /// <summary>
    /// Serves as a major updates service.
    /// </summary>
    public class MajorUpdatesService
    {
        #region Fields
        /// <summary>
        /// The timer for the major updates.
        /// 
        ///     <para>Everytime this service is processed, this 
        ///     time will be checked to see if the time between 
        ///     each major update task has elapsed.</para>
        /// </summary>
        private DateTime majorUpdatesTime = DateTime.Now;
        /// <summary>
        /// The timer for the running healer.
        /// 
        ///     <para>Everytime this service is processed, this
        ///     time will be checked to see if the time between
        ///     each running healer task has elaspsed.</para>
        /// </summary>
        private DateTime runningHealerTime = DateTime.Now;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Processes the major updates.
        /// </summary>
        public void Process()
        {
            List<Npc> npcs = new List<Npc>(GameEngine.World.NpcManager.Spawns);
            List<Character> characters = new List<Character>(GameEngine.World.CharacterManager.Characters.Values);

            // Main tasks that will process character's route ticks, updates, and (any required) resets.
            if ((DateTime.Now - this.majorUpdatesTime).TotalMilliseconds >= 600)
            {
                this.majorUpdatesTime = DateTime.Now;

                // Renerate new random values.
                Npc.RegenerateRandom();

                Parallel.ForEach(characters, EntityManipulation.TickCharacter);
                Parallel.ForEach(npcs, EntityManipulation.TickNpc);
                Parallel.ForEach(characters, (c) => EntityManipulation.UpdateCharacter(c, characters, npcs));
                Parallel.ForEach(characters, EntityManipulation.ResetCharacter);
                Parallel.ForEach(npcs, EntityManipulation.ResetNpc);
            }

            // Heals character's running energy.
            if ((DateTime.Now - this.runningHealerTime).TotalMilliseconds >= 2000)
            {
                this.runningHealerTime = DateTime.Now;
                Parallel.ForEach(characters, EntityManipulation.RestoreRunEnergy);
            }

            npcs = null;
            characters = null;
        }
        #endregion Methods
    }
}
