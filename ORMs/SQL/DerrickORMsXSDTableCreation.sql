USE [ZeroCool]
GO

/****** Object:  Table [dbo].[XSDTable]    Script Date: 1/29/2017 1:56:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[XSDTable](
	[Prim] [bigint] IDENTITY(1,1) NOT NULL,
	[Firstname] [varchar](100) NULL,
	[Lastname] [varchar](100) NULL,
	[Age] [bigint] NULL,
 CONSTRAINT [PK_XSDTable] PRIMARY KEY CLUSTERED 
(
	[Prim] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


