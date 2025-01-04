USE [MpmtDb]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 create   PROCEDURE [dbo].[usp_get_service_charge_category]
 (
	 @CategoryName nvarchar(20) = ''  
	,@CategoryCode nvarchar(20)=''
	,@Status int=0
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
	  FROM [dbo].tbl_service_charge_category with (Nolock) where IsDeleted=0 and

	  ISNULL(CategoryName,'') LIKE ISNULL(REPLACE(@CategoryName,' ',''),'') + '%' 
		AND ISNULL(CategoryCode,'') LIKE ISNULL(@CategoryCode,'') + '%'  
		AND (
		CASE 
		WHEN @Status = 0 AND IsActive IN (0, 1) THEN 1
		WHEN @Status = 1 AND IsActive =  1 THEN 1
		WHEN @Status = 2 AND IsActive = 0 THEN 1
		ELSE 0
		END
		) = 1
END
