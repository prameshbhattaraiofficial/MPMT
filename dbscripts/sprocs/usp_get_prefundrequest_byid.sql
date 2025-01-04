
CREATE OR ALTER PROCEDURE [dbo].[usp_get_prefundrequest_byid]
 @Id as Int
AS
BEGIN
	SET NOCOUNT ON
	SELECT fr.[Id]
		,fr.PartnerCode
		,rp.UserName AS Username
		,rp.Email AS Email
		,(SELECT FundType FROM tbl_fund_type WHERE Id=fr.FundTypeId) AS FundType
		,fr.SourceCurrency
		,fr.DestinationCurrency
		,fr.Amount
		,fr.Remarks
		,fr.[Sign]
		,fr.TransactionId
		,(SELECT StatusName FROM tbl_request_status WHERE Id=fr.RequestStatusId) AS RequestStatus
		,(SELECT StatusCode FROM tbl_request_status WHERE Id=fr.RequestStatusId) AS RequestStatusCode
		,fr.VoucherImgPath	
		,fr.CreatedByName
		,fr.CreatedDate
		,fr.UpdatedDate
		,fr.UpdatedByName
	  FROM [dbo].tbl_fund_request fr WITH (NOLOCK)
		INNER JOIN [dbo].[tbl_remit_partners] rp WITH (NOLOCK) ON fr.Id = @Id AND fr.PartnerCode = rp.PartnerCode
	SET NOCOUNT OFF
END
GO