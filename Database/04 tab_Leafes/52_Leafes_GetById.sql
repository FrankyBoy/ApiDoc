USE [ApiDoc]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Leafes_GetById]
    @id			int,
    @revision   int = null
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	
	WITH allRevisions
	AS (SELECT *, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
		FROM tab_Leafes
		WHERE fID = @id)

    SELECT 
		allRevisions.fID, frParentId, fName, frHttpVerb, descr.fTextData as fDescription,
		fRequiresAuthentication, fRequiresAuthorization, req.fTextData as fRequestSample, res.fTextData as fResponseSample,
		fChangeDate, fAuthor, note.fTextData as fChangeNote, fDeleted, fRevisionNumber
    FROM allRevisions
	JOIN tab_TextData descr ON descr.fID = frDescription
	JOIN tab_TextData note ON note.fID = frChangeNote
	JOIN tab_TextData req ON req.fID = frRequestSample
	JOIN tab_TextData res ON res.fID = frResponseSample
	WHERE fRevisionNumber = COALESCE(@revision, (SELECT MAX(fRevisionNumber) FROM allRevisions))

END

GO
