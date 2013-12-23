USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Apis_Update]
	@apiId        int,
	@apiName      nvarchar(50),
	@description  nvarchar(max),
	@author       nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	IF ANY(select fID from tab_Apis where fID = @apiId)
	BEGIN
		RETURN -1
	END

    INSERT INTO tab_Apis (fID, fApiName, fDescription, fChangeDate, fAuthor, fDeleted)
	VALUES (@apiId, @apiName, @description, GETUTCDATE(), @author, 'FALSE')
END

GO


