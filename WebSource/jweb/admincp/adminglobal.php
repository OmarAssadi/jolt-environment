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

define('ADMINCP', true);

require_once("../global.php");
require_once(INCLUDES . "adminfunctions.php");

if (!defined('JWEB')) {
    exit;
}
/*
 * Attempt to find any registered sessions.
*/
if (isset($_SESSION['JWEB_ACP_USER']) && isset($_SESSION['JWEB_ACP_HASH'])) {
    $user = $_SESSION['JWEB_ACP_USER'];
    $hash = $_SESSION['JWEB_ACP_HASH'];
    $user_id = $users->get_id($_SESSION['JWEB_ACP_USER']);
    $user_rights = $users->get_server_rights($user_id);

    if ($users->validate_details($user, $hash)) {
        if ($user_rights >= 1) {
            define('ACP_LOGGED_IN', true);
            define('ACP_NAME', $user);
            define('ACP_HASH', $hash);
            define('ACP_UID', $user_id);
            define('ACP_RIGHTS', $user_rights);
        } else {
            @session_destroy();
            set_default_vars();
        }
    } else {
        @session_destroy();
        set_default_vars();
    }
} else {
    set_default_vars();
}

?>
