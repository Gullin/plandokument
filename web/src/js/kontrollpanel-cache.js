// Cachar om planernas grundinformation
function RefreshCachePlanAll(element) {
    console.log("id",$(element).attr("id"));
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Loading...</span>");

    $.when(RefreshCachePlanBasis($('#btnRefreshCachePlan')),
        RefreshCachePlandocumenttypes($('#btnRefreshCacheDocumenttypes')),
        RefreshCachePlanBerorFastighet($('#btnRefreshCachePlanBerorFastighet')),
        RefreshCachePlanBerorPlan($('#btnRefreshCachePlanBerorPlan'))).then(function () {
            $spinner.addClass("spinner-hide");
            $spinner.next().remove();
            $spinner.after("<span> Förnya ALLA cacher</span>");
            $spinner.prop('disabled', false);
        });

}; // SLUT RefreshCachePlanAll

function RefreshCachePlanAllExecute(callback) {
    RefreshCachePlanBasis($('#btnRefreshCachePlan'));
    RefreshCachePlandocumenttypes($('#btnRefreshCacheDocumenttypes'));
    RefreshCachePlanBerorFastighet($('#btnRefreshCachePlanBerorFastighet'));
    RefreshCachePlanBerorPlan($('#btnRefreshCachePlanBerorPlan'));
    callback();
}



// Cachar om planernas grundinformation
function RefreshCachePlanBasis(element) {
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Loading...</span>");

    $("#btnRefreshCachePlan").parent().prev().removeClass("clear no");


    return $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheRefreshPlanBasis',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data = "true") {
                $spinner.addClass("spinner-hide");
                $spinner.next().remove();
                $spinner.after("<span> Förnya cache</span>");
                $spinner.prop('disabled', false);
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



// Cachar om planernas grundinformation
function RefreshCachePlandocumenttypes(element) {
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Loading...</span>");

    $("#btnRefreshCacheDocumenttypes").parent().prev().removeClass("clear no");


    return $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheRefreshPlandocumenttypes',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data = "true") {
                $spinner.addClass("spinner-hide");
                $spinner.next().remove();
                $spinner.after("<span> Förnya cache</span>");
                $spinner.prop('disabled', false);
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



// Cachar om planernas grundinformation
function RefreshCachePlanBerorFastighet(element) {
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Loading...</span>");

    $("#btnRefreshCachePlanBerorFastighet").parent().prev().removeClass("clear no");


    return $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheRefreshPlanBerorFastighet',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data = "true") {
                $spinner.addClass("spinner-hide");
                $spinner.next().remove();
                $spinner.after("<span> Förnya cache</span>");
                $spinner.prop('disabled', false);
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



// Cachar om planernas grundinformation
function RefreshCachePlanBerorPlan(element) {
    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Loading...</span>");

    $("#btnRefreshCachePlanBerorPlan").parent().prev().removeClass("clear no");


    return $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheRefreshPlanBerorPlan',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data = "true") {
                $spinner.addClass("spinner-hide");
                $spinner.next().remove();
                $spinner.after("<span> Förnya cache</span>");
                $spinner.prop('disabled', false);
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
}


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
};


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

$(document).ready(
    function () {

        CacheExistsPlanBasis();
        CacheExistsPlandocumenttypes();
        CacheExistsPlanBerorFastighet();
        CacheExistsPlanBerorPlan();

    }
);