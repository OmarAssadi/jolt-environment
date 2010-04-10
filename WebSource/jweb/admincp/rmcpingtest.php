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
define('ACP_TITLE', 'RMC Ping');
define('ACP_TAB', 2);
require_once("adminglobal.php");
check_rights();

require_once("header.php");
?>
<h1>RMC Ping Test</h1><hr>
<p>RMC (Remotely Managed Communication) is networking between the website, and the server.<br>This script will help notify if the server is responsive to a test.</p><br />

<script type="text/javascript">
    function reload() {
        var f = document.getElementById('pingframe');
        f.src = f.src;
    }
</script>
<iframe id="pingframe" height="40" width="300"
        frameborder='0' src="pingframe.php"></iframe>
<br>
<input type='button' class="button2" onclick='reload()' value='Ping'/>

<?php require_once("footer.php"); ?>