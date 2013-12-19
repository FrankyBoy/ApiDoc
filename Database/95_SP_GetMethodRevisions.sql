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
CREATE PROCEDURE [dbo].[GetMethodRevisions]
	@moduleId int,
	@methodName nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT 
		fChangeDate, fAuthor, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
	FROM tab_Methods
	WHERE frServiceId = @moduleId
	AND UPPER(fMethodName) = UPPER(@methodName)
	ORDER BY fRevisionNumber DESC
END

GO


