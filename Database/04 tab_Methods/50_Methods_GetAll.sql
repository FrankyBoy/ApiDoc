USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Methods_GetAll]
	@moduleId      int,
    @showDeleted    bit = 'FALSE'

AS
BEGIN
	SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	SELECT fID, fMethodName, frHttpVerb, fDeleted
	FROM (
		SELECT *, ROW_NUMBER() OVER (PARTITION BY fMethodName,frHttpVerb ORDER BY [fChangeDate] DESC) AS InverseRevision
		from tab_Methods
	) x
	where x.InverseRevision=1
	AND frModuleId = @moduleId -- note: this is in the outer query so we dont show old versions of moved modules in the wrong APIs
    AND (fDeleted = 'FALSE' OR @showDeleted = 'TRUE')
END

GO


