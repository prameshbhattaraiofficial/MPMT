CREATE OR ALTER  procedure [dbo].[usp_validate_wallet_user_log]  
(  
@UniqueRefereneNo varchar(100) = null  
,@RemitTransactionId varchar(100) = null  
,@AgentCode varchar(50) = null  
,@AccountStatus varchar(50) = null  ----ACTIVE/INACTIVE  
--,@FullName varchar(200) = null  
,@ContactNumber varchar(30) = null  
,@IsAccountValidated bit = null  
,@Message varchar(100) = null  ----SUCCESS/ERROR  
,@ResponseMessage varchar(300) = null  
--,@Details varchar(500) = null  
,@ResponseCode varchar(50) = null  
,@Status bit = null  
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
  
 DECLARE @LoggedInUserId INT, @PartnerGMTTimeZone varchar(20),@PartnerCode varchar(50);  
  
DECLARE @ErrorNumber INT         
DECLARE @ErrorMessage NVARCHAR(4000)        
DECLARE @ErrorSeverity INT        
DECLARE @ErrorState INT        
DECLARE @ErrorProcedure NVARCHAR(256)        
DECLARE @ErrorLine INT      
  
IF(ISNULL(@AgentCode,'') = '')    
SET @AgentCode = (SELECT AgentCode FROM dbo.tbl_remit_agents WITH(NOLOCK) WHERE LookupName='MYPAY' AND IsActive=1 AND ISNULL(IsDeleted,0)=0)  
  
SET @PartnerCode = (SELECT PartnerCode FROM dbo.tbl_remit_transaction WITH(NOLOCK) WHERE TransactionId = @RemitTransactionId);  
  
IF(ISNULL(@UserType,'') = '')        
  SET @UserType = 'PARTNER'        
        
 SET @LoggedInUserId = (SELECT [dbo].[func_get_userid](@UserType, @LoggedInUser))    
 SET @PartnerGMTTimeZone = (SELECT ISNULL(GMTTimeZone,'UTC+5:45') FROM dbo.tbl_remit_partners WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND IsActive=1 AND ISNULL(IsDeleted,0)=0);  
  
        
BEGIN TRY          
BEGIN TRANSACTION            
SET NOCOUNT ON;   
  
INSERT INTO dbo.tbl_remit_transaction_history  
 SELECT * FROM dbo.tbl_remit_transaction WITH(NOLOCK) WHERE TransactionId = @RemitTransactionId;  
  
UPDATE dbo.tbl_remit_transaction   
SET   
AgentCode = @AgentCode  
--,FullName = @FullName  
,AccountStatus = @AccountStatus  
,ContactNumber = @ContactNumber  
,IsAccountValidated = ISNULL(@IsAccountValidated,0)  
,Message = @Message  
,ResponseMessage = @ResponseMessage  
--,Details = @Details  
,AgentResponseCode = @ResponseCode  
,AgentStatus = ISNULL(@Status,0)  
--,AgentStatusCode = (select StatusCode from dbo.tbl_agent_status with(nolock) where IsActive=1 and ISNULL(IsDeleted,0)=0 and AgentLookupName='MYPAY' and LookupName='NOTINITIATED')  
,ReasonCode = CASE WHEN @AccountStatus = 'Active' THEN 'R1004'  
     WHEN @AccountStatus = 'InActive' THEN 'R1001'  
     WHEN ISNULL(@IsAccountValidated,0) = 0 THEN 'R1002'  
     WHEN ISNULL(@IsAccountValidated,0) = 1 THEN 'R1005'  
    END  
,CreatedDate = GETUTCDATE()   
,SenderCreatedDate = [dbo].[func_convert_gmt_to_local_date](GETUTCDATE(),@PartnerGMTTimeZone)  
,ReceiverCreatedDate = [dbo].[func_convert_gmt_to_local_date](GETUTCDATE(),'UTC+5:45')  
WHERE TransactionId = @RemitTransactionId;  
  
SET NOCOUNT OFF;        
            
   SET @StatusCode = 200        
   SET @MsgType = 'Ok'        
   SET @MsgText = 'Success'     
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