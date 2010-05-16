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
define('ACP_TITLE', 'Website');
define('ACP_TAB', 3);
require_once("adminglobal.php");
check_rights();

require_once("header.php");
?>
<h1>News Articles</h1><hr>
<p>All published news articles are shown here. Allows you to see who made the article,
    when it was made, and an option to edit the article.</p><br />

<table cellspacing="1">
    <thead>
        <tr>
            <th>Title</th>
            <th>Date</th>
            <th>Author</th>
        </tr>
    </thead>
    <tbody>
        <?php
        $articles = dbquery("SELECT id,author_id,date,title FROM web_news ORDER BY id DESC");

        if (mysql_num_rows($articles) > 0) {
            while ($article = mysql_fetch_assoc($articles)) {
                echo "            <tr>
                <td><strong><a href='editarticle.php?id=" . $article['id'] . "'>" . $article['title'] ."</a></strong></td>
                <td>" . $article['date'] ."</td>
                <td>" . $users->get_name($article['author_id']) ."</td>
            </tr>";
            }
        } else {
            echo "<tr><td colspan='5' style='text-align: center;'>No worlds have been created.</td></tr>";
        }
        ?>
    </tbody>
</table>

<?php require_once("footer.php"); ?>
