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
CREATE PROCEDURE [dbo].[Apis_DeleteApi]
	@apiId int
	
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @latestDate = SELECT MAX(fChangeDate) FROM tab_Apis where fID = @apiId
	
	UPDATE tab_Apis
 	SET fDeleted='True'
 	WHERE fID = @apiId AND fChangeDate = @latestDate
			
END

GO


