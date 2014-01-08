USE [ApiDoc]
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

    SELECT	tab_Nodes.fID, frParentId, fName, descr.fTextData as fDescription,
			fChangeDate, fAuthor, note.fTextData as fChangeNote, fDeleted, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
        FROM tab_Nodes
		JOIN tab_TextData descr ON descr.fID = frDescription
		JOIN tab_TextData note ON note.fID = frChangeNote
        WHERE tab_Nodes.fID = @id
        ORDER BY fRevisionNumber DESC
END

GO


