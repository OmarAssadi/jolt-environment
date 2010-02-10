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

using JoltEnvironment.Utilities;
using RuneScape.Communication.Messages;
using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Items;
using RuneScape.Model.Items.Containers;
using RuneScape.Utilities;

namespace RuneScape.Model.Characters
{
    /// <summary>
    /// Provides character render masks.
    /// </summary>
    public static class RenderMasks
    {
        #region Methods
        /// <summary>
        /// Appends any needed update masks.
        /// </summary>
        /// <param name="character">The character to update.</param>
        /// <param name="updateBlock">The update block to add generated data to.</param>
        /// <param name="forceAppearance">Whether to force the appearance update.</param>
        public static void AppendMasks(Character character, GenericPacketComposer updateBlock, bool forceAppearance)
        {
            int mask = 0x0;

            if (character.UpdateFlags.Damage2UpdateRequired)
            {
                mask |= 0x200;
            }
            if (character.UpdateFlags.FaceToUpdateRequired)
            {
                mask |= 0x20;
            }
            if (character.UpdateFlags.GraphicsUpdateRequired)
            {
                mask |= 0x400;
            }
            if (character.UpdateFlags.ChatUpdateRequired)
            {
                mask |= 0x8;
            }
            if (character.UpdateFlags.AnimationUpdateRequired)
            {
                mask |= 0x1;
            }
            if (character.UpdateFlags.AppearanceUpdateRequired || forceAppearance)
            {
                mask |= 0x80;
            }
            if (character.UpdateFlags.DamageUpdateRequired)
            {
                mask |= 0x2;
            }

            if (mask >= 0x100)
            {
                mask |= 0x10;
                updateBlock.AppendByte((byte)(mask & 0xFF));
                updateBlock.AppendByte((byte)(mask >> 8));
            }
            else
            {
                updateBlock.AppendByte((byte)(mask & 0xFF));
            }

            if (character.UpdateFlags.Damage2UpdateRequired)
            {
                //AppendDamage2Update(p, updateBlock);
            }
            if (character.UpdateFlags.FaceToUpdateRequired)
            {
                updateBlock.AppendShort(character.UpdateFlags.FaceTo);
            }
            if (character.UpdateFlags.GraphicsUpdateRequired)
            {
                updateBlock.AppendShort(character.CurrentGraphic.Id);
                updateBlock.AppendIntSecondary(character.CurrentGraphic.Delay);
            }
            if (character.UpdateFlags.ChatUpdateRequired)
            {
                updateBlock.AppendShortA((short)character.CurrentChatMessage.Effects);
                updateBlock.AppendByteC((byte)character.ClientRights);
                byte[] chatStr = new byte[256];
                chatStr[0] = (byte)character.CurrentChatMessage.Text.Length;
                byte offset = (byte)(1 + ChatUtilities.EncryptChat(chatStr, 0, 1,
                    character.CurrentChatMessage.Text.Length,
                    Encoding.ASCII.GetBytes(character.CurrentChatMessage.Text)));
                updateBlock.AppendByteC(offset);
                updateBlock.AppendBytes(chatStr, 0, offset);
            }
            if (character.UpdateFlags.AnimationUpdateRequired)
            {
                updateBlock.AppendShort(character.CurrentAnimation.Id);
                updateBlock.AppendByteS(character.CurrentAnimation.Delay);
            }
            if (character.UpdateFlags.AppearanceUpdateRequired || forceAppearance)
            {
                AppendAppearanceUpdate(character, updateBlock);
            }
            if (character.UpdateFlags.DamageUpdateRequired)
            {
                //AppendDamageUpdate(p, updateBlock);
            }
        }

        /// <summary>
        /// Appends the character's current appearance.
        /// </summary>
        /// <param name="character">The character to append updates for.</param>
        /// <param name="updateBlock">The character's update block.</param>
        private static void AppendAppearanceUpdate(Character character, GenericPacketComposer updateBlock)
        {
            GenericPacketComposer properties = new GenericPacketComposer(75);

            // Gender.
            properties.AppendByte((byte)character.Appearance.Gender);

            // Head icons.
            properties.AppendByte(unchecked((byte)character.HeadIcon.DeathIcon));
            properties.AppendByte(unchecked((byte)character.HeadIcon.DeathIcon));

            if (!character.Appearance.IsNpc)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (character.Equipment[i] != null)
                    {
                        properties.AppendShort((short)(32768 + character.Equipment[i].Definition.EquipId));
                    }
                    else
                    {
                        properties.AppendByte((byte)0);//I'll check this out later
                    }
                }

