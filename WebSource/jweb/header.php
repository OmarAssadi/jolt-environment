<?php
/*
 *     RageScape Web
 *     Copyright (C) 2010 RageScape
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

if (!defined('JWEB')) {
    exit;
}

$title = SITE_NAME;
if (defined('PAGE_TITLE')) {
    $title .= ' - ' . PAGE_TITLE;
}

$tab = "nav";
if (defined("PAGE_TAB")) {
    $tab_id = PAGE_TAB;

    switch ($tab_id) {
        case 0: $tab = "nav"; break;
        case 1: $tab = "navhome"; break;
        case 2: $tab = "navplay"; break;
        case 3: $tab = "navaccount"; break;
        case 4: $tab = "navguide"; break;
        case 5: $tab = "navcommunity"; break;
        case 6: $tab = "navhelp"; break;
        case 7: $tab = "login"; break;
    }
}

$online =  dbevaluate("SELECT COUNT(id) FROM characters WHERE online = '1';");

?>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>

    <meta http-equiv="content-type" content="text/html;charset=ISO-8859-1">
    <head>
        <link rel="icon" type="image/vnd.microsoft.icon" href="favicon.ico">
        <link rel="SHORTCUT ICON" href="favicon.ico">

        <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
        <meta http-equiv="Content-Language" content="
              en,
              English
              ">
        <meta name="keywords" content="RageScape, RageZone, RuneScape, free, games, online, multiplayer, magic, spells, MMORPG, MPORPG, gaming">
        <meta name="description" content="RageScape is a massive 3d multiplayer adventure, with monsters to kill, quests to complete, and treasure to win. You control your own character who will improve and become more powerful the more you play.">
        <meta name="title" content="RageScape - Ragezone's RuneScape Server">
        <title><?php echo $title; ?></title>
        <style type="text/css">/*\*/@import url(css/global-30.css);/**/</style>
        <script type="text/javascript" src="./js/jquery/jquery_1_3_2.js"></script>

        <style type="text/css">/*\*/@import url(css/home-28.css);/**/</style>
        <script type="text/javascript">
            function h(o){o.getElementsByTagName('span')[0].className='shimHover';}
            function u(o){o.getElementsByTagName('span')[0].className='shim';}
            document.domain='runescape.com';
        </script>
        <link rel="alternate" type="application/rss+xml" title="RageScape - Latest news" href="http://services.runescape.com/m=news/latest_news.rss">
    </head>

    <body id="<?php echo $tab; ?>" class="bodyBackground">
        <a name="top"></a>
        
        <div id="scroll">
            <div id="head"><div id="headBg">
                    <div id="headOrangeTop"></div>
                    <img src="img/main/skins/default/head_image.jpg" alt="RuneScape" />
                    <div id="headImage"><a href="home" id="logo_select"></a>
                        <div id="player_no">There are currently <?php echo $online; ?> people playing!</div>
                    </div>
                    <div id="headOrangeBottom"></div>
                    <div id="headAdvert">
                        <script type="text/javascript"><!--
					google_ad_client = "pub-9355552874197940";
					/* 728x90, created 3/2/09 */
					google_ad_slot = "0844195204";
					google_ad_width = 728;
					google_ad_height = 90;
					//-->
			</script>
			<script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js"></script>
                    </div>
                    <div id="menubox">
                        <ul id="menus">
                            <li class="top"><a href="home" id="home" class="tl"><span class="ts">Home</span></a></li>

                            <li class="top"><a id="play" class="tl" href="play" onclick="if(!this.j){this.href+='?j=1';this.j=true;}"><span class="ts">Play Now</span><!--[if gt IE 6]><!--></a><!--<![endif]-->
                            <!--[if lte IE 6]><table><tr><td><![endif]-->
                                <ul>
                                    <li><a href="create" class="fly"><span>New Users</span></a></li>
                                    <li><a href="play" onclick="if(!this.j){this.href+='?j=1';this.j=true;}" class="fly"><span>Existing Users</span></a></li>
                                </ul>
                                <!--[if lte IE 6]></td></tr></table></a><![endif]-->
                            </li>

                            <li class="top"><a id="account" class="tl" href="account"><span class="ts">Account</span><!--[if gt IE 6]><!--></a><!--<![endif]-->
                            <!--[if lte IE 6]><table><tr><td><![endif]-->
                                <ul>
                                    <li><a href="create" class="fly"><span>Create New Account</span></a></li>
                                    <li><a href="account" class="fly"><span>Account Management</span></a></li>
                                </ul>
                                <!--[if lte IE 6]></td></tr></table></a><![endif]-->
                            </li>

                            <li class="top"><a id="guide" class="tl" href="http://www.runescape.com/kbase/guid/manual"><span class="ts">Game Guide</span><!--[if gt IE 6]><!--></a><!--<![endif]-->
                            <!--[if lte IE 6]><table><tr><td><![endif]-->
                            <!--[if lte IE 6]><iframe src=""></iframe><![endif]-->
                                <ul>
                                    <li><a href="http://www.runescape.com/kbase/guid/manual" class="fly"><span>Manual</span></a></li>
                                    <li><a href="http://services.runescape.com/m=questhelp/index.ws" class="fly"><span>QuestHelp</span></a></li>
                                    <li><a href="http://services.runescape.com/m=itemdb_rs/frontpage.ws" class="fly"><span>Grand Exchange</span></a></li>
                                    <li><a href="http://www.runescape.com/kbase/guid/rules_of_conduct" class="fly"><span>Rules</span></a></li>
                                    <li><a href="http://www.runescape.com/kbase/guid/lore" class="fly"><span>Lores</span></a></li>
                                    <li><a href="splash.html" class="fly"><span>What is RuneScape?</span></a></li>
                                </ul>
                                <!--[if lte IE 6]></td></tr></table></a><![endif]-->
                            </li>

                            <li class="top"><a id="community" class="tl" href="http://services.runescape.com/m=forum/forums.ws"><span class="ts">Community</span><!--[if gt IE 6]><!--></a><!--<![endif]-->
                            <!--[if lte IE 6]><table><tr><td><![endif]-->
                            <!--[if lte IE 6]><iframe src=""></iframe><![endif]-->
                                <ul>
                                    <li><a href="http://services.runescape.com/m=forum/forums.ws" class="fly"><span>Forums</span></a></li>
                                    <li><a href="http://services.runescape.com/m=hiscore/hiscores.ws" class="fly"><span>Hiscores</span></a></li>
                                    <li><a href="http://www.runescape.com/kbase/guid/Player_Submissions" class="fly"><span>Player Submissions</span></a></li>
                                    <li><a href="http://services.runescape.com/m=adventurers-log/display_player_profile.ws" class="fly"><span>Adventurer's Log</span></a></li>

                                    <li><a href="http://services.runescape.com/m=poll/index.ws" class="fly"><span>Polls</span></a></li>
                                    <li><a href="http://www.runescape.com/kbase/guid/Downloads_and_Wallpapers" class="fly"><span>Downloads &amp; Wallpapers</span></a></li>
                                </ul>
                                <!--[if lte IE 6]></td></tr></table></a><![endif]-->
                            </li>

                            <li class="top"><a id="help" class="tl" href="http://www.runescape.com/kbase/guid/Customer_Support"><span class="ts">Help</span><!--[if gt IE 6]><!--></a><!--<![endif]-->
                            <!--[if lte IE 6]><table><tr><td><![endif]-->
                            <!--[if lte IE 6]><iframe src=""></iframe><![endif]-->
                                <ul>
                                    <li><a href="http://www.runescape.com/kbase/guid/Customer_Support" class="fly"><span>Customer Support</span></a></li>
                                    <li><a href="loginapplet/loginappletb4b5.html?mod=www&amp;dest=loginapplet/loginapplet.ws?mod=accountappeal&amp;dest=passwordchoice.ws" class="fly"><span>Password Recovery</span></a></li>
                                    <li><a href="loginapplet/loginappletef7e.html?mod=www&amp;dest=loginapplet/loginapplet.ws?mod=accountappeal&amp;dest=lockchoice.ws" class="fly"><span>Locked Account Recovery</span></a></li>
                                    <li><a href="https://secure.runescape.com/m=offence-appeal/index.ws" class="fly"><span>Appeal Bans &amp; Mutes</span></a></li>
                                    <li><a href="http://services.runescape.com/m=bugtracker_v4/" class="fly"><span>Submit a Bug Report</span></a></li>
                                    <li><a href="parents.html" class="fly"><span>Parents' Guide</span></a></li>
                                </ul>
                                <!--[if lte IE 6]></td></tr></table></a><![endif]-->
                            </li>

                            <?php if (LOGGED_IN) { ?>
                            <li class="top"><a href="logout" id="logout" class="tl"><span class="ts">Log Out</span></a></li>
                            <?php } else { ?>
                            <li class="top"><a href="login" id="login" class="tl"><span class="ts">Log In</span></a></li>
                            <?php } ?>
                        </ul>
                        <br class="clear" />
                    </div>

                </div></div>
            <div id="content">
