﻿var urlBasePath = Lkr.Plan.Dokument.resolvedClientUrl;

// Antal sökta planer som kommit in till servern i URL:n
function nbrOfSearchedPlans(isPlansSearched, callback) {
    $.ajax({
        type: "POST",
        //url: '<%#ResolveClientUrl("~/services/plandokument.asmx/nbrOfSearchedPlans")%>',
        url: urlBasePath + 'services/plandokument.asmx/nbrOfSearchedPlans',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var nbr = msg.d;
            if (nbr != 0) {
                isPlansSearched = true;
                $('#planLista').show();
                $('#planSideWrapper').show();
                var $span = $("#searchResultNbr");
                $span.text("Antal sökta: " + nbr);

                callback(isPlansSearched);
            } else {
                callback(isPlansSearched);
            }
        },
        error: function () {
            alert("Fel!\nnbrOfSearchedPlans");
        }
    })
}; // SLUT nbrOfSearchedPlans



// Redovisar sökkriteret på vad planen söks efter, nyckel i fastighetsregistret, planbeteckningar, arkivserier etc.
function columnConditionOfSearchedPlans() {
    $.ajax({
        type: "POST",
        //url: '<%#ResolveClientUrl("~/services/plandokument.asmx/columnConditionOfSearchedPlans")%>',
        url: urlBasePath + 'services/plandokument.asmx/columnConditionOfSearchedPlans',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var info = JSON.parse(msg.d);
            if (info == '""' || info == "" || info == null) {
                info = "-";
            }
            var $span = $("#searchCriteria");
            $span.text("Kriterie: " + info);
        },
        error: function () {
            alert("Fel!\ncolumnConditionOfSearchedPlans");
        }
    })
}; // SLUT columnConditionOfSearchedPlans



// Redovisar sökkriteriet för vilken/vilka handlingar som eftersökts för planen/planerna via URL:n
function documentConditionOfSearchedPlans() {
    $.ajax({
        type: "POST",
        //url: '<%#ResolveClientUrl("~/services/plandokument.asmx/documentConditionOfSearchedPlans")%>',
        url: urlBasePath + 'services/plandokument.asmx/documentConditionOfSearchedPlans',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var info = JSON.parse(msg.d);
            if (info == '""' || info == "" || info == null || info == "dokument") {
                info = "-";
            } else if (info == "handling") {
                info = "planhandlingar";
            }
            var $span = $("#searchDocs");
            $span.text("Dokumenttyp: " + info);
        },
        error: function () {
            alert("Fel!\ndocumentConditionOfSearchedPlans");
        }
    })
}; // SLUT documentConditionOfSearchedPlans



// Redovisar söksträngen
function searchedPlans() {
    $.ajax({
        type: "POST",
        //url: '<%#ResolveClientUrl("~/services/plandokument.asmx/searchedPlans")%>',
        url: urlBasePath + 'services/plandokument.asmx/searchedPlans',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            var info = JSON.parse(msg.d);
            if (info == '""' || info == "" || info == null) {
                info = "-";
            }
            var $span = $("#searchString");
            $span.text("Sökta: " + info);
        },
        error: function () {
            alert("Fel!\nsearchedPlans");
        }
    })
}; // SLUT searchedPlans



// Redovisar totala antalet planer
function getStatTotNbrPlans() {
    $.ajax({
        type: "POST",
        //url: '<%#ResolveClientUrl("~/services/plandokument.asmx/getStatTotNbrPlans")%>',
        url: urlBasePath + 'services/plandokument.asmx/getStatTotNbrPlans',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d != '') {
                var $span = $("#statNbrPlans")
                $span.text("Totalt antal:");
                $span.append($("<br />"));
                var $table = $("<table>");
                var $tr = $("<tr>");
                var $td1 = $("<td>");
                var $td2 = $("<td>");
                var $td3 = $("<td>");
                $td1.text(" ");
                $td2.text(msg.d);
                $td3.text("st.");
                $tr.append($td1);
                $tr.append($td2);
                $tr.append($td3);
                $table.append($tr);
                $span.append($table);
            }
        },
        error: function () {
            alert("Fel!\ngetStatTotNbrPlans");
        }
    })
}; // SLUT getStatTotNbrPlans



// Redovisar antalet olika typer av planförekomster
function getStatNbrPlanTypes() {
    $.ajax({
        type: "POST",
        //url: '<%#ResolveClientUrl("~/services/plandokument.asmx/getStatNbrPlanTypes")%>',
        url: urlBasePath + 'services/plandokument.asmx/getStatNbrPlanTypes',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d != '') {
                var $span = $("#statNbrPlanTypes")
                $span.text("Typer:");
                $span.append($("<br />"));
                var $table = $("<table>");
                $.each(eval(msg.d), function (key, val) {
                    var $tr = $("<tr>");
                    var $td1 = $("<td>");
                    var $td2 = $("<td>");
                    var $td3 = $("<td>");
                    $td1.text(val.PLANFK);
                    $tr.append($td1);
                    $td2.text(val.ANTAL);
                    $tr.append($td2);
                    $td3.text("st.");
                    $tr.append($td3);
                    $table.append($tr);
                });
                $span.append($table);
            }
        },
        error: function () {
            alert("Fel!\ngetStatNbrPlanTypes");
        }
    })
}; // SLUT getStatNbrPlanTypes



// Redovisar antalet planer inom genomförande
function getStatNbrPlanImplement() {
    $.ajax({
        type: "POST",
        //url: '<%#ResolveClientUrl("~/services/plandokument.asmx/getStatNbrPlanImplement")%>',
        url: urlBasePath + 'services/plandokument.asmx/getStatNbrPlanImplement',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d != '') {
                var $span = $("#statNbrPlanImpImplement")
                $span.text("Inom genomförande:");
                $span.append($("<br />"));
                var $table = $("<table>");
                var $tr = $("<tr>");
                var $td1 = $("<td>");
                var $td2 = $("<td>");
                var $td3 = $("<td>");
                $td1.text(" ");
                $tr.append($td1);
                $td2.text(JSON.parse(msg.d));
                $tr.append($td2);
                $td3.text("st.");
                $tr.append($td3);
                $table.append($tr);
                $span.append($table);
            }
        },
        error: function () {
            alert("Fel!\ngetStatNbrPlanImplement");
        }
    })
}; // SLUT getStatNbrPlanImplement



