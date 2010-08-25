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

$msg_string = "";
$done = false;

if (isset($_POST['submit'])) {
    $cur_pass = $_POST['oldpassword'];
    $new_pass = $_POST['password1'];
    $check_pass = $_POST['password2'];

    if ($_SESSION['JWEB_HASH'] == sha1(USER_NAME . sha1($cur_pass))) {
        if ($new_pass == $check_pass) {
            if (strlen($new_pass) < 5 || strlen($new_pass) > 20) {
                $msg_string = "<font color='red'>Passwords must be between 5 and 20 characters long and may contain letters and numbers.</font>";
            } else {
                $new_pass = sha1(USER_NAME . sha1($new_pass));
                dbquery("UPDATE characters SET password='$new_pass' WHERE id='" . USER_ID . "';");
                $done = true;
            }
        } else {
            $msg_string = "<font color='red'>The new passwords you entered did not match.</font>";
        }
    } else {
        $msg_string = "<font color='red'>The current password you entered did not match against our records.</font>";
    }
}

require_once("manage_header.php");
?>
<div id="content">
    <div id="contentLeft">
        <div class="title">
            <span>
                Change Your Password
            </span>
        </div>

        <?php
        if ($done) {
            ?>
        <p>
            <font color='green'>Successfully updated password.</font> <b>Redirecting...</b>
        </p>
        <head>
            <meta http-equiv="refresh" content="3; URL=<?php echo WWW; ?>/logout.php">
        </head>
            <?php
        } else {
            ?>
        <div class="infoList">
            <p>Use this form if you want to change the password you use to log into your account. If you do not wish to set a new password at this time, click
                the link at the bottom-left of this page.</p>
            <p>Please note that passwords must be between 5 and 20 characters long. We recommend you use a mixture of numbers and letters in your password to make it harder for someone to guess.</p>

            <p><?php
                    if ($msg_string != "") {
                        echo $msg_string;
                    }
                    ?></p>
        </div>

        <form method="post" action="<?php echo WWW; ?>/account/password">
            <fieldset class="activeForm">
                <legend>
                    Change password
                </legend>
                <table>
                    <tr>
                        <td style="text-align:right;padding-right:10px;padding-top:3px;"><label for="oldpassword">Enter your <b>current</b> password:</label></td>
                        <td><div class="inputSmall"><input type="password" id="oldpassword" name="oldpassword" maxlength="20" size="20"></div></td>
                    </tr>
                    <tr>
                        <td style="text-align:right;padding-right:10px;padding-top:3px;"><label for="password1">Enter your <b>new</b> password:</label></td>
                        <td><div class="inputSmall"><input type="password" id="password1" name="password1" maxlength="20" size="20"></div></td>
                    </tr>
                    <tr>
                        <td style="text-align:right;padding-right:10px;padding-top:3px;"><label for="password2">Enter it again:</label></td>
                        <td><div class="inputSmall"><input type="password" id="password2" name="password2" maxlength="20" size="20"></div></td>
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
