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

require_once("global.php");

if (isset($_GET['username'])) {
    $user = str_replace(' ', '_', strtolower(trim($_GET['username'])));

    if(strlen($user) < 1 || !preg_match("%[a-zA-Z0-9\ ]%", $user)) {
        echo json_encode(array('code'  =>  -1,
        'result'  =>  '<b>Invalid username, please try again.</b>'
        ));
        die;
    }

    $query = dbquery("SELECT id FROM characters WHERE username='$user' LIMIT 1;");

    if(mysql_num_rows($query) < 1) {
        echo json_encode(array('code'  =>  1,
        'result'  =>  "<b>Username $user is still available.</b>"
        ));
        die;
    }
    else {
        echo  json_encode(array('code'  =>  0,
        'result'  =>  "<b>Sorry but username $user is already taken.</b>"
        ));
        die;
    }
} else if (isset($_GET['email'])) {
    if ($_GET['email'] != "" && preg_match('/^\S+@[\w\d.-]{2,}\.[\w]{2,6}$/iU', $_GET['email'])) {
        echo  json_encode(array('code'  =>  0,
        'result'  =>  "<b>Invalid email address.</b>"
        ));
        die;
    } else {
        echo  json_encode(array('code'  =>  1,
        'result'  =>  "&nbsp;"
        ));
        die;
    }
}
die;
?>
