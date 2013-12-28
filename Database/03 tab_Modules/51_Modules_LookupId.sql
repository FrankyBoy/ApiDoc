USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Modules_LookupId]
    @apiId      int,
    @name       nvarchar(50)
    
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT TOP(1) fID
    FROM tab_Modules
    ORDER BY fChangeDate DESC
    WHERE frApiId = @apiId
    AND UPPER(fModuleName) = UPPER(@name)


END

GO


