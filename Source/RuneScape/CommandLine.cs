/* 
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

namespace RuneScape
{
    /// <summary>
    /// A command line interpreter allowing us to execute commands 
    /// sumultaneously while the application is running.
    /// </summary>
    public static class CommandLine
    {
        #region Methods
        /// <summary>
        /// Handles a given command.
        /// </summary>
        /// <param name="command">Command to handle.</param>
        /// <param name="arguments">Arguments to handle.</param>
        public static void Handle(string command, string[] arguments)
        {
            try
            {
                switch (command.ToLower())
                {
                    case "add":
                        {
                            RuneScape.Communication.Messages.Frames.SendSystemUpdate(10, true);
                            break;
                        }
                    case "clear":
                        {
                            for (int i = 0; i < GameEngine.World.NpcManager.Spawns.Count; i++)
                            {
                                GameEngine.World.NpcManager.Spawns[i].UpdateFlags.ClearFaceTo = true;
                            }
                            break;
                        }
                    case "about": // Displays information about jolt environment.
                        Console.WriteLine(" Jolt Environment [" + Program.Version.ToString() + " Release-Win32-x86/x64]");
                        Console.WriteLine(" Copyright (C) 2010 Jolt Environment Team <aj@ajravindiran.com>");
                        Console.WriteLine(" Website: http://www.ajravindiran.com/projects/jolt/");
                        Console.WriteLine(" Credits: http://www.ajravindiran.com/projects/jolt/credits/");
                        break;
                    case "system":
                        Console.WriteLine("Operating system: " + Environment.OSVersion);
                        Console.WriteLine("Processors/Cores: " + Environment.ProcessorCount);
                        Console.WriteLine("Logged in username: " + Environment.MachineName);
                        Console.WriteLine("You are running the " + (Environment.Is64BitProcess ? "x64" : "x86") + "-bit jolt version.");
                        break;
                    case "gc":
                        GC.Collect();
                        Program.Logger.WriteInfo("Successfully garbage collected.");
                        break;
                    case "shutdown":
                        GameServer.IsRunning = false;
                        break;
                    case "update":
                        System.Diagnostics.Process.Start("http://jolte.codeplex.com/Release/ProjectReleases.aspx");
                        Program.Logger.WriteInfo("Successfully brang up downloads page.");
                        break;
                    default:
                        Program.Logger.WriteWarn("Unknown command \"" + command + "\".");
                        break;
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
