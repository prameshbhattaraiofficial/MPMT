  
/*        
----------------------------------------------        
--EXECUTE STORED PROCEDURE:        
----------------------------------------------        
 declare @ReturnPrimaryId VARCHAR(50) = NULL               
 declare @StatusCode INT = NULL               
 declare @MsgType NVARCHAR(10) = NULL               
 declare @MsgText NVARCHAR(200) = NULL         
 exec [dbo].[usp_get_payout_referenceNo] '2816215520508616280047002','WALLET','','','','',''  
   ,@ReturnPrimaryId output,@StatusCode output,@MsgType output,@MsgText output        
 select @ReturnPrimaryId AS ReturnPrimaryId,@StatusCode as StatusCode,@MsgType as MsgType,@MsgText as MsgText        
        
*/        
CREATE OR ALTER   PROCEDURE [dbo].[usp_get_payout_referenceNo]             
(            
 @RemitTransactionId varchar(100) = NULL     
 ,@PaymentType varchar(50) = null     --BANK,WALLET OR CASH           
 ,@AgentCode varchar(50) = NULL        
 ,@IpAddress nvarchar(50) = NULL            
 ,@DeviceId nvarchar(500) = NULL        
 ,@LoggedInUser [nvarchar](100) = NULL            
 ,@UserType [varchar](20) = NULL            
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
   
 DECLARE @TransactionType varchar(50), @payoutReferenceNo varchar(100),@NetRecievingAmountNPR decimal(18,4),@WalletNumber varchar(30)  
 ,@WalletName varchar(200),@WalletCode varchar(50),@WalletHolderName varchar(200),@BankCode varchar(50), @BankName varchar(200)  
 , @AccountNumber varchar(50), @AccountHolderName varchar(200),@Branch varchar(100);  
 DECLARE @LoggedInUserId INT, @PartnerGMTTimeZone varchar(20),@PartnerCode varchar(50);        
        
 IF(ISNULL(@UserType,'') = '')        
  SET @UserType = 'PARTNER'        
        
 SET @LoggedInUserId = (SELECT [dbo].[func_get_userid](@UserType, @LoggedInUser))    
 SET @PartnerGMTTimeZone = (SELECT ISNULL(GMTTimeZone,'UTC+5:45') FROM dbo.tbl_remit_partners WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND IsActive=1 AND ISNULL(IsDeleted,0)=0);  
  
 IF(ISNULL(@AgentCode,'') = '')  
 BEGIN  
 SET @AgentCode = (SELECT AgentCode FROM dbo.tbl_remit_agents WITH(NOLOCK) WHERE LookupName='MYPAY' AND IsActive=1 AND ISNULL(IsDeleted,0)=0)  
 END  
        
 --------------START: BANK DETAIL VALIDATION------------------------        
 IF NOT EXISTS(SELECT 1 FROM dbo.tbl_remit_transaction WITH(NOLOCK) WHERE TransactionId=@RemitTransactionId)  
 BEGIN  
 SET @MsgText = 'Invalid Remit TransactionId!'            
   RETURN;    
 END  
   
 IF(ISNULL(@PaymentType,'') NOT IN ('BANK','WALLET','CASH'))  
  BEGIN  
 SET @MsgText = 'Invalid Payout Type!'            
   RETURN;    
 END  
     
 --------------END: BANK DETAIL VALIDATION------------------------       
   
   
  
select @NetRecievingAmountNPR=NetReceivingAmount,@WalletNumber=WalletNumber,@WalletName=WalletName,@WalletHolderName=WalletHolderName  
 ,@WalletCode=WalletCode,@BankCode=BankCode,@BankName=BankName,@AccountNumber=AccountNumber,@AccountHolderName=AccountHolderName  
 ,@Branch=Branch,@PartnerCode=PartnerCode,@TransactionType=TransactionType  
from dbo.tbl_remit_transaction with(nolock) where TransactionId = @RemitTransactionId;   
     
----------------------------------END: DATA VALIDATION---------------------------------------------------------        
 DECLARE @ErrorNumber INT         
 DECLARE @ErrorMessage NVARCHAR(4000)        
 DECLARE @ErrorSeverity INT        
 DECLARE @ErrorState INT        
 DECLARE @ErrorProcedure NVARCHAR(256)        
 DECLARE @ErrorLine INT        
        
 BEGIN TRY          
 BEGIN TRANSACTION            
  SET NOCOUNT ON;        
  DECLARE @newPayoutRefNo NVARCHAR(100);        
  gn: EXEC usp_generate_random_numeric_string 27, @newPayoutRefNo OUTPUT      
  IF EXISTS(SELECT 1 FROM dbo.tbl_remit_payout_transaction WITH(NOLOCK) WHERE PayoutReferenceNo = @newPayoutRefNo)        
   goto gn;   
          
  -----------------INSERT TRANSACTION DATA-------------------------------        
  INSERT INTO [dbo].[tbl_remit_payout_transaction]        
  (        
   PayoutReferenceNo,RemitTransactionId,PartnerCode,TransactionType,PaymentType,BankName,BankCode,Branch,AccountHolderName,AccountNumber  
   ,WalletName,WalletCode,WalletNumber,WalletHolderName  
   ,AgentCode,StatusCode,ReasonCode,IpAddress,DeviceId  
   ,CreatedUserType,CreatedById,CreatedByName,CreatedDate,SenderCreatedDate,ReceiverCreatedDate      
  )        
  VALUES        
  (        
   @newPayoutRefNo,@RemitTransactionId,@PartnerCode,@TransactionType,@PaymentType,@BankName,@BankCode,@Branch,@AccountHolderName,@AccountNumber  
   ,@WalletName,@WalletCode,@WalletNumber,@WalletHolderName  
   ,@AgentCode  
   ,'51','R1007',@IpAddress,@DeviceId        
   ,@UserType,@LoggedInUserId,@LoggedInUser,GETUTCDATE(), [dbo].[func_convert_gmt_to_local_date](GETUTCDATE(),@PartnerGMTTimeZone),[dbo].[func_convert_gmt_to_local_date](GETUTCDATE(),'UTC+5:45')       
  )    
    
            
  SET NOCOUNT OFF;    
    
  SELECT @newPayoutRefNo AS PayoutReferenceNo, @RemitTransactionId AS RemitTransactionId,@PaymentType AS PaymentType, @NetRecievingAmountNPR AS PayableAmount, @WalletNumber AS WalletNumber, @WalletName AS WalletName, @WalletHolderName AS WalletHolderName 
 
  ,@BankCode AS BankCode, @BankName AS BankName, @AccountNumber AS BankAccountNumber, @AccountHolderName AS BankAccountHolderName;  
            
  SET @ReturnPrimaryId = 111 --(SELECT TransactionId FROM @TblTransaction)        
  SET @StatusCode = 200            
  SET @MsgType = 'Success'            
  SET @MsgText = 'Transaction Initiated Successfully!'          
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