USE [MpmtDb]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_Currency]    Script Date: 8/15/2023 11:04:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create   PROCEDURE [dbo].[usp_get_Incomesource_ById]
 (
	@Id int
 )
AS
BEGIN
	SELECT [Id]
		  ,SourceName
		  ,[Description]
		  ,[IsActive]
		  ,CreatedByName
		  ,UpdatedByName
	  FROM [dbo].tbl_income_source with (Nolock) where IsDeleted=0 and Id=@Id
	 

END
