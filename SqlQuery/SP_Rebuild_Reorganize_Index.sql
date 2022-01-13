--Bu SP parametre olarak ald��� DB�de bulunan index�lerin fragmante oranlar�n� sorguluyor, ��kan sonuca g�re fragmante oran� %30 dan fazla olanlar� Rebuild, az olanlar� ReOrganize ediyor. Sonu� olarakta ka� Index�i rebuild, ka� index�i Reorganize etti�i bilgisini d�n�yor.
USE MASTER
GO
CREATE PROC [INDEX_MAINTENANCE] @DBName VARCHAR(100)
AS BEGIN
		SET NOCOUNT ON;
		DECLARE
			@OBJECT_ID INT,
			@INDEX_NAME sysname,
			@SCHEMA_NAME sysname,
			@OBJECT_NAME sysname,
			@AVG_FRAG float,
			@command varchar(8000),
			@RebuildCount int,
			@ReOrganizeCount int

		CREATE TABLE #tempIM (
			[ID] [INT] IDENTITY(1,1) NOT NULL PRIMARY KEY,
			[INDEX_NAME] sysname NULL,
			[OBJECT_ID] INT NULL,
			[SCHEMA_NAME] sysname NULL,
			[OBJECT_NAME] sysname NULL,
			[AVG_FRAG] float
		)		
		SELECT @RebuildCount=0,@ReOrganizeCount=0
		
		--Get Fragentation values
		SELECT @command=
			'Use ' + @DBName + '; 
			INSERT INTO #tempIM (OBJECT_ID, INDEX_NAME, SCHEMA_NAME, OBJECT_NAME, AVG_FRAG)
			SELECT
				ps.object_id,
				i.name as IndexName,
				OBJECT_SCHEMA_NAME(ps.object_id) as ObjectSchemaName,
				OBJECT_NAME (ps.object_id) as ObjectName,
				ps.avg_fragmentation_in_percent
			FROM sys.dm_db_index_physical_stats (NULL, NULL, NULL , NULL, ''LIMITED'') ps
			INNER JOIN sys.indexes i ON i.object_id=ps.object_id and i.index_id=ps.index_id
			WHERE avg_fragmentation_in_percent > 5 AND ps.index_id > 0
				and ps.database_id=DB_ID('''+@DBName+''')
			ORDER BY avg_fragmentation_in_percent desc
			'
		
		exec(@command)
		DECLARE c CURSOR FAST_FORWARD FOR
			SELECT OBJECT_ID,INDEX_NAME, SCHEMA_NAME, OBJECT_NAME, AVG_FRAG
			FROM #tempIM
		OPEN c
		FETCH NEXT FROM c INTO @OBJECT_ID, @INDEX_NAME, @SCHEMA_NAME, @OBJECT_NAME, @AVG_FRAG
		WHILE @@FETCH_STATUS = 0
		BEGIN
			--Reorganize or Rebuild
			IF @AVG_FRAG>30 BEGIN
				SELECT @command = 'Use ' + @DBName + '; ALTER INDEX [' + @INDEX_NAME +'] ON [' 
								  + @SCHEMA_NAME + '].[' + @OBJECT_NAME + '] REBUILD   WITH (ONLINE = ON  )';
				SET @RebuildCount = @RebuildCount+1
			END ELSE BEGIN
				SELECT @command = 'Use ' + @DBName + '; ALTER INDEX [' + @INDEX_NAME +'] ON [' 
								  + @SCHEMA_NAME + '].[' + @OBJECT_NAME + '] REORGANIZE ';
				SET @ReOrganizeCount = @ReOrganizeCount+1
			END
								  
			BEGIN TRY 
				EXEC (@command);	 
			END TRY 
			BEGIN CATCH 
			END CATCH 
			
			FETCH NEXT FROM c INTO @OBJECT_ID, @INDEX_NAME, @SCHEMA_NAME, @OBJECT_NAME, @AVG_FRAG
		END
		CLOSE c
		DEALLOCATE c
		
		DROP TABLE #tempIM
		
		SELECT cast(@RebuildCount as varchar(5))+' index Rebuild,'+cast(@ReOrganizeCount as varchar(5))+' index Reorganize edilmistir.' as Result
END

--USE MASTER GO EXEC master.dbo.INDEX_MAINTENANCE 'EcoplastERP'