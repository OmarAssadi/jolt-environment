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
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using RuneScape.Database.Account;
using RuneScape.Model.Characters;

namespace RuneScape.Workers
{
    /// <summary>
    /// A worker processing "account" tasks.
    /// </summary>
    public class AccountWorker : Worker
    {
        #region Fields
        /// <summary>
        /// A blocking collection holding characters that require saving.
        /// </summary>
        private BlockingCollection<Character> toSave = new BlockingCollection<Character>();
        /// <summary>
        /// Provides account saving accessability.
        /// </summary>
        private IAccountSaver accountSaver = new SqlAccountSaver();
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs an account wokrer with normal priority.
        /// </summary>
        public AccountWorker() : base("Account Worker", ThreadPriority.Normal) { }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Thread's process.
        /// </summary>
        protected override void Run()
        {
            /*
             * Because the server requires saving at shutdown, we cannot 
             * end this thread on the condition of GameServer.IsRunning.
             */
            while (true)
            {
                try
                {
                    /*
                     * We need to sleep the thread for a few milliseconds, 
                     * otherwise the thread would kill the CPU usage.
                     */
                    Thread.Sleep(10);

                    Character character = this.toSave.Take();
                    if (this.accountSaver.Save(character))
                    {
                        Program.Logger.WriteDebug("Saved " + character.Name + "'s game.");
                    }
                    else
                    {
                        Program.Logger.WriteWarn("Could not save " + character.Name + "'s game.");
                    }
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
                catch (Exception ex)
                {
                    Program.Logger.WriteException(ex);
                }
            }
        }

        /// <summary>
        /// Adds a character to the queue to be saved.
        /// </summary>
        /// <param name="character">The charcter to add.</param>
        public void Add(Character character)
        {
            this.toSave.Add(character);
        }
        #endregion Methods

    }
}
