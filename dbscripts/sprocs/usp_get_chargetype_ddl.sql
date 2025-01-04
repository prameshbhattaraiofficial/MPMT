USE [MpmtDb]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_Country_ddl]    Script Date: 8/10/2023 2:31:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create  PROCEDURE [dbo].[usp_get_chargetype_ddl]
AS
BEGIN
	select 
	ChargeType as [Text],
	TypeCode as [Value]
	from tbl_charge_type with (Nolock) where IsActive=1
END
