-- Planer som påverkats av andra planbeslut
CREATE OR REPLACE FORCE VIEW gis_v_planpaverkade (plan_id, beskrivning, pav_plan_id, pav_akt, registrerad)
AS
  (
      SELECT p1.pb, LOWER(kod.beskrivning), han.pb_hanvisn, han.lmakt, 'REGISTRERAT PLANBESLUT'
      FROM   tefat.fir_plan p1,
            (SELECT h.pb pb, h.pb_hanvisn pb_hanvisn, h.hanvtyp hanvtyp, p2.lmakt lmakt FROM tefat.fir_plan_hanvisn h, tefat.fir_plan p2 WHERE h.pb_hanvisn = p2.pb) han,
             tefat.fir_firkoder kod
      WHERE  p1.pb = han.pb
      AND    han.hanvtyp = kod.kodvarde
      AND    kod.kodtyp = 'HANVTYP'
    UNION ALL
      SELECT p1.pb, LOWER(kod.beskrivning), NULL, hant.hanvtext, 'EJ REGISTRERAT PLANBESLUT'
      FROM   tefat.fir_plan p1,
            (SELECT h.pb pb, h.hanvtyp hanvtyp, h.hanvtext hanvtext FROM tefat.fir_plan_hanvtext h, tefat.fir_plan p2 WHERE h.pb = p2.pb) hant,
             tefat.fir_firkoder kod
      WHERE  p1.pb = hant.pb
      AND    hant.hanvtyp = kod.kodvarde
      AND    kod.kodtyp = 'HANVTYP');