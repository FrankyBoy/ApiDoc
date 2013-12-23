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

    SELECT fID, fApiName, fDescription, fChangeDate, fAuthor, fDeleted, fRevisionNumber
    FROM (
        SELECT *, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
        FROM tab_Apis
        WHERE fID = @apiId
        
    )x
    WHERE x.fRevisionNumber = COALESCE(@revision, (SELECT Count(*) FROM tab_Apis WHERE fID = @apiId))


END

GO


