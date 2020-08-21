var username = '${{username}}';
var password = '${{password}}';
var linkHome = 'https://www.vidoix.com/';
var linkLogin = 'https://www.vidoix.com/login';

//jQuery(function () {
inithome();
function inithome() {
    cw("inithome connect");
    setFunctions();
    whereAmI();
    cw("inithome drop");
}
function cw(string) {
    console.log(string + `\t   date : ${getdateTime()}`);
}
function getdateTime() {
    var today = new Date();
    var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
    var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
    return date + ' ' + time;
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
function whereAmI() {
    cw("whereAmI connect");
    switch (window.location.href) {
        case linkHome:
            if (jQuery(`span:contains("${username}")`).length === 1) {
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
    goLogin();
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
            timeOut(function () {
                jQuery('#filterCoins').click();
            });
        } else if (jQuery('#filterCoins > i').length > 0) {
            cw("contiune else if");
            timeOut(function () {
                jQuery('#filterSecond').click();
            });
        } else {
            cw("contiune else");
            jQuery('#filterCoins').click();
        }
    }
    cw("selectVideo drop");
}
function timeOut(fn) {
    setTimeout(fn(), 10 * 1000);
}
function videoJS() {
    cw("videoJS connect");
    var isPlayerSetInterval = setInterval(function () {
        cw("videoJS isPlayerSetInterval connect");
        if (player !== undefined) {
            cw("contiune if");
            clearInterval(isPlayerSetInterval);
            player.addEventListener('onError', 'playerOnError');
            playJS();
        } else {
            cw("contiune else");
        }
        cw("videoJS isPlayerSetInterval drop");
    }, 1000);
    cw("videoJS drop");
}
function playJS() {
    cw("playJS connect");
    playing = true;
    var isPlayedInterval = setInterval(function () {
        cw("playJS isPlayedInterval connect");
        if (player.playVideo !== undefined) {
            clearInterval(isPlayedInterval);
            player.playVideo();
            player.mute();
            isFinish();
        }
        cw("playJS isPlayedInterval drop");
    }, 1000);
    cw("playJS drop");
}
function isFinish() {
    cw("isFinish connect");
    var counter = 0;
    var isPlayedInterval = setInterval(function () {
        cw("isFinish isPlayedInterval connect");
        counter += 1;
        cw(`counter:${counter}`);
        if (player.getPlayerState() === 1) {
            cw("contiune if");
            clearInterval(isPlayedInterval);
            rushVideoControls();
        } else if (counter >= 7) {
            cw("contiune else if");
            clearInterval(isPlayedInterval);
            $('a:contains("Geç")')[0].click();
        } else {
            cw("contiune else");
        }
        cw("isFinish isPlayedInterval drop");
    }, 1000);
    cw("isFinish drop");
}
function rushVideoControls() {
    cw("rushVideoControls connect");
    Swal.fire = function () {
        window.location.href = linkHome;
    }
    var isVideoFinishInterval = setInterval(function () {
        cw("rushVideoControls isVideoFinishInterval connect");
        if (roundedPlayed >= length) {
            cw("contiune if");
            clearInterval(isVideoFinishInterval);
            window.location.href = linkHome;
        } else {
            cw("contiune else");
        }
        cw("rushVideoControls isVideoFinishInterval drop");
    }, 500);
    cw("rushVideoControls drop");
}
function playerOnError(data) {
    cw("playerOnError connect");
    $('a:contains("Geç")')[0].click();
    cw("playerOnError drop");
}
function goLogin() {
    cw("goLogin connect");
    window.location.href = linkLogin;
    cw("goLogin drop");
}
//});