// Listar sökta planer och initierar funktion för global kollaps/expandering av planlistan
function getSearchedPlans() {
    $.ajax({
        type: "POST",
        //url: '<%#ResolveClientUrl("~/services/plandokument.asmx/getPlanInfo")%>',
        url: urlBasePath + 'services/plandokument.asmx/getPlanInfo',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d != '') {
                Lkr.Plan.Dokument.planListInfo = JSON.parse(msg.d);
            } else {
                //Visa något vid inga sökträffar
            }
        },
        error: function () {
            alert("Fel!\ngetPlanInfo");
        },
        complete: function () {
            var $plans = $('#planLista');
            $.each(Lkr.Plan.Dokument.planListInfo, function (key, val) {
                if (val.NYCKEL != null) {
                    Lkr.Plan.Dokument.nbrOfPlanHits += 1;
                    Lkr.Plan.Dokument.initExpColAll = true;
                    var $h3 = $("<h3></h3>");
                    $h3.attr("planId", val.NYCKEL);
                    $h3.attr("columnCondition", val.BEGREPP);
                    $h3.addClass("planLista-header planLista-header-default planLista-corner-all");
                    var $iconSpan = $("<span></span>");
                    $iconSpan.addClass("planLista-header-icon planLista-header-default-icon");
                    $h3.append($iconSpan);
                    var $aktSpan = $('<span></span>');
                    $aktSpan.attr("id", "akt-" + val.NYCKEL);
                    $aktSpan.text(val.AKT);
                    $h3.append($aktSpan);
                    var akttidigare = "-";
                    var aktegen = "-";
                    var plannamn = "-";
                    if (val.PLANNAMN != null) {
                        plannamn = val.PLANNAMN;
                    }
                    if (val.AKTTIDIGARE != null) {
                        akttidigare = val.AKTTIDIGARE;
                    }
                    if (val.AKTEGEN != null) {
                        aktegen = val.AKTEGEN;
                    }
                    var $centerPlanItem = $("<span>(" + val.PLANFK + ")</span>");
                    $centerPlanItem.addClass("planFk");
                    var $arkivSerier = $("<span>Tidigare: " + akttidigare + "</br>Plan/bygglov: " + aktegen + "</span>");
                    $arkivSerier.addClass("arkivSerier");
                    var $knobGenomIndikator = $("<img />");
                    $knobGenomIndikator.addClass("knobGenomIndikator");
                    if (val.ISGENOMF == 1) {
                        $knobGenomIndikator.attr("src", urlBasePath + "pic/knob-orange.png");
                        $knobGenomIndikator.attr("title", "Inom genomförandetid");
                    } else {
                        $knobGenomIndikator.attr("src", urlBasePath + "pic/knob-green.png");
                        $knobGenomIndikator.attr("title", "Genomförandetid löpt ut");
                    }
                    $h3.append($centerPlanItem);
                    $h3.append($arkivSerier);
                    $h3.append($knobGenomIndikator);
                    $h3.click(function () {
                        var $headerPlan = $(this);
                        var planid = $headerPlan.attr("planid");
                        var arrayPlanId = [planid];
                        var $icon = $headerPlan.find('.planLista-header-icon');
                        var $contentPlan = $headerPlan.next();
                        if ($contentPlan.is(':visible')) {
                            $contentPlan.empty();
                            $contentPlan.hide();
                            $headerPlan.removeClass("planLista-header-active");
                            $headerPlan.removeClass("planLista-corner-top");
                            $icon.removeClass("planLista-header-active-icon");
                            $headerPlan.addClass("planLista-header-default");
                            $headerPlan.addClass("planLista-corner-all");
                            $icon.addClass("planLista-header-default-icon");
                        } else {
                            $contentPlan = putPlanContentHolder($contentPlan, planid);
                            getPlansBerorPlans([planid], function (planAffected) {
                                putPlanAffected(planid, planAffected);
                            });
                            $contentPlan.show();
                            $headerPlan.removeClass("planLista-header-default");
                            $headerPlan.removeClass("planLista-corner-all");
                            $icon.removeClass("planLista-header-default-icon");
                            $headerPlan.addClass("planLista-header-active");
                            $headerPlan.addClass("planLista-corner-top");
                            $icon.addClass("planLista-header-active-icon");

                            // Hämta planernas dokument
                            getPlansDocs(arrayPlanId, function (plansDocs) {

                                putPlansDocs($headerPlan, plansDocs);

                                // Hämtar maximal bredd och höjd som är möjlig för kartbild
                                var mapImageWidth = $('#map-' + planid).width();
                                var mapImageHeight = $('#doc-' + planid).outerHeight();
                                // Lagrar dimensioner för mätning av förändring när webbläsarfönster ändras i storlek
                                $('#map-' + planid).attr('dimension', mapImageWidth + "," + mapImageHeight)
                                // Drar bort för 10 % marginal (20 % / 2)
                                mapImageWidth = mapImageWidth - Math.round(mapImageWidth * 0.2);
                                mapImageHeight = mapImageHeight - Math.round(mapImageHeight * 0.2);
                                // Hämtar renderad kartbild med plan
                                putMapOfPlan(planid, mapImageWidth, mapImageHeight);

                            });
                        }

                        // Sätter rätt text för knappen "global kollapps/expandering" av planlistan
                        var divExpCol = $('#expcol');
                        var isPlanContentDivsVisible = false;
                        $("#planLista").children('div').each(function () {
                            if ($(this).is(':visible')) {
                                isPlanContentDivsVisible = true;
                                return false;
                            }
                        });
                        if (isPlanContentDivsVisible) {
                            divExpCol.text("Komprimera alla planer");
                        } else {
                            divExpCol.text("Expandera alla planer");
                        }

                    });

                    $plans.append($h3);

                    // Innehållsdelen för vardera plan i planlistan (under rubriken)
                    var $contentDiv = $("<div class='planLista-content'></div>");
                    $contentDiv.addClass("planLista-corner-bottom");

                    $plans.append($contentDiv);
                } else {
                    Lkr.Plan.Dokument.nbrOfPlanBoms += 1;
                }
                var $span = $("#statNbrPlanHits")
                $span.text("Planträff:");
                $span.append($("<br />"));
                var $table = $("<table>");
                var $tr = $("<tr>");
                var $td1 = $("<td>");
                var $td2 = $("<td>");
                var $td3 = $("<td>");
                $td1.text(" ");
                $td2.text(Lkr.Plan.Dokument.nbrOfPlanHits);
                $td3.text("st.");
                $tr.append($td1);
                $tr.append($td2);
                $tr.append($td3);
                $table.append($tr);
                $span.append($table);

                //$span = $("#statNbrPlanBoms")
                //$span.text("Ej sökträff:");
                //$span.append($("<br />"));
                //var $table = $("<table>");
                //var $tr = $("<tr>");
                //var $td1 = $("<td>");
                //var $td2 = $("<td>");
                //var $td3 = $("<td>");
                //$td1.text(" ");
                //$td2.text(Lkr.Plan.Dokument.nbrOfPlanBoms);
                //$td3.text("st.");
                //$tr.append($td1);
                //$tr.append($td2);
                //$tr.append($td3);
                //$table.append($tr);
                //$span.append($table);

            });

            // Om EN plan i planlistan, exapndera med dokument listningen av planer
            if (Lkr.Plan.Dokument.nbrOfPlanHits == 1) {
                var $contentDiv = $plans.children("div");
                var $headerPlan = $contentDiv.prev();
                var planid = $headerPlan.attr("planid");
                var arrayPlanId = [planid];
                var $contentDiv = putPlanContentHolder($contentDiv, planid);

                $contentDiv.show();

                getPlansBerorPlans([planid], function (planAffected) {
                    putPlanAffected(planid, planAffected);
                });

                var $icon = $headerPlan.find('.planLista-header-icon');;
                $headerPlan.removeClass("planLista-header-default");
                $headerPlan.removeClass("planLista-corner-all");
                $icon.removeClass("planLista-header-default-icon");
                $headerPlan.addClass("planLista-header-active");
                $headerPlan.addClass("planLista-corner-top");
                $icon.addClass("planLista-header-active-icon");

                // Hämta planernas dokument
                getPlansDocs(arrayPlanId, function (plansDocs) {

                    putPlansDocs($headerPlan, plansDocs);

                    // Hämtar maximal bredd och höjd som är möjlig för kartbild
                    var mapImageWidth = $('#map-' + planid).width();
                    var mapImageHeight = $('#doc-' + planid).outerHeight();
                    // Lagrar dimensioner för mätning av förändring när webbläsarfönster ändras i storlek
                    $('#map-' + planid).attr('dimension', mapImageWidth + "," + mapImageHeight)
                    // Drar bort för 10 % marginal (20 % / 2)
                    mapImageWidth = mapImageWidth - Math.round(mapImageWidth * 0.2);
                    mapImageHeight = mapImageHeight - Math.round(mapImageHeight * 0.2);
                    // Hämtar renderad kartbild med plan
                    putMapOfPlan(planid, mapImageWidth, mapImageHeight);

                });
            }

            if (Lkr.Plan.Dokument.initExpColAll)
                initialExpandCollapsAll()
        }
    })

}; // SLUT getSearchedPlans



