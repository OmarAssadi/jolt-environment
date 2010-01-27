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
using System.IO;
using System.Diagnostics;
using System.Text;

namespace JoltEnvironment.Debug
{
    /// <summary>
    /// A standard logger. Used for less 'experienced' users who do not need 
    /// nor care of debug info, and is much eaiser to read and comprehend with.
    /// </summary>
    public class Logger
    {
        #region Fields
        /// <summary>
        /// Gets the asynchronous log worker.
        /// </summary>
        private AsyncLogWorker worker = new AsyncLogWorker();
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets or sets the priority of the printed log. 
        /// 
        ///     <para>If the log priority is lower than this priority, it will not be 
        ///     printed onto the console. Eg. if the current priority warn logs, 
        ///     anything below (such as a info log), will not be printed. Helpful
        ///     for those who don't require a specific type of log.</para>
        /// </summary>
        public LogPriority LogPriority { get; set; }
        /// <summary>
        /// Gets or sets whether the logger should print out logs with a more 
        /// "enriched" color matted system.
        /// 
        ///     <para>If enabled, logs will show a different colour depending on it's 
        ///     log type. Eg. warn logs will be shown in yellow, error logs will
        ///     be shown in red, debug logs will be shown in white etc etc.</para>
        /// </summary>
        public bool Colored { get; set; }
        /// <summary>
        /// Gets or sets whether the logger should log events to a log file.
        /// 
        ///     <para>If enabled, selected logs that are logged will be written to a text
        ///     file (.log) for debugging etc. Prints out a stacktrace so you can
        ///     find the problem and fix it.</para>
        /// </summary>
        public bool LogEvent { get; set; }
        /// <summary>
        /// Gets or sets whether the logger should show traces.
        /// 
        ///     <para>If enabled, traces from exceptions and errors will be shown. If you
        ///     are a developer, debugger and/or a tester, it is highly recommended
        ///     that you enable this to get a full read of any exceptions or errors.</para>
        /// </summary>
        public bool TraceMode { get; set; }
        #endregion Properites

        #region Constructors
        /// <summary>
        /// Constructs a standardized logger.
        /// </summary>
        /// <param name="logPriority">The log priority of the initial logger.</param>
        /// <param name="colored">Whether colors will be enabled.</param>
        /// <param name="traceMode">Whether traces will be logged.</param>
        /// <param name="fullName">Whether the logs will show full names.</param>
        /// <param name="showMethod">Whether the logs will show method names.</param>
        public Logger(LogPriority logPriority, bool colored, bool logEvent)
        {
            this.LogPriority = logPriority;
            this.Colored = colored;
            this.LogEvent = logEvent;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Writes a log depending of the given standard configuration. 
        /// </summary>
        /// <param name="frame">Frame from the stactrace.</param>
        /// <param name="priority">Priority level of this log.</param>
        /// <param name="message">Message to be outputted.</param>
        private void WriteLog(StackFrame frame, ConsoleColor color, object message)
        {
            // Lock this method so that logs don't scramble when printed on the console.
            if (this.Colored)
            {
                Console.Write("[" + DateTime.Now + "] :: ");
                Console.ForegroundColor = color;
                Console.WriteLine(frame.GetMethod().ReflectedType + " " + frame.GetMethod().Name);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("[" + DateTime.Now + "] :: "
                    + frame.GetMethod().ReflectedType + " "
                    + frame.GetMethod().Name
                    + Environment.NewLine + message);
            }
        }

        /// <summary>
        /// Writes a standartized debug log.
        /// </summary>
        /// <param name="message">The message to be printed</param>
        public void WriteDebug(object message)
        {
            if (this.LogPriority <= LogPriority.Debug)
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                this.worker.QueueLog(new Action(() => WriteLog(frame, ConsoleColor.DarkGray, message)));
            }
        }

        /// <summary>
        /// Writes a standardized debug log.
        /// </summary>
        /// <param name="message">The message to be printed</param>
        /// <param name="logEvent">Whether this log should be saved.</param>
        public void WriteDebug(object message, bool logEvent)
        {
            if (this.LogPriority <= LogPriority.Debug)
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                this.worker.QueueLog(new Action(() => WriteLog(frame, ConsoleColor.DarkGray, message)));

                if (logEvent)
                {
                    this.worker.QueueLog(new Action(() => WriteFile(frame, LogPriority.Debug, message)));
                }
            }
        }

