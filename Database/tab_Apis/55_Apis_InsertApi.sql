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
	@apiName nvarchar(50),
	@description nvarchar(max),
	@author nvarchar(50),
	@isUpdate bit

AS
BEGIN
	SET NOCOUNT ON;

	declare @count int
	set @count = (select COUNT(*) from tab_Apis where UPPER(fApiName) = UPPER(@apiName))

	IF @isUpdate != Convert(Bit, Case When @count > 0 Then 1 Else 0 End)
	BEGIN
		RETURN 1
	END

    INSERT INTO tab_Apis (fApiName, fDescription, fChangeDate, fAuthor, fDeleted)
	VALUES (@apiName, @description, GETUTCDATE(), @author, 'FALSE')
END

GO


