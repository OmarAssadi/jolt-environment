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

require_once("./global.php");
require_once("./header.php");

$array = array();
$array[3] = $database->evaluate_query("SELECT COUNT(*) FROM characters;");
echo mysql_error();

?>
<body>
    jWeb coming soon.

    number: <?php echo $array[3] ?><br>
</body>
<?php
$data = send_data("test\0hey there buddy ol' pal!");
if ($data == null) {
    echo "failed";
} else {
    echo $data;
}

include_once("./footer.php");
?>
