using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RuneScape.Communication;
using RuneScape.Communication.Messages;
using RuneScape.Communication.Messages.Outgoing;
using RuneScape.Content.Interfaces;
using RuneScape.Model;
using RuneScape.Model.Characters;
using RuneScape.Model.Items;
using RuneScape.Model.Objects;

namespace RuneScape.Content
{
    /// <summary>
    /// Defines handling of game commands.
    /// </summary>
    public class GameCommands
    {
        #region Methods
        /// <summary>
        /// Handles the specified command (and it's specified arguments).
        /// </summary>
        /// <param name="character">The character to execute command for.</param>
        /// <param name="command">The command to execute.</param>
        /// <param name="arguments">The command arguments.</param>
        public static void Handle(Character character, string command, string[] arguments)
        {
            try
            {
                /*
                 * Commands that are specified for players.
                 */
                if (character.ServerRights >= ServerRights.Player)
                {
                    #region Setup
                    /*
                     * Brings up an interface for character setup.
                     */
                    if (command.Equals("setup"))
                    {
                        CharacterSetup.Show(character);
                    }
                    #endregion Setup

                    #region Clear Inventory
                    /*
                     * Clears the character's inventory.
                     */
                    else if (command.Equals("clearinv"))
                    {
                        character.Inventory.Reset();
                    }
                    #endregion Clear Inventory
                }

                /*
                 * Commands that are specified for donators.
                 */
                if (character.ServerRights >= ServerRights.Donator)
                {
                }

                /*
                 * Commands that are specified for moderators.
                 */
                if (character.ServerRights >= ServerRights.Moderator)
                {
                }

                /*
                 * Commands that are specified for administrators.
                 */
                if (character.ServerRights >= ServerRights.Administrator)
                {
                    #region Teleport
                    /*
                     * Teleports the character to the specified coordinates.
                     */ 
                    if (command.Equals("teleport"))
                    {
                        if (arguments.Length == 3 || arguments.Length == 4)
                        {
                            short x = short.Parse(arguments[1]);
                            short y = short.Parse(arguments[2]);
                            byte z = 0;

                            // The character specified a height.
                            if (arguments.Length == 4)
                            {
                                z = byte.Parse(arguments[3]);
                            }
                            character.UpdateFlags.TeleportLocation = Location.Create(x, y, z);
                            character.UpdateFlags.Teleporting = true;
                        }
                    }
                    #endregion Teleport
                }
                
                /*
                 * Commands that are specified for system administrators (development).
                 */
                if (character.ServerRights >= ServerRights.SystemAdministrator)
                {
                    #region Animate
                    /*
                     * Plays an animation for the character.
                     */
                    if (command.Equals("animate"))
                    {
                        if (arguments.Length == 2 || arguments.Length == 3)
                        {
                            short id = short.Parse(arguments[1]);
                            byte delay = 0;

                            // The character specified a delay.
                            if (arguments.Length == 3)
                            {
                                delay = byte.Parse(arguments[2]);
                            }
                            character.PlayAnimation(Animation.Create(id, delay));
                        }
                    }
                    #endregion Animate

                    #region Graphic
                    /*
                     * Plays a graphic for the character.
                     */
                    else if (command.Equals("graphic"))
                    {
                        if (arguments.Length == 2 || arguments.Length == 3)
                        {
                            short id = short.Parse(arguments[1]);
                            int delay = 0;

                            // The character specified a delay.
                            if (arguments.Length == 3)
                            {
                                delay = int.Parse(arguments[2]);
                            }
                            character.PlayGraphics(Graphic.Create(id, delay));
                        }
                    }
                    #endregion Graphic

                    #region Coords
                    /*
                     * Prints out the character's current coordinates.
                     */ 
                    else if (command.Equals("coords"))
                    {
                        byte[] packet = new MessagePacketComposer(
                            "Current X: " + character.Location.X + 
                            ", Current Y: " + character.Location.Y + 
                            ", Current Z: " + character.Location.Z).Serialize();
                        character.Session.SendData(packet);
                    }
                    #endregion Coords

                    #region Set Energy
                    /*
                     * Sets the character's energy to the specified amount.
                     */ 
                    else if (command.Equals("setenergy"))
                    {
                        if (arguments.Length == 2)
                        {
                            byte energy = 100;

                            if (byte.TryParse(arguments[1], out energy))
                            {
                                character.WalkingQueue.RunEnergy = energy;
                                character.Session.SendData(new RunEnergyPacketComposer(energy).Serialize());
                            }
                        }
                        else
                        {
                            character.WalkingQueue.RunEnergy = 100;
                            character.Session.SendData(new RunEnergyPacketComposer(100).Serialize());
                        }
                    }
                    #endregion Set Energy

                    #region Add Item
                    /*
                     * Spawns an item.
                     */ 
                    else if (command.Equals("additem"))
                    {
                        if (arguments.Length == 2 || arguments.Length == 3)
                        {
                            short item = short.Parse(arguments[1]);
                            int count = 1;

                            if (arguments.Length == 3)
                            {
                                count = int.Parse(arguments[2]);
                            }
                            character.Inventory.AddItem(new Item(item, count));
                        }
                    }
                    #endregion Add Item

                    #region Delete Item
                    /*
                     * Deletes an item.
                     */ 
                    else if (command.Equals("deleteitem"))
                    {
                        if (arguments.Length == 2 || arguments.Length == 3)
                        {
                            short item = short.Parse(arguments[1]);
                            int count = 1;

                            if (arguments.Length == 3)
                            {
                                count = int.Parse(arguments[2]);
                            }
                            character.Inventory.DeleteItem(new Item(item, count));
                        }
                    }
                    #endregion Delete Item

                    #region Object
                    else if (command.Equals("object"))
                    {
                        short objId = short.Parse(arguments[1]);
                        new GroundItem(character.Location, character, objId, 1).Spawn();
                    }

                    else if (command.Equals("report"))
                    {
                        Frames.SendInterface(character, 553, false);
                    }

                    else if (command.Equals("runscript"))
                    {
                        int id = int.Parse(arguments[1]);
                        string argument = arguments[2];
                        character.Session.SendData(new RunScriptPacketComposer(id, "s", argument).Serialize());
                    }
                    #endregion Object
                }
            }
            catch (Exception ex)
            {
                Program.Logger.WriteException(ex);
            }
        }
        #endregion Methods
    }
}
