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
define('ACP_TITLE', 'Edit World');
define('ACP_TAB', 2);
require_once("adminglobal.php");
check_rights(4);
require_once("header.php");

if (isset($_GET['id']) && is_numeric($_GET['id'])) {
    $world_id = $_GET['id'];

    if (isset($_POST['submit'])) {
        $w_id = $_POST['w_id'];
        $name = $_POST['name'];
        $welcome_message = $_POST['welcome_message'];
        $spawn_point = $_POST['spawn_point'];
        $motw = $_POST['motw'];
        $exp_rate = $_POST['exp_rate'];

        $result = "UPDATE worlds SET world_name='$name',welcome_message='$welcome_message',spawn_point='$spawn_point',motw='$motw',exp_rate='$exp_rate' WHERE world_id=$w_id";
        dbquery($result);

        acp_success("Successfully saved world information.");
        add_log(ACP_NAME, USER_IP, "Edited world details. <br />» ID: $w_id");
    }
} else {
    die('<script type="text/javascript">top.location.href = \'dashboard.php\';</script>');
}

$world_qry = dbquery("SELECT * FROM worlds WHERE world_id = $world_id LIMIT 1;");

if (mysql_num_rows($world_qry) > 0) {
    $world_vars = mysql_fetch_assoc($world_qry);
    ?>
<h2>Editing World...</h2>
<form method="post" action="worldedit.php?id=<?php echo $world_id; ?>">
    <input type="hidden" name="w_id" value="<?php echo $world_id; ?>" />
    <fieldset>
        <dl>
            <dt>
                <label for="w_id">ID: </label><br />
                <span>A unique ID assigned to each world.</span>
            </dt>
            <dd><strong><?php echo $world_vars['world_id'] ?></strong></dd>
        </dl>

        <dl>
            <dt>
                <label for="world_name">Name: </label><br />
                <span>The name of the world.</span>
            </dt>
            <dd><input id="world_name" type="text" size="20" maxlength="255"
                       name="name" value="<?php echo $world_vars['world_name'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="welcome_message">Welcome Message: </label><br />
                <span>The welcome message shown to all users at login.</span>
            </dt>
            <dd><input id="welcome_message" type="text" size="20" maxlength="255"
                       name="welcome_message" value="<?php echo $world_vars['welcome_message'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="spawn_point">Spawn Point: </label><br />
                <span>The point at which all newly created characters spawn.</span>
            </dt>
            <dd><input id="spawn_point" type="text" size="20" maxlength="255"
                       name="spawn_point" value="<?php echo $world_vars['spawn_point'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="motw">MOTW: </label><br />
                <span>The message of the week shown on the welcome screen.</span>
            </dt>
            <dd><input id="motw" type="text" size="20" maxlength="255"
                       name="motw" value="<?php echo $world_vars['motw'] ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="exp_rate">Experience Rate: </label><br />
                <span>The experience rate multiplier.</span>
            </dt>
            <dd><input id="exp_rate" type="text" size="3" maxlength="3"
                       name="exp_rate" value="<?php echo $world_vars['exp_rate'] ?>" />
            </dd>
        </dl>
    </fieldset>

    <fieldset class="submit-buttons">
        <input class="button1" type="submit" id="submit" name="submit" value="  Save  " />
    </fieldset>
</form>

    <?php
} else {
    acp_error("<b>Error:</b> World (id:$world_id) does not exist. ");
}
require_once("footer.php");
?>
