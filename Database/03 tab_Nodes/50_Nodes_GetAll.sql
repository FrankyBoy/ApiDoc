USE [ApiDoc]
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

    SELECT x.fID, fName, descr.fTextData as fDescription, fDeleted
    FROM (
        SELECT *, ROW_NUMBER() OVER (PARTITION BY fID ORDER BY fChangeDate DESC) AS InverseRevision
        from tab_Nodes
    ) x
	JOIN tab_TextData descr ON descr.fID = x.frDescription
    where x.InverseRevision=1
    AND frParentId = COALESCE(@parentId, frParentId) -- if parentId == null return all
    AND (fDeleted = 'FALSE' OR @showDeleted = 'TRUE')
END

GO


