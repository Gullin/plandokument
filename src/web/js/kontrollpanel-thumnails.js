﻿var urlBasePath = Lkr.Plan.Dokument.resolvedClientUrl;

function getAllPlansDocs() {

    $.ajax({
        type: "POST",
        url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/GetPlansDocsAll',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {

            if (msg.d != '') {

                $("#ThumnailsContent").empty();

                var $divRow = $("<div>").addClass("row");
                var $divColLeft = $("<div>").addClass("col-auto");
                var $divColRight = $divColLeft.clone();


                // Funktionsknappar
                // Mall definierad
                var $button = $("<button>")
                    .addClass("btn btn-primary btn-sm")
                    .attr("type", "button")
                    .append(
                        $("<span>")
                            .addClass("spinner-border spinner-border-sm spinner-hide")
                            .attr("role", "status")
                            .attr("aria-hidden", "true"),
                        $("<span>")
                    );

                var $btnReCreateThumnail = $button.clone();
                $btnReCreateThumnail
                    .attr("id", "btnReCreateThumnail")
                    .click(function () {
                        ReCreateThumnails(this);
                    })
                    .find("span:nth-child(2)")
                    .text("Skapa om miniatyrbilder för markerade plankarter");

                var $btnReloadThumnail = $button.clone();
                $btnReloadThumnail
                    .attr("id", "btnReloadThumnail")
                    .click(function () {
                        ReloadThumnails(this);
                    })
                    .find("span:nth-child(2)")
                    .text("Uppdatera tabell");



                $("#ThumnailsContent").append("<hr />");
                $("#ThumnailsContent").append("<h3>Befintliga plankartornas tiff-bilder, status miniatyrbilder</h3>");


                var $spanThumnailHave = $("<span>");
                $spanThumnailHave.addClass("clear");
                $spanThumnailHave.attr("title","Miniatyrbild existerar");
                var $spanThumnailNo = $("<span>");
                $spanThumnailNo.addClass("no");
                $spanThumnailNo.attr("title","Miniatyrbild kunde inte hittas");
                var $spanThumnailPeek = $("<span>");
                $spanThumnailPeek.addClass("peek");
                $spanThumnailPeek.attr("title", "Miniatyrbild");



                var $divColRightButtond = $("<div>");
                $divColRightButtond.addClass("sticky-top");

                $divColRightButtond.append($btnReCreateThumnail);
                $divColRightButtond.append("<br />");
                $divColRightButtond.append($btnReloadThumnail);
                $divColRightButtond.append("<br />");

                $divColRightButtond.append("existerande miniatyrbild")
                    .append($spanThumnailHave
                        .clone()
                        .css("padding-left", "1em")
                    );
                $divColRightButtond.append("<br />");
                $divColRightButtond.append(" avsaknad av miniatyrbild")
                    .append($spanThumnailNo
                        .clone()
                        .css("padding-left", "1em")
                    );

                $divColRight.append($divColRightButtond);



                // Table Head
                var $table = $("<table id='table-thumnails'>");
                $table.addClass("table table-hover table-responsive table-sm");
                var $tableHeader = $("<thead>");
                $tableHeader.addClass("thead-light");
                var $tr = $("<tr>");
                // Kolumn 1
                var $th0 = $("<th>");
                $th0.append("<input type='checkbox'>");
                $th0.addClass("center");
                $th0.attr("title", "Markera alla bildfiler");
                // Klick-event, kryssruta i tabellrubrik för kryssning av alla bildposter
                $th0.children("input").change(function () {
                    $thisCheckbox = $(this);
                    var status = this.checked;
                    $('#table-thumnails tbody input').prop('checked', status);
                    if (status) {
                        $thisCheckbox.prop('title', 'Avmarkera alla dokument');
                        $('#table-thumnails tbody input').closest("tr").addClass("tr-selected");
                        $('#table-thumnails tbody input').prop('checked', status)
                    } else {
                        $thisCheckbox.prop('title', 'Markera alla dokument');
                        $('#table-thumnails tbody input').closest("tr").removeClass("tr-selected");
                    }

                    SetTextReCreateThumbnailsButton(
                        $("#table-thumnails").find("tbody").find(':checkbox:checked').length
                    );

                });
                // Kolumn 2
                var $th1 = $("<th>");
                $th1.text("Tiff-filnamn");
                // Kolumn 3
                var $th2 = $("<th>");
                $th2.text("Stor");
                // Kolumn 4
                var $th3 = $("<th>");
                $th3.text("Liten");
                $tr.append($th0);
                $tr.append($th1);
                $tr.append($th2);
                $tr.append($th3);
                // Kolumn 5
                var $th3_5 = $("<th>");
                $tr.append($th3_5);
                // Kolumn 6
                var $th4 = $("<th>");
                $tr.append($th4);
                $tableHeader.append($tr);
                $table.append($tableHeader);


                // Table Body
                var $tableBody = $("<tbody>");
                var anyThumnailExists = false;
                $.each(eval(msg.d), function (key, val) {
                    if (val.EXTENTION.toLowerCase() == ".tif") {
                        var $tr = $("<tr>");
                        $tr.addClass("hand");

                        var $td0 = $("<td>");
                        $td0.append("<input type='checkbox'>");
                        $td0.addClass("center");
                        $td0.attr("id", val.PLAN_ID);
                        $td0.attr("cg-clickable", "true");

                        var $td1 = $("<td>");
                        $td1.text(val.PATH + val.NAME);
                        $td1.attr("cg-clickable", "true");

                        var $td2 = $("<td>");
                        $td2.addClass("center");
                        $td2.attr("cg-clickable", "true");
                        if (val.THUMNAILINDICATION.includes("l")) {
                            $td2.append($spanThumnailHave.clone());
                        }
                        else {
                            $td2.append($spanThumnailNo.clone());
                        }

                        var $td3 = $("<td>");
                        $td3.addClass("center");
                        $td3.attr("cg-clickable", "true");
                        if (val.THUMNAILINDICATION.includes("s")) {
                            $td3.append($spanThumnailHave.clone());
                        }
                        else {
                            $td3.append($spanThumnailNo.clone());
                        }

                        var $td3_5 = $("<td>");
                        $td3_5.addClass("center");

                        var $td4 = $("<td>");
                        $td4.addClass("center");
                        if (val.THUMNAILINDICATION.includes("l") || val.THUMNAILINDICATION.includes("s")) {
                            anyThumnailExists = true;
                            var $aDelete = $("<a>").click(function () {
                                DeleteThumnail(this);
                            }).css({
                                "text-decoration": "underline",
                                "color": "blue"
                            }).text("radera");

                            var $peek = $spanThumnailPeek.clone().popover({
                                trigger: 'manual',
                                placement: 'auto',
                                title: val.NAME + ' innehåller',
                                html: true,
                                content:
                                    '<div><img src="' + urlBasePath + val.THUMNAILPATH + val.NAME.replace(/\.[^/.]+$/, "") + '_thumnail-s.jpg" /><span class="popover-content-big-image" title="Större bild"></span>' + '</div>'
                            }).on('mouseenter', function () {
                                var _this = this;
                                $(this).popover('show');
                                $('.popover').on('mouseleave', function () {
                                    $(_this).popover('hide');
                                });
                                $('.popover').find('span').on('click', function () {

                                    $('<div class="modal" tabindex="-1"><div class="modal-dialog modal-xl modal-wide modal-dialog-centered" style="min-width: 90%"><div class="modal-content"><img style="width:100%;" src="' + urlBasePath + val.THUMNAILPATH + val.NAME.replace(/\.[^/.]+$/, "") + '_thumnail-l.jpg" /></div></div></div>')
                                        .modal({
                                            keyboard: true
                                        });

                                    $(this).closest('.popover').popover('hide');
                                });
                            }).on('mouseleave', function () {
                                var _this = this;
                                setTimeout(function () {
                                    if (!$('.popover:hover').length) {
                                        $(_this).popover('hide');
                                    }
                                }, 200);
                            });

                            $td3_5.append($peek);

                            $td4.append($($aDelete));
                        }
                        else {
                            $td3_5.attr("cg-clickable", "true");
                            $td3_5;//.css("display", "block").html("&nbsp;")
                            $td4.attr("cg-clickable", "true");
                            $td4;
                        }

                        $tr.append($td0);
                        $tr.append($td1);
                        $tr.append($td2);
                        $tr.append($td3);
                        $tr.append($td3_5);
                        $tr.append($td4);

                        // Gör hela raden klickbar och omställer av kryssruta
                        //$tr.children("td").not(":last-child").click(function (event) {
                        $tr.children("td[cg-clickable='true']").click(function (event) {
                            if (event.target.type !== 'checkbox') {
                                //$(':checkbox', this).trigger('click');
                                $clickedRow = $(this).parent("tr");
                                $clickedRow.find(':checkbox').trigger('click');
                                if ($clickedRow.find(':checkbox').is(":checked")) {
                                    $clickedRow.addClass("tr-selected");
                                }
                                else {
                                    $clickedRow.removeClass("tr-selected");
                                }
                            }

                            SetTextReCreateThumbnailsButton(
                                $(this).closest("tbody").find(':checkbox:checked').length
                            );

                        });

                        $tableBody.append($tr);
                    }
                });
                if (anyThumnailExists) {
                //    $tableBody.find("th:nth-child(5)").css("display", "block");
                //    $tableBody.find("td:nth-child(5)").css("display", "block");
                }
                $table.append($tableBody);
                $divColLeft.append($table);
                $divRow.append($divColLeft);
                $divRow.append($divColRight);
                $("#ThumnailsContent").append($divRow);

                $("#ThumnailsContent").show();
            }
            else {
                $("#ThumnailsContent").text("Inga plankartor i tiff-format kunde hittas");
                $("#ThumnailsContent").show();
            }

        },
        error: function () {
            alert("FEL!");
        }
    })

}

