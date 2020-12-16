A Program egy kisvárosi mozi jegyfoglaló rendszerét valósítja meg, annak is az office verzióját, ahol a mozi pénztárában dolgozó eladók rögzítik a jegyeladásokat.
A program képes szűrni a műsorokra film és dátum szerint is, valamint folyamatosan frissíti az előadások szerinti összesített táblázatot.
A programhoz tartozó adatbázis csupán 4 táblát tartalmaz az egyszerűség kedvéért. A Terem tábla az egyetlen teremben található összes székhez hozzárendel egy egyedi azonosítót, a Foglalás tábla pedig az összes eddigi foglalást tartalmazza. Egy műsorhoz annyi rekord tartozik, ahány szék található a teremben, így elég csak foglalt cella értékét változtatni (bool).
A műsor kiválasztása után betölt a terem ülésrajza, megkülönböztetve a foglalt, szabad és kiválasztott ülőhelyeket. 
A Mégse gombra kattintva megszűnnek az eddigi kiválasztások.
Mentéskor egy ablak kiírja a foglalás részleteit, azt elfogadva frissül az adatbázis és az ülésrajz.
Exportáláskor egy összesített táblázatot kapunk Excel-ben, ahol műsoronként láthatóak a részletek, beleértve az eladott jegyek számát és az abból származó bevételt.
Fontos, hogy az adatbázis nincs teljesen feltöltve adatokkal, így csak -a filmcím alapján sorbarendezett- első 10 film, legkorábbi előadásához lehet jegyet "vásárolni", de ezt a program MessageBox-okkal le is kezeli.