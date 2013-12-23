USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Modules_GetByName]
    @apiId      int,
    @name       nvarchar(50),
    @revision   int = null
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT fID, fModuleName, fChangeDate, fAuthor, fDeleted, fRevisionNumber
    FROM (
        SELECT *, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
        FROM tab_Modules
        WHERE UPPER(fModuleName) = UPPER(@name)
        AND frApiId = @apiId
    )x
    WHERE x.fRevisionNumber = COALESCE(@revision, (SELECT Count(*) FROM tab_Modules WHERE UPPER(fModuleName) = UPPER(@name)))


END

GO


