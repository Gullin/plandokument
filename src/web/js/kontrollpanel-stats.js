let startDate;
let endDate;
let originalDataStatRunningTotalRequestsByPeriod = [];
let originalDataStatTotalRequestsByPeriod = [];
let originalDataStatSearchtimeRequestsByPeriod = [];

$(document).ready(

    function () {

        // #region knappar perioder år, månad och dag
        var $divPeriodButtons = $("<div>")
            .addClass("btn-group btn-group-sm")
            .attr({
                "role": "group",
                "aria-label": "Ändra diagramperiod"
            })
            .css("float", "right");
        var $periodButtonTemplate = $("<button>")
            .addClass("btn btn-primary")
            .attr("type", "button");
        $divPeriodButtons.append(
            $periodButtonTemplate.clone()
                .attr("id", "btnChartYear")
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
        // #endregion


        // #region datumintervall
        var $inputDate = $("<input>")
            .attr("type", "date")
            .addClass("form-control form-control-sm");
        var $divInputDateContainer = $("<div>")
            .addClass("col");
        var $divDateIntervall = $("<div>")
            .addClass("form-row");
        $divDateIntervall.append(
            $divInputDateContainer
                .clone()
                .append(
                    $inputDate.clone()
                        .attr({
                            "id": "startdate",
                            "placeholder": "Startdatum"
                        })
                )
        );
        $divDateIntervall.append(
            $divInputDateContainer
                .clone()
                .append(
                    $inputDate.clone()
                        .attr({
                            "id": "enddate",
                            "placeholder": "Slutdatum"
                        })
                )
        );
        // #endregion

        $("#StatCharts").prepend($divDateIntervall);
        $("#StatCharts").prepend($divPeriodButtons);

        
        initStartEndDate();
        
        ChartStatRunningTotalRequestsByPeriod();
        ChartStatTotalRequestsByPeriod();
        ChartStatSearchtimeRequestsByPeriod();

    }    
);    


function initStartEndDate() {
    
    let services = 'services/kontrollpanel.asmx/StatPeriodTotalRequests';
    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + services,
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            
            let data = JSON.parse(msg.d);

            startDate = data[0].date_first_request.slice(0, 10);
            endDate = data[0].date_latest_request.slice(0, 10);

        },    
        error: function () {
            console.error("Fel!\nStatPeriodTotalRequests");
        },    
        complete: function () {

            // Sätt datumfilterna till standardvärden
            document.getElementById('startdate').min = startDate;
            document.getElementById('startdate').max = endDate;
            document.getElementById('enddate').min = startDate;
            document.getElementById('enddate').max = endDate;
            document.getElementById('startdate').value = startDate;
            document.getElementById('enddate').value = endDate;
            
        
            // Lägg till event listeners för start- och slutdatum
            document.getElementById('startdate').addEventListener('input', filterDataByDate);
            document.getElementById('enddate').addEventListener('input', filterDataByDate);

        }    
    });    
}; // SLUT initStartEndDate    


