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
define('ACP_TITLE', 'Staff');
define('ACP_TAB', 5);
require_once("adminglobal.php");
check_rights();

require_once("header.php");
?>
<h1>Staff Members</h1><hr>
<p>A list of staff members who have certain rights around the AdminCP and in game.</p><br />

<table cellspacing="1">
    <thead>
        <tr>
            <th>Username</th>
            <th>Rights</th>
            <th>Email</th>
            <th>Online</th>
        </tr>
    </thead>
    <tbody>
            <?php
            $staff = dbquery("SELECT id,username,server_rights,email,online FROM characters WHERE server_rights >= 2 ORDER BY username");

            if (mysql_num_rows($staff) > 0) {
                while ($person = mysql_fetch_assoc($staff)) {
                    echo "            <tr>
                <td><strong><a href='viewuser.php?id=" . $person['id'] . "'>" . $users->format_name($person['username']) ."</a></strong></td>
                <td>" . get_rights($person['server_rights']) ."</td>
                <td><a href='mailto:" . $person['email'] . "'>" . $person['email'] ."</a></td>
                <td>" . ($person['online'] == 1 ? "Yes" : "No") ."</td>
            </tr>";
                }
            } else {
                echo "<tr><td colspan='5' style='text-align: center;'>No logs have been created.</td></tr>";
            }
            ?>
    </tbody>
</table>

<?php require_once("footer.php"); ?>
