USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Modules_Delete]
	@moduleId int
	
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @latestDate datetime
	set @latestDate = SELECT MAX(fChangeDate) FROM tab_Modules where fID = @moduleId
	
	UPDATE tab_Modules
 	SET fDeleted='True'
 	WHERE fID = @moduleId AND fChangeDate = @latestDate
			
END

GO


