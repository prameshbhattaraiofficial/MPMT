USE [MpmtDb]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_banks]    Script Date: 8/14/2023 2:23:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create   PROCEDURE [dbo].[usp_get_ChargeType_ById]
 (
 @Id int
 )
AS
BEGIN
	SELECT [Id]
		  ,TypeCode
		  ,ChargeType
		  ,[IsActive]
		  ,CreatedByName
		  ,UpdatedByName
	  FROM [dbo].tbl_charge_type with (Nolock) where IsDeleted=0 and Id=@Id
END
