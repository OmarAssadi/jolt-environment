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
define('PAGE_TAB', 3);
define('PAGE_TITLE', 'Change Password');

require_once("global.php");

if (!LOGGED_IN) {
    header("Location: " . WWW . "/login");
}

require_once("header.php");

$msg_string = "";

if (isset($_POST['submit'])) {
    $cur_pass = $_POST['cur_pass'];
    $new_pass = $_POST['new_pass'];
    $check_pass = $_POST['check_pass'];
    
    if ($_SESSION['JWEB_HASH'] == sha1(USER_NAME . sha1($cur_pass))) {
        if ($new_pass == $check_pass) {
            if (strlen($new_pass) < 5 || strlen($new_pass) > 20) {
                $msg_string = "<font color='red'>Passwords must be between 5 and 20 characters long and may contain letters and numbers.</font>";
            } else {
                $new_pass = sha1(USER_NAME . sha1($new_pass));
                dbquery("UPDATE characters SET password='$new_pass' WHERE id='" . USER_ID . "';");
                $msg_string = "<font color='green'>Successfully updated password.</font>";
            }
        } else {
            $msg_string = "<font color='red'>The new passwords you entered did not match.</font>";
        }
    } else {
        $msg_string = "<font color='red'>The current password you entered did not match against our records.</font>";
    }
}
?>

<style type="text/css">/*\*/@import url(<?php echo WWW; ?>/css/kbase-6.css);/**/</style>

<div id="article">
    <div class="sectionHeader">
        <div class="left">
            <div class="right">
                <div class="plaque">
                    Account Management
                </div>
            </div>
        </div>
    </div>
    <div class="section">
        <div class="brown_background">
            <div class="inner_brown_background">
                <div style="margin-bottom:5px" class="brown_box">
                <div class="subsectionHeader">Change Password</div>
                    <div class="inner_brown_box">
                        We recommend that you change your password from time to time. Keep your password safe, and do not give it out to others.
                    </div>

                    <div class="container" style="clear:both;"></div>
                    <div class="brown_box">
                        <div class="inner_brown_box">
                            <?php echo $msg_string; ?><br />
                            <div class="searchtextcategory">
                                <font color="red"><b></b></font>
                                <form id="pass_form" method="post" action="<?php echo WWW; ?>/account/password">
                                    <tr>
                                        <td class="searchtitles" width="60" style="text-align: right">Current password:</td>
                                        <td><input type=password class="textinput" name="cur_pass" id="cur_pass" size="29" maxlength="20" value=""></td>
                                    </tr>

                                    <br />
                                    <tr>
                                        <td class="searchtitles" width="60" style="text-align: right">New password:</td>
                                        <td><input type=password class="textinput" name="new_pass" id="new_pass" size="29" maxlength="20" value=""></td>
                                    </tr>

                                    <br />
                                    <tr>
                                        <td class="searchtitles" width="60" style="text-align: right">Confirm password:</td>
                                        <td><input type=password class="textinput" name="check_pass" id="check_pass" size="29" maxlength="20" value=""></td>
                                    </tr>

                                    <hr>
                                    <center>
                                        <span class="buttonbackground">
                                            <input class="buttonmedium" type="submit" name="submit" value="Submit" name="preview">
                                        </span>
                                    </center>
                                </form>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</div>

<?php require_once("footer.php"); ?>
