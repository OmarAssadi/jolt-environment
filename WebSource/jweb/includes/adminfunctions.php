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
?>
