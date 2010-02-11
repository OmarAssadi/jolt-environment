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

using JoltEnvironment.Storage.Sql;
using RuneScape.Model.Characters;
using RuneScape.Model.Items.Containers;

namespace RuneScape.Database.Account
{
    /// <summary>
    /// Represents a saver that loads accounts from a mysql database.
    /// </summary>
    public class SqlAccountSaver : IAccountSaver
    {
        #region Methods
        /// <summary>
        /// Saves the character to the mysql database.
        /// </summary>
        /// <param name="character">The character to save.</param>
        public bool Save(Character character)
        {
            try
            {
                using (SqlDatabaseClient client = GameServer.Database.GetClient())
                {
                    client.AddParameter("id", character.MasterId);

                    // Appearance.
                    client.AddParameter("gender", character.Appearance.Gender);
                    client.AddParameter("head", character.Appearance.Head);
                    client.AddParameter("chest", character.Appearance.Torso);
                    client.AddParameter("arms", character.Appearance.Arms);
                    client.AddParameter("hands", character.Appearance.Wrist);
                    client.AddParameter("legs", character.Appearance.Legs);
                    client.AddParameter("feet", character.Appearance.Feet);
                    client.AddParameter("beard", character.Appearance.Beard);
                    client.AddParameter("hair_color", character.Appearance.HairColor);
                    client.AddParameter("torso_color", character.Appearance.TorsoColor);
                    client.AddParameter("leg_color", character.Appearance.LegColor);
                    client.AddParameter("feet_color", character.Appearance.FeetColor);
                    client.AddParameter("skin_color", character.Appearance.SkinColor);

                    // Preferences.
                    client.AddParameter("coord_x", character.Location.X);
                    client.AddParameter("coord_y", character.Location.Y);
                    client.AddParameter("coord_z", character.Location.Z);
                    client.AddParameter("run_energy", character.WalkingQueue.RunEnergy);

                    // Containers.
                    client.AddParameter("inv", character.Inventory.Serialize());
                    client.AddParameter("eqp", character.Equipment.Serialize());

                    string query = "UPDATE characters SET gender=@gender,head=@head,chest=@chest,arms=@arms,hands=@hands,legs=@legs,feet=@feet,beard=@beard,hair_color=@hair_color,torso_color=@torso_color,leg_color=@leg_color,feet_color=@feet_color,skin_color=@skin_color, coord_x=@coord_x,coord_y=@coord_y,coord_z=@coord_z,run_energy=@run_energy,inventory_items=@inv,equipment_items=@eqp WHERE id=@id;";
                    client.ExecuteUpdate(query);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
                return false;
            }
        }
        #endregion Methods
    }
}
