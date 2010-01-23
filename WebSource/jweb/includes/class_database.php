<?php
/* 
 *     jWeb
 *     Copyright (C) 2010 Jolt Environment Team
 * 
 *     This program is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 * 
 *     This program is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU General Public License for more details.
 * 
 *     You should have received a copy of the GNU General Public License
 *     along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

/**
 * Controls database connections.
 *
 * @author AJ Ravindiran
 * @version 1.0.0 Jan-02-2010
 */
class database {

    private $host;
    private $username;
    private $password;
    private $database;
    private $connection = null;
    private $connected = false;

    /**
     * Constructs a new database instance, with given details.
     * @param string $host The database server host.
     * @param string $port The database server port.
     * @param string $username The database's username authentication.
     * @param string $password The username's specified password.
     */
    public function __construct($host = "localhost", $username = "root", $password = "") {
        $this->host = $host;
        $this->username = $username;
        $this->password = $password;
    }

    /**
     * Destructs the database instance.
     */
    public function __destruct() {
        $this->disconnect();
    }

    /**
     * Connections to the given MySQL database. The
     * @param string $database The database to write to and read from.
     */
    public function connect($database) {
        $this->database = $database;
        $this->connection = @mysql_connect($this->host, $this->username, $this->password) or $this->error(mysql_error());
        @mysql_select_db($this->database, $this->connection) or $this->error(mysql_error());

        /*
         * If the connection was successful, we can now
         * identify that the connection is sustained.
        */
        if ($this->connection != null) {
            $this->connected = true;
        }
    }

    /**
     * Disconnects this instance from the mysql connection.
     */
    public function disconnect() {
        if ($this->connected) {
            @mysql_close($this->connection);
            $this->connected = false;
        }
    }

    /**
     * Executes the given querie(s).
     * @param string $query The query to execute.
     * @return resource The data recieved from the database.
     */
    public function execute_query($query) {
        $resultset = mysql_query($query, $this->connection) or $this->error('While executing ["' . $query . '"]: ' . mysql_error());
        return $resultset;
    }

    /**
     * Evaluates wheter the given result is valid.
     * @param resource $result_set The result set to evaluate.
     * @return int
     */
    public function evaluate_query($query, $default_value = 0) {
        if(!$result = @$this->execute_query($query)) {
            return 0;
        }
        if(@mysql_num_rows($result) == 0) {
            return $default_value;
        }
        else {
            return @mysql_result($result, 0);
        }
    }

    /**
     * Calls a mysql error report.
     * @global core $core
     * @param string $text The mysql error.
     */
    public function error($text) {
        global $core;
        $core->system_error("mysql", $text);
    }

    /**
     * Gets the specified connection details from this instance.
     * @param boolean $show_details Whether to show explicit details of the
     * connection's username, password, and username. If true, those details
     * will be shown, otherwise, only the host and port are shown.
     * @return string The connection string.
     */
    public function get_connection_string($show_details = false) {
        if ($show_details) {
            return "database[host=" . $this->host  . ", port=" . $this->port . ", user="
                    . $this->username . ", pass=" . $this->password . ", database=" . $this->database . "]";
        } else {
            return "database[host=" . $this->host . ", port=" . $this->port . "]";
        }
    }
}
?>
