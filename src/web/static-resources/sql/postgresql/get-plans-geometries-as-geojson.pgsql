WITH
  planer AS (
    SELECT
      ST_Force2D(l.geom) AS geom,
      l.rk_extid
    FROM td_drk.rk_plan_y l
  ),
  grouped AS (
    SELECT
      COUNT(*) AS cnt,
      array_agg(ST_AsGeoJSON(geom, 6, 8)::TEXT) AS geojson_arr,
      rk_extid
    FROM planer
    GROUP BY rk_extid
  )
SELECT
  COALESCE(
      CASE
        WHEN cnt = 1 THEN
          json_build_object(
            'type',       'Feature',
            'id',         rk_extid,
            'geometry',   geojson_arr[1]::json,
            'properties', '{}'::json
          )
        ELSE
          json_build_object(
            'type',       'Feature',
            'id',         rk_extid,
            'geometry', (
              SELECT ST_AsGeoJSON(ST_Multi(ST_Union(polygons.geom)))::json
              FROM (
                SELECT ST_GeomFromGeoJSON(geojson::json) AS geom
                FROM unnest(grouped.geojson_arr) AS geojson
              ) polygons
            ),
            'properties', '{}'::json
          )
      END::text,
    '{}'::json::text
  ) AS result
FROM grouped