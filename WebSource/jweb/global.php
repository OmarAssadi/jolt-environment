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

// System definitions.
define('JWEB', true);
define('DEBUG_MODE', false);
define('USER_IP', $_SERVER['REMOTE_ADDR']);

// Site configurations.
define('SITE_NAME', 'RageScape');
define('SITE_MOTTO', 'Join the revolution.');

// Directory workspaces.
define('CWD', str_replace('admincp' . DIRECTORY_SEPARATOR, '', dirname(__FILE__) . DIRECTORY_SEPARATOR));
define('INCLUDES', CWD . 'includes' . DIRECTORY_SEPARATOR);

// #############################################################################
set_magic_quotes_runtime(0);
error_reporting(E_ALL ^ E_NOTICE);

// #############################################################################
if (DEBUG_MODE) {

    // We don't want customers to see errors / bugs while debugging!
    if (USER_IP != "127.0.0.1" && USER_IP != "::1") {
        trigger_error("This script does not process external requests while in debug mode. Aborting.", E_USER_ERROR);
	exit;
    }

    error_reporting(E_ALL);
}

// #############################################################################
session_start();

// #############################################################################
require_once(INCLUDES . "class_core.php");
require_once(INCLUDES . "class_database.php");
require_once(INCLUDES . "class_users.php");

$core = new core();
$core->parse_configs();

$database = new database(
        $core->get_config("database.host"),
        $core->get_config("database.user"),
        $core->get_config("database.pass"));
$database->connect($core->get_config("database.name"));

$users = new users();

// #############################################################################
require_once(INCLUDES . "functions_database.php");
require_once(INCLUDES . "functions_filtering.php");
?>