function SetTextReCreateThumbnailsButton(nbrChecked) {
    var thumbnailButtonText = $("#btnReCreateThumnail").find("span:nth-child(2)").text();
    if (nbrChecked == 1) {
        $("#btnReCreateThumnail")
            .find("span:nth-child(2)")
            .text("Skapa om " + nbrChecked + " st. miniatyrbild");
    }
    else if (nbrChecked >= 1) {
        $("#btnReCreateThumnail")
            .find("span:nth-child(2)")
            .text("Skapa om " + nbrChecked + " st. miniatyrbilder");
    }
    else {
        $("#btnReCreateThumnail")
            .find("span:nth-child(2)")
            .text("Skapa om miniatyrbilder för markerade plankarter");
    }

}


function ReCreateThumnails(element) {

    var $spinner = $(element).children("span");
    $spinner.prop('disabled', true);
    $spinner.removeClass("spinner-hide");
    $spinner.next().remove();
    $spinner.after("<span> Skapar om miniatyrbilder...</span>");

    // Hämtar alla sökvägar från tabellcell till Array
    var planImages = GetCheckedPlansInTable();


    if (typeof planImages[0] == 'undefined') {
        $spinner.addClass("spinner-hide");
        $spinner.next().remove();
        $spinner.after("<span>Skapa om miniatyrbilder för markerade plankarter</span>");
        $spinner.prop('disabled', false);

        alert("Inga plankartor valda");

        return;
    }


    if (planImages.length > 0) {

        return $.ajax({
            type: "POST",
            url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/CreateThumnails',
            contentType: "application/json; charset=UTF-8",
            dataType: "json",
            data: JSON.stringify({ planImages: planImages }),
            success: function (msg) {
                var data = msg.d;
                if (data == "true") {
                    alert("Miniatyrbilder omskapade");
                }
                else {
                    alert("Något gick fel vid försök att skapa om miniatyrbilder");
                }

            },
            complete: function () {
                $spinner.addClass("spinner-hide");
                $spinner.next().remove();
                $spinner.after("<span>Skapa om miniatyrbilder för markerade plankarter</span>");
                $spinner.prop('disabled', false);
                getAllPlansDocs();
            },
            error: function () {
                alert("Fel!\nReCreateThumnails");
            }
        })
    }
    else {
        console.log("FEL");
    }

}


function ReloadThumnails() {
    getAllPlansDocs();
}


function DeleteThumnail(element) {

    var planImages = [$(element).parent("td").siblings().eq(1).text()];

    if (planImages) {

        return $.ajax({
            type: "POST",
            url: Lkr.Plan.Dokument.resolvedClientUrl + 'services/kontrollpanel.asmx/DeleteThumnails',
            contentType: "application/json; charset=UTF-8",
            dataType: "json",
            data: JSON.stringify({ planImages: planImages }),
            success: function (msg) {
                console.log(msg);
                var data = msg.d;
                if (data == "true") {
                    alert("Miniatyrbilder raderade");
                }
                else {
                    alert("Något gick fel vid försök att radera miniatyrbilder");
                }

            },
            complete: function () {
                getAllPlansDocs();
            },
            error: function () {
                alert("Fel!\nDeleteThumnails");
            }
        })
    }
    else {
        console.log("FEL");
    }

}


function GetCheckedPlansInTable() {
    return $("#table-thumnails input[type=checkbox]:checked")
        .map(function () {
            return $(this).parent("td").next().html();
        })
        .get();
}


$(document).ready(
    function () {

        getAllPlansDocs();

    }
);