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
define('PAGE_TAB', 0);
define('PAGE_TITLE', 'Create');

require_once("global.php");
require_once("header.php");

if (isset($_POST['submit'])) {
    $name = $_POST['username'];
    $pass1 = $_POST['password1'];
    $pass2 = $_POST['password2'];
    $day = $_POST['day'];
    $month = $_POST['month'];
    $year = $_POST['year'];
    $country = $_POST['country'];
    $email = $_POST['email'];

    if (check_name($name) && check_password($pass1) && check_passwords($pass1, $pass2) && check_year($year) && check_email($email)) {
        $dbname = str_replace(' ', '_', strtolower(trim($name)));
        $pass = sha1($dbname . sha1($pass1));
        $dob = $day . '-' . $month . '-' . $year;
        $ip = $_SERVER["REMOTE_ADDR"];

        if ($email == "") {
            dbquery("INSERT INTO characters (username,password,dob,country,register_ip,register_date)
            VALUES ('$dbname','$pass','$dob','$country','$ip',NOW());");
        } else {
            dbquery("INSERT INTO characters (username,password,dob,country,email,register_ip,register_date)
            VALUES ('$dbname','$pass','$dob','$country','$email','$ip',NOW());");
        }
        
        $uid = dbevaluate("SELECT id FROM characters WHERE username = '$dbname' AND password = '$pass';");
        dbquery("INSERT INTO character_preferences (master_id) VALUES ('$uid');");

        die('<script type="text/javascript">top.location.href = \'' . WWW . '/create?created=' . $dbname . '\';</script>');
    }
}

if (isset($_GET['created'])) {
    $created = $_GET['created'];
}

function check_name($user) {
    if(strlen($user) < 1 || !preg_match("%[a-zA-Z0-9\ ]%", $user)) {
        return true;
    }

    $query = dbquery("SELECT id FROM characters WHERE username='$user' LIMIT 1;");
    if(mysql_num_rows($query) < 1) {
        return true;
    } else {
        return false;
    }
}

function check_password($pass) {
    if (strlen($pass) < 5 || strlen($pass) > 20) {
        return false;
    } else {
        return true;
    }
}

function check_passwords($pass1, $pass2) {
    if ($pass1 != $pass2) {
        return false;
    } else {
        return true;
    }
}

function check_year($year) {
    if (!is_numeric($year) || $year < 1800 || $year > 2010) {
        return false;
    } else {
        return true;
    }
}
?>

<script type="text/javascript">  //function to create ajax object
    function pullAjax(){
        var a;
        try{
            a = new XMLHttpRequest()
        }
        catch(b)
        {
            try
            {
                a = new ActiveXObject("Msxml2.XMLHTTP")
            }
            catch(b)
            {
                try
                {
                    a = new ActiveXObject("Microsoft.XMLHTTP")
                }
                catch(b)
                {
                    alert("Your browser broke!");return false
                }
            }
        }
        return a;
    }

    function validate_name()
    {
        var x = document.getElementById('username');
        var msg = document.getElementById('errorUsername');
        user = x.value;

        code = '';
        message = '';
        obj=pullAjax();
        obj.onreadystatechange=function()
        {
            if(obj.readyState==4)
            {
                eval("result = "+obj.responseText);
                code = result['code'];
                message = result['result'];

                if(code <=0)
                {
                    x.className = "fail";
                    x.style.border = "2px solid red";
                    msg.style.color = "red";
                }
                else
                {
                    x.className = "success";
                    x.style.border = "2px solid green";
                    msg.style.color = "green";
                }
                msg.innerHTML = message;
            }
        }
        obj.open("GET", "validate.php?username="+user, true);
        obj.send(null);
    }

    function validate_pass()
    {
        var pass1 = document.getElementById('password1');
        var pass2 = document.getElementById('password2');
        var msg = document.getElementById('errorPassword');

        var l = pass1.value.toString().length;
        var l2 = pass2.value.toString().length;

        if (l < 5 || l > 20)
        {
            pass1.className = "fail";
            pass1.style.border = "2px solid red";
            msg.style.color = "red";
            msg.innerHTML = "<b>Password needs to be more than 5 characters, and below 20.</b>";
        }
        else if (l2 > 1 && pass1.value != pass2.value)
        {

            pass1.className = "";
            pass1.style.border = "";

            pass2.className = "fail";
            pass2.style.border = "2px solid red";
            msg.style.color = "red";
            msg.innerHTML = "<b>Passwords do not match.</b>";
        }
        else if (pass1.value == pass2.value)
        {
            pass1.className = "success";
            pass1.style.border = "2px solid green";
            pass2.className = "success";
            pass2.style.border = "2px solid green";
            msg.style.color = "green";
            msg.innerHTML = "&nbsp;";
        }
    }

    function validate_email()
    {
        var x = document.getElementById('address1');
        var msg = document.getElementById('errorAddress');
        user = x.value;

        code = '';
        message = '';
        obj=pullAjax();
        obj.onreadystatechange=function()
        {
            if(obj.readyState==4)
            {
                eval("result = "+obj.responseText);
                code = result['code'];
                message = result['result'];

                if(code <=0)
                {
                    x.className = "fail";
                    x.style.border = "2px solid red";
                    msg.style.color = "red";
                }
                else
                {
                    x.className = "success";
                    x.style.border = "2px solid green";
                    msg.style.color = "green";
                }
                msg.innerHTML = message;
            }
        }
        obj.open("GET", "validate.php?email="+user, true);
        obj.send(null);
    }

    function validate_date()
    {
        var x = document.getElementById('year');
        var msg = document.getElementById('errorData');

        if (!is_numeric(x.value) || x.value < 1800 || x.value > 2010)
        {
            msg.style.color = "red";
            msg.innerHTML = "Invalid year.";
        }
        else
        {
            msg.innerHTML = "&nbsp;";
        }
    }

    function is_numeric(input)
    {
        return (input - 0) == input && input.length > 0;
    }
</script>

<style type="text/css">/*\*/@import url(<?php echo WWW; ?>/css/create3-11.css);/**/</style>

<div id="article">
    <div class="sectionHeader">
        <div class="left">
            <div class="right">
                <div class="plaque">
                    Create a Free Account
                </div>
            </div>
        </div>
    </div>
    <div class="section">
        <div class="brown_background sectionContentContainer">
            <div class="inner_brown_background">
                <div class="brown_box">

                    <?php
                    if ($created) {
                        ?>
                    <div class="width756">
                        <div class="inner_brown_box" style="padding: 8px;">
                            <p>Your account <span class="usernamecreate"><?php echo $created ?></span> has now been created with the password you have chosen. We recommend you make a note of it on a bit of paper and keep it somewhere <strong>really</strong> safe, in case you forget it.</p>
                            <center><h2><a href="<?php echo WWW; ?>/play.php">Play Now</a></h2></center>
                        </div>
                        <br class="clear" />
                    </div>
                        <?php
                    } else {
                        ?>
                    <div class="width756">
                        <div id="errorlog" style="position: absolute; top: 5px; right: 5px; float: right; padding: 5px;"></div>
                        <form method="post" id="createForm" action="create">
                            <div class="inner_brown_box brown_box_stack" id="cIntro">
                                Creating an account for RageScape is a simple, free and short process. Fill in the information below, and start playing now!
                            </div>
                            <div id="formBoxes" class="inner_brown_box brown_box_stack brown_box_padded">
                                <div class="formSuperGroup single_line">
                                    <div class="formSection" id="usr">
                                        <label for="username">Your Username:</label>
                                        <input id="username" name="username" autocomplete="off" maxlength="12" onblur="validate_name()" value="">
                                        <br class="clear" />
                                    </div>
                                    <div class="error formError" id="errorUsername">&nbsp;</div>
                                    <br class="clear" />
                                </div>

                                <div class="formSuperGroup double_line">
                                    <div id="pass" class="formGroup">
                                        <div class="formSection">
                                            <label for="password1">Your Password:</label>
                                            <input id="password1" name="password1" type="password" autocomplete="off" value="" maxlength="20" onblur="validate_pass();">
                                        </div>
                                        <div class="formSection">
                                            <label for="password2">Re-enter Password:</label>
                                            <input id="password2" name="password2" type="password" autocomplete="off" value="" maxlength="20" onblur="validate_pass();">
                                        </div>
                                        <br class="clear" />
                                    </div>
                                    <div class="error formError" id="errorPassword">&nbsp;</div>
                                    <br class="clear" />
                                </div>

                                <div class="formSuperGroup double_line">
                                    <div id="data" class="formGroup">
                                        <div class="formSection">
                                            <label for="day">Your Date of Birth:</label>
                                            <div>
                                                <select id="day" name="day">
                                                    <option value="-1" selected="selected" disabled="disabled">Day</option>
                                                    <option value="1">1</option>
                                                    <option value="2">2</option>
                                                    <option value="3">3</option>
                                                    <option value="4">4</option>
                                                    <option value="5">5</option>
                                                    <option value="6">6</option>
                                                    <option value="7">7</option>
                                                    <option value="8">8</option>
                                                    <option value="9">9</option>
                                                    <option value="10">10</option>
                                                    <option value="11">11</option>
                                                    <option value="12">12</option>
                                                    <option value="13">13</option>
                                                    <option value="14">14</option>
                                                    <option value="15">15</option>
                                                    <option value="16">16</option>
                                                    <option value="17">17</option>
                                                    <option value="18">18</option>
                                                    <option value="19">19</option>
                                                    <option value="20">20</option>
                                                    <option value="21">21</option>
                                                    <option value="22">22</option>
                                                    <option value="23">23</option>
                                                    <option value="24">24</option>
                                                    <option value="25">25</option>
                                                    <option value="26">26</option>
                                                    <option value="27">27</option>
                                                    <option value="28">28</option>
                                                    <option value="29">29</option>
                                                    <option value="30">30</option>
                                                    <option value="31">31</option>
                                                </select>
                                                <select id="month" name="month">
                                                    <option value="-1" selected="selected" disabled="disabled">Month</option>
                                                    <option value="1">January</option>
                                                    <option value="2">February</option>
                                                    <option value="3">March</option>
                                                    <option value="4">April</option>
                                                    <option value="5">May</option>
                                                    <option value="6">June</option>
                                                    <option value="7">July</option>
                                                    <option value="8">August</option>
                                                    <option value="9">September</option>
                                                    <option value="10">October</option>
                                                    <option value="11">November</option>
                                                    <option value="12">December</option>
                                                </select>
                                                <input id="year" name="year" maxlength="4" value="Year" onfocus="this.value=''" onchange="validate_date()">
                                            </div>
                                        </div>
                                        <div class="formSection country" >
                                            <label for="country">Your Country of Residence:</label>
                                            <select id="country" name="country" onfocus="display(this.parentNode.parentNode);">
                                                <option value="-1">Select one</option>
                                                <optgroup label="---">
                                                    <option value="5">Afghanistan</option>
                                                    <option value="8">Albania</option>
                                                    <option value="61">Algeria</option>
                                                    <option value="14">American Samoa</option>
                                                    <option value="3">Andorra</option>
                                                    <option value="11">Angola</option>
                                                    <option value="7">Anguilla</option>
                                                    <option value="12">Antarctica</option>
                                                    <option value="6">Antigua and Barbuda</option>
                                                    <option value="13">Argentina</option>
                                                    <option value="9">Armenia</option>
                                                    <option value="17">Aruba</option>
                                                    <option value="16">Australia</option>
                                                    <option value="15">Austria</option>
                                                    <option value="18">Azerbaijan</option>
                                                    <option value="32">Bahamas</option>
                                                    <option value="25">Bahrain</option>
                                                    <option value="21">Bangladesh</option>
                                                    <option value="20">Barbados</option>
                                                    <option value="36" selected="selected">Belarus</option>
                                                    <option value="22">Belgium</option>
                                                    <option value="37">Belize</option>
                                                    <option value="27">Benin</option>
                                                    <option value="28">Bermuda</option>
                                                    <option value="33">Bhutan</option>
                                                    <option value="30">Bolivia</option>
                                                    <option value="19">Bosnia and Herzegovina</option>
                                                    <option value="35">Botswana</option>
                                                    <option value="34">Bouvet Island</option>
                                                    <option value="31">Brazil</option>
                                                    <option value="104">British Indian Ocean Territory</option>
                                                    <option value="29">Brunei Darussalam</option>
                                                    <option value="24">Bulgaria</option>
                                                    <option value="23">Burkina Faso</option>
                                                    <option value="26">Burundi</option>
                                                    <option value="114">Cambodia</option>
                                                    <option value="47">Cameroon</option>
                                                    <option value="38">Canada</option>
                                                    <option value="52">Cape Verde</option>
                                                    <option value="121">Cayman Islands</option>
                                                    <option value="41">Central African Republic</option>
                                                    <option value="207">Chad</option>
                                                    <option value="46">Chile</option>
                                                    <option value="48">China</option>
                                                    <option value="53">Christmas Island</option>
                                                    <option value="39">Cocos (Keeling) Islands</option>
                                                    <option value="49">Colombia</option>
                                                    <option value="116">Comoros</option>
                                                    <option value="42">Congo</option>
                                                    <option value="40">Congo, The Democratic Republic of the</option>
                                                    <option value="45">Cook Islands</option>
                                                    <option value="50">Costa Rica</option>
                                                    <option value="44">Cote D'Ivoire</option>
                                                    <option value="97">Croatia</option>
                                                    <option value="51">Cuba</option>
                                                    <option value="54">Cyprus</option>
                                                    <option value="55">Czech Republic</option>
                                                    <option value="58">Denmark</option>
                                                    <option value="57">Djibouti</option>
                                                    <option value="59">Dominica</option>
                                                    <option value="60">Dominican Republic</option>
                                                    <option value="216">East Timor</option>
                                                    <option value="62">Ecuador</option>
                                                    <option value="64">Egypt</option>
                                                    <option value="203">El Salvador</option>
                                                    <option value="87">Equatorial Guinea</option>
                                                    <option value="66">Eritrea</option>
                                                    <option value="63">Estonia</option>
                                                    <option value="68">Ethiopia</option>
                                                    <option value="71">Falkland Islands (Malvinas)</option>
                                                    <option value="73">Faroe Islands</option>
                                                    <option value="70">Fiji</option>
                                                    <option value="69">Finland</option>
                                                    <option value="74">France</option>
                                                    <option value="75">France, Metropolitan</option>
                                                    <option value="80">French Guiana</option>
                                                    <option value="170">French Polynesia</option>
                                                    <option value="208">French Southern Territories</option>
                                                    <option value="76">Gabon</option>
                                                    <option value="84">Gambia</option>
                                                    <option value="79">Georgia</option>
                                                    <option value="56">Germany</option>
                                                    <option value="81">Ghana</option>
                                                    <option value="82">Gibraltar</option>
                                                    <option value="88">Greece</option>
                                                    <option value="83">Greenland</option>
                                                    <option value="78">Grenada</option>
                                                    <option value="86">Guadeloupe</option>
                                                    <option value="91">Guam</option>
                                                    <option value="90">Guatemala</option>
                                                    <option value="85">Guinea</option>
                                                    <option value="92">Guinea-Bissau</option>
                                                    <option value="93">Guyana</option>
                                                    <option value="98">Haiti</option>
                                                    <option value="95">Heard Island and McDonald Islands</option>
                                                    <option value="228">Holy See (Vatican City State)</option>
                                                    <option value="96">Honduras</option>
                                                    <option value="94">Hong Kong</option>
                                                    <option value="99">Hungary</option>
                                                    <option value="107">Iceland</option>
                                                    <option value="103">India</option>
                                                    <option value="100">Indonesia</option>
                                                    <option value="106">Iran, Islamic Republic of</option>
                                                    <option value="105">Iraq</option>
                                                    <option value="101">Ireland</option>
                                                    <option value="102">Israel</option>
                                                    <option value="108">Italy</option>
                                                    <option value="109">Jamaica</option>
                                                    <option value="111">Japan</option>
                                                    <option value="110">Jordan</option>
                                                    <option value="122">Kazakstan</option>
                                                    <option value="112">Kenya</option>
                                                    <option value="115">Kiribati</option>
                                                    <option value="118">Korea, Democratic People's Republic of</option>
                                                    <option value="119">Korea, Republic of</option>
                                                    <option value="120">Kuwait</option>
                                                    <option value="113">Kyrgyzstan</option>
                                                    <option value="123">Lao People's Democratic Republic</option>
                                                    <option value="132">Latvia</option>
                                                    <option value="124">Lebanon</option>
                                                    <option value="129">Lesotho</option>
                                                    <option value="128">Liberia</option>
                                                    <option value="133">Libyan Arab Jamahiriya</option>
                                                    <option value="126">Liechtenstein</option>
                                                    <option value="130">Lithuania</option>
                                                    <option value="131">Luxembourg</option>
                                                    <option value="143">Macau</option>
                                                    <option value="139">Macedonia, the Former Yugoslav Republic of</option>
                                                    <option value="137">Madagascar</option>
                                                    <option value="151">Malawi</option>
                                                    <option value="153">Malaysia</option>
                                                    <option value="150">Maldives</option>
                                                    <option value="140">Mali</option>
                                                    <option value="148">Malta</option>
                                                    <option value="138">Marshall Islands</option>
                                                    <option value="145">Martinique</option>
                                                    <option value="146">Mauritania</option>
                                                    <option value="149">Mauritius</option>
                                                    <option value="238">Mayotte</option>
                                                    <option value="152">Mexico</option>
                                                    <option value="72">Micronesia, Federated States of</option>
                                                    <option value="136">Moldova, Republic of</option>
                                                    <option value="135">Monaco</option>
                                                    <option value="142">Mongolia</option>
                                                    <option value="242">Montenegro</option>
                                                    <option value="147">Montserrat</option>
                                                    <option value="134">Morocco</option>
                                                    <option value="154">Mozambique</option>
                                                    <option value="141">Myanmar</option>
                                                    <option value="155">Namibia</option>
                                                    <option value="164">Nauru</option>
                                                    <option value="163">Nepal</option>
                                                    <option value="161">Netherlands</option>
                                                    <option value="10">Netherlands Antilles</option>
                                                    <option value="156">New Caledonia</option>
                                                    <option value="166">New Zealand</option>
                                                    <option value="160">Nicaragua</option>
                                                    <option value="157">Niger</option>
                                                    <option value="159">Nigeria</option>
                                                    <option value="165">Niue</option>
                                                    <option value="158">Norfolk Island</option>
                                                    <option value="144">Northern Mariana Islands</option>
                                                    <option value="162">Norway</option>
                                                    <option value="167">Oman</option>
                                                    <option value="173">Pakistan</option>
                                                    <option value="180">Palau</option>
                                                    <option value="178">Palestinian Territory, Occupied</option>
                                                    <option value="168">Panama</option>
                                                    <option value="171">Papua New Guinea</option>
                                                    <option value="181">Paraguay</option>
                                                    <option value="169">Peru</option>
                                                    <option value="172">Philippines</option>
                                                    <option value="176">Pitcairn</option>
                                                    <option value="174">Poland</option>
                                                    <option value="179">Portugal</option>
                                                    <option value="177">Puerto Rico</option>
                                                    <option value="182">Qatar</option>
                                                    <option value="183">Reunion</option>
                                                    <option value="184">Romania</option>
                                                    <option value="185">Russian Federation</option>
                                                    <option value="186">Rwanda</option>
                                                    <option value="193">Saint Helena</option>
                                                    <option value="117">Saint Kitts and Nevis</option>
                                                    <option value="125">Saint Lucia</option>
                                                    <option value="175">Saint Pierre and Miquelon</option>
                                                    <option value="229">Saint Vincent and the Grenadines</option>
                                                    <option value="236">Samoa</option>
                                                    <option value="198">San Marino</option>
                                                    <option value="202">Sao Tome and Principe</option>
                                                    <option value="187">Saudi Arabia</option>
                                                    <option value="199">Senegal</option>
                                                    <option value="239">Serbia</option>
                                                    <option value="189">Seychelles</option>
                                                    <option value="197">Sierra Leone</option>
                                                    <option value="192">Singapore</option>
                                                    <option value="196">Slovakia</option>
                                                    <option value="194">Slovenia</option>
                                                    <option value="188">Solomon Islands</option>
                                                    <option value="200">Somalia</option>
                                                    <option value="240">South Africa</option>
                                                    <option value="89">South Georgia and the South Sandwich Islands</option>
                                                    <option value="67">Spain</option>
                                                    <option value="127">Sri Lanka</option>
                                                    <option value="190">Sudan</option>
                                                    <option value="201">Suriname</option>
                                                    <option value="195">Svalbard and Jan Mayen</option>
                                                    <option value="205">Swaziland</option>
                                                    <option value="191">Sweden</option>
                                                    <option value="43">Switzerland</option>
                                                    <option value="204">Syrian Arab Republic</option>
                                                    <option value="220">Taiwan</option>
                                                    <option value="211">Tajikistan</option>
                                                    <option value="221">Tanzania, United Republic of</option>
                                                    <option value="210">Thailand</option>
                                                    <option value="209">Togo</option>
                                                    <option value="212">Tokelau</option>
                                                    <option value="215">Tonga</option>
                                                    <option value="218">Trinidad and Tobago</option>
                                                    <option value="214">Tunisia</option>
                                                    <option value="217">Turkey</option>
                                                    <option value="213">Turkmenistan</option>
                                                    <option value="206">Turks and Caicos Islands</option>
                                                    <option value="219">Tuvalu</option>
                                                    <option value="223">Uganda</option>
                                                    <option value="222">Ukraine</option>
                                                    <option value="4">United Arab Emirates</option>
                                                    <option value="77">United Kingdom</option>
                                                    <option value="225">United States</option>
                                                    <option value="224">United States Minor Outlying Islands</option>
                                                    <option value="226">Uruguay</option>
                                                    <option value="227">Uzbekistan</option>
                                                    <option value="234">Vanuatu</option>
                                                    <option value="230">Venezuela</option>
                                                    <option value="233">Vietnam</option>
                                                    <option value="231">Virgin Islands, British</option>
                                                    <option value="232">Virgin Islands, U.S.</option>
                                                    <option value="235">Wallis and Futuna</option>
                                                    <option value="65">Western Sahara</option>
                                                    <option value="237">Yemen</option>
                                                    <option value="241">Zambia</option>
                                                    <option value="243">Zimbabwe</option>
                                                </optgroup>
                                            </select>
                                        </div>
                                        <br class="clear" />
                                    </div>
                                    <div class="error formError" id="errorData">&nbsp;</div>
                                    <br class="clear" />
                                </div>

                                <div class="formSuperGroup single_line" style="margin-top: 0px;">
                                    <div id="email" class="formGroup">
                                        <div class="formSection">
                                            <label for="address1" style="line-height: 13px;">Email (Optional):</label>
                                            <div>
                                                <input type="text" class="textBox" id="address1" name="email" value="" onblur="validate_email()">
                                            </div>
                                        </div>
                                        <br class="clear" />
                                        <div class="error formError" id="errorAddress">&nbsp;</div>
                                    </div>
                                    <br class="clear" />
                                </div>

                            </div>
                            <div class="inner_brown_box brown_box_padded">
                                <button type="submit" value="" id="submitbutton" name="submit"></button>
                            </div>
                        </form>
                    </div>
                        <?php } ?>
                </div>
            </div>
        </div>
    </div>
</div> 

<?php require_once("footer.php"); ?>
