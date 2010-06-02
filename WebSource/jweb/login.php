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
define('PAGE_TAB', 7);
define('PAGE_TITLE', 'Web Login');

require_once("global.php");

if (LOGGED_IN) {
    header("Location: home");
}

if (isset($_POST['submit'])) {
    $username = $users->format_dbname($_POST['username']);
    $password = sha1($username . sha1($_POST['password']));
    $remember = $_POST['rem'];

    if ($users->validate_details($username, $password)) {
        $_SESSION['JWEB_USER'] = $username;
        $_SESSION['JWEB_HASH'] = $password;

        if ($remember && !isset($_COOKIE['JWEB_REM_NAME'])) {
            setcookie("JWEB_REM_NAME", $username, time()+60*60*24*30);
        } else {
            setcookie("JWEB_REM_NAME", "", time() - 3600);
        }
        
        die('<script type="text/javascript">top.location.href = \'home\';</script>');
    } else {
        $failed = true;
    }
}

require_once("header.php");
?>
<script type="text/javascript"> 
    function SetFocus() {
        var Username = document.getElementById('username');
        var Password = document.getElementById('password');
        if ( Username.value == ''){
            Username.focus();
            return false;
        }
        if ( Password.value == ''){
            Password.focus();
            return false;
        }
        return true;
    }
</script>
<style type="text/css">/*\*/@import url(css/weblogin-6.css);/**/</style>


<div id="article">
    <div class="sectionHeader">
        <div class="left">
            <div class="right">
                <div class="plaque">
                    Secure Login
                </div>
            </div>
        </div>
    </div>
    <div class="section">
        <div class="brown_background">
            <div id="login_background" class="inner_brown_background" >
                <div id="login_panel" class="brown_box" >
                    <form id="login_form" action="login" method="post" autocomplete="off">
                        <div class="bottom">
                            <div class="repeat">
                                <div class="top_section">
                                    <div id="message">
                                        <?php if(isset($failed)) { ?>
                                        <font color="red">Invalid login details.</font>
                                        <?php } else { ?>
                                        Please enter your username and password to continue.
                                        <?php } ?>

                                        <!-- <font color="red">Invalid username or password.</font> -->
                                    </div> <div class="section_form" id="usernameSection">
                                        <label for="username">Username:</label>
                                        <input size="20" type="text" name="username" id="username" maxlength="12" <?php if (isset($_COOKIE['JWEB_REM_NAME'])) { echo "value='" . $_COOKIE['JWEB_REM_NAME'] . "'"; } ?>/>
                                    </div>
                                    <div class="section_form" id="passwordSection">
                                        <label for="password">Password:</label>
                                        <input size="20" type="password" id="password" name="password" maxlength="20"/>
                                    </div>
                                </div>
                                <div class="bottom_section">
                                    <div id="remember">
                                        <label for="rem">
                                            <input type="checkbox" name="rem" id="rem" value="1" class="checkbox" <?php if (isset($_COOKIE['JWEB_REM_NAME'])) { echo "checked='checked'"; } ?>/>
                                            Check this box to remember username</label>
                                    </div>
                                    <div id="submit_button">
                                        <button type="submit" name="submit" value="Login Now!" onmouseover="this.style.backgroundPosition='bottom';" onmouseout="this.style.backgroundPosition='top';" onclick="return SetFocus();">Login Now!</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="buttons">
                    <a href="create" class="createaccount"><span class="buttbg"></span>Create a New Account<br/>Click Here!</a>
                    <a href="recover" class="recoveraccount"><span class="butt1bg"></span>Lost Your Password?<br/> Click Here!</a>
                </div>
                <br class="clear" />
            </div>
        </div>
    </div>
</div>

<?php require_once("footer.php"); ?>