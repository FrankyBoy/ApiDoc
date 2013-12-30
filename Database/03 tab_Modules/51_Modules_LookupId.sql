USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Modules_LookupId]
    @apiId      int,
    @name       nvarchar(50),
	@result		int OUTPUT
    
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SET @result = 
		(SELECT TOP(1) fID
			FROM tab_Modules
			WHERE frApiId = @apiId
			AND UPPER(fModuleName) = UPPER(@name)
			ORDER BY fChangeDate DESC
		)

END

GO


