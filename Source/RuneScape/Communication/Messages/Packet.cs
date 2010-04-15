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

namespace RuneScape.Communication.Messages
{
    /// <summary>
    /// Provides multiple ways to read a packet.
    /// </summary>
    public partial class Packet
    {
        #region Fields
        /// <summary>
        /// Possible hex values.
        /// </summary>
        private static char[] hex = "0123456789ABCDEF".ToCharArray();
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets or sets the opcode id.
        /// </summary>
        public int Opcode { get; set; }
        /// <summary>
        /// Gets or sets the packet type.
        /// </summary>
        public PacketType Type { get; set; }
        /// <summary>
        /// Gets the packet buffer.
        /// </summary>
        private byte[] Payload { get; set; }

        /// <summary>
        /// Gets the packet's payload length.
        /// </summary>
        public int Length { get { return this.Payload.Length; } }
        /// <summary>
        /// Gets the current position into the payload.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Gets whether the packet is a raw packet (no header).
        /// </summary>
        public bool Raw { get { return this.Opcode == -1; } }

        /// <summary>
        /// Gets the remaining amount of data within the buffer.
        /// </summary>
        public int RemainingAmount { get { return this.Payload.Length - this.Position; } }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new packet.
        /// </summary>
        /// <param name="opcode">The opcode id.</param>
        /// <param name="type">The packet type.</param>
        /// <param name="data">The payload.</param>
        public Packet(int opcode, PacketType type, byte[] data)
        {
            this.Opcode = opcode;
            this.Type = type;
            this.Payload = data;
        }

        /// <summary>
        /// Constructs a new packet.
        /// </summary>
        /// <param name="opcode">The packet id.</param>
        /// <param name="data">The payload.</param>
        public Packet(int opcode, byte[] data) : this(opcode, PacketType.Fixed, data) { }

        /// <summary>
        /// Constructs a raw bare packet.
        /// </summary>
        /// <param name="data">The payload.</param>
        public Packet(byte[] data) : this(-1, data) { }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Gets the remaining data as a seperate array.
        /// </summary>
        /// <param name="movePosition">Whether to move the position of the payload index to the end.</param>
        /// <returns>Returns an array of Int8 integers.</returns>
        public byte[] GetRemainingData(bool movePosition = true)
        {
            byte[] remaining = new byte[this.RemainingAmount];
            Buffer.BlockCopy(this.Payload, this.Position, remaining, 0, this.RemainingAmount);
            this.Position += remaining.Length;
            return remaining;
        }

        /// <summary>
        /// Prints out the packet opcode, length and the payload itself.
        /// </summary>
        /// <returns>Returns a string.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.Length; i++)
            {
                if (sb.Length != 0)
                {
                    sb.Append(",");
                }
                sb.Append(ByteToHex(this.Payload[i], true));
            }
            sb.Append("]");
            sb.Insert(0, "[opcode=" + this.Opcode + ",length=" + this.Length + ",data=");
            return sb.ToString();
        }

        /// <summary>
        /// Turns a byte into a hex.
        /// </summary>
        /// <param name="b">The byte to convert.</param>
        /// <param name="forceLeadingZero">Whether to add a reading zero.</param>
        /// <returns>Returns a string containing the hex value of the byte.</returns>
        private static string ByteToHex(byte b, bool forceLeadingZero)
        {
            StringBuilder sb = new StringBuilder();
		    int ub = b & 0xff;
		    if(ub / 16 > 0 || forceLeadingZero)
			    sb.Append(hex[ub / 16]);
		    sb.Append(hex[ub % 16]);
		    return sb.ToString();
        }

        /// <summary>
        /// Skips a given amount of bytes into the payload.
        /// </summary>
        /// <param name="length">The number of bytes to skip.</param>
        public void Skip(int length)
        {
            this.Position += length;
        }

        /// <summary>
        /// Rewinds the payload's index to 0.
        /// </summary>
        public void Rewind()
        {
            this.Position = 0;
        }

        /// <summary>
        /// Adds a single byte to the end of the current packet.
        /// </summary>
        /// <param name="b">The byte to add.</param>
        public void AddByte(byte b)
        {
            AddBytes(new byte[] { b });
        }

        /// <summary>
        /// Adds an array of bytes to the end of the current packet.
        /// </summary>
        /// <param name="data">The byte array to add.</param>
        public void AddBytes(byte[] data)
        {
            byte[] newData = new byte[this.Payload.Length + data.Length];
            Buffer.BlockCopy(this.Payload, 0, newData, 0, this.Payload.Length);
            Buffer.BlockCopy(data, 0, newData, this.Payload.Length, data.Length);
            this.Payload = newData;
        }
        #endregion Methods
    }
}
