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
using System.Data;

using JoltEnvironment.Storage.Sql;
using RuneScape.Communication.Messages;
using RuneScape.Database.Offence;
using RuneScape.Model.Characters;
using RuneScape.Model.Maps;
using RuneScape.Model.Items;
using RuneScape.Utilities.Filtering;

namespace RuneScape.Model
{
    /// <summary>
    /// Represents a single world.
    /// </summary>
    public class GameWorld
    {
        #region Fields
        /// <summary>
        /// A manager for dealing with character related content.
        /// </summary>
        private CharacterManager characterManager = new CharacterManager();
        /// <summary>
        /// A manager for dealing with offence related content.
        /// </summary>
        private OffenceManager offenceManager = new OffenceManager();
        /// <summary>
        /// A manager for dealing with packet related content.
        /// </summary>
        private PacketManager packetManager = new PacketManager();
        /// <summary>
        /// A manager for dealing with map related content.
        /// </summary>
        private MapManager mapManager = new MapManager();
        /// <summary>
        /// A maanger for dealing with item related content.
        /// </summary>
        private ItemManager itemManager = new ItemManager();
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the world's id.
        /// 
        ///     <para>This id is used to find out configurations
        ///     for this world via the database.</para>
        /// </summary>
        public uint Id { get; private set; }
        /// <summary>
        /// Gets the world's name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the welcome message displayed to every character at login.
        /// </summary>
        public string WelcomeMessage { get; private set; }
        /// <summary>
        /// Gets the default spawn point.
        /// 
        ///     <para>When a character dies, or teleports home, they are sent to this area.</para>
        /// </summary>
        public Location SpawnPoint { get; private set; }
        /// <summary>
        /// The message of the week, displayed at the welcome screen.
        /// </summary>
        public string Motw { get; private set; }
        /// <summary>
        /// The rate at which users gain experience.
        /// </summary>
        public int ExperienceRate { get; private set; }
        /// <summary>
        /// Gets or sets whether account creation via the client is enabled.
        /// </summary>
        public bool AccountCreationEnabled { get; private set; }

        /// <summary>
        /// Gets the character manager.
        /// </summary>
        public CharacterManager CharacterManager { get { return this.characterManager; } }
        /// <summary>
        /// Gets the offense manager.
        /// </summary>
        public OffenceManager OffenseManager { get { return this.offenceManager; } }
        /// <summary>
        /// Gets the packet manager.
        /// </summary>
        public PacketManager PacketManager { get { return this.packetManager; } }
        /// <summary>
        /// Gets the map manager.
        /// </summary>
        public MapManager MapManager { get { return this.mapManager; } }
        /// <summary>
        /// Gets the item manager.
        /// </summary>
        public ItemManager ItemManager { get { return this.itemManager; } }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new world.
        /// 
        ///     <para>With the given world id, it will find the configurations 
        ///     set in the database, and configure this world using them. This 
        ///     allows you to create multiple worlds, yet connect to the same 
        ///     world, with the same players etc.</para>
        /// </summary>
        /// <param name="worldId">The id of this world.</param>
        public GameWorld(uint worldId)
        {
            // World initializations
            this.Id = worldId;

            DataRow vars;
            // Grab the rest of the world-specific variables from the database.
            using (SqlDatabaseClient client = GameServer.Database.GetClient())
            {
                client.AddParameter("id", worldId);
                vars = client.ReadDataRow(
                    "SELECT * FROM worlds WHERE world_id = @id LIMIT 1;" + 
                    "UPDATE characters SET online = '0' WHERE online = '1';");
            }

            this.Name = (string)vars["world_name"];
            this.WelcomeMessage = (string)vars["welcome_message"];
            string[] coords = vars["spawn_point"].ToString().Split(',');
            this.SpawnPoint = Location.Create(
                short.Parse(coords[0]),
                short.Parse(coords[1]),
                byte.Parse(coords[2]));
            this.Motw = (string)vars["motw"];
            this.ExperienceRate = (int)vars["exp_rate"];
            this.AccountCreationEnabled = true;

            // Load bad words.
            BadWords.Load();
        }
        #endregion Constructors

        #region Methods
        #endregion Methods
    }
}
