USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Apis_GetRevisions]
    @name nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; 
    
    declare @id int
    EXEC @id = Apis_LookupId @name, @result = @id OUTPUT
    
    SELECT fID, fApiName, fDescription, fChangeDate, fAuthor, fDeleted, row_number() OVER(ORDER BY fChangeDate ASC) AS fRevisionNumber
    FROM tab_Apis
    WHERE fID = @id
    ORDER BY fRevisionNumber DESC
END

GO


