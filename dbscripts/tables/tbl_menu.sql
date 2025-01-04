USE [MpmtDb]
GO

/****** Object:  Table [dbo].[tbl_menu]    Script Date: 8/28/2023 11:59:58 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_menu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](450) NOT NULL,
	[ParentId] [int] NULL,
	[MenuUrl] [nvarchar](2083) NOT NULL,
	[IsActive] [bit] NULL,
	[DisplayOrder] [int] NULL,
	[ImagePath] [nvarchar](max) NULL,
	[CreatedBy] [varchar](100) NULL,
	[CreatedLocalDate] [datetime2](7) NULL,
	[CreatedUtcDate] [datetime2](7) NULL,
	[CreatedNepaliDate] [varchar](20) NULL,
	[UpdatedBy] [varchar](100) NULL,
	[UpdatedLocalDate] [datetime2](7) NULL,
	[UpdatedUtcDate] [datetime2](7) NULL,
	[UpdatedNepaliDate] [varchar](20) NULL,
 CONSTRAINT [PK_tbl_menu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


