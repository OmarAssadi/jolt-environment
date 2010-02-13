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
using System.Data;

using MySql.Data.MySqlClient;

namespace JoltEnvironment.Storage.Sql
{
    /// <summary>
    /// Serves as a database client. Provides functionality and connection to a database.
    /// </summary>
    public class SqlDatabaseClient : IDisposable
    {
        #region Fields
        /// <summary>
        /// The SqlDatabaseManager which manages this client.
        /// </summary>
        private SqlDatabaseManager manager;
        /// <summary>
        /// The MySQL connnection provider.
        /// </summary>
        private MySqlConnection connection;
        /// <summary>
        /// The MySQL command provider.
        /// </summary>
        private MySqlCommand command;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the client's handle id.
        /// </summary>
        public uint Handle { get; private set; }
        /// <summary>
        /// Gets whether the client is accessing without management.
        /// </summary>
        public bool Anonymous { get { return this.Handle == 0; } }
        /// <summary>
        /// Gets or sets the database's last activity.
        /// </summary>
        public DateTime LastActivity { get; set; }
        /// <summary>
        /// Gets the elapsed inactivity since the last activity.
        /// </summary>
        public double ElapsedInactivity 
        { 
            get { return (DateTime.Now - LastActivity).TotalSeconds; } 
        }

        /// <summary>
        /// Gets the current connection state.
        /// </summary>
        public ConnectionState State
        {
            get { return (this.connection != null) ? this.connection.State : ConnectionState.Broken; }
        }

        /// <summary>
        /// Gets the manager that manages this client.
        /// </summary>
        public SqlDatabaseManager Manager
        {
            get { return this.manager; }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructs a new SqlDatabaseClient with the specified handle id, and manager.
        /// </summary>
        /// <param name="handle">The client's handle.</param>
        /// <param name="manager">The manager that supports the client.</param>
        public SqlDatabaseClient(uint handle, SqlDatabaseManager manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("The specified SqlDatabaseManager object is null.");
            }

            this.Handle = handle;
            this.manager = manager;
            this.connection = new MySqlConnection(manager.ConnectionString);
            this.command = connection.CreateCommand();
            this.LastActivity = DateTime.Now;
        }

        /// <summary>
        /// Constructs a new SqlDatabaseClient with the specified handle id, and manager.
        /// </summary>
        /// <param name="manager">The manager that supports the client.</param>
        public SqlDatabaseClient(SqlDatabaseManager manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("The specified SqlDatabaseManager object is null.");
            }

            this.Handle = 0;
            this.manager = manager;
            this.connection = new MySqlConnection(manager.ConnectionString);
            this.command = connection.CreateCommand();
            this.LastActivity = DateTime.Now;
            Connect();
        }
        #endregion Constructors

        #region Methods


        /// <summary>
        /// Attempts to make a connection from the specified manager's database server.
        /// </summary>
        public void Connect()
        {
            // Check for any conditions that could interupt the connection.
            if (connection == null)
            {
                throw new DatabaseException("Connection instance #" + Handle + " holds no value.");
            }
            if (connection.State != ConnectionState.Closed)
            {
                throw new DatabaseException("Connection instance #" + Handle + " has to be closed before it can be reopened.");
            }

            try
            {
                connection.Open();
            }
            catch (MySqlException mse)
            {
                throw new DatabaseException(mse.Message);
            }
        }

        /// <summary>
        /// Disconnects the client.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                connection.Close();
            }
            catch (MySqlException mse)
            {
                throw new DatabaseException(mse.Message);
            }
        }

        /// <summary>
        /// Destroys the client by disconnecting the connection and releasing all used resources.
        /// </summary>
        public void Destroy()
        {
            Disconnect();

            this.connection.Dispose();
            this.connection = null;
            this.command.Dispose();
            this.command = null;
            this.manager = null;
        }

        /// <summary>
        /// Adds a parameter to the command.
        /// </summary>
        /// <param name="param">The name of the parameter.</param>
        /// <param name="value">The value that corresponds with the parameter.</param>
        public void AddParameter(string parameter, object value)
        {
            this.command.Parameters.AddWithValue(parameter, value);
        }

        /// <summary>
        /// Executes the specified query, and returns a value.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        public object ExecuteQuery(string query)
        {
            this.command.CommandText = query;
            object obj = this.command.ExecuteScalar();
            this.command.CommandText = null;
            return obj;
        }

        /// <summary>
        /// Executes the specified command.
        /// 
        ///     <remarks>Used for non-returning statements, such 
        ///     as 'INSERT', 'DELETE', and 'UPDATE'.</remarks>
        /// </summary>
        /// <param name="query">The query to execute.</param>
        public void ExecuteUpdate(string query)
        {
            this.command.CommandText = query;
            this.command.ExecuteNonQuery();
            this.command.CommandText = null;
        }

        /// <summary>
        /// Reads a set of tables from the database.
        /// </summary>
        /// <param name="query">The query that is executed to return and fill the data set.</param>
        /// <returns>Returns the dataset.</returns>
        public DataSet ReadDataSet(string query)
        {
            DataSet dataSet = new DataSet();
            this.command.CommandText = query;

            using (MySqlDataAdapter adapter = new MySqlDataAdapter(this.command))
            {
                adapter.Fill(dataSet);
            }

            this.command.CommandText = null;
            return dataSet;
        }

        /// <summary>
        /// Reads a single table from the database.
        /// </summary>
        /// <param name="query">The query that is executed to return and fill the data table.</param>
        /// <returns>Returns the data table.</returns>
        public DataTable ReadDataTable(string query)
        {
            DataTable dataTable = new DataTable();
            this.command.CommandText = query;

            using (MySqlDataAdapter adapter = new MySqlDataAdapter(this.command))
            {
                adapter.Fill(dataTable);
            }

            this.command.CommandText = null;
            return dataTable;
        }

        /// <summary>
        /// Reads a single row from the database.
        /// </summary>
        /// <param name="query">The query that is executed to return and fill the data row.</param>
        /// <returns>Returns the data row.</returns>
        public DataRow ReadDataRow(string query)
        {
            DataTable table = ReadDataTable(query);
            if (table != null && table.Rows.Count > 0)
            {
                return table.Rows[0];
            }
            return null;
        }

        /// <summary>
        /// Reads a single column from the database.
        /// </summary>
        /// <param name="query">The query that is executed to return and fill the data column.</param>
        /// <returns>Returns the data column.</returns>
        public DataColumn ReadDataColumn(string query)
        {
            DataTable table = ReadDataTable(query);
            if (table != null && table.Columns.Count > 0)
            {
                return table.Columns[0];
            }
            return null;
        }

        #region IDispose Members
        /// <summary>
        /// Attempts to dispose the client.
        /// </summary>
        public void Dispose()
        {
            /*
             * Since this isn't an anonymous client, it does not require 
             * disposing, and should return back to the manager. otherwise,
             * we will dispose it.
             */
            if (!this.Anonymous)
            {
                // Cleanup.
                this.command.CommandText = null;
                this.command.Parameters.Clear();
                this.manager.ReleaseClient(this.Handle);
            }
            else
            {
                Destroy();
            }
        }
        #endregion IDispose Memebrs
        #endregion Methods
    }
}
