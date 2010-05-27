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
define('ACP_TITLE', 'Offences');
define('ACP_TAB', 4);
require_once("adminglobal.php");
check_rights();

require_once("header.php");
?>
<h1>User Management</h1><hr>
<p>Allows you to view and edit user information, and much more.</p><br />


<h2>Registered Users</h2>
<p>A list containing the 5 newest users.</p>

<table cellspacing="1">
    <thead>
        <tr>
            <th>Username</th>
            <th>Registration Date</th>
            <th>Registration IP</th>
            <th>Active</th>
            <th>Online</th>
        </tr>
    </thead>
    <tbody>
        <?php
        $users = dbquery("SELECT username,online,active,register_ip,register_date FROM characters ORDER BY id DESC LIMIT 5");
        $bool = true;

        if (mysql_num_rows($users) > 0) {
            while ($user = mysql_fetch_assoc($users)) {
                echo "
                <tr>
                    <td><strong><a href='viewuser.php?name=" . $user['username'] . "'>" . $user['username'] . "</a></strong></td>
                    <td>" . $user['register_date'] . "</td>
                    <td>" . $user['register_ip'] . "</td>
                    <td>" . yesno($user['active']) . "</td>
                    <td>" . yesno($user['online']) . "</td>
                </tr>
                ";
                $bool = !$bool;
            }
        } else {
            echo "<tr><td colspan='5' style='text-align: center;'>No notes have been created.</td></tr>";
        }
        ?>
</tbody>
</table>
<br />

<h2>Online Users</h2>
<p>A list containing all the online users</p>

<table cellspacing="1">
    <thead>
        <tr>
            <th>Username</th>
            <th>IP</th>
        </tr>
    </thead>
    <tbody>
        <?php
        $users = dbquery("SELECT username,last_ip FROM characters WHERE online='1' ORDER BY username;");
        $bool = true;

        if (mysql_num_rows($users) > 0) {
            while ($user = mysql_fetch_assoc($users)) {
                echo "
                <tr>
                    <td><strong><a href='viewuser.php?name=" . $user['username'] . "'>" . $user['username'] . "</a></strong></td>
                    <td>" . $user['last_ip'] . "</td>
                </tr>
                ";
                $bool = !$bool;
            }
        } else {
            echo "<tr><td colspan='5' style='text-align: center;'>No users online.</td></tr>";
        }
        ?>
</tbody>
</table>
<br />

<?php require_once("footer.php"); ?>
