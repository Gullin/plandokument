<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="planlist.aspx.cs" Inherits="Plan.Plandokument.planlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Alla planer</title>
    <link href="css/reset.css" rel="stylesheet" />
    <link href="css/page-UI-core.css" rel="stylesheet" />
    <link href="css/page-UI-centering.css" rel="stylesheet" />
    <link href="css/page-UI-plan-alla.css" rel="stylesheet" />
    <link href="css/file-images-li.css" rel="stylesheet" />
    <!-- jTable -->
    <link href="js/jtable.2.4.0/themes/landskrona/metro-lightgray/jtable.css" rel="stylesheet" />
    <link href="css/page-UI-plan-alla-jtable-custom.css" rel="stylesheet" />

    <!-- Inställningar Klient -->
    <script src='<%=ResolveUrl("js/config.js")%>'></script>
    
    <!-- För haneringar av IE 7 och tidigare samt IE:s modernare version i inställda i kompabilitetsläge -->
    <script src='<%#ResolveClientUrl("~/js/json3.min.js")%>' type="text/javascript"></script>

    <!-- jQuery -->
    <script src='<%=ResolveUrl("~/js/jquery-1.9.0.min.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("js/jquery-ui-1.9.2.min.js")%>'></script>

    <!-- Misc -->
    <script src='<%=ResolveClientUrl("~/js/utility.js")%>' type="text/javascript"></script>

    <!-- jTable -->
    <script src='<%=ResolveUrl("js/jtable.2.4.0/jquery.jtable.min.js")%>'></script>    
    <script src='<%=ResolveUrl("js/jtable.2.4.0/extensions/jquery.jtable.aspnetpagemethods.min.js")%>'></script>
    <script src='<%=ResolveUrl("js/jtable.2.4.0/localization/jquery.jtable.se.js")%>'></script>

    <script type="text/javascript">

        console.log(Lkr);

        urlBasePath = '<%=ResolveClientUrl("~")%>';

        $(document).ready(function () {

            //Prepare jtable plugin
            $('#PlanTableContainer').jtable({
                title: 'Planer',
                sorting: true, //Enables sorting
                defaultSorting: 'DatBeslut DESC', //Optional. Default sorting on first load.
                openChildAsAccordion: true,
                //selecting: true,
                //multiselect: true,
                //selectingCheckboxes: true,
                ajaxSettings: {
                    type: 'POST',
                    contentType: "application/json; charset=UTF-8",
                    dataType: 'json',
                    data: { checkHasDocument: false }
                },
                actions: {
                    listAction: '<%=ResolveUrl("~/plandokument.asmx/jtGetAllPlanInfo")%>'
                },
                fields: {
                    DatBeslut: {
                        type: 'hidden'
                    },
                    Nyckel: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    PlanFk: {
                        title: 'Typ',
                        width: '5%',
                        listClass: 'testRow'
                    },
                    Akt: {
                        title: 'Aktbeteckning'
                    },
                    AktEgen: {
                        title: 'Bygglov'
                    },
                    AktTidigare: {
                        title: 'f.d. Stadsingenjör'
                    },
                    PlanNamn: {
                        title: 'Namn',
                        width: '40%'
                    },
                    HasDocument: {
                        title: '',
                        width: '1%',
                        sorting: true,
                        listClass: 'MiddleCenterTD',
                        display: function (planData) {
                            var $img = $("<img>");
                            if (planData.record.HasDocument || planData.record.HasDocument == null) {
                                $img.attr("src", urlBasePath + "pic/planDocuments.png");
                                $img.attr("title", "Klicka för plandokument");
                                $img.attr("class", 'imgPlanDokument hand');
                                $img.click(function () {
                                    var docsOfPlan = [planData.record.Nyckel.toString()];

                                    $('#PlanTableContainer').jtable('openChildTable',
                                        $img.closest('tr'),
                                        {
                                            title: 'Plandokument för ' + planData.record.Akt,
                                            toolbar: {
                                                items: [{
                                                    icon: urlBasePath + 'pic/noun_4501_16x14_white.png',
                                                    text: '',
                                                    tooltip: 'Paketera valda filer och ladda ner',
                                                    click: function () {
                                                        var $selectedRows = $('.jtable-child-table-container').jtable('selectedRows');
                                                        if ($selectedRows.length > 0) {
                                                            getFilesZipped($selectedRows, planData.record.Akt);
                                                        } else {
                                                            alert("Inga dokument valda!");
                                                        }
                                                    }
                                                }]
                                            },
                                            selecting: true,
                                            multiselect: true,
                                            selectingCheckboxes: true,
                                            selectOnRowClick: true,
                                            ajaxSettings: {
                                                type: 'POST',
                                                contentType: "application/json; charset=UTF-8",
                                                dataType: 'json',
                                                data: { planIds: docsOfPlan }
                                            },
                                            actions: {
                                                listAction: '<%=ResolveUrl("~/plandokument.asmx/jtGetPlansDocs")%>'
                                            },
                                            fields: {
                                                files: {
                                                    title: '',
                                                    display: function (planDocs) {
                                                        console.log(planDocs);
                                                        var $ul = $("<ul>");
                                                        var $li = $("<li>");
                                                        var fileExtention = planDocs.record.Extention;
                                                        $li.addClass(fileExtention.substring(1, fileExtention.length) + "-file");

                                                        var $docLink = $("<a>");
                                                        var href = urlBasePath + planDocs.record.Path + planDocs.record.Name;
                                                        $docLink.attr("href", href);
                                                        $docLink.attr("target", "_blank");
                                                        $docLink.attr("title", planDocs.record.Name);
                                                        $docLink.text(planDocs.record.DocumentType + " (" + bytesToSize(planDocs.record.Size) + ")");

                                                        $li.append($docLink);
                                                        $ul.append($li);
                                                        return $ul;
                                                    }
                                                }
                                            }
                                        }, function (data) { //opened handler
                                            data.childTable.jtable('load');
                                        });

                                });
                            } else {
                                $img.attr("src", urlBasePath + "pic/planDocuments-No.png");
                                $img.attr("title", "Inga plandokument");
                            }
                            return $img;
                        }
                    },
                    IsGenomf: {
                        title: '',
                        width: '1%',
                        sorting: true,
                        listClass: 'MiddleCenterTD',
                        display: function (data) {
                            var isGenomf = data.record.IsGenomf;
                            var $img = $('<img />');
                            if (isGenomf == 1) {
                                $img.attr("src", urlBasePath + "pic/knob-orange.png");
                                $img.attr("title", "Inom genomförandetid");
                            } else {
                                $img.attr("src", urlBasePath + "pic/knob-green.png");
                                $img.attr("title", "Genomförandetid löpt ut");
                            }
                            return $img;
                        }
                    }

                }
            });

            //Load student list from server
            $('#PlanTableContainer').jtable('load');
        });


        // Ladda ner dokument zippade via webbtjänst
        function getFilesZipped($selectedRows, zipFileNamePart) {
            toggleLoadingImage(true);

            var files = [];
            $selectedRows.each(function () {
                var record = $(this).data('record');
                files.push(record.Path + record.Name);
            });
            console.log(files);
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
                },
                complete: function () {
                    toggleLoadingImage(false);
                },
                error: function () {
                    alert("Fel!\ngetFilesZipped");
                }
            })
        }; // SLUT getFilesZipped

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="container">
            <div id="PlanTableContainer"></div>



        </div>

        <div id="versionWrapper">
            <asp:PlaceHolder runat="server">
                <a href="<%= ResolveUrl("~/") %>versioninfo.aspx" title="Om versioner och historik">
                    <asp:Label ID="lblVersion" runat="server"></asp:Label>
                </a>
            </asp:PlaceHolder>
        </div>

        <div id="feedback">
            <a href="<%= ResolveUrl("~/") %>om.aspx" title="Beskrivning av funktionalitet och utseende">
                <img src="<%= ResolveUrl("~/") %>pic/help.png" /></a>
            <a href="mailto:gis@landskrona.se?Subject=Webbapplikation Plandokumentation" title="Lämna synpunkter eller rapportera fel">
                <img src="<%= ResolveUrl("~/") %>pic/mail.png" /></a>
        </div>

        <div id="copyrightWrapper">
            2013 -
                <asp:Label ID="lblCopyrightYear" runat="server" />
        </div>

    </form>
</body>
</html>
