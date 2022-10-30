$(document).ready(
    function () {
        ChartStatRunningTotalRequestsByPeriod();
        ChartStatTotalRequestsByPeriod();
        ChartStatSearchtimeRequestsByPeriod();

        var $divPeriodButtons = $("<div>")
            .addClass("btn-group btn-group-sm")
            .attr({
                "role": "group",
                "aria-label": "Ändra diagramperiod"
            })
            .css("float","right");
        var $periodButtonTemplate = $("<button>")
            .addClass("btn btn-primary")
            .attr("type", "button");
        $divPeriodButtons.append(
            $periodButtonTemplate.clone()
                .attr("id","btnChartYear")
                .text("År")
                .addClass("active")
                .click(function () {
                    ChangeChartsPeriod(this);
                })
        );
        $divPeriodButtons.append(
            $periodButtonTemplate.clone()
                .attr("id", "btnChartMonth")
                .text("Månad")
                .click(function () {
                    ChangeChartsPeriod(this);
                })
        );
        $divPeriodButtons.append(
            $periodButtonTemplate.clone()
                .attr("id", "btnChartDay")
                .text("Dag")
                .click(function () {
                    ChangeChartsPeriod(this);
                })
        );
        $("#StatCharts").prepend($divPeriodButtons);
    }
);


function ChartStatRunningTotalRequestsByPeriod(period) {

    if (period == null) {
        period = "year"
    }

    var services = 'services/kontrollpanel.asmx/StatRunningTotalByYearRequests';

    switch (period) {
        case "year":
            services = services;
            break;
        case "month":
            services = 'services/kontrollpanel.asmx/StatRunningTotalByMonthRequests';
            break;
        case "day":
            services = 'services/kontrollpanel.asmx/StatRunningTotalByDayRequests';
            break;
    }

    var chartLabel = [];
    var chartData = [];

    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + services,
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {

            var data = JSON.parse(msg.d);

            for (i = 0; i < data.length; ++i) {
                chartLabel.push(data[i].period);
                chartData.push(data[i].total_running);
            }

        },
        error: function () {
            console.error("Fel!\nChartStatTotalRequestsByPeriod");
        },
        complete: function () {
            const data = {
                labels: chartLabel,
                datasets: [{
                    label: 'Förfrågningar ackumulerat',
                    lineTension: 0.4,
                    backgroundColor: 'rgba(54, 162, 235, 0.2)',
                    borderColor: 'rgb(54, 162, 235)',
                    data: chartData,
                    fill: true
                }]
            };

            const config = {
                type: 'line',
                data: data,
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            };


            var myChart = Chart.getChart("RunningTotalRequestsByYear");

            if (myChart != undefined) {
                myChart.destroy();
            }


            myChart = new Chart(
                    document.getElementById("RunningTotalRequestsByYear"),
                    config
                );
        }
    })


}; // SLUT ChartStatRunningTotalRequestsByPeriod

function ChartStatTotalRequestsByPeriod(period) {

    if (period == null) {
        period = "year"
    }

    var services = 'services/kontrollpanel.asmx/StatTotalByYearRequests';

    switch (period) {
        case "year":
            services = services;
            break;
        case "month":
            services = 'services/kontrollpanel.asmx/StatTotalByMonthRequests';
            break;
        case "day":
            services = 'services/kontrollpanel.asmx/StatTotalByDayRequests';
            break;
    }


    var chartLabel = [];
    var chartData = [];

    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + services,
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {

            var data = JSON.parse(msg.d);

            for (i = 0; i < data.length; ++i) {
                chartLabel.push(data[i].period);
                chartData.push(data[i].total);
            }

        },
        error: function () {
            console.error("Fel!\nChartStatTotalRequestsByPeriod");
        },
        complete: function () {
            const data = {
                labels: chartLabel,
                datasets: [{
                    label: 'Förfrågningar',
                    lineTension: 0.4,
                    backgroundColor: 'rgba(75, 192, 192, 0.2)',
                    borderColor: 'rgb(75, 192, 192)',
                    data: chartData,
                    fill: true
                }]
            };

            const config = {
                type: 'line',
                data: data,
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            };


            var myChart = Chart.getChart("TotalRequestsByYear");

            if (myChart != undefined) {
                myChart.destroy();
            }


            myChart = new Chart(
                document.getElementById("TotalRequestsByYear"),
                config
            );
        }
    })


}; // SLUT ChartStatTotalRequestsByPeriod

function ChartStatSearchtimeRequestsByPeriod(period) {

    if (period == null) {
        period = "year"
    }

    var services = 'services/kontrollpanel.asmx/StatSearchtimeByYearRequests';

    switch (period) {
        case "year":
            services = services;
            break;
        case "month":
            services = 'services/kontrollpanel.asmx/StatSearchtimeByMonthRequests';
            break;
        case "day":
            services = 'services/kontrollpanel.asmx/StatSearchtimeByDayRequests';
            break;
    }


    var chartLabel = [];
    var chartData = [];

    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + services,
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {

            var data = JSON.parse(msg.d);

            for (i = 0; i < data.length; ++i) {
                chartLabel.push(data[i].period);
                chartData.push(data[i].searchtime_ms_snitt);
            }

        },
        error: function () {
            console.error("Fel!\nChartStatSearchtimeRequestsByPeriod");
        },
        complete: function () {
            const data = {
                labels: chartLabel,
                datasets: [{
                    label: 'Genomsnittliga svarstider',
                    lineTension: 0.4,
                    backgroundColor: 'rgba(255, 99, 132, 0.2)',
                    borderColor: 'rgb(255, 99, 132)',
                    data: chartData,
                    fill: true
                }]
            };

            const config = {
                type: 'line',
                data: data,
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            };


            var myChart = Chart.getChart("SearchtimeRequestsByYear");

            if (myChart != undefined) {
                myChart.destroy();
            }


            myChart = new Chart(
                document.getElementById("SearchtimeRequestsByYear"),
                config
            );
        }
    })


}; // SLUT ChartStatSearchtimeRequestsByPeriod

function ChangeChartsPeriod(element) {

    var $pressedButton = $(element)
    $pressedButton.parent().children("button").removeClass("active");
    $pressedButton.addClass("active");

    if ($pressedButton.attr("id") == "btnChartDay") {
        ChartStatRunningTotalRequestsByPeriod("day");
        ChartStatTotalRequestsByPeriod("day");
        ChartStatSearchtimeRequestsByPeriod("day");
    }
    else if ($pressedButton.attr("id") == "btnChartMonth") {
        ChartStatRunningTotalRequestsByPeriod("month");
        ChartStatTotalRequestsByPeriod("month");
        ChartStatSearchtimeRequestsByPeriod("month");
    }
    else {
        ChartStatRunningTotalRequestsByPeriod("year");
        ChartStatTotalRequestsByPeriod("year");
        ChartStatSearchtimeRequestsByPeriod("year");
    }

}; // SLUT ChangeChartsPeriod
