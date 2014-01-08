USE [ApiDoc]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Nodes_Update]
    @parentId		int,
    @id				int,
    @name			nvarchar(50),
	@description	nvarchar(max),
    @author			nvarchar(50),
	@changeNote		nvarchar(max)

AS
BEGIN
    SET NOCOUNT ON;
	
    IF (select COUNT(*) from tab_Nodes where fID = @id) <= 0
    BEGIN
        RETURN -1
    END
	
	declare @descriptionId int
	declare @changeNoteId int
	EXEC TextData_InsertOrGetIndex @text = @description, @result = @descriptionId OUTPUT
	EXEC TextData_InsertOrGetIndex @text = @changeNote, @result = @changeNoteId OUTPUT
	
    INSERT INTO tab_Nodes (fID, frParentId, fName, frDescription, fChangeDate, fAuthor, frChangeNote, fDeleted)
    VALUES (@id, @parentId, @name, @descriptionId, GETUTCDATE(), @author, @changeNoteId, 'FALSE')
END

GO


