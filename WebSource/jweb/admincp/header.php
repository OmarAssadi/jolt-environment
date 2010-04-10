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

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" dir="ltr" lang="en-gb" xml:lang="en-gb">
    <head>

        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
        <meta http-equiv="Content-Style-Type" content="text/css" />
        <meta http-equiv="Content-Language" content="en-gb" />
        <meta http-equiv="imagetoolbar" content="no" />

        <title>AdminCP<?php if(defined('ACP_TITLE')) echo " - " . ACP_TITLE ?></title>

        <link href="./style/admin.css" rel="stylesheet" type="text/css" media="screen" />
    </head>
    <body class="ltr">
        <div id="wrap">
            <div id="page-body">
                <div id="tabs">
                    <ul>
                        <li <?php if (ACP_TAB == 1) echo "id='activetab'"; ?>><a href="dashboard.php"><span>General</span></a></li>
                        <li <?php if (ACP_TAB == 2) echo "id='activetab'"; ?>><a href="server.php"><span>Server</span></a></li>
                        <li <?php if (ACP_TAB == 3) echo "id='activetab'"; ?>><a href="website.php"><span>Website</span></a></li>
                        <li <?php if (ACP_TAB == 4) echo "id='activetab'"; ?>><a href="users.php"><span>Users</span></a></li>
                        <li <?php if (ACP_TAB == 5) echo "id='activetab'"; ?>><a href="moderation.php"><span>Moderation</span></a></li>
                    </ul>
                </div>
                <div id="acp">
                    <div class="panel">
                        <span class="corners-top"><span></span></span>
                        <div id="content">
                            <div id="menu">
                                <p>Logged in as:<br /><strong><?php echo ACP_NAME ?></strong> [&nbsp;<a href="logout.php">Logout</a>&nbsp;]</p>

                                <!-- General !-->
                                <?php if (ACP_TAB == 1) { ?>
                                <ul>
                                    <li class="header">Quick access</li>
                                    <li><a href="dashboard.php"><span>Dashboard</span></a></li>
                                    <li><a href="../index.php"><span>View Site</span></a></li>
                                </ul>

                                <!-- Server !-->
                                <?php } else if (ACP_TAB == 2) { ?>
                                <ul>
                                    <li class="header">General</li>
                                    <li><a href="server.php"><span>Information</span></a></li>
                                    <li><a href="gensettings.php"><span>Settings</span></a></li>

                                    <li class="header">Worlds</li>
                                    <li><a href="worldslist.php"><span>Current worlds</span></a></li>
                                    <li><a href="createworld.php"><span>Create world</span></a></li>

                                    <li class="header">Remote</li>
                                    <li><a href="rmcpingtest.php"><span>RMC Ping</span></a></li>
                                </ul>

                                <!-- Website !-->
                                <?php } else if (ACP_TAB == 3) { ?>
                                <ul>
                                    <li class="header">Quick access</li>
                                    <li><a href="dashboard.php"><span>Dashboard</span></a></li>
                                    <li><a href="../index.php"><span>View Site</span></a></li>

                                    <li class="header">Information</li>
                                    <li><a href="dashboard.php"><span>Dashboard</span></a></li>
                                    <li><a href="../index.php"><span>View Site</span></a></li>
                                </ul>

                                <!-- Users !-->
                                <?php } else if (ACP_TAB == 4) { ?>
                                <ul>
                                    <li class="header">Quick access</li>
                                    <li><a href="dashboard.php"><span>Dashboard</span></a></li>
                                    <li><a href="../index.php"><span>View Site</span></a></li>

                                    <li class="header">Information</li>
                                    <li><a href="dashboard.php"><span>Dashboard</span></a></li>
                                    <li><a href="../index.php"><span>View Site</span></a></li>
                                </ul>

                                <!--Moderation !-->
                                <?php } else if (ACP_TAB == 5) { ?>
                                <ul>
                                    <li class="header">Overview</li>
                                    <li><a href="moderation.php"><span>Overview</span></a></li>
                                    <li><a href="#"><span>Staff Members</span></a></li>

                                    <li class="header">Reports</li>
                                    <li><a href="dashboard.php"><span>Reports</span></a></li>
                                    <li><a href="../index.php"><span>Resolved Reports</span></a></li>

                                    <li class="header">Offenses</li>
                                    <li><a href="#"><span>Banned Characters</span></a></li>
                                    <li><a href="#"><span>Banned Addresses</span></a></li>
                                    <li><a href="#"><span>Muted Characters</span></a></li>

                                    <li class="header">Logs</li>
                                    <li><a href="adminlogs.php?viewall"><span>Admin Logs</span></a></li>
                                    <li><a href="#"><span>Chat Logs</span></a></li>
                                    <li><a href="#"><span>IP Logs</span></a></li>
                                    <li><a href="#"><span>Login Logs</span></a></li>
                                </ul>
                                <?php } ?>
                             </div>
                            <div id="main">

