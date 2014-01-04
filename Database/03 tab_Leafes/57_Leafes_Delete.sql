USE [PosDocumentation]
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
	
	INSERT INTO tab_Leafes(
			fID, frParentId, fName, frHttpVerb, fDescription,
			fRequiresAuthentication, fRequiresAuthorization, fRequestSample, fResponseSample,
			fChangeDate, fAuthor, fChangeNote, fDeleted)
		(SELECT TOP(1) fID, frParentId, fName, frHttpVerb, fDescription,
			fRequiresAuthentication, fRequiresAuthorization, fRequestSample, fResponseSample,
			GETUTCDATE(), @author, @reason, 'True'
			FROM tab_Leafes WHERE fID = @id AND fChangeDate = @latestDate)
		
END

GO


