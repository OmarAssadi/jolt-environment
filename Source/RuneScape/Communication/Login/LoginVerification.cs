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
using System.Threading.Tasks;

using JoltEnvironment.Security;
using JoltEnvironment.Utilities;
using RuneScape.Communication.Messages;
using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Characters;

namespace RuneScape.Communication.Login
{
    /// <summary>
    /// Represents the login verification stage.
    /// </summary>
    public class LoginVerification
    {
        #region Methods
        /// <summary>
        /// Processes the login verification.
        /// </summary>
        /// <param name="request">The request to process and verify.</param>
        /// <param name="type">The login connection type.</param>
        public static void Process(LoginRequest request, LoginConnectionType type)
        {
            short packetSize = -1;
            if (request.Buffer.RemainingAmount >= 2)
            {
                packetSize = request.Buffer.ReadShort(); // The size will vary depending on user's inputted username and password.
            }
            else return;

            if (request.Buffer.RemainingAmount >= packetSize)
            {
                Packet p = new Packet(request.Buffer.RemainingData);
                int encryptedPacketSize = packetSize - packetSize - (36 + 1 + 1 + 2); // Shouldn't be under 0.

                int clientVersion = p.ReadInt();
                if (clientVersion != 508) // Check to make sure the client is 508.
                {
                    request.LoginStage = -3;
                    return;
                }

                // Client preferences.
                byte lowMemory = p.ReadByte();
                bool hd = p.ReadByte() == 1 ? true : false;
                bool resized = p.ReadByte() == 1 ? true : false;
                short width = p.ReadShort();
                short height = p.ReadShort();

                Console.WriteLine("HD: " + hd);
                Console.WriteLine("Resized: " + resized);

                p.Skip(141);

                int tmpEncryptPacketSize = p.ReadByte();
                if (tmpEncryptPacketSize != 10)
                {
                    int encryptPacketId = p.ReadByte();
                }

                // Session data.
                long clientKey = p.ReadLong(); // The client's session key.
                long serverKey = p.ReadLong(); // The client's server session key.

                // Hash verification.
                long longName = p.ReadLong();
                int hash = (int)(31 & longName >> 16); // Verify client session hash.
                if (hash != request.NameHash) // Possibly a bot attack.
                {
                    request.LoginStage = -3;
                    return;
                }

                // User data.
                string username = StringUtilities.LongToString(longName);
                string password = Hash.GetHash(username + Hash.GetHash(p.ReadString(), HashType.SHA1), HashType.SHA1);

                // Try to load the account with the given details.
                Details details = new Details(request.Connection, username, password, hd, resized, clientKey, serverKey);
                Program.Logger.WriteDebug("Login request: " + details.ToString());
                Task.Factory.StartNew(() => GameEngine.World.CharacterManager.LoadAccount(details, type));
                request.Finished = true;
            }
            return;
        }
        #endregion Methods
    }
}
