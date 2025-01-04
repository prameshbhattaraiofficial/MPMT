create or alter procedure [dbo].[usp_remit_wallet_status_log]  
(  
@PayoutRefereneNo varchar(100) = null  
,@TransactionStatus varchar(200) = null  
,@AgentStatusCode varchar(50) = null  
,@Message varchar(100) = null  ----SUCCESS/ERROR  
,@ResponseMessage varchar(300) = null  
,@Details varchar(500) = null  
,@ResponseCode varchar(50) = null  
,@Status bit = null  
,@UserType varchar(100) = null  
,@LoggedInUser varchar(100) = null  
,@ReturnPrimaryId INT = NULL OUTPUT  
,@StatusCode INT = NULL OUTPUT        
,@MsgType NVARCHAR(10) = NULL OUTPUT        
,@MsgText NVARCHAR(200) = NULL OUTPUT   
)  
AS   
BEGIN  
SET @StatusCode = 400        
SET @MsgType = 'Error'        
SET @MsgText = 'Bad Request'   
SET @ReturnPrimaryId = 0  
  
DECLARE @ErrorNumber INT         
DECLARE @ErrorMessage NVARCHAR(4000)        
DECLARE @ErrorSeverity INT        
DECLARE @ErrorState INT        
DECLARE @ErrorProcedure NVARCHAR(256)        
DECLARE @ErrorLine INT      
  
  
IF NOT EXISTS(select 1 from dbo.tbl_remit_payout_transaction with(nolock) where PayoutReferenceNo=@PayoutRefereneNo)  
BEGIN  
 SET @MsgText = 'Invalid Transaction Reference Number!'     
 RETURN;  
END  
  
DECLARE @PartnerCode varchar(20), @SourceCurrency varchar(3), @DestinationCurrency varchar(3), @PaymentType varchar(50), @SendingAmount decimal(18,4)    
 ,@PartnerCommission decimal(18,4),@PartnerGMTValue varchar(20);   
  
DECLARE @RemitTransactionId varchar(100), @ExistingAgentStatusCode varchar(50);  
SET @RemitTransactionId = (SELECT RemitTransactionId FROM dbo.tbl_remit_payout_transaction WITH(NOLOCK) WHERE PayoutReferenceNo=@PayoutRefereneNo)  
SET @ExistingAgentStatusCode = (SELECT top 1 AgentStatusCode FROM dbo.tbl_remit_payout_transaction with(nolock) where RemitTransactionId=@RemitTransactionId order by CreatedDate DESC)  
  
SELECT @PartnerCode=t.PartnerCode,@SourceCurrency=t.SourceCurrency,@DestinationCurrency=t.DestinationCurrency,@PaymentType=t.PaymentType    
 ,@SendingAmount=t.SendingAmount,@PartnerGMTValue=ISNULL(p.GMTTimeZone,'UTC+5:45')    
FROM dbo.tbl_remit_transaction t WITH(NOLOCK)    
INNER JOIN dbo.tbl_remit_partners p WITH(NOLOCK) ON p.PartnerCode=t.PartnerCode    
WHERE TransactionId=@RemitTransactionId;    
  
SET @PartnerCommission = (SELECT [dbo].[func_get_partner_commission](@PartnerCode,@SourceCurrency,@DestinationCurrency,@SendingAmount,@PaymentType));    
  
----------------RETURN TRUE INCASE OF ALREADY PAYOUT SUCCESSFUL--------------------------------  
IF EXISTS(SELECT 1 FROM dbo.tbl_remit_transaction WITH(NOLOCK) WHERE Transactionid=@RemitTransactionId AND StatusCode='55')  
BEGIN  
 SET @StatusCode = 200        
 SET @MsgType = 'Ok'        
 SET @MsgText = 'Transaction Payout already Success!'     
 SET @ReturnPrimaryId = 111  
 RETURN;  
END  
  
        
BEGIN TRY          
BEGIN TRANSACTION            
SET NOCOUNT ON;   
  
