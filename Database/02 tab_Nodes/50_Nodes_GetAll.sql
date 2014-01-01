USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Nodes_GetAll]
    @parentId       int,
    @showDeleted    bit = 'FALSE'
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT fID, fName, fDescription, fDeleted
    FROM (
        SELECT *, ROW_NUMBER() OVER (PARTITION BY fID ORDER BY fChangeDate DESC) AS InverseRevision
        from tab_Nodes
    ) x
    where x.InverseRevision=1
    AND frParentId = @parentId -- note: this is in the outer query so we dont show old versions of moved modules in the wrong APIs
    AND (fDeleted = 'FALSE' OR @showDeleted = 'TRUE')
END

GO

