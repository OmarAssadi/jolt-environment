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
define('ACP_TITLE', 'Edit User');
define('ACP_TAB', 4);
require_once("adminglobal.php");
check_rights(4);
require_once("header.php");

if (isset($_GET['id'])) {
    $uid = $_GET['id'];
    if (!is_numeric($uid)) {
        $uid = $users->get_id($uid);
    }

    if (isset($_POST['submit_general'])) {
        $user_id2 = $_POST['user_id'];
        $email = $_POST['email'];
        $dob = $_POST['dob'];
        $country_code = $_POST['country_code'];
        $c_rights = $_POST['client_rights'];
        $s_rights = $_POST['server_rights'];

        dbquery("UPDATE characters SET email='$email',dob='$dob',country='$country_code',client_rights='$c_rights',server_rights='$s_rights' WHERE id='$user_id2'");
        acp_success("Successfully saved user details.");
        add_log(ACP_NAME, USER_IP, "Edited user general details. <br />» " . $users->get_name($uid));
    }
    
    if (isset($_POST['submit_appearance'])) {
        
    }


} else {
    die('<script type="text/javascript">top.location.href = \'dashboard.php\';</script>');
}

$user_qry = dbquery("SELECT * FROM characters WHERE id = $uid LIMIT 1");
if (mysql_num_rows($user_qry) > 0) {
    $user_vars = mysql_fetch_assoc($user_qry);
    ?>

<h2>Editing User...</h2><br />

<form method="post" action="edituser.php?id=<?php echo $uid; ?>">
    <input type="hidden" name="user_id" value="<?php echo $uid; ?>" />
    <fieldset>
        <legend>General</legend>
        <dl>
            <dt><label for="w_id">ID:</label><br /></dt>
            <dd><strong><?php echo $user_vars['id'] ?></strong></dd>
        </dl>

        <dl>
            <dt><label for="world_name">Name:</label><br /></dt>
            <dd><strong><?php echo $user_vars['username'] ?></strong></dd>
        </dl>

        <dl>
            <dt><label>Online:</label></dt>
            <dd><strong><?php echo $user_vars['online'] == 1 ? "Yes" : "No"; ?></strong></dd>
        </dl>

        <dl>
            <dt><label>Active:</label></dt>
            <dd><strong><?php echo $user_vars['active'] == 1 ? "Yes" : "No"; ?></strong></dd>
        </dl>

        <dl>
            <dt><label>Email:</label></dt>
            <dd><input id="email" type="text" size="20" maxlength="255"
                       name="email" value="<?php echo $user_vars['email'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt><label>Register date:</label></dt>
            <dd><strong><?php echo $user_vars['register_date']; ?></strong></dd>
        </dl>

        <dl>
            <dt><label>Register IP:</label></dt>
            <dd><strong><?php echo $user_vars['register_ip']; ?></strong></dd>
        </dl>

        <dl>
            <dt><label>Last IP:</label></dt>
            <dd><strong><?php echo $user_vars['last_ip']; ?></strong></dd>
        </dl>

        <dl>
            <dt><label>Last signin:</label></dt>
            <dd><strong><?php echo $user_vars['last_signin']; ?></strong></dd>
        </dl>

        <dl>
            <dt>
                <label for="dob">DOB:</label><br />
                <span>The user's date of birth.</span>
            </dt>
            <dd><input id="dob" type="text" size="20" maxlength="255"
                       name="dob" value="<?php echo $user_vars['dob'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="country_code">Country code:</label><br />
                <span>The country code of the user's current residence.</span>
            </dt>
            <dd><input id="country_code" type="text" size="20" maxlength="255"
                       name="country_code" value="<?php echo $user_vars['country'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="client_rights">Client rights:</label><br />
            </dt>
            <dd>
                <select id="client_rights" name="client_rights">
                    <option value="0" <?php if ($user_vars['client_rights'] == 0) { echo "selected='selected'"; } ?>>Player</option>
                    <option value="1" <?php if ($user_vars['client_rights'] == 1) { echo "selected='selected'"; } ?>>Moderator</option>
                    <option value="2" <?php if ($user_vars['client_rights'] == 2) { echo "selected='selected'"; } ?>>Administrator</option>
                </select>
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="server_rights">Server rights:</label><br />
            </dt>
            <dd>
                <select id="server_rights" name="server_rights">
                    <option value="0" <?php if ($user_vars['server_rights'] == 0) { echo "selected='selected'"; } ?>>Player</option>
                    <option value="1" <?php if ($user_vars['server_rights'] == 1) { echo "selected='selected'"; } ?>>Donator</option>
                    <option value="2" <?php if ($user_vars['server_rights'] == 2) { echo "selected='selected'"; } ?>>Moderator</option>
                    <option value="3" <?php if ($user_vars['server_rights'] == 3) { echo "selected='selected'"; } ?>>Adminstrator</option>
                    <option value="4" <?php if ($user_vars['server_rights'] == 4) { echo "selected='selected'"; } ?>>System Administrator</option>
                </select>
            </dd>
        </dl>

        <p class="quick">
            <input class="button1" type="submit" id="submit_general" name="submit_general" value=" Save " />
        </p>
    </fieldset>
</form>
<br />
<form method="post" action="edituser.php?id=<?php echo $uid; ?>">
    <input type="hidden" name="user_id" value="<?php echo $uid; ?>" />
    <fieldset>
        <legend>In-Game</legend>

        <dl>
            <dt>
                <label for="coord_x">Coord X:</label><br />
                <span>The X coordiante of where the character is located.</span>
            </dt>
            <dd><input id="coord_x" type="text" size="5" maxlength="5"
                       name="coord_x" value="<?php echo $user_vars['coord_x'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="coord_y">Coord Y:</label><br />
                <span>The Y coordiante of where the character is located.</span>
            </dt>
            <dd><input id="coord_y" type="text" size="5" maxlength="5"
                       name="coord_y" value="<?php echo $user_vars['coord_y'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="coord_z">Coord Z:</label><br />
                <span>The Z coordiante of where the character is located.</span>
            </dt>
            <dd><input id="coord_z" type="text" size="5" maxlength="5"
                       name="coord_z" value="<?php echo $user_vars['coord_z'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="energy">Energy:</label><br />
                <span>The amount of running energy the character has.</span>
            </dt>
            <dd><input id="energy" type="text" size="5" maxlength="5"
                       name="energy" value="<?php echo $user_vars['run_energy'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="inventory">Inventory:</label><br />
                <span>Items located in the character's inventory.</span>
            </dt>
            <dd><input id="inventory" type="text" size="20"
                       name="inventory" value="<?php echo $user_vars['inventory_items'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="equipment">Equipment:</label><br />
                <span>Items located in the character's equipment.</span>
            </dt>
            <dd><input id="equipment" type="text" size="20"
                       name="equipment" value="<?php echo $user_vars['equipment_items'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="bank">Bank:</label><br />
                <span>Items located in the character's bank.</span>
            </dt>
            <dd><input id="bank" type="text" size="20"
                       name="bank" value="<?php echo $user_vars['bank_items'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="friends">Friends:</label><br />
                <span>The character's friends.</span>
            </dt>
            <dd><input id="friends" type="text" size="20"
                       name="friends" value="<?php echo $user_vars['friends'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="ignores">Ignores:</label><br />
                <span>The character's ignores.</span>
            </dt>
            <dd><input id="ignores" type="text" size="20"
                       name="ignores" value="<?php echo $user_vars['friends'] ?>" />
            </dd>
        </dl>



        <p class="quick">
            <input class="button1" type="submit" id="submit_general" name="submit_general" value=" Save " />
        </p>
    </fieldset>
</form>

    <?php
} else {
    acp_error("<b>Error:</b> User (id:$uid) does not exist. ");
}
require_once("footer.php");
?>
