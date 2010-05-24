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
define('ACP_TITLE', 'Admin Logs');
define('ACP_TAB', 5);
require_once("adminglobal.php");
check_rights();

$show = 50;

if (isset($_GET['page'])) {
    $page = $_GET['page'];
} else {
    $page = 1;
}

if (isset($_GET['viewall'])) {
    $total_logs = dbevaluate("SELECT COUNT(id) FROM web_acp_logs;");
    $total_pages = ceil($total_logs / $show);
    $tmp_key = $_POST['key'];
    $sp = "<span>";

    $i2 = $page - 5;
    $i3 = $page + 5;
    if ($i2 < 1) {
        $i2 = 1;
    }
    if ($i3 > $total_pages) {
        $i3 = $total_pages;
    }

    for ($i = $i2; $i <= $i3; $i++) {
        if ($i == $page) {
            $sp .= "<strong>$page</strong><span class='page-sep'>,</span>";
        } else {
            $sp .= "<a href='adminlogs.php?viewall&page=$i'>$i</a>";
        }
    }
    if ($page > $total_pages) {
        $page = 1;
    }
    $next_page = $page + 1;
    if ($next_page > $total_pages) {
        $next_page = $total_pages;
    }
    $sp .= "&nbsp;&nbsp;<a href='adminlogs.php?viewall&page=" . $next_page . "'>Next</a></span>";
} else if (isset($_GET['key'])) {
    $key = $_GET['key'];
    $key = str_replace(' ', '_', strtolower(trim($key)));

    if ($key == "") {
        header("Location: adminlogs.php?viewall");
    }
    
    $total_logs = dbevaluate("SELECT COUNT(id) FROM web_acp_logs WHERE user = '$key';");
    $total_pages = ceil($total_logs / $show);
    $tmp_key = $_POST['key'];
    $sp = "<span>";

    $i2 = $page - 5;
    $i3 = $page + 5;
    if ($i2 < 1) {
        $i2 = 1;
    }
    if ($i3 > $total_pages) {
        $i3 = $total_pages;
    }

    for ($i = $i2; $i <= $i3; $i++) {
        if ($i == $page) {
            $sp .= "<strong>$page</strong><span class='page-sep'>,</span>";
        } else {
            $sp .= "<a href='adminlogs.php?key=$key&page=$i'>$i</a>";
        }
    }

    $next_page = $page + 1;
    if ($next_page > $total_pages) {
        $next_page = $total_pages;
    }
    $sp .= "&nbsp;&nbsp;<a href='adminlogs.php?key=$key&page=" . $next_page . "'>Next</a></span>";
}

require_once("header.php");
?>
<h1>Admin Logs</h1><hr>
<p>Staff activity is always logged in the AdminCP. You can see all logged activity here.</p><br />

<?php if (isset($_GET['viewall'])) { ?>

<form method="get">
    <fieldset class="display-options" style="float: left">
	Search by name or ip:
        <input type="text" name="key" value="" />&nbsp;
        <input type="submit" class="button2" value="Search" />
    </fieldset>
</form>

<div class="pagination" style="float: right; margin: 15px 0 2px 0">
        <?php echo "Page " . $page . " of " . $total_pages; ?>
    &bull;
        <?php echo $sp; ?>
</div>

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
            $start_from = ($page - 1) * $show;
            $logs = dbquery("SELECT * FROM web_acp_logs ORDER BY id DESC LIMIT $start_from, $show");

            if (mysql_num_rows($logs) > 0) {
                while ($log = mysql_fetch_assoc($logs)) {
                    echo "<tr>
                <td><strong><a href='viewuser.php?name=" . $log['user'] . "'>" . $users->format_name($log['user']) ."</a></strong></td>
                <td>" . $log['user_ip'] ."</td>
                <td>" . $log['log_time'] ."</td>
                <td><strong>" . $log['log_message'] ."</strong></td>
                </tr>";
                }
            } else {
                echo "<tr><td colspan='5' style='text-align: center;'>No logs found.</td></tr>";
            }
            ?>
    </tbody>
</table>
    <?php } else if (isset($_GET['key'])) { ?>
<form method="get">
    <fieldset class="display-options" style="float: left">
	Search by name or ip:
        <input type="text" name="key" value="<?php echo $key ?>" />&nbsp;
        <input type="submit" class="button2" value="Search" />
    </fieldset>
</form>

<div class="pagination" style="float: right; margin: 15px 0 2px 0">
        <?php echo "Page " . $page . " of " . $total_pages; ?>
    &bull;
        <?php echo $sp; ?>
</div>

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
            $start_from = ($page - 1) * $show;
            $logs = dbquery("SELECT * FROM web_acp_logs WHERE user = '$key' OR user_ip = '$key' ORDER BY id DESC LIMIT $start_from, $show");

            if (mysql_num_rows($logs) > 0) {
                while ($log = mysql_fetch_assoc($logs)) {
                    echo "<tr>
                <td><strong><a href='viewuser.php?id=" . $log['user'] . "'>" . $log['user'] ."</a></strong></td>
                <td>" . $log['user_ip'] ."</td>
                <td>" . $log['log_time'] ."</td>
                <td><strong>" . $log['log_message'] ."</strong></td>
                </tr>";
                }
            } else {
                echo "<tr><td colspan='5' style='text-align: center;'>No logs found.</td></tr>";
            }
            ?>
    </tbody>
</table>
    <?php } ?>

<?php require_once("footer.php"); ?>