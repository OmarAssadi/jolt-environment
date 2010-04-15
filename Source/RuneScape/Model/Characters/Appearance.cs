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

namespace RuneScape.Model.Characters
{
    /// <summary>
    /// Represents a character's appearance.
    /// </summary>
    public class Appearance
    {
        #region Properties
        /// <summary>
        /// Gets the character's gender.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets whether the character is a npc.
        /// </summary>
        public bool IsNpc { get; set; }
        /// <summary>
        /// Gets the character's npc id (only valid if the character <code>IsNpc</code>).
        /// </summary>
        public short NpcId { get; set; }

        /// <summary>
        /// Gets the character's hair color.
        /// </summary>
        public byte HairColor { get; set; }
        /// <summary>
        /// Gets the character's torso color.
        /// </summary>
        public byte TorsoColor { get; set; }
        /// <summary>
        /// Gets the character's leg color.
        /// </summary>
        public byte LegColor { get; set; }
        /// <summary>
        /// Gets the character's feet color.
        /// </summary>
        public byte FeetColor { get; set; }
        /// <summary>
        /// Gets the character's skin color.
        /// </summary>
        public byte SkinColor { get; set; }

        /// <summary>
        /// Gets the character's head appearance.
        /// </summary>
        public short Head { get; set; }
        /// <summary>
        /// Gets the character's chest appearance.
        /// </summary>
        public short Torso { get; set; }
        /// <summary>
        /// Gets the character's arm appearance.
        /// </summary>
        public short Arms { get; set; }
        /// <summary>
        /// Gets the character's hands appearance.
        /// </summary>
        public short Wrist { get; set; }
        /// <summary>
        /// Gets the character's legs appearance.
        /// </summary>
        public short Legs { get; set; }
        /// <summary>
        /// Gets the character's feet appearance.
        /// </summary>
        public short Feet { get; set; }
        /// <summary>
        /// Gets the character's beard appearance.
        /// </summary>
        public short Beard { get; set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a character's appearance with the default look.
        /// </summary>
        /// <param name="b">This is just a filler, the value of this does 
        /// not affect creation of appearance instances.</param>
        public Appearance()
        {
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Gives default appearance values.
        /// 
        ///     <para>This is usually used if there is a problem 
        ///     loading the character from sql database.</para>
        /// </summary>
        public void SetDefaults()
        {
            this.Gender = Gender.Male;
            this.IsNpc = false;
            this.NpcId = -1;
            this.Head = 0;
            this.Torso = 18;
            this.Arms = 26;
            this.Wrist = 33;
            this.Legs = 36;
            this.Feet = 42;
            this.Beard = 10;
            this.HairColor = 0;
            this.TorsoColor = 0;
            this.LegColor = 0;
            this.FeetColor = 0;
            this.SkinColor = 0;
        }

        /// <summary>
        /// Gets the all apearance properties as an array.
        /// </summary>
        /// <returns>Returns an array of Int16 values representing appearance properties.</returns>
        public short[] GetLook()
        {
            return new short[]
                {
                    (byte)this.Gender,
                    this.HairColor,
                    this.TorsoColor,
			        this.LegColor,
			        this.FeetColor,
			        this.SkinColor,
			        this.Head,
			        this.Torso,
			        this.Arms,
			        this.Wrist,
			        this.Legs,
			        this.Feet,
			        this.Beard
                };
        }

        /// <summary>
        /// Sets the character's appearance by using properties from the specified array.
        /// </summary>
        /// <param name="value">An array of Int16 values representing appearance properties.</param>
        public void SetLook(short[] value)
        {
            if (value.Length != 13)
                throw new ArgumentOutOfRangeException("Length of array must be 13.");
            Gender = (Gender)value[0];
            this.Head = value[6];
            this.Torso = value[7];
            this.Arms = value[8];
            this.Wrist = value[9];
            this.Legs = value[10];
            this.Feet = value[11];
            this.Beard = value[12];
            this.HairColor = (byte)value[1];
            this.TorsoColor = (byte)value[2];
            this.LegColor = (byte)value[3];
            this.FeetColor = (byte)value[4];
            this.SkinColor = (byte)value[5];
        }

        /// <summary>
        /// Transforms the character into an npc.
        /// </summary>
        /// <param name="npcId">The npc to transform into.</param>
        public void TransformToNpc(short npcId)
        {
            this.IsNpc = true;
            this.NpcId = npcId;
        }

        /// <summary>
        /// Transforms the character back to a character (if previously was a npc).
        /// </summary>
        public void TransformToCharacter()
        {
            this.IsNpc = false;
        }
        #endregion Methods
    }
}
