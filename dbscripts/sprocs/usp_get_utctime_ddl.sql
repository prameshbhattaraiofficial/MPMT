USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create  PROCEDURE [dbo].[usp_get_utctime_ddl]
AS
BEGIN
	select 
	[Value] as [Text],
	Code as [Value]
	from tbl_utcdata with (Nolock) where IsActive=1
END
