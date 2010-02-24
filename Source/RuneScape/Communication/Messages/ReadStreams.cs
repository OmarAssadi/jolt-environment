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

namespace RuneScape.Communication.Messages
{
    /// <summary>
    /// Provides a read buffer for working work bytes.
    /// </summary>
    public partial class Packet
    {
        #region Methods
        /// <summary>
        /// Reads the next byte in the packet without removing it from the payload.
        /// </summary>
        /// <returns>Returns a 8-bit integer.</returns>
        public byte Peek()
        {
            return this.Payload[this.Position];
        }

        /// <summary>
        /// Reads a single byte from the buffer and advances one position in the payload index.
        /// </summary>
        /// <returns>Returns a 8-bit integer.</returns>
        public byte ReadByte()
        {
            return this.Payload[this.Position++];
        }

        /// <summary>
        /// Reads a C type byte from the buffer.
        /// </summary>
        /// <returns>Returns a 8-bit integer.</returns>
        public byte ReadByteC()
        {
            return (byte)-this.Payload[this.Position++];
        }

        /// <summary>
        /// Reads a S type byte from the buffer.
        /// </summary>
        /// <returns>Returns a 8-bit integer.</returns>
        public byte ReadByteS()
        {
            return (byte)(128 - this.Payload[this.Position++]);
        }

        /// <summary>
        /// Reads the buffer and fulls up the whole specified array.
        /// </summary>
        /// <param name="array"></param>
        public void Read(byte[] array)
        {
            Read(array, 0, array.Length);
        }

        /// <summary>
        /// Reads an array of bytes to the specified array.
        /// </summary>
        /// <param name="array">The array to read to.</param>
        /// <param name="offset">The start point to start reading.</param>
        /// <param name="length">The length to read for.</param>
        public void Read(byte[] array, int offset, int length)
        {
            /*for (int i = offset; i < offset + length; i++)
            {
                array[i] = ReadByte();
            }*/

            Buffer.BlockCopy(this.Payload, this.Position, array, offset, length);
            Skip(length);
        }

        /// <summary>
        /// Reads a short from the buffer.
        /// </summary>
        /// <returns>Returns a 16-bit integer.</returns>
        public short ReadShort()
        {
            return (short)((short)((this.Payload[this.Position++] & 0xff) << 8) |
                (short)(this.Payload[this.Position++] & 0xff));
        }

        /// <summary>
        /// Reads a type A short from the buffer.
        /// </summary>
        /// <returns>Returns a 16-bit integer.</returns>
        public short ReadShortA()
        {
            return (short)(((this.Payload[this.Position++] & 0xFF) << 8)
                + (this.Payload[this.Position++] - 128 & 0xFF));
        }

        /// <summary>
        /// Reads a little endian short from the buffer.
        /// </summary>
        /// <returns>Returns a 16-bit integer.</returns>
        public short ReadLEShort()
        {
            int i = ((this.Payload[this.Position++] & 0xff)) +
                ((this.Payload[this.Position++] & 0xff) << 8);
            if (i > 32767)
                i -= 0x10000;
            return (short)i;
        }

        /// <summary>
        /// Reads a little endian type A short from the buffer.
        /// </summary>
        /// <returns>Returns a 16-bit integer.</returns>
        public short ReadLEShortA()
        {
            int i = ((this.Payload[this.Position++] - 128 & 0xff)) +
                ((this.Payload[this.Position++] & 0xff) << 8);
            if (i > 32767)
                i -= 0x10000;
            return (short)i;
        }

        /// <summary>
        /// Reads a int from the buffer.
        /// </summary>
        /// <returns>Returns a 32-bit integer.</returns>
        public int ReadInt()
        {
            return ((this.Payload[this.Position++] & 0xff) << 24)
                | ((this.Payload[this.Position++] & 0xff) << 16)
                | ((this.Payload[this.Position++] & 0xff) << 8)
                | (this.Payload[this.Position++] & 0xff);
        }

        /// <summary>
        /// Reads a little endian from the buffer
        /// </summary>
        /// <returns>Returns a 32-bit integer.</returns>
        public int ReadLEInt()
        {
            return (this.Payload[this.Position++] & 0xff)
                | ((this.Payload[this.Position++] & 0xff) << 8)
                | ((this.Payload[this.Position++] & 0xff) << 16)
                | ((this.Payload[this.Position++] & 0xff) << 24);
        }

        /// <summary>
        /// Reads a long from the buffer.
        /// </summary>
        /// <returns>Returns a 64-bit integer.</returns>
        public long ReadLong()
        {
            return ((long)(this.Payload[this.Position++] & 0xff) << 56)
                | ((long)(this.Payload[this.Position++] & 0xff) << 48)
                | ((long)(this.Payload[this.Position++] & 0xff) << 40)
                | ((long)(this.Payload[this.Position++] & 0xff) << 32)
                | ((long)(this.Payload[this.Position++] & 0xff) << 24)
                | ((long)(this.Payload[this.Position++] & 0xff) << 16)
                | ((long)(this.Payload[this.Position++] & 0xff) << 8)
                | ((long)((long)this.Payload[this.Position++] & 0xff));
        }
        
        /// <summary>
        /// Reads a string from the buffer.
        /// </summary>
        /// <returns>Returns a string.</returns>
        public string ReadString()
        {
            StringBuilder sb = new StringBuilder();
            byte b;

            while ((b = ReadByte()) != 0)
            {
                sb.Append((char)b);
            }
            return sb.ToString();
        }
        #endregion Methods
    }
}
