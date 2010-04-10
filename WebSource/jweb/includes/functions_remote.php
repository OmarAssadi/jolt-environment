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

/**
 * Sends information to the given remote server.
 * @param string $data Information about the command and arguments.
 * @return string Information recieved according to the data sent.
 */
function send_data($data) {
    $connection = @fsockopen ("127.0.0.1", 43595, $fserrno, $fserrstr, 1);
    
    if ($connection) {
        fwrite($connection, $data);
        $rd = fread($connection, 256);
        fclose($connection);
        return $rd;
    } else {
        return null;
    }
}
?>
