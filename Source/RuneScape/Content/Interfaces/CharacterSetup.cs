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

using RuneScape.Communication.Messages;
using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Characters;

namespace RuneScape.Content.Interfaces
{
    /// <summary>
    /// Represents the character setup options 
    /// </summary>
    public class CharacterSetup : IInterfaceHandler
    {
        #region Fields
        /// <summary>
        /// The id of this interface.
        /// </summary>
        public const int InterfaceId = 771;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Handles the character setup options.
        /// </summary>
        /// <param name="character">The character to handle for.</param>
        /// <param name="packetId">The packet id of the button.</param>
        /// <param name="buttonId">The button to handle.</param>
        /// <param name="buttonId2">The secondary button to handle.</param>
        public void HandleButton(Character character, int packetId, int buttonId, int buttonId2)
        {
            switch (buttonId)
            {
                #region Mouse options
                // 2 click mouse option.
                case 37:
                    {
                        character.Preferences.SingleMouse = false;
                        character.Session.SendData(new ConfigPacketComposer(170, 0).Serialize());
                        break;
                    }
                // 1 click mouse option. 
                case 40:
                    {
                        character.Preferences.SingleMouse = true;
                        character.Session.SendData(new ConfigPacketComposer(170, 1).Serialize());
                        break;
                    }
                #endregion Mouse options

                #region Gender options
                // Male option. 
                case 49:
                    {
                        ChangeToMale(character);
                        break;
                    }
                // Female option.
                case 52:
                    {
                        ChangeToFemale(character);
                        break;
                    }
                #endregion Gender Options

                #region Close
                // Confirm close.
                case 362:
                    {
                        character.UpdateFlags.AppearanceUpdateRequired = true;
                        Frames.SendCloseInterface(character);
                        character.Session.SendData(new MinimapViewPacketComposer(MapViewType.Normal).Serialize());
                        break;
                    }
                #endregion Close

                #region Skin Color
                // Very pale.
                case 158:
                    {
                        character.Appearance.SkinColor = 7;
                        break;
                    }
                // Pale.
                case 151:
                    {
                        character.Appearance.SkinColor = 0;
                        break;
                    }
                // Fair.
                case 152:
                    {
                        character.Appearance.SkinColor = 1;
                        break;
                    }
                // Tanned.
                case 153:
                    {
                        character.Appearance.SkinColor = 2;
                        break;
                    }
                // Olive.
                case 154:
                    {
                        character.Appearance.SkinColor = 3;
                        break;
                    }
                // Light brown.
                case 155:
                    {
                        character.Appearance.SkinColor = 4;
                        break;
                    }
                // Brown.
                case 156:
                    {
                        character.Appearance.SkinColor = 5;
                        break;
                    }
                // Dark brown.
                case 157:
                    {
                        character.Appearance.SkinColor = 6;
                        break;
                    }
                #endregion Skin Color

                #region Hair
                // Hair decrement.
                case 92:
                    {
                        character.Appearance.Head--;
                        if (character.Appearance.Gender == Gender.Male)
                        {
                            if (character.Appearance.Head < 0)
                                character.Appearance.Head = 97;
                            if (character.Appearance.Head > 8 && character.Appearance.Head < 91)
                                character.Appearance.Head = 8;
                            if (character.Appearance.Head > 97 && character.Appearance.Head < 24)
                                character.Appearance.Head = 97;
                        }
                        else
                        {
                            if (character.Appearance.Head < 45)
                                character.Appearance.Head = 146;
                            if (character.Appearance.Head > 54 && character.Appearance.Head < 135)
                                character.Appearance.Head = 54;
                            if (character.Appearance.Head > 146 && character.Appearance.Head < 269)
                                character.Appearance.Head = 146;
                            if (character.Appearance.Head > 280 && character.Appearance.Head < 1000)
                                character.Appearance.Head = 45;
                        }
                        break;
                    }
                // Hair increment.
                case 93:
                    {
                        character.Appearance.Head++;
                        if (character.Appearance.Gender == Gender.Male)
                        {
                            if (character.Appearance.Head > 8 && character.Appearance.Head < 91)
                                character.Appearance.Head = 91;
                            if (character.Appearance.Head > 97 && character.Appearance.Head < 246)
                                character.Appearance.Head = 0;
                        }
                        else
                        {
                            if (character.Appearance.Head > 54 && character.Appearance.Head < 135)
                                character.Appearance.Head = 135;
                            if (character.Appearance.Head > 146 && character.Appearance.Head < 269)
                                character.Appearance.Head = 45;
                            if (character.Appearance.Head > 280 && character.Appearance.Head < 1000)
                                character.Appearance.Head = 45;
                        }
                        break;
                    }
                #endregion Hair

                #region Beard
                // Beard decrement.
                case 97:
                    {
                        character.Appearance.Beard--;
                        if (character.Appearance.Beard < 10)
                            character.Appearance.Beard = 104;
                        if (character.Appearance.Beard > 17 && character.Appearance.Beard < 98)
                            character.Appearance.Beard = 17;
                        break;
                    }
                // Beard increment.
                case 98: 
                    {
                        character.Appearance.Beard++;
                        if (character.Appearance.Beard > 17 && character.Appearance.Beard < 98)
                            character.Appearance.Beard = 98;
                        if (character.Appearance.Beard > 104)
                            character.Appearance.Beard = 10;
                        break;
                    }
                #endregion Beard

                #region Hair Color
                // Burgundy
                case 100:
                    {
                        character.Appearance.HairColor = 20;
                        break;
                    }
                // Red
                case 101:
                    {
                        character.Appearance.HairColor = 19;
                        break;
                    }
                // Vermillion
                case 102:
                    {
                        character.Appearance.HairColor = 10;
                        break;
                    }
                // Pink
                case 103:
                    {
                        character.Appearance.HairColor = 18;
                        break;
                    }
                // Orange
                case 104:
                    {
                        character.Appearance.HairColor = 4;
                        break;
                    }
                // Yellow
                case 105:
                    {
                        character.Appearance.HairColor = 5;
                        break;
                    }
                // Peach
                case 106:
                    {
                        character.Appearance.HairColor = 15;
                        break;
                    }
                // Brown
                case 107:
                    {
                        character.Appearance.HairColor = 7;
                        break;
                    }
                // Dark brown
                case 108:
                    {
                        character.Appearance.HairColor = 26;
                        break;
                    }
                // Light brown
                case 109:
                    {
                        character.Appearance.HairColor = 6;
                        break;
                    }
                // Mint green
                case 110:
                    {
                        character.Appearance.HairColor = 21;
                        break;
                    }
                // Green
                case 111:
                    {
                        character.Appearance.HairColor = 9;
                        break;
                    }
                // Dark green
                case 112:
                    {
                        character.Appearance.HairColor = 22;
                        break;
                    }
                // Dark blue
                case 113:
                    {
                        character.Appearance.HairColor = 17;
                        break;
                    }
                // Turquoise
                case 114:
                    {
                        character.Appearance.HairColor = 8;
                        break;
                    }
                // Cyan
                case 115:
                    {
                        character.Appearance.HairColor = 16;
                        break;
                    }
                // Purple
                case 116:
                    {
                        character.Appearance.HairColor = 11;
                        break;
                    }
                // Violet
                case 117:
                    {
                        character.Appearance.HairColor = 24;
                        break;
                    }
                // Indigo
                case 118:
                    {
                        character.Appearance.HairColor = 23;
                        break;
                    }
                // Dark grey
                case 119:
                    {
                        character.Appearance.HairColor = 3;
                        break;
                    }
                // Military grey
                case 120:
                    {
                        character.Appearance.HairColor = 2;
                        break;
                    }
                // White
                case 121:
                    {
                        character.Appearance.HairColor = 1;
                        break;
                    }
                // Light grey
                case 122:
                    {
                        character.Appearance.HairColor = 14;
                        break;
                    }
                // Tawpe
                case 123:
                    {
                        character.Appearance.HairColor = 13;
                        break;
                    }
                // Black
                case 124:
                    {
                        character.Appearance.HairColor = 12;
                        break;
                    }
                #endregion Hair Color

                #region Torso
                // Torso decrement.
                case 341:
                    {
                        character.Appearance.Torso--;
                        if (character.Appearance.Gender == Gender.Male)
                        {
                            if (character.Appearance.Torso < 18)
                                character.Appearance.Torso = 116;
                            if (character.Appearance.Torso > 25 && character.Appearance.Torso < 111)
                                character.Appearance.Torso = 25;
                        }
                        else
                        {
                            if (character.Appearance.Torso < 56)
                                character.Appearance.Torso = 158;
                            if (character.Appearance.Torso > 60 && character.Appearance.Torso < 153)
                                character.Appearance.Torso = 60;
                        }
                        break;
                    }
                // Torso increment.
                case 342:
                    {
                        character.Appearance.Torso++;
                        if (character.Appearance.Gender == Gender.Male)
                        {
                            if (character.Appearance.Torso > 25 && character.Appearance.Torso < 111)
                                character.Appearance.Torso = 111;
                            if (character.Appearance.Torso > 116)
                                character.Appearance.Torso = 18;
                        }
                        else
                        {
                            if (character.Appearance.Torso > 60 && character.Appearance.Torso < 153)
                                character.Appearance.Torso = 153;
                            if (character.Appearance.Torso > 158)
                                character.Appearance.Torso = 56;
                        }
                        break;
                    }
                #endregion Torso

                #region Torso Color
                // Burgundy.
                case 189:
                    {
                        character.Appearance.TorsoColor = 24;
                        break;
                    }
                // Red.
                case 190:
                    {
                        character.Appearance.TorsoColor = 23;
                        break;
                    }
                // Dark red.
                case 191:
                    {
                        character.Appearance.TorsoColor = 2;
                        break;
                    }
                // Pink.
                case 192:
                    {
                        character.Appearance.TorsoColor = 22;
                        break;
                    }
                // Dark pink.
                case 193:
                    {
                        character.Appearance.TorsoColor = 12;
                        break;
                    }
                // Orange.
                case 194:
                    {
                        character.Appearance.TorsoColor = 11;
                        break;
                    }
                // Burnt orange.
                case 195:
                    {
                        character.Appearance.TorsoColor = 6;
                        break;
                    }
                // Peach.
                case 196:
                    {
                        character.Appearance.TorsoColor = 19;
                        break;
                    }
                // Light khaki.
                case 197:
                    {
                        character.Appearance.TorsoColor = 4;
                        break;
                    }
                // Khaki.
                case 198:
                    {
                        character.Appearance.TorsoColor = 0;
                        break;
                    }
                // Gold.
                case 199:
                    {
                        character.Appearance.TorsoColor = 9;
                        break;
                    }
                // Moss green.
                case 200:
                    {
                        character.Appearance.TorsoColor = 13;
                        break;
                    }
                // Mint green.
                case 201:
                    {
                        character.Appearance.TorsoColor = 25;
                        break;
                    }
                // Sea green.
                case 202:
                    {
                        character.Appearance.TorsoColor = 8;
                        break;
                    }
                // Forest green.
                case 203:
                    {
                        character.Appearance.TorsoColor = 15;
                        break;
                    }
                // Dark green.
                case 204:
                    {
                        character.Appearance.TorsoColor = 26;
                        break;
                    }
                // Royal blue.
                case 205:
                    {
                        character.Appearance.TorsoColor = 21;
                        break;
                    }
                // Dark blue.
                case 206:
                    {
                        character.Appearance.TorsoColor = 7;
                        break;
                    }
                // Light blue.
                case 207:
                    {
                        character.Appearance.TorsoColor = 20;
                        break;
                    }
                // Pale blue.
                case 208:
                    {
                        character.Appearance.TorsoColor = 14;
                        break;
                    }
                // Mauve.
                case 209:
                    {
                        character.Appearance.TorsoColor = 10;
                        break;
                    }
                // Violet.
                case 210:
                    {
                        character.Appearance.TorsoColor = 28;
                        break;
                    }
                // Indigo.
                case 211:
                    {
                        character.Appearance.TorsoColor = 27;
                        break;
                    }
                // Navy blue.
                case 212:
                    {
                        character.Appearance.TorsoColor = 3;
                        break;
                    }
                // Beige.
                case 213:
                    {
                        character.Appearance.TorsoColor = 5;
                        break;
                    }
                // Light grey.
                case 214:
                    {
                        character.Appearance.TorsoColor = 18;
                        break;
                    }
                // Taupe.
                case 215:
                    {
                        character.Appearance.TorsoColor = 17;
                        break;
                    }
                // Dark grey.
                case 216:
                    {
                        character.Appearance.TorsoColor = 1;
                        break;
                    }
                // Black.
                case 217:
                    {
                        character.Appearance.TorsoColor = 16;
                        break;
                    }
                #endregion Torso Color

                #region Arms
                // Arms decrement.
                case 345:
                    {
                        character.Appearance.Arms--;
                        if (character.Appearance.Gender == Gender.Male)
                        {
                            if (character.Appearance.Arms < 26)
                                character.Appearance.Arms = 110;
                            if (character.Appearance.Arms > 31 && character.Appearance.Arms < 105)
                                character.Appearance.Arms = 31;
                        }
                        else
                        {
                            if (character.Appearance.Arms < 61)
                                character.Appearance.Arms = 152;
                            if (character.Appearance.Arms > 65 && character.Appearance.Arms < 147)
                                character.Appearance.Arms = 65;
                        }
                        break;
                    }
                // Arms increment.
                case 346:
                    {
                        character.Appearance.Arms++;
                        if (character.Appearance.Gender == Gender.Male)
                        {
                            if (character.Appearance.Arms > 31 && character.Appearance.Arms < 105)
                                character.Appearance.Arms = 105;
                            if (character.Appearance.Arms > 110)
                                character.Appearance.Arms = 26;
                        }
                        else
                        {
                            if (character.Appearance.Arms > 65 && character.Appearance.Arms < 147)
                                character.Appearance.Arms = 147;
                            if (character.Appearance.Arms > 152)
                                character.Appearance.Arms = 61;
                        }
                        break;
                    }
                #endregion Arms

                #region Wrists
                // Wrist decrement.
                case 349:
                    {
                        character.Appearance.Wrist--;
                        if (character.Appearance.Gender == Gender.Male)
                        {
                            if (character.Appearance.Wrist < 33)
                                character.Appearance.Wrist = 125;
                            if (character.Appearance.Wrist > 84 && character.Appearance.Wrist < 117)
                                character.Appearance.Wrist = 84;
                            if (character.Appearance.Wrist > 34 && character.Appearance.Wrist < 84)
                                character.Appearance.Wrist = 34;
                        }
                        else
                        {
                            if (character.Appearance.Wrist < 67)
                                character.Appearance.Wrist = 167;
                            if (character.Appearance.Wrist > 127 && character.Appearance.Wrist < 159)
                                character.Appearance.Wrist = 127;
                            if (character.Appearance.Wrist > 68 && character.Appearance.Wrist < 127)
                                character.Appearance.Wrist = 68;
                        }
                        break;
                    }
                // Wrist increment.
                case 350:
                    {
                        character.Appearance.Wrist++;
                        if (character.Appearance.Gender == Gender.Male)
                        {
                            if (character.Appearance.Wrist > 34 && character.Appearance.Wrist < 84)
                                character.Appearance.Wrist = 84;
                            if (character.Appearance.Wrist > 84 && character.Appearance.Wrist < 117)
                                character.Appearance.Wrist = 117;
                            if (character.Appearance.Wrist > 126)
                                character.Appearance.Wrist = 33;
                        }
                        else
                        {
                            if (character.Appearance.Wrist > 68 && character.Appearance.Wrist < 127)
                                character.Appearance.Wrist = 127;
                            if (character.Appearance.Wrist > 127 && character.Appearance.Wrist < 159)
                                character.Appearance.Wrist = 159;
                            if (character.Appearance.Wrist > 168)
                                character.Appearance.Wrist = 67;
                        }
                        break;
                    }
                #endregion Wrists

                #region Legs
                // Leg decrement.
                case 353:
                    {
                        character.Appearance.Legs--;
                        if (character.Appearance.Gender == Gender.Male)
                        {
                            if (character.Appearance.Legs < 36)
                                character.Appearance.Legs = 90;
                            if (character.Appearance.Legs > 40 && character.Appearance.Legs < 85)
                                character.Appearance.Legs = 40;
                        }
                        else
                        {
                            if (character.Appearance.Legs < 70)
                                character.Appearance.Legs = 134;
                            if (character.Appearance.Legs > 77 && character.Appearance.Legs < 128)
                                character.Appearance.Legs = 77;
                        }
                        break;
                    }
                // Leg increment.
                case 354:
                    {
                        character.Appearance.Legs++;
                        if (character.Appearance.Gender == Gender.Male)
                        {
                            if (character.Appearance.Legs > 40 && character.Appearance.Legs < 85)
                                character.Appearance.Legs = 85;
                            if (character.Appearance.Legs > 90)
                                character.Appearance.Legs = 36;
                        }
                        else
                        {
                            if (character.Appearance.Legs > 77 && character.Appearance.Legs < 128)
                                character.Appearance.Legs = 128;
                            if (character.Appearance.Legs > 134)
                                character.Appearance.Legs = 70;
                        }
                        break;
                    }
                #endregion Legs

                #region Legs Color
                // Dark green.
                case 248:
                    {
                        character.Appearance.LegColor = 26;
                        break;
                    }
                // Burgundy.
                case 249:
                    {
                        character.Appearance.LegColor = 24;
                        break;
                    }
                // Red.
                case 250:
                    {
                        character.Appearance.LegColor = 23;
                        break;
                    }
                // Dark red.
                case 251:
                    {
                        character.Appearance.LegColor = 3;
                        break;
                    }
                // Pink.
                case 252:
                    {
                        character.Appearance.LegColor = 22;
                        break;
                    }
                // Dark pink.
                case 253:
                    {
                        character.Appearance.LegColor = 13;
                        break;
                    }
                // Orange.
                case 254:
                    {
                        character.Appearance.LegColor = 12;
                        break;
                    }
                // Burnt orange.
                case 255:
                    {
                        character.Appearance.LegColor = 7;
                        break;
                    }
                // Peach.
                case 256:
                    {
                        character.Appearance.LegColor = 19;
                        break;
                    }
                // Light khaki.
                case 257:
                    {
                        character.Appearance.LegColor = 5;
                        break;
                    }
                // Khaki.
                case 258:
                    {
                        character.Appearance.LegColor = 1;
                        break;
                    }
                // Gold.
                case 259:
                    {
                        character.Appearance.LegColor = 10;
                        break;
                    }
                // Moss green.
                case 260:
                    {
                        character.Appearance.LegColor = 14;
                        break;
                    }
                // Mint green.
                case 261:
                    {
                        character.Appearance.LegColor = 25;
                        break;
                    }
                // Sea green.
                case 262:
                    {
                        character.Appearance.LegColor = 9;
                        break;
                    }
                // Forest green.
                case 263:
                    {
                        character.Appearance.LegColor = 0;
                        break;
                    }
                // Royal blue.
                case 264:
                    {
                        character.Appearance.LegColor = 21;
                        break;
                    }
                // Dark blue.
                case 265:
                    {
                        character.Appearance.LegColor = 8;
                        break;
                    }
                // Light blue.
                case 266:
                    {
                        character.Appearance.LegColor = 20;
                        break;
                    }
                // Pale blue.
                case 267:
                    {
                        character.Appearance.LegColor = 15;
                        break;
                    }
                // Mauve.
                case 268:
                    {
                        character.Appearance.LegColor = 11;
                        break;
                    }
                // Violet.
                case 269:
                    {
                        character.Appearance.LegColor = 28;
                        break;
                    }
                // Indigo.
                case 270:
                    {
                        character.Appearance.LegColor = 27;
                        break;
                    }
                // Navy blue.
                case 271:
                    {
                        character.Appearance.LegColor = 4;
                        break;
                    }
                // Beige.
                case 272:
                    {
                        character.Appearance.LegColor = 6;
                        break;
                    }
                // Light grey.
                case 273:
                    {
                        character.Appearance.LegColor = 18;
                        break;
                    }
                // Taupe.
                case 274:
                    {
                        character.Appearance.LegColor = 17;
                        break;
                    }
                // Dark grey.
                case 275:
                    {
                        character.Appearance.LegColor = 2;
                        break;
                    }
                // Black.
                case 276:
                    {
                        character.Appearance.LegColor = 16;
                        break;
                    }

                #endregion Legs Color

                #region Feet
                // Feet decrement.
                case 357:
                    {
                        character.Appearance.Feet--;
                        if (character.Appearance.Gender == Gender.Male)
                        {
                            if (character.Appearance.Feet < 42)
                                character.Appearance.Feet = 43;
                        }
                        else
                        {
                            if (character.Appearance.Feet < 79)
                                character.Appearance.Feet = 80;
                        }
                        break;
                    }
                // Feet increment.
                case 358:
                    {
                        character.Appearance.Feet++;
                        if (character.Appearance.Gender == Gender.Male)
                        {
                            if (character.Appearance.Feet > 43)
                                character.Appearance.Feet = 42;
                        }
                        else
                        {
                            if (character.Appearance.Feet > 80)
                                character.Appearance.Feet = 79;
                        }
                        break;
                    }
                #endregion Feet

                #region Feet Color
                // Brown.
                case 307:
                    {
                        character.Appearance.FeetColor = 0;
                        break;
                    }
                // Dark green.
                case 308:
                    {
                        character.Appearance.FeetColor = 1;
                        break;
                    }
                // Pale grey.
                case 309:
                    {
                        character.Appearance.FeetColor = 2;
                        break;
                    }
                // Black.
                case 310:
                    {
                        character.Appearance.FeetColor = 3;
                        break;
                    }
                // Light brown.
                case 311:
                    {
                        character.Appearance.FeetColor = 4;
                        break;
                    }
                // Grey.
                case 312:
                    {
                        character.Appearance.FeetColor = 5;
                        break;
                    }
                #endregion Feet Color
            }
        }

