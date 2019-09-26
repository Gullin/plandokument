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

    //RefreshCachePlanAllExecute(function () {
    //    $spinner.addClass("spinner-hide");
    //    $spinner.next().remove();
    //    $spinner.after("<span> Förnya ALLA cacher</span>");
    //    $spinner.prop('disabled', false);
    //});

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
        error: function () {
            alert("Fel!\nRefreshCachePlanBerorPlan");
        }
    })
}; // SLUT RefreshCachePlanBerorPlan



$(document).ready(
    function () {

        $.ajax({
            type: "POST",
            url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CacheExistsPlanBasis',
            contentType: "application/json; charset=UTF-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;
                if (data = "true") {
                    $("#btnRefreshCachePlan").parent().prev();
                }
            },
            error: function () {
                //alert("Fel!\nRefreshCachePlanBerorFastighet");
            }
        })

    }
);