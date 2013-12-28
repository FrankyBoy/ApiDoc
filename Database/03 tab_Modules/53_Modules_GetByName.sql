USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Modules_GetByName]
    @apiId      int,
    @name       nvarchar(50),
    @revision   int = null
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    declare @id int
    EXEC @id = Modules_LookupId @apiId, @name
    
    EXEC Modules_GetById @id, @revision
    
END

GO


