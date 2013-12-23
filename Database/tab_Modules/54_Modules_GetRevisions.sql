USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Modules_GetApiRevisions]
	@moduleId int
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT 
		fModuleName, fChangeDate, fAuthor, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
	FROM tab_Modules
	WHERE fID = @moduleId
	ORDER BY fRevisionNumber DESC
END

GO


