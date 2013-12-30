USE [PosDocumentation]
GO

/****** Object:  Table [dbo].[tab_Methods]    Script Date: 19.12.2013 17:38:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tab_Methods](
	[fID] [int] NOT NULL,
	[frServiceId] [int] NOT NULL,
	[fMethodName] [nvarchar](100) NOT NULL,
	[frHttpVerb] [int] NOT NULL,
	[fRequiresAuthentication] [bit] NOT NULL,
	[fRequiresAuthorization] [bit] NOT NULL,
	[fRequestSample] [varchar](max) NULL,
	[fResponseSample] [varchar](max) NULL,
	[fDescription] [varchar](max) NOT NULL,
	[fChangeDate] [datetime] NOT NULL,
	[fAuthor] [nvarchar](50) NOT NULL,
	[fDeleted] [bit] NOT NULL
)

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tab_Methods]  WITH CHECK ADD CONSTRAINT [FK_tab_Methods_tab_HttpVerbs] FOREIGN KEY([frHttpVerb])
REFERENCES [dbo].[tab_HttpVerbs] ([fID])
GO

ALTER TABLE [dbo].[tab_Methods] CHECK CONSTRAINT [FK_tab_Methods_tab_HttpVerbs]
GO

ALTER TABLE [dbo].[tab_Methods]  WITH CHECK ADD  CONSTRAINT [FK_tab_Methods_tab_Services] FOREIGN KEY([frServiceId])
REFERENCES [dbo].[tab_Modules] ([fID])
GO

ALTER TABLE [dbo].[tab_Methods] CHECK CONSTRAINT [FK_tab_Methods_tab_Services]
GO

