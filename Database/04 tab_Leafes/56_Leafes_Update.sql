USE [ApiDoc]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Leafes_Update]
    @parentId		int,
    @id				int,
    @name			nvarchar(50),
	@httpVerb		int,
	@description	nvarchar(max),
    @requiresAuthentication	bit,
	@requiresAuthorization	bit,
	@requestSample	nvarchar(max),
	@responseSample	nvarchar(max),
    @author			nvarchar(50),
	@changeNote		nvarchar(max)


AS
BEGIN
    SET NOCOUNT ON;
	
    IF (select COUNT(*) from tab_Leafes where fID = @id) <= 0
    BEGIN
        RETURN -1
    END
	
		
	declare @descriptionId int
	declare @changeNoteId int
	declare @requestId int
	declare @responseId int
	EXEC TextData_InsertOrGetIndex @text = @description, @result = @descriptionId OUTPUT
	EXEC TextData_InsertOrGetIndex @text = @changeNote, @result = @changeNoteId OUTPUT
	EXEC TextData_InsertOrGetIndex @text = @requestSample, @result = @requestId OUTPUT
	EXEC TextData_InsertOrGetIndex @text = @responseSample, @result = @responseId OUTPUT


    INSERT INTO tab_Leafes (
		fID, frParentId, fName, frHttpVerb, frDescription,
		fRequiresAuthentication, fRequiresAuthorization, frRequestSample, frResponseSample,
		fChangeDate, fAuthor, frChangeNote, fDeleted)
    VALUES	(
		@id, @parentId, @name, @httpVerb, @descriptionId,
		@requiresAuthentication, @requiresAuthorization, @requestId, @responseId,
		GETUTCDATE(), @author, @changeNoteId, 'FALSE')
END

GO


