USE [ApiDoc]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tab_TextData](
    [fID]       [int] IDENTITY(1,1) PRIMARY KEY,
	[fTextHash]	[varbinary](256),
	[fTextData] [nvarchar](MAX)
)

CREATE INDEX [IX_tab_TextData_fTextHash] ON [tab_TextData] (fTextHash)

GO


