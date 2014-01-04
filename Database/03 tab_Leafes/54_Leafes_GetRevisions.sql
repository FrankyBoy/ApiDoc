USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Leafes_GetRevisions]
    @parentId int,
    @name nvarchar(50),
	@httpVerb int
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    declare @id int
    EXEC Leafes_LookupId @parentId, @name, @httpVerb, @result = @id OUTPUT

    SELECT 
		fID, frParentId, fName, frHttpVerb, fDescription,
		fRequiresAuthentication, fRequiresAuthorization, fRequestSample, fResponseSample,
		fChangeDate, fAuthor, fChangeNote, fDeleted, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
        FROM tab_Leafes
        WHERE fID = @id
        ORDER BY fRevisionNumber DESC
END

GO


