USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create   PROCEDURE [dbo].[usp_get_DocumentType]
 
AS
BEGIN
	SELECT [Id]
		 ,[DocumentType]
		 ,[DocumentTypeCode]
		 ,[IsExpirable]
		 ,[Remarks]
		 ,[CreatedBy]
		 ,UpdatedBy
	  FROM [dbo].tbl_document_type with (Nolock) 
END
