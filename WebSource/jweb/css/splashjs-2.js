





function _(o){
 return document.getElementById(o);
}

function addClass(o,c){
 if(!o) return;
 if(!o.className) o = _(o);
 o.className = o.className + " " + c;
}

function removeClass(o,c){
 if(!o) return;
 if(!o.className) o = _(o);
 var pattern = new RegExp(c, "g");
 o.className = o.className.replace(pattern);
}

function kidsByClass(o,c){
 var childs = _(o).getElementsByTagName("*");
 var out = new Array();
 var pattern = new RegExp("(\\s|^)" + c + "(\\s|$)");
 for(var i in childs){
  if(childs[i].className && pattern.test(childs[i].className)){
   out.push(childs[i]);
  }
 }
 return out;
}





function setCookie(name, value){
 var date = new Date();
 date.setTime(date.getTime() + 315360000000);
 var expires = date.toGMTString();
 document.cookie = name + "=" + value + "; expires=" + expires + "; path=/; domain=.runescape.com;";
}

function getCookie(name){
 var cookies = document.cookie.split('; ');
 var regex = '';
 regex = new RegExp("^" + name + "=(.*)");
 for(var i in cookies){
  if(t = cookies[i].match(regex)){
   return t[1];
  }
 }
 return null;
}


function init(){




_("play").href += "?j=1";




var skip = false;

var nS = _("noSplash");

if(nS){

 addClass(nS, "js");

 var cookieCount = getCookie(cookieName) || 0;
 if(cookieCount == cookieLimit){
  addClass(nS, "checked");
  skip = true;
  cookieCount = 0;
 }

 nS.onclick = function(){
  if(skip){
   removeClass(this, "checked");
   setCookie(cookieName, cookieCount);
  }else{
   addClass(this, "checked");
   setCookie(cookieName, cookieLimit);
  }
  skip = !skip;
  return false;
 };

}



var imgPath = 'http://www.runescape.com/img/main/splash/';

_("trailer").onclick = function(){ return mediaSwitch("screenshots", "trailer"); }
_("screenshots").onclick = function(){ return mediaSwitch("trailer", "screenshots"); }

function mediaSwitch(a,b){
 removeClass(a, 'mediaButtonActive');
 removeClass(a+"Content", 'mediaContentActive');
 _(a).getElementsByTagName("img")[0].src = imgPath + a + ".png";

 addClass(b, 'mediaButtonActive');
 addClass(b+"Content", 'mediaContentActive');
 _(b).getElementsByTagName("img")[0].src = imgPath + b + "_active.png";

 _(b).blur();

 return false;
}




var numScreenShots = 8;

var screenImages = kidsByClass("screenshotsContent","screen");
var screenJump = kidsByClass("screenshotsContent","screenshotsJump");

for(var i in screenJump){
 screenJump[i].onclick = function(){
  var thisIndex;
  for(var i in screenJump){
   if(this == screenJump[i]){
    thisIndex = i;
    if(thisIndex == currentScreen){
     this.blur();
     return false;
    }
    break;
   }
  }
  this.blur();
  return screenJumpTo(thisIndex);
 }
}

_("next").onclick = function(){
 var next = currentScreen + 1;
 if(next >= numScreenShots){ next = 0; }

 this.blur();
 return screenJumpTo(next);
}

_("previous").onclick = function(){
 var previous = currentScreen - 1;
 if(previous < 0){ previous = numScreenShots - 1; }

 this.blur();
 return screenJumpTo(previous);
}

function screenJumpTo(thisIndex){

  screenImages[currentScreen].style.display = 'none';
  removeClass(screenJump[currentScreen], 'screenshotsJumpActive');

  screenImages[thisIndex].style.display = '';
  addClass(screenJump[thisIndex], 'screenshotsJumpActive');

  currentScreen = thisIndex;

  return false;

}




if(swfobject.hasFlashPlayerVersion(flashVer)){
 _("trailer").onclick();
}


};




(function(i) {
 var u = navigator.userAgent;
 var e = false;
 var st = setTimeout;
 if(/webkit/i.test(u)){
  st(function(){
   var dr = document.readyState;
   if(dr == "loaded" || dr == "complete"){
    i()
   }
   else{
    st(arguments.callee, 10);
   }
  }, 10);
 }
 else if((/mozilla/i.test(u) && !/(compati)/.test(u)) || (/opera/i.test(u))){
  document.addEventListener("DOMContentLoaded", i, false);
 }
 else if(e){
  (function(){
   var t = document.createElement('doc:rdy');
   try{
    t.doScroll('left');
    i();
    t = null;
   }
   catch(e){
    st(arguments.callee, 0);
   }
  })();
 }
 else{
  window.onload = i;
 }
})
(init);
