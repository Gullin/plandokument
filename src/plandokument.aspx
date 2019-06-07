<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="plandokument.aspx.cs" Inherits="Plan.Plandokument.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

    <head runat="server">
        <title>Plandokument</title>
        <link href="css/jquery-ui-1.10.3-smoothness/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" />
        <link href="lib/bootstrap-4.1.2-dist/css/bootstrap.min.css" rel="stylesheet" />
        <link href="css/reset.css" rel="stylesheet" />
        <link href="css/page-UI-core.css" rel="stylesheet" />
        <link href="css/page-UI-plan.css" rel="stylesheet" />
        <link href="css/file-images-li.css" rel="stylesheet" />
        <style type="text/css">
            .btn-group > .btn, .btn-xs {
                padding: .25rem .4rem;
                /*font-size: .875rem;*/
                font-size: 1em;
                /*line-height: .5;*/
                border-radius: .2rem;
            }
        </style>

        <!-- Inställningar Klient -->
        <script src='<%=ResolveUrl("js/config.js")%>'></script>
        <script type="text/javascript">

			if (location.protocol == 'https:') {
				Lkr.Plan.Dokument.resolvedClientUrl = "https://" + '<%=Request.Url.Host%>' + '<%=ResolveUrl("~")%>';
			} else if (location.protocol == 'http:') {
				Lkr.Plan.Dokument.resolvedClientUrl = "http://" + '<%=Request.Url.Host%>' + '<%=ResolveUrl("~")%>';
			} else {
				console.error("Protokoll " + location.protocol + " stöds ej.");
            }

        </script>


        <!-- För haneringar av IE 7 och tidigare samt IE:s modernare version i inställda i kompabilitetsläge -->
        <script src='<%#ResolveClientUrl("~/js/json3.min.js")%>' type="text/javascript"></script>
        <script src='<%#ResolveClientUrl("~/js/jquery-3.4.1.min.js")%>' type="text/javascript"></script>
    	<script src='<%#ResolveClientUrl("~/lib/bootstrap-4.1.2-dist/js/bootstrap.bundle.min.js")%>'></script>
        <script src='<%#ResolveClientUrl("~/js/utility.js")%>' type="text/javascript"></script>
        <!-- Används vid utveckling -->
        <script src='<%#ResolveClientUrl("~/js/planInfoDokument.js")%>' type="text/javascript"></script>
        <script type="text/javascript">


            //var nsp = Lkr.Plan.Dokument;


            $(document).ready(function () {
                // Redan startade ajax-request som ej gett svar avslutas innan initiering av nya (motverkar ev. dubblering av information
                // Görs endast för de ajax-anrop som har för avsikt att komplettera den generella planinformation
                if (Lkr.Plan.AjaxCalls.putMapOfPlan != undefined) {
                    Lkr.Plan.AjaxCalls.putMapOfPlan.abort();
                }
                if (Lkr.Plan.AjaxCalls.getPlansDocs != undefined) {
                    Lkr.Plan.AjaxCalls.getPlansDocs.abort();
                }


                $('#planLista').hide();
                $('#planSideWrapper').hide();
                //Lkr.Plan.Dokument.currentWindowSizeHeight = window.innerHeight;
                //Lkr.Plan.Dokument.currentWindowSizeWidth = window.innerWidth;

                nbrOfSearchedPlans(Lkr.Plan.Dokument.isPlansSearched, function (isPlansSearched) {
                    if (isPlansSearched) {
                        getDocumenttypes(function (Dokumenttyper) {
                            if (Dokumenttyper) {
                                documentConditionOfSearchedPlans();
                                columnConditionOfSearchedPlans();
                                searchedPlans();

                                getStatTotNbrPlans()
                                getStatNbrPlanTypes();
                                getStatNbrPlanImplement();

                                getSearchedPlans();
                            }
                        });
                    }
                });


                // Förändring kartstorlek
                //Kartbild hämtas efter
                //1) en viss tid och
                //2) bara om förändringen av fönsterstorleken påverkar bilden avseevärt, mätt separat för höjd och bredd
                var resizeTimer;
                
                $(window).resize(function () {
                    if (resizeTimer) {
                        clearTimeout(resizeTimer);
                    }

                    resizeTimer = setTimeout(function () {
                        resizeTimer = null;

                        var planId = null;
                        var mapImageWidth = null;
                        var mapImageHeight = null;
                        //console.clear();
                        $('#planLista').children('h3').each(function () {
                            planId = $(this).attr("planid");
                            var resize = false;
                            var resizeTolerance = 0.1;
                            var widthDiff = 0;
                            var heightDiff = 0;
                            var dimensions = $('#map-' + planId).attr('dimension');
                            var dimension = []
                            if (dimensions != undefined) {
                                dimension = dimensions.split(',');
                            } else {
                                dimension[0] = 0;
                                dimension[1] = 0;
                            }

                            // Hämtar maximal bredd och höjd som är möjlig för kartbild
                            mapImageWidth = $('#map-' + planId).width();
                            mapImageHeight = $('#doc-' + planId).outerHeight();

                            // Kontrollerar förändringsgrad i storlek på platshållaren
                            if (mapImageWidth > dimension[0]) {
                                widthDiff = mapImageWidth - dimension[0];
                            } else {
                                widthDiff = dimension[0] - mapImageWidth;
                            }
                            if (mapImageHeight > dimension[1]) {
                                heightDiff = mapImageHeight - dimension[1];
                            } else {
                                heightDiff = dimension[1] - mapImageHeight;
                            }
                            if ((widthDiff / mapImageWidth) >= Lkr.Plan.Setting.Map.mapResizeTolerance || (heightDiff / mapImageHeight) >= Lkr.Plan.Setting.Map.mapResizeTolerance) {
                                resize = true;
                            }


                            // Om tolleransnivån för förändring av storlek på platshållare överstigits
                            if (resize) {
                                // Lagrar dimensioner för mätning av förändring när webbläsarfönster ändras i storlek
                                $('#map-' + planId).attr('dimension', mapImageWidth + "," + mapImageHeight)
                                // Drar bort för 10 % marginal till kartöversikt (20 % / 2)
                                mapImageWidth = mapImageWidth - Math.round(mapImageWidth * 0.2);
                                mapImageHeight = mapImageHeight - Math.round(mapImageHeight * 0.2);
                                // Hämtar renderad kartbild med plan
                                putMapOfPlan(planId, mapImageWidth, mapImageHeight);
                            }
                        });

                    }, Lkr.Plan.Setting.Map.mapResizeTimespan);
                });
                

                // Kontrollerar om variabeln är jQuery-objekt
                function isjQuery(obj) {
                    if (obj instanceof jQuery) {
                        alert("jQuery object")
                    } else {
                        alert("! jQuery");
                    }
                }

            }); // SLUT $(document).ready


            function hover(element) {

                var filePathPart = splitFilePath(element.getAttribute('src'));

                element.setAttribute('src', window.location.protocol + '//' + window.location.hostname + '/app/plan/pic/' + filePathPart[1] + '-invers.' + filePathPart[2]);

            }

            function unhover(element) {

                var filePathPart = splitFilePath(element.getAttribute('src'));
                var fileName = filePathPart[1].substring(0, filePathPart[1].lastIndexOf('-'));

                element.setAttribute('src', filePathPart[0] + '/' + fileName + '.' + filePathPart[2]);

            }

            function splitFilePath(filePath) {
                var fullFilePath = filePath;
                var path = fullFilePath.substring(0, fullFilePath.lastIndexOf('/'));
                var fullFileName = fullFilePath.substring(fullFilePath.lastIndexOf('/') + 1);
                var fileName = fullFileName.substring(0, fullFileName.lastIndexOf('.'));
                var fileExtension = fullFileName.substring(fullFileName.lastIndexOf('.') + 1);

                return [path, fileName, fileExtension]
            }

        </script>
    </head>

    <body>
        <form id="form1" runat="server">
            <asp:Label ID="logg" runat="server"></asp:Label>

            <div>
                <div id="planLista">
                </div>
                <div id="planSideWrapper">
                    <h4>Sökparametrar</h4>
                    <div id="planSearch">
                        <span id="searchResultNbr"></span>
                        <span id="searchCriteria"></span>
                        <span id="searchDocs"></span>
                        <span id="searchString"></span>
                    </div>
                    <h4>Sökresultat</h4>
                    <div id="planSearchResult">
                        <span id="statNbrPlanHits"></span>
                        <span id="statNbrPlanBoms"></span>
                        <span id="Span3"></span>
                    </div>
                    <h4>Allmän planstatistik</h4>
                    <div id="planAllm">
                        <span id="statNbrPlans"></span>
                        <span id="statNbrPlanTypes"></span>
                        <span id="statNbrPlanImpImplement"></span>
                    </div>
                </div>
            </div>

            <div id="versionWrapper">
                <asp:PlaceHolder runat="server">
                    <a href="<%= ResolveUrl("~/") %>versioninfo.aspx" title="Om versioner och historik">
                        <asp:Label ID="lblVersion" runat="server"></asp:Label>
                    </a>
                </asp:PlaceHolder>
            </div>

            <div id="feedback">
                <a href="<%= ResolveUrl("~/") %>dokument/alla" title="Lista alla planer från Planregistret">
                    <img id="listAllPlanes" src="<%= ResolveUrl("~/") %>pic/list_all_planes.png" onmouseover="hover(this);" onmouseout="unhover(this);" /></a>
                <a href="<%= ResolveUrl("~/") %>dokument/om" title="Beskrivning av funktionalitet och utseende">
                    <img id="help" src="<%= ResolveUrl("~/") %>pic/help.png" onmouseover="hover(this);" onmouseout="unhover(this);" /></a>
                <a href="mailto:gis@landskrona.se?Subject=Webbapplikation Plandokumentation" title="Lämna synpunkter eller rapportera fel">
                    <img id="mail" src="<%= ResolveUrl("~/") %>pic/mail.png" onmouseover="hover(this);" onmouseout="unhover(this);" /></a>
            </div>

            <div id="copyrightWrapper">
                2013 - <asp:Label ID="lblCopyrightYear" runat="server" /> 
            </div>
        </form>
    </body>

</html>
