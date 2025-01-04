USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create   PROCEDURE [dbo].[usp_get_transferpurpose_ById]
 @Id as int
AS
BEGIN
	SELECT [Id]
		  ,PurposeName
		  ,[Description]		
		  ,[IsActive]
		  ,CreatedByName
		  ,UpdatedByName
	  FROM [dbo].tbl_transfer_purpose with (Nolock) where IsDeleted=0 and Id=@Id
END
