USE [MpmtDb]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_patnerbank_byId]    Script Date: 8/24/2023 10:07:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create PROCEDURE [dbo].[usp_get_recipients_byId] 
 (
 @Id as varchar(100)
 )
AS
BEGIN
	SELECT [Id]			 
			  ,[SenderId]
			  ,[FirstName]
			  ,[SurName]
			  ,[IsSurNamePresent]
			  ,[MobileNumber]
			  ,[Email]
			  ,[GenderId]
			  ,[DateOfBirth]
			  ,[CountryCode]
			  ,[ProvinceCode]
			  ,[DistrictCode]
			  ,[LocalBodyCode]
			  ,[City]
			  ,[Zipcode]
			  ,[Address]
			  ,[SourceCurrency]
			  ,[DestinationCurrency]
			  ,[RelationshipId]
			  ,[PayoutTypeId]
			  ,[BankName]
			  ,[BankCode]
			  ,[Branch]
			  ,[AccountHolderName]
			  ,[AccountNumber]
			  ,[WalletName]
			  ,[WalletId]
			  ,[WalletRegisteredName]
			  ,[GMTTimeZone]
			  ,[IsActive]
			  ,[IsDeleted]
			  ,[IsBlocked]
			  ,[KycStatusCode]
			  ,[CreatedUserType]
			  ,[CreatedById]
			  ,[CreatedByName]
			  ,[CreatedDate]
			  ,[UpdatedUserType]
			  ,[UpdatedById]
			  ,[UpdatedByName]
			  ,[UpdatedDate]
	  FROM [dbo].tbl_recipients with (Nolock) where IsDeleted=0 and Id=@Id
END
