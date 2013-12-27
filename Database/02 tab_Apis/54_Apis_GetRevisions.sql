USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Apis_GetApiRevisions]
    @name nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; 
    
    declare @id int
    set @id = EXEC Apis_LookupId @name
    
    SELECT 
        fApiName, fChangeDate, fAuthor, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
    FROM tab_Apis
    WHERE fID = @id
    ORDER BY fRevisionNumber DESC
END

GO


