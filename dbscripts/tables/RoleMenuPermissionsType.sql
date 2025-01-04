USE [MpmtDb]
GO

/****** Object:  UserDefinedTableType [dbo].[RoleMenuPermissionsType]    Script Date: 8/29/2023 3:50:04 PM ******/
CREATE TYPE [dbo].[RoleMenuPermissionsType] AS TABLE(
	[MenuId] [int] NOT NULL,
	[ViewPer] [bit] NOT NULL,
	[CreatePer] [bit] NOT NULL,
	[UpdatePer] [bit] NOT NULL,
	[DeletePer] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[IsActive] [bit] NULL
)
GO


