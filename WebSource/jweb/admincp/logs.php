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
define('ACP_TAB', 5);

require_once("adminglobal.php");
check_rights();

if (isset($_GET['type'])) {
    $type = $_GET['type'];
    if ($type == "admin") {
        define('ACP_TITLE', 'Administration Logs');
    } else if ($type == "chat") {
        define('ACP_TITLE', 'Chat Logs');
    } else if ($type == "ip") {
        define('ACP_TITLE', 'IP Logs');
    } else if ($type == "login") {
        define('ACP_TITLE', 'Login Logs');
    }
}

if (isset($_GET['page'])) {
    $page = $_GET['page'];
} else {
    $page = 1;
}

$total_logs = dbevaluate("SELECT COUNT(id) FROM web_acp_logs;");
$total_pages = ceil($total_logs / 10);
$tmp_key = $_POST['key'];
$sp = "<span>";

for ($i = 1; $i <= $total_pages; $i++) {
    if ($i == $page) {
        $sp .= "<strong>$page</strong><span class='page-sep'>,</span>";
    } else {
        $sp .= "<a href='logs.php?type=$type&page=$i'>$i</a>";
    }
}

$next_page = $page + 1;
if ($next_page > $total_pages) {
    $next_page = $total_pages;
}
$sp .= "&nbsp;&nbsp;<a href='logs.php?type=$type&page=" . $next_page . "'>Next</a></span>";



include_once("header.php");
?>

<h1>Logs</h1><hr>
<p>Activity is always logged. You can browser through all logged activities here.</p><br />

<?php if ($type == "admin") { ?>
<h2>Administration Logs</h2>
<p>This gives an overview of the last five actions carried out by staff.</p>

<form action="logs.php?type=admin" method="post">
    <fieldset class="display-options" style="float: left">
	Search by name or ip:
        <input type="text" name="key" value="<?php echo $_POST['key'] ?>" />&nbsp;
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
            $start_from = ($page - 1) * 10;

            if (isset($_POST['key'])) {
                $key = $_POST['key'];
                $logs = dbquery("SELECT * FROM web_acp_logs WHERE user = '$key' OR user_ip = '$key' ORDER BY id DESC LIMIT $start_from, 10");
            } else {
                $logs = dbquery("SELECT * FROM web_acp_logs ORDER BY id DESC LIMIT $start_from, 10");
            }

            if (mysql_num_rows($logs) > 0) {
                while ($log = mysql_fetch_assoc($logs)) {
                    echo "            <tr>
                <td><strong>" . $log['user'] ."</strong></td>
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

<?php include_once("footer.php"); ?>
