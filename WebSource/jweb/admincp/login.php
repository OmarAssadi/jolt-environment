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

require_once("adminglobal.php");

/*
 * This is checked against the given login details,
 * and if details do not match database details,
 * this will be true and shows "wrong password".
 */
$login_failed = false;

if (ACP_LOGGED_IN) {
    header("Location: dashboard.php");
    exit;
}

if (isset($_POST['user']) && isset($_POST['pass'])) {
    $user = strtolower($_POST['user']);
    $pass = sha1($_POST['user'] . sha1(($_POST['pass'])));
    
    if ($users->validate_details($user, $pass)) {
        if ($users->get_server_rights($users->get_id($user)) > 1) {
            $_SESSION['JWEB_ACP_USER'] = $user;
            $_SESSION['JWEB_ACP_HASH'] = $pass;

            die('<script type="text/javascript">top.location.href = \'adminglobal.php\';</script>');
        }
    } else {
        $login_failed = true;
    }
}

if ($login_failed) {
    print("failed.");
}
?>

		<form method="post" action="login.php">

		<p>
			Username:<br />
			<input type="text" name="user" value=""><br />
		</p>

		<p>
			Password:<br />
			<input type="password" name="pass" value=""><br />
		</p>

		<p>
			<input type="submit" value="Log in">
		</p>

		</form>