WITH
  -- Temporär tabell med alla aktbeteckningar
  input_ids AS (
      SELECT unnest(ARRAY[@search_string]) AS id     -- Två poster med olika sökningar, representerar enkel- och multi-geometri
  ),
  -- Räknar antalet totalt sökta
  id_count AS (
    SELECT COUNT(*) AS total_ids
    FROM input_ids
  ),
  -- Hämtar geometrier som matchar posterna i input_ids
  matched AS (
    SELECT
      i.id AS input_id,
      ST_Force2D(l.geom) AS geom,
      l.rk_extid
    FROM input_ids i
    JOIN td_drk.rk_plan_y l ON l.rk_extid = i.id
  ),
  -- Grupperar per input_id och samlar geometrier och namn
  grouped AS (
    SELECT
      input_id,
      COUNT(*) AS cnt,
      array_agg(ST_AsGeoJSON(geom, 6, 8)::TEXT) AS geojson_arr,
      array_agg(rk_extid) AS name_arr
    FROM matched
    GROUP BY input_id
  ),
  -- Kombinera de grupperade geometrierna med totala antalet för möjlig aggreggering senare
  combined AS (
    SELECT g.*, ic.total_ids
    FROM grouped g
    CROSS JOIN id_count ic
  )
SELECT
  CASE
    WHEN (SELECT total_ids FROM id_count) = 0 THEN
      '{}'::JSON::TEXT  -- tomt JSON-objekt som textsträng om ingen sökträff
    WHEN (SELECT COUNT(*) FROM combined) = 0 THEN
      '{}'::JSON::TEXT  -- tomt JSON-objekt som textsträng om ingen matchning trots att total_ids > 0
    WHEN (SELECT total_ids FROM id_count) = 1 THEN
      (
        SELECT
          CASE
            WHEN cnt = 1 THEN
              geojson_arr[1]
            ELSE
              (
                SELECT ST_AsGeoJSON(ST_Multi(ST_Union(polygons.geom)))
                FROM (
                  SELECT ST_GeomFromGeoJSON(geojson::json) AS geom
                  FROM unnest(geojson_arr) AS geojson
                ) polygons
              )
          END
        FROM combined
        LIMIT 1
      )
    ELSE
      (
        SELECT json_build_object(
          'type',     'FeatureCollection',
          'features', json_agg(
            CASE
              WHEN cnt = 1 THEN
                json_build_object(
                  'type',     'Feature',
                  'id',       input_id,
                  'geometry', geojson_arr[1]::JSON,
                  'properties',  '{}'::JSON
                )
              ELSE
                json_build_object(
                  'type',     'Feature',
                  'id',       input_id,
                  'geometry', (
                    SELECT ST_AsGeoJSON(ST_Multi(ST_Union(polygons.geom)))
                    FROM (
                      SELECT ST_GeomFromGeoJSON(geojson::json) AS geom
                      FROM unnest(geojson_arr) AS geojson
                    ) polygons
                  )::JSON,
                  'properties',  '{}'::JSON
                )
            END
          )
        )::TEXT
        FROM combined
      )
  END AS result;