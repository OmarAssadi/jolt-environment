
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
    <head>
        <title>Jagex Ltd.</title>
        <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
        <meta http-equiv="Content-Language" content="en-gb, English">
        <meta http-equiv="imagetoolbar" content="no">
        <meta http-equiv="EXPIRES" content="0">
        <meta http-equiv="PRAGMA" content="NO-CACHE">
        <meta http-equiv="CACHE-CONTROL" content="NO-CACHE">
        <link href="https://www.runescape.com/css/jagex/print-41.css" rel="stylesheet" type="text/css" media="print">
        <style type="text/css" media="screen">
            @import url("https://www.runescape.com/css/jagex/global-41.css");
        </style>

        <style class="text/css">
            table{
                width:100%;
            }
            tr.vspacer td{
                background-color: #453E33;
            }
            tr.form_row td{
                height:50px;
            }
            td{
                vertical-align: middle;
            }
            td.tac_pp_row{
                vertical-align: top;
            }
            td.addressError{
                width: 21%;
            }
            tr.entrypair_top td{
                height: 40px;
                padding-top: 15px;
            }
            tr.entrypair_bot td{
                height: 40px;
                padding-bottom: 15px;
            }
            div.padded{
                padding:20px;
            }
            span.label{
                font-weight:bold;
                line-height:16px;
            }
            span.address{
                font-weight: bold;
                color: #DBC68F;
            }
            span.err_msg{
                color:#CC0000;
                line-height:16px
            }
            .inner_title {
                background: #453e33 url(https://www.jagex.com/img/jagex/innerbox_title.png);
                margin: auto;
                width: 723px;
                line-height:26px;
                color: #DBC68F;
                font-size: 12px;
                font-weight: bold;
                text-align: center;
            }
            .inner_box {
                background: #302a21;
                padding-left: 15px;
                padding-right: 15px;
                width: 689px;
                margin: auto;
                line-height: 32px;
                border-top: 1px solid black;
                border-left: 2px solid black;
                border-right: 2px solid black;
            }
            .inner_bottom {
                font-size: 0px;
                margin: auto;
                height:6px;
                width:723px;
                background: url(https://www.jagex.com/img/jagex/innerbox_bottom.png);
            }
            .inputMedium {
                line-height: 20px;
            }
            .inputLong {
                line-height: 20px;
            }
        </style>
        <style type="text/css">
            #checkboxexplain p{
                padding:    0px;
                line-height:  20px;
            }
            input.cb{
                cursor:     pointer;
                float:     left;
                margin-bottom: 25px;
                margin-right:  8px;
            }
        </style>
        <script type="text/javascript">
            function check_emails(form, under13) {

                var err=false;

                var na1 = document.getElementById("na");
                var na2 = document.getElementById("na2");
                var oa = document.getElementById("oa");

                if(oa){document.getElementById("oa_err_msg").innerHTML="";}
                document.getElementById("na_err_msg").innerHTML="";
                document.getElementById("na2_err_msg").innerHTML="";
                if(!oa){document.getElementById("tandc_err_msg").innerHTML="&nbsp;";}

                if(oa){
                    if(oa.value.length>0){
                        if(!emailCheck(oa.value,document.getElementById("oa_err_msg"))){ err=true;}
                    }
                }

                if(na1.value.length>0){
                    if(na1.value.toLowerCase() != na2.value.toLowerCase()){
                        var message = document.getElementById("na2_err_msg");
                        message.innerHTML = "Confirmation address does not match new address.";
                        err=true;
                    }
                    else if(!emailCheck(na1.value, document.getElementById("na_err_msg"))){ err=true;}
                    else if(oa && oa.value && oa.value.toLowerCase() == na1.value.toLowerCase()) {
                        var message = document.getElementById("na_err_msg");
                        message.innerHTML = "Your new address matches your old address.";
                        err=true;
                    }
                }
                if(na1.value.length<1){
                    var message = document.getElementById("na_err_msg");
                    message.innerHTML = "This field is required";
                    err=true;
                }
                if(na2.value.length<1){
                    var message = document.getElementById("na2_err_msg");
                    message.innerHTML = "This field is required";
                    err=true;
                }

                err2 = !validate_checkboxes();
                if (!err) err=err2;
                return !err;
            }
            function validate_checkboxes(){
                var err = false;

                var tandc = document.getElementById("agree_privacy");
                var message = document.getElementById("tandc_err_msg");

                if(tandc){
                    message.innerHTML = "";
                    if((!tandc.checked)){
                        message.innerHTML = "You must agree to our Privacy Policy and Terms & Conditions in order to register your account.";
                        err=true;
                    }
                }
                return !err;
            }
        </script>
        <script type="text/javascript">
            function emailCheck (emailStr,errSpan) {
                if(!emailStr || emailStr.length == 0){
                    errSpan.innerHTML="This field is required"
                    return false;
                }
                var emailPat=/^([^@]+)@(.+)$/;
                var localPartChars="a-zA-Z0-9!#$%&'*+\\\/=?^_`\\{\\|\\}~-";
                var domainPartChars="a-zA-Z0-9-";
                var quotedLocal=/^".+"$/;
                var localPat=new RegExp("^([^\\.]+)(\\.([^\\.]+))*$","g");
                var atomPat=new RegExp("^[\\."+localPartChars + "]+$");
                var labelPat=new RegExp("^["+ domainPartChars +"]+$");
                var labelPat2=new RegExp("^-|-$");

                var matchArray=emailStr.match(emailPat);
                if (matchArray==null){
                    errSpan.innerHTML="This email address is invalid.";
                    return false;
                }

                var local=matchArray[1];
                if (local.length == 0){
                    errSpan.innerHTML="This email address is invalid.";
                    return false;
                }
                if (local.length > 64){
                    errSpan.innerHTML="The address you have submitted is too long.";
                    return false;
                }
                if (local.match(quotedLocal)!=null){
                    local=local.substring(1,local.length-1);
                    if(local.match(new RegExp("[^\\\\]\"|^\"$","g"))!=null){
                        errSpan.innerHTML="This email address contains invalid characters.";
                        return false;
                    }
                }
                else{
                    var atoms = local.match(localPat);
                    if (atoms==null) {
                        errSpan.innerHTML="This email address is invalid.";
                        return false;
                    }
                    var a = atoms[0];
                    if (a.match(atomPat)==null){
                        errSpan.innerHTML="This email address contains invalid characters.";
                        return false;
                    }
                }

                var domain=matchArray[2];
                if (domain.length == 0){
                    errSpan.innerHTML="This email address is invalid.";
                    return false;
                }
                if (domain.length > 255){
                    errSpan.innerHTML="The address you have submitted is too long.";
                    return false;
                }

                var domArray=domain.split(".");
                if (domArray.length < 2){
                    errSpan.innerHTML="This email address is invalid.";
                    return false;
                }
                for (i=0; i<domArray.length; i++){
                    var label=domArray[i];
                    if (label.length==0) {
                        errSpan.innerHTML="This email address is invalid.";
                        return false;
                    }
                    if (label.length > 63) {
                        errSpan.innerHTML="The address you have submitted is too long.";
                        return false;
                    }
                    if (label.match(labelPat)==null){
                        errSpan.innerHTML="This email address contains invalid characters.";
                        return false;
                    }
                    if (label.match(labelPat2)!=null){
                        errSpan.innerHTML="This email address contains invalid characters.";
                        return false;
                    }
                }
                var tld=domArray[domArray.length-1];
                if (tld.match(new RegExp("[^0-9]+"))==null){
                    errSpan.innerHTML="This email address is invalid.";
                    return false;
                }
                return true;
            }
        </script>
    </head>
    <body id="register_address">


        <div id="container">
            <div id="header">
                <ul id="headerNavLarge">
                    <li><span>Shuxboi logged in</span></li>
                </ul>
                <ul id="headerNav">
                    <li><span><a href="http://www.runescape.com/c=PH8RsHc9gYc/title.ws">
                                RuneScape Home</a> | <a href="https://secure.runescape.com/m=weblogin/c=mmcqQVEZCMg/logout.ws">Logout</a></span></li>
                </ul>
                <div id="innerHeader">
                    <span id="innerHeaderCnr"></span>
                    <a id="innerHeaderLogo" href="http://www.runescape.com/c=PH8RsHc9gYc/">
                        <img src="https://www.runescape.com/img/jagex/header_logo_runescape.jpg?1" alt="RuneScape" />
                    </a>
                    <span id="logoJagex"></span>
                </div>
            </div>
            <div id="content">
                <div id="contentLeft">
                    <div class="title"><span>
                            Email Registration </span></div>
                    <div id="info" class="padded">
                        Your email address will be used to help you to recover your password and our newsletter will keep you informed. <br/><br/>
                        We will not share it with any third parties without your permission.
                    </div>
                    <div id="form_title"class="inner_title">
                        <span>Change Your Registered Email Address</span>
                    </div>
                    <div id="form" class="inner_box">
                        <form action="https://secure.runescape.com/m=email-register/c=mmcqQVEZCMg/submit_address.ws" method="post" onSubmit="return check_emails(this,false);">
                            <table>
                                <tr class="form_row">
                                    <td>
                                        <label for="oa">Your current email address:</label>
                                    </td>
                                    <td>
                                        <div class="inputMedium">
                                            <input type="text" name="oa" id="oa" value=""/>
                                        </div>
                                    </td>
                                    <td class="addressError">
                                        <span class="err_msg"  id="oa_err_msg">


                                        </span>
                                    </td>
                                </tr>
                                <tr class="vspacer">
                                    <td colspan=3></td>
                                </tr>
                                <tr class="form_row entrypair_top">
                                    <td>
                                        <label for="na">Your new email address:</label>
                                    </td>
                                    <td>
                                        <div class="inputMedium">
                                            <input type="text" name="na" id="na" value=""/>
                                        </div>
                                    </td>
                                    <td class="addressError">
                                        <span class="err_msg"  id="na_err_msg">


                                        </span>
                                    </td>
                                </tr>
                                <tr class="form_row entrypair_bot">
                                    <td>
                                        <label for="na2">Confirm your email address:</label>
                                    </td>
                                    <td>
                                        <div class="inputMedium">
                                            <input type="text" name="na2" id="na2" value=""/>
                                        </div>
                                    </td>
                                    <td class="addressError">
                                        <span class="err_msg"  id="na2_err_msg">


                                        </span>
                                    </td>
                                </tr>
                            </table>
                            <input type="hidden" name="reg" value="0" />
                            <input type="hidden" name="action" value="-1">
                            <div style="padding:5px 0 10px 0">
                                <input class="buttonlarge2" type="submit" value="Submit" style="margin:auto;"/>
                            </div>
                        </form>
                    </div>
                    <div class="inner_bottom"></div>
                    <div class="padded">
                        When the change above has been verified, your new address will be associated with your account.
                        <br/><br/>
                        Please be aware that if you submit an incorrect email address, you will need to return here to cancel the registration process.
                    </div>
                </div>
            </div>
            <div id="footer">
                <ul>
                    <li><a href="http://www.runescape.com/c=PH8RsHc9gYc/kbase/guid/jagex" target="_blank">About Jagex</a></li>
                </ul>
                <div id="copyRight">&copy; Jagex 1999 - 2010</div>
            </div>
            <div id="TnC">
                By using our service you are agreeing to our <a href="http://www.runescape.com/c=PH8RsHc9gYc/terms/terms.ws" name="terms">Terms &amp; Conditions</a> and <a href="http://www.runescape.com/c=PH8RsHc9gYc/privacy/privacy.ws" name="privacy">Privacy Policy</a>.
            </div>
            <div class="clear"></div>
        </div>
        <br><br>
    </body>
</html> 