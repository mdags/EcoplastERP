SELECT
  dbschemas.[name] AS '�ema'
 ,dbtables.[name] AS 'Tablo'
 ,dbindexes.[name] AS '�ndex'
 ,indexstats.alloc_unit_type_desc AS 'Ay�rma Birimi T�r�'
 ,indexstats.avg_fragmentation_in_percent AS 'Ortalama Par�alanma'
 ,indexstats.page_count AS 'Sayfa Say�s�'
FROM
  sys.dm_db_index_physical_stats(
    DB_ID()
   ,NULL
   ,NULL
   ,NULL
   ,NULL) AS indexstats
  INNER JOIN sys.tables dbtables
    ON dbtables.[object_id] = indexstats.[object_id]
  INNER JOIN sys.schemas dbschemas
    ON dbtables.[schema_id] = dbschemas.[schema_id]
  INNER JOIN sys.indexes AS dbindexes
    ON dbindexes.[object_id] = indexstats.[object_id] AND
       indexstats.index_id = dbindexes.index_id
WHERE
  indexstats.database_id = DB_ID() and indexstats.avg_fragmentation_in_percent > 0
ORDER BY
  indexstats.avg_fragmentation_in_percent DESC