IF(@ResponseCode='1' AND ISNULL(@Status,0)=1)  ------PAYMENT SUCCESS  
BEGIN  
 IF(@AgentStatusCode='1')  
 BEGIN  
  IF EXISTS(SELECT 1 FROM dbo.tbl_remit_payout_transaction with(nolock) where RemitTransactionId=@RemitTransactionId AND AgentStatusCode='1')  
  BEGIN  
   SET @StatusCode = 200        
     SET @MsgType = 'Transaction Payout already Success!'        
     SET @MsgText = 'Ok'     
     SET @ReturnPrimaryId = 111  
  END  
  
  UPDATE dbo.tbl_remit_payout_transaction   
  SET   
  AgentTransactionStatus = @TransactionStatus  
  ,AgentStatusCode = @AgentStatusCode  
  ,Message = @Message  
  ,Details = @Details  
  ,ResponseMessage = @ResponseMessage  
  ,AgentResponseCode = @ResponseCode  
  ,AgentStatus = ISNULL(@Status,0)   
  ,StatusCode = (select StatusCode from dbo.tbl_transaction_status with(nolock) where IsActive=1 and ISNULL(IsDeleted,0)=0  and LookupName='SUCCESS')  
  ,Reasoncode = 'R1009'  
  ,CreatedDate = GETUTCDATE()   
  ,SenderCreatedDate = [dbo].[func_convert_gmt_to_local_date](GETUTCDATE(),@PartnerGMTValue)  
  ,ReceiverCreatedDate = [dbo].[func_convert_gmt_to_local_date](GETUTCDATE(),'UTC+5:45')  
  WHERE PayoutReferenceNo = @PayoutRefereneNo;  
   
  
  
  INSERT INTO dbo.tbl_remit_transaction_history  
  SELECT * FROM dbo.tbl_remit_transaction WITH(NOLOCK) WHERE TransactionId = @RemitTransactionId;  
  
  UPDATE dbo.tbl_remit_transaction   
  SET   
  AgentTransactionStatus = @TransactionStatus  
  ,AgentStatusCode = @AgentStatusCode  
  ,Message = @Message  
  ,ResponseMessage = @ResponseMessage  
  ,AgentResponseCode = @ResponseCode  
  ,AgentStatus = ISNULL(@Status,0)  
  ,StatusCode = (select StatusCode from dbo.tbl_transaction_status with(nolock) where IsActive=1 and ISNULL(IsDeleted,0)=0  and LookupName='SUCCESS')  
  ,Reasoncode = 'R1009'  
  WHERE TransactionId = @RemitTransactionId;  
  
  declare @newTransactionId varchar(100);  
  gn: EXEC dbo.usp_generate_random_numeric_string 23, @newTransactionId OUTPUT  
  IF EXISTS(SELECT 1 FROM dbo.tbl_commission_transaction WITH(NOLOCK) WHERE TransactionId=@newTransactionId)  
   GOTO gn;  
  
  declare @existingWalletBalance decimal(18,4);  
  SET @existingWalletBalance = (SELECT ISNULL(Balance,0) from dbo.tbl_partner_wallets with(nolock) where PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency and IsActive=1 and ISNULL(IsDeleted,0)=0)  
  
  --SELECT @newTransactionId  
  
  INSERT INTO dbo.tbl_commission_transaction   
   (TransactionId,ParentTransactionId,PartnerCode,SourceCurrency,DestinationCurrency,Commission  
   ,PreviousWalletBalance,CurrentWalletBalance,Sign,Status  
   ,CreatedDate  
   ,SenderCreatedDate  
   ,ReceiverCreatedDate)  
  VALUES  
   (@newTransactionId,@RemitTransactionId,@PartnerCode,@SourceCurrency,@DestinationCurrency,@PartnerCommission  
   ,@existingWalletBalance, @existingWalletBalance+@PartnerCommission,'CR','Success'  
   ,GETUTCDATE()  
   ,dbo.func_convert_gmt_to_local_date(GETUTCDATE(), @PartnerGMTValue)  
   ,dbo.func_convert_gmt_to_local_date(GETUTCDATE(), 'UTC+5:45'))  
  
  INSERT INTO tbl_partner_wallets_history  
   SELECT * FROM dbo.tbl_partner_wallets WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency AND IsActive=1 AND ISNULL(IsDeleted,0)=0;  
  
  UPDATE dbo.tbl_partner_wallets SET Balance = Balance + @PartnerCommission WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency   
    AND IsActive=1 AND ISNULL(IsDeleted,0)=0;  
  
 END  
 ELSE   
 BEGIN  
  IF(@ExistingAgentStatusCode <> @AgentStatusCode)  
  BEGIN  
   UPDATE dbo.tbl_remit_payout_transaction   
   SET   
   AgentTransactionStatus = @TransactionStatus  
   ,AgentStatusCode = @AgentStatusCode  
   ,Message = @Message  
   ,Details = @Details  
   ,ResponseMessage = @ResponseMessage  
   ,AgentResponseCode = @ResponseCode  
   ,AgentStatus = ISNULL(@Status,0)   
   ,StatusCode = case when @AgentStatusCode='2' then '54'  
       when @AgentStatusCode='3' then '53'  
       when @AgentStatusCode='4' then '52'  
       when @AgentStatusCode='5' then '51'  
       END  
   ,Reasoncode = case when @AgentStatusCode='2' then 'R1010'  
       when @AgentStatusCode='3' then 'R1011'  
       when @AgentStatusCode='4' then 'R1008'  
       when @AgentStatusCode='5' then 'R1007'  
       else 'R1008'  
       END  
   ,CreatedDate = GETUTCDATE()   
   ,SenderCreatedDate = [dbo].[func_convert_gmt_to_local_date](GETUTCDATE(),@PartnerGMTValue)  
   ,ReceiverCreatedDate = [dbo].[func_convert_gmt_to_local_date](GETUTCDATE(),'UTC+5:45')  
   WHERE PayoutReferenceNo = @PayoutRefereneNo;  
  
  
  INSERT INTO dbo.tbl_remit_transaction_history  
  SELECT * FROM dbo.tbl_remit_transaction WITH(NOLOCK) WHERE TransactionId = @RemitTransactionId;  
  
  UPDATE dbo.tbl_remit_transaction   
  SET   
  AgentTransactionStatus = @TransactionStatus  
  ,AgentStatusCode = @AgentStatusCode  
  ,Message = @Message  
  ,ResponseMessage = @ResponseMessage  
  ,AgentResponseCode = @ResponseCode  
  ,AgentStatus = ISNULL(@Status,0)  
  ,StatusCode = case when @AgentStatusCode='2' then '54'  
       when @AgentStatusCode='3' then '53'  
       when @AgentStatusCode='4' then '52'  
       when @AgentStatusCode='5' then '51'  
       END  
  ,Reasoncode = case when @AgentStatusCode='2' then 'R1010'  
       when @AgentStatusCode='3' then 'R1011'  
       when @AgentStatusCode='4' then 'R1008'  
       when @AgentStatusCode='5' then 'R1007'  
       else 'R1008'  
       END  
  WHERE TransactionId = @RemitTransactionId;  
  
  END  
 END  
   
END  
  
SET @StatusCode = 200        
SET @MsgType = 'Ok'        
SET @MsgText = 'Success'     
SET @ReturnPrimaryId = 111  
  
SET NOCOUNT OFF;        
            
   SET @StatusCode = 200        
   SET @MsgType = 'Success'        
   SET @MsgText = 'Ok'     
   SET @ReturnPrimaryId = 111  
 COMMIT TRANSACTION          
          
 END TRY          
 BEGIN CATCH          
 ROLLBACK TRANSACTION          
        
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