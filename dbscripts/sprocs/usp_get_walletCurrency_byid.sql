USE [MpmtDb]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_WalletCurrency]    Script Date: 8/20/2023 5:18:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create   PROCEDURE [dbo].[usp_get_walletCurrency_byid]
 (
 @Id int
 )
AS
BEGIN
	select Id,PartnerCode,SourceCurrency,DestinationCurrency,Balance,NotificationBalanceLimit
	,MarkupMinValue,MarkupMaxValue,TypeCode,Remarks,IsActive
	from  dbo.tbl_partner_wallets
	where IsDeleted=0 and Id=@Id
END
