-- Utredning
SELECT *
FROM fir_plan
WHERE lmakt = '1282-P28';

SELECT lmakt
FROM fir_plan
WHERE pb = 740;

DESC fir_plan;

SELECT *
FROM user_tab_columns
WHERE column_name LIKE '%HANVTYP%';

-- FIR_PLAN_HANVISN
-- FIR_PLAN_HANVTEXT
-- FIR_FIRKODER WHERE kodtyp = 'HANVTYP'

SELECT p1.pb, p1.lmakt, hvt.beskrivning, p2.lmakt, ht.hanvtext
FROM   fir_plan p1,
       fir_plan p2,
       fir_plan_hanvisn hv,
       fir_plan_hanvtext ht,
      (SELECT kodtyp as kodtyp, kodvarde as kodvarde, beskrivning as beskrivning FROM fir_firkoder WHERE kodtyp = 'HANVTYP') hvt
WHERE  hv.pb = p1.pb
AND    hv.pb_hanvisn = p2.pb
AND    hv.pb = ht.pb(+)
AND    hv.hanvtyp = hvt.kodvarde(+)
AND    p1.lmakt = '1282K-P01/179';

SELECT TO_CHAR(p1.pb) AS nyckel, p1.lmakt As akt, hvt1.beskrivning AS besk_reg, p2.lmakt AS akt_hanvisad, hvt2.beskrivning AS besk_oreg, ht.hanvtext AS text_oreg
FROM   fir_plan p1,
       fir_plan p2,
       fir_plan_hanvisn hv,
       fir_plan_hanvtext ht,
      (SELECT kodtyp as kodtyp, kodvarde as kodvarde, beskrivning as beskrivning FROM fir_firkoder WHERE kodtyp = 'HANVTYP') hvt1,
      (SELECT kodtyp as kodtyp, kodvarde as kodvarde, beskrivning as beskrivning FROM fir_firkoder WHERE kodtyp = 'HANVTYP') hvt2
WHERE  hv.pb = p1.pb
AND    hv.pb_hanvisn = p2.pb
AND    hv.pb = ht.pb(+)
AND    hv.hanvtyp = hvt1.kodvarde(+)
AND    ht.hanvtyp = hvt2.kodvarde(+)
ORDER BY p1.pb;


-- VY
-- Planer som påverkats av andra planbeslut
CREATE OR REPLACE FORCE VIEW gis_v_planpaverkade (plan_id, beskrivning, pav_plan_id, pav, registrerat_beslut)
AS
  (
      SELECT p1.pb, LOWER(kod.beskrivning), han.pb_hanvisn, han.lmakt, 1
      FROM   tefat.fir_plan p1,
            (SELECT h.pb pb, h.pb_hanvisn pb_hanvisn, h.hanvtyp hanvtyp, p2.lmakt lmakt FROM tefat.fir_plan_hanvisn h, tefat.fir_plan p2 WHERE h.pb_hanvisn = p2.pb) han,
             tefat.fir_firkoder kod
      WHERE  p1.pb = han.pb
      AND    han.hanvtyp = kod.kodvarde
      AND    kod.kodtyp = 'HANVTYP'
    UNION ALL
      SELECT p1.pb, LOWER(kod.beskrivning), NULL, hant.hanvtext, 0
      FROM   tefat.fir_plan p1,
            (SELECT h.pb pb, h.hanvtyp hanvtyp, h.hanvtext hanvtext FROM tefat.fir_plan_hanvtext h, tefat.fir_plan p2 WHERE h.pb = p2.pb) hant,
             tefat.fir_firkoder kod
      WHERE  p1.pb = hant.pb
      AND    hant.hanvtyp = kod.kodvarde
      AND    kod.kodtyp = 'HANVTYP');