// Skapar upp platshållare för rubrik, dokument och karta under var plans platshållare
function putPlanContentHolder($contentPlan, planid) {

    var $contentPlanHeader = $("<div></div>");
    $contentPlanHeader.attr("id", "tools-" + planid)
    $contentPlanHeader.addClass("planContent-tools");
    $contentPlan.append($contentPlanHeader);

    var $contentPlanHeader = $("<div></div>");
    $contentPlanHeader.attr("id", "affected-" + planid)
    $contentPlanHeader.addClass("planContent-affected");
    $contentPlan.append($contentPlanHeader);

    var $contentPlanHeader = $("<div></div>");
    $contentPlanHeader.attr("id", "head-" + planid)
    $contentPlanHeader.addClass("planContent-header");
    $contentPlan.append($contentPlanHeader);

    var $avsnittPlanhandling = $("<span></span>")
    $avsnittPlanhandling.addClass("planContent-avsnitt");
    $avsnittPlanhandling.text("Planhandlingar");

    var $contentPlanDocs = $("<div></div>");
    $contentPlanDocs.attr("id", "doc-" + planid)
    $contentPlanDocs.addClass("planContent-left");
    $contentPlanDocs.append($avsnittPlanhandling);
    $contentPlan.append($contentPlanDocs);

    var $avsnittPlanMap = $("<span></span>")
    $avsnittPlanMap.addClass("planContent-avsnitt");
    $avsnittPlanMap.text("Kartöversikt");

    var $contentPlanMap = $("<div></div>");
    $contentPlanMap.attr("id", "map-" + planid)
    $contentPlanMap.addClass("planContent-right");
    $contentPlanMap.append($avsnittPlanMap);
    $contentPlan.append($contentPlanMap);

    var $contentPlanDummy = $("<div></div>");
    $contentPlanDummy.addClass("planContent-dummy");
    $contentPlan.append($contentPlanDummy);

    return $contentPlan;

}; // SLUT putPlanContentHolder



function putPlanAffected(planid, planAffected) {

    if (planAffected.length > 0) {

        // Skapar stödmetod för kontroll om planen/institutet är möjlig att länka
        planAffected.isPlanfkLinkable = function (planfk) {
            switch (planfk) {
                case 'BPL':
                    return true;
                case 'DP':
                    return true;
                case 'OB':
                    return true;
                case 'SPL':
                    return true;
                case 'ÄDP':
                    return true;
                case 'ÄOB':
                    return true;
                default:
                    return false;
            }
        };


        // Platshållare för alla menyobjekt, grupp med berörda planer för drop down från meny
        var $divAffectedDropDown = $('<div>');
        $divAffectedDropDown.addClass('dropdown-menu dropdown-menu-right');
        $divAffectedDropDown.attr('aria-labelledby', 'affected-btn-' + planid);


        var statusPlanToText = [
            ["A", "Avregistrerad"],
            ["B", "Beslut"],
            ["F", "Förslag"],
            ["P", "Preliminär registrering"]
        ];


        var isSearchPlanChangedByDecision = false;
        var linkablePlanAll = [];
        var $menuGroupItemDecision = $('<div>');
        var $menuGroupItemDecisionOff = $('<div>');
        var $menuGroupItemNoDecision = $('<div>');
        var $menuGroupItemNoDecisionOff = $('<div>');
        // Menyavgränsare
        var affectedDivider = '<div class="dropdown-divider">'
        // Ikon länk
        var spanLinkNewWindow = '<span class="linkNewWindow" title="Öppnar länk i nytt webbläsarfönster"> </span>';


        // För varje påverkande beslut
        planAffected.forEach(function (itemAffected, index, affected) {

            // Länkning
            if (itemAffected.NYCKEL_PAVARKAN && itemAffected.STATUS_PAVARKAN == "B" && planAffected.isPlanfkLinkable(itemAffected.PAV_PLANFK)) {

                linkablePlanAll.push(itemAffected.NYCKEL_PAVARKAN);

                var $menuItem = $('<a>');
                $menuItem.attr({
                    'href': Lkr.Plan.Dokument.resolvedClientUrl + 'dokument,nyckel/' + itemAffected.NYCKEL_PAVARKAN,
                    'target': '_blank'
                });
                $menuItem.append(spanLinkNewWindow);

            }
            else {

                var $menuItem = $('<span>');
                $menuItem.addClass('cursor-redirect');

            }


            // Varning - Om plan blir ändrad av ÄDP eller ÄOB
            if ((itemAffected.BESKRIVNING == 'ändrad av' || itemAffected.BESKRIVNING == 'upphävd av' ||
                itemAffected.BESKRIVNING == 'har koppling till' || itemAffected.BESKRIVNING == 'består av') &&
                (itemAffected.PAV_PLANFK == 'ÄDP' || itemAffected.PAV_PLANFK == 'ÄOB' || itemAffected.PAV_PLANFK == 'TI')) {
                $menuItem.addClass('planContent-affected-warning');
                $menuItem.attr('title', 'Bör kontrollera vad som påverkar');

                if (!isSearchPlanChangedByDecision) {
                    isSearchPlanChangedByDecision = true;
                }
            }
            // Varning - Om ÄDP och ÄOB och ändrar
            if (itemAffected.BESKRIVNING == 'ändrar' && (itemAffected.PLANFK == 'ÄDP' || itemAffected.PLANFK == 'ÄOB')) {
                $menuItem.addClass('planContent-affected-warning');
                $menuItem.attr('title', 'Läses tillsammans med sökt ' + itemAffected.PLANFK);

                if (!isSearchPlanChangedByDecision) {
                    isSearchPlanChangedByDecision = true;
                }
            }



            // Avregistrerade
            if (itemAffected.STATUS_PAVARKAN == "A") {
                $menuItem.addClass('planContent-deregistered');
                statusPlanToText.forEach(function (element) {
                    if (element[0] == itemAffected.STATUS_PAVARKAN) {
                        $menuItem.attr('title', 'Påverkan är ej länkningsbar p.g.a. att den har status "' + element[1].toLowerCase() + '"');
                    }
                });

            }



            // Meny Item
            $menuItem.addClass('dropdown-item');

            if (itemAffected.REGISTRERAT_BESLUT == 1) {

                $menuItem.prepend(itemAffected.BESKRIVNING + ' ' + itemAffected.PAV_PLANFK + ' ' + itemAffected.PAVERKAN); // + ' - (' + itemAffected.STATUS_PAVARKAN + ')');

                if (itemAffected.STATUS_PAVARKAN == "A") {
                    $menuGroupItemDecisionOff.append($menuItem);
                }
                else {
                    $menuGroupItemDecision.append($menuItem);
                }

            }
            else {

                $menuItem.prepend(itemAffected.BESKRIVNING + ' ' + itemAffected.PAV_PLANFK + ' ' + itemAffected.PAVERKAN);

                if (itemAffected.STATUS_PAVARKAN == "A") {
                    $menuGroupItemNoDecisionOff.append($menuItem);
                }
                else {
                    $menuGroupItemNoDecision.append($menuItem);
                }

            }

        });


        // Länk för alla länkbara påverkande beslut
        if (linkablePlanAll.length > 0) {
            var $menuItem = $('<a>');
            $menuItem.addClass('dropdown-item');
            $menuItem.attr({
                'href': Lkr.Plan.Dokument.resolvedClientUrl + 'dokument,nyckel/' + linkablePlanAll.join('\,'),
                'target': '_blank',
                'title': 'Öppnar ny sida och listar alla nedan planer som existerar i fastighetsregistrets bestämmelsedel (Planregistret)'
            });
            $menuItem.text('Lista alla');
            // Komponering av meny
            $divAffectedDropDown.append($menuItem);
        }



        // Meny Rubriker
        var $planRegisterBeslut = $('<h6>');
        $planRegisterBeslut.addClass('dropdown-header');
        $planRegisterBeslut.attr({
            'title': 'Registrerade i planregistret'
        });
        $planRegisterBeslut.text('Planbeslut');
        
        var $ejPlanRegisterBeslut = $('<h6>');
        $ejPlanRegisterBeslut.addClass('dropdown-header');
        $ejPlanRegisterBeslut.attr({
            'title': 'Beslut som ej är registrerade i planregistret'
        });
        $ejPlanRegisterBeslut.text('Övriga beslut');



        // Komponering av meny
        if ($menuGroupItemDecision.html().length > 0 || $menuGroupItemDecisionOff.html().length > 0) {
            if (linkablePlanAll.length > 0) {
                $divAffectedDropDown.append(affectedDivider);
            }
            $divAffectedDropDown.append($planRegisterBeslut);
            $divAffectedDropDown.append($menuGroupItemDecision);
            $divAffectedDropDown.append($menuGroupItemDecisionOff);
        }
        if ($menuGroupItemNoDecision.html().length > 0 || $menuGroupItemNoDecisionOff.html().length > 0) {
            if (linkablePlanAll.length > 0 || $menuGroupItemDecision.html().length > 0) {
                $divAffectedDropDown.append(affectedDivider);
            }
            $divAffectedDropDown.append($ejPlanRegisterBeslut);
            $divAffectedDropDown.append($menuGroupItemNoDecision);
            $divAffectedDropDown.append($menuGroupItemNoDecisionOff);
        }



        // Meny
        var $btnAffectedGroup = $('<button>');
        $btnAffectedGroup.attr({
            id: 'affected-btn-' + planid,
            type: 'button',
            'data-toggle': 'dropdown',
            'data-boundary': 'viewport',
            'aria-haspopup': true,
            'aria-expanded': false,
            title: 'Andra planer som berör samma område, påverkar planen eller har påverkats av planen'
        });
        $btnAffectedGroup.addClass(
            'btn btn-secondary dropdown-toggle btn-xs'
        );
        if (isSearchPlanChangedByDecision) {
            $btnAffectedGroup.addClass('btn-outline-danger');
        }
        else {
            $btnAffectedGroup.addClass('btn-outline-secondary');
        }
        $btnAffectedGroup.text('Planpåverkan');

        var $divAffectedBtnGroup = $('<div>');
        $divAffectedBtnGroup.addClass('btn-group');
        $divAffectedBtnGroup.attr('role', 'group');

        $divAffectedBtnGroup.append($btnAffectedGroup);
        $divAffectedBtnGroup.append($divAffectedDropDown);
    }
    else {
        var $divAffectedBtnGroup = $('<button disabled>');
        $divAffectedBtnGroup.addClass('btn btn-outline-success btn-xs');
        $divAffectedBtnGroup.attr({
            id: 'affected-btn-' + planid,
            type: 'button',
            title: 'Ingen påverkan från andra registrerade planer och beslut'
        });
        $divAffectedBtnGroup.text('Ingen planpåverkan');

    }

    $('#affected-' + planid).append($divAffectedBtnGroup);

}; // SLUT putPlanAffected



