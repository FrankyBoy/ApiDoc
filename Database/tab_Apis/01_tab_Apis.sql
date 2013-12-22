USE [PosDocumentation]
GO

/****** Object:  Table [dbo].[tab_APIs]    Script Date: 19.12.2013 17:35:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tab_Apis](
	[fID] [int] NOT NULL,
	[fApiName] [nvarchar](50) NOT NULL,
	[fDescription] [nvarchar](max),
	[fChangeDate] datetime NOT NULL,
	[fAuthor] [nvarchar](50) NOT NULL,
	[fDeleted] [bit] NOT NULL,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE INDEX [IN_tab_Apis_fChangeDate] ON [tab_Apis] (fChangeDate)
CREATE INDEX [IN_tab_Apis_fApiName] ON [tab_Apis] (fApiName)
 
GO


