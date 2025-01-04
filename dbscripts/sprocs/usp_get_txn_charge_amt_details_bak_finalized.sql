/*
----------------------------------------------
--EXECUTE STORED PROCEDURE:
----------------------------------------------
 declare @ReturnPrimaryId INT = NULL       
 declare @StatusCode INT = NULL       
 declare @MsgType NVARCHAR(10) = NULL       
 declare @MsgText NVARCHAR(200) = NULL 
 exec [dbo].[usp_get_txn_charge_amt_details] 'REM3304621679','AUD','NPR',100.00,4
			,@ReturnPrimaryId output,@StatusCode output,@MsgType output,@MsgText output
 select @ReturnPrimaryId AS ReturnPrimaryId,@StatusCode as StatusCode,@MsgType as MsgType,@MsgText as MsgText

*/

CREATE OR ALTER   procedure [dbo].[usp_get_txn_charge_amt_details_bak_finalized] --'REM5468670119', 'AUD', 'NPR', 100.00  
(  
@PartnerCode varchar(20) = null  
,@SourceCurrency varchar(3) = null  
,@DestinationCurrency varchar(3) = null  
,@SendingAmount decimal(18,2) = null  
,@PaymentType varchar(50) = null			-----BANK/WALLET/CASH
,@ReturnPrimaryId INT = NULL OUTPUT      
,@StatusCode INT = NULL OUTPUT      
,@MsgType NVARCHAR(10) = NULL OUTPUT      
,@MsgText NVARCHAR(200) = NULL OUTPUT  
)  
as  
BEGIN  
  
SET NOCOUNT ON;  
  
SET @StatusCode = 400      
SET @MsgType = 'Error'      
SET @MsgText = 'Bad Request'      
SET @ReturnPrimaryId = 0   


DECLARE @partnerCurrDateTime DATETIME, @partnerGMTTimeZone varchar(20), @PaymentTypeId INT;  
DECLARE @unitValue INT, @conversionRate decimal(18,4), @serviceCharge decimal(18,4), @netSendingAmount decimal(18,4), @netReceivingAmountNPR decimal(18,4)  
  ,@commission decimal(18,4), @partnerChargeCategoryId int, @partnerChargeCategory varchar(50);  

declare @serviceChargePercent decimal(18,2), @serviceChargeFixed decimal(18,4), @minServiceCharge decimal(18,4),@maxServiceCharge decimal(18,4);  
DECLARE @adminServiceChargePercent decimal(18,4), @adminServiceChargeFixed decimal(18,4),@adminMinServiceCharge decimal(18,4)
		,@adminMaxServiceCharge decimal(18,4),@partnerServiceCharge decimal(18,4);
declare @commissionPercent decimal(18,2), @commissionFixed decimal(18,4), @minCommission decimal(18,4),@maxCommission decimal(18,4);  
  
  
---------------------------------------VALIDATIONS BEGINS------------------------------------------------------  

IF NOT EXISTS(select 1 from dbo.tbl_remit_partners with(nolock) where  PartnerCode=@PartnerCode AND IsActive=1 AND ISNULL(IsDeleted,0)=0)  
BEGIN
	SET @MsgText = 'Invalid Partner!'  
	RETURN;
END
IF(ISNULL(@SourceCurrency,'') = '') 
BEGIN
	SET @MsgText = 'Please select Source Currency!'  
	RETURN;
END
IF(ISNULL(@DestinationCurrency,'') = '') 
BEGIN
	SET @MsgText = 'Please select Destination Currency!'  
	RETURN;
END
IF(@DestinationCurrency <> 'NPR') 
BEGIN
	SET @MsgText = 'Destination Currency should be NPR!'  
	RETURN;
END
IF(ISNULL(@SendingAmount,0) <= 0) 
BEGIN
	SET @MsgText = 'Invalid Sending Amount!'  
	RETURN;
END

------------------------START: SET DEFAULT VALUES---------------------------------
select @unitValue = UnitValue, @conversionRate=CurrentRate from dbo.tbl_exchange_rate with(nolock) where SourceCurrency=@SourceCurrency and DestinationCurrency=@DestinationCurrency 
	and IsActive=1 and ISNULL(IsDeleted,0)=0;
