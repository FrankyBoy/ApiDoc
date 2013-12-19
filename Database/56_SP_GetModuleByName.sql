USE [PosDocumentation]
GO

/****** Object:  StoredProcedure [dbo].[GetModuleByName]    Script Date: 19.12.2013 17:41:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetModuleByName]
	@name	nvarchar(50),
	@apiId	int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT fID, fServiceName
	FROM tab_Modules
	WHERE UPPER(fServiceName) = UPPER(@name)
		AND frApiId = @apiId
END

GO


