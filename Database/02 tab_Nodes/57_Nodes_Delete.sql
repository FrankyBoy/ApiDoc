USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Nodes_Delete]
    @nodeId int,
	@author nvarchar(50),
	@reason nvarchar(max)
	
AS
BEGIN
    SET NOCOUNT ON;
	
    DECLARE @latestDate datetime
    set @latestDate = (SELECT MAX(fChangeDate) FROM tab_Nodes where fID = @nodeId)
	
	INSERT INTO tab_Nodes
		(fID, frParentId, fName, fDescription, fChangeDate, fAuthor, fChangeNote, fDeleted)
		SELECT TOP(1) fID, frParentId, fName, fDescription, GETUTCDATE(), @author, @reason, 'True'
		FROM tab_Nodes WHERE fID = @nodeId AND fChangeDate = @latestDate
		
END

GO


