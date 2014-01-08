USE [ApiDoc]
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
		tab_Leafes.fID, frParentId, fName, frHttpVerb, descr.fTextData as fDescription,
		fRequiresAuthentication, fRequiresAuthorization, req.fTextData as fRequestSample, res.fTextData as fResponseSample,
		fChangeDate, fAuthor, note.fTextData as fChangeNote, fDeleted, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
        FROM tab_Leafes
		JOIN tab_TextData descr ON descr.fID = frDescription
		JOIN tab_TextData note ON note.fID = frChangeNote
		JOIN tab_TextData req ON req.fID = frRequestSample
		JOIN tab_TextData res ON res.fID = frResponseSample
        WHERE tab_Leafes.fID = @id
        ORDER BY fRevisionNumber DESC
END

GO


