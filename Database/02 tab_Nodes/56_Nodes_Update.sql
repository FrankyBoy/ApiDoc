USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Nodes_Update]
    @parentId		int,
    @id				int,
    @name			nvarchar(50),
	@description	nvarchar(max),
    @author			nvarchar(50),
	@changeNote		nvarchar(max)

AS
BEGIN
    SET NOCOUNT ON;
	
    IF (select COUNT(*) from tab_Nodes where fID = @id) <= 0
    BEGIN
        RETURN -1
    END

    INSERT INTO tab_Nodes (fID, frParentId, fName, fDescription, fChangeDate, fAuthor, fChangeNote, fDeleted)
    VALUES (@id, @parentId, @name, @description, GETUTCDATE(), @author, @changeNote, 'FALSE')
END

GO


