USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Apis_GetByName]
    @name       nvarchar(50),
    @revision   int = null
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    declare @id int
    set @id = EXEC Apis_LookupId @name
    
    EXEC Apis_GetById @id, @revision
        
END

GO