        /// <summary>
        /// Writes a standardized information log.
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        public void WriteInfo(object message)
        {
            if (this.LogPriority <= LogPriority.Info)
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                this.worker.QueueLog(new Action(() => WriteLog(frame, ConsoleColor.DarkCyan, message)));
            }
        }

        /// <summary>
        /// Writes a standardized information log.
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        /// <param name="logEvent">Whether this log should be saved.</param>
        public void WriteInfo(object message, bool logEvent)
        {
            if (this.LogPriority <= LogPriority.Info)
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                this.worker.QueueLog(new Action(() => WriteLog(frame, ConsoleColor.DarkCyan, message)));

                if (logEvent)
                {
                    this.worker.QueueLog(new Action(() => WriteFile(frame, LogPriority.Info, message)));
                }
            }
        }

        /// <summary>
        /// Writes a standardized warning log.
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        /// <param name="logEvent">Whether this log should be saved.</param>
        public void WriteWarn(object message)
        {
            if (this.LogPriority <= LogPriority.Warn)
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                this.worker.QueueLog(new Action(() => WriteLog(frame, ConsoleColor.Yellow, message)));
            }
        }

        /// <summary>
        /// Writes a standardized warning log.
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        /// <param name="logEvent">Whether this log should be saved.</param>
        public void WriteWarn(object message, bool logEvent)
        {
            if (this.LogPriority <= LogPriority.Warn)
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                WriteLog(frame, ConsoleColor.Yellow, message);

                if (logEvent)
                {
                    WriteFile(frame, LogPriority.Warn, message);
                }
            }
        }

        /// <summary>
        /// Writes a standardized error log.
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        /// <param name="logEvent">Whether this log should be saved.</param>
        public void WriteError(object message)
        {
            if (this.LogPriority <= LogPriority.Error)
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                WriteLog(frame, ConsoleColor.Red, message);
            }
        }

        /// <summary>
        /// Writes a standardized error log.
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        /// <param name="logEvent">Whether this log should be saved.</param>
        public void WriteError(object message, bool logEvent)
        {
            if (this.LogPriority <= LogPriority.Error)
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                WriteLog(frame, ConsoleColor.Red, message);

                if (logEvent)
                {
                    WriteFile(frame, LogPriority.Error, message);
                }
            }
        }

        /// <summary>
        /// Writes a standardized exception log.
        /// </summary>
        /// <param name="ex">The System.Exception object that was caught.</param>
        public void WriteException(Exception ex)
        {
            if (this.LogPriority <= LogPriority.Error)
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                WriteLog(frame, ConsoleColor.Red, ex);
                WriteFile(frame, LogPriority.Error, ex);
            }
        }

        /// <summary>
        /// Writes a standardized exception log.
        /// </summary>
        /// <param name="ex">The System.Exception object that was caught.</param>
        /// <param name="logEvent">Whether this log should be saved.</param>
        public void WriteException(Exception ex, bool logEvent)
        {
            if (this.LogPriority <= LogPriority.Error)
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                WriteLog(frame, ConsoleColor.Red, ex);

                if (logEvent)
                {
                    WriteFile(frame, LogPriority.Error, ex);
                }
            }
        }

        /// <summary>
        /// Writes a standardized exception log.
        /// </summary>
        /// <param name="message">The exception's message.</param>
        /// <param name="ex">The System.Exception object taht was caught.</param>
        public void WriteException(object message, Exception ex)
        {
            if (this.LogPriority <= LogPriority.Error)
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                WriteLog(frame, ConsoleColor.Red, message + "\n" + ex.StackTrace);
                WriteFile(frame, LogPriority.Error, message + "\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// Writes a standardized exception log.
        /// </summary>
        /// <param name="message">The exception's message.</param>
        /// <param name="ex">The System.Exception object that was caught.</param>
        /// <param name="logEvent">Whether the log should be saved.</param>
        public void WriteException(object message, Exception ex, bool logEvent)
        {
            if (this.LogPriority <= LogPriority.Error)
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                WriteLog(frame, ConsoleColor.Red, message + "\n" + ex.StackTrace);

                if (logEvent)
                {
                    WriteFile(frame, LogPriority.Error, ex);
                }
            }
        }

        /// <summary>
        /// Writes a custom log.
        /// </summary>
        /// <param name="priority">The priority of the log.</param>
        /// <param name="color">The color of the log.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="ignorePriority">Whether to ignore priority rules.</param>
        /// <param name="logEvent">Whether to log to file.</param>
        public void WriteCustom(LogPriority priority, ConsoleColor color, object message, bool ignorePriority, bool logEvent)
        {
            StackFrame frame = new StackTrace().GetFrame(1);
            if (this.LogPriority <= priority)
            {
                WriteLog(frame, color, message);
            }
            else if (ignorePriority)
            {
                WriteLog(frame, color, message);
            }

            if (logEvent)
            {
                WriteFile(frame, priority, message);
            }
        }

        /// <summary>
        /// Writes down a log or a plain mesage to a log file.
        /// </summary>
        /// <param name="frame">Frame from the stactrace.</param>
        /// <param name="message">Message to be outputted.</param>
        /// <param name="writePlain">Whether the file is written in log format or plain.</param>
        public void WriteFile(StackFrame frame, LogPriority priority, object message)
        {
            TextWriter writer = new StreamWriter(@"..\data\logs\" + priority.ToString().ToLower() + ".log", true);
            writer.WriteLine("[" + DateTime.Now + "] :: " + frame.GetMethod().ReflectedType + " " + frame.GetMethod().Name);
            writer.WriteLine(message);
            writer.Dispose();
            writer = null;
        }

        /// <summary>
        /// Writes a plain log into the log file.
        /// </summary>
        /// <param name="message"></param>
        public void WriteFile(object message)
        {
            TextWriter writer = new StreamWriter(@"..\data\logs\extra.log", true);
            writer.WriteLine(message);
            writer.Dispose();
            writer = null;
        }

        /// <summary>
        /// Writes a plain log into the log file.
        /// </summary>
        /// <param name="message"></param>
        public void WriteFile(object message, string file)
        {
            TextWriter writer = new StreamWriter(@"..\data\logs\" + file + ".log", true);
            writer.WriteLine(message);
            writer.Dispose();
            writer = null;
        }
        #endregion Methods
    }
}
