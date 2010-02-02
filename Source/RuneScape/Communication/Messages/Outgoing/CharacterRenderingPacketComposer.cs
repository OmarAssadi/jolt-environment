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

using RuneScape.Model.Characters;

namespace RuneScape.Communication.Messages.Outgoing
{
    /// <summary>
    /// Composes an update which renders the character(s) to the player's client.
    /// </summary>
    public class CharacterRenderingPacketComposer : PacketComposer
    {
        #region Constructors
        /// <summary>
        /// Constructs the character rendering packet.
        /// </summary>
        /// <param name="character">The character to update.</param>
        /// <param name="allChars">A collection of characters that are checked against a few conditions.</param>
        public CharacterRenderingPacketComposer(Character character, List<Character> allChars)
        {
            if (character.UpdateFlags.MapRegionChanging)
            {
                Frames.SendMapRegion(character);
            }

            SetOpcode(216);
            SetType(PacketType.Short);
            InitializeBit();

            // The update block. Any updates that are pending will be added to this block.
            GenericPacketComposer updateBlock = new GenericPacketComposer();

            // Update this character's movement.
            UpdateThisMovement(character, this);

            if (character.UpdateFlags.UpdateRequired)
            {
                /*
                 * No need to force appeal since appearance hasn't changed 
                 * (unless appearance update is requied in update flags).
                 */
                RenderMasks.AppendMasks(character, updateBlock, false);
            }
            AppendBits(8, character.LocalCharacters.Count);

            // Characters that have been binned during local characters updating.
            List<Character> charBin = new List<Character>();
            character.LocalCharacters.ForEach((otherCharacter) =>
            {
                if (GameEngine.World.CharacterManager.Contains(otherCharacter)
                    && !otherCharacter.UpdateFlags.Teleporting
                    && otherCharacter.Location.WithinDistance(character.Location)
                    && otherCharacter.Session.Connection.Connected)
                {
                    UpdateMovement(otherCharacter, this);

                    if (otherCharacter.UpdateFlags.UpdateRequired)
                    {
                        RenderMasks.AppendMasks(otherCharacter, updateBlock, false);
                    }
                }
                else
                {
                    // Remove this character, as it doesn't meet local standards.
                    charBin.Add(otherCharacter);

                    // Signify the client that this character needs to be removed.
                    AppendBits(1, 1);
                    AppendBits(2, 3);

                }
            });

            // Remove binned characters.
            charBin.ForEach((c) => character.LocalCharacters.Remove(c));

            foreach (Character otherCharacter in allChars)
            {
                /*
                 * If there is no more space in the local area for the main character to 
                 * see, then we will not show those other characters until some leave.
                 */
                if (character.LocalCharacters.Count > 255)
                {
                    break;
                }

                if (otherCharacter == character
                    || character.LocalCharacters.Contains(otherCharacter)
                    || !otherCharacter.Location.WithinDistance(character.Location))
                {
                    continue;
                }

                character.LocalCharacters.Add(otherCharacter);
                AddNewCharacter(character, otherCharacter, this);
                RenderMasks.AppendMasks(otherCharacter, updateBlock, true);
            }

            /*
             * If there are masks that need updating, we will append a
             * special key identifying the start of the update masks, 
             * and append the serialized update block.
             */
            if (!updateBlock.Empty)
            {
                AppendBits(11, 2047);

                // Finish bit access so we can write bytes normally.
                FinishBit();

                AppendBytes(updateBlock.SerializeBuffer());
            }
            else
            {
                // Nothing else to write.
                FinishBit();
            }
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Adds a new character to the current character's client.
        /// </summary>
        /// <param name="character">The current character.</param>
        /// <param name="other">The character to add.</param>
        /// <param name="mainPacket">The packet to write the data to.</param>
        private static void AddNewCharacter(Character character, Character other, PacketComposer mainPacket)
        {
            mainPacket.AppendBits(11, other.Index);
            int xPos = other.Location.X - character.Location.X;
            int yPos = other.Location.Y - character.Location.Y;

            if (xPos < 0)
            {
                xPos += 32;
            }
            if (yPos < 0)
            {
                yPos += 32;
            }

            mainPacket.AppendBits(5, xPos);
            mainPacket.AppendBits(1, 1);
            mainPacket.AppendBits(3, 1);
            mainPacket.AppendBits(1, 1);
            mainPacket.AppendBits(5, yPos);
        }

        /// <summary>
        /// Updates the "other" character (The character from local players list).
        /// </summary>
        /// <param name="character">The "other" character to update.</param>
        /// <param name="mainPacket">The main packet for appending required data.</param>
        private static void UpdateMovement(Character character, PacketComposer mainPacket)
        {
            if (character.Sprites.PrimarySprite == -1)
            {
                if (character.UpdateFlags.UpdateRequired)
                {
                    mainPacket.AppendBits(1, 1);
                    mainPacket.AppendBits(2, 0);
                }
                else
                {
                    mainPacket.AppendBits(1, 0);
                }
            }
            else if (character.Sprites.SecondarySprite == -1)
            {
                mainPacket.AppendBits(1, 1);
                mainPacket.AppendBits(2, 1);
                mainPacket.AppendBits(3, character.Sprites.PrimarySprite);
                mainPacket.AppendBits(1, character.UpdateFlags.UpdateRequired ? 1 : 0);
            }
            else
            {
                mainPacket.AppendBits(1, 1);
                mainPacket.AppendBits(2, 2);
                mainPacket.AppendBits(3, character.Sprites.PrimarySprite);
                mainPacket.AppendBits(3, character.Sprites.SecondarySprite);
                mainPacket.AppendBits(1, character.UpdateFlags.UpdateRequired ? 1 : 0);
            }
        }

        /// <summary>
        /// Updates "this" character (The character based upon for the local players list).
        /// </summary>
        /// <param name="character">The character to update.</param>
        /// <param name="mainPacket">The main packet for appending required data.</param>
        private static void UpdateThisMovement(Character character, PacketComposer mainPacket)
        {
            if (character.UpdateFlags.Teleporting)
            {
                mainPacket.AppendBits(1, 1);
                mainPacket.AppendBits(2, 3);
                mainPacket.AppendBits(7, character.Location.GetLocalX(character.UpdateFlags.LastRegion));
                mainPacket.AppendBits(1, 1);
                mainPacket.AppendBits(2, character.Location.Z);
                mainPacket.AppendBits(1, character.UpdateFlags.UpdateRequired ? 1 : 0);
                mainPacket.AppendBits(7, character.Location.GetLocalY(character.UpdateFlags.LastRegion));
            }
            else
            {
                if (character.Sprites.PrimarySprite == -1)
                {
                    mainPacket.AppendBits(1, character.UpdateFlags.UpdateRequired ? 1 : 0);
                    if (character.UpdateFlags.UpdateRequired)
                    {
                        mainPacket.AppendBits(2, 0);
                    }
                }
                else
                {
                    if (character.Sprites.SecondarySprite != -1)
                    {
                        mainPacket.AppendBits(1, 1);
                        mainPacket.AppendBits(2, 2);
                        mainPacket.AppendBits(3, character.Sprites.PrimarySprite);
                        mainPacket.AppendBits(3, character.Sprites.SecondarySprite);
                        mainPacket.AppendBits(1, character.UpdateFlags.UpdateRequired ? 1 : 0);
                    }
                    else
                    {
                        mainPacket.AppendBits(1, 1);
                        mainPacket.AppendBits(2, 1);
                        mainPacket.AppendBits(3, character.Sprites.PrimarySprite);
                        mainPacket.AppendBits(1, character.UpdateFlags.UpdateRequired ? 1 : 0);
                    }
                }
            }
        }
        #endregion Methods
    }
}
