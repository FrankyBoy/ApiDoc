USE [ApiDoc]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Nodes_Insert]
    @parentId		int,
    @name			nvarchar(50),
	@description	nvarchar(max),
    @author			nvarchar(50),
	@changeNote		nvarchar(max)

AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @currentNodes TABLE (
        fID				int,
        fName			nvarchar(50),
		fDescription	nvarchar(max),
        fDeleted        bit
    )
	
	INSERT INTO @currentNodes
	EXEC Nodes_GetAll @parentId, @showDeleted='False';

    IF (SELECT COUNT(*) FROM @currentNodes WHERE UPPER(fName)=UPPER(@name)) > 0
    BEGIN
        RETURN -1
    END
    
    declare @newId int
    set @newId = (SELECT COALESCE(MAX(fID),0) from tab_Nodes) + 1

	declare @descriptionId int
	declare @changeNoteId int
	EXEC TextData_InsertOrGetIndex @text = @description, @result = @descriptionId OUTPUT
	EXEC TextData_InsertOrGetIndex @text = @changeNote, @result = @changeNoteId OUTPUT

	INSERT INTO tab_Nodes (fID, frParentId, fName, frDescription, fChangeDate,	fAuthor, frChangeNote, fDeleted)
    VALUES	(@newId, @parentId,	@name, @descriptionId, GETUTCDATE(), @author, @changeNoteId, 'FALSE')
    
    RETURN @newId
END

GO


