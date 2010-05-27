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
    /// Lengths of packets.
    /// </summary>
    public class PacketLengths
    {
        #region Fields
        /// <summary>
        /// An array of integers representing the length of the packet.
        /// </summary>
        private int[] length = new int[256];
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the length of the specified packet opcode.
        /// </summary>
        /// <param name="packetOpcode">The packet opcode to get length for.</param>
        /// <returns>Returns the length.</returns>
        public int this[int packetOpcode]
        {
            get
            {
                if (packetOpcode < length.Length)
                {
                    return length[packetOpcode];
                }
                return -3;
            }
        }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Sets the packet lengths.
        /// </summary>
        public PacketLengths()
        {
            length[0] = -3;
            length[1] = -3;
            length[2] = 8;
            length[3] = 8;
            length[4] = -3;
            length[5] = -3;
            length[6] = -3;
            length[7] = 2;
            length[8] = -3;
            length[9] = -3;
            length[10] = -3;
            length[11] = -3;
            length[12] = -3;
            length[13] = 10;
            length[14] = -3;
            length[15] = -3;
            length[16] = -3;
            length[17] = -3;
            length[18] = -3;
            length[19] = -3;
            length[20] = -3;
            length[21] = 6;
            length[22] = 4;
            length[23] = -3;
            length[24] = -3;
            length[25] = -3;
            length[26] = -3;
            length[27] = -3;
            length[28] = -3;
            length[29] = -3;
            length[30] = 8;
            length[31] = -3;
            length[32] = -3;
            length[33] = -3;
            length[34] = -3;
            length[35] = -3;
            length[36] = -3;
            length[37] = 2;
            length[38] = 2;
            length[39] = -3;
            length[40] = 16;
            length[41] = -3;
            length[42] = -3;
            length[43] = -3;
            length[44] = -3;
            length[45] = -3;
            length[46] = -3;
            length[47] = 0;
            length[48] = -3;
            length[49] = -1;
            length[50] = -3;
            length[51] = -3;
            length[52] = 2;
            length[53] = -3;
            length[54] = -3;
            length[55] = -3;
            length[56] = -3;
            length[57] = -3;
            length[58] = -3;
            length[59] = 6;
            length[60] = 0;
            length[61] = 8;
            length[62] = -3;
            length[63] = 6;
            length[64] = -3;
            length[65] = -3;
            length[66] = -3;
            length[67] = -3;
            length[68] = -3;
            length[69] = -3;
            length[70] = 8;
            length[71] = -3;
            length[72] = -3;
            length[73] = -3;
            length[74] = -3;
            length[75] = -3;
            length[76] = -3;
            length[77] = -3;
            length[78] = -3;
            length[79] = -3;
            length[80] = -3;
            length[81] = -3;
            length[82] = -3;
            length[83] = -3;
            length[84] = 2;
            length[85] = -3;
            length[86] = -3;
            length[87] = -3;
            length[88] = 2;
            length[89] = -3;
            length[90] = 6;
            length[91] = -3;
            length[92] = -3;
            length[93] = -3;
            length[94] = -3;
            length[95] = -3;
            length[96] = -3;
            length[97] = -3;
            length[98] = -3;
            length[99] = 4;
            length[100] = -3;
            length[101] = -3;
            length[102] = -3;
            length[103] = -3;
            length[104] = -3;
            length[105] = -3;
            length[106] = -3;
            length[107] = -1;
            length[108] = 0;
            length[109] = -3;
            length[110] = -3;
            length[111] = -3;
            length[112] = -3;
            length[113] = 4;
            length[114] = -3;
            length[115] = 0;
            length[116] = -3;
            length[117] = -1;
            length[118] = -3;
            length[119] = -1;
            length[120] = -3;
            length[121] = -3;
            length[122] = -3;
            length[123] = 2;
            length[124] = -3;
            length[125] = -3;
            length[126] = -3;
            length[127] = -3;
            length[128] = -3;
            length[129] = 6;
            length[130] = -3;
            length[131] = 10;
            length[132] = 8;
            length[133] = 6;
            length[134] = -3;
            length[135] = -3;
            length[136] = -3;
            length[137] = -3;
            length[138] = -1;
            length[139] = -3;
            length[140] = -3;
            length[141] = -3;
            length[142] = -3;
            length[143] = -3;
            length[144] = -3;
            length[145] = -3;
            length[146] = -3;
            length[147] = -3;
            length[148] = -3;
            length[149] = -3;
            length[150] = -3;
            length[151] = -3;
            length[152] = -3;
            length[153] = -3;
            length[154] = -3;
            length[155] = -3;
            length[156] = -3;
            length[157] = -3;
            length[158] = 6;
            length[159] = -3;
            length[160] = 2;
            length[161] = -3;
            length[162] = -3;
            length[163] = -3;
            length[164] = -3;
            length[165] = 4;
            length[166] = -3;
            length[167] = 9;
            length[168] = -3;
            length[169] = 6;
            length[170] = -3;
            length[171] = -3;
            length[172] = -3;
            length[173] = 6;
            length[174] = -3;
            length[175] = -3;
            length[176] = -3;
            length[177] = -3;
            length[178] = -1;
            length[179] = 12;
            length[180] = -3;
            length[181] = -3;
            length[182] = -3;
            length[183] = -3;
            length[184] = -3;
            length[185] = -3;
            length[186] = 8;
            length[187] = -3;
            length[188] = -3;
            length[189] = -3;
            length[190] = -3;
            length[191] = -3;
            length[192] = -3;
            length[193] = -3;
            length[194] = -3;
            length[195] = -3;
            length[196] = -3;
            length[197] = -3;
            length[198] = -3;
            length[199] = 2;
            length[200] = -3;
            length[201] = 6;
            length[202] = -3;
            length[203] = 8;
            length[204] = -3;
            length[205] = -3;
            length[206] = -3;
            length[207] = -3;
            length[208] = -3;
            length[209] = -3;
            length[210] = -3;
            length[211] = 8;
            length[212] = -3;
            length[213] = -3;
            length[214] = 6;
            length[215] = -3;
            length[216] = -3;
            length[217] = -3;
            length[218] = -3;
            length[219] = -3;
            length[220] = 8;
            length[221] = -3;
            length[222] = -1;
            length[223] = -3;
            length[224] = 14;
            length[225] = -3;
            length[226] = -3;
            length[227] = 2;
            length[228] = 6;
            length[229] = -3;
            length[230] = -3;
            length[231] = -3;
            length[232] = 6;
            length[233] = 6;
            length[234] = -3;
            length[235] = -3;
            length[236] = -3;
            length[237] = -3;
            length[238] = -3;
            length[239] = -3;
            length[240] = -3;
            length[241] = -3;
            length[242] = -3;
            length[243] = -3;
            length[244] = -3;
            length[245] = -3;
            length[246] = -3;
            length[247] = 4;
            length[248] = 1;
            length[249] = -3;
            length[250] = 4;
            length[251] = -3;
            length[252] = -3;
            length[253] = 2;
            length[254] = -3;
            length[255] = -3;
        }
        #endregion Constructor
    }
}