--IF(ISNULL(@PaymentType,'') = '')
--BEGIN
--	select @PaymentTypeId=Id from dbo.tbl_payment_type with(nolock) where IsActive=1 and isnull(IsDeleted,0)=0 and PaymentTypeCode='BANK';   
--END
select @partnerChargeCategoryId=ChargeCategoryId,@partnerChargeCategory=c.CategoryName, @partnerGMTTimeZone=GMTTimeZone from dbo.tbl_remit_partners p with(nolock) 
	inner join dbo.tbl_service_charge_category c with(nolock) on c.Id=p.ChargeCategoryId and c.IsActive=1 and isnull(c.IsDeleted,0)=0
	where PartnerCode=@PartnerCode and p.IsActive=1 and isnull(p.IsDeleted,0)=0; 
SET @partnerCurrDateTime =  (select [dbo].[func_convert_gmt_to_local_date](GETUTCDATE(),@partnerGMTTimeZone))

------------------------END: SET DEFAULT VALUES--------------------------------------------------
IF(ISNULL(@partnerGMTTimeZone,'') = '')
BEGIN
	SET @MsgText = 'Partner GMTTimeZone not defined!'  
	RETURN;
END
IF(ISNULL(@PaymentType,'') = '')
BEGIN
	SET @MsgText = 'Select Payment Type!'  
	RETURN;
END
IF NOT EXISTS(select 1 from dbo.tbl_payment_type with(nolock) where @PaymentType in ('BANK','WALLET','CASH') AND IsActive=1 AND ISNULL(IsDeleted,0)=0)  
BEGIN
	SET @MsgText = 'Invalid Payment Type!'  
	RETURN;
END

SET @PaymentTypeId = (select Id from dbo.tbl_payment_type with(nolock) where PaymentTypeCode = @PaymentType)

IF(@partnerChargeCategory LIKE '%provision%')	----IF CATEGORY IS PROVISIONAL
BEGIN
	IF NOT EXISTS(select 1 from dbo.tbl_service_charge_settings with(nolock) where  SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency   
		AND ChargeCategoryId=@partnerChargeCategoryId AND PaymentTypeId=@PaymentTypeId AND @SendingAmount BETWEEN MinAmountSlab AND MaxAmountSlab
		AND CAST(@partnerCurrDateTime AS DATE) BETWEEN FromDate AND ToDate
		)  
	BEGIN
		SET @MsgText = 'Slab is not defined by admin, so can not fetch service charge and commission details for given PaymentType, Charge Category and Currency combination!'  
		RETURN;
	END
END
ELSE	----IF CATEGORY IS OTHER THAN PROVISIONAL
BEGIN
	IF NOT EXISTS(select 1 from dbo.tbl_service_charge_settings with(nolock) where  SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency   
		AND ChargeCategoryId=@partnerChargeCategoryId AND PaymentTypeId=@PaymentTypeId AND @SendingAmount BETWEEN MinAmountSlab AND MaxAmountSlab
		)  
	BEGIN
		SET @MsgText = 'Slab is not defined by admin, so can not fetch service charge and commission details for given PaymentType, Charge Category and Currency combination!'  
		RETURN;
	END
END


  
---------------------------------------VALIDATIONS END------------------------------------------------------ 

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
  
SELECT @SourceCurrency AS SourceCurrency, @DestinationCurrency AS DestinationCurrency, @SendingAmount AS SendingAmount,@conversionRate AS ConversionRate  
,@netSendingAmount AS NetSendingAmount, @netReceivingAmountNPR AS RecivingAmountNPR, @serviceCharge AS ServiceCharge, ISNULL(@commission,0) AS Commission 
,@partnerServiceCharge AS PartnerServiceCharge
  
SET @StatusCode = 200      
SET @MsgType = 'Succeess'      
SET @MsgText = 'Transaction Amount detail fetched successfully!'      
SET @ReturnPrimaryId = 111  
  
SET NOCOUNT OFF;  
END