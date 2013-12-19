USE [PosDocumentation]
GO

/****** Object:  StoredProcedure [dbo].[GetMethodByName]    Script Date: 19.12.2013 17:41:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetMethodByName]
	@moduleId int,
	@name nvarchar(100),
	@revision int = null
AS
BEGIN
	DECLARE @AllVersions TABLE
	(
		fID int,
		frServiceId int,
		fMethodName nvarchar(100),
		frHttpVerb int,
		fRequiresAuthentication bit,
		fRequiresAuthorization bit,
		fRequestSample nvarchar(max),
		fResponseSample nvarchar(max),
		fDescription nvarchar(max),
		fChangeDate datetime,
		fAuthor nvarchar(50),
		fRevisionNumber int
	);


	INSERT INTO @AllVersions
	SELECT *, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
	FROM tab_Methods
	WHERE frServiceId = @moduleId
	AND UPPER(fMethodName) = UPPER(@name)
		
	IF @revision is null 
	BEGIN
		SELECT TOP(1) * FROM @AllVersions
		ORDER BY fRevisionNumber DESC
	END
	ELSE
	BEGIN
		SELECT TOP(1) * FROM @AllVersions
		where fRevisionNumber = @revision
	END
END

GO


