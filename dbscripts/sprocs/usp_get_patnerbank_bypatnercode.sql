USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create PROCEDURE [dbo].[usp_get_patnerbank_bypatnercode]
 (
 @PatnerCode as varchar(100)
 )
AS
BEGIN
	SELECT [Id]
		  ,[PartnerCode]
		  ,[BankCode]
		  ,[AccountNumber]
		  ,[AccountName]
		  ,[CreatedByName]
		  ,[UpdatedByName]
	  FROM [dbo].tbl_partner_banks with (Nolock) where IsDeleted=0 and PartnerCode=@PatnerCode
END
