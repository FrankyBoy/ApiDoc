USE [ApiDoc]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Nodes_GetById]
    @id			int,
    @revision   int = null
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	
	WITH allRevisions
	AS (SELECT *, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
		FROM tab_Nodes
		WHERE fID = @id)

    SELECT allrevisions.fID, frParentId, fName, descr.fTextData as fDescription, fChangeDate, fAuthor, note.fTextData as fChangeNote, fDeleted, fRevisionNumber
	FROM allRevisions
	JOIN tab_TextData descr ON descr.fID = frDescription
	JOIN tab_TextData note ON note.fID = frChangeNote
    WHERE fRevisionNumber = COALESCE(@revision, (SELECT MAX(fRevisionNumber) FROM allRevisions))

END

GO
