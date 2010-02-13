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
using System.Threading;

using JoltEnvironment.Service.Events;
using RuneScape.Content;
using RuneScape.Model;
using RuneScape.Workers;

namespace RuneScape
{
    /// <summary>
    /// Presents a provider for workers and managers.
    /// </summary>
    public static class GameEngine
    {
        #region Fields
        /// <summary>
        /// Management for event services.
        /// </summary>
        private static EventManager events = new EventManager();
        /// <summary>
        /// Defines a single RuneScape world. 
        /// 
        ///     <para>Provides management for models.</para>
        /// </summary>
        private static GameWorld world;
        /// <summary>
        /// Management for runescape content.
        /// </summary>
        private static GameContent content;

        /// <summary>
        /// The login worker.
        /// </summary>
        private static LoginWorker loginWorker = new LoginWorker();
        /// <summary>
        /// The major updates worker.
        /// </summary>
        private static MajorWorker majorWorker = new MajorWorker();
        /// <summary>
        /// The account worker.
        /// </summary>
        private static AccountWorker accountWorker = new AccountWorker();
        /// <summary>
        /// The autosave worker.
        /// </summary>
        private static AutoSaveWorker autosaveWorker = new AutoSaveWorker();
        /// <summary>
        /// The items worker.
        /// </summary>
        private static ItemsWorker itemsWorker = new ItemsWorker();
        #endregion Fields

        #region Properites
        /// <summary>
        /// Gets the event manager.
        /// </summary>
        public static EventManager Events { get { return events; } }
        /// <summary>
        /// Gets the game world.
        /// </summary>
        public static GameWorld World { get { return world; } }
        /// <summary>
        /// Gets the game content.
        /// </summary>
        public static GameContent Content { get { return content; } }

        /// <summary>
        /// Gets the login worker.
        /// </summary>
        public static LoginWorker LoginWorker { get { return loginWorker; } }
        /// <summary>
        /// Gets the account worker.
        /// </summary>
        public static AccountWorker AccountWorker { get { return accountWorker; } }
        /// <summary>
        /// Gets the items worker.
        /// </summary>
        public static ItemsWorker ItemsWorker { get { return itemsWorker; } }
        #endregion Properites

        #region Methods
        /// <summary>
        /// Initialize the gaming engine environment.
        /// </summary>
        public static void Initialize()
        {
            Program.Logger.WriteInfo("Initializing the game engine and world...");

            // Create a new world using the given world id.
            world = new GameWorld(GameServer.Configuration["World.Id"]);
            content = new GameContent();

            // Start the workers.
            loginWorker.Start();
            majorWorker.Start();
            accountWorker.Start();
            autosaveWorker.Start();
            itemsWorker.Start();
        }

        /// <summary>
        /// Terminates the engine, world and the workers.
        /// </summary>
        public static void Terminate()
        {
        }
        #endregion Methods
    }
}
