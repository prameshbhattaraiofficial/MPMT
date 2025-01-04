
/*
----------------------------------------------
--EXECUTE STORED PROCEDURE:
----------------------------------------------
 declare @ReturnPrimaryId INT = NULL       
 declare @StatusCode INT = NULL       
 declare @MsgType NVARCHAR(10) = NULL       
 declare @MsgText NVARCHAR(200) = NULL 
 exec [dbo].[usp_get_txn_charge_amt_details] 'REM3304621679','AUD','NPR',100.00,'BANK'
			,@ReturnPrimaryId output,@StatusCode output,@MsgType output,@MsgText output
 select @ReturnPrimaryId AS ReturnPrimaryId,@StatusCode as StatusCode,@MsgType as MsgType,@MsgText as MsgText

*/

CREATE OR ALTER  PROCEDURE [dbo].[usp_get_txn_charge_amt_details] --'REM5468670119', 'AUD', 'NPR', 100.00  
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
DECLARE @partnerChargeCategoryId int, @partnerChargeCategory varchar(50);
  
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

------------------------------------------RESULT-----------------------------------------------
SELECT SourceCurrency,DestinationCurrency,SendingAmount,ConversionRate,NetSendingAmount,ReceivingAmountNPR,ServiceCharge
	,Commission,PartnerServiceCharge
FROM [dbo].[func_get_partner_service_charge](@PartnerCode,@SourceCurrency,@DestinationCurrency,@SendingAmount,@PaymentType);
  
SET @StatusCode = 200      
SET @MsgType = 'Succeess'      
SET @MsgText = 'Transaction Amount detail fetched successfully!'      
SET @ReturnPrimaryId = 111  
  
SET NOCOUNT OFF;  
END