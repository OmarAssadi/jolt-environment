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

define('ACP_TITLE', 'Moderation');
define('ACP_TAB', 5);

include_once("adminglobal.php");
check_rights();

include_once("header.php");
?>

<h1>Moderation</h1><hr>
<p>Moderators and Administrators can manage moderation here. They are provided with tools, logs, and information to make the best out of the evidence they have.</p><br />

<h2>Actions requiring moderation</h2>
<p>A list of actions that are currently under moderation.</p>

<?php include_once("footer.php"); ?>
