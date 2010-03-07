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

using RuneScape.Model.Npcs;
using RuneScape.Utilities;
using Character = RuneScape.Model.Characters.Character;

namespace RuneScape.Communication.Messages.Outgoing
{
    /// <summary>
    /// Composes an update which renders npc's within distance for the character.
    /// </summary>
    public class NpcRenderingPacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs the npc rendering packet.
        /// </summary>
        /// <param name="character"></param>
        public NpcRenderingPacketComposer(Character character, List<Npc> allNpcs)
        {
            SetOpcode(222);
            SetType(PacketType.Short);
            InitializeBit();

            // The update block. Any updates that are pending will be added to this block.
            GenericPacketComposer updateBlock = new GenericPacketComposer();

            AppendBits(8, character.LocalNpcs.Count);

            // A bin of npcs that have been removed during local npc updating.
            List<Npc> npcBin = new List<Npc>();
            character.LocalNpcs.ForEach((npc) =>
            {
                if (GameEngine.World.NpcManager.Spawns.Values.Contains(npc)
                    && npc.Location.WithinDistance(character.Location)
                    && !character.UpdateFlags.Teleporting)
                {
                    UpdateMovement(npc, this);

                    // Update the npc is required, since it is conditionally valid.
                    if (npc.UpdateFlags.UpdateRequired)
                    {
                        RenderMasks.AppendMasks(npc, updateBlock);
                    }
                }
                else
                {
                    // Remove this npc, as it doesn't meet local standards.
                    npcBin.Add(npc);

                    // Signify the client that this npc needs to be removed.
                    AppendBits(1, 1);
                    AppendBits(2, 3);
                }
            });

            // Remove all binned npcs.
            npcBin.ForEach((npc) => character.LocalNpcs.Remove(npc));

            // Search for npcs that may qualify for locality.
            foreach (Npc npc in allNpcs)
            {
                // We cannot have more than 255 npcs in the client simultaneously.
                if (character.LocalNpcs.Count >= 255)
                {
                    break;
                }

                if (character.LocalNpcs.Contains(npc)  || !npc.Location.WithinDistance(character.Location))
                {
                    continue;
                }

                character.LocalNpcs.Add(npc);
                AddNewNpc(character, npc, this);

                // Update the npc if required.
                if (npc.UpdateFlags.UpdateRequired)
                {
                    RenderMasks.AppendMasks(npc, updateBlock);
                }
            }

            // Finalize the composition.
            if (updateBlock.Position >= 3)
            {
                AppendBits(15, 32767);
            }

            FinishBit();

            if (updateBlock.Position > 0)
            {
                AppendBytes(updateBlock.SerializeBuffer());
            }
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Adds a new npc to the character's client.
        /// </summary>
        /// <param name="character">The client to add npc for.</param>
        /// <param name="npc">The npc to add.</param>
        /// <param name="mainPacket">The packet composer to append bits to.</param>
        public static void AddNewNpc(Character character, Npc npc, PacketComposer mainPacket)
        {
            mainPacket.AppendBits(15, npc.Index);
            mainPacket.AppendBits(14, npc.CacheIndex);
            mainPacket.AppendBits(1, npc.UpdateFlags.UpdateRequired ? 1 : 0);
            int xPos = npc.Location.X - character.Location.X;
            int yPos = npc.Location.Y - character.Location.Y;

            if (xPos < 0)
            {
                xPos += 32;
            }
            if (yPos < 0)
            {
                yPos += 32;
            }

            mainPacket.AppendBits(5, yPos);
            mainPacket.AppendBits(5, xPos);
            mainPacket.AppendBits(3, 0);
            mainPacket.AppendBits(1, 1);
        }

        /// <summary>
        /// Updates an npc's movement.
        /// </summary>
        /// <param name="npc">The npc to update movement for.</param>
        /// <param name="mainPacket">The packet composer to append bits to.</param>
        private static void UpdateMovement(Npc npc, PacketComposer mainPacket)
        {
            if (npc.Sprite == -1)
            {
                if (npc.UpdateFlags.UpdateRequired)
                {
                    mainPacket.AppendBits(1, 1);
                    mainPacket.AppendBits(2, 0);
                }
                else
                {
                    mainPacket.AppendBits(1, 0);
                }
            }
            else
            {
                mainPacket.AppendBits(1, 1);
                mainPacket.AppendBits(2, 1);
                mainPacket.AppendBits(3, DirectionUtilities.SCDirectionTranslation[npc.Sprite]);
                mainPacket.AppendBits(1, npc.UpdateFlags.UpdateRequired ? 1 : 0);
            }
        }
        #endregion Methods
    }
}
