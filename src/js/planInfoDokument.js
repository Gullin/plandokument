﻿var urlBasePath = Lkr.Plan.Dokument.resolvedClientUrl;

// Antal sökta planer som kommit in till servern i URL:n
function nbrOfSearchedPlans(isPlansSearched, callback) {
    $.ajax({
        type: "POST",
        //url: '<%#ResolveClientUrl("~/plandokument.asmx/nbrOfSearchedPlans")%>',
        url: urlBasePath + 'plandokument.asmx/nbrOfSearchedPlans',
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
        //url: '<%#ResolveClientUrl("~/plandokument.asmx/columnConditionOfSearchedPlans")%>',
        url: urlBasePath + 'plandokument.asmx/columnConditionOfSearchedPlans',
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
        //url: '<%#ResolveClientUrl("~/plandokument.asmx/documentConditionOfSearchedPlans")%>',
        url: urlBasePath + 'plandokument.asmx/documentConditionOfSearchedPlans',
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
        //url: '<%#ResolveClientUrl("~/plandokument.asmx/searchedPlans")%>',
        url: urlBasePath + 'plandokument.asmx/searchedPlans',
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
        //url: '<%#ResolveClientUrl("~/plandokument.asmx/getStatTotNbrPlans")%>',
        url: urlBasePath + 'plandokument.asmx/getStatTotNbrPlans',
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
        //url: '<%#ResolveClientUrl("~/plandokument.asmx/getStatNbrPlanTypes")%>',
        url: urlBasePath + 'plandokument.asmx/getStatNbrPlanTypes',
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
        //url: '<%#ResolveClientUrl("~/plandokument.asmx/getStatNbrPlanImplement")%>',
        url: urlBasePath + 'plandokument.asmx/getStatNbrPlanImplement',
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
        //url: '<%#ResolveClientUrl("~/plandokument.asmx/getPlanInfo")%>',
        url: urlBasePath + 'plandokument.asmx/getPlanInfo',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d != '') {
                Lkr.Plan.Dokument.planListInfo = JSON.parse(msg.d);

                //console.log(Lkr.Plan.Dokument.planListInfo);
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
    $contentPlanHeader.attr("id", "head-" + planid)
    $contentPlanHeader.addClass("planContent-header");
    $contentPlan.append($contentPlanHeader);

    var $contentPlanDocs = $("<div></div>");
    $contentPlanDocs.attr("id", "doc-" + planid)
    $contentPlanDocs.addClass("planContent-left");
    $contentPlanDocs.text("Planhandlingar");
    $contentPlan.append($contentPlanDocs);

    var $contentPlanMap = $("<div></div>");
    $contentPlanMap.attr("id", "map-" + planid)
    $contentPlanMap.addClass("planContent-right");
    $contentPlanMap.text("Kartöversikt");
    $contentPlan.append($contentPlanMap);

    var $contentPlanDummy = $("<div></div>");
    $contentPlanDummy.addClass("planContent-dummy");
    $contentPlan.append($contentPlanDummy);

    return $contentPlan;

}; // SLUT putPlanContentHolder



// Hämtar planens olika dokument
function getPlansDocs(planIds, callback) {
    Lkr.Plan.AjaxCalls.getPlansDocs = $.ajax({
        type: "POST",
        //url: '<%=ResolveClientUrl("~/plandokument.asmx/getPlansDocs")%>',
        url: urlBasePath + 'plandokument.asmx/getPlansDocs',
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
                    plans[arrayItemPlace] = [val.PLAN_ID,
                                             val.NAME,
                                             val.EXTENTION,
                                             val.SIZE,
                                             val.PATH,
                                             val.DOCUMENTTYPE];
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



// Listar plandokumenten i var listad plans dokumenthållaren
// Skicka in parameter som vektor med plan-ID
function putPlansDocs($headerPlan, plansDocs) {
    // För varje sökt plan
    $.each(Lkr.Plan.Dokument.planListInfo, function (planKey, planVal) {
        // Om plannyckel finns (träff på sökning, sökparameter har träff när värde finns för bl.a. nyckel. Alla sökparametrar returneras för presentation av ev. brister i sökningen)
        // och när sökt plan är samma som plan på hemsidan
        if (planVal.NYCKEL != null && planVal.NYCKEL == $headerPlan.attr("planid")) {
            var planid = planVal.NYCKEL;
            var plannamn = planVal.PLANNAMN;

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
            if (plansDocs) {
                // Dokumenttyper samt indikering om respektive dokumenttyp är hittad bland dokument
                // 0 = Planhandling
                // 1 = Övrigt plandokument
                // 2 = Dokumenttyp utan matchande dokument
                // true = dokumenttyp ses som planhandling
                var documenttypesArray = [["Beskrivning", 2, true],
                    ["Bestämmelser", 2, true],
                    ["Fastighetsförteckning", 2, false],
                    ["Genomförande", 2, true],
                    ["Grundkarta", 2, false],
                    ["Illustration", 2, true],
                    ["Karta", 2, true],
                    ["Samrådsredogörelse", 2, true],
                    ["Utlåtande", 2, true],
                    ["Plan- och genomförandebeskrivning", 2, true],
                    ["Kvalitetsprogram", 2, false],
                    ["Miljökonsekvensbeskrivning", 2, false],
                    ["Bullerutredning", 2, false],
                    ["Gestaltningsprogram", 2, false],
                    ["Övriga", 2, false]];

                var $ul = $("<ul>");
                var $ulOvrPlandok = $("<ul>");
                $ulOvrPlandok.addClass("ovrPlandok");
                var $ulNoDoc = $("<ul>");
                var isDocFound = false;
                // För varje dokument
                $.each(eval(plansDocs), function (key, val) {
                    // Om dokument tillhör specifik sökt plan
                    if (planVal.NYCKEL == val[0]) {
                        isDocFound = true;
                        // Sätter indikering för om dokumenttyp finns, förutsättning för lista med saknade dokumenttyper
                        // För varje dokumenttyp
                        $.each(eval(documenttypesArray), function (keyDoctype, valDoctype) {
                            // Indikering att dokumenttyp finns
                            if (valDoctype[0] == val[5]) {
                                // Lista med dokumenttyper
                                var $li = $("<li>");
                                $li.addClass(val[2].substring(1, val[2].length) + "-file");

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
                                    });

                                    if (nbrCheckedBoxes >= $checkboxes.length) {
                                        // kryssas
                                        $('#allCheck-' + planVal.NYCKEL).prop('indeterminate', false);
                                        $('#allCheck-' + planVal.NYCKEL).prop('checked', true);
                                        $('#allCheck-' + planVal.NYCKEL).prop('title', 'Avmarkera alla dokument');
                                    } else if (nbrCheckedBoxes == 0) {
                                        // kryssas ej
                                        $('#allCheck-' + planVal.NYCKEL).prop('indeterminate', false);
                                        $('#allCheck-' + planVal.NYCKEL).prop('checked', false);
                                        $('#allCheck-' + planVal.NYCKEL).prop('title', 'Markera alla dokument');
                                    } else {
                                        // indeterminate
                                        $('#allCheck-' + planVal.NYCKEL).prop('indeterminate', true);
                                        if (this.checked){
                                            $('#allCheck-' + planVal.NYCKEL).prop('title', 'Markera alla dokument');
                                        } else {
                                            $('#allCheck-' + planVal.NYCKEL).prop('title', 'Avmarkera alla dokument');
                                        }
                                    }


                                });

                                // Skapa dokumentlänk
                                var $docLink = $("<a>");
                                var href = urlBasePath + val[4] + val[1];
                                $docLink.attr("href", href);
                                $docLink.attr("target", "_blank");
                                $docLink.attr("title", val[1]);
                                $docLink.attr("relhref", val[4] + val[1]);
                                $docLink.text(val[5] + " (" + bytesToSize(val[3]) + ")");
                                $li.append($checkbox);
                                $li.append($docLink);

                                // Placerar resp. dokument beroende på vilken rubrik de hamnar under (planhandling eller övriga)
                                if (valDoctype[2]) {
                                    $ul.append($li);
                                    valDoctype[1] = 0;
                                } else {
                                    valDoctype[1] = 1;
                                    $ulOvrPlandok.append($li);
                                }
                            }
                        });
                    }
                });


                // För varje dokumenttyp
                $.each(eval(documenttypesArray), function (keyDoctype, valDoctype) {
                    // Om plandokument ej funnen uppdelad på specifik dokumenttyp
                    if (valDoctype[1] == 2) {
                        var $li = $("<li>");
                        $li.addClass("no-file");
                        $li.text(valDoctype[0]);
                        $ulNoDoc.append($li);
                    }
                });

                var $contentPlanDocs = $('#doc-' + planid)

                if ($ul.children().length > 0) {
                    $contentPlanDocs.append($ul);
                } else {
                    $contentPlanDocs.append($("<br />"))
                    $contentPlanDocs.append("-");
                    $contentPlanDocs.append($("<br />"))
                    $contentPlanDocs.append($("<br />"))
                }

                $contentPlanDocs.append("Övriga plandokument");
                if ($ulOvrPlandok.children().length > 0) {
                    $contentPlanDocs.append($ulOvrPlandok);
                } else {
                    $contentPlanDocs.append($("<br />"))
                    $contentPlanDocs.append("-");
                    $contentPlanDocs.append($("<br />"))
                    $contentPlanDocs.append($("<br />"))
                }

                // Bygger dispositionen för dokumenttyperna utan filer
                var $noPlanDocsWrapper = $("<div>");
                var $noPlanDocsHeader = $("<div>");
                var $noPlanDocsIcon = $("<div>");
                var $noPlanDocsSpan = $("<span>...</span>");
                $noPlanDocsHeader.addClass("docs");
                $noPlanDocsHeader.attr({ "title": "Klicka för att expandera" });
                $noPlanDocsHeader.append("Ej enskilt upprättade dokument");
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



                // Ansluter event expandering/komprimering av dokumenttyper utan filer
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
        //url: '<%#ResolveClientUrl("~/plandokument.asmx/getDocsZipped")%>',
        url: urlBasePath + 'plandokument.asmx/getDocsZipped',
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
    var newDivExpCol = $("<div id='expcol'></div>");
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


            $contentDivs.each(function () {
                var $contentDiv = $(this);
                var planid = $contentDiv.prev().attr("planid");
                putPlanContentHolder($contentDiv, planid);
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
        url: urlBasePath + 'plandokument.asmx/getPlanMapImageAsBase64String',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        data: "{planID: '" + planID + "', imageWidth: '" + imageWidth + "', imageHeight: '" + imageHeight + "'}",
        success: function (msg) {
            if (msg.d != '') {
                var imageObject = eval(msg.d);
                if (imageObject)
                {
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
                else
                {
                    $('#map-' + planID).find('img').attr("src", urlBasePath + "pic/no-image.png");
                    $('#map-' + planID).find('span').html(errorMessage);
                }
            }
        },
        error: function (jqxhr, status, error) {
            $('#map-' + planID).find('img').attr("src", urlBasePath + "pic/no-image.png");
            $('#map-' + planID).find('span').html(errorMessage);
            var err = eval(jqxhr);
            console.log(err.status + " " + err.statusText + "\n" +
                        err.statusCode);
        },
        complete: function () {
        }
    })
}; // SLUT putMapOfPlan