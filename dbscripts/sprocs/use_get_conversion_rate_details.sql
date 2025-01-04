CREATE OR ALTER    PROCEDURE [dbo].[use_get_conversion_rate_details]
(
@PartnerCode VARCHAR(20) = NULL
,@SourceCurrency VARCHAR(3) = NULL
,@DestinationCurrency VARCHAR(3) = NULL
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
WHERE w.PartnerCode=@PartnerCode AND w.SourceCurrency=@SourceCurrency AND w.DestinationCurrency=@DestinationCurrency;


SELECT
MinAmountSlab	
,MaxAmountSlab	
,r.CurrentRate AS ConversionRate	
,ServiceChargePercent	
,ServiceChargeFixed	
,MinServiceCharge	
,MaxServiceCharge	
,CommissionPercent	
,CommissionFixed	
,MinCommission	
,MaxCommission	
FROM dbo.tbl_service_charge_settings s WITH(NOLOCK) 
INNER JOIN dbo.tbl_partner_wallets w WITH(NOLOCK) ON w.SourceCurrency=s.SourceCurrency AND w.DestinationCurrency=s.DestinationCurrency 
INNER JOIN dbo.tbl_exchange_rate r WITH(NOLOCK) ON r.SourceCurrency=w.SourceCurrency AND r.DestinationCurrency=w.DestinationCurrency
WHERE s.SourceCurrency=@SourceCurrency AND s.DestinationCurrency=@DestinationCurrency AND w.PartnerCode=@PartnerCode
	AND s.IsActive=1 AND s.IsDeleted=0 AND w.IsActive=1 AND w.IsDeleted=0 AND r.IsActive=1 AND r.IsDeleted=0
GROUP BY MinAmountSlab,MaxAmountSlab,r.CurrentRate,ServiceChargePercent,ServiceChargeFixed,MinServiceCharge,MaxServiceCharge,CommissionPercent	
	,CommissionFixed,MinCommission,MaxCommission	
ORDER BY MinAmountSlab, MaxAmountSlab;

END