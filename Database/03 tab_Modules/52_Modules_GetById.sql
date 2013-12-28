USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Modules_GetById]
    @moduleId   int,
    @revision   int = null
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	
	WITH allRevisions
	AS (SELECT *, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
		FROM tab_Modules
		WHERE fID = 1)

    SELECT fID, fModuleName, fChangeDate, fAuthor, fDeleted, fRevisionNumber
    FROM allRevisions
    WHERE fRevisionNumber = COALESCE(@revision, (SELECT MAX(fRevisionNumber) FROM allRevisions))

END

GO


