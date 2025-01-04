USE [MpmtDb]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_patnerbank_bypatnercode]    Script Date: 8/17/2023 11:43:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create PROCEDURE [dbo].[usp_get_patnerbank_byId] 
 (
 @Id as varchar(100)
 )
AS
BEGIN
	SELECT [Id]
		  ,[PartnerCode]
		  ,[BankCode]
		  ,[AccountNumber]
		  ,[AccountName],
		  [IsActive]
		  ,[CreatedByName]
		  ,[UpdatedByName]
	  FROM [dbo].tbl_partner_banks with (Nolock) where IsDeleted=0 and Id=@Id
END
