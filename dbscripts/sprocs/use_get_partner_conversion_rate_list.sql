CREATE OR ALTER PROCEDURE [dbo].[use_get_partner_conversion_rate_list] --'REM3304621679'
(
@PartnerCode VARCHAR(20) = NULL
)
AS
BEGIN

	SELECT w.SourceCurrency, w.DestinationCurrency, r.UnitValue
	,CASE WHEN w.TypeCode='FLAT' THEN r.CurrentRate + w.MarkupMinValue ELSE (r.CurrentRate + (r.CurrentRate * w.MarkupMinValue)/100) END AS MinRate
	,CASE WHEN w.TypeCode='FLAT' THEN r.CurrentRate + w.MarkupMaxValue ELSE (r.CurrentRate + (r.CurrentRate * w.MarkupMaxValue)/100) END AS MaxRate
	,r.CurrentRate 
	FROM dbo.tbl_partner_wallets w WITH(NOLOCK)
	INNER JOIN dbo.tbl_exchange_rate r WITH(NOLOCK) ON r.SourceCurrency=w.SourceCurrency AND r.DestinationCurrency=w.DestinationCurrency 
			AND w.IsActive=1 AND ISNULL(w.IsDeleted,0)=0 AND r.IsActive=1 AND ISNULL(r.IsDeleted,0)=0
	WHERE w.PartnerCode=@PartnerCode;

END