USE [ApiDoc]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tab_Nodes](
    [fID]           [int] NOT NULL,
    [frParentId]    [int] NOT NULL,
    [fName]			[nvarchar](50) NOT NULL,
	[frDescription]	[int] NOT NULL,
    [fChangeDate]   [datetime] NOT NULL,
    [fAuthor]       [nvarchar](50) NOT NULL,
	[frChangeNote]	[int] NOT NULL,
    [fDeleted]      [bit] NOT NULL,
)

CREATE INDEX [IX_tab_Nodes_fChangeDate] ON [tab_Nodes] (fChangeDate)
CREATE INDEX [IX_tab_Nodes_fName] ON [tab_Nodes] (fName)
CREATE INDEX [IX_tab_Nodes_fID] ON [tab_Nodes] (fID)
GO

ALTER TABLE [dbo].[tab_Nodes]  WITH CHECK ADD CONSTRAINT [FK_tab_Nodes_tab_TextData] FOREIGN KEY([frDescription])
REFERENCES [dbo].[tab_TextData] ([fID])
GO

ALTER TABLE [dbo].[tab_Nodes] CHECK CONSTRAINT [FK_tab_Nodes_tab_TextData]
GO

ALTER TABLE [dbo].[tab_Nodes]  WITH CHECK ADD CONSTRAINT [FK_tab_Nodes_tab_TextData1] FOREIGN KEY([frChangeNote])
REFERENCES [dbo].[tab_TextData] ([fID])
GO

ALTER TABLE [dbo].[tab_Nodes] CHECK CONSTRAINT [FK_tab_Nodes_tab_TextData1]
GO
