USE [MpmtDb]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_patnerbank]    Script Date: 8/17/2023 11:56:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create PROCEDURE [dbo].[usp_get_exchangerate]
 (
	 @SourceCurrency nvarchar(20) = ''  	
	,@DestinationCurrency nvarchar(20) = '' 
	,@Status int=0
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
	  FROM [dbo].tbl_exchange_rate with (Nolock) where IsDeleted=0 and
	   ISNULL(SourceCurrency,'') LIKE ISNULL(REPLACE(@SourceCurrency,' ',''),'') + '%'  
	   AND ISNULL(DestinationCurrency,'') LIKE ISNULL(REPLACE(@DestinationCurrency,' ',''),'') + '%' 	  
		AND (
		CASE 
		WHEN @Status = 0 AND IsActive IN (0, 1) THEN 1
		WHEN @Status = 1 AND IsActive =  1 THEN 1
		WHEN @Status = 2 AND IsActive = 0 THEN 1
		ELSE 0
		END
		) = 1
END
