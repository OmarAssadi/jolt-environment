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

namespace RuneScape.Model.Npcs
{    
    /// <summary>
    /// Represents a npc's definition.
    /// </summary>
    public partial class NpcDefinition
    {
        #region Methods
        public static Dictionary<short, NpcDefinition> Load()
        {
            DataRow[] rows = null;
            Dictionary<short, NpcDefinition> definitions = new Dictionary<short, NpcDefinition>();

            try
            {
                // Get all npc definitions.
                using (SqlDatabaseClient client = GameServer.Database.GetClient())
                {
                    rows = client.ReadDataTable("SELECT * FROM npc_definitions").Select();
                }

                // Parse the definitions.
                foreach (DataRow row in rows)
                {
                    short id = (short)row[0];
                    string name = (string)row[1];
                    string examine = (string)row[2];
                    byte respawn = (byte)row[3];
                    byte combat = (byte)row[4];
                    byte hitpoints = (byte)row[5];
                    byte maxHit = (byte)row[6];
                    byte attackSpeed = (byte)row[7];
                    short attackAnim = (short)row[8];
                    short defenceAmin = (short)row[9];
                    short deathAnim = (short)row[10];

                    NpcDefinition definition = new NpcDefinition(id, name, examine, respawn, combat, hitpoints, 
                        maxHit, attackSpeed, attackAnim, defenceAmin, deathAnim);

                    definitions.Add(id, definition);
                }

                Program.Logger.WriteInfo("Loaded " + rows.Length + " npc definitions.");
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }

            return definitions;
        }
        #endregion Methods
    }
}
