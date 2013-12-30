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

    SELECT fID, fModuleName, fChangeDate, fAuthor, fDeleted, fRevisionNumber
    FROM (
        SELECT *, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
        FROM tab_Modules
        WHERE fID = @moduleId
    )x
    WHERE x.fRevisionNumber = COALESCE(@revision, MAX(x.fRevisionNumber))--(SELECT Count(*) FROM tab_Modules WHERE fID = @moduleId))


END

GO

