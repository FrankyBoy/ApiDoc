USE [PosDocumentation]
GO

/****** Object:  StoredProcedure [dbo].[GetModulesForApi]    Script Date: 19.12.2013 17:41:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetModulesForApi]
	@ApiId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT fID, fServiceName
	FROM tab_Modules
	WHERE frApiId = COALESCE(NULLIF(@ApiId,0),frApiId)
END

GO


