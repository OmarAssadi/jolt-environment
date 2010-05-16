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
define('ACP_TITLE', 'Offences');
define('ACP_TAB', 5);
require_once("adminglobal.php");
check_rights();

require_once("header.php");

$now = date_create();
$ban_date = dbdate($now->getTimestamp());
print("$ban_date <br />");
$ban_expire = dbdate($now->getTimestamp());
print($ban_expire);
//dbquery("INSERT INTO offences (userid,type,date,expire_date,moderatorid,reason)
//    VALUES ('1','1',NOW(), NOW(),'1', 'nothing');");
?>

<?php require_once("footer.php"); ?>
