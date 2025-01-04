USE [MpmtDb]
GO

/****** Object:  Table [dbo].[tbl_action]    Script Date: 8/29/2023 5:23:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_action](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Action] [varchar](250) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[CreatedBy] [varchar](20) NULL,
	[CreatedLocalDate] [datetime2](7) NULL,
	[CreatedUtcDate] [datetime2](7) NULL,
	[CreatedNepaliDate] [varchar](20) NULL,
	[UpdatedBy] [varchar](20) NULL,
	[UpdatedLocalDate] [datetime2](7) NULL,
	[UpdatedUtcDate] [datetime2](7) NULL,
	[UpdatedNepaliDate] [varchar](7) NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


