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
using System.Data;
using System.Linq;
using System.Text;

using JoltEnvironment.Storage.Sql;
using RuneScape.Utilities.Indexing;

namespace RuneScape.Model.Npcs
{
    /// <summary>
    /// Defines management for npcs.
    /// </summary>
    public class NpcManager
    {
        #region Fields
        /// <summary>
        /// A collection of npc definitions.
        /// </summary>
        private Dictionary<short, NpcDefinition> definitions;
        /// <summary>
        /// A collection of spawned npcs.
        /// </summary>
        private List<Npc> npcSpawns = new List<Npc>();

        /// <summary>
        /// A slot manager which manages the npc's client indexing.
        /// </summary>
        private NpcSlotManager slotManager = new NpcSlotManager(5000);
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the spawned npcs.
        /// </summary>
        public List<Npc> Spawns { get { return this.npcSpawns; } }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs the npc manager.
        /// </summary>
        public NpcManager()
        {
            // Load definitions.
            this.definitions = NpcDefinition.Load();

            // Load spawns.
            try
            {
                DataRow[] rows = null;

                // Get all spawns.
                using (SqlDatabaseClient client = GameServer.Database.GetClient())
                {
                    DataTable table = client.ReadDataTable("SELECT * FROM npc_spawns;");

                    if (table != null)
                    {
                        rows = table.Select();
                    }
                }

                // If there is any spawns in the database, spawn them in game.
                if (rows != null)
                {
                    foreach (DataRow row in rows)
                    {
                        //DataRow row = rows[0];
                        short id = (short)row[1];
                        Location originalCoords = Location.Create((short)row[2], (short)row[3], (byte)row[4]);
                        WalkType walkType = (WalkType)row[5];
                        Location minRange = Location.Create((short)row[6], (short)row[7], (byte)row[8]);
                        Location maxRange = Location.Create((short)row[9], (short)row[10], (byte)row[11]);

                        string[] messages = null;
                        if (row[12] is string)
                        {
                            messages = ((string)row[12]).Split(';');
                        }

                        Npc npc = new Npc(id, originalCoords, minRange, maxRange, walkType, definitions[id], messages);
                        if (this.slotManager.ReserveSlot(npc))
                        {
                            this.npcSpawns.Add(npc);
                        }
                    }

                    Program.Logger.WriteInfo("Loaded " + this.definitions.Count + " npc definitions, and " + this.npcSpawns.Count + " spawn(s).");
                }
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
        }
        #endregion Cosntructors

        #region Methods
        /// <summary>
        /// Gets the definition of the npc by the specified id.
        /// </summary>
        /// <param name="id">The cache index id of the npc.</param>
        /// <returns>Returns the npc definition set.</returns>
        public NpcDefinition GetDefinition(short id)
        {
            return definitions[id];
        }

        public void RemoveNpc(Npc npc)
        {
        }

        public void RegisterNpc(Npc npc)
        {
            if (this.slotManager.ReserveSlot(npc))
            {
                this.npcSpawns.Add(npc);
            }
        }
        #endregion Methods
    }
}
