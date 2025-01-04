CREATE OR ALTER   PROCEDURE [dbo].[usp_get_province_ddl]  
@CountryCode nvarchar(3) = NULL  
AS    
BEGIN    
 SELECT  
      [Province] AS [Text]  
   ,ProvinceCode AS [Value]  
   ,c.CountryCode AS [CountryCode]  
  FROM [dbo].[tbl_province] p WITH(NOLOCK)  
  INNER JOIN [dbo].[tbl_country] c WITH(NOLOCK) ON c.CountryCode = p.CountryCode  
  WHERE ISNULL(@CountryCode,'') = '' OR c.CountryCode = @CountryCode  
  ORDER By [Text]  
END    



