var username = '${{username}}';
var password = '${{password}}';
var linkHome = 'https://www.vidoix.com/';
var linkLogin = 'https://www.vidoix.com/login';
var isSetQuality = false;
inithome();
function cw(string) {
    console.log(string + `\t   date : ${getdateTime()}`);
}
function getdateTime() {
    var today = new Date();
    var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
    var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
    return date + ' ' + time;
}
function timeOut10(fn) {
    setTimeout(fn(), 10 * 1000);
}
function timeOut(fn, seconds) {
    setTimeout(fn(), seconds * 1000);
}
function setFunctions() {
    cw("setFunctions connect");
    alert = function () {
        return true;
    }
    confirm = function () {
        return true;
    }
    window.confirm = function () {
        return true;
    }
    document.hasFocus = function () {
        return true;
    }
    cw("inithome drop");
}
function inithome() {
    cw("inithome connect");
    setFunctions();
    whereAmI();
    cw("inithome drop");
}
function whereAmI() {
    cw("whereAmI connect");
    switch (window.location.href) {
        case linkHome:
            if (jQuery('a[href="/logout"]').length > 0 && jQuery('#filterCoins').length > 0 && jQuery('#loadMoreVideo').length > 0) {
                cw("whereAmI linkHome if");
                mainJS();
            } else {
                cw("whereAmI linkHome else");
                homeJS();
            }
            break;
        case linkLogin:
            cw("whereAmI linkLogin");
            loginJS();
            break;
        default:
            if (window.location.href.indexOf("youtube") > -1) {
                cw("whereAmI default if");
                videoJS();
            } else if (window.location.href.indexOf("dashboard") > -1) {
                cw("whereAmI default else if");
                mainJS();
            } else {
                cw("whereAmI default else");
            }
            break;
    }
    cw("whereAmI drop");
}
function homeJS() {
    cw("homeJS connect");
    window.location.href = linkLogin;
    cw("homeJS drop");
}
function loginJS() {
    cw("loginJS");
    jQuery('#username').val(username);
    jQuery('#password').val(password);
    jQuery('#js-login button').click();
    cw("loginJS drop");
}
function mainJS() {
    cw("mainJS connect");
    var counter = 0;
    var loadMoreVideoInterval = setInterval(function () {
        cw("mainJS loadMoreVideoInterval connect");
        counter += 1;
        cw(`counter:${counter}`);
        if (jQuery('#loadMoreVideo *').length > 0) {
            cw("contiune if");
            clearInterval(loadMoreVideoInterval);
            selectVideo();
        } else if (counter >= 7) {
            cw("contiune else if");
            clearInterval(loadMoreVideoInterval);
            selectVideo();
        } else {
            cw("contiune else");
        }
        cw("mainJS loadMoreVideoInterval drop");
    }, 1000);
    cw("mainJS drop");
}
function selectVideo() {
    cw("selectVideo connect");
    if (jQuery('#loadMoreVideo > div > a:first').length > 0) {
        window.location.href = jQuery('#loadMoreVideo > div > a:first').attr('href');
    } else {
        if (jQuery('#filterSecond > i').length > 0) {
            cw("contiune if");
            timeOut10(function () {
                jQuery('#filterCoins').click();
            });
        } else if (jQuery('#filterCoins > i').length > 0) {
            cw("contiune else if");
            timeOut10(function () {
                jQuery('#filterSecond').click();
            });
        } else {
            cw("contiune else");
            jQuery('#filterCoins').click();
        }
    }
    cw("selectVideo drop");
}
function videoJS() {
    cw("videoJS connect");
    jQuery(function () {
        Swal.fire = function () {
            window.location.href = linkHome;
        }
    });
    var isPlayerSetInterval = setInterval(function () {
        cw("videoJS isPlayerSetInterval connect");
        if (player !== undefined) {
            cw("contiune if");
            clearInterval(isPlayerSetInterval);
            player.addEventListener('onError', 'OnErrorQwerty');
            player.addEventListener('onReady', 'onReadyQwerty');
            player.addEventListener('onStateChange', 'onStateChangeQwerty');
        } else {
            cw("contiune else");
        }
        cw("videoJS isPlayerSetInterval drop");
    }, 250);
    cw("videoJS drop");
}
function OnErrorQwerty(event) {
    cw("OnErrorQwerty connect");
    //--$('a:contains("Geç")')[0].click();
    cw("OnErrorQwerty drop");
}
function onReadyQwerty(event) {
    cw("onReadyQwerty connect");
    player.mute();
    player.playVideo();
    playing = true;
    isFinish();
    cw("onReadyQwerty drop");
}
function onStateChangeQwerty(event) {
    cw("onStateChangeQwerty connect");
    if (event.data == YT.PlayerState.PLAYING && !isSetQuality) {
        cw("onStateChangeQwerty if");
        isSetQuality = !isSetQuality;
        console.log('//--setQualityToBestLowerQuality');
    } else if (event.data == YT.PlayerState.ENDED) {
        cw("onStateChangeQwerty else if");
        //--window.location.href = linkHome;
    } else {
        cw("onStateChangeQwerty else");
    }
    cw("onStateChangeQwerty drop");
}
function isFinish() {
    cw("isFinish connect");
    var isVideoFinishInterval = setInterval(function () {
        if (roundedPlayed >= length) {
            clearInterval(isVideoFinishInterval);
            //--window.location.href = linkHome;
        } else {
        }
    }, 500);
    cw("isFinish drop");
}
