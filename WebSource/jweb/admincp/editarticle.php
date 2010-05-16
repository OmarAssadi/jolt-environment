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

if (isset($_GET['id']) && is_numeric($_GET['id'])) {
    $article_id = $_GET['id'];

    if (isset($_POST['submit'])) {
        $a_id = $_POST['a_id'];
        $title = $_POST['title'];
        $description = $_POST['description'];
        $story = $_POST['story'];

        dbquery("UPDATE web_news SET title='$title',description='$description',story='$story' WHERE id='$a_id';");
        acp_success("Successfully edited article.");
    }

    if (isset($_POST['delete'])) {
        $a_id = $_POST['a_id'];

        dbquery("DELETE FROM web_news WHERE id='$a_id';");
    }

    $article_qry = dbquery("SELECT * FROM web_news WHERE id='$article_id' LIMIT 1;");

    if (mysql_num_rows($article_qry) > 0) {
        $article_vars = mysql_fetch_assoc($article_qry);
        ?>

<h2>Editing news article...</h2>
<form method="post" action="editarticle.php?id=<?php echo $article_id; ?>">
    <input type="hidden" name="a_id" value="<?php echo $article_id; ?>" />
    <fieldset>
        <dl>
            <dt>
                <label>Author:</label><br />
                <span>The name of the author who created the article.</span>
            </dt>
            <dd><strong><?php echo $users->get_name($article_vars['author_id']); ?></strong></dd>
        </dl>


        <dl>
            <dt>
                <label>Date:</label><br />
                <span>The date the article was created.</span>
            </dt>
            <dd><strong><?php echo $article_vars['date']; ?></strong></dd>
        </dl>

        <dl>
            <dt>
                <label for="title">Title:</label>
                <span>A title shown on the news heading on front page.</span>
            </dt>
            <dd><input id="title" type="text" size="20" maxlength="255"
                       name="title" value="<?php echo $article_vars['title']; ?>" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="description">Description:</label><br />
                <span>A brief summerization of what the article is about.</span>
            </dt>
            <dd>
                <textarea id="description" name="description" rows="5" cols="45"><?php echo $article_vars['description']; ?></textarea>
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="story">Story:</label><br />
                <span>The full story of the article.</span>
            </dt>
            <dd>
                <textarea id="story" name="story" rows="10" cols="45"><?php echo $article_vars['story']; ?></textarea>
            </dd>
        </dl>
    </fieldset>

    <fieldset class="submit-buttons">
        <input class="button1" type="submit" id="submit" name="submit" value="  Save  " />&nbsp;
        <input class="button1" type="submit" id="submit" name="delete" value="  Delete  " />&nbsp;
    </fieldset>
</form>
        <?php
    } else {
        die('<script type="text/javascript">top.location.href = \'viewarticles.php\';</script>');
    }
} else {
    die('<script type="text/javascript">top.location.href = \'viewarticles.php\';</script>');
}
?>



<?php require_once("footer.php"); ?>
