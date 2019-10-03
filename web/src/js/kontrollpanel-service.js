function Test() {
    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/ThumnailServiceExists',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            console.log(data);
            if (data) {
                alert(data);
            }

        },
        error: function () {
            alert("Fel!\nRefreshCachePlanBasis");
        }
    })
}; // SLUT RefreshCachePlanBasis