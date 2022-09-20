<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kontrollpanel.aspx.cs" Inherits="Plan.Plandokument.kontrollpanel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administrering plandokumentsapplikation</title>
    <link rel="icon" type="image/x-icon" href="~/pic/favicon.ico" />

    <link href="lib/bootstrap-4.3.1-dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/reset.css" rel="stylesheet" />
    <link href="css/page-UI-core.css" rel="stylesheet" />
    <link href="css/page-UI-kontrollpanel.css" rel="stylesheet" />
    <link href="css/page-UI-popover.css" rel="stylesheet" />
    <style type="text/css">
        h3 {

        }
        .right {
            text-align: right;
        }
        table {
            border-spacing: 0px;
            border-collapse: separate;
            /*border: 1px solid red;*/
        }
        thead {
            font-weight: bold;
        }
        td {
            /*border: 1px solid red;*/
        }
        .tr-selected {
            background-color: rgb(224, 242, 255)
        }
        .center {
            text-align: center;
            background-position: center;
            align-content: center;
            -webkit-align-content: center;
            margin: 0px auto;
            vertical-align: middle;
        }
        .clear {
            content: url('../lib/octicons-9.1.1/check.svg');
            color: green;
            fill: green;
            filter: invert(48%) sepia(79%) saturate(2476%) hue-rotate(86deg) brightness(118%) contrast(119%);
            transform: scale(1.5);
        }
        .no {
            content: url('../lib/octicons-9.1.1/circle-slash.svg');
            color: red;
            fill: red;
            /*filter: grayscale(100%) sepia(100%) saturate(600%) hue-rotate(-50deg) brightness(40%) contrast(0.8);*/
            filter: invert(27%) sepia(51%) saturate(2878%) hue-rotate(346deg) brightness(104%) contrast(97%);
            transform: scale(1.5);
        }
        .peek {
            content: url('../lib/octicons-9.1.1/eye.svg');
            transform: scale(0.75);
        }
        .error {
            color: red;
        }
        .amplify {
            font-weight: bolder;
        }

        .modal {
            background-color: rgba(255,255,255,0.5);
        }

        .modal-dialog .modal-content {
            background-color: transparent;
            border: none;
        }

        .modal-spinner-width {
            width: 48px;
        }

        .tab-pane {
            margin: 0 3em;
        }

        .spinner-hide {
            display: none;
        }

        .spinner-visible {
            display: inline;
        }


        ul {
            list-style-type: disc;
            list-style-position: inside;
            margin: 0 2em;
        }

        ol {
            list-style-type: decimal;
            list-style-position: inside;
            margin: 0 2em;
        }

            ul ul, ol ul {
                list-style-type: circle;
                list-style-position: inside;
                margin-left: 15px;
            }

            ol ol, ul ol {
                list-style-type: lower-latin;
                list-style-position: inside;
                margin-left: 15px;
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
    <script src="<%= ResolveUrl("~/") %>lib/popper.js-1.14.7/popper.min.js" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/") %>lib/bootstrap-4.3.1-dist/js/bootstrap.bundle.min.js"></script>
    <!-- Misc -->
    <script src="<%= ResolveUrl("~/") %>js/utility.js" type="text/javascript"></script>

    <script src="<%= ResolveUrl("~/") %>js/kontrollpanel.js" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/") %>js/kontrollpanel-cache.js" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/") %>js/kontrollpanel-service.js" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/") %>js/kontrollpanel-thumnails.js" type="text/javascript"></script>
    <script type="text/javascript">


    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>


            <ul class="nav nav-tabs" id="menu" role="tablist">
<%--                <li class="nav-item">
                    <a class="nav-link active" id="tab-overview" data-toggle="tab" href="#overview" role="tab" aria-controls="overview" aria-selected="true">Översikt</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="tab-logs" data-toggle="tab" href="#logs" role="tab" aria-controls="logs" aria-selected="false">Loggar</a>
                </li>--%>
                <li class="nav-item">
                    <a class="nav-link" id="tab-cache" data-toggle="tab" href="#cache" role="tab" aria-controls="cache" aria-selected="false">Cache</a>
                </li>
<%--                <li class="nav-item">
                    <a class="nav-link" id="tab-validate" data-toggle="tab" href="#validate" role="tab" aria-controls="validate" aria-selected="false">Kontroll</a>
                </li>--%>
                <li class="nav-item">
                    <a class="nav-link" id="tab-thumnails" data-toggle="tab" href="#thumnails" role="tab" aria-controls="thumnails" aria-selected="false">Miniatyrbilder</a>
                </li>
<%--                <li class="nav-item">
                    <a class="nav-link" id="tab-system" data-toggle="tab" href="#system" role="tab" aria-controls="system" aria-selected="false">System</a>
                </li>--%>
            </ul>

            <div class="tab-content" id="menu-content">
                <%--Översikt--%>
<%--                <div class="tab-pane fade show active" id="overview" role="tabpanel" aria-labelledby="tab-overview">
                    Kommer senare
                </div>--%>


                <%--Logg--%>
<%--                <div class="tab-pane fade" id="logs" role="tabpanel" aria-labelledby="tab-logs">
                    Kommer senare
                </div>--%>


                <%--Cache--%>
                <div class="tab-pane fade" id="cache" role="tabpanel" aria-labelledby="tab-cache">
                    <p>
                        Systemet har fem informationsdelar i cache för snabbare svarstider.
                        </p>
                    <hr />
                    <h3>
                        Automatisk förnyande av cache
                    </h3>
                    <p>
                        Sker
                    </p>
                    <ul>
                        <li><asp:Label ID="NyCacheEfterAntalDagar" runat="server"></asp:Label></li>
                        <li><asp:Label ID="NyCacheKlockan" runat="server"></asp:Label></li>
                    </ul>

                    <hr />

                    <h3>
                        Metadata
                    </h3>
                    <!-- Byggs upp av javascript //-->
                    <div id="CacheMeta">

                    </div>

                    <hr />

                    <h3>
                        Förnya Cache
                    </h3>
                    <p>
                        Sker förändringar av den underliggande informationen och som ska slå igenom innan systemet gör en egen schemalagd cache-ning kan manuell om-cache-ning göras nedan.
                    </p>
                    <table id="ReCacheTable">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Existerar</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="right">Grundläggande planregisterinformation</td>
                                <td class="center"></td>
                                <td>
                                    <button id="btnRefreshCachePlan" class="btn btn-primary btn-sm" type="button" onclick="RefreshCachePlanBasis(this)">
                                        <span class="spinner-border spinner-border-sm spinner-hide" role="status" aria-hidden="true"></span>
                                        <span>Förnya cache</span>
                                    </button>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Plandokument</td>
                                <td class="center"></td>
                                <td>
                                    <button id="btnRefreshCachePlanDocuments" class="btn btn-primary btn-sm" type="button" onclick="RefreshCachePlanDocuments(this)">
                                        <span class="spinner-border spinner-border-sm spinner-hide" role="status" aria-hidden="true"></span>
                                        <span>Förnya cache</span>
                                    </button>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Dokumenttyper</td>
                                <td class="center"></td>
                                <td>
                                    <button id="btnRefreshCacheDocumenttypes" class="btn btn-primary btn-sm" type="button" onclick="RefreshCachePlandocumenttypes(this)">
                                        <span class="spinner-border spinner-border-sm spinner-hide" role="status" aria-hidden="true"></span>
                                        <span>Förnya cache</span>
                                    </button>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Planers berörkrets (fastigheter relation till plan)</td>
                                <td class="center"></td>
                                <td>
                                    <button id="btnRefreshCachePlanBerorFastighet" class="btn btn-primary btn-sm" type="button" onclick="RefreshCachePlanBerorFastighet(this)">
                                        <span class="spinner-border spinner-border-sm spinner-hide" role="status" aria-hidden="true"></span>
                                        <span>Förnya cache</span>
                                    </button>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Planpåverkan (planers relation)</td>
                                <td class="center"></td>
                                <td>
                                    <button id="btnRefreshCachePlanBerorPlan" class="btn btn-primary btn-sm" type="button" onclick="RefreshCachePlanBerorPlan(this)">
                                        <span class="spinner-border spinner-border-sm spinner-hide" role="status" aria-hidden="true"></span>
                                        <span>Förnya cache</span>
                                    </button>
                                </td>
                            </tr>
                        </tbody>
<%--                        <tfoot>
                            <tr>
                                <td></td>
                                <td colspan="2">
                                    <button id="btnRefreshCacheAll" style="width: 100%;" class="btn btn-primary btn-sm" type="button" onclick="RefreshCachePlanAll(this)">
                                        <span class="spinner-border spinner-border-sm spinner-hide" role="status" aria-hidden="true"></span>
                                        <span>Förnya ALLA cacher</span>
                                    </button>
                                </td>
                            </tr>
                        </tfoot>--%>
                    </table>
                </div>


                <%--Kontroll / Validera--%>
<%--                <div class="tab-pane fade" id="validate" role="tabpanel" aria-labelledby="tab-validate">
                    Kommer senare
                </div>--%>


                <%--ThumNails--%>
                <div class="tab-pane fade" id="thumnails" role="tabpanel" aria-labelledby="tab-thumnails">

                    <div id="ThumnailsServiceContent" style="display: none;">
                        <p>
                            Respektive plans plankarta lagras som pdf-fil och georefererad tiff-fil. Tiff-filen konverteras till två generella bilder, en större och en mindre,
                        som översikter.
                            Konverteringen till mindre bilder sker med automatik av en Windows-tjänst.
                            Uppgifter om och enklare administrering av denna Windows-tjänst kan hittas nedan.
                        </p>

                        <hr />

                        <h3>
                            Konfigurerade Windows Tjänsteuppgifter
                        </h3>
                        <div id="ServiceMeta">
                        </div>

                        <hr />

                        <h3>
                            Status
                        </h3>
                        <p>
                            Kör: <span id="status">N/A</span>
                        </p>

                        <button id="btnServiceRestart" class="btn btn-primary btn-sm" type="button" onclick="ServiceRestart(this)">
                            <span class="spinner-border spinner-border-sm spinner-hide" role="status" aria-hidden="true"></span>
                            <span>Starta om tjänst</span>
                        </button>

                        <button id="btnServiceStart" class="btn btn-primary btn-sm" type="button" onclick="ServiceStart(this)">
                            <span class="spinner-border spinner-border-sm spinner-hide" role="status" aria-hidden="true"></span>
                            <span>Starta tjänst</span>
                        </button>

                        <button id="btnServiceStop" class="btn btn-primary btn-sm" type="button" onclick="ServiceStop(this)">
                            <span class="spinner-border spinner-border-sm spinner-hide" role="status" aria-hidden="true"></span>
                            <span>Stoppa tjänst</span>
                        </button>
                        <p style="font-size: 0.8em;">
                            För att ovan funktioner ska fungera behöver webbapplikationen köras av användare på server som har administratörsrättigheter.
                            <br />
                            Hanteras enklast med att ändra för applikationspoolen på servern under "Avancerade inställningar >> Processmodell >> Identitet" till "LocalSystem".
                        </p>
                    </div>

                    <div id="ThumnailsContent" style="display: none; position: relative;">

                    </div>

                </div>


                <%--System--%>
<%--                <div class="tab-pane fade" id="system" role="tabpanel" aria-labelledby="tab-system">
                    Kommer senare
                </div>--%>
            </div>





<%--            <button type="button" style="position: absolute; bottom: 5px;" class="btn btn-outline-primary" onclick="modal();">Open spinner, close in 3 secs</button>

            <div class="modal" data-backdrop="static" data-keyboard="false" tabindex="-1">
                <div class="modal-dialog modal-spinner-width modal-dialog-centered" style="text-align: center;">
                    <div class="modal-content">
                        <div class="spinner-border text-primary" style="width: 48px; height: 48px;" role="status">
                            <span class="sr-only">Loading...</span>
                        </div>
                    </div>
                </div>
            </div>--%>




            <div id="versionWrapper">
                <asp:PlaceHolder runat="server">
                    <a href="<%= ResolveUrl("~/") %>dokument/version" title="Om versioner och historik">
                        <asp:Label ID="lblVersion" runat="server"></asp:Label>
                    </a>
                </asp:PlaceHolder>
            </div>

            <div id="user" class="hand">
                <asp:Label ID="lblUser" ToolTip="Användare kunde inte identifieras fullt ut" runat="server">[ N/ A]</asp:Label>
            </div>

            <div id="feedback">
                <a href="<%= ResolveUrl("~/") %>dokument/alla" title="Lista alla planer från Planregistret">
                    <img id="listAllPlanes" src="<%= ResolveUrl("~/") %>pic/list_all_planes.png" onmouseover="hover(this);" onmouseout="unhover(this);" /></a>
                <a href="<%= ResolveUrl("~/") %>dokument/om" title="Beskrivning av funktionalitet och utseende">
                    <img id="help" src="<%= ResolveUrl("~/") %>pic/help.png" onmouseover="hover(this);" onmouseout="unhover(this);" /></a>
                <a href="mailto:gis@landskrona.se?Subject=Webbapplikation Plandokument" title="Lämna synpunkter eller rapportera fel">
                    <img id="mail" src="<%= ResolveUrl("~/") %>pic/mail.png" onmouseover="hover(this);" onmouseout="unhover(this);" /></a>
            </div>

            <div id="copyrightWrapper">
                2013 -
                <asp:Label ID="lblCopyrightYear" runat="server" />
            </div>


        </div>
    </form>
</body>
</html>
