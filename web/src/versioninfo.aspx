﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="versioninfo.aspx.cs" Inherits="Plan.Plandokument.versioninfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="lib/bootstrap-4.1.2-dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/page-UI-core.css" rel="stylesheet" />
    <link href="css/page-UI-help.css" rel="stylesheet" />
    <title>Plandokument / Versionsinformation</title>

    <script src="<%= ResolveUrl("~/") %>js/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/") %>lib/bootstrap-4.1.2-dist/js/bootstrap.bundle.min.js"></script>

    <style type="text/css">
        li {
            list-style-type: none;
        }

        span {
            font-style: italic;
            font-weight: bold;
            margin-right: 1em;
            font-size: 1.1em;
            letter-spacing: 0.1em;
        }

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
                <li><a class="list-group-item list-group-item-action" href="#avsnitt1">Versionsinformation</a>
                </li>
                <li><a class="list-group-item list-group-item-action" href="#asvnitt2">vNext</a>
                </li>
                <li><a class="list-group-item list-group-item-action" href="<%= ResolveUrl("~/") %>dokument/om">Dokumentation<span class="linkNewWindow"> </span></a>
                </li>
            </ul>
        </div>


        <div id="content">

            <div>
                <h4 id="avsnitt1">Versionsinformation</h4>
                <div id="version">
                    <ul>
                        <li>
                            <ul>
                                <li><span>v0.18.3</span> BUGGRÄTTNING: Länkningsfel för skriptresurser.
                                </li>
                                <li><span>v0.18.2</span> Indikering i menyn för vilket avsnitt som visas för hjälpen och versionsinformationen (s.k. scrollspy).
                                </li>
                                <li><span>v0.18.1</span> Loggning för cache-händelser till händelseloggen.
                                </li>
                            </ul>
                        </li>
                        <li><span>v0.18</span> Utökat med generell händelseloggning (Audit.log).
                        </li>
                        <li><span>v0.17</span> Metod för att hålla webbapplikationen i liv (ping:ar sig själv). På detta sätt hålls chachen i liv och svartider ska vara snabba trots lång tid mellan requests.
                        </li>
                        <li>
                            <ul>
                                <li><span>v0.16.17</span> Planpåverkan, hjälpdokumentation <a href="<%= ResolveUrl("~/") %>dokument/om#avsnitt2-3">här</a>.
                                </li>
                                <li><span>v0.16.16</span> BUGGRÄTTNING: Planpåverkan, tomtindelningar flaggas som varningar men länkas ej.
                                </li>
                                <li><span>v0.16.15</span> BUGGRÄTTNING: Planpåverkan, länkningsbara beslut listade under "Övriga beslut" var ej länkade.
                                </li>
                                <li><span>v0.16.14</span> Vid sökning på ÄDP och ÄOB ges varning i planpåverkan att de ska läsas tillsammans med plan som ändras.
                                </li>
                                <li><span>v0.16.13</span> Layout, paddning topp för knapp komprimering/kollapsa har lagts till.
                                </li>
                                <li><span>v0.16.12</span> Layout, paddning topp för avsnittsrubrik "Övriga plandokument" har utökats.
                                </li>
                                <li><span>v0.16.11</span> Sidoinformationen med knapp för komprimering/expandering och statistik har ändrats till att vara "flytande" och "glider" med när man har många planer.
                                </li>
                                <li><span>v0.16.10</span> Dokumenttyper, om endast en fil i ett filformat i dokumenttypsgrupp ges standardbenämning "dokument".
                                </li>
                                <li><span>v0.16.9</span> Färger ändrades samt dess betydelse för Planpåverkan.
                                </li>
                                <li><span>v0.16.8</span> BUGGRÄTTNING: Verktygsbilderna fungerade ej som tänkt under SSL via https.
                                </li>
                                <li><span>v0.16.7</span> BUGGRÄTTNING: Flera dokumenttyper grupperas ej rätt när suffix är littera och när dokumenten inom samma dokumenttyp existerar i flera filformat.
                                </li>
                                <li><span>v0.16.6</span> BUGGRÄTTNING: Stöd för Internet Explorer (IE) förlorades genom ett för modernt sätt att bygga upp javascript-objekt. Alternativ lösning för bibehållet IE-stöd.
                                </li>
                                <li><span>v0.16.5</span> Layout, åtgärdat för stor paddning mellan avsnittsrubrik "Ej enskilt upprättade dokument" och bild.
                                </li>
                                <li><span>v0.16.4</span> Planpåverkan, avregistrerade planer länkas ej då det ej finns någon konsekvent hantering av plandokumenten för planer som upphört.
                                </li>
                                <li><span>v0.16.3</span> BUGGRÄTTNING: Vid många listade beslut i planpåverkan och scrollning behövs, visas ej sista beslutet.
                                </li>
                                <li><span>v0.16.2</span> Planpåverkan, beslut som påverkar sökt plan har getts titel som uppmanar till kontroll.
                                </li>
                                <li><span>v0.16.1</span> Planpåverkan, titel på länkbild om externt öppnande i nytt webbläsarfönster.
                                </li>
                            </ul>
                        </li>
                        <li><span>v0.16</span> Infört redovisning av planers påverkan på varandra som meny under specifik listad plan. Menyn-UI kommer från Bootstrap.
                        </li>
                        <li>
                            <ul>
                                <li><span>v0.15.5</span> Tydliggjort avsnittsindelningen av "Planhandlingar", "Övriga plandokument" och "Ej enskilt upprättade dokument".
                                </li>
                                <li><span>v0.15.4</span> Utökat marginal till webbläsarfönstret för knapp som expanderar och kollapsar alla planer.
                                </li>
                                <li><span>v0.15.3</span> Funktion för expandera och kollapsa alla planer ändrad till Bootstrap-knapp.
                                </li>
                                <li><span>v0.15.2</span> Tydliggjort funktionsknapparna (alla planer, hjälp och e-post) för sida som listar specifika planer.
                                </li>
                                <li><span>v0.15.1</span> Adderat länk till listning av alla planer och dess dokument.
                                </li>
                            </ul>
                        </li>
                        <li><span>v0.15</span> UI-förbättringar.</li>
                        <li><span>v0.14</span> Uppgraderat ramverket jQuery från v1.9.0 till v3.4.1.</li>
                        <li><span>v0.13</span> Adderat <a href="https://getbootstrap.com" target="_blank">Bootstrap</a> som UI-komponent.</li>
                        <li><span>v0.12</span> Dokumenttyperna är domändrivna där domänvärdena sätts i filen dokumenttyper.csv.</li>
                        <li><span>v0.11</span> Dokumenttyperna kan bestå av flera delar och grupperas då i gränssnittet.</li>
                        <li>
                            <ul>
                                <li><span>v0.10.3</span> BUGGRÄTTNING: Cachening av information på servern.
                                    <br />
                                    <i>Cachening gjordes på nytt vid varje nystartad session, vilket upplevdes långsamt och motverkade anledningen till cachening. Cachening sker nu endast vid första sessionen som startar applikationen sedan utgången av cache.</i>
                                </li>
                                <li><span>v0.10.2</span>  Korslänkning mellan dokumentation och versionsinformation under menyer.
                                </li>
                                <li><span>v0.10.1</span>  Ny typ av plandokument kan hanteras - Gestaltningsprogram.
                                    <br />
                                    <a href="om.aspx#avsnitt1-2-2">Läs mer under dokumentation</a>
                                </li>
                            </ul>
                        </li>
                        <li><span>v0.10</span> Stöd för SSL över https.</li>
                        <li>
                            <ul>
                                <li><span>v0.9.11</span> Kollapsning och expandering för avsnittet med ej befintliga plandokument ("Ej enskilt upprättande dokument")
                                </li>
                                <li><span>v0.9.10</span> Hantering för planbetecknings ändrade namnkonvention, ex. 1282K-P17/1 --> 1282K-P2018/1
                                </li>
                                <li><span>v0.9.9</span> Rubrik för dokumenttyper med ej existerande plandokument ändrad från "Ej tillgängliga dokument" --> "Ej enskilt upprättade dokument".
                                </li>
                                <li><span>v0.9.8</span> Ny typ av plandokument kan hanteras - Plan- och genomförandebeskrivning.
                                    <br />
                                    <a href="om.aspx#avsnitt1-2-2">Läs mer under dokumentation</a>
                                </li>
                                <li><span>v0.9.7</span> Ytterligare fler typer av plandokument kan hanteras;
                                        <ul>
                                            <li>Kvalitetsprogram,</li>
                                            <li>Miljökonsekvensbeskrivning (MKB) och</li>
                                            <li>Bullerutredning.</li>
                                        </ul>
                                    <a href="om.aspx#avsnitt1-2-2">Läs mer under dokumentation.</a>
                                </li>
                                <li><span>v0.9.6</span> Kompilerat mott och nytt stöd till MapGuide Open Source version 3.1 och Autodesk Infrastructure Map Server (AIMS) 2016.</li>
                                <li><span>v0.9.5</span> Ändrat till nativt bibliotek för komprimering till zipp-paket av filer som laddas ner. Dokumentation med biblioteket plockas bort senare efter tester.</li>
                                <li>
                                    <ul>
                                        <li><span>v0.9.4.6</span> Radering av konfigurationer för testpubliceringar.</li>
                                        <li><span>v0.9.4.5</span> BUGGRÄTTNING: URL-uppbyggnad på klintsidan fungerade inte. Löst.</li>
                                        <li><span>v0.9.4.4</span> Uppdaterat dokumentation gällande beroenden till webbapplikationen.</li>
                                        <li><span>v0.9.4.3</span> BUGGRÄTTNING: Vid listning av plandokument för alla planer presenterades filstorleken fel utan avrundning och enhet.</li>
                                        <li><span>v0.9.4.2</span> BUGGRÄTTNING: Endast plankartan som pdf och tiff visades av plandokument vid listning av alla planer.</li>
                                        <li><span>v0.9.4.1</span> BUGGRÄTTNING: Skapa temporär zipkatalog låg fel i kodsekvens. Kontroll efter filer i temp-katalog för ev. radering gjordes innan kontroll om temp-katalogen först fanns. För första körningen uppstod fel eller när temp-katalogen ej existerade.</li>
                                    </ul>
                                </li>
                                <li><span>v0.9.4</span> Vid nedladdning och paketering av flera dokument presenteras genomgående grafik för indikering av pågående process.</li>
                                <li>
                                    <ul>
                                        <li><span>v0.9.3.4</span> BUGGRÄTTNING: Asterix som tecken för skiljet mellan block och enhet i fastighetens registernummer ses som säkerhetsbrist av ASP.NET. Tecken ändrat.</li>
                                        <li><span>v0.9.3.3</span> BUGGRÄTTNING: Avmarkering av global urvalsruta för dokument efter nedladdning fungerade ej.</li>
                                        <li><span>v0.9.3.2</span> BUGGRÄTTNING: Urvalsrutor för dokument vid nedladdning av flera fungerade ej vid flera planer öppna samtidigt.</li>
                                        <li><span>v0.9.3.1</span> Felmeddelande vid hämtande av kartbild ändrad.</li>
                                    </ul>
                                </li>
                                <li><span>v0.9.3</span> Weppapplikationen har möjlighet att skicka e-post som signal vid fel i applikationen.</li>
                                <li>
                                    <ul>
                                        <li><span>v0.9.2.2</span> Vid listning av alla planer.</li>
                                        <li><span>v0.9.2.1</span> För de enstaka sökningarna.</li>
                                    </ul>
                                </li>
                                <li><span>v0.9.2</span> Paketering och nedladdning av valda plandokument.</li>
                                <li>
                                    <ul>
                                        <li><span>v0.9.1.1</span> Alias "alla".</li>
                                    </ul>
                                </li>
                                <li><span>v0.9.1</span> Utökat URL-routingen.</li>
                            </ul>
                        </li>
                        <li><span>v0.9</span> Listning av alla planer</li>

                        <li>
                            <ul>
                                <li>
                                    <ul>
                                        <li><span>v0.8.5.2</span> För versionshistorik med tre alias "version", "information" och "info".</li>
                                        <li><span>v0.8.5.1</span> För hjälpsida med tre alias "om", "hjalp" och "help". </li>
                                    </ul>
                                </li>
                                <li><span>v0.8.5</span> Utökat URL-routingen.</li>
                                <li><span>v0.8.4</span> Fullständig sökning på fastighetsbeteckning innehållande block och enhet (kolon ":" ersätts med tillåtet tecken, se hjälptexten)</li>
                                <li>
                                    <ul>
                                        <li><span>v0.8.3.1</span> BUGGRÄTTNING: Fel rotadress till webbapplikationen i förklarande text under om.aspx.</li>
                                    </ul>
                                </li>
                                <li><span>v0.8.3</span> Sparar sökstatistik till fil under webbapplikationens installationskatalog och "log".</li>
                                <li><span>v0.8.2</span> Utökat funktionaliteten för loggning av fel. Möjligt att dela upp loggning på fler filer och begränsa filstorlek.</li>
                                <li><span>v0.8.1</span> Sparar fel på serversidan till fil under webbapplikationens installationskatalog och "log".</li>
                            </ul>
                        </li>
                        <li><span>v0.8</span> Loggning</li>

                        <li>
                            <ul>
                                <li>
                                    <ul>
                                        <li><span>v0.7.4.3</span> BUGGRÄTTNING: Vid snabba återkommande förfrågning genom fullständigt anrop eller genom ajax kan dubblering av information uppstå. Åtgärdat med att alltid kontrollera om anrop finns och i så fall avbryta innan nytt anrop (endast för den detaljerade planinformationen som t.ex. dokument och översikt)</li>
                                        <li><span>v0.7.4.2</span> BUGGRÄTTNING: Rubriken "Kartöversik" plockas bort när karta läggs in. Problem uppstod efter v0.7.4 - löst.</li>
                                        <li><span>v0.7.4.1</span> BUGGRÄTTNING: Kartöversikt vid sökning på en plan visas ej. Problem uppstod efter v0.7.4 - löst.</li>
                                    </ul>
                                </li>
                                <li><span>v0.7.4</span> Ändrar storleken för planens översiktskarta vid väsentlig storleksförändring på webbläsarfönstret.</li>
                                <li>
                                    <ul>
                                        <li><span>v0.7.3.6</span> BUGGRÄTTNING: Redovisning av söktext flödade över utrymmet i layouten. Åtgärdat med mellanslag efter skiljetecknet.</li>
                                        <li><span>v0.7.3.5</span> Plan utan registretat plannamn (beror ofast på äldre planer då planer ej namnades) ändrad text från "registrerat plannamn saknas" till "inget plannamn i fastighetsregistret"</li>
                                        <li><span>v0.7.3.4</span> BUGGRÄTTNING: Returnerar sökt planinformation en gång trotts träff på flera begrepp eller fastigheter (fungerade ej fullt ut för fastighet)</li>
                                        <li><span>v0.7.3.3</span> Standardsorteringen på returnerad planinformation efter sökning ändrad från att sortera på databasnyckel till formell aktbeteckning</li>
                                        <li><span>v0.7.3.2</span> Dokumentrubrik "Ej presenterade dokument" ändras till "Ej tillgängliga dokument"</li>
                                        <li><span>v0.7.3.1</span> Bild istället för text till länken för hjälp</li>
                                    </ul>
                                </li>
                                <li><span>v0.7.3</span> Funktion för att enklare lämna synpunkter och rapportera fel</li>
                                <li><span>v0.7.2</span> Ny struktur och sortering av versionsdokumentation</li>
                                <li>
                                    <ul>
                                        <li><span>v0.7.1.7</span> BUGGRÄTTNING: I vissa fall returnerades endast ett dokument vid sökning på plan som hade flera dokument. Anledningen var felanvändning av vektorvariabel innehållande dokumentsuffixen i filnamnskonventionen, uppstod konflikt i uppbyggnad av sökt fil vars suffix skrevs över.</li>
                                        <li><span>v0.7.1.6</span> BUGGRÄTTNING: Vid sökning med dokumenttypsignal 'dokument' hämtades alla filer som började med samma tecken trotts olika planer,
                                            <br />
                                            ex. sökt 1282K-P10/1 resulterade i alla dokument från 1282K-P10/1, 1282K-P10/11, 1282K-P10/12, ... 1282K-P10/111 osv.</li>
                                        <li><span>v0.7.1.5</span> BUGGRÄTTNING: I "om.aspx" och placeringen av index/innehållsförteckning fungerar ej "position: fix" i IE 9:s kompabilitetsvy. Överlappar text. Måste sätta top och left position till 0 samt nolla marginalerna för punktlista</li>
                                        <li><span>v0.7.1.4</span> BUGGRÄTTNING: Planhandlingen plankarta var ej hanterad som formell planhandling vid selektering av endast planhandling vid sökning</li>
                                        <li><span>v0.7.1.3</span> BUGGRÄTTNING: Genom införande av att selektera vad som ses som formell planhandling infördes fel där filnamn enligt den äldre namnkonventionen ej hittades</li>
                                        <li><span>v0.7.1.2</span> Separering av klientlogik från markup</li>
                                        <li><span>v0.7.1.1</span> Träff på antal planer</li>
                                    </ul>
                                </li>
                                <li><span>v0.7.1</span> Sökresultat
                                </li>
                            </ul>
                        </li>

                        <li><span>v0.7</span> Stöddokument med hjälptext skapad
                        </li>

                        <li><span>v0.6</span> Möjliggjort sökning på fastighet, presentation av plan efter berörkrets i fastighetsregistret
                        </li>

                        <li>
                            <ul>
                                <li><span>v0.5.9</span> Tydliggjort upphovsrätten</li>
                                <li><span>v0.5.8</span> Länkat till hjälptext</li>
                                <li>
                                    <ul>
                                        <li><span>v0.5.7.1</span> BUGGRÄTTNING: Bilder, planöversik, i större format skickas ej från server (höjt standardgränsen på JSON-sträng från 102500 byte.</li>
                                    </ul>
                                </li>
                                <li><span>v0.5.7</span> Rörlig GIF vid generering av kartbild för indikation av händelse</li>
                                <li><span>v0.5.6</span> Rubrik - ovanför genererd kartabild över planområdet, "Kartöversikt"</li>
                                <li>
                                    <ul>
                                        <li><span>v0.5.5.1</span> BUGGRÄTTNING: Visar text "INGEN BILD" i IE trotts bild genererad i base64. Borttaget då bildlänk aldrig existerar och därför ej ger tom ruta</li>
                                    </ul>
                                </li>
                                <li><span>v0.5.5</span> Rubrik - "Ej existerande eller separerade dokument" --> "Ej presenterade dokument" när projekt om digitalisering av dokument slutförts</li>
                                <li></li>
                                <li><span>v0.5.4</span> Infört begreppen "dokument" och "handling" som urskiljare och sökvillkor av presenterade dokumenttyper, "dokument" = alla filer || "handling" = endast filer som ses som planhandling</li>
                                <li><span>v0.5.3</span> Genomgående breddat applikationen något i hantering av olika dokumenttyper från endast planhandling till plandokument</li>
                                <li><span>v0.5.2</span> Funktionaliteten från version 0.1.3.1 har kompletterats för sökninng genom standard URL-parametrisering. Anpassning mot när sökning sker programmatiskt eller från annan applikation</li>
                                <li><span>v0.5.1</span> Rubrik - "Ej sorterade eller existerande" --> "Ej existerande eller separerade dokument"</li>
                            </ul>
                        </li>

                        <li><span>v0.5</span> Översiktskarta per plan - presentation av gällande planområde
                        </li>

                        <li>
                            <ul>
                                <li>
                                    <ul>
                                        <li><span>v0.4.9.1</span> BUGGRÄTTNING: Dokument "Samrådsredogörelse" och "Utlåtande" länkas rätt men med grundkarta som dokumentnamn för båda dokumenttyperna.</li>
                                    </ul>
                                </li>
                                <li><span>v0.4.9</span> Fastighetskonsekvensbeskrivning, med som separat dokument i.o.m. PBL 2010:900. Nej, enligt KLM-chef. Hanteras som ett avsnitt i planbeskrivningen.</li>
                                <li><span>v0.4.8</span> indikering i planlista om plan är inom genomförandetid eller ej</li>
                                <li>
                                    <ul>
                                        <li><span>v0.4.7.1</span> BUGGRÄTTNING: Fel i IE:s och kompabilitetsläge (körs då som IE7 och tidigare), felkod "SCRIPT5009: 'JSON' is undefined" och inga resultat från webbtjänst kan parsas och presenteras
                                            <br />
                                            http://stackoverflow.com/questions/8332362/script5009-json-is-undefined
                                            <br />
                                            http://stackoverflow.com/questions/2503175/json-on-ie6-ie7
                                            <br />
                                            http://bestiejs.github.io/json3/
                                        </li>
                                    </ul>
                                </li>
                                <li><span>v0.4.7</span> uppdelning av plandokument efter:
                                    <ul>
                                        <li>Planhandlingar
                                            <br />
                                            delar av plandokumenten som ses som planhandlingar enligt PBL
                                        </li>
                                        <li>Övriga plandokument (om de finns uppdelade)
                                            <br />
                                            resterande dokument som exempel
                                            <ul>
                                                <li>Grundkarta</li>
                                                <li>Fastighetsförteckning</li>
                                                <li>Utredningar</li>
                                                <li>Kungörelse</li>
                                                <li>Beslut</li>
                                                <li>Yttranden</li>
                                                <li>Konversationer</li>
                                            </ul>
                                        </li>
                                        <li>Ej sorterade eller existerande
                                            <br />
                                            Presenterar de olika dokumenttyperna som kan hanteras i webbapplikationen men som ej är extraherade eller som ej finns som del bland plandokumenten.
                                        </li>
                                    </ul>
                                </li>
                                <li><span>v0.4.6</span> utökade dokumenttyper
                                    <ul>
                                        <li>Samrådsredogörelse</li>
                                        <li>Utlåtande</li>
                                    </ul>
                                </li>
                                <li><span>v0.4.5</span> Sökning på f.d. Stadsingenjörskontorets arkivserie både som arkivserienummer och som lokal aktbeteckning i fastighetsregistret (ex. 1486 eller 1282K-1486 ger samma resultat).</li>
                                <li><span>v0.4.4</span> Buggrättning: konfigurationsfil Settings.congif, slash-tecken i sökväg till dokument fungerade ej</li>
                                <li><span>v0.4.3</span> Redovisning av alla dokumenttyper för vardera sökt plan, grå ikon och ej aktiv hyperlänk</li>
                                <li><span>v0.4.2</span> beskrivning av URL-sökning och uppbyggnad</li>
                                <li><span>v0.4.1</span> namnkonvention</li>
                            </ul>
                        </li>
                        <li><span>v0.4</span> dokumentering
                        </li>
                        <li><span>v0.3</span> versionering</li>
                        <li>
                            <ul>
                                <li>
                                    <ul>
                                        <li><span>v0.2.2.1</span> dokumenttyper
                                            <ul>
                                                <li>Karta, Plankarta</li>
                                                <li>Bestämmelser, Planbestämmelser då dessa inte existerar på plankartan</li>
                                                <li>Illustration, Planillustration</li>
                                                <li>Beskrivning, Planbeskrivning</li>
                                                <li>Genomförande, Genomförandebeskrivning</li>
                                                <li>Fastighetsförteckning, Fastighetsförteckning som noterar sakägare</li>
                                                <li>Grundkarta, Grundkarta då den inte existerar i plankarta</li>
                                                <li>Övriga, Dokument som ej klassificerats på något annat sätt</li>
                                            </ul>
                                        </li>
                                    </ul>
                                </li>
                                <li><span>v0.2.2</span> innehåll
                                </li>
                                <li>
                                    <ul>
                                        <li><span>v0.2.1.4</span> f.d. plan- och bygglovsavdelningen arkivseri</li>
                                        <li><span>v0.2.1.3</span> f.d. Landskrona stads Stadsingenjörskontorets arkivserie, sedermera planregistrets kompletterande kommunegen akt</li>
                                        <li><span>v0.2.1.2</span> planförkortning</li>
                                        <li><span>v0.2.1.1</span> formell akt registrerad i planregistret</li>
                                    </ul>
                                </li>
                                <li><span>v0.2.1</span> rubrik
                                </li>
                            </ul>
                        </li>
                        <li><span>v0.2</span> grafiskt gränssnitt, GUI
                        </li>
                        <li><span>v0.1</span> webbtjänst
                            <ul>
                                <li>
                                    <ul>
                                        <li><span>v0.1.4.3</span> antal planer inom genomförandetid</li>
                                        <li><span>v0.1.4.2</span> antal olika planer</li>
                                        <li><span>v0.1.4.1</span> totalt antal planer</li>
                                    </ul>
                                </li>
                                <li><span>v0.1.4</span> statistik
                                </li>
                                <li>
                                    <ul>
                                        <li><span>v0.1.3.7</span> begränsning av dokument</li>
                                        <li><span>v0.1.3.6</span> redovisning av eventuell specifikt informationsfält</li>
                                        <li><span>v0.1.3.5</span> redovisning av söksträng</li>
                                        <li>
                                            <ul>
                                                <li><span>v0.1.3.2</span> typ av dokument</li>
                                                <li><span>v0.1.3.3</span> alternativt begränsning för sökmöjlighet efter olika informationsfält eller sökning på alla</li>
                                                <li><span>v0.1.3.4</span> sökparamerisering</li>
                                            </ul>
                                        </li>
                                        <li><span>v0.1.3.1</span> URL Routing
                                        </li>
                                    </ul>
                                </li>
                                <li><span>v0.1.3</span> sök
                                </li>
                                <li><span>v0.1.2</span> plandokument</li>
                                <li><span>v0.1.1</span> planinformation</li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>



            <div>
                <h4 id="asvnitt2">vNext</h4>
                <div id="vnext">
                    <ul>
                        <li>Rubrik "Kartöversikt" ändras till "Kartöversikt - gällande planområde"</li>
                        <li>Sökvillkor NYCKEL i kod, ändras till plannyckel för konsekvens jmfr. FASTIGHETNYCKEL</li>
                        <li>Kartbild
                            <ul>
                                <li>cachning av kartbild
                                </li>
                            </ul>
                        </li>
                        <li>Beskrivande text (tooltip) till dokumentnamn för dokument som kan existera i andra dokument, t.ex. grundkarta, bestämmelser</li>
                        <li>Sökresultat
                            <ul>
                                <li>Träff på antal planer efter sökbegrepp
                                </li>
                            </ul>
                        </li>
                        <li>Redovisning från vilket/vilka begrepp planen fick träff på (sökinfo per plan)</li>
                        <li>Sökparameter
                            <ul>
                                <li>Sökta visas, t.ex. med annan färg, om ej träff</li>
                            </ul>
                        </li>
                        <li>Datum presenteras
                            <ul>
                                <li>lagakraftdatum
                                </li>
                                <li>beslutsdatum
                                </li>
                                <li>Genomförandetid
                                </li>
                            </ul>
                        </li>
                        <li>Maxbredd på container för planerna
                        </li>
                        <li>Integrera bilden på plankartan
                            <ul>
                                <li>visas som thumb nail
                                </li>
                                <li>integreras i kartbilden ...
                                </li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>

        </div>

    </form>
</body>
</html>
