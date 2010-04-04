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

using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Content.Trading;
using RuneScape.Model.Characters;

namespace RuneScape.Content
{
    /// <summary>
    /// Represents a request managed for trading, dueling, etc.
    /// </summary>
    public class Request
    {
        #region Fields
        /// <summary>
        /// The character who owns the request.
        /// </summary>
        private Character character;
        #endregion Fields

        #region Properties
        /// <summary>
        /// The trade manager between this character, and the other.
        /// </summary>
        public Trade Trade { get; set; }
        /// <summary>
        /// The character requesting a trade with this character.
        /// </summary>
        public Character TradeReq { get; set; }

        /// <summary>
        /// Gets whether this request/character is trading.
        /// </summary>
        public bool Trading
        {
            get { return this.Trade != null; }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new request.
        /// </summary>
        /// <param name="character">The character who will own the request.</param>
        public Request(Character character)
        {
            this.character = character;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Requests a trade with the specified character.
        /// </summary>
        /// <param name="other">The character to request.</param>
        public void RequestTrade(Character other)
        {
            if (other.Request.TradeReq == this.character)
            {
                AnswerTrade(other);
            }
            else
            {
                this.character.Session.SendData(new MessagePacketComposer("Sending trade request...").Serialize());
                other.Session.SendData(new MessagePacketComposer(this.character.PrettyName + ":tradereq:").Serialize());
                this.TradeReq = other;
            }
        }

        /// <summary>
        /// Answers a trade request from the specified character.
        /// </summary>
        /// <param name="other">The character to answer.</param>
        public void AnswerTrade(Character other)
        {
            if (this.Trade != null)
            {
                if (this.Trade.Character1 == other && this.Trade.Character2 == this.character)
                {
                    return;
                }

                if (this.Trade.Character2 == other && this.Trade.Character1 == this.character)
                {
                    return;
                }
                this.Trade.Close();
            }

            if (other.Request.TradeReq == this.character)
            {
                EstablishTrade(other);
            }
            else
            {
                RequestTrade(other);
            }
        }

        /// <summary>
        /// Establishes a trade between this character, and the other.
        /// </summary>
        /// <param name="other">The character to establish trade with.</param>
        private void EstablishTrade(Character other)
        {
            Trade trade = new Trade(this.character, other);
            this.character.Request.Trade = trade;
            other.Request.Trade = trade;
        }

        /// <summary>
        /// Attempts to close any current trading operations.
        /// </summary>
        public void CloseTrade()
        {
            if (this.Trade != null)
            {
                this.Trade.Close();
            }
        }
        #endregion Methods
    }
}
