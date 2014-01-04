USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tab_Nodes](
    [fID]           [int] NOT NULL,
    [frParentId]    [int] NOT NULL,
    [fName]			[nvarchar](50) NOT NULL,
	[fDescription]	[nvarchar](MAX),
    [fChangeDate]   [datetime] NOT NULL,
    [fAuthor]       [nvarchar](50) NOT NULL,
	[fChangeNote]	[nvarchar](MAX) NOT NULL,
    [fDeleted]      [bit] NOT NULL,
)

CREATE INDEX [IX_tab_Nodes_fChangeDate] ON [tab_Nodes] (fChangeDate)
CREATE INDEX [IX_tab_Nodes_fName] ON [tab_Nodes] (fName)
CREATE INDEX [IX_tab_Nodes_fID] ON [tab_Nodes] (fID)

GO


