USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 Create   PROCEDURE [dbo].[usp_get_prefundrequestapproved_byid]
 @Id as Int
AS
BEGIN
	SELECT [Id]
		,PartnerCode
		,(select UserName from tbl_remit_partners where PartnerCode=fr.PartnerCode) as Username 
		,(select FundType from tbl_fund_type where Id=FundTypeId) as FundType
		,SourceCurrency
		,DestinationCurrency
		,Amount
		,Remarks
		,[Sign]
		,TransactionId
		,(select StatusName from tbl_request_status where Id=fr.RequestStatusId) as RequestStatus
		,VoucherImgPath	
		,CreatedByName
		,CreatedDate
		,UpdatedDate
		,UpdatedByName
	  FROM [dbo].tbl_fund_request_approved fr with (Nolock) where Id=@Id 
END
