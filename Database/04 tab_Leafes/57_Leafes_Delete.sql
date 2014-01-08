USE [ApiDoc]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Leafes_Delete]
    @id int,
	@author nvarchar(50),
	@reason nvarchar(max)
	
AS
BEGIN
    SET NOCOUNT ON;
	
    DECLARE @latestDate datetime
    set @latestDate = (SELECT MAX(fChangeDate) FROM tab_Leafes where fID = @id)
	
	
	declare @reasonId int
	EXEC TextData_InsertOrGetIndex @text = @reason, @result = @reasonId OUTPUT


	INSERT INTO tab_Leafes(
			fID, frParentId, fName, frHttpVerb, frDescription,
			fRequiresAuthentication, fRequiresAuthorization, frRequestSample, frResponseSample,
			fChangeDate, fAuthor, frChangeNote, fDeleted)
		(SELECT TOP(1) fID, frParentId, fName, frHttpVerb, frDescription,
			fRequiresAuthentication, fRequiresAuthorization, frRequestSample, frResponseSample,
			GETUTCDATE(), @author, @reasonId, 'True'
			FROM tab_Leafes WHERE fID = @id AND fChangeDate = @latestDate)
		
END

GO


