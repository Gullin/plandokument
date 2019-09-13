<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="om.aspx.cs" Inherits="Plan.Plandokument.om" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="lib/bootstrap-4.1.2-dist/css/bootstrap.min.css" rel="stylesheet" />

    <link href="css/page-UI-core.css" rel="stylesheet" />
    <link href="css/page-UI-help.css" rel="stylesheet" />
    <title>Plandokument / Dokumentation</title>

    <script src="../js/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script src="../lib/bootstrap-4.1.2-dist/js/bootstrap.bundle.min.js"></script>

    <style>
        body {
            position: relative;
        }

        #menu a {
            border: none;
            padding: 0px 0px 0px 0.5em;
        }

        .list-group-item.active {
            background-color: rgb(155, 193, 235);
        }
    </style>
</head>
<body data-spy="scroll" data-target="#menu">
    <form id="form1" runat="server">
            <div id="contentIndex" class="list-group">
                <ul id="menu">
                    <li><a class="list-group-item list-group-item-action" href="#avsnitt1">Hitta plandokument från</a>
                    <ul>
                        <li><a class="list-group-item list-group-item-action" href="#avsnitt1-1">Karta</a>
                            <ul>
                                <li><a class="list-group-item list-group-item-action" href="#avsnitt1-1-1">Plan</a></li>
                                <li><a class="list-group-item list-group-item-action" href="#avsnitt1-1-2">Fastighet</a></li>
                            </ul>
                        </li>
                        <li><a class="list-group-item list-group-item-action" href="#avsnitt1-2">Adressfält i webbläsare</a>
                            <ul>
                                <li><a class="list-group-item list-group-item-action" href="#avsnitt1-2-1">För alla</a></li>
                                <li><a class="list-group-item list-group-item-action" href="#avsnitt1-2-2">För enskilda</a></li>
                            </ul>
                        </li>
                    </ul>
                    </li>
                    <li><a class="list-group-item list-group-item-action" href="#avsnitt2">Grafiskt användargränssnitt</a>
                        <ul>
                            <li><a class="list-group-item list-group-item-action" href="#avsnitt2-1">För alla</a>
                                <ul>
                                    <li><a class="list-group-item list-group-item-action" href="#avsnitt2-1-1">Enkel listform</a></li>
                                    <li><a class="list-group-item list-group-item-action" href="#avsnitt2-1-2">Listning av plandokument</a></li>
                                </ul>
                            </li>
                            <li><a class="list-group-item list-group-item-action" href="#avsnitt2-2">För enskilda</a>
                                <ul>
                                    <li><a class="list-group-item list-group-item-action" href="#avsnitt2-2-1">Översiktlig panel</a></li>
                                    <li><a class="list-group-item list-group-item-action" href="#avsnitt2-2-2">Plandokument</a></li>
                                </ul>
                            </li>
                            <li><a class="list-group-item list-group-item-action" href="#avsnitt2-3">Planpåverkan</a>

                            </li>
                        </ul>
                    </li>
                    <li><a class="list-group-item list-group-item-action" href="#avsnitt3">Om</a></li>
                    <li><a class="list-group-item list-group-item-action" href="#avsnitt4">Inställningar / konfiguration</a>
                        <ul>
                            <li><a class="list-group-item list-group-item-action" href="#avsnitt4-1">Dokumentbegrepp</a></li>
                            <li><a class="list-group-item list-group-item-action" href="#avsnitt4-2">Settings.config</a></li>
                        </ul>
                    </li>
                    <li><a class="list-group-item list-group-item-action" href="#avsnitt5">För utvecklare</a></li>
                    <li><a href="<%=ResolveUrl("~")%>versioninfo.aspx">Versionsinformation</a><span class="linkNewWindow"> </span></li>
                </ul>
            </div>

            <div id="content">

                <h1 id="avsnitt1">1 Hitta plandokument från</h1>
                <p>
                    Plandokumenten är tillgängliga genom karta eller genom att söka direkt på plan i webbläsarens adressfält.
                </p>


                <h2 id="avsnitt1-1">1.1 Karta</h2>
                <p>
                    Plandokumenten är åtkomliga genom kartorna ”<a href="https://geodata.landskrona.local">Medarbetarkartan</a>” och ”<a href="https://geodata-handlaggare.landskrona.local">Handläggarkartan</a>” samt ”<a href="https://karta.landskrona.se">Landskronakartan</a>” (begränsas till plandokumenten plankarta, planbeskrivning och genomförandebeskrivning i "Landskronakartan"). Åtkomsten sker antingen genom att utgå ifrån planområdet för gällande planer, fastighet (genom planens berörkrets) eller indirekt genom sökning på planbeteckning.
                </p>
                <p>
                    Alternativen nedan kan användas när objekt väljs/markeras i kartan. Plandokument tillhörande en plan åt gången går att ladda ner eller öppna.
                </p>

                <h3 id="avsnitt1-1-1">1.1.1 Plan</h3>
                <p>
                    Lagergruppen ”Planer” innehåller lager ”Gällande planer” (kan ses i nedan exempelbild från "Handläggarkartan"). ”Gällande planer” är ett delvis transparent och valbart lager.
                </p>
                <img src="<%= ResolveUrl("~/") %>pic/help/handlaggarkartan-gallande_planer.png" alt="Karta med gällande planer" />
                <p>För att ladda ner eller öppna plandokumenten gör enligt följande:</p>
                <ol>
                    <li>Se till så att lagergruppen "Planer" och lagret "Gällande planer" är ibockat enligt nedan bild samt att planerna visas som i bilden ovan. <br />
                        <img src="<%= ResolveUrl("~/") %>pic/help/handlaggarkartan-gallande_planer-lager_ibockad.png" alt="Gällande planlager ibockad/tänt" />
                    </li>
                    <li>
                        Flytta kartan (panorera) och närma dig området och planen (zooma in) som är av intresse.
                    </li>
                    <li>
                        Klicka på den lila halvtransparenta planytan. En informationsruta tänds upp och centreras samt plan markeras blå. <br />
                        <img src="<%= ResolveUrl("~/") %>pic/help/handlaggarkartan-gallande_planer-informationsruta.png" alt="Gällande plans utflaggade informationsruta" />
                    </li>
                    <li>
                        Genom att klicka på länken "Plandokument" under avsnittet "Länk till plandokument" görs ett uppslag av existerande dokument tillhörande markerad plan. Uppslaget öppnas i nytt fönster.
                    </li>
                </ol>



                <h3 id="avsnitt1-1-2">1.1.2 Fastighet</h3>
                <p>
                    Lagergruppen ”Fastighet mm” innehåller ytterligare en lagergrupp ”Fastigheter” på samma sätt som för gällande planer i lagergrupp planer (se avsnitt ovan). Genom att använda sig av fastighet som ingång till planer väljs plan beroende av vilka fastigheter en plan berör. En plans s.k. berörkrets är registrerad i fastighetsregistret.
                </p>
                <p>
                    Värt att notera är att fastighetens område är klickbar/markerbar i kartans alla skalor. Fastighetsavgränsning visas först från skala 1:30 000 (skalan kan ses i kartfönstrets nedre högra hörn).
                </p>
                <img src="<%= ResolveUrl("~/") %>pic/help/handlaggarkartan-fastigheter.png" alt="Karta med gällande planer" />
                <p>
                    Om andra lager, exempelvis gällande planer, är tända kan dessa dölja lagret med fastigheter och behöver bockas av/släckas. I annat fall blir inte fastigheterna klickbara/markeringsbara. <br />
                    För att ladda ner eller öppna plandokumenten genom fastighet (via planens berörkrets) gör enligt följande:
                </p>
                
                <ol>
                    <li>Se till så att lagergrupperna "Fastigheter mm" och "Fastigheter" är ibockat enligt nedan bild. <br />
                        <img src="<%= ResolveUrl("~/") %>pic/help/handlaggarkartan-fastigheter-lager_ibockad.png" alt="Gällande plans utflaggade informationsruta" />
                    </li>
                    <li>
                        Sök upp fastigheten. Finns det en osäkerhet var fastigheten är avgränsad, se till så att skalan är större än eller lika med tidigare nämnda 1:30 000 för fastighetsavgränsningen samt större än eller lika med 1:5 000 för fastighetsbeteckningen.
                    </li>
                    <li>Klicka på fastigheten. En informationsruta tänds upp och centreras samt fastigheten markeras blå. <br />
                        <img src="<%= ResolveUrl("~/") %>pic/help/handlaggarkartan-fastigheter-informationsruta.png" alt="Gällande plans utflaggade informationsruta" />
                    </li>
                    <li>Genom att klicka på länken "Plandokument från fastighet (berörskrets)" under avsnittet "Plandokument" görs ett uppslag av existerande dokument tillhörande den plan fastigheten blir berörd av. Uppslaget öppnas i nytt fönster.</li>
                </ol>



                <h2 id="avsnitt1-2">1.2 Adressfält i webbläsare</h2>
                <p>
                    Plandokumenten kan nås, inte bara genom kartan, utan även direkt genom webbläsarens adressfält. Kräver dock att det finns kännedom om planens formella aktbeteckning, stadens f.d. stadsingenjörskontors arkivserie, plan- och bygglovsverksamheternas f.d. arkivserie eller fastighet planen berör. De båda sistnämnda arkivserierna uppstår det inga nya av (för planer beslutade efter år 1999 existerar ingen arkivserie från f.d. stadsingenjörskontoret samt planer beslutade efter år 2012 existerar inte heller någon arkivserie från plan-och bygglovsverksamheterna).
                </p>
                <p>
                    Adressen och stommen till webbtillämpningen är:
                </p>
                <p>Fungerar endast internt inom Landskrona stad.</p>
                <div class="adresSearch">
                    https://geodata.landskrona.local/app/plan/dokument
                </div>
                <p>
                    (lägg denna adress förslagvis som favorit i din webbläsare)
                    <br />
                    efterkommande delar i adressen styr vilka och på vilket sätt plandokument presenteras.
                </p>
                <h3 id="avsnitt1-2-1">1.2.1 För alla</h3>
                <p>
                    För att lista alla gällande och registrerade planer gäller nedan adress. Vardera plans plandokument går att ladda ner. <br />
                    Information som presenteras är typ av plan, aktbeteckning, de äldre arkivserierna, dess plannamn och om planen är inom genomförande tid eller ej. <br />
                    Initialt sorteras listan efter beslutsdatum.
                </p>
                <div class="adresSearch">
                    https://geodata.landskrona.local/app/plan/dokument/alla
                </div>
                <h3 id="avsnitt1-2-2">1.2.2 För enskilda</h3>
                <p>
                    I detta fall finns det tre delar för specificering:
                </p>
                <div class="adresSearch">
                    https://geodata.landskrona.local/app/plan/[1],[2]/[3]
                </div>
                <p>
                    Del [1]
                    <br />
                    obligatorisk del och är en signal om vilket/vilka plandokument som presenteras och är nedladdningsbara efter sökning.<br />
                    <br />
                    Möjliga signaler om dokumenttyp(er) är:<br />
                    * indikerar om plandokumentet ses som formell planhandling.
                </p>
                <table>
                    <tr>
                        <td></td>
                        <td>dokument</td>
                        <td>Är en övergripande signal och selekterar inte endast en dokumenttyp. Presenterar resp. plans alla inskannade plandokument</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>handling</td>
                        <td>Är en övergripande signal och selekterar inte endast en dokumenttyp. Presenterar resp. plans plandokument som enligt planens gällande lagstiftning ses som planhandling och är en direkt del av planen</td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>karta</td>
                        <td>Presenterar resp. plans plankarta</td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>bestammelse</td>
                        <td>Presenterar resp. plans bestämmelser som eget dokument i de fall då bestämmelserna ej ligger i plankartan (majoriteten av fall är de äldre planerna beslutade innan år 1987)</td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>illustration</td>
                        <td>Presenterar resp. plans illustrationskarta i de fall den ligger som eget dokument och inte tillsammans med plankartan</td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>beskrivning</td>
                        <td>Presenterar resp. plans planbeskrivning</td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>genomforande</td>
                        <td>Presenterar resp. plans genomförandebeskrivning</td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>planochgenomforandebeskrivning</td>
                        <td>Presenterar resp. plans plan- och genomförandebeskrivning när dessa delar utgörs av ett och samma dokument</td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>samradsredogorelse</td>
                        <td>Presenterar resp. plans samrådsredogörelse</td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>utlatande</td>
                        <td>Presenterar resp. plans utlåtande</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>grk</td>
                        <td>Presenterar resp. plans grundkarta i de fall den ligger som eget dokument och inte tillsammans med plankartan</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>fastighetsforteckning</td>
                        <td>Presenterar resp. plans fastighetsförteckning då den finns framtagen samt är medskickad planen</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>kvalitetsprogram</td>
                        <td>Presenterar resp. plans kvalitetsprogram då ett sådant finns framtaget till planen</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>mkb</td>
                        <td>Presenterar resp. plans miljökonsekvensbeskrivning (MKB) då en sådan finns framtagen till planen</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>bullerutredning</td>
                        <td>Presenterar resp. plans bullerutredning då en sådan finns framtagen till planen</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>gestaltningsprogram</td>
                        <td>Presenterar resp. plans gestaltningsprogram då en sådan finns framtagen till planen</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>ovriga</td>
                        <td>Här under samlas alla övriga dokument som inte direkt ses som en formell planhandling men som ingått i planens framställning. Det kan röra sig om utredningar, konsekvensbeskrivningar, offentliga konversationer, kungörelser m.m.</td>
                    </tr>
                </table>
                <div class="exempel">
                    <span class="exempelrubrik">Exempel: Val av presenterad(e) dokumenttyp(er)</span>
                    <p>
                        Följande plan eftersöks genom att den formella aktbeteckning, 1282K-P08/256, är känd.
                    </p>
                    https://geodata.landskrona.local/app/plan/dokument/1282K-P08/256
                    <p>
                        Alla tillgängliga plandokument presenteras och blir nedladdningsbara.
                    </p>
                    <br />
                    https://geodata.landskrona.local/app/plan/bestammelse/1282K-P08/256
                    <p>
                        Endast bestämmelserna som fristående dokument presenteras och blir nedladdningsbart.
                    </p>
                    <br />
                    https://geodata.landskrona.local/app/plan/handling/1282K-P08/256
                    <p>
                        Dokumenten som ses som en del av planen, formella planhandlingar, presenteras och blir nedladdningsbart.
                    </p>
                </div>
                <p>
                    Del [2]<br />
                    frivillig del. Används delen ska den separeras från [1] med tecknet ”<asp:Label ID="lblPart2UrlSplitter" runat="server"></asp:Label>”. [2] Avgör mot vad sökbegreppet matchas mot. Kan endast använda en signal åt gången.<br />
                    Möjliga signaler om villkor är:
                </p>
                <table>
                    <tr>
                        <td>akt</td>
                        <td>indikerar att sökning endast görs efter planens formella aktbeteckning i fastighetsregistret</td>
                    </tr>
                    <tr>
                        <td>akttidigare</td>
                        <td>indikerar att sökning endast görs efter arkivserien under Landskrona stads f.d. stadsingenjörskontor med prefixet "1282K-"</td>
                    </tr>
                    <tr>
                        <td>aktegen</td>
                        <td>indikerar att sökning endast görs efter den tidigare arkivserien från Landskrona stads plan- och bygglovsverksamheter</td>
                    </tr>
                    <tr>
                        <td>nyckel</td>
                        <td>indikerar att sökning endast görs efter planens nyckel i fastighetsregistret</td>
                    </tr>
                    <tr>
                        <td>fastighet</td>
                        <td>indikerar att sökning endast görs efter fastighetsbeteckning. Fastighetsbetecknings registernummerdel har block och enhet separerade av tecknet kolon ":". Tillsammans med s.k. ”ren adress” (teknisk term ”URL Routing”, för fullständig funktion se under avsnitt ”För utvecklare”) hindras tecknet kolon av säkerhetsskäl. Kolon ersätts med tecknet "<asp:Label ID="ParcelBlockUnitSign" runat="server"></asp:Label>" vid sökning på fastighet.</td>
                    </tr>
                    <tr>
                        <td>fastighetnyckel</td>
                        <td>indikerar att sökning endast görs efter fastighetens nyckel i fastighetsregistret</td>
                    </tr>
                </table>
                <p>
                    Utelämnas signal görs en sökning mot alla möjligheter (skrivs fastighetsbeteckning med kolon fungerar det ej).
                </p>
                <div class="exempel">
                    <span class="exempelrubrik">Exempel: Val av vad sökbegrepp jämförs mot</span>
                    <p>
                        Följande plan eftersöks genom plan- och bygglovsverksamheternas egna f.d. arkivserie, 274, är känd.
                    </p>
                    http://geodata/app/plan/dokument/274
                    <p>
                        Resultatet i detta fall blir dock presentaton av två planer, 12-LAN-431 och 12-LAN-618. Anledningen är följande.
                    </p>
                    <br />
                    http://geodata/app/plan/dokument,nyckel/274
                    <p>
                        Sökningen specificeras genom att signalera, med hjälp av ovan signaler, om att 274 är en plannyckel.
                        <br />
                        Resultatet blir då istället endast planen 12-LAN-618.
                    </p>
                    <br />
                    http://geodata/app/plan/dokument,aktegen/274
                    <p>
                        Sökningen specificeras genom att signalera, med hjälp av ovan signaler, om att 274 specifikt är plan- och bygglovsverksamheternas f.d. arkivserie.
                        <br />
                        Resultatet blir då istället endast planen 12-LAN-431.
                    </p>
                </div>
                <p>
                    Del [3]<br />
                    sökbegrepp, planen vars dokument eftersöks. Söks flera samtidigt ska dessa separeras med tecknet ”<asp:Label ID="lblPart3UrlSplitter" runat="server"></asp:Label>”. Sökning sker på resp. sökbegrepps exakta lydelse. Någon s.k. ”wild card” sökning går ej att göra. Sökningen gör ej skillnad på gemener eller versaler.
                </p>
                <div class="exempel">
                    <span class="exempelrubrik">Exempel: Stadsplan, kv. Gamla Bryggan 20</span>
                    <p>
                        Vid sökning direkt på en äldre plan, som i detta exempel, är planen sökbar på fem sätt.
                        <br />
                        Formell aktbeteckning i fastighetsregistret för stadsplanen är 12-LAN-182.
                        <br />
                        Landskrona stads f.d. Stadsingenjörskontors f.d. arkivnummer för planen är 1397. Denna arkivserie är även nationell alternativ aktbeteckning och har då 1282K-1397 som beteckning.
                        <br />
                        Plan- och bygglovsverksamheternas arkivnummer för samma plan är 325.
                        <br />
                        Unik nyckel för planen i lokalt fastighetsregister är 116.
                        <br />
                        Alla dessa fem benämningar av samma plan är sökbara (12-LAN-182, 1397, 1282K-1397, 325 och 116) genom webbläsarens adressfält och då genom följande:
                    </p>
                    http://geodata/app/plan/dokument/12-LAN-182
                    <p>
                        alternativt
                    </p>
                    http://geodata/app/plan/dokument/1397
                    <br />
                    http://geodata/app/plan/dokument/1282K-1397
                    <br />
                    http://geodata/app/plan/dokument/325
                    <br />
                    http://geodata/app/plan/dokument/116
                    <p>
                        Alla kommer att ge ett och samma sökresultat.
                    </p>
                    <p>
                        Om sökt planbenämning återkommer i olika sammanhang som t.ex. de olika arkivserierna och nyckel i fastighetsregistret kan sökningen ge flertalet träff på olika planer. Då behövs ett detaljerat villkorande (se del [2] ovan).
                    </p>
                </div>
                <div class="exempel">
                    <span class="exempelrubrik">Exempel: Bestämmelser för flera planer efter plan- och bygglovsverksamheternas f.d. arkivserie</span>
                    http://geodata/app/plan/bestammelse,aktegen/ö3,524,311
                    <p>
                        I följande sökning önskas dokumenttyp bestämmelse med känd plan- och bygglovsverksamheternas f.d. arkivserie. Följande tre planer presenteras.
                    </p>
                    <ul>
                        <li>12-LAN-626</li>
                        <li>1282K-P12/2</li>
                        <li>12-LAN-86</li>
                    </ul>
                    <p>
                        Dokument med bestämmelser kan laddas ner i de fall då bestämmelser ej ligger i plankarta, i annat fal återfinns inget dokument.
                    </p>
                </div>


                <h1 id="avsnitt2">2 Grafiskt användargränssnitt</h1>
                <h2 id="avsnitt2-1">2.1 För alla</h2>
                <p>
                    Indexen i bilden nedan hänvisas det till i den förklarande texten. Var rad i tabellen motsvarar en plan.
                    <img src="<%= ResolveUrl("~/") %>pic/LayoutExplanationAlla.png" />
                </p>
                <h3 id="avsnitt2-1-1">2.1.1 Enkel listform (i bildens del 'A')</h3>
                <p style="font-size: 1.5em; font-weight: bold;">A</p>
                <ol>
                    <li>Sorterar kolumn</li>
                    <li>Öppnar raden med planens alla plandokument</li>
                    <li>Indikerar planens genomförandestatus:
                        <ul>
                            <li><img src="<%= ResolveUrl("~/") %>pic/knob-green.png" title="Genomförandetid löpt ut" /> = ej inom genomförandetid</li>
                            <li><img src="<%= ResolveUrl("~/") %>pic/knob-orange.png" title="Inom genomförandetid" /> = inom genomförandetid</li>
                        </ul>
                    </li>
                </ol>

                <h3 id="avsnitt2-1-2">2.1.2 Listning av plandokument (i bildens del 'B')</h3>
                <p style="font-size: 1.5em; font-weight: bold;">B</p>
                <ol start="4">
                    <li>Urvalsruta - markerar eller avmarkerar alla plandokument.</li>
                    <li>Urvalsrutor för plandokumenten.</li>
                    <li>Nedladdning - paketerar valda dokument till en zip-fil.</li>
                    <li>Stänger planens tabell med plandokument.</li>
                </ol>

                <h2 id="avsnitt2-2">2.2 För enskilda</h2>
                <p>
                    Indexen i bilden nedan hänvisas det till i den förklarande texten. Presentationen av sökt plan görs i en layout liknande bibliotekens gamla kort i kartoteken, i huvudsak enligt ’B’, där ett kort är detsamma som en plan. Vid sökning där fler än en plan presenteras är dessa kort initialt kollapsade till att endast presentera planerna i rubrikform enligt ’C’. Alla sökningar presenteras översiktligt tillsammans med annan information av mer statistisk karaktär enligt ’A’.
                    <br />
                    <img src="<%= ResolveUrl("~/") %>pic/LayoutExplanationv2.png" />
                </p>


                <h3 id="avsnitt2-2-1">2.2.1 Översiktlig panel (i bildens del 'A')</h3>
                <p style="font-size: 1.5em; font-weight: bold;">A</p>
                <ol>
                    <li>Funktion som för alla planer kollapsar/stänger alternativt expanderar den detaljerade informationen med plandokument och kartöversikt.</li>
                    <li>Redogör för vilka parametrar sökningen är gjord med.
                        <ul>
                            <li>"Antal sökta" är antalet delar, separerade med "<asp:Label id="lblPart5UrlSplitter" runat="server"></asp:Label>", webbapplikationen har hittat i textsträngen från adressfältet.</li>
                            <li>"Kriterie" är de specificerande signaler beskrivna under avsnitt "1.2 Adressfält i webbläsare" och som del [2].</li>
                            <li>"Dokumenttyp" är den avgränsning som kan göras med del [1] under avsnitt "1.2 Adressfält i webbläsare".</li>
                            <li>"Sökta" är den söksträng med alla sökta objekt så som den är inskriven i adressfältet.</li>
                        </ul>
                    </li>
                    <li>Redogör för antalet hittade planer. 
                        <br />
                        Om antalet ej är samma som antalet sökta behöver det inte innebära att sökt ej fått någon planträff. Beroende på hur sökningen är gjord, med vilka parametrar, kan samma plan fått träff på flera sätt, t.ex. en berörd fastighet, plannyckel och aktbeteckning.</li>
                    <li>Visar antalet gällande planer, vilka olika plantyper som existerar av dessa och hur många vardera är samt även hur många av dessa som är inom genomförandetid.</li>
                </ol>

                <h3 id="avsnitt2-2-2">2.2.2 Plandokument (i bildens del 'B')</h3>
                <p style="font-size: 1.5em; font-weight: bold;">B</p>
                <ol>
                    <li>Planens rubrikparti, innehållande i huvudsak formell aktbeteckning och arkivserier, är aktiv där det är möjligt att kollapsa respektive expandera planinnehållet.</li>
                    <li>Formell aktbeteckning som ges genom registrering i det nationella fastighetsregistret. Aktbeteckningen följs av en parantes innehållande plantypens förkortning.</li>
                    <li>Lokal benämning på plan i nationellt fastighetsregister. Sista delen av lokal beteckning är tillika Landskrona stads f.d. Stadsingenjörskontors arkivserie. Arkivserien används fram till man startade egen lokal lantmäterimyndighet år 1999.
                        <br />
                        ex. 1282K-1234
                        <br />
                        1282K-1234 = lokal aktbeteckning i nationellt fastighetsregister
                        <br />
                        1234 = Landskrona stads f.d. Stadsingenjörskontors arkivserie
                    </li>
                    <li>Plan- och bygglovsverksamheternas tidigare arkivserie. Användes fram t.o.m. år 2012.</li>
                    <li>Bild indikerande om plan är inom genomförandetid
                        <br />
                        <img src="<%= ResolveUrl("~/") %>pic/knob-green.png" title="Genomförandetid löpt ut" /> = ej inom genomförandetid
                        <br />
                        <img src="<%= ResolveUrl("~/") %>pic/knob-orange.png" title="Inom genomförandetid" /> = inom genomförandetid
                    </li>
                    <li>Plannamn om det existerar i nationellt fastighetsregister. Presenteras inget namn är det troligast att planen ej getts något namn och då är en äldre plan.</li>
                    <li>Här under presenteras de dokument från planen som sorterats och är formell planhandling. Existerar ej något specifikt dokument finns det ej i planen.</li>
                    <li>De dokument som ej ses som formell planhandling listas här under. Vissa dokumentyper som fastighetsförteckning och grundkarta listas separat om dessa existerat separat. I annat fall finns dokument som exempelvis
                        <ul>
                            <li>kungörelser</li>
                            <li>utredningar</li>
                            <li>korrespondans</li>
                            <li>yttranden</li>
                            <li>etc.</li>
                        </ul>
                    </li>
                    <li>Presenterar möjliga utsorterade dokumenttyper men som ej är tillgängliga.</li>
                    <li>Översiktsbild som visar planens gällande område.</li>
                    <li>Funktioner för att göra dokumenten valbara och nedladdningsbara som en paketerad fil (en nedladdningbar fil i filformatet zip). <br />
                        Klickas på ikonen utökas bilden med ytterligare funktioner och dokumenten får urvalsrutor (kryssrutor) enligt ovan bilds del 'a'.
                    </li>
                </ol>

                <p style="font-size: 1.5em; font-weight: bold;">a</p>
                <ol start="12">
                    <li>Består av en urvalsruta och en funktion för nedladdning av fil.
                        <ul>
                            <li>Urvalsruta - markerar eller avmarkerar alla dokument.</li>
                            <li>Nedladdning - paketerar valda dokument till en zip-fil.</li>
                        </ul>
                    </li>
                    <li>Urvalsrutor för planhandlingar</li>
                    <li>Urvalsrutor för övriga plandokument</li>
                </ol>

                <p style="font-size: 1.5em; font-weight: bold;">C</p>
                <p>
                    Resultat av presenterade planer i kollapsat läge, ej presenterad detaljerad information med planens dokument och översiktsbild.
                </p>

                <h2 id="avsnitt2-3">2.3 Planpåverkan</h2>
                <p>
                    Sökt plan redovisar dess beroende/relationer (planpåverkan) till andra beslut som ex. andra planer, tomtindelningar, ändrade planer, beslut som upphäver men som ej 
                    är en registrerad plan, etc. Relevansen av planpåverkan markeras med någon av nedan knappars utseende.
                </p>
                <table>
                    <tr>
                        <td>
                            <button disabled="disabled" class="btn btn-outline-success btn-xs" type="button" title="Ingen påverkan från andra registrerade planer och beslut">
                                Ingen planpåverkan</button>
                        </td>
                        <td style="vertical-align: middle;">
                            Ingen relation till något annat beslut.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <button class="btn btn-secondary dropdown-toggle btn-xs btn-outline-secondary" type="button" title="Andra planer som berör samma område, påverkar planen eller har påverkats av planen">
                                Planpåverkan</button>
                        </td>
                        <td style="vertical-align: middle;">
                            Relation till andra beslut men som <span style="font-weight: bold; text-decoration: underline;">inte</span> påverkar sökt plans bestämmelse(r).
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <button class="btn btn-secondary dropdown-toggle btn-xs btn-outline-danger" type="button" title="Andra planer som berör samma område, påverkar planen eller har påverkats av planen">
                                Planpåverkan</button>
                        </td>
                        <td style="vertical-align: middle;">
                            Relation till andra beslut som <span style="font-weight: bold; text-decoration: underline;">påverkar</span> sökt plan i någon form. Markerade relationer ska läsas tillsammans med sökt plan!
                        </td>
                    </tr>
                </table>
                <p>
                    <img style="float: right;" src="<%= ResolveUrl("~/") %>pic/help/planpavarkan-beslut.png" />
                    Vid klickning på knappen, vid existerande relation, listas påverkande beslut. Alla relationer listas oberoende typ av relation.
                    Relationer som är av betydelse för gällande bestämmelser markeras specifikt. <br />
                    Beslut som påverkar sökt plan markeras med <img src="<%= ResolveUrl("~/") %>pic/warning.png" alt="Röd varningstriangel med utropstecken" />
                    och ska läsas tillsammans med sökt plan. Om relation är en registrerad plan länkas beslutet och plandokument är möjliga att få fram även för det beslutet.
                </p>
                <p>
                    Existerar länkade beslut sammanfattas de även överst under en länk "Lista alla". <br />
                    Beslut listas som "Planbeslut" eller "Övriga beslut". <br />
                    "Planbeslut" är planer som registrerats i fastighetsregistret och i registerkartan och har en faktisk representation i kartan. <br />
                    "Övriga beslut" listar beslut som inte nödvändigt är en registrerad plan men som påverkar sökt plan. Kan exempelvis vara upphävande beslut.
                </p>


                <h1 id="avsnitt3" style="clear: right;">3 Om</h1>
                <p>
                    Plandokumenten skannas och bearbetas i rutinen för planens lagakrafthantering och i den efterföljande registrering. Alla dokument kopplade till planen skannas och genomgår sortering om vad som bl.a. utgör formell planhandling. Dokumenten är åtkomliga inom Landskrona stads organisation och för de som har tillgång till intranätet (det s.k. Arbetsnätet) samt som delmängd på www.landskrona.se. Det är originalhandlingen, undertecknade dokument, som skannas. Undertecknad handling existerar inte för alla dokument i alla planer då de ej alltid gått att finna.
                </p>
                <p>
                    Dokumenten är i huvudsak lagrade som pdf-dokument. Plankartan finns digitalt både som pdf-dokument (i dess original arkstorlek/format) och tiff-fil (georefererad bildfil i verklig storlek). Bildfilens georeferering innebär att filen kan hanteras som en vanlig digital bild men även tolkas i verklig storlek (utan skalning) i kartan.
                </p>
                <p>
                    Planer är sökbara på sju olika sätt:
                </p>
                <ol>
                    <li>Planens lokala nyckel i fastighetsregistret</li>
                    <li>Planens formella aktbeteckning</li>
                    <li>Kommunal beteckning i fastighetsregistret
                        <br />
                        (samma som Landskrona stads f.d. Stadsingenjörskontors f.d. arkivserie men med 1282K- som prefix)</li>
                    <li>Landskrona stads f.d. Stadsingenjörskontors f.d. arkivserie</li>
                    <li>Landskrona stads plan- och bygglovsverksamheters f.d. arkivserie</li>
                    <li>Planens berörkrets av fastigheter genom resp. fastighets lokala nyckel i fastighetsregistret</li>
                    <li>Planens berörkrets av fastigheter genom resp. fastighets fastighetsbeteckning</li>
                </ol>
                <p>
                    Respektive sökt plan listar även relaterande beslut för en så fullständig bild över bestämmelserna som möjligt.
                </p>



                <h1 id="avsnitt4">4 Inställningar / konfiguration</h1>
                <h2 id="avsnitt4-1">4.1 Dokumentbegrepp</h2>
                <p>
                    Alla plandokument blir klassad som någon dokumenttyp. Dessa dokumenttyper definieras i filen "dokumenttyper.csv". Denna går att hitta i webbapplikationens rotkatalog.
                    csv-filen kan ses som en tabell utan kolumnrubriker och där resp. kolumn separeras av semikolon (;). csv-filen består utav <asp:Label ID="nbrColumnDocumenttypes" runat="server" /> st. kolumner.
                </p>
                <ol id="columnsDocumenttypes" runat="server">
                </ol>
                <p>
                    Enligt radspecifikation:
                </p>
                <div class="adresSearch">
                    <asp:Label ID="rowspecDocumenttypes" runat="server" />
                </div>
                <p>
                    Filen måste uppfylla specifikation för att kunna läsas av webbapplikationen.
                    </p>
                <p>
                    För närvarande existerar det
                    <asp:Label ID="nbrDocumenttypes" runat="server" />
                    st. dokumenttyper och där "dokumenttyper.csv" är enligt nedan:
                </p>
                <p>
                    <asp:Literal ID="tableDocumenttypes" runat="server" />
                </p>


                <h2 id="avsnitt4-2">4.2 Settings.config</h2>
                <p>
                    Flera grundinställningar av webbappikationen kan göras i filen "Settings.config". Denna går att hitta i webbapplikationens rotkatalog. För närmare beskrivning av resp. inställning hänvisas man till kommentarerna i filen.
                    <br />
                    Nedan följer en sammanfattning av vad som kan konfigureras:
                </p>
                <ul>
                    <li>Anslutningsuppgifter databas</li>
                    <li>Applikationsversion</li>
                    <li>Avgränsande tecken vid sökning på flera</li>
                    <li>Skiljetecken mellan block och enhet för fastighet. <br />
                        Traditionellt kolon-tecken är ej tillåtet som internationell standard i URL:er.</li>
                    <li>Vid sökning enligt alternativet namn/värde-par
                        <ul>
                            <li>Parameternamn som håller söksträngen i URL:n.</li>
                            <li>Parameternamn som håller vilken dokumenttyp sökningen filtreras efter.</li>
                            <li>Parameternamn som håller vilken signal som sökningen ska göras mot.</li>
                        </ul>
                    </li>
                    <li>
                        Cache-ning
                        <ul>
                            <li>Antal dagar</li>
                            <li>Klockslag då cache går ut</li>
                        </ul>
                    </li>
                    <li>Katalog där plandokument hittas</li>
                    <li>Om sökning av dokument ska ske i underkataloger eller inte</li>
                    <li>För vilka filändelser som sökningen sker mot</li>
                    <li>Karta
                        <ul>
                            <li>Om kartbildens storlek ska sättas dynamiskt eller inte</li>
                            <li>Fasta dimensioner för kartbilden om dynamiskt storlek inte används</li>
                            <li>Skalning av vyn runt sökt objekt</li>
                            <li>Bakgrundskarta som ska användas</li>
                            <li>Vilket planlager i kartan</li>
                            <li>Plannyckel i kartan</li>
                            <li>Sökträffens färgpresentation</li>
                            <li>...</li>
                        </ul>
                    </li>
                    <li>Loggning
                        <ul>
                            <li>Händelser (audit)</li>
                            <li>Statistik</li>
                            <li>Fel</li>
                        </ul>
                    </li>
                    <li>Aktivera funktion som kan "hålla webbapplikationen i liv" för när webbservern vill återvinna applikationen</li>
                    <li>E-post</li>
                </ul>



                <h1 id="avsnitt5">5 För utvecklare</h1>
                <p>
                    Ett sätt att söka via webbläsarens adressfält och på ett mer användarvänligt och intuativt sätt är enligt avsnittet "Hitta plandokument genom" "Adressfält i webbläsare". Yterliggare ett sätt att söka är genom parameter/värde-par. Ordningen på dessa namn/värde-par har ingen betydelse. Denna modell för sökning är främst tänkt att användas av andra applikationer.
                </p>
                <p>
                    Uppbyggnad av adress och sökning är http://[domän och adress inkl. filnamn]?[namn/värde-par]&[...]. Namn/värde-par upprepas för vardera namn (parameter) skillt av och-tecken (&).
                </p>
                <p>Namn/värde-par är:</p>
                <ul class="ul-bas">
                    <li><asp:Label ID="UrlParameterSearchString1" runat="server"></asp:Label> = samma betydelse som för del [3] ovan</li>
                    <li><asp:Label ID="UrlParameterDocumentType1" runat="server"></asp:Label> = samma betydelse som för del [1] ovan</li>
                    <li><asp:Label ID="UrlParameterSearchType1" runat="server"></asp:Label> = samma betydelse som för del [2] ovan</li>
                </ul>
                <p>
                    Adressen och stommen till webbtillämpningen är då istället:
                </p>
                <div class="adresSearch">
                    http://geodata/app/plan/plandokument.aspx?<asp:Label ID="UrlParameterSearchString2" runat="server"></asp:Label>=[SÖKTA PLANER SEPARERADE AV TECKEN "<asp:Label id="lblPart4UrlSplitter" runat="server"></asp:Label>"]&<asp:Label ID="UrlParameterDocumentType2" runat="server"></asp:Label>=[DOKUMENTTYP ENLIGT DEL [1] OVAN]&<asp:Label ID="UrlParameterSearchType2" runat="server"></asp:Label>=[SIGNAL ENLIGT DEL [2] OVAN]
                </div>
                <div class="exempel">
                    <span class="exempelrubrik">Exempel: Ovan exempel "Bestämmelser för flera planer efter plan- och bygglovsverksamheternas f.d. arkivserie" omtolkat</span>
                    http://geodata/app/plan/plandokument.aspx?<asp:Label ID="UrlParameterSearchString3" runat="server"></asp:Label>=ö3,524,311&<asp:Label ID="UrlParameterDocumentType3" runat="server"></asp:Label>=bestammelse&<asp:Label ID="UrlParameterSearchType3" runat="server"></asp:Label>=aktegen
                    <p>
                    </p>
                </div>
            </div>
    </form>
</body>
</html>
