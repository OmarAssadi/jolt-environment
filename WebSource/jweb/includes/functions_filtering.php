<?php
/* 
 *     RageScape Web
 *     Copyright (C) 2010 RageScape
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
 * Filters the given input by stripping slashes, trimming
 * eccess bits, and adding mysql escapes, to the string, thus
 * now making it safe enough to input into a mysql database.
 * @param string $string The string to filter.
 * @return string The string suitable to be stored into a mysql database.
 */
function filter_for_input($string) {
    return mysql_real_escape_string(stripslashes(trim($string)));
}

/**
 * Filters the given input by stripping slashs, trimming
 * eccess bits and setting specified conditions.
 * @param string $string The string to filer.
 * @param bool $ignore_html Whether to ignore html tagging.
 * @param bool $break_lines Whether to insert breaklines after every html newline.
 * @return string The string suiable to be displayed.
 */
function filter_for_outout($string, $ignore_html = false, $break_lines = false) {
    $output = stripslashes(trim($string));

    if ($ignore_html) {
        $output = htmlentities($output);
    }

    if ($break_lines) {
        $output = nl2br($output);
    }

    return $output;
}

/**
 * Generates a visually pretty date with a specified timestamp.
 * @param int $timestamp The timestamp.
 * @return string A pretty date.
 */
function prettydate($timestamp) {
    return date("F dS, Y h:i A", $timestamp);
}

/**
 * Generates a simplized date with the specified timestamp.
 * @param int $timestamp The timestamp.
 * @return string A simple date.
 */
function simpledate($timestamp) {
    return date('m-d-y', $timestamp);
}

/**
 * Formats a timestamp to MySQL datetime format.
 * @param int $timestamp The time to format.
 * @return string The formatted time.
 */
function dbdate($timestamp) {
    return date("Y-m-d H:i:s", $timestamp);
}

/**
 * Formats a DateTime object to MySQL datetime format.
 * @param DateTime $date The time to format
 * @return string The formatted time.
 */
function dbdate_format($date) {
    return date_format($date, "Y-m-d H:i:s");
}

?>
