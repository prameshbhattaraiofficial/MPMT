CREATE OR ALTER PROCEDURE [dbo].[usp_get_WalletCurrencyBalance] --'REM2771010766'
(
 @PartnerCode nvarchar(max)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @StatusPending INT = (SELECT Id FROM tbl_request_status WHERE StatusCode = 'PENDING')
	
	SELECT 
		pw.Id,
		pw.PartnerCode,
		pw.SourceCurrency,
		pw.DestinationCurrency,
		pw.Balance,
		--fr.Amount,
		--CASE WHEN pw.SourceCurrency=fr.SourceCurrency then  SUM(ISNULL(fr.Amount,0)) 
		CASE WHEN pw.SourceCurrency=fr.SourceCurrency AND ISNULL(fr.RequestStatusId, -1) = (ISNULL(@StatusPending, 0)) then  SUM(ISNULL(fr.Amount,0)) 
		else 0 END as [Ledger],
		pw.NotificationBalanceLimit,
		pw.MarkupMinValue,
		pw.MarkupMaxValue,
		pw.TypeCode,
		pw.IsFavourite,
		pw.Remarks,
		pw.IsNPRSourceCurrency IsSourceCurrencyNPR,
		pw.IsActive
	FROM dbo.tbl_partner_wallets pw 
	left JOIN tbl_fund_request fr ON pw.PartnerCode = fr.PartnerCode and pw.SourceCurrency=fr.SourceCurrency
	WHERE pw.IsDeleted = 0 	
		--AND(( fr.RequestStatusId = (SELECT Id FROM tbl_request_status WHERE StatusCode = 'PENDING')) or (ISNULL(fr.RequestStatusId,0)=0) )
	    AND pw.PartnerCode = @PartnerCode
	GROUP BY 
		pw.Id,
		pw.PartnerCode,
		pw.SourceCurrency,
		pw.DestinationCurrency,
		pw.Balance,
		fr.SourceCurrency,
		fr.DestinationCurrency,
		fr.RequestStatusId,
		pw.NotificationBalanceLimit,
		pw.MarkupMinValue,
		pw.MarkupMaxValue,
		pw.TypeCode,
		pw.IsFavourite,
		pw.Remarks,
		pw.IsNPRSourceCurrency,
		pw.IsActive
	
	SET NOCOUNT OFF
END
GO