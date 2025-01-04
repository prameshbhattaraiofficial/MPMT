

CREATE TABLE [dbo].[tbl_user_activity_logs]
(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](200) NULL,
	[Email] [nvarchar](200) NULL,
	[IsCustomer] [bit] NULL,
	[UserAgent] [nvarchar](500) NULL,
	[RemoteIpAddress] [nvarchar](100) NULL,
	[HttpMethod] [nvarchar](10) NULL,
	[ControllerName] [nvarchar](100) NULL,
	[ActionName] [nvarchar](100) NULL,
	[QueryString] [nvarchar](2000) NULL,
	[IsFormData] [bit] NULL,
	[RequestBody] [nvarchar](max) NULL,
	[Headers] [nvarchar](max) NULL,
	[RequestUrl] [nvarchar](500) NULL,
	[MachineName] [nvarchar](200) NULL,
	[Environment] [nvarchar](200) NULL,
	[UserAction] [nvarchar](500) NULL,
	[LogLocalDate] [datetime2](7) NULL,
	[LogUtcDate] [datetime2](7) NULL,
)


