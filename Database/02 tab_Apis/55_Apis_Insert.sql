USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Apis_Insert]
    @apiName 		nvarchar(50),
    @description 	nvarchar(max),
    @author 		nvarchar(50)

AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @currentApis TABLE (
        fID        		int,
        fApiName        nvarchar(50),
        fDescription    nvarchar(max),
        fDeleted        bit
    )
	
	INSERT INTO @currentApis
	EXEC Apis_GetAll @showDeleted='False';
    
    IF (SELECT COUNT(*) FROM @currentApis WHERE UPPER(fApiName)=UPPER(@apiName)) > 0
    BEGIN
        RETURN -1
    END
    
    declare @newId int
    set @newId = (SELECT MAX(fID) from tab_Apis) + 1
	
    INSERT INTO tab_Apis (fID, fApiName, fDescription, fChangeDate, fAuthor, fDeleted)
    VALUES (@newId, @apiName, @description, GETUTCDATE(), @author, 'FALSE')
    
    RETURN @newId
END

GO


