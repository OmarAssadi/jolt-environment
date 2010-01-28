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

namespace RuneScape.Model.Npcs
{
    /// <summary>
    /// Provides npc render masks.
    /// </summary>
    public class RenderMasks
    {
        #region Methods
        /// <summary>
        /// Appends any needed update masks.
        /// </summary>
        /// <param name="npc">The npc to update.</param>
        /// <param name="updateBlock">The update block to add generated data to.</param>
        public static void AppendMasks(Npc npc, PacketComposer updateBlock)
        {
            int mask = 0x0;

            if (npc.UpdateFlags.FaceToUpdateRequired)
            {
                mask |= 0x10;
            }
            if (npc.UpdateFlags.SpeakUpdateRequired)
            {
                mask |= 0x40;
            }
            if (npc.UpdateFlags.AnimationUpdateRequired)
            {
                mask |= 0x1;
            }
            if (npc.UpdateFlags.GraphicsUpdateRequired)
            {
                mask |= 0x2;
            }
            if (npc.UpdateFlags.Damage2UpdateRequired)
            {
                mask |= 0x20;
            }
            if (npc.UpdateFlags.DamageUpdateRequired)
            {
                mask |= 0x4;
            }

            updateBlock.AppendByte((byte) mask);
            if (npc.UpdateFlags.FaceToUpdateRequired)
            {
                updateBlock.AppendShort(npc.UpdateFlags.FaceTo);
            }
            if (npc.UpdateFlags.SpeakUpdateRequired)
            {
                updateBlock.AppendString(npc.CurrentChatMessage.Text);
            }
            if (npc.UpdateFlags.AnimationUpdateRequired)
            {
                updateBlock.AppendShortA(npc.CurrentAnimation.Id);
                updateBlock.AppendByte(npc.CurrentAnimation.Delay);
            }
            if (npc.UpdateFlags.GraphicsUpdateRequired)
            {
                updateBlock.AppendShortA(npc.CurrentGraphic.Id);
                updateBlock.AppendIntTertiary(npc.CurrentGraphic.Delay);
            }
            if (npc.UpdateFlags.Damage2UpdateRequired)
            {
                //AppendHit2Update(npc, updateBlock);
            }
            if (npc.UpdateFlags.DamageUpdateRequired)
            {
                //AppendHit1Update(npc, updateBlock);
            }
        }
        #endregion Methods
    }
}
