USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Apis_GetByName]
    @name       nvarchar(50),
    @revision   int = null
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT fID, fApiName, fDescription, fChangeDate, fAuthor, fDeleted, fRevisionNumber
    FROM (
        SELECT *, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
        FROM tab_Apis
        WHERE UPPER(fApiName) = UPPER(@name)
        
    )x
    WHERE x.fRevisionNumber = COALESCE(@revision, (SELECT Count(*) FROM tab_Apis WHERE UPPER(fApiName) = UPPER(@name)))


END

GO


