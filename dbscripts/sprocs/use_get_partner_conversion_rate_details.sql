CREATE OR ALTER PROCEDURE [dbo].[use_get_partner_conversion_rate_details]
(
@PartnerCode VARCHAR(20) = NULL
,@SourceCurrency VARCHAR(3) = NULL
,@DestinationCurrency VARCHAR(3) = NULL
)
AS
BEGIN

--DECLARE @tmpWallet AS TABLE(SourceCurrency varchar(3),DestinationCurrency varchar(3), UnitValue int, MinRate decimal(18,4), MaxRate decimal(18,4),CurrentRate decimal(18,4));
--INSERT INTO @tmpWallet
	SELECT w.SourceCurrency, w.DestinationCurrency, r.UnitValue
	,CASE WHEN w.TypeCode='FLAT' THEN r.CurrentRate + w.MarkupMinValue ELSE (r.CurrentRate + (r.CurrentRate * w.MarkupMinValue)/100) END AS MinRate
	,CASE WHEN w.TypeCode='FLAT' THEN r.CurrentRate + w.MarkupMaxValue ELSE (r.CurrentRate + (r.CurrentRate * w.MarkupMaxValue)/100) END AS MaxRate
	,r.CurrentRate 
	FROM dbo.tbl_partner_wallets w WITH(NOLOCK)
	INNER JOIN dbo.tbl_exchange_rate r WITH(NOLOCK) ON r.SourceCurrency=w.SourceCurrency AND r.DestinationCurrency=w.DestinationCurrency 
			AND w.IsActive=1 AND ISNULL(w.IsDeleted,0)=0 AND r.IsActive=1 AND ISNULL(r.IsDeleted,0)=0
	WHERE w.PartnerCode=@PartnerCode AND w.SourceCurrency=@SourceCurrency AND w.DestinationCurrency=@DestinationCurrency;

--SELECT  
--SourceCurrency
--,DestinationCurrency
--,UnitValue
--,MinRate
--,MaxRate
--,CurrentRate
--FROM @tmpWallet;

SELECT
MinAmountSlab	
,MaxAmountSlab	
,ConversionRate	
,ServiceChargePercent	
,ServiceChargeFixed	
,MinServiceCharge	
,MaxServiceCharge	
,CommissionPercent	
,CommissionFixed	
,MinCommission	
,MaxCommission	
FROM dbo.tbl_partner_conversion_rate_setting WITH(NOLOCK) 
WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency
ORDER BY MinAmountSlab, MaxAmountSlab;

END