USE [MpmtDb]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 create   PROCEDURE [dbo].[usp_get_servicecharge_category_byid]
 (
	@Id int
 )
AS
BEGIN
	SELECT [Id]
		  ,CategoryName
		  ,CategoryCode
		  ,[Description]
		  ,[IsActive]
		  ,CreatedByName
		  ,UpdatedByName
	  FROM [dbo].tbl_service_charge_category with (Nolock) where IsDeleted=0 and Id=@Id
END
