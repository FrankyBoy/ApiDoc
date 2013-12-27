USE [PosDocumentation]
GO

/****** Object:  StoredProcedure [dbo].[GetMethodById]    Script Date: 19.12.2013 17:41:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetMethodById]
	@methodId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		fID,
		frServiceId,
		fMethodName,
		frHttpVerb,
		fRequiresAuthentication,
		fRequiresAuthorization,
		fRequestSample,
		fResponseSample,
		fDescription
	FROM tab_Methods
	WHERE fID = @methodId
END

GO


