USE [PosDocumentation]
GO

/****** Object:  StoredProcedure [dbo].[GetAPIs]    Script Date: 19.12.2013 17:41:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Apis_GetAllApis]
	@showDeleted bit = 'FALSE'
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT fID, fApiName, fDescription, fDeleted
	FROM (
		SELECT *, ROW_NUMBER() OVER (PARTITION BY fID ORDER BY fChangeDate DESC) AS InverseRevision
		from tab_Apis
	) x
	where x.InverseRevision=1
	AND (fDeleted = 'FALSE' OR @showDeleted = 'TRUE')
END

GO


