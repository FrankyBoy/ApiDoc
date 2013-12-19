USE [PosDocumentation]
GO

/****** Object:  Table [dbo].[tab_Modules]    Script Date: 19.12.2013 17:39:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tab_Modules](
	[fID] [int] NOT NULL IDENTITY(1,1),
	[frApiId] [int] NOT NULL,
	[fServiceName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tab_Services] PRIMARY KEY CLUSTERED 
(
	[fID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tab_Modules]  WITH CHECK ADD  CONSTRAINT [FK_tab_Services_tab_Apis] FOREIGN KEY([frApiId])
REFERENCES [dbo].[tab_APIs] ([fID])
GO

ALTER TABLE [dbo].[tab_Modules] CHECK CONSTRAINT [FK_tab_Services_tab_Apis]
GO


