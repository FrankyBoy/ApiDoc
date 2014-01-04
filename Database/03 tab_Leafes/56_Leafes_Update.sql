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

    INSERT INTO tab_Leafes (
		fID, frParentId, fName, frHttpVerb, fDescription,
		fRequiresAuthentication, fRequiresAuthorization, fRequestSample, fResponseSample,
		fChangeDate, fAuthor, fChangeNote, fDeleted)
    VALUES	(
		@id, @parentId, @name, @httpVerb, @description,
		@requiresAuthentication, @requiresAuthorization, @requestSample, @responseSample,
		GETUTCDATE(), @author, @changeNote, 'FALSE')
END

GO


