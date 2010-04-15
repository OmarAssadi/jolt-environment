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
using System.IO;

namespace RuneScape.Communication.Messages.Outgoing
{
    /// <summary>
    /// Defines composition of a runescape packet.
    /// </summary>
    public abstract partial class PacketComposer
    {
        #region Fields
        /// <summary>
        /// The bitmasks used to work with updating etc.
        /// </summary>
        private static int[] bitmasks = 
        {
		    0, 0x1, 0x3, 0x7,
		    0xf, 0x1f, 0x3f, 0x7f,
		    0xff, 0x1ff, 0x3ff, 0x7ff,
		    0xfff, 0x1fff, 0x3fff, 0x7fff,
		    0xffff, 0x1ffff, 0x3ffff, 0x7ffff,
		    0xfffff, 0x1fffff, 0x3fffff, 0x7fffff,
		    0xffffff, 0x1ffffff, 0x3ffffff, 0x7ffffff,
		    0xfffffff, 0x1fffffff, 0x3fffffff, 0x7fffffff,
		    -1
	    };

        /// <summary>
        /// The default packet size.
        /// </summary>
        private const int DefaultSize = 32;
        #endregion Fields

        #region Properties
        /// <summary>
        /// The id of the opcode.
        /// </summary>
        public int Opcode { get; set; }
        /// <summary>
        /// Gets the buffer payload.
        /// </summary>
        private byte[] Payload { get; set; }
        /// <summary>
        /// Gets the packet type.
        /// </summary>
        public PacketType Type { get; set; }
        /// <summary>
        /// Gets the current index into the buffer (by bits).
        /// </summary>
        public int BitPosition { get; set; }

        /// <summary>
        /// Gets the payload length.
        /// </summary>
        public int Length { get { return this.Payload.Length; } }
        /// <summary>
        /// Gets the payload position into the payload. (current length).
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Gets whether the packet is a raw packet (no header).
        /// </summary>
        public bool Raw { get { return this.Opcode == -1; } }
        /// <summary>
        /// Gets whether the payload is empty.
        /// </summary>
        public bool Empty { get { return this.Position == 0; } }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a basic packet builder.
        /// </summary>
        public PacketComposer() : this(PacketComposer.DefaultSize) { }

        /// <summary>
        /// Constructs a new packet builder.
        /// </summary>
        /// <param name="capacity">The buffer's initial capacity.</param>
        /// <param name="opcode">The opcode id.</param>
        /// <param name="type">The packet type.</param>
        public PacketComposer(int capacity)
        {
            this.Payload = new byte[capacity];
            this.Opcode = -1;
            this.Type = PacketType.Fixed;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Ensures that the buffer is at least <code>minimumBytes</code> bytes.
        /// </summary>
        /// <param name="minimumCapacity">The minimum size required.</param>
        private void EnsureCapacity(int minimumCapacity)
        {
            if (minimumCapacity >= this.Payload.Length)
            {
                ExpandCapacity(minimumCapacity);
            }
        }

        /// <summary>
        /// Expands the buffer.
        /// </summary>
        /// <param name="minimumCapacity">The minimum amount to expand.</param>
        public void ExpandCapacity(int minimumCapacity)
        {
            int newCapacity = (this.Payload.Length + 1) * 2;
            if (newCapacity < 0)
            {
                newCapacity = int.MaxValue;
            }
            else if (minimumCapacity > newCapacity)
            {
                newCapacity = minimumCapacity;
            }

            byte[] newPayload = new byte[newCapacity];
            try
            {
                while (this.Position > this.Payload.Length)
                {
                    this.Position--;
                }
                Buffer.BlockCopy(this.Payload, 0, newPayload, 0, this.Position);
            }
            catch (Exception) { }
            this.Payload = newPayload;
        }

        /// <summary>
        /// Initializes the bit access.
        /// </summary>
        public void InitializeBit()
        {
            this.BitPosition = this.Position * 8;
        }

        /// <summary>
        /// Finishes the bit access.
        /// </summary>
        public void FinishBit()
        {
            this.Position = (this.BitPosition + 7) / 8;
        }

        /// <summary>
        /// Sets the builder's opcode id.
        /// </summary>
        /// <param name="id">The opcode's id.</param>
        public void SetOpcode(int id)
        {
            this.Opcode = id;
        }

        /// <summary>
        /// Sets the builder's opcode type.
        /// </summary>
        /// <param name="type">The opcode type.</param>
        public void SetType(PacketType type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Turns a packet bulder into a packet for reading.
        /// </summary>
        /// <returns>Returns a RuneScape.Network.Packets.Packet object.</returns>
        public Packet ToPacket()
        {
            return new Packet(this.Opcode, this.Type, SerializeBuffer());
        }

        /// <summary>
        /// Serializes the packet to a byte array.
        /// </summary>
        /// <returns>Returns a byte array serving the data composed in this packet.</returns>
        public byte[] Serialize()
        {
            try
            {
                if (this.Raw) // The packet is raw.
                {
                    return this.SerializeBuffer();
                }
                else
                {
                    byte[] data = this.SerializeBuffer(); // Trim the current buffer.
                    //MemoryStream buffer = new MemoryStream(data.Length + 3);
                    using (MemoryStream buffer = new MemoryStream(data.Length + 3))
                    {
                        buffer.WriteByte((byte)this.Opcode);
                        if (this.Type != PacketType.Fixed)
                        {
                            if (this.Type == PacketType.Byte) // A 8-bit packet type.
                            {
                                if (data.Length > 255) // Stack overflow.
                                    throw new ArgumentOutOfRangeException("Could not send a packet with " + data.Length + " bytes within 8 bits.");
                                buffer.WriteByte((byte)data.Length);
                            }
                            else if (this.Type == PacketType.Short) // A 16-bit packet type.
                            {
                                if (data.Length > 65535) // Stack overflow.
                                    throw new ArgumentOutOfRangeException("Could not send a packet with " + data.Length + " bytes within 16 bits.");
                                buffer.WriteByte((byte)(data.Length >> 8));
                                buffer.WriteByte((byte)data.Length);
                            }
                        }
                        buffer.Write(data, 0, data.Length);
                        //byte[] bData = buffer.ToArray();
                        //buffer.Dispose();
                        return buffer.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
            return new byte[] { 0 };
        }

        /// <summary>
        /// Serializes the packet's buffer only.
        /// </summary>
        /// <returns>Returns a byte array holding only buffer data.</returns>
        public byte[] SerializeBuffer()
        {
            byte[] trimmed = new byte[this.Position];
            Buffer.BlockCopy(this.Payload, 0, trimmed, 0, this.Position);
            return trimmed;
        }
        #endregion Methods
    }
}
