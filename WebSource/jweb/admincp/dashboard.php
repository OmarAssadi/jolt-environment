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

define('ACP_TITLE', 'Dashboard');
define('ACP_TAB', 1);

require_once("adminglobal.php");
check_rights();
require_once("header.php");

$stat['server_version'] = dbevaluate("SELECT server_version FROM versioning;");
$stat['website_version'] = dbevaluate("SELECT website_version FROM versioning;");
$stat['database_version'] = dbevaluate("SELECT database_version FROM versioning;");
$stat['client_version'] = dbevaluate("SELECT client_version FROM versioning;");
$stat['user_count'] = dbevaluate("SELECT COUNT(id) FROM characters;");
$stat['inactive_users'] = dbevaluate("SELECT COUNT(id) FROM characters WHERE active = '0';");
$stat['users_online'] = dbevaluate("SELECT COUNT(id) FROM characters WHERE online = '1';");
$stat['staff_members'] = dbevaluate("SELECT COUNT(id) FROM characters WHERE server_rights >= '1';");
$stat['last_startup'] = dbevaluate("SELECT startup_time FROM worlds ORDER BY startup_time LIMIT 1");
$stat['mysql_version'] = dbevaluate("SELECT version();");
$stat['world_count'] = dbevaluate("SELECT COUNT(world_id) FROM worlds");

$result = dbquery("SHOW TABLE STATUS");
while($row = mysql_fetch_array($result)) {
    $db_size += $row["Data_length"] + $row["Index_length"];
}

$db_size = formatsize($db_size);

/**
 * Check if any notes are trying to be added.
 */
if (isset($_GET['add_note'])) {
    $note = filter_for_input($_GET['add_note']);

    if (strlen($note) < 3 || strlen($note) > 100) {
        acp_error("Unable to add note.");
    } else {
        dbquery("INSERT INTO web_acp_notes (user,note_date,note_message) VALUES ('" . ACP_NAME . "', NOW(), '" . $note . "')");
        add_log(ACP_NAME, USER_IP, "Added a note.");
        acp_success("Successfully added note.");
    }
}

/**
 * Attempts to delete the specified note id.
 */
if (isset($_GET['delete_note'])) {
    if (ACP_RIGHTS > 2) {
        dbquery("DELETE FROM web_acp_notes WHERE id = '" . $_GET['delete_note'] . "'");
        add_log(ACP_NAME, USER_IP, "Deleted a note.");
        acp_success("Successfully deleted note.");
    } else {
        acp_error("Moderators cannot delete notes.");
    }
}

/**
 * Check if access denied error has redirected the user to this page.
 */
if (isset($_GET['access_denied'])) {
    acp_error("You do not have permissions to view that page.");
}

?>
<h1>Dashboard</h1><hr>
<p>Welcome to the administration backend. You can find tools for managing the game, server, website, and all the configurations. Depending on your given priviledges, you will only be able to access certain controls within the administration.</p><br />

<?php if (is_int($core->get_config("remote.port"))) {
    echo "fail";
} ?>


<h2>Server statistics</h2>
<table cellspacing="1">
    <col class="col1" /><col class="col2" /><col class="col1" /><col class="col2" />
    <thead>
        <tr>
            <th>Statistic</th>
            <th>Value</th>
            <th>Statistic</th>
            <th>Value</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Registered users: </td>
            <td><strong><?php echo $stat['user_count']; ?></strong></td>
            <td>Server version: </td>
            <td><strong><?php echo $stat['server_version']; ?></strong></td>
        </tr>
        <tr>
            <td>Inactiveusers: </td>
            <td><strong><?php echo $stat['inactive_users']; ?></strong></td>
            <td>Website version: </td>
            <td><strong><?php echo $stat['website_version']; ?></strong></td>
        </tr>
        <tr>
            <td>Users online: </td>
            <td><strong><?php echo $stat['users_online']; ?></strong></td>
            <td>Database version: </td>
            <td><strong><?php echo $stat['database_version']; ?></strong></td>
        </tr>
        <tr>
            <td>Staff members: </td>
            <td><strong><?php echo $stat['staff_members']; ?></strong></td>
            <td>Client version: </td>
            <td><strong><?php echo $stat['client_version']; ?></strong></td>
        </tr>
        <tr>
            <td>Last startup: </td>
            <td><strong><?php echo $stat['last_startup']; ?></strong></td>
            <td>Database version: </td>
            <td><strong><?php echo "MySQL " . $stat['mysql_version']; ?></strong></td>
        </tr>
        <tr>
            <td>Worlds: </td>
            <td><strong><?php echo $stat['world_count']; ?></strong></td>
            <td>Database size: </td>
            <td><strong><?php echo $db_size; ?></strong></td>
        </tr>
    </tbody>
</table>
<br />

<h2>Administration Notes</h2>
<p>Notes and general chatting between staff can be done with this.</p>
<div style="float: right;"><a href="adminnotes.php?viewall">&raquo; View administration notes</a></div>

<table cellspacing="1">
    <thead>
        <tr>
            <th>Username</th>
            <th>Date</th>
            <th>Message</th>
        </tr>
    </thead>
    <tbody>
        <?php
        $notes = dbquery("SELECT * FROM web_acp_notes ORDER BY id DESC LIMIT 10");
        $notebool = true;

        if (mysql_num_rows($notes) > 0) {
            while ($note = mysql_fetch_assoc($notes)) {
                echo "
                <tr>
                    <td><a href='dashboard.php?delete_note=" . $note['id'] . "'><img src='./images/icon_delete.gif' /></a> <a href='viewuser.php?name=" . $note['user'] . "'><strong>" . $users->format_name($note['user']) . "</strong></a></td>
                    <td style='text-align: center;'>" . $note['note_date'] . "</td>
                    <td>" . filter_for_outout($note['note_message'], true) . "</td>
                </tr>
                ";
                $notebool = !$notebool;
            }
        } else {
            echo "<tr><td colspan='5' style='text-align: center;'>No notes have been created.</td></tr>";
}
?>
        <tr>
    <form method="get" action="dashboard.php">
        <td colspan='5' style='text-align: center;'>
            <input id="note" name="add_note" size="50"/>
            <input class="button1" type="submit" id="submit" value="Post Note" />
        </td>
    </form>
</tr>
</tbody>
</table>
<br />

<h2>Administration Logs</h2>
<p>This gives an overview of the last five actions carried out by staff.</p>
<div style="float: right;"><a href="adminlogs.php?viewall">&raquo; View administration logs</a></div>

<table cellspacing="1">
    <thead>
        <tr>
            <th>Username</th>
            <th>User IP</th>
            <th>Time</th>
            <th>Action</th>
        </tr>
    </thead>
    <tbody>
        <?php
        $logs = dbquery("SELECT * FROM web_acp_logs ORDER BY id DESC LIMIT 5");

        if (mysql_num_rows($logs) > 0) {
            while ($log = mysql_fetch_assoc($logs)) {
                echo "            <tr>
                <td><strong><a href='viewuser.php?name=" . $log['user'] . "'>" . $users->format_name($log['user']) ."</a></strong></td>
                <td style='text-align: center;'>" . $log['user_ip'] ."</td>
                <td style='text-align: center;'>" . $log['log_time'] ."</td>
                <td><strong>" . $log['log_message'] ."</strong></td>
            </tr>";
            }
        } else {
            echo "<tr><td colspan='5' style='text-align: center;'>No logs have been created.</td></tr>";
}
?>
    </tbody>
</table>

<?php include_once("footer.php"); ?>
