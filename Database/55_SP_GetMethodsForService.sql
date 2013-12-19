USE [PosDocumentation]
GO

/****** Object:  StoredProcedure [dbo].[GetMethodsForService]    Script Date: 19.12.2013 17:41:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetMethodsForService]
	-- Add the parameters for the stored procedure here
	@serviceId int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT fID, fMethodName, frHttpVerb 
	FROM (
		SELECT *, ROW_NUMBER() OVER (PARTITION BY fMethodName,frHttpVerb ORDER BY [fChangeDate] DESC) AS InverseRevision
		from tab_Methods
	) x
	where x.InverseRevision=1
END

GO


