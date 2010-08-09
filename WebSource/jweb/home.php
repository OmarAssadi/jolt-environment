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
define('PAGE_TAB', 1);
define('PAGE_TITLE', 'Homepage');

require_once("global.php");
require_once("header.php");

?>
<div id="left">
    <a href="<?php echo WWW; ?>/create" class="createbutton" onmouseover="h(this)" onmouseout="u(this)">
        <img src="<?php echo WWW; ?>/img/main/skins/default/created3d9.jpg?10" alt="Create a Free Account - Start your adventure today!" />
        <span class="shim"></span>
    </a>
    <div id="features">
        <div class="narrowHeader"><img src="<?php echo WWW; ?>/img/main/titles/websitefeatures.png" alt="Website Features" /></div>
        <div class="section">
            <div class="feature">
                <img src="<?php echo WWW; ?>/img/main/home/feature_upgrade_icon.jpg" alt="Upgrade Your Account" />
                <div class="featureTitle">stub</div>
                <div class="featureDesc">stub</div>
            </div>

            <div class="feature">
                <img src="<?php echo WWW; ?>/img/main/home/feature_upgrade_icon.jpg" alt="Upgrade Your Account" />
                <div class="featureTitle">stub</div>
                <div class="featureDesc">stub</div>
            </div>

            <div class="feature">
                <img src="<?php echo WWW; ?>/img/main/home/feature_upgrade_icon.jpg" alt="Upgrade Your Account" />
                <div class="featureTitle">stub</div>
                <div class="featureDesc">stub</div>
            </div>

            <div class="feature">
                <img src="<?php echo WWW; ?>/img/main/home/feature_upgrade_icon.jpg" alt="Upgrade Your Account" />
                <div class="featureTitle">stub</div>
                <div class="featureDesc">stub</div>
            </div>

            <div class="feature">
                <img src="<?php echo WWW; ?>/img/main/home/feature_upgrade_icon.jpg" alt="Upgrade Your Account" />
                <div class="featureTitle">stub</div>
                <div class="featureDesc">stub</div>
            </div>

            <div class="feature">
                <img src="<?php echo WWW; ?>/img/main/home/feature_upgrade_icon.jpg" alt="Upgrade Your Account" />
                <div class="featureTitle">stub</div>
                <div class="featureDesc">stub</div>
            </div>

            <div class="feature">
                <img src="<?php echo WWW; ?>/img/main/home/feature_upgrade_icon.jpg" alt="Upgrade Your Account" />
                <div class="featureTitle">stub</div>
                <div class="featureDesc">stub</div>
            </div>

            <div class="feature">
                <img src="<?php echo WWW; ?>/img/main/home/feature_upgrade_icon.jpg" alt="Upgrade Your Account" />
                <div class="featureTitle">stub</div>
                <div class="featureDesc">stub</div>
            </div>
        </div>
    </div>
</div>
<div id="right">
    <a href="<?php echo WWW; ?>/play" id="playbutton" onmouseover="h(this)" onmouseout="u(this)" onclick="if(!this.j){this.href+='?j=1';this.j=true;}">
        <img src="<?php echo WWW; ?>/img/main/skins/default/playd3d9.jpg?10" alt="Play RuneScape - Continue Your Adventure" />
        <span class="shim"></span>
    </a>
    <div id="recentnews">
        <div class="sectionHeader">
            <div class="left">
                <div class="right">
                    <div class="plaque">
                        <img src="<?php echo WWW; ?>/img/main/titles/recentnews.png" alt="Recent News" />
                    </div>
                </div>
            </div>
        </div>
        <div class="section">

            <?php
            $news_qry = dbquery("SELECT * FROM web_news ORDER BY id DESC LIMIT 4;");

            if (mysql_num_rows($news_qry) > 0) {
                while ($news = mysql_fetch_assoc($news_qry)) {
                    echo '
                        <div class="sectionBody">
                            <div class="recentNews">
                                <div class="newsTitle">
                                    <h3>' . $news['title'] . ' </h3>
                                    <span>' . $news['date'] . '</span>
                                </div>
                                <p>' . $news['description'] . ' <a href="' . WWW . '/viewnews/id/' . $news['id'] . '">Read more...</a></p>
                            </div>
                        </div>';
                }
            }
            ?>

            <div id="feed">
                <a href="http://services.runescape.com/m=news/latest_news.rss">
                    <!--[if gte IE 7]><!---><img title="Subscribe to RSS" alt="" src="<?php echo WWW; ?>/img/rss/logo.png"/><!--<![endif]-->
                    <!--[if IE 6]><img title="Subscribe to RSS" alt="" src="http://www.jagex.com/img/crossservice/RSS/logo_ie6.png"/><![endif]--> <span>Subscribe to RSS</span></a>
            </div>
            <div class="moreArchive"><a href="listnews">Browse our news archive</a></div>
        </div>
    </div>
</div>

<?php
include_once("./footer.php");
?>
