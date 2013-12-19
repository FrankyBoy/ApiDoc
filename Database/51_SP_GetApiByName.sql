USE [PosDocumentation]
GO

/****** Object:  StoredProcedure [dbo].[GetApiByName]    Script Date: 19.12.2013 17:41:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetApiByName]
	@name nvarchar(50) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT fID, fApiName, fDescription
	FROM tab_Apis
	WHERE UPPER(fApiName) = UPPER(@name)
END

GO


