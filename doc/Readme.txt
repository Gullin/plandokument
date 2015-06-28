SETUP
 * Förinstallationer
    + Beroende av kartmotor AIMS eller MGOS
    + Beroende till två objekt i databas med namn och kolumner
       1) GIS_V_PLANYTOR
           a) NYCKEL
           b) AKT
           c) AKTTIDIGARE
           d) AKTEGN
           e) PLANFK
           f) PLANNAMN
           g) ISGENOMF
       2) GIS_V_PLANBERORFASTIGHET
           a) NYCKEL
           b) NYCKEL_FASTIGHET
           c) FASTIGHET
    + Säkerställ skrivrättigheter för fysiska installationskatalogens underkatalog "log" för IIS-användaren
      Skapas först vid första behovet av applikationen om den inte redan finns. Kan skapas manuellt innan.
    + Inskannade plandokuments virtuella eller fysiska katalognamn skrivs in i applikationens "Settings.config"
      Kan antingen vara fysisk katalog som ligger under applikationsinstallationen eller
      en virtuell katalog under applikationsinstallationen om den fysiska platsen är annan (behöver dock vara åtkomlig från servern)
    + DotNetZip
      http://dotnetzip.codeplex.com/
      Bibliotek för packetering (zip) av plandokument
    + jQuery 1.9
    + jQuery UI 1.9.2
    + JSON 3
    + jTable