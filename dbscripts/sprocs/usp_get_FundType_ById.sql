USE [MpmtDb]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_ChargeType]    Script Date: 8/14/2023 2:26:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create   PROCEDURE [dbo].[usp_get_FundType_ById]
 (
 @Id int
 )
AS
BEGIN
	SELECT [Id]
		  ,[FundType]
		  ,[FundTypeCode]
		  ,[Remarks]
		  ,[IsActive]
		  ,CreatedByName
		  ,UpdatedByName
	  FROM [dbo].tbl_fund_type with (Nolock) where IsDeleted=0 and Id=@Id
END
