USE [MpmtDb]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_walletCurrency_byid]    Script Date: 9/12/2023 10:36:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create   PROCEDURE [dbo].[usp_get_feewalletCurrency_byid]
 (
 @Id int
 )
AS
BEGIN
	select Id,PartnerCode,SourceCurrency,Balance,NotificationBalanceLimit
	,MarkupMinValue,MarkupMaxValue,TypeCode,Remarks,IsActive
	from  dbo.tbl_partner_fee_account
	where IsDeleted=0 and Id=@Id
END
