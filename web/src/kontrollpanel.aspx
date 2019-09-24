﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kontrollpanel.aspx.cs" Inherits="Plan.Plandokument.kontrollpanel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administrering plandokumentsapplikation</title>

    <link href="lib/bootstrap-4.3.1-dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/reset.css" rel="stylesheet" />
    <link href="css/page-UI-core.css" rel="stylesheet" />
    <link href="css/page-UI-kontrollpanel.css" rel="stylesheet" />
    <style type="text/css">
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

    <script type="text/javascript">
        function modal() {
            $('.modal').modal('show');
            setTimeout(function () {
                $('.modal').modal('hide');
            }, 3000);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>


            <ul class="nav nav-tabs" id="menu" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="tab-overview" data-toggle="tab" href="#overview" role="tab" aria-controls="overview" aria-selected="true">Översikt</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="tab-logs" data-toggle="tab" href="#logs" role="tab" aria-controls="logs" aria-selected="false">Loggar</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="tab-cache" data-toggle="tab" href="#cache" role="tab" aria-controls="cache" aria-selected="false">Cache</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="tab-validate" data-toggle="tab" href="#validate" role="tab" aria-controls="validate" aria-selected="false">Kontroll</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="tab-thumnails" data-toggle="tab" href="#thumnails" role="tab" aria-controls="thumnails" aria-selected="false">Thumnails</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="tab-system" data-toggle="tab" href="#system" role="tab" aria-controls="system" aria-selected="false">System</a>
                </li>
            </ul>

            <div class="tab-content" id="menu-content">
                <div class="tab-pane fade show active" id="overview" role="tabpanel" aria-labelledby="tab-overview">...</div>
                <div class="tab-pane fade" id="logs" role="tabpanel" aria-labelledby="tab-logs">...</div>
                <div class="tab-pane fade" id="cache" role="tabpanel" aria-labelledby="tab-cache">...</div>
                <div class="tab-pane fade" id="validate" role="tabpanel" aria-labelledby="tab-validate">...</div>
                <div class="tab-pane fade" id="thumnails" role="tabpanel" aria-labelledby="tab-thumnails">...</div>
                <div class="tab-pane fade" id="system" role="tabpanel" aria-labelledby="tab-system">...</div>
            </div>



            <button type="button" class="btn btn-primary" onclick="modal();">Open and close in 3 secs</button>

            <div class="modal" data-backdrop="static" data-keyboard="false" tabindex="-1">
                <div class="modal-dialog modal-spinner-width modal-dialog-centered" style="text-align: center;">
                    <div class="modal-content">
                        <div class="spinner-border text-primary" style="width: 48px; height: 48px;" role="status">
                            <span class="sr-only">Loading...</span>
                        </div>
                    </div>
                </div>
            </div>




            <div id="versionWrapper">
                <asp:PlaceHolder runat="server">
                    <a href="<%= ResolveUrl("~/") %>dokument/version" title="Om versioner och historik">
                        <asp:Label ID="lblVersion" runat="server"></asp:Label>
                    </a>
                </asp:PlaceHolder>
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
                2013 - <asp:Label ID="lblCopyrightYear" runat="server" /> 
            </div>


        </div>
    </form>
</body>
</html>
