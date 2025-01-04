USE [MpmtDb]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_patnerbank]    Script Date: 8/17/2023 11:56:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create PROCEDURE [dbo].[usp_get_exchangerate_byid]
 (

	@Id int
 )
AS
BEGIN
	SELECT [Id]
		  ,SourceCurrency
		  ,DestinationCurrency
		  ,UnitValue
		  ,BuyingRate
		  ,SellingRate
		  ,CurrentRate
		  ,[IsActive]
		  ,[CreatedByName]
		  ,[UpdatedByName]
	  FROM [dbo].tbl_exchange_rate with (Nolock) where IsDeleted=0 and Id=@Id
	 
END
