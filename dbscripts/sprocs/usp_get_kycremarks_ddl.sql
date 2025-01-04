USE [MpmtDb]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_Country_ddl]    Script Date: 8/10/2023 11:26:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create  PROCEDURE [dbo].[usp_get_kycremarks_ddl]
AS
BEGIN
	select 
	RemarksName as [Text],
	Id as [Value]
	from tbl_kyc_remarks with (Nolock) where IsActive=1
END
