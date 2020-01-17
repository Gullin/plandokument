// kontrollerar vid page-load om cacher existerar och indikerar i så fall status med bild för resp. cache
$(document).ready(
    function () {

        CacheExistsPlanBasis();
        CacheExistsPlanDocuments();
        CacheExistsPlandocumenttypes();
        CacheExistsPlanBerorFastighet();
        CacheExistsPlanBerorPlan();

        $.ajax({
            type: "POST",
            url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheMeta',
            contentType: "application/json; charset=UTF-8",
            dataType: "json",
            success: function (msg) {

                var cacheMeta = JSON.parse(msg.d);

                var text = [];
                text[0] = "Antal applikations-cachar: " + cacheMeta.NumberOfApplicationCaches;
                text[1] = "Totalt antal cachar: " + cacheMeta.NumberOfTotalCaches;
                text[2] = "Tillgängligt minne för cachar: " + bytesToSize(cacheMeta.AvailableBytes);
                text[3] = "Outnyttjat minne för webbapplikation: " + cacheMeta.AvailablePhysicalMemory + " %";

                text.forEach(function (element, index) {
                    if (index != text.length) {
                        $('#CacheMeta').append(element + "<br />")
                    }
                    else {
                        $('#CacheMeta').append(element)
                    }
                });


            },
            error: function () {
                alert("FEL!");
            }
        })


    }
);



//#region Re-caching, alla och individuellt
// Cachar om alla
function RefreshCachePlanAll(element) {
    // Diablar knappen, tänder spinning och bytar ut text på knappen
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Loading...</span>");
    console.log("Alla");

    $.when(
        RefreshCachePlanBasis($('#btnRefreshCachePlan')),
        RefreshCachePlanDocuments($('#btnRefreshCachePlanDocuments')),
        RefreshCachePlandocumenttypes($('#btnRefreshCacheDocumenttypes')),
        RefreshCachePlanBerorFastighet($('#btnRefreshCachePlanBerorFastighet')),
        RefreshCachePlanBerorPlan($('#btnRefreshCachePlanBerorPlan'))).then(function () {
            // Ställer tillbaka knappen till ursprungsläget
            $spinner.addClass("spinner-hide");
            $spinner.next().remove();
            $spinner.after("<span> Förnya ALLA cacher</span>");
            $spinner.prop('disabled', false);
        });

}; // SLUT RefreshCachePlanAll



// Cachar om planernas grundinformation
function RefreshCachePlanBasis(element) {
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Loading...</span>");

    $("#btnRefreshCachePlan").parent().prev().removeClass("clear no");


    var t0 = new Date().getTime();
    return $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheRefreshPlanBasis',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data) {
                var t1 = new Date().getTime();
                setTimeout(function () {
                    $spinner.addClass("spinner-hide");
                    $spinner.next().remove();
                    $spinner.after("<span> Förnya cache</span>");
                    $spinner.prop('disabled', false);
                }, (t1 - t0 < Lkr.Plan.AjaxCalls.Delay) ? (Lkr.Plan.AjaxCalls.Delay - (t1 - t0)) : 0);
            }

        },
        complete: function () {
            CacheExistsPlanBasis();
        },
        error: function () {
            alert("Fel!\nRefreshCachePlanBasis");
        }
    })
}; // SLUT RefreshCachePlanBasis



// Cachar om plandokumenten
function RefreshCachePlanDocuments(element) {
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Loading...</span>");

    $("#btnRefreshCachePlanDocuments").parent().prev().removeClass("clear no");


    var t0 = new Date().getTime();
    return $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheRefreshPlanDocuments',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data) {
                var t1 = new Date().getTime();
                setTimeout(function () {
                    $spinner.addClass("spinner-hide");
                    $spinner.next().remove();
                    $spinner.after("<span> Förnya cache</span>");
                    $spinner.prop('disabled', false);
                }, (t1 - t0 < Lkr.Plan.AjaxCalls.Delay) ? (Lkr.Plan.AjaxCalls.Delay - (t1 - t0)) : 0);
            }
        },
        complete: function () {
            CacheExistsPlanDocuments();
        },
        error: function () {
            alert("Fel!\nRefreshCachePlanDocuments");
        }
    })
}; // SLUT RefreshCachePlanDocuments



// Cachar om dokumenttyperna
function RefreshCachePlandocumenttypes(element) {
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Loading...</span>");

    $("#btnRefreshCacheDocumenttypes").parent().prev().removeClass("clear no");


    var t0 = new Date().getTime();
    return $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheRefreshPlandocumenttypes',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data) {
                var t1 = new Date().getTime();
                setTimeout(function () {
                    $spinner.addClass("spinner-hide");
                    $spinner.next().remove();
                    $spinner.after("<span> Förnya cache</span>");
                    $spinner.prop('disabled', false);
                }, (t1 - t0 < Lkr.Plan.AjaxCalls.Delay) ? (Lkr.Plan.AjaxCalls.Delay - (t1 - t0)) : 0);
            }
        },
        complete: function () {
            CacheExistsPlandocumenttypes();
        },
        error: function () {
            alert("Fel!\nRefreshCachePlandocumenttypes");
        }
    })
}; // SLUT RefreshCachePlandocumenttypes



