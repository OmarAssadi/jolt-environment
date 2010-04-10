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

<?php
if (isset($_GET['view'])) {
    $staff_id = $_GET['view'];
    $qry = dbquery("SELECT * FROM characters WHERE id = '$staff_id' AND server_rights >= 2 LIMIT 1;");

    if (mysql_num_rows($qry) > 0) {
        $staff_vars = mysql_fetch_assoc($qry);
        ?>
<fieldset>
    <dl>
        <dt><label>Name</label></dt>
        <dd><strong><?php echo ucwords($staff_vars['username']); ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Client rights</label></dt>
        <dd><strong><?php echo get_client_rights($staff_vars['client_rights']); ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Server rights</label></dt>
        <dd><strong><?php echo get_rights($staff_vars['server_rights']); ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Online</label></dt>
        <dd><strong><?php echo $staff_vars['online'] == 1 ? "Yes" : "No"; ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Active</label></dt>
        <dd><strong><?php echo $staff_vars['active'] == 1 ? "Yes" : "No"; ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Email</label></dt>
        <dd><strong><a href='mailto:<?php echo $staff_vars['email']; ?>'><?php echo $staff_vars['email'] ?></a></strong></dd>
    </dl>
    <dl>
        <dt><label>Register date</label></dt>
        <dd><strong><?php echo $staff_vars['register_date']; ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Register IP</label></dt>
        <dd><strong><?php echo $staff_vars['register_ip']; ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Last IP</label></dt>
        <dd><strong><?php echo $staff_vars['last_ip']; ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Last signin</label></dt>
        <dd><strong><?php echo $staff_vars['last_signin']; ?></strong></dd>
    </dl>




    <p class="quick">
        <input class="button1" type="submit" value=" Edit User "
               onclick="parent.location='edituser.php?id=<?php echo $staff_id; ?>'" />
        <input class="button1" type="submit" value=" Staff Overview "
               onclick="parent.location='staff.php'" />
    </p>
</fieldset> 
        <?php
    } else {
        acp_error("The specified user does not exist or is not staff.");
    }
} else { ?>
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
                <td><strong><a href='staff.php?view=" . $person['id'] . "'>" . $users->format_name($person['username']) ."</a></strong></td>
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
    <?php } ?>

<?php require_once("footer.php"); ?>
