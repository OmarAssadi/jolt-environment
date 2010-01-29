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
 * Sets admincp default vars.
 */
function set_default_vars() {
    define('ACP_LOGGED_IN', false);
    define('ACP_NAME', 'Guest');
    define('ACP_HASH', '');
    define('ACP_UID', 0);
    define('ACP_RIGHTS', 0);
}

/**
 * Checks the rights required to access to the page.
 * @param int $rights Minimum rights require to check against user.
 */
function check_rights($rights = 1) {
    if (!ADMINCP) {
        exit;
    }
    if (!ACP_LOGGED_IN) {
        header("Location: login.php");
        exit;
    }
    if (ACP_RIGHTS < $rights) {
        header("Location: 403.php");
        exit;
    }
}

/**
 * Formats a given size.
 * @param int $data The data to format.
 * @return string The formatted size.
 */
function formatsize($data) {
    if( $data < 1024 ) {
        return $data . ' Bytes';
    } else if( $data < 1024000 ) {
        return round( ( $data / 1024 ), 2 ) . ' KiB';
    } else {
        return round( ( $data / 1024000 ), 2 ) . ' MiB';
    }
}

/**
 * Adds the specified log into the database.
 * @param int $user The user to log for.
 * @param string $user_ip The user's ip.
 * @param string $message The log's message.
 */
function add_log($user, $ip, $message) {
    dbquery("INSERT INTO web_acp_logs (user, user_ip, log_time, log_message) VALUES ('" . $user . "', '" . $ip . "', NOW(), '" . $message . "');");
}

/**
 * Prints banner with a success message on top of the page.
 * @param string $message The message to be shown in the banner.
 */
function acp_success($message) {
    echo("<div class='successbox'>" . $message . "<p></p></div>");
}

/**
 * Prints banner with a notice message on top of the page.
 * @param string $message The message to be shown in the banner.
 */
function acp_notice($message) {
    echo("<div class='successbox notice'>" . $message . "<p></p></div>");
}

/**
 * Prints banner with a error message on top of the page.
 * @param string $message The message to be shown in the banner.
 */
function acp_error($message) {
    echo("<div class='errorbox'>" . $message . "<p></p></div>");
}
?>
