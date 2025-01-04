
-- =============================================
-- Author:		<SAROJ KUMAR CHAUDAHRY>
-- Create date: <2023-SEP-07>
-- Description:	<Get Parner Service charge and amount details of remit Transaction>
-- Execution: select * from [dbo].[func_get_partner_service_charge]('REM3304621679','AUD','NPR',100.00,'BANK')
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[func_get_partner_service_charge] (@PartnerCode varchar(20),@SourceCurrency varchar(3), @DestinationCurrency VARCHAR(3),@SendingAmount decimal(18,2),@PaymentType varchar(50))
RETURNS @tblServiceCharge TABLE (SourceCurrency varchar(3),DestinationCurrency varchar(3),SendingAmount decimal(18,4),ConversionRate decimal(18,4)
	,NetSendingAmount decimal(18,4),ReceivingAmountNPR decimal(18,4),ServiceCharge decimal(18,4),Commission decimal(18,4),PartnerServiceCharge decimal(18,4))
AS
BEGIN
	
	DECLARE @partnerCurrDateTime DATETIME, @partnerGMTTimeZone varchar(20), @PaymentTypeId INT;  
	DECLARE @unitValue INT, @conversionRate decimal(18,4), @serviceCharge decimal(18,4), @netSendingAmount decimal(18,4), @netReceivingAmountNPR decimal(18,4)  
	  ,@commission decimal(18,4), @partnerChargeCategoryId int, @partnerChargeCategory varchar(50);  

	DECLARE @serviceChargePercent decimal(18,2), @serviceChargeFixed decimal(18,4), @minServiceCharge decimal(18,4),@maxServiceCharge decimal(18,4);  
	DECLARE @adminServiceChargePercent decimal(18,4), @adminServiceChargeFixed decimal(18,4),@adminMinServiceCharge decimal(18,4)
			,@adminMaxServiceCharge decimal(18,4),@partnerServiceCharge decimal(18,4);
	declare @commissionPercent decimal(18,2), @commissionFixed decimal(18,4), @minCommission decimal(18,4),@maxCommission decimal(18,4); 

	select @unitValue = UnitValue, @conversionRate=CurrentRate from dbo.tbl_exchange_rate with(nolock) where SourceCurrency=@SourceCurrency and DestinationCurrency=@DestinationCurrency 
	and IsActive=1 and ISNULL(IsDeleted,0)=0;

	select @partnerChargeCategoryId=ChargeCategoryId,@partnerChargeCategory=c.CategoryName, @partnerGMTTimeZone=GMTTimeZone from dbo.tbl_remit_partners p with(nolock) 
		inner join dbo.tbl_service_charge_category c with(nolock) on c.Id=p.ChargeCategoryId and c.IsActive=1 and isnull(c.IsDeleted,0)=0
		where PartnerCode=@PartnerCode and p.IsActive=1 and isnull(p.IsDeleted,0)=0; 
	
	SET @partnerCurrDateTime =  (SELECT [dbo].[func_convert_gmt_to_local_date](GETUTCDATE(),@partnerGMTTimeZone))
	SET @PaymentTypeId = (SELECT Id from dbo.tbl_payment_type with(nolock) where PaymentTypeCode=@PaymentType AND IsActive=1 AND IsDeleted=0);

	
	--------------------SERVICE CHARGE CALCULATION--------------------------------------  
IF EXISTS(select 1 from dbo.tbl_partner_conversion_rate_setting with(nolock) where PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency   
	AND DestinationCurrency=@DestinationCurrency AND @SendingAmount BETWEEN MinAmountSlab AND MaxAmountSlab)  
BEGIN  
	select @conversionRate=ConversionRate,@serviceChargePercent=ServiceChargePercent, @serviceChargeFixed=ServiceChargeFixed,@minServiceCharge=MinServiceCharge,@maxServiceCharge=MaxServiceCharge  
	from dbo.tbl_partner_conversion_rate_setting with(nolock) where PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency   
	AND DestinationCurrency=@DestinationCurrency AND @SendingAmount BETWEEN MinAmountSlab AND MaxAmountSlab;  
