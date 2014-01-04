USE [ApiDoc]
GO

/****** Object:  Table [dbo].[tab_HttpVerbs]    Script Date: 19.12.2013 17:35:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tab_HttpVerbs](
	[fID] [int] NOT NULL,
	[fHttpVerb] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tab_HttpVerbs] PRIMARY KEY CLUSTERED 
(
	[fID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[tab_HttpVerbs] (fID, fHttpVerb)
VALUES (1, 'GET'), (2, 'POST'), (3,'DELETE')


