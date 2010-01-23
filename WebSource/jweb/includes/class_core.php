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
 * Provides core functionality.
 *
 * @author AJ Ravindiran
 * @version 1.0.0 Jan-02-2010
 */
class core {

    private $config;

    /**
     * Gets the configuration value for the given key.
     * @param string $key The key to search for.
     * @return string The specified key's configuration value.
     */
    public function get_config($key) {
        return $this->config[$key];
    }

    /**
     * Tries to parse the configs in the config.php file.
     */
    public function parse_configs() {
        require_once(INCLUDES . "config.php");

        if (!isset($config) || count($config) < 2) {
            $this->system_error("configuration", "Configuration has missing data and/or has an invalid format.");
        }

        $this->config = $config;
    }

    /**
     * Prints out an error message and exits any calls.
     * @param string $error_type The type of error to be shown (eg. mysql, system, parsing)
     * @param string $text The message to show (usually the internal error caught).
     */
    public function system_error($error_type = "general", $text = "unknown") {
        print("<h1>System error: " . $error_type ."</h1>");
        print("<hr />");
        print("An error has occured. We apoligise for the possible inconvenience. Please try again later.");
        print("<hr />");
		print("<textarea cols='40' rows='6' readonly='readonly'>" . $text . "</textarea>");
		exit;
    }
}
?>
