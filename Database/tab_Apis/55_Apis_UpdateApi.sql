USE [PosDocumentation]
GO

/****** Object:  StoredProcedure [dbo].[GetMethodRevisions]    Script Date: 19.12.2013 17:53:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Apis_InsertApi]
	@apiId        int,
	@apiName      nvarchar(50),
	@description  nvarchar(max),
	@author       nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	IF NOT ANY(select fID from tab_Apis where fID = @apiId)
	BEGIN
		RETURN -1
	END

    INSERT INTO tab_Apis (fID, fApiName, fDescription, fChangeDate, fAuthor, fDeleted)
	VALUES (@apiId, @apiName, @description, GETUTCDATE(), @author, 'FALSE')
END

GO