END  
ELSE  
BEGIN 
	IF(@partnerChargeCategory LIKE '%provision%')	----IF CATEGORY IS PROVISIONAL
	BEGIN
		select @serviceChargePercent=ServiceChargePercent, @serviceChargeFixed=ServiceChargeFixed,@minServiceCharge=MinServiceCharge,@maxServiceCharge=MaxServiceCharge  
		from dbo.tbl_service_charge_settings with(nolock) where  SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency 
		AND ChargeCategoryId=@partnerChargeCategoryId AND PaymentTypeId=@PaymentTypeId AND @SendingAmount BETWEEN MinAmountSlab AND MaxAmountSlab 
		AND CAST(@partnerCurrDateTime AS DATE) BETWEEN FromDate AND ToDate;  
	END
	ELSE	----IF CATEGORY IS OTHER THAN PROVISIONAL
	BEGIN
		select @serviceChargePercent=ServiceChargePercent, @serviceChargeFixed=ServiceChargeFixed,@minServiceCharge=MinServiceCharge,@maxServiceCharge=MaxServiceCharge  
		from dbo.tbl_service_charge_settings with(nolock) where  SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency 
		AND ChargeCategoryId=@partnerChargeCategoryId AND PaymentTypeId=@PaymentTypeId AND @SendingAmount BETWEEN MinAmountSlab AND MaxAmountSlab; 
	END
END


-------------PARTNER SERVICE CHARGE AND COMMISSION CALCULATION----------------------
IF(@partnerChargeCategory LIKE '%provision%')	----IF CATEGORY IS PROVISIONAL
BEGIN
	select  @adminServiceChargePercent=ServiceChargePercent, @adminServiceChargeFixed=ServiceChargeFixed,@adminMinServiceCharge=MinServiceCharge,@adminMaxServiceCharge=MaxServiceCharge
		,@commissionPercent=CommissionPercent, @commissionFixed=CommissionFixed,@mincommission=MinCommission,@maxcommission=MaxCommission  
	from dbo.tbl_service_charge_settings with(nolock) where  SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency   
		AND ChargeCategoryId=@partnerChargeCategoryId AND PaymentTypeId=@PaymentTypeId AND @SendingAmount BETWEEN MinAmountSlab AND MaxAmountSlab 
		AND CAST(@partnerCurrDateTime AS DATE) BETWEEN FromDate AND ToDate; 
END
ELSE	----IF CATEGORY IS OTHER THAN PROVISIONAL
BEGIN
select  @adminServiceChargePercent=ServiceChargePercent, @adminServiceChargeFixed=ServiceChargeFixed,@adminMinServiceCharge=MinServiceCharge,@adminMaxServiceCharge=MaxServiceCharge
	,@commissionPercent=CommissionPercent, @commissionFixed=CommissionFixed,@mincommission=MinCommission,@maxcommission=MaxCommission  
from dbo.tbl_service_charge_settings with(nolock) where  SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency   
	AND ChargeCategoryId=@partnerChargeCategoryId AND PaymentTypeId=@PaymentTypeId AND @SendingAmount BETWEEN MinAmountSlab AND MaxAmountSlab;
END

SET @serviceCharge = @serviceChargeFixed + ((@SendingAmount * @serviceChargePercent)/100);  
SET @commission = @commissionFixed + ((@SendingAmount * @commissionPercent)/100);  
SET @partnerServiceCharge = @adminServiceChargeFixed + ((@SendingAmount * @adminServiceChargePercent)/100);
  
IF(@serviceCharge < @minServiceCharge)  
BEGIN  
 SET @serviceCharge = @minServiceCharge;  
END  
ELSE IF(@serviceCharge > @maxServiceCharge)  
BEGIN  
 SET @serviceCharge = @maxServiceCharge;  
END  
  
IF(@commission < @minCommission)  
BEGIN  
 SET @commission = @minCommission;  
END
IF(@commission > @maxCommission)  
BEGIN  
 SET @commission = @maxCommission;  
END  

IF(@partnerServiceCharge < @adminMinServiceCharge)  
BEGIN  
 SET @partnerServiceCharge = @adminMinServiceCharge;  
END  
ELSE IF(@partnerServiceCharge > @adminMaxServiceCharge)  
BEGIN  
 SET @partnerServiceCharge = @adminMaxServiceCharge;  
END 
  
SET @netSendingAmount =  @SendingAmount - @serviceCharge;  
SET @conversionRate = @conversionRate/@unitValue;  
SET @netReceivingAmountNPR = @netSendingAmount * @conversionRate;  

IF(@netSendingAmount < 0)
	SET @netSendingAmount = 0;
IF(@netReceivingAmountNPR < 0)
	SET @netReceivingAmountNPR = 0;
 
 INSERT INTO @tblServiceCharge
SELECT @SourceCurrency , @DestinationCurrency, @SendingAmount,@conversionRate,@netSendingAmount, @netReceivingAmountNPR, @serviceCharge
	,ISNULL(@commission,0),@partnerServiceCharge
	
RETURN;

END;
GO


