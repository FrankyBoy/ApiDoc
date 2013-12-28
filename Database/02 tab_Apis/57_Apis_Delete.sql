USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Apis_Delete]
    @apiId int
    
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @latestDate datetime
    SET @latestDate = (SELECT MAX(fChangeDate) FROM tab_Apis where fID = @apiId)
    
    UPDATE tab_Apis
    SET fDeleted='True'
    WHERE fID = @apiId AND fChangeDate = @latestDate
            
END

GO