// Hämtar planens olika dokument
function getPlansDocs(planIds, callback) {
    Lkr.Plan.AjaxCalls.getPlansDocs = $.ajax({
        type: "POST",
        //url: '<%=ResolveClientUrl("~/services/plandokument.asmx/getPlansDocs")%>',
        url: urlBasePath + 'services/plandokument.asmx/getPlansDocs',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        data: JSON.stringify({ planIds: planIds }),
        success: function (msg) {
            if (msg.d != '') {
                var jsonData = $.parseJSON(msg.d);
                // Initiering av vektor för planers dokument
                var plans = new Array(jsonData.length);
                for (i = 0; i < jsonData.length; i++) {
                    plans[i] = new Array(6);
                }
                // Fyll vektor med plandokument
                var arrayItemPlace = 0;
                $.each(eval(msg.d), function (key, val) {
                    plans[arrayItemPlace] = [
                        val.PLAN_ID,
                        val.NAME,
                        val.EXTENTION,
                        val.SIZE,
                        val.PATH,
                        val.DOCUMENTTYPE,
                        val.FINDTYPE,
                        val.DOCUMENTPART,
                        val.THUMNAILPATH,
                        val.THUMNAILINDICATION
                    ];
                    arrayItemPlace++;
                });
                // Returnera plandokument med dess plan-ID
                callback(plans);
            } else {
                callback(false);
            }
        },
        error: function () {
            alert("Fel: Systemfel (metod getPlansDocs i Landskrona.App.Plan.Dokument.Ws.WsPlanhandling), kontakta gis@landskrona.se vid upprepande fel.");
        }
    })

}; // SLUT getPlansDocs




// Hämtar planernas berörsrelationer till andra planer
function getPlansBerorPlans(planIds, callback) {
    Lkr.Plan.AjaxCalls.getPlansBerorPlans = $.ajax({
        type: "POST",
        url: urlBasePath + 'services/plandokument.asmx/getPlansBerorPlans',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        data: JSON.stringify({ planIds: planIds }),
        success: function (msg) {
            if (msg.d != '') {
                var jsonData = $.parseJSON(msg.d);
                // Initiering av vektor för planers dokument
                //var plans = new Array(jsonData.length);
                //for (i = 0; i < jsonData.length; i++) {
                //    plans[i] = new Array(6);
                //}
                //// Fyll vektor med plandokument
                //var arrayItemPlace = 0;
                //$.each(eval(msg.d), function (key, val) {
                //    plans[arrayItemPlace] = [
                //        val.PLAN_ID,
                //        val.NAME,
                //        val.EXTENTION,
                //        val.SIZE,
                //        val.PATH,
                //        val.DOCUMENTTYPE,
                //        val.FINDTYPE,
                //        val.DOCUMENTPART
                //    ];
                //    arrayItemPlace++;
                //});
                // Returnera planer med dess berörsrelation till andra planer
                callback(jsonData);
            } else {
                callback(false);
            }
        },
        error: function () {
            alert("Fel: Systemfel (metod getPlansDocs i Landskrona.App.Plan.Dokument.Ws.WsPlanhandling), kontakta gis@landskrona.se vid upprepande fel.");
        }
    })

}; // SLUT getPlansBerorPlans





// Dokumenttyper
function getDocumenttypes(callback) {
    $.ajax({
        type: "POST",
        url: urlBasePath + 'services/plandokument.asmx/getDokumenttyper',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d != '') {
                Lkr.Plan.Dokument.Dokumenttyper = JSON.parse(msg.d);
                callback(Lkr.Plan.Dokument.Dokumenttyper);

            } else {
                callback(Lkr.Plan.Dokument.Dokumenttyper);

                //Visa något vid inga dokumettyper

            }
        },
        error: function () {
            alert("Fel!\ngetDokumenttyper");
        }
    })
}; // SLUT getDocumenttypes



