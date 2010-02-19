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

using RuneScape.Model.Items;

namespace RuneScape.Workers
{
    /// <summary>
    /// A worker processing 
    /// </summary>
    public class ItemsWorker : Worker
    {
        #region Fields
        /// <summary>
        /// The interval between each thread iteration.
        /// </summary>
        public const int Interval = 1000;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructs an items worker with normal priority.
        /// </summary>
        public ItemsWorker() : base("Item Worker", ThreadPriority.Normal) { }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Thread's process.
        /// </summary>
        protected override void Run()
        {
            while (GameServer.IsRunning)
            {
                try
                {
                    Thread.Sleep(ItemsWorker.Interval);

                    /*
                     * Process the ground items.
                     */ 
                    GameEngine.World.ItemManager.GroundItems.Process();
                }
                catch (Exception ex)
                {
                    Program.Logger.WriteException(ex);
                }
            }
            Abort();
        }
        #endregion Methods
    }
}
