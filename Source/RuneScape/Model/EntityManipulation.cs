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
using RuneScape.Model.Characters;
using RuneScape.Model.Npcs;

namespace RuneScape.Model
{
    /// <summary>
    /// Provides manipulation for entities.
    /// </summary>
    public class EntityManipulation
    {
        #region Methods
        /// <summary>
        /// The routine tick called to execute any side enquiries.
        /// </summary>
        /// <param name="character">The character to tick.</param>
        public static void TickCharacter(Character character)
        {
            character.Tick();
            character.WalkingQueue.ProcessNextMovement();
        }

        /// <summary>
        /// Updates the entities around the character (including itself).
        /// </summary>
        /// <param name="character">The character to update.</param>
        public static void UpdateCharacter(Character character, List<Character> allChars, List<Npc> allNpcs)
        {
            // Make sure there is no nulls.
            if (!character.Session.Connection.Connected)
            {
                GameEngine.World.CharacterManager.Unregister(character);
            }
            else
            {
                character.Session.SendData(new CharacterRenderingPacketComposer(
                    character, allChars).Serialize());
                character.Session.SendData(new NpcRenderingPacketComposer(
                    character, allNpcs).Serialize());
            }
        }

        /// <summary>
        /// Resets any flagged updates.
        /// </summary>
        /// <param name="character">The character to reset.</param>
        public static void ResetCharacter(Character character)
        {
            character.UpdateFlags.Clear();
            if (character.UpdateFlags.ClearFaceTo)
            {
                character.UpdateFlags.FaceToUpdateRequired = true;
                character.UpdateFlags.FaceTo = 0;
                character.UpdateFlags.ClearFaceTo = false;
            }
        }

        /// <summary>
        /// The routine tick called to execute any side enquiries.
        /// </summary>
        /// <param name="character">The npc to tick.</param>
        public static void TickNpc(Npc npc)
        {
            npc.Tick();
        }

        /// <summary>
        /// Resets any flagged updates.
        /// </summary>
        /// <param name="character">The character to reset.</param>
        public static void ResetNpc(Npc npc)
        {
            npc.UpdateFlags.Clear();
            if (npc.UpdateFlags.ClearFaceTo)
            {
                npc.UpdateFlags.FaceToUpdateRequired = true;
                npc.UpdateFlags.FaceTo = 0;
                npc.UpdateFlags.ClearFaceTo = true;
            }
        }

        /// <summary>
        /// Restores the character's running by 1.
        /// </summary>
        /// <param name="character"></param>
        public static void RestoreRunEnergy(Character character)
        {
            if (character.WalkingQueue.RunEnergy < 100)
            {
                character.Session.SendData(new RunEnergyPacketComposer(
                    ++character.WalkingQueue.RunEnergy).Serialize());
            }
        }

        #endregion Methods
    }
}
