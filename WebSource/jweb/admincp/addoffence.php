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

if (isset($_POST['submit'])) {
    $name = $_POST['name'];
    $uid = $users->get_id($name);
    $mod_id = ACP_UID;
    $reason = $_POST['reason'];
    $type = $_POST['ban_type'];
    $length = $_POST['ban_length'];

    $date_time = date_create();
    $now = dbdate_format($date_time);
    $expire = dbdate_format($date_time->add(new DateInterval("P" . $length . "D")));
    
    dbquery("INSERT INTO offences (userid,type,date,expire_date,moderatorid,reason)
        VALUES ('$uid','$type','$now','$expire','$mod_id', '$reason');");

    add_log(ACP_NAME, USER_IP, "Added new ban.<br />» $name");
    die('<script type="text/javascript">top.location.href = \'offences.php?new=true\';</script>');
}

/*$now = date_create();
echo dbdate_format($now) . '<br />';
echo dbdate_format($now->add(new DateInterval("P10D")));*/

?>
<h1>Add Offence</h1><hr>
<p>This page allows staff to ban users who have broken a rule.</p><br />

<form method="post" action="addoffence.php">
    <fieldset>
        <dl>
            <dt>
                <label for="name">Username:</label><br />
                <span>The name of the user to ban.</span>
            </dt>
            <dd><input id="name" type="text" size="20" maxlength="12"
                       name="name" value="" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="reason">Reason:</label><br />
                <span>A brief description of why the user is being banned.</span>
            </dt>
            <dd>
                <textarea id="reason" name="reason" rows="5" cols="45"></textarea>
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="ban_type">Offense type:</label><br />
                <span>The type of ban to put in this user..</span>
            </dt>
            <dd>
                <select id="ban_type" name="ban_type">
                    <option value="0">Mute</option>
                    <option value="1">Ban</option>
                </select>
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="ban_length">Ban length:</label><br />
                <span>How long the user is banned for.</span>
            </dt>
            <dd>
                <select id="ban_length" name="ban_length">
                    <option value="1">1 Day</option>
                    <option value="3">3 Days</option>
                    <option value="5">5 Days</option>
                    <option value="7">1 Week</option>
                    <option value="14">2 Weeks</option>
                    <option value="21">3 Weeks</option>
                    <option value="30">1 Month</option>
                    <option value="60">2 Months</option>
                    <option value="90">3 Months</option>
                    <option value="180">6 Months</option>
                    <option value="365">1 Year</option>
                    <option value="830">2 Years</option>
                </select>
            </dd>
        </dl>
    </fieldset>

    <fieldset class="submit-buttons">
        <input class="button1" type="submit" id="submit" name="submit" value="  Submit Article  " />&nbsp;
    </fieldset>
</form>

<?php require_once("footer.php"); ?>
