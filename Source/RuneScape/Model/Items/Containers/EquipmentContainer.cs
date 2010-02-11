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

using RuneScape.Communication.Messages;
using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Model.Characters;

namespace RuneScape.Model.Items.Containers
{
    /// <summary>
    /// A container for character inventories.
    /// </summary>
    public class EquipmentContainer : Container
    {
        #region Fields
        /// <summary>
        /// The size of the inventory container..
        /// 
        ///     <remarks>This value cannot be changed, because 
        ///     this is the maximum amount required for the 
        ///     runescape client.</remarks>
        /// </summary>
        public const int Size = 14;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets or sets whether the currently equipted weapon has a special attack.
        /// </summary>
        public bool SpecialWeapon { get; private set; }

        /// <summary>
        /// The stand animation according to the equipted weapon.
        /// </summary>
        public short StandAnimation { get; private set; }
        /// <summary>
        /// The walk animation according to the equipted weapon.
        /// </summary>
        public short WalkAnimation { get; private set; }
        /// <summary>
        /// The run animation according to the equipted weapon.
        /// </summary>
        public short RunAnimation { get; private set; }

        public byte AttackStyle { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new equipment container.
        /// </summary>
        public EquipmentContainer(Character character)
            : base(EquipmentContainer.Size, ContainerType.Standard, character)
        {
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Sets a certain slot's item.
        /// </summary>
        /// <param name="slot">The slot to set.</param>
        /// <param name="item">The item to set on slot.</param>
        /// <returns>Returns true if successfully set; false if not.</returns>
        public bool Set(int slot, Item item)
        {
            try
            {
                if (slot == 3)
                {
                    this.Character.Preferences.AttackStyle = 1;
                    this.Character.Session.SendData(new ConfigPacketComposer(43, 0).Serialize());
                }
                this[slot] = item;
                Refresh();
                return true;
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
                return false;
            }
        }

        /// <summary>
        /// Refreshes the character's equipment appearance.
        /// </summary>
        public override void Refresh()
        {
            UpdateWeapon();
            this.Character.UpdateFlags.AppearanceUpdateRequired = true;
            this.Character.Session.SendData(new InterfaceItemsPacketComposer(387, 28, 94, this).Serialize());
            this.Character.Bonuses.Refresh(false);
        }

        /// <summary>
        /// Updates the attack style tab interface according to the 
        /// </summary>
        public void UpdateWeapon()
        {
            // Update animations according to equipted weapon.
            this.StandAnimation = UpdateStandAnimation();
            this.WalkAnimation = UpdateWalkAnimation();
            this.RunAnimation = UpdateRunAnimation();

            // The character is unarmed (no weapons equipted).
            if (this[3] == null)
            {
                Frames.SendTab(this.Character, 73, 92);
                this.Character.Session.SendData(new StringPacketComposer("Unarmed", 92, 0).Serialize());
                this.SpecialWeapon = false;
                return;
            }

            short itemId = this[EquipmentSlot.Weapon].Id;
            string weaponName = this[EquipmentSlot.Weapon].Name;

            // Try to update the interface.
            if (EquipmentItems.WeaponInterfaces.ContainsKey(itemId))
            {
                short childId = EquipmentItems.WeaponInterfaces[itemId];
                Frames.SendTab(this.Character, (short)(this.Character.Preferences.Resized ? 87 : 73), childId);
                this.Character.Session.SendData(new StringPacketComposer(weaponName, childId, 0).Serialize());
            }
            else
            {
                Frames.SendTab(this.Character, (short)(this.Character.Preferences.Resized ? 87 : 73), 82);
                this.Character.Session.SendData(new StringPacketComposer(weaponName, 82, 0).Serialize());
            }

            // Update specials interface.
            UpdateSpecials();
        }

        /// <summary>
        /// Updates the attack tab to suit any special weapon needs.
        /// </summary>
        private void UpdateSpecials()
        {
            short itemId = this[EquipmentSlot.Weapon].Id;
            this.SpecialWeapon = false;

            if (EquipmentItems.SpecialWeapons.ContainsKey(itemId))
            {
                byte[] data = EquipmentItems.SpecialWeapons[itemId];
                this.Character.Session.SendData(new InterfaceConfigPacketComposer(data[0], data[1], false).Serialize());
                this.SpecialWeapon = true;
            }
        }

        /// <summary>
        /// Updates the stand animation for the character suitable for the equipt weapon.
        /// </summary>
        /// <returns>Returns a 16-bit integer containing the updated stand animation value.</returns>
        private short UpdateStandAnimation()
        {
            if (this[EquipmentSlot.Weapon] == null)
            {
                return 0x328;
            }

            short itemId = this[EquipmentSlot.Weapon].Id;
            if (EquipmentItems.WeaponStandAnimations.ContainsKey(itemId))
            {
                return EquipmentItems.WeaponStandAnimations[itemId];
            }
            else
            {
                return 0x328;
            }
        }

        /// <summary>
        /// Updates the walk animation for the character suitable for the equipt weapon.
        /// </summary>
        /// <returns>Returns a 16-bit integer containing the updated walk animation value.</returns>
        private short UpdateWalkAnimation()
        {
            if (this[EquipmentSlot.Weapon] == null)
            {
                return 0x333;
            }

            short itemId = this[EquipmentSlot.Weapon].Id;
            if (EquipmentItems.WeaponWalkAnimations.ContainsKey(itemId))
            {
                return EquipmentItems.WeaponWalkAnimations[itemId];
            }
            else
            {
                return 0x333;
            }
        }

        /// <summary>
        /// Updates the run animation for the character suitable for the equipt weapon.
        /// </summary>
        /// <returns>Returns a 16-bit integer containing the updated run animation value.</returns>
        private short UpdateRunAnimation()
        {
            if (this[3] == null)
            {
                return 0x338;
            }

            short itemId = this[EquipmentSlot.Weapon].Id;
            if (EquipmentItems.WeaponRunAnimations.ContainsKey(itemId))
            {
                return EquipmentItems.WeaponRunAnimations[itemId];
            }
            else
            {
                return 0x338;
            }
        }
        #endregion Methods
    }
}