USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create   PROCEDURE [dbo].[usp_get_WalletCurrency]
AS
BEGIN
	select Id,PartnerCode,SourceCurrency,DestinationCurrency,Balance,NotificationBalanceLimit,MarkupMinValue,MarkupMaxValue,TypeCode,Remarks,IsActive from  dbo.tbl_partner_wallets
	where IsDeleted=0
END
