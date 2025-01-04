USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create   PROCEDURE [dbo].[usp_get_PaymentType_byid]
 (
	@Id int
 )
AS
BEGIN
	SELECT [Id]
		  ,PaymentTypeName
		  ,PaymentTypeCode
		  ,[Description]
		  ,[IsActive]
		  ,CreatedByName
		  ,UpdatedByName
	  FROM [dbo].tbl_payment_type with (Nolock) where IsDeleted=0 and Id=@Id
	   
END
