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
using System.Collections;
using System.Collections.Generic;
using System.IO;

using CSScriptLibrary;

namespace RuneScape.Scripting
{
    /// <summary>
    /// Manages scripts within the scripting folder.
    /// </summary>
    public class ScriptManager
    {
        #region Fields
        /// <summary>
        /// A generic collection containing loaded and parsed scripts.
        /// </summary>
        private Dictionary<string, dynamic> scripts;
        /// <summary>
        /// Directory information for main script folder.
        /// </summary>
        private DirectoryInfo directory;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Initializes the scripting manager.
        /// </summary>
        public void Initialize()
        {
            this.scripts = new Dictionary<string, dynamic>();
            this.directory = new DirectoryInfo(@"..\data\scripts\");

            if (directory.Exists)
            {
                // Load files that are in the main folder.
                foreach (FileInfo f in directory.GetFiles())
                {
                    if (f.Exists)
                    {
                        dynamic script = null;
                        try
                        {
                            using (StreamReader reader = new StreamReader(f.Open(FileMode.Open)))
                            {
                                script = CSScript.LoadCode(reader.ReadToEnd()).CreateObject("*");
                                this.scripts.Add(f.Name, script);
                            }
                        }
                        catch (Exception ex)
                        {
                            Program.Logger.WriteException(ex);
                        }
                    }
                }

                // Load files that are in subfolders.
                foreach (DirectoryInfo d in directory.GetDirectories())
                {
                    foreach (FileInfo f in d.GetFiles())
                    {
                        if (f.Exists)
                        {
                            dynamic script = null;
                            try
                            {
                                using (StreamReader reader = new StreamReader(f.Open(FileMode.Open)))
                                {
                                    script = CSScript.LoadCode(reader.ReadToEnd()).CreateObject("*");
                                    this.scripts.Add(@"\" + d.Name + @"\" + f.Name, script);
                                }
                            }
                            catch (Exception ex)
                            {
                                Program.Logger.WriteException(ex);
                            }
                        }
                    }
                }
            }
            new List<string>(this.scripts.Keys).ForEach(new Action<string>((s) => { Console.WriteLine(s); }));
        }

        /// <summary>
        /// Gets the specified script.
        /// </summary>
        /// <param name="script">The script to find.</param>
        /// <returns>Returns a dynamic object.</returns>
        public dynamic this[string script]
        {
            get
            {
                try
                {
                    return this.scripts[script];
                }
                catch (Exception ex)
                {
                    Console.WriteLine("TEST");
                    Program.Logger.WriteException(ex);
                }
                return null;
            }
        }
        #endregion Methods
    }
}
