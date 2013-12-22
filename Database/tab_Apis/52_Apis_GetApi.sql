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
CREATE PROCEDURE [dbo].[Apis_GetApi]
	@name nvarchar(50),
	@revision int = null
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	SELECT fID, fApiName, fDescription, fChangeDate, fAuthor, fDeleted, fRevisionNumber
	FROM (
		SELECT fID, fApiName, fDescription, fChangeDate, fAuthor, fDeleted, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
		FROM tab_Apis
		WHERE UPPER(fApiName) = UPPER(@name)
		
	)x
	WHERE x.fRevisionNumber = COALESCE(@revision, (SELECT Count(*) FROM tab_Apis WHERE UPPER(fApiName) = UPPER(@name)))


END

GO


