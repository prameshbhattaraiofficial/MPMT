Alter Procedure [dbo].[usp_get_remitpartner_register]--'Bibek@gmail.com'
(
@Email nvarchar(100)
)
As 
Begin
Declare @partnerCode nvarchar(100)=(select PartnerCode from tbl_remit_partners_register where Email=@Email)


SELECT  [Id]
      ,[PartnerCode]
      ,[FirstName]
      ,[SurName]
      ,[Withoutfirstname]
      ,[MobileNumber]
      ,[MobileConfirmed]
      ,[Email]
      ,[EmailConfirmed]
      ,[Post]  
      ,[Address]
      ,[OrganizationName]
      ,[OrgEmail]
      ,[OrgEmailConfirmed]
      ,[CountryCode]
      ,[City]
      ,[FullAddress]
      ,[GMTTimeZone]
      ,[RegistrationNumber]
      ,[SourceCurrency]     
      ,[IpAddress]
      ,[CompanyLogoImgPath]
      ,[DocumentTypeId]
      ,[DocumentNumber]
      ,[IdFrontImgPath]
      ,[IdBackImgPath]
      ,[ExpiryDate]   
      ,[AddressProofTypeId]
      ,[AddressProofImgPath]   
      ,[IsActive]     
      ,[Is2FAAuthenticated]  
      ,[LicensedocImgPath]
      ,[Maker]
      ,[Checker]
      ,[CreatedById]
      ,[CreatedByName]
      ,[CreatedDate]
      ,[UpdatedById]
      ,[UpdatedByName]
      ,[UpdatedDate]
      ,[BusinessNumber]
      ,[FinancialTransactionRegNo]
      ,[RemittancRegNumber]
      ,[LicenseNumber]
      ,[ZipCode]
      ,[OrgState]
      ,[Callingcode]
  FROM [MpmtDb].[dbo].[tbl_remit_partners_register] where Email=@Email

  ----partner Director--
    select DirectorId,PartnerCode,FirstName,SurName,IsSurNamepresent,ContactNumber,Email
  from tbl_partner_directors where PartnerCode=@partnerCode

  END

