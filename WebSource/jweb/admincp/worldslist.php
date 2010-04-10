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
define('ACP_TITLE', 'Worlds');
define('ACP_TAB', 2);
require_once("adminglobal.php");
check_rights();

require_once("header.php");
?>
<h1>Current Worlds</h1><hr>
<p>A list of worlds that are currently availible for the server.</p><br>

<table cellspacing="1">
    <thead>
        <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Last Startup</th>
            <th>Actions</th>
        </tr>
    </thead>
    <tbody>
        <?php
        $worlds = dbquery("SELECT world_id,world_name,startup_time FROM worlds ORDER BY world_id DESC");

        if (mysql_num_rows($worlds) > 0) {
            while ($world = mysql_fetch_assoc($worlds)) {
                echo "            <tr>
                <td><strong>" . $world['world_id'] ."</strong></td>
                <td>" . $world['world_name'] ."</td>
                <td>" . $world['startup_time'] ."</td>
                <td><strong><a href='worldedit.php?id=" . $world['world_id'] . "'><img src='./images/icon_edit.gif' alt='Edit' /></a></strong></td>
            </tr>";
            }
        } else {
            echo "<tr><td colspan='5' style='text-align: center;'>No worlds have been created.</td></tr>";
        }
        ?>
    </tbody>
</table>

<img  />

<?php require_once("footer.php"); ?>
