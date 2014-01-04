USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Leafes_LookupId]
    @parentId   int,
    @name       nvarchar(50),
	@httpVerb	int,
	@result		int OUTPUT

AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SET @result = 
		(SELECT TOP(1) fID
			FROM tab_Leafes
			WHERE frParentId = @parentId
			AND UPPER(fName) = UPPER(@name)
			AND frHttpVerb = COALESCE(@httpVerb, frHttpVerb)
			ORDER BY fChangeDate DESC
		)

END

GO


