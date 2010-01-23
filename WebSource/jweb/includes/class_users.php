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
 * Provides management for uesrs.
 *
 * @author AJ Ravindiran
 * @version 1.0.0 Jan-03-2010
 */
class users {

    /**
     * Attempts to validate the given details. The give username,
     * and hashed password are checked against the connected database,
     *  if there are any matches, the detiails given are valid.
     * @param string $username The username to check for.
     * @param string $password_hash The hashed password specified with the username.
     * @return bool True if details are valid; false if not valid.
     */
    public function validate_details($username = '', $password_hash = '') {
        $q = dbquery("SELECT null FROM characters WHERE username = '"
                . filter_for_input($username) . "' AND password = '" . $password_hash . "' LIMIT 1");
        if(mysql_num_rows($q) > 0) {
            return true;
        }
        return false;
    }

    /**
     * Gets the given user id's server rights.
     * @param int $user_id The user's id.
     * @return int The specified user's server rights.
     */
    public function get_server_rights($user_id) {
        if(is_numeric($user_id)) {
            $q = dbquery("SELECT server_rights FROM characters WHERE id = '" . filter_for_input($user_id) . "' LIMIT 1");
            if(mysql_num_rows($q) > 0) {
                $row = mysql_fetch_assoc($q);
                return $row['server_rights'];
            }
        }
        return 0;
    }

    /**
     * Gets the given uesr id's client rights.
     * @param int $user_id The user's id.
     * @return int The specified user's client rights.
     */
    public function get_client_rights($user_id) {
        if(is_numeric($user_id)) {
            $q = dbquery("SELECT client_rights FROM characters WHERE id = '" . filter_for_input($user_id) . "' LIMIT 1");
            if(mysql_num_rows($q) > 0) {
                $row = mysql_fetch_assoc($q);
                return $row['client_rights'];
            }
        }
        return 0;
    }

    /**
     * Gets the specified username's connected master id from the database.
     * @param string $username The username to search for to get id.
     * @return id The id that's connected to the given username.
     */
    public function get_id($username) {
        $q = dbquery("SELECT id FROM characters WHERE username = '" . filter_for_input($username) . "' LIMIT 1");

        if(mysql_num_rows($q) > 0) {
            $row = mysql_fetch_assoc($q);
            return $row['id'];
        }

        return -1;
    }

    /**
     * Gets the id's connected username from the database.
     * @param int $user_id The user id to search for to get username.
     * @return string The name that's connected to the given id.
     */
    public function get_name($user_id) {
        if(is_numeric($user_id)) {
            $q = dbquery("SELECT username FROM characters WHERE id = '" . filter_for_input($user_id) . "' LIMIT 1");

            if(mysql_num_rows($q) > 0) {
                $row = mysql_fetch_assoc($q);
                return $row['username'];
            }
        }

        return "";	
    }
}
?>
