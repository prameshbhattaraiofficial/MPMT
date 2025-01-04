USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[usp_get_recipient_type_ddl]
AS  
BEGIN  
 SELECT 
      RecipientType AS [text]
	  ,Id AS [value]
	  ,LookupName As [lookup]
  FROM [dbo].tbl_recipient_type WITH(NOLOCK) where IsActive=1 and IsDeleted=0
END  

