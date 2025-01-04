  
CREATE OR ALTER   procedure [dbo].[usp_get_existing_senders_by_partnercode] --'REM9328276416','1DFRH87784382438',''  
(  
@PartnerCode varchar(20) = null  
,@MemberId varchar(20) = null  
,@FullName varchar(200) = null  
)  
AS  
BEGIN  
 SELECT    
 TRIM(REPLACE(FirstName,'-','') + ' ' + SurName) AS FullName  
 ,MemberId  
 ,AccountNumber  
 ,BankName  
 ,MobileNumber AS ContactNumber  
 ,Email  
 FROM dbo.tbl_senders WITH(NOLOCK)   
 WHERE PartnerCode = @PartnerCode AND (MemberId=@MemberId OR ISNULL(@MemberId,'')='')  
 AND REPLACE(REPLACE((FirstName+SurName),' ',''),'-','') LIKE '%' + ISNULL(@FullName,'') + '%'  
END  
  