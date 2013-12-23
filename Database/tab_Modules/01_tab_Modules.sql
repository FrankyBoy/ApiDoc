USE [PosDocumentation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tab_Modules](
    [fID]           [int] NOT NULL,
    [frApiId]       [int] NOT NULL,
    [fModuleName]   [nvarchar](50) NOT NULL,
    [fChangeDate]   [datetime] NOT NULL,
    [fAuthor]       [nvarchar](50) NOT NULL,
    [fDeleted]      [bit] NOT NULL,
)

CREATE INDEX [IN_tab_Apis_fChangeDate] ON [tab_Apis] (fChangeDate)
CREATE INDEX [IN_tab_Apis_fApiName] ON [tab_Apis] (fApiName)

ALTER TABLE [dbo].[tab_Modules]  WITH CHECK ADD CONSTRAINT [FK_tab_Modules_tab_Apis] FOREIGN KEY([frApiId])
REFERENCES [dbo].[tab_Apis] ([fID])
GO

ALTER TABLE [dbo].[tab_Modules] CHECK CONSTRAINT [FK_tab_Modules_tab_Apis]
GO


GO


