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
		fID int not null,
		frServiceId int not null,
		fMethodName nvarchar(100) not null,
		frHttpVerb int not null,
		fRequiresAuthentication bit not null,
		fRequiresAuthorization bit not null,
		fRequestSample nvarchar(max),
		fResponseSample nvarchar(max),
		fDescription nvarchar(max) not null,
		fChangeDate datetime not null,
		fAuthor nvarchar(50) not null,
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