// Cachar om planernas berörskretsar till fastigheter
function RefreshCachePlanBerorFastighet(element) {
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Loading...</span>");

    $("#btnRefreshCachePlanBerorFastighet").parent().prev().removeClass("clear no");


    var t0 = new Date().getTime();
    return $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheRefreshPlanBerorFastighet',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data) {
                var t1 = new Date().getTime();
                setTimeout(function () {
                    $spinner.addClass("spinner-hide");
                    $spinner.next().remove();
                    $spinner.after("<span> Förnya cache</span>");
                    $spinner.prop('disabled', false);
                }, (t1 - t0 < Lkr.Plan.AjaxCalls.Delay) ? (Lkr.Plan.AjaxCalls.Delay - (t1 - t0)) : 0);
            }
        },
        complete: function () {
            CacheExistsPlanBerorFastighet();
        },
        error: function () {
            alert("Fel!\nRefreshCachePlanBerorFastighet");
        }
    })
}; // SLUT RefreshCachePlanBerorFastighet



// Cachar om planernas påverkan på varandra
function RefreshCachePlanBerorPlan(element) {
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Loading...</span>");

    $("#btnRefreshCachePlanBerorPlan").parent().prev().removeClass("clear no");


    var t0 = new Date().getTime();
    return $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheRefreshPlanBerorPlan',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data) {
                var t1 = new Date().getTime();
                setTimeout(function () {
                    $spinner.addClass("spinner-hide");
                    $spinner.next().remove();
                    $spinner.after("<span> Förnya cache</span>");
                    $spinner.prop('disabled', false);
                }, (t1 - t0 < Lkr.Plan.AjaxCalls.Delay) ? (Lkr.Plan.AjaxCalls.Delay - (t1 - t0)) : 0);
            }
        },
        complete: function () {
            CacheExistsPlanBerorPlan();
        },
        error: function () {
            alert("Fel!\nRefreshCachePlanBerorPlan");
        }
    })
}; // SLUT RefreshCachePlanBerorPlan
//#endregion


//#region Cache Existerar
// Kontrollera så att cache för planers grundinformation existerar
function CacheExistsPlanBasis() {
    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheExistsPlanBasis',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            var $td = $("#btnRefreshCachePlan").parent().prev();
            if (data == "true") {
                $td.addClass('clear');
                $td.attr('title', "Godkänt");
            }
            else {
                $td.addClass('no');
                $td.attr('title', "Underkänd");
            }
        },
        error: function () {
            //alert("Fel!\nRefreshCachePlanBerorFastighet");
        }
    })
} // SLUT CacheExistsPlanBasis



// Kontrollera så att cache för plandokument existerar
function CacheExistsPlanDocuments() {
    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheExistsPlanDocuments',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            var $td = $("#btnRefreshCachePlanDocuments").parent().prev();
            if (data == "true") {
                $td.addClass('clear');
                $td.attr('title', "Godkänt");
            }
            else {
                $td.addClass('no');
                $td.attr('title', "Underkänd");
            }
        },
        error: function () {
            //alert("Fel!\nRefreshCachePlanBerorFastighet");
        }
    })
} // SLUT CacheExistsPlanDocuments



// Kontrollerar så att cache för dokumenttyper existerar
function CacheExistsPlandocumenttypes() {
    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheExistsPlandocumenttypes',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            var $td = $("#btnRefreshCacheDocumenttypes").parent().prev();
            if (data == "true") {
                $td.addClass('clear');
                $td.attr('title', "Godkänt");
            }
            else {
                $td.addClass('no');
                $td.attr('title', "Underkänd");
            }
        },
        error: function () {
            //alert("Fel!\nRefreshCachePlanBerorFastighet");
        }
    })
}; // SLUT CacheExistsPlandocumenttypes


function CacheExistsPlanBerorFastighet() {
    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheExistsPlanBerorFastighet',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            var $td = $("#btnRefreshCachePlanBerorFastighet").parent().prev();
            if (data == "true") {
                $td.addClass('clear');
                $td.attr('title', "Godkänt");
            }
            else {
                $td.addClass('no');
                $td.attr('title', "Underkänd");
            }
        },
        error: function () {
            //alert("Fel!\nRefreshCachePlanBerorFastighet");
        }
    })
};


function CacheExistsPlanBerorPlan() {
    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheExistsPlanBerorPlan',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            var $td = $("#btnRefreshCachePlanBerorPlan").parent().prev();
            if (data == "true") {
                $td.addClass('clear');
                $td.attr('title', "Godkänt");
            }
            else {
                $td.addClass('no');
                $td.attr('title', "Underkänd");
            }
        },
        error: function () {
            //alert("Fel!\nRefreshCachePlanBerorFastighet");
        }
    })
};
//#endregion
