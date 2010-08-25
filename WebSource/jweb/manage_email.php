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
require_once("global.php");

if (!LOGGED_IN) {
    header("Location: " . WWW . "/login");
}

$q = dbquery("SELECT email FROM characters WHERE id = '" . USER_ID . "' LIMIT 1");
if(mysql_num_rows($q) > 0) {
    $row = mysql_fetch_assoc($q);
    $cur_email = $row['email'];
    if ($cur_email == "" || $cur_email == null) {
        $cur_email = "null";
    }
}

$msg_string = "";
$done = false;

if (isset($_POST['submit'])) {
    $cur_pass = $_POST['curpassword'];
    $email = $_POST['email'];

    if ($_SESSION['JWEB_HASH'] == sha1(USER_NAME . sha1($cur_pass))) {
        if (check_email($email)) {
            dbquery("UPDATE characters SET email='$email' WHERE id='" . USER_ID . "';");
            $done = true;
        } else {
            $msg_string = "<font color='red'><b>The given email is not valid.</b></font>";
        }
    } else {
        $msg_string = "<font color='red'><b>The current password you entered did not match against our records.</b></font>";
    }
}

require_once("manage_header.php");
?>

<div id="content">
    <div id="contentLeft">
        <div class="title">
            <span>
                Email Settings
            </span>
        </div>

        <?php
        if ($done) {
            ?>
        <p>
            <font color='green'>Successfully updated email.</font> <b>Redirecting...</b>
        </p>
        <head>
            <meta http-equiv="refresh" content="3; URL=<?php echo WWW; ?>">
        </head>
            <?php
        } else {
            ?>
        <div class="infoList">
            <p>Your email address will be used to help you to recover your password and our newsletter will keep you informed. </p>
            <p>We will not share it with any third parties without your permission.</p>
                <?php if ($msg_string != "") {
        echo $msg_string;
    } ?>
        </div>

        <form method="post" action="<?php echo WWW; ?>/account/email">
            <fieldset class="activeForm">
                <legend>
                    Change Email
                </legend>
                <table>
                    <p><b>Your current email: <?php echo $cur_email; ?></b></p>
                    <tr>
                        <td style="text-align:right;padding-right:10px;padding-top:3px;"><label for="curpassword">Enter your <b>current</b> password:</label></td>
                        <td><div class="inputSmall"><input type="password" id="curpassword" name="curpassword" maxlength="20" size="20"></div></td>
                    </tr>
                    <tr>
                        <td style="text-align:right;padding-right:10px;padding-top:3px;"><label for="email">Enter your <b>new</b> email:</label></td>
                        <td><div class="inputSmall"><input id="email" name="email" maxlength="20" size="20"></div></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <input class="buttonLong" type="submit" name="submit" value="Submit Request">
                        </td>
                    </tr>
                </table>
            </fieldset>
            <br>
        </form>

    <?php } ?>
    </div>
</div>

<?php require_once("manage_footer.php"); ?>
