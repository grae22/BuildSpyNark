SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE spProcessImportData
GO

CREATE PROCEDURE spProcessImportData
AS
BEGIN
	SET NOCOUNT ON;

  BEGIN TRANSACTION

  -- Set up a cursor for import data table.
  DECLARE @filename nvarchar( 1000 )
  DECLARE @eventType nvarchar( 20 )
  DECLARE @timestamp smalldatetime

	DECLARE importDataCursor CURSOR LOCAL
    FORWARD_ONLY
	  FOR SELECT * FROM ImportData
    ORDER BY Filename

	OPEN importDataCursor

  FETCH NEXT FROM importDataCursor
  INTO @filename, @eventType, @timestamp

  DECLARE @buildStartFilename nvarchar( 1000 )
  DECLARE @buildStartId int
  DECLARE @buildEndId int

  SET @buildStartId = -1
  SET @buildEndId = -1

  WHILE @@FETCH_STATUS = 0
  BEGIN
    -- Get the project name.
    DECLARE @projectName nvarchar( 1000 )
    DECLARE @tag nvarchar( 1000 )
        
    DECLARE tagCursor CURSOR LOCAL
     FORWARD_ONLY
     FOR SELECT * FROM splitstring( @filename, '#' )

    OPEN tagCursor

    FETCH NEXT FROM tagCursor
    INTO @tag

    DECLARE @tagIndex int
    SET @tagIndex = 0;

    WHILE @@FETCH_STATUS = 0
    BEGIN
      IF @tagIndex = 2 SET @projectName = @tag

      SET @tagIndex = @tagIndex + 1

      FETCH NEXT FROM tagCursor
      INTO @tag
    END

    CLOSE tagCursor
    DEALLOCATE tagCursor

    -- Do we have the project name?
    IF LEN( @projectName ) > 0
    BEGIN
      DECLARE @projectId int

      SET @projectId = -1

      SELECT @projectId = Id
      FROM Projects
      WHERE LOWER( Name ) = @projectName

      -- Do we need to add the project?
      IF @projectId = -1
      BEGIN
        INSERT INTO Projects ( Name )
        OUTPUT Inserted.id
        VALUES ( @projectName )

        SET @projectId = SCOPE_IDENTITY()
      END

      -- Does the build event type already exist?
      DECLARE @eventTypeId int
      SET @eventTypeId = -1

      SELECT @eventTypeId = Id
      FROM BuildEventTypes
      WHERE LOWER( Description ) = @eventType

      IF @eventTypeId = -1
      BEGIN
        INSERT INTO BuildEventTypes ( Description )
        VALUES ( @eventType )

        SET @eventTypeId = SCOPE_IDENTITY()
      END

      -- Add the build event.
      INSERT INTO BuildEvents ( ProjectId, EventId, Timestamp )
      VALUES ( @projectId, @eventTypeId, @timestamp )

      DECLARE @buildId int
      SET @buildId = -1

      IF LOWER( @eventType ) = 'build.start'
      BEGIN
        SET @buildStartId = SCOPE_IDENTITY()
        SET @buildStartFilename = @filename
      END
      ELSE
      BEGIN
        IF @filename = @buildStartFilename AND @buildStartId > -1
        BEGIN
          SET @buildEndId = SCOPE_IDENTITY()

          INSERT INTO ProjectBuilds ( ProjectId, BuildStartEventId, BuildEndEventId )
          VALUES ( @projectId, @buildStartId, @buildEndId )

          SET @buildId = SCOPE_IDENTITY()
        END
        ELSE
        BEGIN
          IF @buildStartId > -1
            INSERT INTO ProjectBuilds ( ProjectId, BuildStartEventId, BuildEndEventId )
            VALUES ( @projectId, @buildStartId, null )

          SET @buildStartId = -1
          SET @buildEndId = -1
        END
      END

      -- Add the tags.
      IF @buildId > -1
      BEGIN
        DECLARE tagCursor2 CURSOR LOCAL
          FORWARD_ONLY
          FOR SELECT * FROM splitstring( @filename, '#' )

        OPEN tagCursor2

        FETCH NEXT FROM tagCursor2
        INTO @tag

        SET @tagIndex = 0;

        WHILE @@FETCH_STATUS = 0
        BEGIN
          -- Ignore first 2 tags.
          IF @tagIndex > 2
          BEGIN
            -- Get the tag id or add it.
            DECLARE @tagId int
            SET @tagId = -1

            SELECT @tagId = Id
            FROM Tags
            WHERE LOWER( Name ) = @tag

            IF @tagId = -1
            BEGIN
              INSERT INTO Tags ( Name )
              VALUES ( @tag )

              SET @tagId = SCOPE_IDENTITY()
            END

            INSERT INTO BuildTags ( TagId, BuildId )
            VALUES ( @tagId, @buildId )
          END

          SET @tagIndex = @tagIndex + 1

          FETCH NEXT FROM tagCursor2
          INTO @tag
        END

        CLOSE tagCursor2
        DEALLOCATE tagCursor2
      END
    END
    
    FETCH NEXT FROM importDataCursor
    INTO @filename, @eventType, @timestamp
  END

  CLOSE importDataCursor
  DEALLOCATE importDataCursor

  COMMIT TRANSACTION
END
GO
