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
using System.Threading;

using RuneScape.Model.Characters;

namespace RuneScape.Workers
{
    /// <summary>
    /// A worker that auto saves all characters every so minutes.
    /// </summary>
    public class AutoSaveWorker : Worker
    {
        #region Fields
        /// <summary>
        /// The interval between each iteration.
        /// </summary>
        public const int Interval = 600000;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs a autosave worker with normal priority.
        /// </summary>
        public AutoSaveWorker() : base("AutoSave Worker", ThreadPriority.Normal) { }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Thread's process.
        /// </summary>
        protected override void Run()
        {
            try
            {
                while (GameServer.IsRunning)
                {
                    Thread.Sleep(AutoSaveWorker.Interval);
                    SaveAll();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
            finally
            {
                Abort();
            }
        }

        /// <summary>
        /// Saves all the characters.
        /// </summary>
        public void SaveAll()
        {
            List<Character> chars = new List<Character>(GameEngine.World.CharacterManager.Characters.Values);
            chars.ForEach((c) => 
            {
                GameEngine.AccountWorker.Add(c);
            });
        }
        #endregion Methods
    }
}
