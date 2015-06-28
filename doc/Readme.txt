SETUP
 * F�rinstallationer
    + Beroende av kartmotor AIMS eller MGOS
    + Beroende till tv� objekt i databas med namn och kolumner
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
    + S�kerst�ll skrivr�ttigheter f�r fysiska installationskatalogens underkatalog "log" f�r IIS-anv�ndaren
      Skapas f�rst vid f�rsta behovet av applikationen om den inte redan finns. Kan skapas manuellt innan.
    + Inskannade plandokuments virtuella eller fysiska katalognamn skrivs in i applikationens "Settings.config"
      Kan antingen vara fysisk katalog som ligger under applikationsinstallationen eller
      en virtuell katalog under applikationsinstallationen om den fysiska platsen �r annan (beh�ver dock vara �tkomlig fr�n servern)
    + DotNetZip
      http://dotnetzip.codeplex.com/
      Bibliotek f�r packetering (zip) av plandokument
    + jQuery 1.9
    + jQuery UI 1.9.2
    + JSON 3
    + jTable