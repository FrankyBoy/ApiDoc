USE [ApiDoc]
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
	
	declare @reasonId int
	EXEC TextData_InsertOrGetIndex @text = @reason, @result = @reasonId OUTPUT

	INSERT INTO tab_Nodes
		(fID, frParentId, fName, frDescription, fChangeDate, fAuthor, frChangeNote, fDeleted)
		SELECT TOP(1) fID, frParentId, fName, frDescription, GETUTCDATE(), @author, @reason, 'True'
		FROM tab_Nodes WHERE fID = @nodeId AND fChangeDate = @latestDate
		
END

GO


