



var username = '${{username}}';
var password = '${{password}}';
var linkHome = 'https://www.vidoix.com/';
var linkLogin = 'https://www.vidoix.com/login';
var isNotSetQuality = true;
function timeOut10(fn) {
    setTimeout(fn(), 10 * 1000);
}
function timeOut(fn, seconds) {
    setTimeout(fn(), seconds * 1000);
}
function setFunctions() {
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
}
function inithome() {
    setFunctions();
    whereAmI();
}
function whereAmI() {
    switch (window.location.href) {
        case linkHome:
            if ($('a[href="/logout"]').length > 0 && $('#filterCoins').length > 0 && $('#loadMoreVideo').length > 0) {
                mainJS();
            } else {
                homeJS();
            }
            break;
        case linkLogin:
            loginJS();
            break;
        default:
            if (window.location.href.indexOf("youtube") > -1) {
                videoJS();
            } else if (window.location.href.indexOf("dashboard") > -1) {
                mainJS();
            }
            break;
    }
}
function homeJS() {
    window.location.href = linkLogin;
}
function loginJS() {
    $('#rememberme').click();
    $('#username').val(username);
    $('#password').val(password);
    $('#js-login button').click();
}
function mainJS() {
    var counter = 0;
    var loadMoreVideoInterval = setInterval(function () {
        counter += 1;
        if ($('#loadMoreVideo *').length > 0) {
            clearInterval(loadMoreVideoInterval);
            selectVideo();
        } else if (counter >= 7) {
            clearInterval(loadMoreVideoInterval);
            selectVideo();
        }
    }, 1000);
}
function selectVideo() {
    if ($('#loadMoreVideo > div > a:first').length > 0) {
        window.location.href = $('#loadMoreVideo > div > a:first').attr('href');
    } else {
        if ($('#filterSecond > i').length > 0) {
            timeOut10(function () {
                $('#filterCoins').click();
            });
        } else if ($('#filterCoins > i').length > 0) {
            timeOut10(function () {
                $('#filterSecond').click();
            });
        } else {
            $('#filterCoins').click();
        }
    }
}
function videoJS() {
    $(function () {
        Swal.fire = function () {
            window.location.href = linkHome;
        }
    });
    var isPlayerSetInterval = setInterval(function () {
        if (player !== undefined) {
            player.addEventListener('onReady', 'onReadyQwerty');
            player.addEventListener('onError', 'OnErrorQwerty');
            player.addEventListener('onStateChange', 'onStateChangeQwerty');
            clearInterval(isPlayerSetInterval);
        }
    }, 250);
}
function OnErrorQwerty(event) {
    $('a:contains("Geç")')[0].click();
}
function onReadyQwerty(event) {
    player.mute();
    player.playVideo();
    playing = true;
    isFinish();
}
function onStateChangeQwerty(event) {
    if (event.data == YT.PlayerState.ENDED) {
        window.location.href = linkHome;
    } else if (event.data == YT.PlayerState.PLAYING && isNotSetQuality) {
        isNotSetQuality = false;
        try {
            window.frames["ytPlayer"].contentWindow.postMessage('setQuality', '*');
        } catch (e) {

        }
    }
}
function isFinish() {
    var isVideoFinishInterval = setInterval(function () {
        if (roundedPlayed >= length) {
            clearInterval(isVideoFinishInterval);
            window.location.href = linkHome;
        } else {
        }
    }, 500);
}
inithome();