function filterDataByDate() {
    let currentFromToDates;

    let activePeriodButton = $(".btn-group .btn.active").attr("id");

    switch (activePeriodButton) {
        case "btnChartYear":
            currentFromToDates = getCurrentFromToDates("year");
            break;
        case "btnChartMonth":
            currentFromToDates = getCurrentFromToDates("month");
            break;
        case "btnChartDay":
            currentFromToDates = getCurrentFromToDates("day");
            break;
    }

    console.log("startDate", startDate);
    console.log("endDate", endDate);

    // Filtrera endast om båda datum är angivna
    if (startDate && endDate) {

        console.log("ÄNDRA DIAGRAM");
        console.log("DATA RunningTotalRequests", originalDataStatRunningTotalRequestsByPeriod);
        console.log("DATA TotalRequests", originalDataStatTotalRequestsByPeriod);
        console.log("DATA SearchtimeRequests", originalDataStatSearchtimeRequestsByPeriod);


        let chartRunningTotalRequestsByYear = Chart.getChart("RunningTotalRequestsByYear");
        let chartTotalRequestsByYear = Chart.getChart("TotalRequestsByYear");
        let chartSearchtimeRequestsByYear = Chart.getChart("SearchtimeRequestsByYear");

        // Filtrera dataset baserat på valt datumintervall  
        const filteredDataStatRunningTotalRequestsByPeriod = originalDataStatRunningTotalRequestsByPeriod.filter(item => {
            return item.period >= currentFromToDates.fromDate && item.period <= currentFromToDates.toDate;
        });

        // Uppdatera labels och data
        const filteredLabelsStatRunningTotalRequestsByPeriod = filteredDataStatRunningTotalRequestsByPeriod.map(item => item.period);
        const filteredValuesStatRunningTotalRequestsByPeriod = filteredDataStatRunningTotalRequestsByPeriod.map(item => item.total_running);

        // Uppdatera Chart.js dataset och labels
        chartRunningTotalRequestsByYear.data.labels = filteredLabelsStatRunningTotalRequestsByPeriod;
        chartRunningTotalRequestsByYear.data.datasets[0].data = filteredValuesStatRunningTotalRequestsByPeriod;
        chartRunningTotalRequestsByYear.update();



        // Filtrera dataset baserat på valt datumintervall  
        const filteredDataStatTotalRequestsByPeriod = originalDataStatTotalRequestsByPeriod.filter(item => {
            return item.period >= currentFromToDates.fromDate && item.period <= currentFromToDates.toDate;
        });

        // Uppdatera labels och data
        const filteredLabelsStatTotalRequestsByPeriod = filteredDataStatTotalRequestsByPeriod.map(item => item.period);
        const filteredValuesStatTotalRequestsByPeriod = filteredDataStatTotalRequestsByPeriod.map(item => item.total);

        // Uppdatera Chart.js dataset och labels
        chartTotalRequestsByYear.data.labels = filteredLabelsStatTotalRequestsByPeriod;
        chartTotalRequestsByYear.data.datasets[0].data = filteredValuesStatTotalRequestsByPeriod;
        chartTotalRequestsByYear.update();
        


        // Filtrera dataset baserat på valt datumintervall  
        const filteredDataStatSearchtimeRequestsByPeriod = originalDataStatSearchtimeRequestsByPeriod.filter(item => {
            return item.period >= currentFromToDates.fromDate && item.period <= currentFromToDates.toDate;
        });

        // Uppdatera labels och data
        const filteredLabelsStatSearchtimeRequestsByPeriod = filteredDataStatSearchtimeRequestsByPeriod.map(item => item.period);
        const filteredValuesStatSearchtimeRequestsByPeriod = filteredDataStatSearchtimeRequestsByPeriod.map(item => item.searchtime_ms_snitt);

        // Uppdatera Chart.js dataset och labels
        chartSearchtimeRequestsByYear.data.labels = filteredLabelsStatSearchtimeRequestsByPeriod;
        chartSearchtimeRequestsByYear.data.datasets[0].data = filteredValuesStatSearchtimeRequestsByPeriod;
        chartSearchtimeRequestsByYear.update();


    }  
  }; // SLUT filterDataByDate  





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
            originalDataStatRunningTotalRequestsByPeriod = data;

            const currentFromToDates = getCurrentFromToDates(period);

            // Filtrera dataset baserat på valt datumintervall  
            const filteredData = data.filter(item => {
                return item.period >= currentFromToDates.fromDate && item.period <= currentFromToDates.toDate;
            });

            for (i = 0; i < filteredData.length; ++i) {
                chartLabel.push(filteredData[i].period);
                chartData.push(filteredData[i].total_running);
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
                        x: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Tid'
                            }
                        },
                        y: {
                            beginAtZero: true,
                            display: true,
                            title: {
                                display: true,
                                text: 'Requests'
                            }
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
            originalDataStatTotalRequestsByPeriod = data;

            const currentFromToDates = getCurrentFromToDates(period);

            // Filtrera dataset baserat på valt datumintervall  
            const filteredData = data.filter(item => {
                return item.period >= currentFromToDates.fromDate && item.period <= currentFromToDates.toDate;
            });

            for (i = 0; i < filteredData.length; ++i) {
                chartLabel.push(filteredData[i].period);
                chartData.push(filteredData[i].total);
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
                        x: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Tid'
                            }
                        },
                        y: {
                            beginAtZero: true,
                            display: true,
                            title: {
                                display: true,
                                text: 'Requests'
                            }
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
            originalDataStatSearchtimeRequestsByPeriod = data;

            const currentFromToDates = getCurrentFromToDates(period);

            // Filtrera dataset baserat på valt datumintervall  
            const filteredData = data.filter(item => {
                return item.period >= currentFromToDates.fromDate && item.period <= currentFromToDates.toDate;
            });

            for (i = 0; i < filteredData.length; ++i) {
                chartLabel.push(filteredData[i].period);
                chartData.push(filteredData[i].searchtime_ms_snitt);
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
                        x: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Tid'
                            }
                        },
                        y: {
                            beginAtZero: true,
                            display: true,
                            title: {
                                display: true,
                                text: 'Millisekunder'
                            }
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

function getCurrentFromToDates(period) {
    switch (period) {
        case "year":
            return {
                fromDate: document.getElementById('startdate').value.slice(0,4),
                toDate: document.getElementById('enddate').value.slice(0,4)
            }
        case "month":
            return {
                fromDate: document.getElementById('startdate').value.slice(0,7),
                toDate: document.getElementById('enddate').value.slice(0,7)
            }
        case "day":
            return {
                fromDate: document.getElementById('startdate').value,
                toDate: document.getElementById('enddate').value
            }
    }
}