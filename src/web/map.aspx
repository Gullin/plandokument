<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="map.aspx.cs" Inherits="Plan.Plandokument.map" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Karta med planer</title>
    <link rel="icon" type="image/x-icon" href="~/pic/favicon.ico" />

    <link href="lib/bootstrap-4.1.2-dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="lib/bootstrap-icons-1.13.1/font/bootstrap-icons.min.css" rel="stylesheet" />
    <%--<link href="css/reset.css" rel="stylesheet" />--%>
    <link href="css/page-UI-core.css" rel="stylesheet" />
    <link href="css/page-UI-kontrollpanel.css" rel="stylesheet" />
    <link href="css/page-UI-popover.css" rel="stylesheet" />
    <link href="lib/ol-10.6.1/ol.css" rel="stylesheet" />
    <link href="lib/ol-ext-4.0.33/ol-ext.css" rel="stylesheet" />
    <link href="css/page-UI-map.css" rel="stylesheet" />

    <style>
        * {
            box-sizing: border-box;
        }

        html,
        body {
            height: 100%;
            margin: 0;
            font-size: 1em;
            line-height: 1em;
            font-family: Verdana, Arial, sans-serif;
        }
        p {
            margin: 0;
        }

        #versionWrapper {
            font-size: 0.65em;
        }

        #copyrightWrapper {
            left: 1em;
            font-size: 0.65em;
        }

        .ol-zoom-extent-map {
            position: absolute;
            left: 1em;
            top: 1em;
        }

        .ol-zoom-extent-object {
            position: absolute;
            left: 1em;
            top: 3em;
        }

        .ol-fullscreen {
            position: absolute;
            right: 1em;
            bottom: 1em;
        }

        .ol-popup-content {
            font-size: 0.7em;
            line-height: 1em;
        }

        .ol-popup-content h6 {
            font-size: 1em;
            line-height: 1em;
            margin: 0;
            padding: 0;
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

    <script src="<%= ResolveUrl("~/") %>js/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/") %>lib/bootstrap-4.3.1-dist/js/bootstrap.bundle.min.js"></script>
    <script src='<%= ResolveUrl("~/lib/proj4-2.19.3/proj4.js")%>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/lib/ol-10.6.1/ol.js")%>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/lib/ol-ext-4.0.33/ol-ext.js")%>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/js/utility.js")%>' type="text/javascript"></script>

    <script src='<%= ResolveUrl("~/js/ol-mapbackground.js")%>' type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="mapcontainer" style="position: relative; min-height: 100vh;"></div>



            <div id="versionWrapper">
                <asp:PlaceHolder runat="server">
                    <a href="<%= ResolveUrl("~/") %>dokument/version" title="Om versioner och historik">
                        <asp:Label ID="lblVersion" runat="server"></asp:Label>
                    </a>
                </asp:PlaceHolder>
            </div>

            <div id="feedback">
                <a href="<%= ResolveUrl("~/") %>dokument/alla" title="Lista alla planer från Planregistret">
                    <i id="allPlanes" class="bi bi-list"></i>
                </a>
                <a href="<%= ResolveUrl("~/") %>dokument/om" title="Beskrivning av funktionalitet och utseende">
                    <i id="info" class="bi bi-info-circle"></i>
                </a>
                <a href="mailto:gis@landskrona.se?Subject=Webbapplikation Plandokument" title="Lämna synpunkter eller rapportera fel">
                    <i id="mail" class="bi bi-envelope"></i>
                </a>
            </div>

            <div id="copyrightWrapper">
                2013 -
                <asp:Label ID="lblCopyrightYear" runat="server" />
            </div>
        </div>
    </form>

    <script>
        const map = new MapBackground('mapcontainer');
        map.addAllPlans(true);
    </script>
</body>
</html>
