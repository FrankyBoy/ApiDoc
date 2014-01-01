USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Apis_GetById]
    @apiId      int,
    @revision   int = null
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	WITH allRevisions
	AS (SELECT *, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
		FROM tab_Apis
		WHERE fID = 1)

    SELECT fID, fApiName, fDescription, fChangeDate, fAuthor, fDeleted, fRevisionNumber
    FROM allRevisions
    WHERE fRevisionNumber = COALESCE(@revision, (SELECT MAX(fRevisionNumber) FROM allRevisions))
	 
END

GO


