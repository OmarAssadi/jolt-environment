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
        /// <summary>
        /// The different slots for equipment
        /// </summary>
        public const int Slot_Hat = 0,
            Slot_Cape = 1,
            Slot_Amulet = 2,
            Slot_Weapon = 3,
            Slot_Chest = 4,
            Slot_Shield = 5,
            Slot_Legs = 7,
            Slot_Hands = 9,
            Slot_Feet = 10,
            Slot_Ring = 12,
            Slot_Arrows = 13;
        /// <summary>
        /// A collection of cape names.
        /// </summary>
        private static readonly string[] CAPES = { "cape", "Cape" };
        /// <summary>
        /// A collection of hat names.
        /// </summary>
        private static readonly string[] HATS = { "Bandana and eyepatch", "wig", "helm", "hood", "coif", "Coif", "hat", "partyhat", "Hat", "full helm (t)", "full helm (g)", "hat (t)", "hat (g)", "cav", "boater", "helmet", "mask", "Helm of neitiznot" };
        /// <summary>
        /// A collection of boot names.
        /// </summary>
        private static readonly string[] BOOTS = { "boots", "Boots" };
        /// <summary>
        /// A collection of glove names
        /// </summary>
        private static readonly string[] GLOVES = { "Crabclaw and hook", "gloves", "gauntlets", "Gloves", "vambraces", "vamb", "bracers" };
        /// <summary>
        /// A collection of shield names.
        /// </summary>
        private static readonly string[] SHIELDS = { "kiteshield", "sq shield", "Toktz-ket", "books", "book", "kiteshield (t)", "kiteshield (g)", "kiteshield(h)", "defender", "shield" };
        /// <summary>
        /// A collection of amulet names.
        /// </summary>
        private static readonly string[] AMULETS = { "amulet", "necklace", "Amulet of" };
        /// <summary>
        /// A collection of arrows names
        /// </summary>
        private static readonly string[] ARROWS = { "arrow", "arrows", "arrow(p)", "arrow(+)", "arrow(s)", "bolt", "Bolt rack", "Opal bolts", "Dragon bolts" };
        /// <summary>
        /// A collection of rings.
        /// </summary>
        private static readonly string[] RINGS = { "ring" };
        /// <summary>
        /// A collection of body armour names.
        /// </summary>
        private static readonly string[] BODY = { "platebody", "chainbody", "robetop", "leathertop", "platemail", "top", "brassard", "Robe top", "body", "platebody (t)", "platebody (g)", "body(g)", "body_(g)", "chestplate", "torso", "shirt" };
        /// <summary>
        /// A collection of leg armour names.
        /// </summary>
        private static readonly string[] LEGS = { "robe", "platelegs", "plateskirt", "skirt", "bottoms", "chaps", "platelegs (t)", "platelegs (g)", "bottom", "skirt", "skirt (g)", "skirt (t)", "chaps (g)", "chaps (t)", "tassets", "legs" };
        /// <summary>
        /// A collection of weapon names
        /// </summary>
        private static readonly string[] WEAPONS = { "Banner", "Excalibur", "scimitar","longsword","sword","longbow","shortbow","dagger","mace","halberd","spear",
	    "Abyssal whip","axe","flail","crossbow","Torags hammers","dagger(p)","dagger(+)","dagger(s)","spear(p)","spear(+)",
	    "spear(s)","spear(kp)","maul","dart","dart(p)","javelin","javelin(p)","knife","knife(p)","Longbow","Shortbow",
	    "Crossbow","Toktz-xil","Toktz-mej","Tzhaar-ket","staff","Staff","godsword","c'bow","Crystal bow","Dark bow"};
        /// <summary>
        /// Covers your arms.
        /// </summary>
        private static readonly string[] FULL_BODY = { "top", "shirt", "platebody", "Ahrims robetop", "Karils leathertop", "brassard", "Robe top", "robetop", "platebody (t)", "platebody (g)", "chestplate", "torso" };
        /// <summary>
        /// Covers your head but not beard.
        /// </summary>
        private static readonly string[] FULL_HAT = { "Bandana and eyepatch",  "wig", "med helm", "coif", "Dharoks helm", "hood", "Initiate helm", "Coif", "Helm of neitiznot" };
        /// <summary>
        /// Covers your head.
        /// </summary>
        private static readonly string[] FULL_MASK = { "heraldic helm", "full helm", "mask", "Veracs helm", "Guthans helm", "Torags helm", "Karils coif", "full helm (t)", "full helm (g)", "mask" };
        #endregion Fields

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
        /// Refreshes the equipment items.
        /// </summary>
        public override void Refresh()
        {
            this.character.UpdateFlags.AppearanceUpdateRequired = true;
            this.character.Session.SendData(new InterfaceItemsPacketComposer(387, 28, 94, this).Serialize());
            //TODO: Bonuses
        }

        /// <summary>
        /// Sets the weapon's interface in the attack tab.
        /// </summary>
        public void SetWeapon()
        {
            if (this.character.Equipment[3] == null)
            {
                Frames.SendTab(character, 73, 92);
                this.character.Session.SendData(new StringPacketComposer("Unarmed", 92, 0).Serialize());
                //specialWeapon = false;
                return;
            }
            String weapon = this.character.Equipment[3].Definition.Name;
            if (weapon.Equals("Abyssal whip"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 93);
                this.character.Session.SendData(new StringPacketComposer(weapon, 93, 0).Serialize());
            }
            else if (weapon.Equals("Granite maul") || weapon.Equals("Tzhaar-ket-om") || weapon.Equals("Torags hammers"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 76);
                this.character.Session.SendData(new StringPacketComposer(weapon, 76, 0).Serialize());
            }
            else if (weapon.Equals("Veracs flail") || weapon.EndsWith("mace"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 88);
                this.character.Session.SendData(new StringPacketComposer(weapon, 88, 0).Serialize());
            }
            else if (weapon.EndsWith("crossbow") || weapon.EndsWith(" c'bow"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 79);
                this.character.Session.SendData(new StringPacketComposer(weapon, 79, 0).Serialize());
            }
            else if (weapon.EndsWith("bow") || weapon.EndsWith("bow full") || weapon.Equals("Seercull"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 77);
                this.character.Session.SendData(new StringPacketComposer(weapon, 77, 0).Serialize());
            }
            else if (weapon.StartsWith("Staff") || weapon.EndsWith("staff") || weapon.Equals("Toktz-mej-tal"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 90);
                this.character.Session.SendData(new StringPacketComposer(weapon, 90, 0).Serialize());
            }
            else if (weapon.EndsWith("dart") || weapon.EndsWith("knife") || weapon.EndsWith("javelin") || weapon.EndsWith("thrownaxe") || weapon.Equals("Toktz-xil-ul"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 91);
                this.character.Session.SendData(new StringPacketComposer(weapon, 91, 0).Serialize());
            }
            else if (weapon.EndsWith("dagger") || weapon.EndsWith("dagger(s)") || weapon.EndsWith("dagger(+)") || weapon.EndsWith("dagger(p)"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 89);
                this.character.Session.SendData(new StringPacketComposer(weapon, 89, 0).Serialize());
            }
            else if (weapon.EndsWith("pickaxe"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 83);
                this.character.Session.SendData(new StringPacketComposer(weapon, 83, 0).Serialize());
            }
            else if (weapon.EndsWith("axe") || weapon.EndsWith("battleaxe"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 75);
                this.character.Session.SendData(new StringPacketComposer(weapon, 75, 0).Serialize());
            }
            else if (weapon.EndsWith("halberd"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 84);
                this.character.Session.SendData(new StringPacketComposer(weapon, 84, 0).Serialize());
            }
            else if (weapon.EndsWith("spear") || weapon.Equals("Guthans warspear"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 85);
                this.character.Session.SendData(new StringPacketComposer(weapon, 85, 0).Serialize());
            }
            else if (weapon.EndsWith("claws"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 78);
                this.character.Session.SendData(new StringPacketComposer(weapon, 78, 0).Serialize());
            }
            else if (weapon.EndsWith("2h sword") || weapon.EndsWith("godsword") || weapon.Equals("Saradomin sword"))
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 81);
                this.character.Session.SendData(new StringPacketComposer(weapon, 81, 0).Serialize());
            }
            else
            {
                Frames.SendTab(character, (short)(character.Preferences.Hd ? 87 : 73), 82);
                this.character.Session.SendData(new StringPacketComposer(weapon, 82, 0).Serialize());
            }
            //TODO: Specials
        }

        /// <summary>
        /// Gets the appropriate stand animation for the weapon being wielded.
        /// </summary>
        public short StandAnimation
        {
            get
            {
                if (this[3] == null)
                {
                    return 0x328;
                }
                Item item = this[3];
                short id = item.Definition.Id;
                string weapon = item.Definition.Name;

                if (id == 4718)
                {
                    return 2065;
                }
                else if (id == 4755)
                {
                    return 2061;
                }
                else if (id == 4734)
                {
                    return 2074;
                }
                else if (id == 6528 || id == 1319)
                {
                    return 0x811;
                }
                else if (weapon.Equals("Saradomin staff") || weapon.Equals("Guthix staff") || weapon.Equals("Zamorak staff"))
                {
                    return 0x328;
                }
                else if (id == 4726 || weapon.EndsWith("spear") || weapon.EndsWith("halberd") || weapon.Contains("Staff") || weapon.Contains("staff") || id == 1305)
                {
                    return 809;
                }
                else if (weapon.EndsWith("2h sword") || weapon.EndsWith("godsword") || weapon.Equals("Saradomin sword"))
                {
                    return 7047;
                }
                else if (weapon.Equals("Abyssal whip"))
                {
                    return 10080;
                }
                else if (id == 4153)
                {
                    return 1662;
                }
                return 0x328;
            }
            set
            {
                this.StandAnimation = value;
            }
        }

        /// <summary>
        /// Gets the appropriate walking animation for the weapon being wielded.
        /// </summary>
        public short WalkAnimation
        {
            get
            {
                if (this[3] == null)
                {
                    return 0x333;
                }
                Item item = this[3];

                short id = item.Definition.Id;
                string weapon = item.Definition.Name;

                if (weapon.Equals("Saradomin staff") || weapon.Equals("Guthix staff") || weapon.Equals("Zamorak staff"))
                {
                    return 0x333;
                }
                else if (id == 4755)
                {
                    return 2060;
                }
                else if (id == 4734)
                {
                    return 2076;
                }
                else if (id == 4153)
                {
                    return 1663;
                }
                else if (weapon.Equals("Abyssal whip"))
                {
                    return 1660;
                }
                else if (id == 4718 || weapon.EndsWith("2h sword") || id == 6528 || weapon.EndsWith("godsword") || weapon.Equals("Saradomin sword"))
                {
                    return 7046;
                }
                else if (id == 4726 || weapon.Contains("spear") || weapon.EndsWith("halberd") || weapon.Contains("Staff") || weapon.Contains("staff"))
                {
                    return 1146;
                }
                return 0x333;
            }
            set
            {
                this.WalkAnimation = value;
            }
        }

        /// <summary>
        /// Gets the appropriate run animation for the weapon being wielded.
        /// </summary>
        public short RunAnimation
        {
            get
            {
                if (this[3] == null)
                {
                    return 0x338;
                }
                Item item = this[3];

                string weapon = item.Definition.Name;
                short id = item.Definition.Id;

                if (id == 4718 || weapon.EndsWith("2h sword") || id == 6528 || weapon.EndsWith("godsword") || weapon.Equals("Saradomin sword"))
                {
                    return 7039;
                }
                else if (weapon.Equals("Saradomin staff") || weapon.Equals("Guthix staff") || weapon.Equals("Zamorak staff"))
                {
                    return 0x338;
                }
                else if (id == 4755)
                {
                    return 1831;
                }
                else if (id == 4734)
                {
                    return 2077;
                }
                else if (id == 4726 || weapon.Contains("Spear") || weapon.EndsWith("halberd") || weapon.Contains("Staff") || weapon.Contains("staff"))
                {
                    return 1210;
                }
                else if (weapon.Equals("Abyssal whip"))
                {
                    return 1661;
                }
                else if (id == 4153)
                {
                    return 1664;
                }
                return 0x338;
            }
            set
            {
                this.RunAnimation = value;
            }
        }

        /// <summary>
        /// Returns whether or not an item requested to equip is two handed.
        /// </summary>
        /// <param name="definition">The details of the item being equipped.</param>
        /// <returns>Whether or not an item can be equipped.</returns>
        public static bool TwoHanded(ItemDefinition definition)
        {
            short itemId = definition.Id;
            string wepEquiped = definition.Name;
            if (itemId == 4212)
                return true;
            else if (itemId == 4214)
                return true;
            else if (wepEquiped.EndsWith("2h sword"))
                return true;
            else if (wepEquiped.EndsWith("longbow"))
                return true;
            else if (wepEquiped.Equals("Seercull"))
                return true;
            else if (wepEquiped.EndsWith("shortbow"))
                return true;
            else if (wepEquiped.EndsWith("Longbow"))
                return true;
            else if (wepEquiped.EndsWith("Shortbow"))
                return true;
            else if (wepEquiped.EndsWith("bow full"))
                return true;
            else if (wepEquiped.EndsWith("halberd"))
                return true;
            else if (wepEquiped.Equals("Granite maul"))
                return true;
            else if (wepEquiped.Equals("Karils crossbow"))
                return true;
            else if (wepEquiped.Equals("Torags hammers"))
                return true;
            else if (wepEquiped.Equals("Veracs flail"))
                return true;
            else if (wepEquiped.Equals("Dharoks greataxe"))
                return true;
            else if (wepEquiped.Equals("Guthans warspear"))
                return true;
            else if (wepEquiped.Equals("Tzhaar-ket-om"))
                return true;
            else if (wepEquiped.EndsWith("godsword"))
                return true;
            else if (wepEquiped.Equals("Saradomin sword"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns whether or not an item covers the chest.
        /// </summary>
        /// <param name="definition">The details of the item being equipped.</param>
        /// <returns>Whether or not the item is for the body.</returns>
        public static bool FullBody(ItemDefinition definition)
        {
            string weapon = definition.Name;
            for (int i = 0; i < FULL_BODY.Length; i++)
            {
                if (weapon.Contains(FULL_BODY[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns whether or not an item is a Full Hat.
        /// </summary>
        /// <param name="definition">The details of the item being equipped.</param>
        /// <returns>Whether or not the item covers your beard.</returns>
        public static bool FullHat(ItemDefinition definition)
        {
            string weapon = definition.Name;
            for (int i = 0; i < FULL_HAT.Length; i++)
            {
                if (weapon.EndsWith(FULL_HAT[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns whether or not an item is a Full Mask.
        /// </summary>
        /// <param name="definition">The details of the item being equipped.</param>
        /// <returns>Whether or not the item covers your head.</returns>
        public static bool FullMask(ItemDefinition definition)
        {
            String weapon = definition.Name;
            for (int i = 0; i < FULL_MASK.Length; i++)
            {
                if (weapon.EndsWith(FULL_MASK[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns which slot an item belongs to.
        /// </summary>
        /// <param name="wearId">The itemId to equip</param>
        /// <returns>The slot to place the item in.</returns>
        public static sbyte GetItemType(short wearId)
        {
            string weapon = new Item(wearId).Definition.Name;

            for (int i = 0; i < WEAPONS.Length; i++)
            {
                if (weapon.EndsWith(WEAPONS[i]) || weapon.StartsWith(WEAPONS[i]))
                    return 3;
            }
            for (int i = 0; i < CAPES.Length; i++)
            {
                if (weapon.Contains(CAPES[i]))
                    return 1;
            }
            for (int i = 0; i < HATS.Length; i++)
            {
                if (weapon.Contains(HATS[i]))
                    return 0;
            }
            for (int i = 0; i < BOOTS.Length; i++)
            {
                if (weapon.EndsWith(BOOTS[i]) || weapon.StartsWith(BOOTS[i]))
                    return 10;
            }
            for (int i = 0; i < GLOVES.Length; i++)
            {
                if (weapon.EndsWith(GLOVES[i]) || weapon.StartsWith(GLOVES[i]))
                    return 9;
            }
            for (int i = 0; i < SHIELDS.Length; i++)
            {
                if (weapon.Contains(SHIELDS[i]))
                    return 5;
            }
            for (int i = 0; i < AMULETS.Length; i++)
            {
                if (weapon.EndsWith(AMULETS[i]) || weapon.StartsWith(AMULETS[i]))
                    return 2;
            }
            for (int i = 0; i < ARROWS.Length; i++)
            {
                if (weapon.EndsWith(ARROWS[i]) || weapon.StartsWith(ARROWS[i]))
                    return 13;
            }
            for (int i = 0; i < RINGS.Length; i++)
            {
                if (weapon.EndsWith(RINGS[i]) || weapon.StartsWith(RINGS[i]))
                    return 12;
            }
            for (int i = 0; i < BODY.Length; i++)
            {
                if (weapon.Contains(BODY[i]))
                    return 4;
            }
            for (int i = 0; i < LEGS.Length; i++)
            {
                if (weapon.Contains(LEGS[i]))
                    return 7;
            }
            return -1;
        }
        #endregion Methods
    }
}