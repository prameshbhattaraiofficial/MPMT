CREATE OR ALTER  PROCEDURE [dbo].[usp_get_fundtype_ddl]    
AS    
BEGIN    
 select     
 FundType as [Text],    
 Id as [Value],    
 FundTypeCode as [lookup]    
 from tbl_fund_type with (Nolock) where IsActive=1 and IsDeleted=0    
END 