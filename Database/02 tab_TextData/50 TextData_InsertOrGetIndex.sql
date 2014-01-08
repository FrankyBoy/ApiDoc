USE [ApiDoc]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[TextData_InsertOrGetIndex]
    @text	nvarchar(max),
	@result	int OUTPUT
    
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	declare @hash varbinary(256)
	select @hash = HASHBYTES('SHA2_256', CONVERT(nvarchar(4000), @text))


	SELECT @result = fID
		FROM tab_TextData
		WHERE (@hash is null AND fTextHash is null) OR (fTextHash = @hash)
		AND fTextData = @text
	
	IF @result = null
	BEGIN
		INSERT INTO tab_TextData (fTextData, fTextHash)
			VALUES (@text, @hash)

		SET @result = SCOPE_IDENTITY()
	END
END