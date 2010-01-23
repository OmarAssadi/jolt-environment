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
using System.Threading;

using MySql.Data.MySqlClient;

namespace JoltEnvironment.Storage.Sql
{
    /// <summary>
    /// Management for SQL databases.
    /// </summary>
    public class SqlDatabaseManager
    {
        #region Fields
        /// <summary>
        /// The database this manager will work with.
        /// </summary>
        private SqlDatabase database;
        /// <summary>
        /// The server at which the database is located.
        /// </summary>
        private SqlDatabaseServer server;

        /// <summary>
        /// A thread which monitors all the clients and cleans where nessesary.
        /// </summary>
        private Thread monitor;
        /// <summary>
        /// Provides sychronization for client manipulation.
        /// </summary>
        private object lockObject = new object();

        /// <summary>
        /// The clients which will serve for this management.
        /// </summary>
        private SqlDatabaseClient[] clients = new SqlDatabaseClient[0];
        /// <summary>
        /// An array which matches the same length as the <code>clients</code> 
        /// array, holding information if the client is availible.
        /// </summary>
        private bool[] availibleClients = new bool[0];
        /// <summary>
        /// The number of client starvations since startup.
        /// </summary>
        private int clientStarvation;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        public string ConnectionString { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new SqlDatabaseManager with the specified database to 
        /// work with, aswell as the database server connection credenitials.
        /// </summary>
        /// <param name="database">The database to work with.</param>
        /// <param name="server">The server credenitials for access to database.</param>
        public SqlDatabaseManager(SqlDatabase database, SqlDatabaseServer server)
        {
            this.database = database;
            this.server = server;
            this.ConnectionString = GenerateConnectionString(database, server);
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Starts the monitor on a below normal priority.
        /// </summary>
        public void StartMonitor()
        {
            this.monitor = new Thread(MonitorLoop);
            this.monitor.Priority = ThreadPriority.BelowNormal;
            this.monitor.Name = "Sql Database Monitor";
            this.monitor.Start();
        }

        /// <summary>
        /// Stops the monitor.
        /// </summary>
        public void StopMonitor()
        {
            if (this.monitor != null)
            {
                this.monitor.Abort();
            }
        }

        /// <summary>
        /// Destroys the management.
        /// </summary>
        public void Destroy()
        {
            lock (this.lockObject)
            {
                for (int i = 0; i < this.clients.Length; i++)
                {
                    this.clients[i].Destroy();
                    this.clients[i] = null;
                }
            }

            this.server = null;
            this.database = null;
            this.clients = null;
            this.availibleClients = null;

            StopMonitor();
            this.monitor = null;
        }

        /// <summary>
        /// A loop that is continuous, which cleans up the clients being inactive for too long.
        /// </summary>
        private void MonitorLoop()
        {
            while (true)
            {
                try
                {
                    lock (this.lockObject)
                    {
                        DateTime date = DateTime.Now;

                        // Loop through all the clients to check for ones that may be dead/closed.
                        for (int i = 0; i < clients.Length; i++)
                        {
                            if (this.clients[i].State != ConnectionState.Closed)
                            {
                                if (this.clients[i].ElapsedInactivity >= 60)
                                {
                                    this.clients[i].Disconnect();
                                    JEnvironment.Logger.WriteDebug("Closed connection for client #" + this.clients[i].Handle + ".");
                                }
                            }
                        }
                    }

                    // Sleep for 10 seconds, before monitoring again.
                    Thread.Sleep(10000);
                }
                catch (ThreadAbortException)
                {
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Generates a connection string from the given database credinitials.
        /// </summary>
        /// <param name="database">The database the conection will connect to.</param>
        /// <param name="server">The database server credenitials.</param>
        /// <returns>Returns a string holding the generated connection string.</returns>
        private static string GenerateConnectionString(SqlDatabase database, SqlDatabaseServer server)
        {
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();

            // Database.
            csb.Database = database.Name;
            csb.MinimumPoolSize = database.MinPoolSize;
            csb.MaximumPoolSize = database.MaxPoolSize;

            // Server.
            csb.Server = server.Host;
            csb.Port = server.Port;
            csb.UserID = server.User;
            csb.Password = server.Password;

            return csb.ToString();
        }

        /// <summary>
        /// The amount of clients that will be availible for usage.
        /// </summary>
        /// <param name="amount">The amount of clients to allow.</param>
        public void SetClientAmount(uint amount)
        {
            lock (this.lockObject)
            {
                if (this.clients.Length == amount)
                {
                    return;
                }

                // The amount has shrunk, so we remove the required amount.
                if (amount < this.clients.Length)
                {
                    for (uint i = amount; i < this.clients.Length; i++)
                    {
                        this.clients[i].Destroy();
                        this.clients[i] = null;
                    }
                }

                SqlDatabaseClient[] tmpClients = new SqlDatabaseClient[amount];
                bool[] tmpAvailibleClients = new bool[amount];

                for (uint i = 0; i < amount; i++)
                {
                    // Keep clients that already exist.
                    if (i < this.clients.Length)
                    {
                        tmpClients[i] = this.clients[i];
                        tmpAvailibleClients[i] = this.availibleClients[i];
                    }

                    // Make any that is required.
                    else
                    {
                        tmpClients[i] = new SqlDatabaseClient(i + 1, this);
                        tmpAvailibleClients[i] = true;
                    }
                }

                // Modify manager's actual arrays.
                this.clients = tmpClients;
                this.availibleClients = tmpAvailibleClients;
            }
        }

        /// <summary>
        /// Attempts to get an availible SqlDatabaseClient.
        /// </summary>
        /// <returns>Returns an SqlDatabaseClient by this manager.</returns>
        public SqlDatabaseClient GetClient()
        {
            lock (this.lockObject)
            {
                // Attempt to find an availible client.
                for (uint i = 0; i < this.clients.Length; i++)
                {
                    // Check if availible.
                    if (this.availibleClients[i])
                    {
                        // Since there is an availble client, there is no more starvation.
                        this.clientStarvation = 0;

                        // The client's current conncetion state is closed, so we must reopen.
                        if (this.clients[i].State == ConnectionState.Closed)
                        {
                            this.clients[i].Connect();
                            JEnvironment.Logger.WriteDebug("Opened connection for client #" + this.clients[i].Handle + ".");
                        }

                        // If the connection is open, reserve this client.
                        if (this.clients[i].State == ConnectionState.Open)
                        {
                            this.availibleClients[i] = false; // Reserve.
                            this.clients[i].LastActivity = DateTime.Now;
                            return this.clients[i];
                        }
                    }
                }

                // Starvation is growing, no clients availble.
                this.clientStarvation++;

                // Check if we can cure starvation.
                if (this.clientStarvation >= ((this.clients.Length + 1) / 2))
                {
                    this.clientStarvation = 0;
                    SetClientAmount((uint)(this.clients.Length + 1 * 1.3F));

                    // Retry.
                    return GetClient();
                }

                /* At this point, we cannot do anything at the moment because 
                 * all threads are busy, and starvation is not yet in priority. 
                 * We must create an anonymous client temporarily.
                 */
                SqlDatabaseClient anonClient = new SqlDatabaseClient(0, this);
                anonClient.Connect();
                return anonClient;
            }
        }

        /// <summary>
        /// Releases the client with the specified handle.
        /// </summary>
        /// <param name="handle">The handle index.</param>
        public void ReleaseClient(uint handle)
        {
            if (this.clients.Length >= (handle - 1))
            {
                this.availibleClients[handle - 1] = true;
            }
        }

        /// <summary>
        /// Inserts the given database object.
        /// </summary>
        /// <param name="obj">The object to delete.</param>
        /// <returns>Returns true if successfully deleted; false if not.</returns>
        public bool Insert(ISqlDataObject obj)
        {
            using (SqlDatabaseClient client = GetClient())
            {
                return obj.Insert(client);
            }
        }

        /// <summary>
        /// Deletes the given database object.
        /// </summary>
        /// <param name="obj">The object to delete.</param>
        /// <returns>Returns true if successfully deleted; false if not.</returns>
        public bool Delete(ISqlDataObject obj)
        {
            using (SqlDatabaseClient client = GetClient())
            {
                return obj.Delete(client);
            }
        }

        /// <summary>
        /// Updates the given database object.
        /// </summary>
        /// <param name="obj">The object to update.</param>
        /// <returns>Returns true if successfully updated; false if not.</returns>
        public bool Update(ISqlDataObject obj)
        {
            using (SqlDatabaseClient client = GetClient())
            {
                return obj.Update(client);
            }
        }

        /// <summary>
        /// Loads the given database object.
        /// </summary>
        /// <param name="obj">The object to load.</param>
        /// <returns>Returns true if successfully loaded; false if not.</returns>
        public bool Load(ISqlDataObject obj)
        {
            using (SqlDatabaseClient client = GetClient())
            {
                return obj.Load(client);
            }
        }

        /// <summary>
        /// Executes a query via a free client.
        /// </summary>
        /// <param name="obj">The object to execute.</param>
        /// <returns>Returns true if successfully executed; false if not.</returns>
        public object Execute(ISqlDataQuery obj)
        {
            using (SqlDatabaseClient client = GetClient())
            {
                return obj.Execute(client);
            }
        }

        /// <summary>
        /// Executes an update via a free client.
        /// </summary>
        /// <param name="obj">The object to execute.</param>
        public void ExecuteUpdate(ISqlDataQuery obj)
        {
            using (SqlDatabaseClient client = GetClient())
            {
                obj.ExecuteUpdate(client);
            }
        }

        /// <summary>
        /// Prints out an overview string of this manager.
        /// </summary>
        /// <returns>Returns a string with connection details for this manager.</returns>
        public override string ToString()
        {
            return this.ConnectionString;
        }
        #endregion Methods
    }
}
