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
    $user = str_replace(' ', '_', strtolower($_POST['user']));
    $pass = sha1($user . sha1(($_POST['pass'])));

    if ($users->validate_details($user, $pass)) {
        if ($users->get_server_rights($users->get_id($user)) > 2) {
            $_SESSION['JWEB_ACP_USER'] = $user;
            $_SESSION['JWEB_ACP_HASH'] = $pass;

            add_log($user, USER_IP, "Successfully logged in.");
            die('<script type="text/javascript">top.location.href = \'dashboard.php\';</script>');
        } else {
            $login_failed = true;
        }
    } else {
        $login_failed = true;
    }
}
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" dir="ltr" lang="en-gb" xml:lang="en-gb">
    <head>

        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
        <meta http-equiv="Content-Style-Type" content="text/css" />
        <meta http-equiv="Content-Language" content="en-gb" />
        <meta http-equiv="imagetoolbar" content="no" />

        <title>AdminCP - Login</title>

        <link href="./style/admin.css" rel="stylesheet" type="text/css" media="screen" />
    </head>
    <body class="ltr">
        <div id="wrap">
            <div id="page-body">
                <div id="acp">
                    <div class="panel">
                        <span class="corners-top"><span></span></span>
                        <div id="content">
                            <div id="menu">
                                <ul>
                                    <li><a href="../"><span>Back to homepage</span></a></li>
                                </ul>
                            </div>
                            <div id="main" class="install-body">

                                <h1>jWeb Administration Panel</h1>
                                <p>
                                    Please login below with your moderator/administrator credentials as given. Players who do not have staff priviledges should not attempt to login here. All ip's are logged as of every login attempt, and will be kept to check for unauthorized attempts. Login wisely.
                                </p>

                                <?php if ($login_failed) {
                                    acp_error("Invalid username, password or rights.");
                                } ?>

                                <br />
                                <form method="post" action="login.php" onsubmit="submit.disabled = 'disabled';">

                                    <fieldset>
                                        <dl>
                                            <dt><label for="userinput">Username:</label></dt>
                                            <dd><input id="userinput" type="text" name="user" /></dd>
                                        </dl>
                                        <dl>
                                            <dt><label for="passinput">Password:</label></dt>
                                            <dd><input id="passinput" type="password" name="pass" /></dd>
                                        </dl>
                                    </fieldset>

                                    <fieldset class="submit-buttons">
                                        <legend>Proceed to dashboard</legend>
                                        <input class="button1" type="submit" id="submit" onclick="this.className = 'button1 disabled';" name="submit" value="  Proceed to dashboard  " />
                                    </fieldset>
                                </form>
                            </div>
                            <?php include_once("footer.php"); ?>