
-- =============================================
-- Author:		<SAROJ KUMAR CHAUDAHRY>
-- Create date: <2023-SEP-07>
-- Description:	<Get Parner Commission value in Source Currency Set by ADMIN>
-- Execution: select [dbo].[func_get_partner_commission]('REM3304621679','AUD','NPR',100.00,'BANK')
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[func_get_partner_commission] (@PartnerCode varchar(20),@SourceCurrency varchar(3), @DestinationCurrency VARCHAR(3),@SendingAmount decimal(18,2),@PaymentType varchar(50))
RETURNS DECIMAL(18,4)
AS
BEGIN
	
	DECLARE @Result DECIMAL(18,4);
	
	DECLARE @partnerCurrDateTime DATETIME, @partnerGMTTimeZone varchar(20), @PaymentTypeId INT;  
	DECLARE @unitValue INT, @conversionRate decimal(18,4)--, @serviceCharge decimal(18,4), @netSendingAmount decimal(18,4), @netReceivingAmountNPR decimal(18,4)  
	,@commission decimal(18,4), @partnerChargeCategoryId int, @partnerChargeCategory varchar(50);  
	declare @commissionPercent decimal(18,2), @commissionFixed decimal(18,4), @minCommission decimal(18,4),@maxCommission decimal(18,4);  

	select @partnerChargeCategoryId=ChargeCategoryId,@partnerChargeCategory=c.CategoryName, @partnerGMTTimeZone=GMTTimeZone from dbo.tbl_remit_partners p with(nolock) 
	inner join dbo.tbl_service_charge_category c with(nolock) on c.Id=p.ChargeCategoryId and c.IsActive=1 and isnull(c.IsDeleted,0)=0
	where PartnerCode=@PartnerCode and p.IsActive=1 and isnull(p.IsDeleted,0)=0; 
	
	SET @partnerCurrDateTime =  (SELECT [dbo].[func_convert_gmt_to_local_date](GETUTCDATE(),@partnerGMTTimeZone))
	SET @PaymentTypeId = (SELECT Id from dbo.tbl_payment_type with(nolock) where PaymentTypeCode=@PaymentType);

	
	-------------PARTNER COMMISSION CALCULATION----------------------
	IF(@partnerChargeCategory LIKE '%provision%')	----IF CATEGORY IS PROVISIONAL
	BEGIN
		select  @commissionPercent=CommissionPercent, @commissionFixed=CommissionFixed,@mincommission=MinCommission,@maxcommission=MaxCommission  
		from dbo.tbl_service_charge_settings with(nolock) where  SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency   
			AND ChargeCategoryId=@partnerChargeCategoryId AND PaymentTypeId=@PaymentTypeId AND @SendingAmount BETWEEN MinAmountSlab AND MaxAmountSlab 
			AND CAST(@partnerCurrDateTime AS DATE) BETWEEN FromDate AND ToDate; 
	END
	ELSE	----IF CATEGORY IS OTHER THAN PROVISIONAL
	BEGIN
	select  @commissionPercent=CommissionPercent, @commissionFixed=CommissionFixed,@mincommission=MinCommission,@maxcommission=MaxCommission  
	from dbo.tbl_service_charge_settings with(nolock) where  SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency   
		AND ChargeCategoryId=@partnerChargeCategoryId AND PaymentTypeId=@PaymentTypeId AND @SendingAmount BETWEEN MinAmountSlab AND MaxAmountSlab;
	END

	SET @commission = @commissionFixed + ((@SendingAmount * @commissionPercent)/100);  
  
  
	IF(@commission < @minCommission)  
	BEGIN  
	 SET @commission = @minCommission;  
	END
	IF(@commission > @maxCommission)  
	BEGIN  
	 SET @commission = @maxCommission;  
	END  

	IF(@commission < 0)
	SET @commission = 0;

	SET @Result = ISNULL(@commission,0);
	
	RETURN @Result;
	
END;
GO


