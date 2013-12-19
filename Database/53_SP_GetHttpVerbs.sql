USE [PosDocumentation]
GO

/****** Object:  StoredProcedure [dbo].[GetHttpVerbs]    Script Date: 19.12.2013 17:41:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetHttpVerbs]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT fID, fHttpVerb
	FROM tab_HttpVerbs
END

GO


