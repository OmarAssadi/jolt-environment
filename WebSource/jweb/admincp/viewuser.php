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
define('ACP_TAB', 4);
require_once("adminglobal.php");
check_rights();

require_once("header.php");
?>
<h1>View User</h1><hr>
<p>A small amount of information availible to staff who cannot edit/view all information (moderators, etc).</p><br />

<?php
if (isset($_GET['id'])) {
    $uid = $_GET['id'];
    if (!is_numeric($uid)) {
        $uid = $users->get_id($uid);
    }
    $qry = dbquery("SELECT * FROM characters WHERE id = '$uid' LIMIT 1;");

    if (mysql_num_rows($qry) > 0) {
        $u_vars = mysql_fetch_assoc($qry);
        ?>
<fieldset>
    <dl>
        <dt><label>Name</label></dt>
        <dd><strong><?php echo ucwords($u_vars['username']); ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Client rights</label></dt>
        <dd><strong><?php echo get_client_rights($u_vars['client_rights']); ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Server rights</label></dt>
        <dd><strong><?php echo get_rights($u_vars['server_rights']); ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Online</label></dt>
        <dd><strong><?php echo $u_vars['online'] == 1 ? "Yes" : "No"; ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Active</label></dt>
        <dd><strong><?php echo $u_vars['active'] == 1 ? "Yes" : "No"; ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Email</label></dt>
        <dd><strong><a href='mailto:<?php echo $u_vars['email']; ?>'><?php echo $u_vars['email'] ?></a></strong></dd>
    </dl>
    <dl>
        <dt><label>Register date</label></dt>
        <dd><strong><?php echo $u_vars['register_date']; ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Register IP</label></dt>
        <dd><strong><?php echo $u_vars['register_ip']; ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Last IP</label></dt>
        <dd><strong><?php echo $u_vars['last_ip']; ?></strong></dd>
    </dl>
    <dl>
        <dt><label>Last signin</label></dt>
        <dd><strong><?php echo $u_vars['last_signin']; ?></strong></dd>
    </dl>

            <?php if (ACP_RIGHTS > 2) { ?>
    <p class="quick">
        <input class="button1" type="submit" value=" Edit User "
               onclick="parent.location='edituser.php?id=<?php echo $uid; ?>'" />
    </p>
                <?php } ?>
</fieldset>
        <?php
    } else {
        acp_error("The specified user does not exist.");
    }
}
require_once("footer.php");
?>
