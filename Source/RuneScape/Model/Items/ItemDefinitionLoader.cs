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

namespace RuneScape.Model.Items
{
    /// <summary>
    /// Represents a item definition.
    /// </summary>
    public partial class ItemDefinition
    {
        #region Fields
        /// <summary>
        /// Contains all definitions loaded from database.
        /// </summary>
        private static ItemDefinition[] definitions;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Get an item definition from the array specified by the id.
        /// </summary>
        /// <param name="itemId">The item id of the item definitions wanted.</param>
        /// <returns>Returns a RuneScape.Model.Items.ItemDefinition instance 
        /// containing specified item definitions.</returns>
        public static ItemDefinition Get(short itemId)
        {
            if (itemId > 0 && itemId < definitions.Length)
            {
                return definitions[itemId];
            }
            return definitions[0];
        }

        /// <summary>
        /// Loads the item defitions from mysql database and stores in a local array.
        /// </summary>
        /// <param name="destination">The array to store data to.</param>
        public static void Load()
        {
            // The largest item id value in the item_definitions table.
            int largestValue = 0;
            // The defitions storage.
            ItemDefinition[] definitions = null;
            // The raw definitions from mysql database.
            DataRow[] rows = null;

            try
            {
                using (SqlDatabaseClient client = GameServer.Database.GetClient())
                {
                    // Find the highest id value so we can make an array based on that value.
                    largestValue = (short)client.ExecuteQuery("SELECT MAX(id) FROM item_definitions;");
                    // Get raw definitions.
                    
                    rows = client.ReadDataTable("SELECT * FROM item_definitions").Select();
                }   

                /*
                 * Create the array with specified largest value + 1 (because items start at 0).
                 */ 
                definitions = new ItemDefinition[largestValue + 1];

                // Parse definitions.
                foreach (DataRow row in rows)
                {
                    // General definitions.
                    short id = Convert.ToInt16(row[0]);
                    string name = Convert.ToString(row[1]);
                    string examine = Convert.ToString(row[2]);
                    short equipId = Convert.ToInt16(row[3]);
                    bool stackable = Convert.ToBoolean(row[4]);
                    bool tradable = Convert.ToBoolean(row[5]);
                    bool noted = Convert.ToBoolean(row[6]);
                    short noteId = Convert.ToInt16(row[7]);

                    // Price definitions.
                    int[] prices = new int[3];
                    prices[0] = Convert.ToInt32(row[8]); // Minimum price.
                    prices[1] = Convert.ToInt32(row[9]); // Normal price.
                    prices[2] = Convert.ToInt32(row[10]); // Maximum price.

                    // Bonus definitions.
                    short[] bonuses = new short[16];
                    bonuses[0] = Convert.ToInt16(row[11]); // Attack Stab Bonus
                    bonuses[1] = Convert.ToInt16(row[12]); // Attack Slash Bonus
                    bonuses[2] = Convert.ToInt16(row[13]); // Attack Crush Bonus
                    bonuses[3] = Convert.ToInt16(row[14]); // Attack Magic Bonus
                    bonuses[4] = Convert.ToInt16(row[15]); // Attack Ranged Bonus
                    bonuses[5] = Convert.ToInt16(row[16]); // Defence Stab Bonus
                    bonuses[6] = Convert.ToInt16(row[17]); // Defence Slash Bonus
                    bonuses[7] = Convert.ToInt16(row[18]); // Defence Crush Bonus
                    bonuses[8] = Convert.ToInt16(row[19]); // Defence Magic Bonus
                    bonuses[9] = Convert.ToInt16(row[20]); // Defence Ranged Bonus
                    bonuses[10] = Convert.ToInt16(row[21]); // Defence Summoning Bonus
                    bonuses[11] = Convert.ToInt16(row[22]); // Strength Bonus
                    bonuses[12] = Convert.ToInt16(row[23]); // Prayer Bonus

                    // Configure definition.
                    definitions[id] = new ItemDefinition(id, name, examine, equipId, noted, noteId, stackable, tradable, prices, bonuses);
                }

                /*
                 * This must be done so no errors are caused if there 
                 * are null items. If the player does somehow spawn 
                 * a null item, they'll recieve drawf remains instead.
                 */ 
                for (int i = 0; i < definitions.Length; i++)
                {
                    if (definitions[i] == null)
                    {
                        definitions[i] = definitions[0];
                    }
                }

                //Program.Logger.WriteInfo("Loaded " + rows.Length + " item definitions.");
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
            ItemDefinition.definitions = definitions;
        }

        /// <summary>
        /// Loads the item defitions from mysql database and stores in a local array.
        /// </summary>
        /// <param name="destination">The array to store data to.</param>
        public static ItemDefinition[] LoadEquips()
        {
            // The largest item id value in the item_definitions table.
            int largestValue = 0;
            // The defitions storage.
            ItemDefinition[] definitions = null;
            // The raw definitions from mysql database.
            DataRow[] rows = null;

            try
            {
                using (SqlDatabaseClient client = GameServer.Database.GetClient())
                {
                    // Find the highest id value so we can make an array based on that value.
                    largestValue = (short)client.ExecuteQuery("SELECT MAX(id) FROM item_definitions;");
                    // Get raw definitions.

                    rows = client.ReadDataTable("SELECT * FROM item_definitions WHERE equip_id > -1").Select();
                }

                /*
                 * Create the array with specified largest value + 1 (because items start at 0).
                 */
                definitions = new ItemDefinition[largestValue + 1];

                // Parse definitions.
                foreach (DataRow row in rows)
                {
                    // General definitions.
                    short id = Convert.ToInt16(row[0]);
                    string name = Convert.ToString(row[1]);
                    string examine = Convert.ToString(row[2]);
                    short equipId = Convert.ToInt16(row[3]);
                    bool stackable = Convert.ToBoolean(row[4]);
                    bool tradable = Convert.ToBoolean(row[5]);
                    bool noted = Convert.ToBoolean(row[6]);
                    short noteId = Convert.ToInt16(row[7]);

                    // Price definitions.
                    int[] prices = new int[3];
                    prices[0] = Convert.ToInt32(row[8]); // Minimum price.
                    prices[1] = Convert.ToInt32(row[9]); // Normal price.
                    prices[2] = Convert.ToInt32(row[10]); // Maximum price.

                    // Bonus definitions.
                    short[] bonuses = new short[16];
                    bonuses[0] = Convert.ToInt16(row[11]); // Attack Stab Bonus
                    bonuses[1] = Convert.ToInt16(row[12]); // Attack Slash Bonus
                    bonuses[2] = Convert.ToInt16(row[13]); // Attack Crush Bonus
                    bonuses[3] = Convert.ToInt16(row[14]); // Attack Magic Bonus
                    bonuses[4] = Convert.ToInt16(row[15]); // Attack Ranged Bonus
                    bonuses[5] = Convert.ToInt16(row[16]); // Defence Stab Bonus
                    bonuses[6] = Convert.ToInt16(row[17]); // Defence Slash Bonus
                    bonuses[7] = Convert.ToInt16(row[18]); // Defence Crush Bonus
                    bonuses[8] = Convert.ToInt16(row[19]); // Defence Magic Bonus
                    bonuses[9] = Convert.ToInt16(row[20]); // Defence Ranged Bonus
                    bonuses[10] = Convert.ToInt16(row[21]); // Defence Summoning Bonus
                    bonuses[11] = Convert.ToInt16(row[22]); // Strength Bonus
                    bonuses[12] = Convert.ToInt16(row[23]); // Prayer Bonus

                    // Configure definition.
                    definitions[id] = new ItemDefinition(id, name, examine, equipId, noted, noteId, stackable, tradable, prices, bonuses);
                }

                /*
                 * This must be done so no errors are caused if there 
                 * are null items. If the player does somehow spawn 
                 * a null item, they'll recieve drawf remains instead.
                 */
                for (int i = 0; i < definitions.Length; i++)
                {
                    if (definitions[i] == null)
                    {
                        definitions[i] = definitions[0];
                    }
                }

                //Program.Logger.WriteInfo("Loaded " + rows.Length + " item definitions.");
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
