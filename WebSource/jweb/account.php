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
define('PAGE_TITLE', 'Account');

require_once("global.php");

if (!LOGGED_IN) {
    header("Location: " . WWW . "/login");
}

require_once("header.php");
?>

<style type="text/css">
    .listBoxSmall_left,.listBoxSmall_center,.listBoxSmall_right {
        float: left;
        width: 235px;
        height: 109px;
        border: 1px solid #6C5937;
        background: #261c09;
        position: relative;
    }

    .listBoxSmall_center {
        width: 236px;
    }
    .listBoxSmall_center .listBoxButton {
        width: 228px;
    }

    .listBox_right,.listBox_left {
        float: none;
        width: 356px;
        height: 100px;
        border: 1px solid #6C5937;
        background: #261c09;
        position: relative;
    }
    .listBoxSmall_right,.listBoxSmall_center,.listBox_right {
        margin: 4px 0px 0px 4px;
    }
    .section a {
        text-decoration: none;
    }
    .listBoxSmall_left,.listBox_left {
        margin: 4px 0px 0px 0px;
    }
    .listBoxTitle {
        padding: 5px 0px 0px 0px;
        font-weight: bold;
    }
    .listBoxIcon {
        float: left;
        margin: 0px 3px 20px;
    }
    .listBoxButton_Big, .listBoxButton {
        background-image: url('<?php echo WWW; ?>/img/main/account_management/button_back.gif');
        width: 348px;
        position: absolute;
        bottom: 4px;
        height: 25px;
        left: 4px;
    }
    .listBoxButton {
        width: 227px;
    }
    .listBoxButton a, .listBoxButton_Big a {
        width: 140px;
        height: 17px;
        padding-top: 5px;
        font-weight: bold;
        background: transparent url(<?php echo WWW; ?>/img/main/account_management/button.gif) no-repeat top left;
        margin:auto;
        text-align: center;
        vertical-align: middle;
        cursor: pointer;
        display: block;
        color: #382f1f;
        outline: none;
    }

    .listBoxButton a:hover, .listBoxButton_Big a:hover {
        background-position: left 90%;
        color: #898989;
    }

    .description {
        margin-top: 5px;
        padding-right: 5px;
    }

    .padded {
        padding: 5px;
    }
</style> 

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
                    <div class="subsectionHeader">Account Settings</div>
                    <div class="inner_brown_box">
                        We recommend that you change your password from time to time. If you have received a ban or caused an offence, you can appeal against it (see below). Also, by clicking 'Read Messages' below, you can read correspondence from <?php echo SITE_NAME; ?>.
                    </div>

                    <center>
                        <div class="listBox_left">
                            <div class="padded">
                                <div class="listBoxTitle"><a href="<?php echo WWW; ?>/account/password">Change Password</a></div>
                                <div class="description">Change your password every few months.</div>
                                <div class="listBoxButton_Big">
                                    <a href="<?php echo WWW; ?>/account/password">Change Password</a>
                                </div>
                            </div>
                        </div>
                    </center>

                    <br class="clear" />

                    <center>
                        <div class="listBox_left">
                            <div class="padded">
                                <div class="listBoxTitle"><a href="http://www.runescape.com/c=vac8Ay3GozQ/redirect.ws?mod=password_history&dest=password.ws">Change Password</a></div>
                                <div class="description">Change your password every few months.</div>
                                <div class="listBoxButton_Big">
                                    <a href="http://www.runescape.com/c=vac8Ay3GozQ/redirect.ws?mod=password_history&dest=password.ws">Change Password</a>
                                </div>
                            </div>
                        </div>
                    </center>
                    
                </div>
            </div>
        </div>
    </div>
</div>
<?php require_once("footer.php"); ?>
