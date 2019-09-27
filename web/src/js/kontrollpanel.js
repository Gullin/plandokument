// Testfunktion för spinner i modalt fönster över hela webbläsarfönstret
function modal() {
    $('.modal').modal('show');
    setTimeout(function () {
        $('.modal').modal('hide');
    }, 3000);
}


// Hanterar vilken panel som ska vara aktiv genom länk/URL
$(function () {
    // Javascript to enable link to tab
    var url = document.location.toString();
    if (url.match('#')) {
        $('.nav-tabs a[href="#' + url.split('#')[1] + '"]').tab('show');
    }

    // Change hash for page-reload
    $('.nav-tabs a').on('shown.bs.tab', function (e) {
        window.location.hash = e.target.hash;
    })
});