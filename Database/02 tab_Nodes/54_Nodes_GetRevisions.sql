USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Nodes_GetRevisions]
    @parentId int,
    @name nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    declare @id int
    EXEC Nodes_LookupId @parentId, @name, @result = @id OUTPUT

    SELECT fID, fName, fDescription, fChangeDate, fAuthor, fChangeNote, fDeleted, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
        FROM tab_Nodes
        WHERE fID = @id
        ORDER BY fRevisionNumber DESC
END

GO


