var timer = 2000;
var latestFileSize = 0;
var latestTotalRowCount = 0;


function GetLogPeriodically(timer) {

    console.log("INNE");
    console.log("timer", timer);
    console.log("tab-log", $('#tab-logs').hasClass('active'));
    console.log("tab-log-audit", $('#tab-log-audit').hasClass('active'));

    setTimeout(
        function () {
            if ($('#tab-logs').hasClass('active') && $('#tab-log-audit').hasClass('active')) {
                console.log("Hämtar logginformation igen");
                console.log("Antal aktuella rader: ", latestTotalRowCount);
                GetLog(
                    latestTotalRowCount
                );
            }
            else {
                console.log("Slutar hämta logginformation");
            }
        },
        timer
    );

}; // SLUT GetLogPeriodically



function GetLog(nbrOfRows) {
    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/GetLog',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        data: "{latestFileSize: '10', latestRow: '" + nbrOfRows + "'}",
        success: function (msg) {

            var data = JSON.parse(msg.d);
            if (data.NewLineCounts > 0) {
                $auditTextLogElmnt = $('#textarea-log-audit');

                // Töm loggfältet vid första hämtningen
                if (latestTotalRowCount == 0) {
                    $auditTextLogElmnt.text("");
                }

                let regexWarning = /([2-3][0-9]{3})-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1]) ([0-1][0-9]|2[0-4]):([0-5][0-9]):([0-5][0-9])(\|   WARN\|)[^+]?/;
                let regexError = /([2-3][0-9]{3})-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1]) ([0-1][0-9]|2[0-4]):([0-5][0-9]):([0-5][0-9])(\|  ERROR\|)[^+]?/;
                // Dela textsträng efter javascript-ny-rad och filtrera bort tomma textsträngar
                var logRows = data.Content.split('\n').filter(function (el) {
                    return el != "";
                });
                var i;
                var logMessages = '';
                for (i = 0; i < logRows.length; i++) {
                    var isWarn = regexWarning.exec(logRows[i]);
                    var isError = regexError.exec(logRows[i]);
                    if (isWarn) {
                        logMessages += "<span style='color:orange'>" + logRows[i] + "</span><br/>";
                    }
                    else if (isError) {
                        logMessages += "<span style='color:red'>" + logRows[i] + "</span><br/>";
                    }
                    else {
                        logMessages += logRows[i] + "<br/>";
                    }

                }
                //console.log("Loggelement", $auditTextLogElmnt.html());
                $auditTextLogElmnt.html($auditTextLogElmnt.html() + logMessages);
                $auditTextLogElmnt.scrollTop(document.getElementById('textarea-log-audit').scrollHeight);
            }
            latestFileSize = data.FileSize;
            latestTotalRowCount = data.FileLineCounts;
            console.log("Antal rader: ", data.FileLineCounts);
            console.log("Antal NYA rader: ", data.NewLineCounts);
            //console.log("Innehåll: ", logRows);
            console.log("scrollhöjd: ", document.getElementById('textarea-log-audit').scrollHeight);

        },
        error: function () {
            console.error("Fel!\nGetLog");
        },
        complete: function () {
            GetLogPeriodically(timer);
        },
        timeout: 0
    })
}; // SLUT GetLog
