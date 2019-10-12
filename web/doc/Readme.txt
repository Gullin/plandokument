SETUP
 * Förutsättningar
    + Beroende av kartmotor AIMS eller MGOS
    + Beroende till två objekt i databas med namn och kolumner
       - Vyer
          # LKR_GIS.GIS_V_PLANYTOR
             ¤ Kolumner
                NYCKEL
                AKT
                AKTTIDIGARE
                AKTEGN
                PLANFK
                PLANNAMN
                ISGENOMF
             ¤ Tabeller
                topo_ndrk.pl_plan_tsur
                topo_ndrk.pl_area
                topo_ndrk.pl_plan_tcen
                topo_ndrk.pl_area_planavgift
                topo_ndrk.pl_polygon
                topo_ndrk.pl_polygon_planavgift
                tefat.fir_plan
                tefat.fir_plan_egnauppg
                tefat.fir_plan_beslut
                tefat.fir_firkoder
          # LKR_GIS.GIS_V_PLANBERORFASTIGHET
             ¤ Kolumner
                NYCKEL
                NYCKEL_FASTIGHET
                FASTIGHET
             ¤ Tabeller
                tefat.fir_fastigh
                tefat.fir_plan_planberor
    + Säkerställ skrivrättigheter för fysiska installationskatalogens underkatalog "log" för IIS-användaren (ApplicationPool som används för webbapplikationen).
      Sök efter användare: IIS AppPool/[namn]
      Katalogen skapas först vid första behovet av applikationen om den inte redan finns. Kan skapas manuellt innan.
    + Säkerställ skrivrättigheter för fysiska installationskatalogens underkatalog "zipTemp" för IIS-användaren (ApplicationPool som används för webbapplikationen).
      Sök efter användare: IIS AppPool/[namn]
      Katalogen skapas först vid första behovet av applikationen om den inte redan finns. Kan skapas manuellt innan.
    + Inskannade plandokuments virtuella eller fysiska katalognamn skrivs in i applikationens "Settings.config"
      Kan antingen vara fysisk katalog som ligger under applikationsinstallationen eller
      en virtuell katalog under applikationsinstallationen om den fysiska platsen är annan (behöver dock vara åtkomlig från servern)
    + Ändra IIS applikationspool rättigheter under "Avancerade inställningar >> Processmodell >> Identitet" till LocalSystem.
      Behövs administratörsrättigheter för om Windows-tjänsten för thumnails ska kunna administreras från dashboard-sidan.
 * Beroenden
    + Windows-tjänst för tillverkning av thumnails-bilder till plankartorna
    + jQuery 3.4.1
    + jQuery UI 1.9.2
    + JSON 3
    + jTable 2.4.0
    + Bootstrap 4.1.2
    + Bootstrap 4.3.1