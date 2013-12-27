USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Methods_LookupId]
    @moduleId   int,
    @name       nvarchar(50)
    
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT fID
    FROM (
        SELECT fID, row_number() OVER(ORDER BY fChangeDate DESC) AS inverseRevision
        FROM tab_Methods
        WHERE frModuleId = @moduleId
        AND UPPER(fMethodName) = UPPER(@name)
    )x
    WHERE x.inverseRevision = 1


END

GO


