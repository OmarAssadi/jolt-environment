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
define('PAGE_TAB', 0);
define('PAGE_TITLE', 'Homepage');

require_once("global.php");
require_once("header.php");

if (isset($_GET['id']) && is_numeric($_GET['id'])) {
    $news_id = $_GET['id'];
    $news_qry = dbquery("SELECT * FROM web_news WHERE id='$news_id' LIMIT 1;");

    if (mysql_num_rows($news_qry) > 0) {
        $news_item = mysql_fetch_assoc($news_qry);
    } else {
        die('<script type="text/javascript">top.location.href = \'home.php\';</script>');
    }
} else {
    die('<script type="text/javascript">top.location.href = \'home.php\';</script>');
}
?>
<style type="text/css">/*\*/@import url(css/news-2.css);/**/</style> 
<style type="text/css">#feed {float: none;}</style> 


<div id="article">
    <div class="sectionHeader">
        <div class="left">
            <div class="right">
                <div class="plaque">
                    News
                </div>
            </div>
        </div>
    </div>
    <div class="section">
        <div class="article">
            <div class="topshadow">
                <div class="bottombordershad">
                    <div class="leftshadow">
                        <div class="rightshadow">
                            <div class="leftcorner">
                                <div class="rightcorner">
                                    <div class="bottomleftshad">
                                        <div class="bottomrightshad">
                                            <div class="pagepad">
                                                <div id="newsTitle">
                                                    <a href="viewnews.php?id=<?php echo $news_id - 1; ?>">
                                                        <img class=imiddle border="0" width="30" height="15" hspace="0" vspace="0" alt="Previous" title="Previous" src="./img/news/arrow_back.gif">
                                                    </a>

                                                    &nbsp;
                                                    <b><?php echo $news_item['date'] . ' - ' . $news_item['title']; ?></b>
                                                    &nbsp;

                                                    <a href="viewnews.php?id=<?php echo $news_id + 1; ?>">
                                                        <img class=imiddle border="0" width="30" height="15" hspace="0" vspace="0" alt="Next" title="Next" src="./img/news/arrow_forward.gif">
                                                    </a>
                                                </div>

                                                <div class="newsJustify">
                                                    <?php echo $news_item['story']; ?>
                                                    <hr>
                                                    <p><b><i>By <?php echo $users->format_name($users->get_name($news_item['author_id'])); ?></i></b></p>
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div> 

<?php require_once("footer.php"); ?>
