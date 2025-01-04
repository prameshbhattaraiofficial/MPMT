USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create PROCEDURE [dbo].[usp_get_relation_byid]
 (
 @Id as int
 )
AS
BEGIN
	SELECT [Id]
		  ,RelationName
		  ,[Description]
		  ,[IsActive]
		  ,CreatedByName
		  ,UpdatedByName
	  FROM [dbo].tbl_relation with (Nolock) where IsDeleted=0 and Id=@Id
END