// Listar plandokumenten i var listad plans dokumenthållaren
// Skicka in parameter som vektor med plan-ID
function putPlansDocs($headerPlan, plansDocs) {

    var documenttyper;
    if (Lkr.Plan.Dokument.Dokumenttyper) {
        documenttyper = Lkr.Plan.Dokument.Dokumenttyper;
    }
    else {
        console.err("Dokumenttyper saknas.");
    }


    // Adderar signal om vilket avsnitt dokumentypen ska hamna under, planhandling, övriga plandokument eller inga plandokument
    // 0 = Planhandling
    // 1 = Övrigt plandokument
    // 2 = Dokumenttyp utan matchande dokument
    //documenttyper.forEach((object => { object.Avsnitt = 2; })); /* Ej fungerande i IE */
    documenttyper.forEach(function (object) { object.Avsnitt = 2 });
    //  Ska renderas som dokumenttyp med flera dokument som dokumenttypsgrupp (default false)
    //    true = finns fler än 1 dokument av samma dokumenttyp trotts namnkonvention som indikerar dokumentdelar
    //documenttyper.forEach((object => { object.Dokumenttypsgrupp = false; })); /* Ej fungerande i IE */
    documenttyper.forEach(function (object) { object.Dokumenttypsgrupp = false });
    // Antal deldokument (default 0)
    //documenttyper.forEach((object => { object.DokumenttypsdelarAntal = 0; })); /* Ej fungerande i IE */
    documenttyper.forEach(function (object) { object.DokumenttypsdelarAntal = 0 });
    // Dokumenttyp förekommer både som delat dokument och odelat
    //documenttyper.forEach((object => { object.DokumenttypDelatOdelat = false; })); /* Ej fungerande i IE */
    documenttyper.forEach(function (object) { object.DokumenttypDelatOdelat = false });
    // Antal dokument per filformat och dokumenttyp
    //documenttyper.forEach((object => { object.DokumenttypFormatAntal = []; })); /* Ej fungerande i IE */
    documenttyper.forEach(function (object) { object.DokumenttypFormatAntal = [] });


    // För varje sökt plan
    $.each(Lkr.Plan.Dokument.planListInfo, function (planKey, valuePlan) {
        // Om plannyckel finns (träff på sökning, sökparameter har träff när värde finns för bl.a. nyckel. Alla sökparametrar returneras för presentation av ev. brister i sökningen)
        // och när sökt plan är samma som plan på hemsidan
        if (valuePlan.NYCKEL != null && valuePlan.NYCKEL == $headerPlan.attr("planid")) {
            var planid = valuePlan.NYCKEL;
            var plannamn = valuePlan.PLANNAMN;

            var $contentPlanHeader = $('#head-' + planid)
            if (plannamn == null || plannamn == "") {
                plannamn = "inget plannamn i fastighetsregistret";
                $contentPlanHeader.addClass("ejplannamn");
            } else {
                $contentPlanHeader.addClass("plannamn");
            }
            $contentPlanHeader.text(plannamn);

            // Om dokument funna på plan-ID
            // plansDocs :
            // [0].PLAN_ID,
            // [1].NAME,
            // [2].EXTENTION,
            // [3].SIZE,
            // [4].PATH,
            // [5].DOCUMENTTYPE
            // [6].FINDTYPE
            // [7].DOCUMENTPART
            // [8].THUMNAILPATH
            // [9].THUMNAILINDICATION
            if (plansDocs) {
                console.log(plansDocs);

                //#region För varje dokument, räknar dokumentdelar för resp. dokumenttyp samt signalerar om dokumenttypen ska renderas som grupp
                $.each(eval(plansDocs), function (key, valueDoc) {
                    // Om dokument tillhör specifikt sökt plan
                    if (valuePlan.NYCKEL == valueDoc[0]) {

                        documenttyper.forEach(function (valueDoctype) {
                            if (valueDoc[6] == "IsPart" && valueDoc[5] == valueDoctype.Type) {
                                valueDoctype.DokumenttypsdelarAntal += 1;
                            }
                        });

                    }
                });
                documenttyper.forEach(function (valueDoctype) {

                    // Markera om dokumenttypen ska renderas som grupp
                    if (valueDoctype.DokumenttypsdelarAntal > 1) {
                        valueDoctype.Dokumenttypsgrupp = true;
                    }

                    // Förekommer dokument under flera matchningstyper (FINDTYPE)
                    // Markera för bristvarning i webbklient
                    var isIsPart = false, isExact = false;
                    plansDocs.forEach(function (valueDoc) {
                        // Om dokument tillhör specifikt sökt plan
                        if (valuePlan.NYCKEL == valueDoc[0]) {

                            // Om dokumentets dokumenttyp stämmer med vektorns dokumenttyp
                            if (valueDoctype.Type == valueDoc[5]) {

                                if (valueDoc[6] == "IsPart") {
                                    isIsPart = true;
                                }
                                if (valueDoc[6] == "Exact") {
                                    isExact = true;
                                }


                                // Dokumenterar vilka format och antal per dokumenttyp
                                if (valueDoctype.DokumenttypFormatAntal.length == 0) {
                                    valueDoctype.DokumenttypFormatAntal.push([valueDoc[2], 1]);
                                }
                                else {

                                    var isFormatEarlier = false;
                                    valueDoctype.DokumenttypFormatAntal.forEach(function (dfaItem, dfaIndex) {
                                        if (dfaItem[0] == valueDoc[2]) {
                                            isFormatEarlier = true;
                                            dfaItem[1] += 1;
                                        }
                                    });

                                    if (!isFormatEarlier) {
                                        valueDoctype.DokumenttypFormatAntal.push([valueDoc[2], 1]);
                                    }

                                }

                            }
                        }
                    });


                    if (isIsPart && isExact) {
                        valueDoctype.DokumenttypDelatOdelat = true;
                        console.warn("Plandokumenten redovisas ologiskt för plan " + valuePlan.AKT + " och bryter mot namnkonvetion.\nDokumenttypen " + valueDoctype.Type + " har dokument som matchat på exakt söksträng men även dokumentdelar. Är ej tillåtet enl. namnkonvention.\nDokumentobjekt:", plansDocs);
                    }

                });
                //#endregion


                // Platshållare för dokumentlistobjekt
                var $ulPlanhandlingar = $("<ul>");
                var $ulOvrPlandok = $("<ul>");
                $ulOvrPlandok.addClass("ovrPlandok");
                var $ulNoDoc = $("<ul>");

                var isDocFound = false;
                var arrayPlanhandlingItems = [];
                var arrayOvrPlandokItems = [];


                //#region Bygger dokumentlistobjekt
                // För varje dokument
                $.each(eval(plansDocs), function (key, valueDoc) {
                    // Om dokument tillhör specifik sökt plan
                    if (valuePlan.NYCKEL == valueDoc[0]) {
                        isDocFound = true;
                        // Sätter indikering för om dokumenttyp finns, förutsättning för lista med saknade dokumenttyper
                        // För varje dokumenttyp
                        documenttyper.forEach(function (valueDoctype) {

                            // Indikering att dokumenttyp finns
                            if (valueDoctype.Type == valueDoc[5]) {
                                // Lista med dokumenttyper
                                var $li = $("<li>");
                                $li.addClass(valueDoc[2].substring(1, valueDoc[2].length) + "-file");

                                // Valbarhet för varje dokument genom kryssruta
                                var $checkbox = $("<input />",
                                    { type: 'checkbox' });
                                // Hantera markering och flyout-texter för dokumentens kryssrutor och den globala kryssrutan
                                $checkbox.change(function () {
                                    $checkboxes = $('#doc-' + planid + ' ul input');
                                    var nbrCheckedBoxes = 0;
                                    $checkboxes.each(function () {
                                        var $inputCheck = $(this);
                                        if ($inputCheck.is(':checked')) {
                                            nbrCheckedBoxes++;
                                        }
                                        if (nbrCheckedBoxes >= $checkboxes.length) {
                                            // kryssas
                                            $('#allCheck-' + valuePlan.NYCKEL).prop('indeterminate', false);
                                            $('#allCheck-' + valuePlan.NYCKEL).prop('checked', true);
                                            $('#allCheck-' + valuePlan.NYCKEL).prop('title', 'Avmarkera alla dokument');
                                        } else if (nbrCheckedBoxes == 0) {
                                            // kryssas ej
                                            $('#allCheck-' + valuePlan.NYCKEL).prop('indeterminate', false);
                                            $('#allCheck-' + valuePlan.NYCKEL).prop('checked', false);
                                            $('#allCheck-' + valuePlan.NYCKEL).prop('title', 'Markera alla dokument');
                                        } else {
                                            // indeterminate
                                            $('#allCheck-' + valuePlan.NYCKEL).prop('indeterminate', true);
                                            if (this.checked) {
                                                $('#allCheck-' + valuePlan.NYCKEL).prop('title', 'Markera alla dokument');
                                            } else {
                                                $('#allCheck-' + valuePlan.NYCKEL).prop('title', 'Avmarkera alla dokument');
                                            }
                                        }

                                    });
                                });

                                // Skapa dokumentlänk
                                var $docLink = $("<a>");
                                var href = urlBasePath + valueDoc[4] + valueDoc[1];
                                $docLink.attr("href", href);
                                $docLink.attr("relhref", valueDoc[4] + valueDoc[1]);
                                $docLink.attr("target", "_blank");
                                // Möjliggör Bootstrap Popover för dokumenttyp plankarta
                                // TODO: slå ihop thumnails för paginering till pdf-fil
                                if (valueDoc[5] == "Karta" && valueDoc[2] == ".tif") {
                                    var thumnailsImgParam;
                                    if (valueDoc[9].indexOf("s")  != -1) {
                                        thumnailsImgParam = '"' + urlBasePath + valueDoc[8] + valueDoc[1].replace(/\.[^/.]+$/, "") + '_thumnail-s.jpg" /><span class="popover-content-big-image" title="Större bild"></span>';
                                    }
                                    else {
                                        thumnailsImgParam = '"' + urlBasePath + 'pic/no-image.png" alt="Avsaknad plankarta som miniatyr" title="Ingen miniatyrbild av plankartan" />';
                                    }
                                    // Ansluter Popover
                                    $docLink.popover({
                                        trigger: 'manual',
                                        placement: 'auto',
                                        title: valueDoc[1] + ' innehåller',
                                        html: true,
                                        content:
                                            '<div><img src=' + thumnailsImgParam + '</div>'
                                    }).on('mouseenter', function () {
                                        var _this = this;
                                        $(this).popover('show');
                                        $('.popover').on('mouseleave', function () {
                                            $(_this).popover('hide');
                                        });
                                        $('.popover').find('span').on('click', function () {

                                            $('<div class="modal" tabindex="-1"><div class="modal-dialog modal-wide modal-dialog-centered"><div class="modal-content"><img style="width:100%;" src="' + urlBasePath + valueDoc[8] + valueDoc[1].replace(/\.[^/.]+$/, "") + '_thumnail-l.jpg" /></div></div></div>').modal({
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
                                }
                                else {
                                    $docLink.attr("title", valueDoc[1]);
                                }
                                if (valueDoctype.Dokumenttypsgrupp) {
                                    var isOneFilePerFormat = false;
                                    valueDoctype.DokumenttypFormatAntal.forEach(function (item, index) {
                                        // Samma format och ett dokument
                                        if (item[0] == valueDoc[2] && item[1] == 1) {
                                            $docLink.text("dokument (" + bytesToSize(valueDoc[3]) + ")");
                                            isOneFilePerFormat = true;
                                        }
                                    });

                                    if (!isOneFilePerFormat) {
                                        if (valueDoc[7] == "") {
                                            $docLink.text("[SAKNAS DELTEXT] (" + bytesToSize(valueDoc[3]) + ")");
                                        }
                                        else {
                                            $docLink.text(valueDoc[7] + " (" + bytesToSize(valueDoc[3]) + ")");
                                        }
                                    }
                                }
                                else {
                                    $docLink.text(valueDoc[5] + " (" + bytesToSize(valueDoc[3]) + ")");
                                }

                                $li.append($checkbox);
                                $li.append($docLink);



                                var Doctype = {};
                                Doctype.LiItem = [];

                                // Om planhandling eller övrigt plandokument
                                if (valueDoctype.IsPlanhandling) {
                                    valueDoctype.Avsnitt = 0;

                                    // Skapar upp 1:a objektet, därefter fylls på
                                    if (arrayPlanhandlingItems.length == 0) {
                                        Doctype.Name = valueDoctype.Type
                                        Doctype.LiItem.push($li);
                                        arrayPlanhandlingItems.push({ Doctype: Doctype });
                                    }
                                    else {
                                        // Indikerar om dokumentet inte har hanterats som dokumenttypsdel. Läggs till senare som "vanligt" dokument i så fall.
                                        var isLiAdded = false;

                                        // Om dokumentet ska renderas som dokumenttypsdel enl. backend matchning efter namnkonvention
                                        if (valueDoctype.Dokumenttypsgrupp) {
                                            // Itterera igenom alla tidigare dokument för gruppering tillsammans efter dokumenttyp
                                            arrayPlanhandlingItems.forEach(function (item, index, theArray) {
                                                if (item.Doctype.Name == valueDoctype.Type) {
                                                    item.Doctype.LiItem.push($li);
                                                    isLiAdded = true;
                                                }
                                            });
                                        }

                                        // Adderar dokument som ej tidigare adderats
                                        if (!isLiAdded) {
                                            Doctype.Name = valueDoctype.Type;
                                            Doctype.LiItem.push($li);
                                            arrayPlanhandlingItems.push({ Doctype: Doctype });
                                        }
                                    }


                                } else {
                                    valueDoctype.Avsnitt = 1;

                                    // Skapar upp 1:a objektet, därefter fylls på
                                    if (arrayOvrPlandokItems.length == 0) {
                                        Doctype.Name = valueDoctype.Type
                                        Doctype.LiItem.push($li);
                                        arrayOvrPlandokItems.push({ Doctype: Doctype });
                                    }
                                    else {
                                        // Indikerar om dokumentet inte har hanterats som dokumenttypsdel. Läggs till senare som "vanligt" dokument i så fall.
                                        var isLiAdded = false;

                                        // Om dokumentet ska renderas som dokumenttypsdel enl. backend matchning efter namnkonvention
                                        if (valueDoctype.Dokumenttypsgrupp) {
                                            // Itterera igenom alla tidigare dokument för gruppering tillsammans efter dokumenttyp
                                            arrayOvrPlandokItems.forEach(function (item, index, theArray) {
                                                if (item.Doctype.Name == valueDoctype.Type) {
                                                    item.Doctype.LiItem.push($li);
                                                    isLiAdded = true;
                                                }
                                            });
                                        }

                                        // Adderar dokument som ej tidigare adderats
                                        if (!isLiAdded) {
                                            Doctype.Name = valueDoctype.Type;
                                            Doctype.LiItem.push($li);
                                            arrayOvrPlandokItems.push({ Doctype: Doctype });
                                        }
                                    }

                                }


                                // Placerar resp. dokument beroende på vilken rubrik de hamnar under (planhandling eller övriga)
                                if (valueDoctype.IsPlanhandling) {
                                    valueDoctype.Avsnitt = 0;
                                    $ulPlanhandlingar.append($li);
                                } else {
                                    valueDoctype.Avsnitt = 1;
                                    $ulOvrPlandok.append($li);
                                }
                            }
                        });


                    }

                });
                //#endregion



                //#region Skapar GUI-grupper med listobjekt för dokumenttyper med flera deldokument samt placerar listobjekt under i resp. platshållare för dokumentrubrik
                arrayPlanhandlingItems.forEach(function (item) {
                    // Om flera listobjekt, gruppera dessa under dokumenttypsrubrik, annars addera direkt till lista
                    if (item.Doctype.LiItem.length > 1) {
                        var $liDoctypeGroup = $("<li>");
                        $liDoctypeGroup.css({
                            "padding": "0"
                        });
                        $liDoctypeGroup.text(item.Doctype.Name + " (dokumenttyp flera)");
                        var $olDoctypeGroup = $("<ol>");
                        $olDoctypeGroup.css({
                            "padding-left": "24px"
                        });

                        // Bygg grupplista för alla listobjekt av dokumenttypen
                        item.Doctype.LiItem.forEach(function (liItem) {
                            $olDoctypeGroup.append(liItem);
                        });
                        $liDoctypeGroup.append($olDoctypeGroup);
                        $ulPlanhandlingar.append($liDoctypeGroup);
                    }
                    else {
                        $ulPlanhandlingar.append(item.Doctype.LiItem[0]);
                    }
                });
                arrayOvrPlandokItems.forEach(function (item) {
                    // Om flera listobjekt, gruppera dessa under dokumenttypsrubrik, annars addera direkt till lista
                    if (item.Doctype.LiItem.length > 1) {
                        var $liDoctypeGroup = $("<li>");
                        $liDoctypeGroup.css({
                            "padding": "0"
                        });
                        $liDoctypeGroup.text(item.Doctype.Name + " (dokumenttyp flera)");
                        var $olDoctypeGroup = $("<ol>");
                        $olDoctypeGroup.css({
                            "padding-left": "24px"
                        });
                        +
                            // Bygg grupplista för alla listobjekt av dokumenttypen
                            item.Doctype.LiItem.forEach(function (liItem) {
                                $olDoctypeGroup.append(liItem);
                            });
                        $liDoctypeGroup.append($olDoctypeGroup);
                        $ulOvrPlandok.append($liDoctypeGroup);
                    }
                    else {
                        $ulOvrPlandok.append(item.Doctype.LiItem[0]);
                    }
                });
                //#endregion


                // För varje dokumenttyp
                documenttyper.forEach(function (valDoctype) {
                    // Om plandokument ej funnen uppdelad på specifik dokumenttyp
                    if (valDoctype.Avsnitt == 2) {
                        var $li = $("<li>");
                        $li.addClass("no-file");
                        $li.text(valDoctype.Type);
                        $ulNoDoc.append($li);
                    }
                });


                var $contentPlanDocs = $('#doc-' + planid)

                // GUI om planhandlingar inte har dokument
                if ($ulPlanhandlingar.children().length > 0) {
                    $contentPlanDocs.append($ulPlanhandlingar);
                } else {
                    $contentPlanDocs.append($("<br />"))
                    $contentPlanDocs.append("-");
                    $contentPlanDocs.append($("<br />"))
                    $contentPlanDocs.append($("<br />"))
                }

                // GUI om övriga plandokument inte har dokument
                var $avsnittPlanOvrPlandok = $("<span></span>")
                $avsnittPlanOvrPlandok.addClass("planContent-avsnitt");
                $avsnittPlanOvrPlandok.text("Övriga plandokument");

                $contentPlanDocs.append($avsnittPlanOvrPlandok);
                if ($ulOvrPlandok.children().length > 0) {
                    $contentPlanDocs.append($ulOvrPlandok);
                } else {
                    $contentPlanDocs.append($("<br />"))
                    $contentPlanDocs.append("-");
                    $contentPlanDocs.append($("<br />"))
                    $contentPlanDocs.append($("<br />"))
                }

                // Bygger dispositionen för dokumenttyperna utan plandokument
                var $avsnittEjDokument = $("<span></span>")
                $avsnittEjDokument.addClass("planContent-avsnitt");
                $avsnittEjDokument.text("Ej enskilt upprättade dokument");
                var $noPlanDocsWrapper = $("<div>");
                var $noPlanDocsHeader = $("<div>");
                var $noPlanDocsIcon = $("<div>");
                var $noPlanDocsSpan = $("<span>...</span>");
                $noPlanDocsHeader.addClass("docs");
                $noPlanDocsHeader.attr({ "title": "Klicka för att expandera" });
                $noPlanDocsHeader.append($avsnittEjDokument);
                $noPlanDocsHeader.append("<br />");
                $noPlanDocsIcon.addClass("docs-collapsed");
                $noPlanDocsIcon.css({ "margin-left": "0", "display": "inline-block" });
                $noPlanDocsHeader.append($noPlanDocsIcon);
                $noPlanDocsHeader.append($noPlanDocsSpan);
                var $noPlanDocsList = $("<div>");
                $noPlanDocsList.css({ "margin-left": "1em", "margin-top": "-1em" });
                $noPlanDocsList.toggle(false);
                if ($ulNoDoc.children().length > 0) {
                    $noPlanDocsList.append($ulNoDoc);
                } else {
                    $noPlanDocsList.append($("<br />"));
                    $noPlanDocsList.append("-");
                }

                $noPlanDocsWrapper.append($noPlanDocsHeader);
                $noPlanDocsWrapper.append($noPlanDocsList);
                $contentPlanDocs.append($noPlanDocsWrapper);



                // Ansluter event expandering/komprimering av avsnitt med dokumenttyper utan filer
                // Hover-event
                $noPlanDocsHeader.hover(
                    function () {
                        if ($noPlanDocsList.is(":visible")) {
                            $noPlanDocsIcon.addClass("docs-expand-hover");
                        }
                        else {
                            $noPlanDocsIcon.addClass("docs-collapsed-hover");
                        }
                    }, function () {
                        if ($noPlanDocsList.is(":visible")) {
                            $noPlanDocsIcon.removeClass("docs-expand-hover");
                            $noPlanDocsIcon.addClass("docs-expand");
                        }
                        else {
                            $noPlanDocsIcon.removeClass("docs-collapsed-hover");
                            $noPlanDocsIcon.addClass("docs-collapsed");
                        }
                    }
                );
                // Click-event
                $noPlanDocsHeader.click(function () {
                    $noPlanDocsList.toggle();
                    if ($noPlanDocsList.is(":visible")) {
                        $noPlanDocsIcon.removeClass("docs-collapsed");
                        $noPlanDocsIcon.removeClass("docs-collapsed-hover");
                        $noPlanDocsIcon.addClass("docs-expand");
                        $noPlanDocsIcon.addClass("docs-expand-hover");
                        $noPlanDocsSpan.empty();
                    }
                    else {
                        $noPlanDocsIcon.removeClass("docs-expand");
                        $noPlanDocsIcon.removeClass("docs-expand-hover");
                        $noPlanDocsIcon.addClass("docs-collapsed");
                        $noPlanDocsIcon.addClass("docs-collapsed-hover");
                        $noPlanDocsSpan.text("...");
                    }
                });



                // Skapa toolbar om dokument existerar
                if (isDocFound) {

                    var $imgPack = $('<img />', {
                        src: urlBasePath + 'pic/noun_4501_16x16_black.png',
                        title: 'Paketera valda filer och ladda ned'
                    });
                    $imgPack.addClass('hand imgPlanDokument');
                    $imgPack.click(function () {
                        var zipFileNamePart = $('#akt-' + planid).text();
                        var files = [];
                        var documentChecked = false;
                        $('#doc-' + planid + ' ul input').each(function () {
                            var $inputCheck = $(this);
                            if ($inputCheck.is(':checked')) {
                                files.push($inputCheck.siblings('a').attr('relhref'));
                                documentChecked = true;
                            }
                        });

                        if (documentChecked) {
                            toggleToolZip(planid);
                            clearToolCheckedZip(planid);
                            getFilesZipped(files, zipFileNamePart)
                        } else {
                            alert("Inga dokument valda!");
                        }
                    });

                    var $checkboxAllDocs = $("<input />", {
                        type: 'checkbox',
                        id: 'allCheck-' + planid,
                        title: 'Markera alla dokument'
                    }).addClass('checkboxPlanDokument');
                    $checkboxAllDocs.change(function () {
                        $thisCheckbox = $(this);
                        var status = this.checked;
                        if (status) {
                            $('#doc-' + planid + ' ul input').prop('checked', status);
                            $thisCheckbox.prop('title', 'Avmarkera alla dokument');
                        } else {
                            $('#doc-' + planid + ' ul input').prop('checked', status);
                            $thisCheckbox.prop('title', 'Markera alla dokument');
                        }
                    });

                    var $divSelectAndPack = $('<div>').addClass('planContent-tools-checkzip');
                    $divSelectAndPack.append($checkboxAllDocs);
                    $divSelectAndPack.append($imgPack);

                    var $fileButtonSelect = $('<div title="Markera filer och ladda ner">');
                    $fileButtonSelect.addClass('planContent-tools-zip');
                    $fileButtonSelect.append('<div class="clickArea">').click(function () {
                        toggleToolZip(planid);
                    });

                    var $divPlaceHolder = $('<div>');
                    $divPlaceHolder.append($fileButtonSelect)
                    $divPlaceHolder.append($divSelectAndPack);

                    $('#tools-' + planid).append($divPlaceHolder);

                }


                if (!isDocFound) {
                    console.warn("Inga dokument till sökt plan " + valuePlan.AKT + " hittades. Kontakta gis@landskrona.se.\nPlanobjekt: ", valuePlan);
                }

            } else {

                alert("Inga dokument till sökt(a) plan(er) hittades. Kontakta gis@landskrona.se.");
                //TODO: Skriv ut i planens platshållare att dokument saknas
            }
        }
    });


}; // SLUT putPlansDocs



// Öppna/stänga funktioner till paketering (zip) och nedladdning
function toggleToolZip(planid) {

    $('#tools-' + planid + ' .planContent-tools-checkzip').animate({
        width: 'toggle',
        duration: 'slow'
    }, function () {
        $('#doc-' + planid + ' ul input').toggle();
    });

} // SLUT toggleToolZip



// Avmarkera alla kryssrutor
function clearToolCheckedZip(planid) {

    $('.planContent-tools input').prop('indeterminate', false);
    $('.planContent-tools input').prop('checked', false);
    $('.planContent-left input').prop('checked', false);

} // SLUT clearToolCheckedZip



// Ladda ner dokument zippade via webbtjänst
function getFilesZipped(files, zipFileNamePart) {
    toggleLoadingImage(true);

    $.ajax({
        type: "POST",
        //url: '<%#ResolveClientUrl("~/services/plandokument.asmx/getDocsZipped")%>',
        url: urlBasePath + 'services/plandokument.asmx/getDocsZipped',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        data: JSON.stringify({ planDocsPaths: files, zipFileNamePart: zipFileNamePart }),
        success: function (zipFile) {
            if (zipFile.d != '') {
                location.href = urlBasePath + $.parseJSON(zipFile.d);
            }
            ///TODO: Plocka bort gif-bild
        },
        complete: function () {
            toggleLoadingImage(false);
        },
        error: function () {
            alert("Fel!\ngetFilesZipped");
        }
    })
}; // SLUT getFilesZipped



// Instantiera knappen för "global kollapps/expandering" av planlistan
function initialExpandCollapsAll() {
    var newDivExpCol = $('<button id="expcol" type="button" class="btn btn-outline-dark btn-sm">');
    var isVisible = false;
    $("#planLista").children('div').each(function () {
        if ($(this).is(':visible')) {
            isVisible = true;
            return false;
        }
    });
    if (isVisible) {
        newDivExpCol.text("Komprimera alla planer");
    } else {
        newDivExpCol.text("Expandera alla planer");
    }

    // Ansluta funktion till DIV för kollappsa eller expandera alla planer i planlistan
    newDivExpCol.click(function () {
        var $expcol = $(this);
        var isVisible = false;
        $("#planLista").children('div').each(function () {
            if ($(this).is(':visible')) {
                isVisible = true;
                return false;
            }
        });

        var $plans = $("#planLista")
        var $planHeaders = $plans.children('h3');
        var $planHeaderIcons = $planHeaders.find('.planLista-header-icon');
        var $contentDivs = $plans.children('div')
        if (isVisible) {
            $contentDivs.empty();
            $contentDivs.hide();
            $planHeaders.removeClass("planLista-header-active planLista-corner-top");
            $planHeaders.addClass("planLista-header-default planLista-corner-all");
            $planHeaderIcons.removeClass("planLista-header-active-icon");
            $planHeaderIcons.addClass("planLista-header-default-icon");
            $expcol.text("Expandera alla planer");
        } else {
            // Redan startade ajax-request som ej gett svar avslutas innan initiering av nya (motverkar ev. dubblering av information
            if (Lkr.Plan.AjaxCalls.putMapOfPlan != null) {
                Lkr.Plan.AjaxCalls.putMapOfPlan.abort();
            }
            if (Lkr.Plan.AjaxCalls.getPlansDocs != null) {
                Lkr.Plan.AjaxCalls.getPlansDocs.abort();
            }
            if (Lkr.Plan.AjaxCalls.getPlansBerorPlans != null) {
                Lkr.Plan.AjaxCalls.getPlansBerorPlans.abort();
            }


            $contentDivs.each(function () {
                var $contentDiv = $(this);
                var planid = $contentDiv.prev().attr("planid");
                putPlanContentHolder($contentDiv, planid);
                getPlansBerorPlans([planid], function (planAffected) {
                    putPlanAffected(planid, planAffected);
                });
            });
            $contentDivs.show();
            $planHeaders.removeClass("planLista-header-default planLista-corner-all");
            $planHeaderIcons.removeClass("planLista-header-default-icon");
            $planHeaders.addClass("planLista-header-active planLista-corner-top");
            $planHeaderIcons.addClass("planLista-header-active-icon");
            $('#expcol').text("Komprimera alla planer");

            // Skapa vektor av alla listade planers plan-ID
            var planids = [];
            $planHeaders.each(function () {
                planids.push($(this).attr("planid"));
            });

            // Hämta planernas dokument
            getPlansDocs(planids, function (plansDocs) {
                $planHeaders.each(function () {
                    putPlansDocs($(this), plansDocs);

                    var planID = $(this).attr("planid");

                    // Hämtar maximal bredd och höjd som är möjlig för kartbild
                    var mapImageWidth = $('#map-' + planID).width();
                    var mapImageHeight = $('#doc-' + planID).outerHeight();
                    // Lagrar dimensioner för mätning av förändring när webbläsarfönster ändras i storlek
                    $('#map-' + planID).attr('dimension', mapImageWidth + "," + mapImageHeight)
                    // Drar bort för 10 % marginal (20 % / 2)
                    mapImageWidth = mapImageWidth - Math.round(mapImageWidth * 0.2);
                    mapImageHeight = mapImageHeight - Math.round(mapImageHeight * 0.2);
                    // Hämtar renderad kartbild med plan
                    putMapOfPlan(planID, mapImageWidth, mapImageHeight);
                });
            });
        }
    });

    $('#planSideWrapper').prepend(newDivExpCol);

}; // SLUT initialExpandCollapsAll



// Hämtar och placerar kartöversikt efter inskickat plan-ID och möjlig storlek på bildplatsen.
function putMapOfPlan(planID, imageWidth, imageHeight) {
    var $animatedMapDiv = $("<div id='mapLoadDiv-" + planID + "'></div>");
    $animatedMapDiv.css('text-align', 'center');
    $animatedMapDiv.addClass('mapGeneratingGif');
    $animatedMapDiv.append("<img id='getingMap-" + planID + "' src='" + urlBasePath + "pic/animated_windows8_64.GIF' />");
    $animatedMapDiv.append("<br /><br /><span>Kartbild hämtas...</span>");

    $('#map-' + planID).find('div, img').remove();
    $('#map-' + planID).append($animatedMapDiv);

    var errorMessage = "Kartbild kunde inte hämtas.<br />Uppdatera sidan genom t.ex. knappen F5,<br /> återkommer felet vänd dig till kontakt (se höger e-postikon).";

    Lkr.Plan.AjaxCalls.putMapOfPlan = $.ajax({
        type: "POST",
        url: urlBasePath + 'services/plandokument.asmx/getPlanMapImageAsBase64String',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        data: "{planID: '" + planID + "', imageWidth: '" + imageWidth + "', imageHeight: '" + imageHeight + "'}",
        success: function (msg) {
            if (msg.d != '') {
                var imageObject = eval(msg.d);
                if (imageObject) {
                    var mapSessionImage = imageObject[0].MAPIMAGEBASE64;
                    var mapSessionImageWidth = imageObject[0].WIDTH;
                    var mapSessionImageHeight = imageObject[0].HEIGHT;
                    var $mapImg = $('<img />');
                    //$mapImg.attr('src', "data:image/png;base64," + mapSessionImage.substring(1).substring(0, mapSessionImage.length - 2));
                    $mapImg.attr('src', "data:image/png;base64," + mapSessionImage);
                    $mapImg.attr('width', mapSessionImageWidth);
                    $mapImg.attr('height', mapSessionImageHeight);
                    // Om kartbild är högre än plattsen med listade dokument eller bredare än avseed plats ska inte absolut positionering göras
                    var docListHeight = $('#doc-' + planID).outerHeight();
                    var mapPlaceholderWidth = $('#map-' + planID).width();
                    if (docListHeight > mapSessionImageHeight && mapPlaceholderWidth > mapSessionImageWidth) {
                        var topPosition = $('#doc-' + planID).position().top;
                        var leftPosition = $('#map-' + planID).position().left;
                        // Mitten på dokumentlistan minus halva kartbildshöjden
                        var newTopPosition = (topPosition + docListHeight / 2) - (mapSessionImageHeight / 2);
                        // Mitten på hållaren för kartbild minus halva bredden på kartbild
                        var newLeftPosition = (leftPosition + mapPlaceholderWidth / 2) - (mapSessionImageWidth / 2);
                        $mapImg.css({
                            position: "absolute",
                            top: newTopPosition + "px",
                            left: newLeftPosition + "px"
                        });
                    }

                    $('#map-' + planID).children('div, img').remove();
                    $('#map-' + planID).append($mapImg);
                }
                else {
                    $('#map-' + planID).find('img').attr("src", urlBasePath + "pic/no-image.png");
                    $('#map-' + planID).find('span').html(errorMessage);
                }
            }
        },
        error: function (jqxhr, status, error) {
            $('#map-' + planID).find('img').attr("src", urlBasePath + "pic/no-image.png");
            $('#map-' + planID).find('span').html(errorMessage);
            var err = eval(jqxhr);
            console.error(err.status + " " + err.statusText + "\n" +
                err.statusCode);
        },
        complete: function () {
        }
    })
}; // SLUT putMapOfPlan