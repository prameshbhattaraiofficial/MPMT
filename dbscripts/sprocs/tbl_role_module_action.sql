
GO

/****** Object:  Table [dbo].[tbl_role_module_action]    Script Date: 9/4/2023 2:39:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_role_module_action](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Module] [varchar](250) NOT NULL,
	[ActionId] [int] NOT NULL,
	[Action] [varchar](250) NOT NULL,
	[CreatedBy] [varchar](100) NULL,
	[CreatedLocalDate] [datetime2](7) NULL,
	[CreatedUtcDate] [datetime2](7) NULL,
	[CreatedNepaliDate] [varchar](20) NULL,
	[UpdatedBy] [varchar](100) NULL,
	[UpdatedLocalDate] [datetime2](7) NULL,
	[UpdatedUtcDate] [datetime2](7) NULL,
	[UpdatedNepaliDate] [varchar](20) NULL,
 CONSTRAINT [PK_tbl_role_module_action] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


