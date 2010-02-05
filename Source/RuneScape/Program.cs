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
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using JoltEnvironment;
using JoltEnvironment.Debug;

namespace RuneScape
{
    /// <summary>
    /// Start point of this application.
    /// </summary>
    public static class Program
    {
        #region Fields
        /// <summary>
        /// A commentary logger for informative data.
        /// </summary>
        private static Logger logger;
        /// <summary>
        /// A thread that updates the title every 1 second with the uptime and memory usage.
        /// </summary>
        private static Thread titleMonitor = new Thread(new ThreadStart(Monitor));
        /// <summary>
        /// Jolt Environment's main version details.
        /// </summary>
        private static Version version = new Version(1, 0, 3473);
        /// <summary>
        /// Whether to show memory usage and server uptime.
        /// </summary>
        private static bool showInfo = false;
        #endregion Fields

        #region Properties
        /// <summary>
        /// A user-configured logger used to print data from debugging, to exceptions.
        /// </summary>
        internal static Logger Logger
        {
            get { return logger; }
        }

        /// <summary>
        /// Gets the version of this application.
        /// </summary>
        public static Version Version
        {
            get { return version; }
        }

        /// <summary>
        /// The amount of bytes send and recieved.
        /// </summary>
        public static int NetworkUsage
        {
            get;
            set;
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a foundation for the RuneScape emulator.
        /// 
        ///     <para>When a user brings up the 'runescape.exe' (this) 
        ///     application, Jolt Environment libraries which are 
        ///     needed at startup are loaded and configured.</para>
        /// </summary>
        public static void Main()
        {
            Console.Title = "Jolt Environment";

            // Load and configure console settings from console.ini
            try
            {
                Configuration configuration = Configuration.Load(@"..\data\console.ini");
                if (configuration["Console.Resize"])
                {
                    int width = configuration["Console.Width"];
                    int height = configuration["Console.Height"];

                    if (width > 0)
                    {
                        Console.WindowWidth = width;
                    }
                    if (height > 0)
                    {
                        Console.WindowHeight = height;
                    }
                }

                showInfo = configuration["Console.ShowInfo"];

                JEnvironment.SetupLogger(new Logger(
                    (LogPriority)configuration["Logger.Priority"],
                    configuration["Logger.Colored"],
                    configuration["Logger.LogEvent"]));
                logger = JEnvironment.Logger;
            }
            catch (Exception ex) // Most probably there is a mistake in the configuration file.
            {
                Console.WriteLine(ex);
                Thread.Sleep(5000);
                Environment.Exit(0);
            }

            // Catch any unhandled exceptions.
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => Logger.WriteError("UNHANDLED EXCEPTION: " + e.ExceptionObject);

            // DO NOT EDIT / REMOVE THIS BANNER.
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"    _     _ _     ___         _                            _   ");
            Console.WriteLine(@" _ | |___| | |_  | __|_ ___ _(_)_ _ ___ _ _  _ __  ___ _ _| |_ ");
            Console.WriteLine(@"| || / _ \ |  _| | _|| ' \ V / | '_/ _ \ ' \| '  \/ -_) ' \  _|");
            Console.WriteLine(@" \__/\___/_|\__| |___|_||_\_/|_|_| \___/_||_|_|_|_\___|_||_\__|");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Jolt Environment [" + version + "] x" + (Environment.Is64BitProcess ? "64" : "32") + "-bit");
            Console.WriteLine(" Copyright (C) 2010 Jolt Environment Team");
            Console.WriteLine(" http://www.ajravindiran.com/projects/jolt/");
            Console.WriteLine();
            Console.WriteLine();

            //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            //watch.Start();
            //watch.Stop();
            //Console.WriteLine(watch.Elapsed.TotalMilliseconds);

            /*
             * Start a new thread that will update every second 
             * the current memory usage and uptime, if we are allowed
             * to show the information.
             */
            if (showInfo)
            {
                // Start the monitor at the lowest priority.
                titleMonitor.Name = "Title Monitor";
                titleMonitor.Priority = ThreadPriority.Lowest;
                titleMonitor.Start();
            }

            GameServer.Initialize(); // Initialize the runescape emulator.
            ListenForCommand();
            GameServer.Terminate(); // The server is not running anymore, shutdown server.
        }


        /// <summary>
        /// Listens for commands inputted into the console.
        /// </summary>
        private static void ListenForCommand()
        {
            while (GameServer.IsRunning) // A "command-line" based function that waits for input from a user then handles it.
            {
                string input = Console.ReadLine().Trim();
                string command = input.Split(' ')[0];
                string[] arguments = input.Substring(command.Length).Trim().Split(' ');
                CommandLine.Handle(command, arguments);
            }
        }

        /// <summary>
        /// A monitor that updates the console title of the garbage 
        /// collection memory currently being used by this application, 
        /// aswell as the application uptime.
        /// </summary>
        private static void Monitor()
        {
            var startTime = DateTime.Now;
            while (true)
            {
                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception ex) { Console.WriteLine(ex); }

                Console.Title = "Jolt Environment [Memory: " + 
                    GC.GetTotalMemory(false) / 1024 + "KB | Uptime: " + 
                    (DateTime.Now - startTime) + "]";
            }
        }
        #endregion Methods
    }
}
