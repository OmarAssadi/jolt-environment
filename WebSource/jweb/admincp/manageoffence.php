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

if (isset($_GET['id']) && is_numeric($_GET['id'])) {
    $offence_id = $_GET['id'];

    if (isset($_GET['appeal_denied'])) {
        $denied = $_GET['appeal_denied'];

        if ($denied == "true") {
            dbquery("UPDATE offences SET appeal_status ='1' WHERE id='$offence_id';");
        } else if ($denied == "false") {
            dbquery("DELETE FROM offences WHERE id='$offence_id';");
            acp_success("The ban's appeal has been approved and is removed.");
            die;
        }
    }
} else {
    die('<script type="text/javascript">top.location.href = \'offences.php\';</script>');
}

$offence_qry = dbquery("SELECT * FROM offences WHERE id='$offence_id' LIMIT 1;");
if (mysql_num_rows($offence_qry) > 0) {
    $offence_vars = mysql_fetch_assoc($offence_qry);
    ?>

<form method="post" action="manageoffence.php?id=<?php echo $offence_id; ?>">
    <input type="hidden" name="w_id" value="<?php echo $offence_id; ?>" />
    <fieldset>
        <dl>
            <dt><label>Name:</label><br /></dt>
            <dd>
                <strong>
                    <a href='viewuser.php?id=<?php echo $offence_vars['userid']; ?>'>
                            <?php echo $users->get_name($offence_vars['userid']); ?>
                    </a>
                </strong>
            </dd>
        </dl>

        <dl>
            <dt>
                <label>Type:</label><br />
                <span>Type of ban against the user.</span>
            </dt>
            <dd><strong><?php echo get_type($offence_vars['type']); ?></strong></dd>
        </dl>

        <dl>
            <dt>
                <label>Date:</label><br />
                <span>The date the user was banned.</span>
            </dt>
            <dd><strong><?php echo $offence_vars['date']; ?></strong></dd>
        </dl>

        <dl>
            <dt>
                <label>Expire Date:</label><br />
                <span>The date the user will be unbanned.</span>
            </dt>
            <dd><strong><?php echo $offence_vars['expire_date']; ?></strong></dd>
        </dl>

        <dl>
            <dt>
                <label>Moderator:</label><br />
                <span>The moderator who banned the user.</span>
            </dt>
            <dd>
                <strong>
                    <a href='viewuser.php?id=<?php echo $offence_vars['userid']; ?>'>
                            <?php echo $users->get_name($offence_vars['moderatorid']); ?>
                    </a>
                </strong>
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="reason">Reason:</label><br />
                <span>The reason why the user is banned.</span>
            </dt>
            <dd>
                <textarea id="reason" name="reason" rows="3" cols="45"><?php echo $offence_vars['reason']; ?></textarea>
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="reason">Appeal Data:</label><br />
                <span>A message by the user in attempt to appeal the ban.</span>
            </dt>
            <dd>
                <textarea id="reason" name="reason" rows="3" cols="45"><?php echo $offence_vars['appeal_data']; ?></textarea>
            </dd>
        </dl>

        <dl>
            <dt>
                <label>Appeal Status:</label><br />
                <span>The status of the appealed data.</span>
            </dt>
            <dd><strong><?php
                        $appealed = ($log['appeal_data'] != "" ? "Yes" : "No");
                        if ($offence_vars['appeal_status'] == 1) {
                            $appealed = "Yes, denied";
                        }
                        echo $appealed;
                        ?></strong></dd>
        </dl>

        <dl>
            <dt>
                <label>Expired:</label><br />
                <span>Whether the ban has expired.</span>
            </dt>
            <dd><strong><?php echo ($offence_vars['expired'] == 1 ? "Yes" : "No"); ?></strong></dd>
        </dl>

        <p class="quick">
            <input class="button1" value=" Deny Appeal "
                   onclick="parent.location='manageoffence.php?id=<?php echo $offence_id; ?>&appeal_denied=true'" />

            <input class="button1" value=" Approve Appeal "
                   onclick="parent.location='manageoffence.php?id=<?php echo $offence_id; ?>&appeal_denied=false'" />
        </p>
    </fieldset>

    <fieldset class="submit-buttons">
        <input class="button1" type="submit" id="submit" name="submit" value="  Save  " />
    </fieldset>
</form>

    <?php
} else {
    acp_error("Offence not found.");
}
require_once("footer.php");
?>
