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
define('ACP_TAB', 5);
require_once("adminglobal.php");
check_rights();

require_once("header.php");
?>
<h1>Active Offenses</h1><hr>
<p>A list of current offenses. This page allows you to see the characters banned, 
    type of ban, how long they are banned for, why they are banned, and who banned them.</p><br />

<form method="get">
    <fieldset class="display-options" style="float: left">
	Search by name:
        <input type="text" name="key" value="" />&nbsp;
        <input type="submit" class="button2" value="Search" />
    </fieldset>
</form>

<table cellspacing="1">
    <thead>
        <tr>
            <th>Username</th>
            <th>Type</th>
            <th>Date</th>
            <th>Expire Date</th>
            <th>Appealed</th>
            <th>Manage</th>
        </tr>
    </thead>
    <tbody>
            <?php
            $logs = dbquery("SELECT * FROM offences ORDER BY id");

            if (mysql_num_rows($logs) > 0) {
                while ($log = mysql_fetch_assoc($logs)) {
                    $appealed = ($log['appeal_data'] != "" ? "Yes" : "No");
                    if ($log['appeal_status'] == 1) {
                        $appealed = "Yes, denied";
                    }
                    echo "<tr>
                <td>" . $users->get_name($log['userid']) . "</td>
                <td>" . get_type($log['type']) . "</td
                <td>" . $log['date'] . "</td>
                <td>" . $log['expire_date'] . "</td>
                <td>" . $appealed . "</td>
                <td><a href='manageoffence.php?id=" . $log['id'] . "'><img src='./images/icon_edit.gif' alt='Edit' /></a></td>
                </tr>";
                }
            } else {
                echo "<tr><td colspan='5' style='text-align: center;'>No offences found.</td></tr>";
            }

            function yesno($data) {
                if ($data == 0) {
                    return "No";
                } else if ($data == 1) {
                    return "Yes";
                }
            }
            ?>
</tbody>
</table>

<?php require_once("footer.php"); ?>
