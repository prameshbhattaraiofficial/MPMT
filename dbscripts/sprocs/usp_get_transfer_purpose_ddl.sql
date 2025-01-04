
USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[usp_get_transfer_purpose_ddl]
AS  
BEGIN  
 SELECT 
      PurposeName AS [Text]
	  ,Id AS [value]
  FROM [dbo].tbl_transfer_purpose WITH(NOLOCK) where IsActive=1 and IsDeleted=0
END  
