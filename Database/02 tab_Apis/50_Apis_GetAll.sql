USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Apis_GetAll]
    @showDeleted bit = 'FALSE'
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT fID, fApiName, fDescription, fDeleted
    FROM (
        SELECT *, ROW_NUMBER() OVER (PARTITION BY fID ORDER BY fChangeDate DESC) AS InverseRevision
        from tab_Apis
    ) x
    where x.InverseRevision=1
    AND (fDeleted = 'FALSE' OR @showDeleted = 'TRUE')
END

GO


