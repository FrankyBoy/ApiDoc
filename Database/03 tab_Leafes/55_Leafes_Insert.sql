USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Leafes_Insert]
    @parentId		int,
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

    DECLARE @currentLeafes TABLE (
        fID				int,
        fName			nvarchar(50),
		frHttpverb		bit,
		fDescription	nvarchar(max),
        fDeleted        bit
    )
	
	INSERT INTO @currentLeafes
	EXEC Leafes_GetAll @parentId, @showDeleted='False';

    IF (SELECT COUNT(*) FROM @currentLeafes WHERE UPPER(fName)=UPPER(@name)) > 0
    BEGIN
        RETURN -1
    END
    
    declare @newId int
    set @newId = (SELECT COALESCE(MAX(fID),0) from tab_Leafes) + 1

    INSERT INTO tab_Leafes (
		fID, frParentId, fName, frHttpVerb, fDescription,
		fRequiresAuthentication, fRequiresAuthorization, fRequestSample, fResponseSample,
		fChangeDate, fAuthor, fChangeNote, fDeleted)
    VALUES	(
		@newId, @parentId, @name, @httpVerb, @description,
		@requiresAuthentication, @requiresAuthorization, @requestSample, @responseSample,
		GETUTCDATE(), @author, @changeNote, 'FALSE')
    
    RETURN @newId
END

GO