        /// <summary>
        /// Shows the character setup interface.
        /// </summary>
        /// <param name="character">The character to show interface for.</param>
        public static void Show(Character character)
        {
            Frames.SendInterface(character, 771, false);
            character.Session.SendData(new AnimateInterfacePacketComposer(9835, 771, 79).Serialize());
            character.Session.SendData(new HeadOnInterfacePacketComposer(771, 79).Serialize());
            character.Session.SendData(new MinimapViewPacketComposer(MapViewType.BlackMinimap).Serialize());

            if (character.Appearance.Gender == Gender.Male)
            {
                ChangeToMale(character);
            }
            else
            {
                ChangeToFemale(character);
            }
        }

        /// <summary>
        /// Changes the character's appearance to male defaults.
        /// </summary>
        /// <param name="character">The character to change gender for.</param>
        public static void ChangeToMale(Character character)
        {
            character.Appearance.Gender = Gender.Male;
            character.Appearance.Head = 0;
            character.Appearance.Torso = 18;
            character.Appearance.Arms = 26;
            character.Appearance.Wrist = 33;
            character.Appearance.Legs = 36;
            character.Appearance.Feet = 42;
            character.Appearance.Beard = 10;
            character.Appearance.FeetColor = 0;
            character.Appearance.HairColor = 0;
            character.Appearance.LegColor = 0;
            character.Appearance.SkinColor = 0;
            character.Appearance.TorsoColor = 0;
            character.Session.SendData(new ConfigPacketComposer(1262, 1).Serialize());
            character.UpdateFlags.AppearanceUpdateRequired = true;
        }

        /// <summary>
        /// Changes the character's appearance to female defaults.
        /// </summary>
        /// <param name="character">The character to change gender for.</param>
        public static void ChangeToFemale(Character character)
        {
            character.Appearance.Gender = Gender.Female;
            character.Appearance.Head = 45;
            character.Appearance.Torso = 56;
            character.Appearance.Arms = 61;
            character.Appearance.Wrist = 67;
            character.Appearance.Legs = 70;
            character.Appearance.Feet = 79;
            character.Appearance.Beard = 1000;
            character.Appearance.FeetColor = 0;
            character.Appearance.HairColor = 0;
            character.Appearance.LegColor = 0;
            character.Appearance.SkinColor = 0;
            character.Appearance.TorsoColor = 0;
            character.Session.SendData(new ConfigPacketComposer(1262, 8).Serialize());
            character.UpdateFlags.AppearanceUpdateRequired = true;
        }
        #endregion Methods
    }
}
