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
define('PAGE_TITLE', 'News');

require_once("global.php");
require_once("header.php");


?>

<style type="text/css">/*\*/@import url(css/news-2.css);/**/</style>
<style type="text/css">#feed {float: none;}</style> 

<div id="article">
    <div class="sectionHeader">
        <div class="left">
            <div class="right">
                <div class="plaque">
                    Latest News
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
                                            <div class="sectioncontainer">
                                                <br />

                                                <div class="newsBorder">
                                                    <table border="0" class="width100" cellspacing=0 id="newsList">
                                                        <tr class="row_a">
                                                            <td align="left" width="50%" id="titleTitle" class="item">
                                                                <b>News Item</b>
                                                            </td>
                                                            <td align="right" width="20%" class="date">
                                                                <b>Date</b>
                                                            </td>
                                                        </tr>

                                                        <?php
                                                        $articles = dbquery("SELECT id,date,title FROM web_news ORDER BY id DESC");

                                                        if (mysql_num_rows($articles) > 0) {
                                                            while ($article = mysql_fetch_assoc($articles)) {
                                                                echo '<tr class="pad">
                                                            <td align="left" valign="top" >
                                                                <a href="viewnews.php?id=' . $article['id'] . '">
                                                                    <span>' . $article['title'] . '</span>
                                                                </a>
                                                            </td>
                                                            <td align="right" class="date" valign="middle">
                                                                ' . $article['date'] . '
                                                            </td>
                                                        </tr>';
                                                            }
                                                        }
                                                        ?>
                                                    </table>
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
</div>

<?php require_once("footer.php"); ?>
