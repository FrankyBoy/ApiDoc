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
		fID, frParentId, fName, frHttpVerb, fDescription,
		fRequiresAuthentication, fRequiresAuthorization, fRequestSample, fResponseSample,
		fChangeDate, fAuthor, fChangeNote, fDeleted, fRevisionNumber
    FROM allRevisions
    WHERE fRevisionNumber = COALESCE(@revision, (SELECT MAX(fRevisionNumber) FROM allRevisions))

END

GO
