﻿// Avrundar byte och sätter suffix för storlek (Bytes, kB, ...)
function bytesToSize(bytes) {
    var sizes = ['Bytes', 'kB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB']; // k=kilo, M=Mega, G=Giga, T=Tera, P=Peta, E=Exa, Z=Zetta, Y=Yotta
    if (bytes == 0) return 'ingen filstorlek';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
}; // SLUT bytesToSize


// Rörlig gif-bild
function toggleLoadingImage(on) {

    if (!document.getElementById("LoadingImgObj")) {
        var divCover = $('<div>');
        divCover.attr('id', 'LoadingImgObj');
        divCover.css({
            'background-color': 'rgb(255,255,255)',
            'opacity': '0.5',
            'filter': 'alpha(opacity=50)',
            'position': 'absolute',
            'top': '0',
            'left': '0',
            'z-index': '100',
            'text-align': 'center',
            'display': 'none'
        });

        var $divLoadingImg = $('<div>');
        $divLoadingImg.css('z-index', '200');
        var aLoadingImg = $('<img>');
        aLoadingImg.attr('src', urlBasePath + 'pic/animated_windows8_64.GIF');
        textBottom = "<br /><br />Vänta...";
        $divLoadingImg.append(aLoadingImg);
        $divLoadingImg.append(textBottom);

        divCover.append($divLoadingImg);
        $('body').append(divCover);
    }

    var objLoadingImg = $('#LoadingImgObj');
    if (on) {
        var windowHeight = $(window).height();
        var documentHeight = $(document).height();
        var windowWidth = $(window).width();

        objLoadingImg.css({
            'height': documentHeight,
            'width': windowWidth,
            'display': 'block'
        });
    } else {
        objLoadingImg.css({
            'display': 'none'
        });
    }

} // SLUT toggleLoadingImage