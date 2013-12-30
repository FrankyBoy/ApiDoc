USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Apis_LookupId]
    @name       nvarchar(50)

AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT TOP(1) fID
    FROM tab_Apis
    WHERE UPPER(fApiName) = UPPER(@name)
    ORDER BY fChangeDate DESC
    
        
END

GO


