CREATE OR ALTER PROCEDURE [dbo].[usp_partner_conversion_rate_insert]  
(  
@PartnerCode varchar(20) = NULL  
,@SourceCurrency varchar(3) = NULL  
,@DestinationCurrency varchar(3) = NULL  
,@ConversionSettingsType ConversionSettingsType READONLY  
,@OperationMode nvarchar(10) = NULL      
,@LoggedInUser [nvarchar](100) = NULL      
,@UserType [varchar](20) = NULL      
,@ReturnPrimaryId INT = NULL OUTPUT      
,@StatusCode INT = NULL OUTPUT      
,@MsgType NVARCHAR(10) = NULL OUTPUT      
,@MsgText NVARCHAR(200) = NULL OUTPUT  
)  
AS  
BEGIN  
   
 SET NOCOUNT ON;  
  
 SET @StatusCode = 400      
 SET @MsgType = 'Error'      
 SET @MsgText = 'Bad Request'      
 SET @ReturnPrimaryId = 0   
      
 DECLARE @LoggedInUserId INT;  
  
 IF(ISNULL(@UserType,'') = '')  
  SET @UserType = 'ADMIN'  
  
 SET @LoggedInUserId = (SELECT [dbo].[func_get_userid](@UserType, @LoggedInUser))  
  
 DECLARE @tmpWallet AS TABLE(SourceCurrency varchar(3),DestinationCurrency varchar(3), UnitValue int, MinRate decimal(18,4), MaxRate decimal(18,4),CurrentRate decimal(18,4));  
 INSERT INTO @tmpWallet  
  SELECT w.SourceCurrency, w.DestinationCurrency, r.UnitValue  
  ,CASE WHEN w.TypeCode='FLAT' THEN r.CurrentRate + w.MarkupMinValue ELSE (r.CurrentRate + (r.CurrentRate * w.MarkupMinValue)/100) END AS MinRate  
  ,CASE WHEN w.TypeCode='FLAT' THEN r.CurrentRate + w.MarkupMaxValue ELSE (r.CurrentRate + (r.CurrentRate * w.MarkupMaxValue)/100) END AS MaxRate  
  ,r.CurrentRate   
  FROM dbo.tbl_partner_wallets w WITH(NOLOCK)  
  INNER JOIN dbo.tbl_exchange_rate r WITH(NOLOCK) ON r.SourceCurrency=w.SourceCurrency AND r.DestinationCurrency=w.DestinationCurrency   
   AND w.IsActive=1 AND ISNULL(w.IsDeleted,0)=0 AND r.IsActive=1 AND ISNULL(r.IsDeleted,0)=0  
  WHERE w.PartnerCode=@PartnerCode AND w.SourceCurrency=@SourceCurrency AND w.DestinationCurrency=@DestinationCurrency;  
           
 DECLARE @ErrorNumber INT   
 DECLARE @ErrorMessage NVARCHAR(4000)  
 DECLARE @ErrorSeverity INT  
 DECLARE @ErrorState INT  
 DECLARE @ErrorProcedure NVARCHAR(256)  
 DECLARE @ErrorLine INT  
  
 ------=================VALIDATION BEGINS=======================================================  
 DECLARE @sn INT, @length INT, @minSlab DECIMAL(18,2), @maxSlab DECIMAL(18,2), @conversionRate DECIMAL(18,4);   
 DECLARE @tmpCharge AS TABLE (Id INT IDENTITY(1,1), MinSlab DECIMAL(18,2), MaxSlab DECIMAL(18,2), ConversionRate DECIMAL(18,4));  
 INSERT INTO @tmpCharge  
 SELECT MinAmountSlab,MaxAmountSlab,ConversionRate FROM @ConversionSettingsType;  
   
 SET @sn = 1;  
 SET @length = (SELECT COUNT(*) FROM @tmpCharge);  
 WHILE (@sn <= @length)  
 BEGIN  
  SELECT @minSlab = MinSlab, @maxSlab=MaxSlab, @conversionRate=ConversionRate FROM @tmpCharge WHERE Id=@sn;  
  IF(@minSlab > @maxSlab)  
  BEGIN  
   SET @MsgText = 'Invalid Slab. Min slab can not be greater than Max slab!'   
   RETURN;  
  END  
  IF NOT EXISTS(SELECT 1 FROM @tmpWallet WHERE @conversionRate BETWEEN MinRate AND MaxRate)  
  BEGIN  
   SET @MsgText = 'Invalid Conversion Rate. One of Conversion Rate is out of Min and Max Rate range!'   
   RETURN;  
  END  
  
  IF EXISTS(SELECT 1 FROM @tmpCharge WHERE ((@minSlab between MinSlab AND MaxSlab) OR  (@maxSlab between MinSlab AND MaxSlab)) AND Id<>@sn)  
  BEGIN  
   SET @MsgText = 'Invalid Slab range!'   
   RETURN;  
  END  
  
  SET @sn = @sn +1;  
 END   
  
  
 IF EXISTS(SELECT 1 FROM @ConversionSettingsType WHERE ServiceChargePercent >= 100 OR CommissionPercent >= 100)  
 BEGIN  
  SET @MsgText = 'Invalid ServiceCharge/Commission Percentage value!'   
  RETURN;  
 END  
 IF EXISTS(SELECT 1 FROM @ConversionSettingsType WHERE MinServiceCharge > MaxServiceCharge)  
 BEGIN  
  SET @MsgText = 'MinServiceCharge should not be greater than MaxServiceCharge!'   
  RETURN;  
 END  
 IF EXISTS(SELECT 1 FROM @ConversionSettingsType WHERE MinCommission > MaxCommission)  
 BEGIN  
  SET @MsgText = 'MinCommission should not be greater than MaxCommision!'   
  RETURN;  
 END  
  
 ------=================VALIDATION ENDS=======================================================  
  
 IF NOT EXISTS(SELECT 1 FROM dbo.tbl_partner_conversion_rate_setting WITH(NOLOCK) WHERE SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency)  ------INSERT RECORD  
 BEGIN  
  BEGIN TRY  
    
  Insert INTO [dbo].[tbl_partner_conversion_rate_setting]([PartnerCode],[SourceCurrency],[DestinationCurrency]  
  ,[MinAmountSlab],[MaxAmountSlab],[ConversionRate],[ServiceChargePercent],[ServiceChargeFixed],[MinServiceCharge]  
  ,[MaxServiceCharge],[CommissionPercent],[CommissionFixed],[MinCommission],[MaxCommission],[FromDate],[ToDate]  
  ,[IsActive],[IsDeleted],[CreatedById],[CreatedByName],[CreatedDate])   
  
  SELECT       
    @PartnerCode  
      ,@SourceCurrency  
      ,@DestinationCurrency  
      ,[MinAmountSlab]  
      ,[MaxAmountSlab]  
   ,[ConversionRate]  
      ,[ServiceChargePercent]  
      ,[ServiceChargeFixed]  
      ,[MinServiceCharge]  
      ,[MaxServiceCharge]  
      ,[CommissionPercent]  
      ,[CommissionFixed]  
      ,[MinCommission]  
      ,[MaxCommission]  
      ,NULL  
      ,NULL  
      ,1  
      ,0  
      ,@LoggedInUserId  
      ,@LoggedInUser  
      ,GETUTCDATE()  
      from @ConversionSettingsType;  
  
  SET @StatusCode = 200      
 SET @MsgType = 'Success'      
 SET @MsgText = 'Data added successfuly!'      
 SET @ReturnPrimaryId = 111  
  
   END TRY  
 BEGIN CATCH  
  SET @ErrorNumber  = ERROR_NUMBER();  
        SET @ErrorMessage  = ERROR_MESSAGE();  
        SET @ErrorSeverity  = ERROR_SEVERITY();  
        SET @ErrorState  = ERROR_STATE();  
        SET @ErrorProcedure  = ERROR_PROCEDURE();  
        SET @ErrorLine  = ERROR_LINE();  
  
        EXEC [dbo].[usp_logdberror] @ErrorNumber, @ErrorMessage, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine;  
    
  -- Re-throw the error to the calling application  
        THROW;  
 END CATCH  
  
 END  
 ELSE   ------UPDATE RECORD  
 BEGIN  
  BEGIN TRY  
    
  INSERT INTO dbo.tbl_partner_conversion_rate_setting_history  
   SELECT * FROM dbo.tbl_partner_conversion_rate_setting WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency;  
  
  DELETE FROM dbo.tbl_partner_conversion_rate_setting WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency;  
  
  Insert INTO [dbo].[tbl_partner_conversion_rate_setting]([PartnerCode],[SourceCurrency],[DestinationCurrency]  
  ,[MinAmountSlab],[MaxAmountSlab],[ConversionRate],[ServiceChargePercent],[ServiceChargeFixed],[MinServiceCharge]  
  ,[MaxServiceCharge],[CommissionPercent],[CommissionFixed],[MinCommission],[MaxCommission],[FromDate],[ToDate]  
  ,[IsActive],[IsDeleted],[UpdatedById],[UpdatedByName],[UpdatedDate])   
  
  SELECT       
    @PartnerCode  
      ,@SourceCurrency  
      ,@DestinationCurrency  
      ,[MinAmountSlab]  
      ,[MaxAmountSlab]  
   ,[ConversionRate]  
      ,[ServiceChargePercent]  
      ,[ServiceChargeFixed]  
      ,[MinServiceCharge]  
      ,[MaxServiceCharge]  
      ,[CommissionPercent]  
      ,[CommissionFixed]  
      ,[MinCommission]  
      ,[MaxCommission]  
      ,NULL  
      ,NULL  
      ,1  
      ,0  
      ,@LoggedInUserId  
      ,@LoggedInUser  
      ,GETUTCDATE()  
      from @ConversionSettingsType;  
  
   SET @StatusCode = 200      
 SET @MsgType = 'Success'      
 SET @MsgText = 'Data updated successfuly!'      
 SET @ReturnPrimaryId = 111  
  
   END TRY  
 BEGIN CATCH  
  SET @ErrorNumber  = ERROR_NUMBER();  
        SET @ErrorMessage  = ERROR_MESSAGE();  
        SET @ErrorSeverity  = ERROR_SEVERITY();  
        SET @ErrorState  = ERROR_STATE();  
        SET @ErrorProcedure  = ERROR_PROCEDURE();  
        SET @ErrorLine  = ERROR_LINE();  
  
        EXEC [dbo].[usp_logdberror] @ErrorNumber, @ErrorMessage, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine;  
    
  -- Re-throw the error to the calling application  
        THROW;  
 END CATCH  
 END  
  
 SET NOCOUNT OFF;  
 RETURN 1;  
End