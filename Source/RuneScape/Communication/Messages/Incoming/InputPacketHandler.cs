using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Characters;

namespace RuneScape.Communication.Messages.Incoming
{
    /// <summary>
    /// Handler for user input from interfaces.
    /// </summary>
    public class InputPacketHandler : IPacketHandler
    {
        #region Methods
        /// <summary>
        /// Handles user input from interfaces.
        /// </summary>
        /// <param name="character">The character to handle packet for.</param>
        /// <param name="packet">The packet containing handle data.</param>
        public void Handle(Character character, Packet packet)
        {
            switch (packet.Opcode)
            {
                case 43:
                    int count = packet.ReadInt();

                    #region Deposit
                    /**
                     * Checks if the character requested a deposit.
                     */
                    if (character.Preferences["deposit"] != null)
                    {
                        Program.Logger.WriteDebug("byte deposit:" + (byte)character.Preferences["deposit"]);
                        character.Bank.Deposit((byte)character.Preferences["deposit"], count);
                        character.Preferences.Remove("deposit");
                        character.Preferences.BankX = count;
                        character.Session.SendData(new ConfigPacketComposer(1249, count).Serialize());
                    }
                    #endregion Deposit

                    #region Withdraw
                    /**
                     * Checks if the character requested a deposit.
                     */
                    if (character.Preferences["withdraw"] != null)
                    {
                        Program.Logger.WriteDebug("byte withdraw:" + (byte)character.Preferences["withdraw"]);
                        character.Bank.Withdraw((byte)character.Preferences["withdraw"], count);
                        character.Preferences.Remove("withdraw");
                        character.Preferences.BankX = count;
                        character.Session.SendData(new ConfigPacketComposer(1249, count).Serialize());
                    }
                    #endregion Deposit
                    break;
            }
        }
        #endregion Methods
    }
}