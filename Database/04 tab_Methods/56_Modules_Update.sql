USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Modules_Update]
    @apiId          int,
    @moduleId       int,
    @moduleName     nvarchar(50),
    @author         nvarchar(50)

AS
BEGIN
    SET NOCOUNT ON;

    IF NOT ANY(select fID from tab_Modules where fID = @moduleId)
    BEGIN
        RETURN -1
    END

    INSERT INTO tab_Modules (fID, frApiId, fModuleName, fChangeDate, fAuthor, fDeleted)
    VALUES (@moduleId, @apiId, @moduleName, GETUTCDATE(), @author, 'FALSE')
END

GO