                Item chest = character.Equipment[EquipmentSlot.Chest];
                Item shield = character.Equipment[EquipmentSlot.Shield];
                Item legs = character.Equipment[EquipmentSlot.Legs];
                Item hat = character.Equipment[EquipmentSlot.Hat];
                Item hands = character.Equipment[EquipmentSlot.Hands];
                Item feet = character.Equipment[EquipmentSlot.Feet];

                // Physical appearance.
                if (chest != null)
                {
                    properties.AppendShort((short)(32768 + chest.Definition.EquipId));
                }
                else
                {
                    properties.AppendShort((short)(0x100 + character.Appearance.Torso));
                }

                if (shield != null)
                {
                    properties.AppendShort((short)(32768 + shield.Definition.EquipId));
                }
                else
                {
                    properties.AppendByte((byte)0);
                }

                if (chest != null)
                {
                    if (!EquipmentItems.IsFullBody(chest.Id))
                    {
                        properties.AppendShort((short)(0x100 + character.Appearance.Arms));
                    }
                    else
                    {
                        properties.AppendByte((byte)0);
                    }
                }
                else
                {
                    properties.AppendShort((short)(0x100 + character.Appearance.Arms));
                }

                if (legs != null)
                {
                    properties.AppendShort((short)(32768 + legs.Definition.EquipId));
                }
                else
                {
                    properties.AppendShort((short)(0x100 + character.Appearance.Legs));
                }

                if (hat != null)
                {
                    if (!EquipmentItems.IsFullHat(hat.Id) && !EquipmentItems.IsFullMask(hat.Id))
                    {
                        properties.AppendShort((short)(0x100 + character.Appearance.Head));
                    }
                    else
                    {
                        properties.AppendByte((byte)0);
                    }
                }
                else
                {
                    properties.AppendShort((short)(0x100 + character.Appearance.Head));
                }

                if (hands != null)
                {
                    properties.AppendShort((short)(32768 + hands.Definition.EquipId));
                }
                else
                {
                    properties.AppendShort((short)(0x100 + character.Appearance.Wrist));
                }

                if (feet != null)
                {
                    properties.AppendShort((short)(32768 + feet.Definition.EquipId));
                }
                else
                {
                    properties.AppendShort((short)(0x100 + character.Appearance.Feet));
                }

                if (hat != null)
                {
                    if (!EquipmentItems.IsFullMask(hat.Id))
                    {
                        properties.AppendShort((short)(0x100 + character.Appearance.Beard));
                    }
                    else
                    {
                        properties.AppendByte((byte)0);
                    }
                }
                else
                {
                    properties.AppendShort((short)(0x100 + character.Appearance.Beard));
                }
            }
            else
            {
                properties.AppendShort(-1);
                properties.AppendShort(character.Appearance.NpcId);
            }

            // Hair colors.
            properties.AppendByte(character.Appearance.HairColor); //Hair Color
            properties.AppendByte(character.Appearance.TorsoColor); //Torso color
            properties.AppendByte(character.Appearance.LegColor); //Legs color
            properties.AppendByte(character.Appearance.FeetColor); //Feet color
            properties.AppendByte(character.Appearance.SkinColor); //Skin color

            // Emotions.
            properties.AppendShort(character.Equipment.StandAnimation); // stand
            properties.AppendShort(0x337); // stand turn
            properties.AppendShort(character.Equipment.WalkAnimation); // walk
            properties.AppendShort(0x334); // turn 180
            properties.AppendShort(0x335); // turn 90 cw
            properties.AppendShort(0x336); // turn 90 ccw
            properties.AppendShort(character.Equipment.RunAnimation); // run

            properties.AppendLong(character.LongName); // character's name
            properties.AppendByte((byte)character.Skills.CombatLevel); // combat level
            properties.AppendShort(0);
            updateBlock.AppendByte((byte)(properties.Position & 0xFF));
            updateBlock.AppendBytes(properties.SerializeBuffer());
        }
        #endregion Methods
    }
}
