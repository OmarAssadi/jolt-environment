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

if (isset($_POST['submit'])) {
    $uid = ACP_UID;
    $title = $_POST['title'];
    $description = filter_for_input($_POST['description']);
    $story = filter_for_input($_POST['story']);

    dbquery("INSERT INTO web_news (author_id,date,title,description,story) VALUES ($uid, NOW(), '$title', '$description', '$story');");
    acp_success("Successfully created a new article.");
    add_log(ACP_NAME, USER_IP, "Added a new article.<br />» $title");
}
?>
<h1>Add Article</h1><hr>
<p>This page allows you to add a new article which is shown on the front-end website.</p><br />

<form method="post" action="addarticle.php">
    <fieldset>
        <dl>
            <dt>
                <label for="title">Title:</label><br />
                <span>A title shown on the news heading on front page.</span>
            </dt>
            <dd><input id="title" type="text" size="20" maxlength="255"
                       name="title" value="" />
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="description">Description:</label><br />
                <span>A brief summerization of what the article is about.</span>
            </dt>
            <dd>
                <textarea id="description" name="description" rows="5" cols="45"></textarea>
            </dd>
        </dl>

        <dl>
            <dt>
                <label for="story">Story:</label><br />
                <span>The full story of the article.</span>
            </dt>
            <dd>
                <textarea id="story" name="story" rows="10" cols="45"></textarea>
            </dd>
        </dl>
    </fieldset>

    <fieldset class="submit-buttons">
        <input class="button1" type="submit" id="submit" name="submit" value="  Submit Article  " />&nbsp;
    </fieldset>
</form>

<?php require_once("footer.php"); ?>
