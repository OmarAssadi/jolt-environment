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

namespace RuneScape.Communication.Messages.Outgoing
{
    /// <summary>
    /// Provides writing functions to work with bytes.
    /// </summary>
    public abstract partial class PacketComposer
    {
        #region Methods
        /// <summary>
        /// Appends a number of bits to the buffer.
        /// </summary>
        /// <param name="numBits">Number of bits to add.</param>
        /// <param name="value">Values for the bits.</param>
        public void AppendBits(int numBits, int value)
        {
            int bytePos = this.BitPosition >> 3;
            int bitOffset = 8 - (this.BitPosition & 7);
            this.BitPosition += numBits;
            this.Position = (this.BitPosition + 7) / 8;
            EnsureCapacity(this.Position);
            for (; numBits > bitOffset; bitOffset = 8)
            {
                this.Payload[bytePos] &= (byte)~bitmasks[bitOffset];	 // mask out the desired area
                this.Payload[bytePos++] |= (byte)((value >> (numBits - bitOffset)) & bitmasks[bitOffset]);

                numBits -= bitOffset;
            }
            if (numBits == bitOffset)
            {
                this.Payload[bytePos] &= (byte)~bitmasks[bitOffset];
                this.Payload[bytePos] |= (byte)(value & bitmasks[bitOffset]);
            }
            else
            {
                this.Payload[bytePos] &= (byte)~(bitmasks[numBits] << (bitOffset - numBits));
                this.Payload[bytePos] |= (byte)((value & bitmasks[numBits]) << (bitOffset - numBits));
            }
        }

        /// <summary>
        /// Appends a byte to the buffer and advanced one index.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <param name="checkCapacity">Whether to check for the capacity.</param>
        public void AppendByte(byte value, bool checkCapacity)
        {
            if (checkCapacity)
                EnsureCapacity(this.Position + 1);
            Payload[this.Position++] = value;
        }

        /// <summary>
        /// Appends a byte to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendByte(byte value)
        {
            AppendByte(value, true);
        }

        /// <summary>
        /// Appends an A type byte to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendByteA(byte value)
        {
            AppendByte((byte)(value + 128), true);
        }

        /// <summary>
        /// Appends an C type byte to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendByteC(byte value)
        {
            AppendByte((byte)-value);
        }

        /// <summary>
        /// Appends an S type byte to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendByteS(byte value)
        {
            AppendByte((byte)(128 - value));
        }

        /// <summary>
        /// Appends a series of bytes to the buffer.
        /// </summary>
        /// <param name="data">The byte array to append.</param>
        /// <param name="offset">The startpoint in the buffer to start copying.</param>
        /// <param name="length">How long data to copy from the byte array.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendBytes(byte[] data, int offset, int length)
        {
            int newPos = this.Position + length;
            EnsureCapacity(newPos);
            Buffer.BlockCopy(data, offset, this.Payload, this.Position, length);
            this.Position = newPos;
        }

        /// <summary>
        /// Appends a series of bytes to the buffer.
        /// </summary>
        /// <param name="data">The byte array to append.</param>
        /// <returns>returns this instance.</returns>
        public void AppendBytes(byte[] data)
        {
            AppendBytes(data, 0, data.Length);
        }

        /// <summary>
        /// Appends a short to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendShort(short value)
        {
            EnsureCapacity(this.Position + 2);
            AppendByte((byte)(value >> 8), false);
            AppendByte((byte)value, false);
        }

        /// <summary>
        /// Appends a little endian short to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendLEShort(short value)
        {
            EnsureCapacity(this.Position + 2);
            AppendByte((byte)value, false);
            AppendByte((byte)(value >> 8), false);
        }

        /// <summary>
        /// Appends an A type short to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendShortA(short value)
        {
            EnsureCapacity(this.Position + 2);
            AppendByte((byte)(value >> 8), false);
            AppendByte((byte)(value + 128), false);
        }

        /// <summary>
        /// Appends a little endian A type short the buffer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns this instance.</returns>
        public void AppendLEShortA(short value)
        {
            EnsureCapacity(this.Position + 2);
            AppendByte((byte)(value + 128), false);
            AppendByte((byte)(value >> 8), false);
        }

        /// <summary>
        /// Appends an int to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendInt(int value)
        {
            EnsureCapacity(this.Position + 4);
            AppendByte((byte)(value >> 24), false);
            AppendByte((byte)(value >> 16), false);
            AppendByte((byte)(value >> 8), false);
            AppendByte((byte)value, false);
        }

        /// <summary>
        /// Appends an secondary type int to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendIntSecondary(int value)
        {
            EnsureCapacity(this.Position + 4);
            AppendByte((byte)(value >> 8), false);
            AppendByte((byte)value, false);
            AppendByte((byte)(value >> 24), false);
            AppendByte((byte)(value >> 16), false);
        }

        /// <summary>
        /// Appends an tertiary type int to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendIntTertiary(int value)
        {
            EnsureCapacity(this.Position + 4);
            AppendByte((byte)(value >> 16), false);
            AppendByte((byte)(value >> 24), false);
            AppendByte((byte)value, false);
            AppendByte((byte)(value >> 8), false);
        }

        /// <summary>
        /// Appends a little endian int to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instnace.</returns>
        public void AppendLEInt(int value)
        {
            EnsureCapacity(this.Position + 4);
            AppendByte((byte)value, false);
            AppendByte((byte)(value >> 8), false);
            AppendByte((byte)(value >> 16), false);
            AppendByte((byte)(value >> 24), false);
        }

        /// <summary>
        /// Appends a long to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendLong(long value)
        {
            AppendInt((int)(value >> 32));
            AppendInt((int)(value & -1L));
        }

        /// <summary>
        /// Appends a long to the buffer.
        /// </summary>
        /// <param name="value">The value of the integer.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendLELong(long value)
        {
            AppendLEInt((int)(value & -1L));
            AppendLEInt((int)(value >> 32));
        }

        /// <summary>
        /// Appends a string to the buffer.
        /// </summary>
        /// <param name="value">The value of the string.</param>
        /// <returns>Returns this instance.</returns>
        public void AppendString(string value)
        {
            EnsureCapacity(this.Position + value.Length + 1);
            AppendBytes(new ASCIIEncoding().GetBytes(value));
            AppendByte(0);
        }
        #endregion Methods
    }
}
