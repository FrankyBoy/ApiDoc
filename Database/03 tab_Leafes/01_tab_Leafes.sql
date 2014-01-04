USE [PosDocumentation]
GO

/****** Object:  Table [dbo].[tab_Methods]    Script Date: 19.12.2013 17:38:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tab_Leafes](
	[fID]			[int] NOT NULL,
	[frParentId]	[int] NOT NULL,
	[fName]			[nvarchar](100) NOT NULL,
	[frHttpVerb]	[int] NOT NULL,
	[fDescription]	[varchar](max) NOT NULL,
	
	[fRequiresAuthentication]	[bit] NOT NULL,
	[fRequiresAuthorization]	[bit] NOT NULL,
	[fRequestSample]			[varchar](max) NULL,
	[fResponseSample]			[varchar](max) NULL,
	
	[fChangeDate]	[datetime] NOT NULL,
	[fAuthor]		[nvarchar](50) NOT NULL,
	[fChangeNote]	[nvarchar](MAX) NOT NULL,
	[fDeleted]		[bit] NOT NULL,
)

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tab_Leafes]  WITH CHECK ADD CONSTRAINT [FK_tab_Leafes_tab_HttpVerbs] FOREIGN KEY([frHttpVerb])
REFERENCES [dbo].[tab_HttpVerbs] ([fID])
GO

ALTER TABLE [dbo].[tab_Leafes] CHECK CONSTRAINT [FK_tab_Leafes_tab_HttpVerbs]
GO

CREATE INDEX [IX_tab_Leafes_fChangeDate] ON [tab_Leafes] (fChangeDate)
CREATE INDEX [IX_tab_Leafes_fName] ON [tab_Leafes] (fName)
CREATE INDEX [IX_tab_Leafes_fID] ON [tab_Leafes] (fID)



