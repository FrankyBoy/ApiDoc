USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Modules_GetRevisions]
    @apiId int,
    @name nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    declare @id int
    EXEC Modules_LookupId @apiId, @name, @result = @id OUTPUT

    SELECT fID, fModuleName, fChangeDate, fAuthor, fDeleted, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
        FROM tab_Modules
        WHERE fID = @id
        ORDER BY fRevisionNumber DESC
END

GO


