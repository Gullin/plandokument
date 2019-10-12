function isRunning() {
    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/ThumnailServiceIsRunning',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            console.log(data);
            if (data) {
                $("#status").text("Ja");
            }
            else {
                $("#status").text("Nej");
            }

        },
        error: function () {
            alert("Fel!\nisRunning");
        }
    })
}; // SLUT isRunning



$(document).ready(
    function () {

        $.ajax({
            type: "POST",
            url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/ThumnailServiceExists',
            contentType: "application/json; charset=UTF-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;
                if (data) {
                    $("#ThumnailsContent").show();
                    isRunning();
                    createServiceDiv();
                }
                else {
                    warning();
                }

            },
            error: function () {
                alert("Fel!\nKontroll av om Service existerar");
            }
        })

        function warning() {
            $divWarningNoService = $("<div>");
            $divWarningNoService.addClass("alert alert-warning");
            $divWarningNoService.attr("role", "alert");
            $divWarningNoService.text("Tjänsten med namn enligt webbapplikationens konfigurationsinställningar existerar ej på servern");
            $("#thumnails").append($divWarningNoService);
        }

        function createServiceDiv() {
            $.ajax({
                type: "POST",
                url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/ServiceMeta',
                contentType: "application/json; charset=UTF-8",
                dataType: "json",
                success: function (msg) {
                    var serviceMeta = JSON.parse(msg.d);

                    var text = [];
                    text[0] = "Namn: " + serviceMeta.ServiceName;
                    text[1] = "Visningsnamn: " + serviceMeta.ServiceDisplayName;
                    text[2] = "Beskrivning: " + serviceMeta.ServiceDescription;

                    text.forEach(function (element, index) {
                        if (index != text.length) {
                            $('#ServiceMeta').append(element + "<br />")
                        }
                        else {
                            $('#ServiceMeta').append(element)
                        }
                    });

                },
                error: function () {
                    alert("Fel!\ncreateServiceDiv");
                }
            })
        }

    }
);



// Startar om Windows-tjänst
function ServiceRestart(element) {
    $("#status").text("N/A");
    $('#btnServiceStart').prop('disabled', true);
    $('#btnServiceStop').prop('disabled', true);
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Startar om...</span>");

    return $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/ThumnailRebootService',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data == "true") {
                alert("Tjänst omstartad");
                $("#status").text("Ja");
            }
            else {
                alert("Något gick fel vid försök till omstart av tjänst");
            }

        },
        complete: function () {
            $spinner.addClass("spinner-hide");
            $spinner.next().remove();
            $spinner.after("<span> Starta om tjänst</span>");
            $spinner.prop('disabled', false);

            $('#btnServiceStart').prop('disabled', false);
            $('#btnServiceStop').prop('disabled', false);
        },
        error: function () {
            alert("Fel!\nServiceRestart");
        }
    })
}; // SLUT ServiceRestart

// Startar Windows-tjänst
function ServiceStart(element) {
    $("#status").text("N/A");
    $('#btnServiceRestart').prop('disabled', true);
    $('#btnServiceStop').prop('disabled', true);
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Startar...</span>");

    return $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/ThumnailStartService',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data == "true") {
                alert("Tjänst startad");
                $("#status").text("Ja");
            }
            else {
                alert("Något gick fel vid försök till start av tjänst");
            }

        },
        complete: function () {
            $spinner.addClass("spinner-hide");
            $spinner.next().remove();
            $spinner.after("<span> Starta tjänst</span>");
            $spinner.prop('disabled', false);

            $('#btnServiceRestart').prop('disabled', false);
            $('#btnServiceStop').prop('disabled', false);
        },
        error: function () {
            alert("Fel!\nServiceStart");
        }
    })
}; // SLUT ServiceRestart

// Stoppar Windows-tjänst
function ServiceStop(element) {
    $("#status").text("N/A");
    $('#btnServiceRestart').prop('disabled', true);
    $('#btnServiceStart').prop('disabled', true);
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Stoppar...</span>");

    return $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/ThumnailStopService',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data == "true") {
                alert("Tjänst stoppad");
                $("#status").text("Nej");
            }
            else {
                alert("Något gick fel vid försök till stopp av tjänst");
            }

        },
        complete: function () {
            $spinner.addClass("spinner-hide");
            $spinner.next().remove();
            $spinner.after("<span> Stoppa tjänst</span>");
            $spinner.prop('disabled', false);

            $('#btnServiceRestart').prop('disabled', false);
            $('#btnServiceStart').prop('disabled', false);
        },
        error: function () {
            alert("Fel!\nServiceStop");
        }
    })
}; // SLUT ServiceRestart