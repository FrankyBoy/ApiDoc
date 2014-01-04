USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Leafes_GetByName]
    @parentId   int,
    @name       nvarchar(50),
	@httpVerb	int,
    @revision   int = null
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    declare @id int
    EXEC Leafes_LookupId @parentId, @name, @httpVerb, @result = @id OUTPUT
    
    EXEC Leafes_GetById @id, @revision
    
END

GO


