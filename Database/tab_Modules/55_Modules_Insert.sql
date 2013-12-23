USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Modules_Insert]
	@apiId 		int,
	@moduleName nvarchar(50),
	@author 	nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @currentModules TABLE (
		fID 			id,
		fModuleName     nvarchar(50),
		fDeleted 		bit
	)
	SET @currentModules = EXECUTE Modules_GetAll @apiId=@apiId, @showDeleted='False';
	
	IF (SELECT COUNT(*) FROM @currentApis WHERE UPPER(fModuleName)=UPPER(@moduleName)) > 0
	BEGIN
		RETURN -1
	END
	
	declare @newId int,
	set @newId = (SELECT MAX(fID) from tab_Module) + 1

    INSERT INTO tab_Modules (fID, frApiId, fModuleName, fChangeDate, fAuthor, fDeleted)
	VALUES (@newId, @apiId, @moduleName, GETUTCDATE(), @author, 'FALSE')
	
	RETURN @newId
END

GO


