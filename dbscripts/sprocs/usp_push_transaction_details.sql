
/*      
----------------------------------------------      
--EXECUTE STORED PROCEDURE:      
----------------------------------------------      
 declare @ReturnPrimaryId VARCHAR(50) = NULL             
 declare @StatusCode INT = NULL             
 declare @MsgType NVARCHAR(10) = NULL             
 declare @MsgText NVARCHAR(200) = NULL       
 exec [dbo].[usp_push_transaction_details] 'REM5468670119','AUD','NPR',100.00,4      
   ,@ReturnPrimaryId output,@StatusCode output,@MsgType output,@MsgText output      
 select @ReturnPrimaryId AS ReturnPrimaryId,@StatusCode as StatusCode,@MsgType as MsgType,@MsgText as MsgText      
      
*/      
CREATE OR ALTER   PROCEDURE [dbo].[usp_push_transaction_details]           
(          
 @PartnerCode varchar(20) = NULL      
 ,@SourceCurrency varchar(3) = NULL        
 ,@DestinationCurrency varchar(3) = NULL --DESTINATION CURRENCY IN NPR       
 ,@SendingAmount decimal(18,2) = null  --SENDING AMOUNT IN SOURCE CURRENCY      
 ,@PaymentType varchar(50) = null     --BANK,WALLET OR CASH      
 ,@ServiceCharge decimal(18,2) = null  ----SERVICE CHARGE IN SOURCE CURRENCY      
 ,@NetSendingAmount decimal(18,2) = null  ----NET SENDING AMOUNT IN SOURCE CURRENCY AFTER DEDUCTION OF SERVICE CHARGE      
 ,@ConversionRate decimal(18,4) = null  ----CURRENT CONVERSION RATE (UNIT CONVERSION RATE)      
 ,@NetRecievingAmountNPR decimal(18,2) = null ----NET RECIEVING AMOUNT IN NPR (THIS AMOUNT RECIPIENT WILL GET IN NPR)      
 ,@PartnerServiceCharge decimal(18,2) = null  ----PARTNER SERVICE CHARGE IN SOURCE CURRENCY LIABLE TO PAY TO MYPAY REMIT      
 ,@TransactionType varchar(50) = null  ----WALLETLOAD/WALLETCONVERSION,COMMISSION,APITRANSACTION,PARTNERTRANSACTION      
      
 ,@ExistingSender bit = 0      
 ,@MemberId varchar(20) = null      
 ,@SenderFirstName varchar(100) = null      
 ,@NoSenderFirstName bit = null      
 ,@SenderLastName varchar(100) = null      
 --,@IsSenderSurnamePresent bit = null      
 ,@SenderContactNumber varchar(30) = null      
 ,@SenderEmail varchar(150) = null      
 ,@SenderCountryCode varchar(10) = null      
 ,@SenderProvince nvarchar(150) = null      
 ,@SenderCity nvarchar(150) = null      
 ,@SenderZipcode nvarchar(50) = null      
 ,@SenderAddress nvarchar(300) = null      
 ,@SenderRelationshipId int = null      
 ,@SenderIdProofImgPath1 nvarchar(500) = null      
 ,@SenderIdProofImgPath2 nvarchar(500) = null      
 ,@SenderPurposeId int = NULL        
 ,@SenderRemarks nvarchar(300) = NULL      
      
 ,@ExistingRecipient bit = 0      
 ,@RecipientId varchar(20) = null      
 ,@RecipientType varchar(50) = null   ----someone/joint/charity    
 ,@RecipientFirstName varchar(100) = null      
 ,@NoRecipientFirstName bit = null      
 ,@RecipientLastName varchar(100) = null      
 --,@IsRecipientSurnamePresent bit = null      
 ,@JointAccountFirstName varchar(100) = null      
 ,@NoJointAccountFirstName bit = null      
 ,@JointAccountLastName varchar(100) = null      
 --,@IsJointAccountSurnamePresent bit = null      
 ,@BusinessName varchar(200) = null      
 ,@RecipientContactNumber varchar(30) = null      
 ,@RecipientEmail varchar(150) = null      
 ,@RecipientDateOfBirth datetime = null      
 ,@RecipientCountryCode varchar(10) = null      
 ,@RecipientProvinceCode varchar(10) = null      
 ,@RecipientDistrictCode varchar(10) = null      
 ,@RecipientLocalBodyCode varchar(10) = null      
 ,@RecipientCity varchar(150) = null      
 ,@RecipientZipcode varchar(50) = null      
 ,@RecipientAddress varchar(300) = null      
 ,@RecipientRelationshipId int = null      
      
 --,@BankName varchar(200) = null      
 ,@BankCode varchar(100) = null      
 ,@Branch varchar(200) = null      
 ,@AccountHolderName varchar(200) = null      
 ,@AccountNumber varchar(50) = null      
 --,@WalletName varchar(200) = null      
 ,@WalletCode varchar(50) = null      
 ,@WalletNumber varchar(50) = null      
 ,@WalletHolderName varchar(200) = null      
      
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
 DECLARE @TblTransaction TABLE (TransactionId varchar(50));       
 DECLARE @LoggedInUserId INT;      
      
 IF(ISNULL(@UserType,'') = '')      
  SET @UserType = 'PARTNER'      
      
 SET @LoggedInUserId = (SELECT [dbo].[func_get_userid](@UserType, @LoggedInUser))      
      
 ----------------------------------START: SETTING DETAILS---------------------------------------------------------      
 declare @creditSendingAmount decimal(18,4), @BankName varchar(200), @WalletName varchar(200);      
 declare @partnerName varchar(200),@partnerCountryCode varchar(10),@chargeCategory varchar(50),@fundType varchar(50),@creditUptoLimitPerc decimal(18,2), @creditSendTxnLimitNPR decimal(18,2),@cashPayoutSendTxnLimitNPR decimal(18,2)      
  ,@walletSendTxnLimitNPR decimal(18,2), @bankSendTxnLimitNPR decimal(18,2), @walletBalance decimal(18,4),@transactionApproval bit,@paymentTypeId int;      
     
 declare @tmpSetting as table(partnerName varchar(200),partnerCountryCode varchar(10),chargeCategory varchar(50), fundType varchar(50), creditUptoLimitPerc decimal(18,2), creditSendTxnLimitNPR decimal(18,2)    
  ,cashPayoutSendTxnLimitNPR decimal(18,2),walletSendTxnLimitNPR decimal(18,2), bankSendTxnLimitNPR decimal(18,2), walletBalance decimal(18,4), transactionApproval bit, paymentTypeId int)      
      
 insert into @tmpSetting      
 select TRIM(ISNULL(p.OrganizationName,'')),p.CountryCode, c.CategoryCode,f.FundTypeCode,ISNULL(p.CreditUptoLimitPerc,0),ISNULL(p.CreditSendTxnLimit,0),ISNULL(p.CashPayoutSendTxnLimit,0)      
 ,ISNULL(WalletSendTxnLimit,0),ISNULL(BankSendTxnLimit,0),ISNULL(w.Balance,0),ISNULL(p.TransactionApproval,0), pt.Id      
 from dbo.tbl_remit_partners p with(nolock)      
 INNER JOIN dbo.tbl_partner_wallets w with(nolock) on w.PartnerCode=p.PartnerCode AND w.IsActive=1 AND ISNULL(w.IsDeleted,0)=0 AND p.IsActive=1 AND ISNULL(p.IsDeleted,0)=0      
 INNER JOIN dbo.tbl_service_charge_category c with(nolock) on c.Id=p.ChargeCategoryId AND c.IsActive=1 and ISNULL(c.IsDeleted,0)=0      
 LEFT JOIN dbo.tbl_fund_type f with(nolock) ON f.Id = (CASE WHEN ISNULL(p.FundTypeId,'') = '' THEN (SELECT Id FROM dbo.tbl_fund_type with(nolock) where FundTypeCode='PREFUNDING') ELSE p.FundTypeId END) and f.IsActive=1 AND ISNULL(f.IsDeleted,0)=0      
 LEFT JOIN dbo.tbl_payment_type pt with(nolock) ON 1=1 AND pt.PaymentTypeCode=@PaymentType AND pt.IsActive=1 AND ISNULL(pt.IsDeleted,0)=0      
 where p.PartnerCode=@PartnerCode and w.SourceCurrency=@SourceCurrency and w.DestinationCurrency=@DestinationCurrency;      
      
 select       
  @partnerName=partnerName,@partnerCountryCode=partnerCountryCode, @chargeCategory=chargeCategory,@fundType=fundType,@creditUptoLimitPerc=creditUptoLimitPerc,@creditSendTxnLimitNPR=creditSendTxnLimitNPR,@cashPayoutSendTxnLimitNPR=cashPayoutSendTxnLimitNPR      
 ,@walletSendTxnLimitNPR=walletSendTxnLimitNPR,@bankSendTxnLimitNPR=bankSendTxnLimitNPR,@walletBalance=walletBalance,@transactionApproval=transactionApproval,@paymentTypeId=paymentTypeId     
 from @tmpSetting;      
      
 ----------------------------------END: SETTING DETAILS---------------------------------------------------------      
      
      
 ----------------------------------START: DATA VALIDATION---------------------------------------------------------      
       
 IF(TRIM(ISNULL(@PartnerCode,'')) = '')          
 BEGIN          
  SET @MsgText = 'PartnerCode can not be empty!'          
  RETURN;          
 END      
 IF NOT EXISTS(SELECT 1 FROM tbl_remit_partners WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND IsActive=1 AND ISNULL(IsDeleted,0)=0)          
 BEGIN          
  SET @MsgText = 'Invalid PartnerCode!'          
  RETURN;          
 END      
 IF(TRIM(ISNULL(@SourceCurrency,'')) = '')          
 BEGIN          
  SET @MsgText = 'SourceCurrency can not be empty!'          
  RETURN;          
 END      
 IF(TRIM(ISNULL(@DestinationCurrency,'')) = '')          
 BEGIN          
  SET @MsgText = 'DestinationCurrency can not be empty!'          
  RETURN;          
 END      
 IF(TRIM(ISNULL(@DestinationCurrency,'')) <> 'NPR')          
 BEGIN          
  SET @MsgText = 'DestinationCurrency should be NPR!'          
  RETURN;          
 END      
 IF(ISNULL(@SendingAmount,0) <= 0)          
 BEGIN          
  SET @MsgText = 'Invalid Sending Amount!'          
  RETURN;          
 END      
 IF(ISNULL(@PaymentType,'') NOT IN ('BANK','WALLET','CASH'))          
 BEGIN          
  SET @MsgText = 'Invalid Payout Type!'          
  RETURN;          
 END      
 IF(ISNULL(@ServiceCharge,0) < 0)          
 BEGIN          
  SET @MsgText = 'Invalid Service charge!'          
  RETURN;          
 END      
 IF(ISNULL(@NetSendingAmount,0) <= 0)          
 BEGIN          
  SET @MsgText = 'Invalid Net Sending Amount!'          
  RETURN;          
 END      
 IF(ISNULL(@ConversionRate,0) <= 0)          
 BEGIN          
  SET @MsgText = 'Invalid Conversion Rate!'          
  RETURN;          
 END      
 IF(ISNULL(@NetRecievingAmountNPR,0) <= 0)          
 BEGIN          
  SET @MsgText = 'Invalid Recieving NPR Amount!'          
  RETURN;          
 END      
 IF(ISNULL(@PartnerServiceCharge,0) < 0)          
 BEGIN          
  SET @MsgText = 'Invalid Partner Service Charge!'          
  RETURN;          
 END     
       
 --------------SENDER VALIDATION------------------------      
 IF(ISNULL(@ExistingSender,0) = 1)      
 BEGIN      
  IF(ISNULL(@MemberId,'') = '')      
  BEGIN      
   SET @MsgText = 'Sender MemberId should not be empty!'          
   RETURN;      
  END      
  IF NOT EXISTS(SELECT 1 FROM dbo.tbl_senders WITH(NOLOCK) WHERE MemberId = @MemberId AND IsActive=1 AND ISNULL(IsDeleted,0)=0)      
  BEGIN      
   SET @MsgText = 'Invalid Sender MemberId!'          
   RETURN;      
  END      
 END      
 ELSE      
 BEGIN      
  IF(ISNULL(@NoSenderFirstName,0) = 0)      
  BEGIN      
   IF(ISNULL(@SenderFirstName,'') = '')      
   BEGIN      
    SET @MsgText = 'Provide Sender Firstname!'          
    RETURN;      
   END      
  END      
  IF(ISNULL(@SenderLastName,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Sender Lastname!'          
   RETURN;      
  END      
  IF(ISNULL(@SenderContactNumber,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Sender Contact number!'          
   RETURN;      
  END      
  IF(ISNULL(@SenderCountryCode,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Sender Country!'          
   RETURN;      
  END      
  IF(ISNULL(@SenderCountryCode,'') = 'NP')      
  BEGIN      
   SET @MsgText = 'Invalid Country. Please select Sender Country other than Nepal!'          
   RETURN;      
  END      
  IF(ISNULL(@SenderCity,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Sender City!'          
   RETURN;      
  END      
  IF(ISNULL(@SenderAddress,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Sender Full Address!'          
   RETURN;      
  END      
  IF(ISNULL(@SenderRelationshipId,0) <= 0)      
  BEGIN      
   SET @MsgText = 'Provide Sender Relationship with Recipient!'          
   RETURN;      
  END      
  IF(ISNULL(@SenderPurposeId, 0) <= 0)      
  BEGIN      
   SET @MsgText = 'Select Transaction Purpose!'          
   RETURN;      
  END      
  IF EXISTS(SELECT 1 FROM dbo.tbl_transfer_purpose WITH(NOLOCK) WHERE Id=@SenderPurposeId AND PurposeName like '%OTHER%')      
  BEGIN      
   IF(ISNULL(@SenderRemarks,'') = '')      
   BEGIN      
    SET @MsgText = 'Provide Purpose Remarks!'          
    RETURN;      
   END      
  END      
 END      
       
 --------------END: SENDER VALIDATION------------------------      
      
 --------------RECIPIENT VALIDATION------------------------      
 IF(ISNULL(@ExistingRecipient,0) = 1)      
 BEGIN      
  IF(ISNULL(@RecipientId,'') = '')      
  BEGIN      
   SET @MsgText = 'RecipientId should not be empty!'          
   RETURN;      
  END      
  IF NOT EXISTS(SELECT 1 FROM dbo.tbl_recipients WITH(NOLOCK) WHERE RecipientId = @RecipientId AND IsActive=1 AND ISNULL(IsDeleted,0)=0)      
  BEGIN      
   SET @MsgText = 'Invalid RecipientId!'          
   RETURN;      
  END      
 END      
 ELSE      
 BEGIN      
  declare @recipientTypeId int;      
  IF(ISNULL(@RecipientType,'') = '')      
  BEGIN      
   SET @MsgText = 'Please select Recipient Type!'          
   RETURN;      
  END      
  IF NOT EXISTS(SELECT 1 FROM dbo.tbl_recipient_type WITH(NOLOCK) WHERE LookupName = @RecipientType AND LookupName IN ('someone','joint','charity') AND IsActive=1 AND ISNULL(IsDeleted,0)=0)      
  BEGIN      
   SET @MsgText = 'Invalid Recipient Type!'          
   RETURN;      
  END      
  SET @recipientTypeId = (SELECT Id FROM dbo.tbl_recipient_type WITH(NOLOCK) WHERE LookupName=@RecipientType  AND IsActive=1 AND ISNULL(IsDeleted,0)=0)      
      
  IF(@RecipientType='someone')      
  BEGIN      
   IF(ISNULL(@NoRecipientFirstName,0) = 0)      
   BEGIN      
    IF(ISNULL(@RecipientFirstName,'') = '')      
    BEGIN      
     SET @MsgText = 'Provide Recipient Firstname!'          
     RETURN;      
    END      
   END      
   IF(ISNULL(@RecipientLastName,'') = '')      
   BEGIN      
    SET @MsgText = 'Provide Recipient Lastname!'          
    RETURN;      
   END      
  END      
  ELSE IF(@RecipientType='joint')      
  BEGIN      
   IF(ISNULL(@NoJointAccountFirstName,0) = 0)      
   BEGIN      
    IF(ISNULL(@JointAccountFirstName,'') = '')      
    BEGIN      
     SET @MsgText = 'Provide Recipient Joint Account Firstname!'          
     RETURN;      
    END      
   END      
   IF(ISNULL(@JointAccountLastName,'') = '')      
   BEGIN      
    SET @MsgText = 'Provide Recipient Joint Account Lastname!'          
    RETURN;      
   END      
  END      
  ELSE IF(@RecipientType='charity')      
  BEGIN      
   IF(ISNULL(@BusinessName,'') = '')      
   BEGIN      
    SET @MsgText = 'Provide Recipient Business name!'          
    RETURN;      
   END      
  END      
      
  IF(ISNULL(@RecipientContactNumber,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Recipient Contact number!'          
   RETURN;      
  END      
  IF(ISNULL(@RecipientCountryCode,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Recipient Country!'          
   RETURN;      
  END      
  IF(ISNULL(@RecipientCountryCode,'') <> 'NP')      
  BEGIN      
   SET @MsgText = 'Invalid Country. Please select Nepal as Destination Country!'          
   RETURN;      
  END      
  IF(ISNULL(@RecipientProvinceCode,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Recipient Province!'          
   RETURN;      
  END      
  IF(ISNULL(@RecipientDistrictCode,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Recipient District!'          
   RETURN;      
  END      
  --IF(ISNULL(@RecipientLocalBodyCode,'') = '')      
  --BEGIN      
  -- SET @MsgText = 'Provide Recipient LocalBody!'          
  -- RETURN;      
  --END      
  IF(ISNULL(@RecipientAddress,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Recipient Full Address!'          
   RETURN;      
  END      
  IF(ISNULL(@RecipientRelationshipId,0) <= 0)      
  BEGIN      
   SET @MsgText = 'Provide Recipient Relationship with Sender!'          
   RETURN;      
  END      
        
 END      
 --------------END: RECIPIENT VALIDATION------------------------      
      
 --------------START: BANK DETAIL VALIDATION------------------------      
 IF(@PaymentType='BANK')      
 BEGIN       
  IF(ISNULL(@BankCode,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Recipient Bank Code!'          
   RETURN;      
  END      
    
  SET @BankName = (SELECT BankName FROM dbo.tbl_banks WITH(NOLOCK) WHERE BankCode=@BankCode AND IsActive=1 AND ISNULL(IsDeleted,0) = 0);    
    
  IF(ISNULL(@AccountHolderName,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Recipient Account holder name!'          
   RETURN;      
  END      
 IF(ISNULL(@AccountHolderName,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Recipient Account number!'          
   RETURN;      
  END      
 END      
 ELSE IF(@PaymentType='WALLET')      
 BEGIN      
  IF(ISNULL(@WalletCode,'') = '')      
  BEGIN      
   SET @MsgText = 'Select Wallet name!'          
   RETURN;      
  END      
    
  SET @WalletName = (SELECT WalletName FROM dbo.tbl_wallets WITH(NOLOCK) WHERE WalletCode=@WalletCode AND IsActive=1 AND ISNULL(IsDeleted,0) = 0);    
    
  IF(ISNULL(@WalletNumber,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Wallet Number (Registered Mobile number)!'          
   RETURN;      
  END      
  IF(ISNULL(@WalletHolderName,'') = '')      
  BEGIN      
   SET @MsgText = 'Provide Wallet holder name!'          
   RETURN;      
  END      
 END      
 --------------END: BANK DETAIL VALIDATION------------------------     
     
 ------------START: WALLET BALANCE AND SENDING AMOUNT VALIDATION--------------------    
 SET @creditSendingAmount = 0;      
 IF(@fundType='PREFUNDING')      
 BEGIN      
  IF(@walletBalance < @SendingAmount)      
  BEGIN      
   DECLARE @calculatedWalletBal DECIMAL(18,4);      
   SET @creditSendingAmount = ((@SendingAmount * @creditUptoLimitPerc)/100);      
   SET @calculatedWalletBal = @walletBalance + @creditSendingAmount;      
   IF(@calculatedWalletBal < @SendingAmount)      
   BEGIN      
    SET @MsgText = 'Insufficient Wallet Balance!'          
    RETURN;      
   END      
  END        
 END      
 ELSE IF(@fundType='CONSUMPTION')      
 BEGIN      
 -------------WALLET BALANCE GOES NEGATIVE IN CONSUMPTION, HENCE SUM OF CREDITSENDTXNLIMT     
 -------------AND NEGATIVE WALLET BALANCE SHOULD BE GREAER THAN RECEIVING AMOUNT --------------    
  IF((@creditSendTxnLimitNPR + @walletBalance) < @NetRecievingAmountNPR)      
  BEGIN      
   SET @MsgText = 'Insufficient Credit Balance. Please load fund in your respective wallet!'          
   RETURN;      
  END    
    
 END      
 ELSE      
 BEGIN      
  IF(@walletBalance < @SendingAmount)      
  BEGIN      
   SET @MsgText = 'Insufficient Balance!'          
   RETURN;      
  END      
 END      
      
 IF(@PaymentType='BANK')      
 BEGIN      
  IF(@bankSendTxnLimitNPR < @NetRecievingAmountNPR)      
  BEGIN      
   SET @MsgText = 'Net Recieving Amount exceeded Total Bank Transfer Amount allowed for the Partner!'          
   RETURN;     
  END      
 END      
 ELSE IF(@PaymentType='WALLET')      
 BEGIN      
  IF(@walletSendTxnLimitNPR < @NetRecievingAmountNPR)      
  BEGIN      
   SET @MsgText = 'Net Recieving Amount exceeded Total Wallet Transfer Amount allowed for the Partner!'          
   RETURN;      
  END      
 END      
 ELSE IF(@PaymentType='CASH')      
 BEGIN      
  IF(@cashPayoutSendTxnLimitNPR < @NetRecievingAmountNPR)      
  BEGIN      
   SET @MsgText = 'Net Recieving Amount exceeded Total Cash Transfer Amount allowed for the Partner!'          
   RETURN;      
  END      
 END     
 ------------END: WALLET BALANCE AND SENDING AMOUNT VALIDATION--------------------    
    
        
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
  DECLARE @newTransactionId NVARCHAR(100), @newTokenNumber NVARCHAR(50);      
  gn: EXEC usp_generate_random_numeric_string 25, @newTransactionId OUTPUT      
  IF EXISTS(SELECT 1 FROM dbo.tbl_remit_transaction WITH(NOLOCK) WHERE TransactionId = @newTransactionId)      
   goto gn;      
       
  tk: EXEC usp_generate_random_alphanumeric_string 16, @newTokenNumber OUTPUT      
  IF EXISTS(SELECT 1 FROM dbo.tbl_remit_transaction WITH(NOLOCK) WHERE TokenNumber = @newTokenNumber)      
   goto tk;      
        
  -----------------INSERT TRANSACTION DATA-------------------------------      
  INSERT INTO [dbo].[tbl_remit_transaction]      
  (      
   TransactionId,PartnerCode,PartnerName,PartnerCountryCode,SourceCurrency,DestinationCurrency,SendingAmount,ServiceCharge,ConversionRate,NetSendingAmount,NetReceivingAmount,CreditSendingAmount      
   ,SenderRegistered,MemberId,RecipientRegistered,RecipientId,Sign,PartnerServiceCharge,TransactionType,WalletType      
   ,CurrentBalance,PreviousBalance,      
   TokenNumber,PaymentTypeId,PaymentType,BankName,BankCode,Branch,AccountHolderName,AccountNumber,WalletName,WalletCode,WalletNumber,WalletHolderName,TransactionApproval,      
   PartnerTransactionId,PartnerTrackerId,PartnerStatus,AgentCode,AgentTransactionId,AgentTrackerId,AgentStatus,GatewayTxnId,GatewayStatus,ComplianceStatusCode      
   ,ComplianceStatus,StatusCode,IpAddress,DeviceId      
   ,CreatedUserType,CreatedById,CreatedByName,CreatedDate      
  )      
  OUTPUT inserted.TransactionId INTO @TblTransaction      
  VALUES      
  (      
   @newTransactionId,@PartnerCode,@partnerName,@partnerCountryCode,@SourceCurrency,@DestinationCurrency,@SendingAmount,@ServiceCharge,@ConversionRate,@NetSendingAmount,@NetRecievingAmountNPR,@creditSendingAmount      
   ,@ExistingSender,@MemberId,@ExistingRecipient,@RecipientId,'DR',@PartnerServiceCharge,@TransactionType,@fundType      
   ,((@walletBalance+@creditSendingAmount)-(@NetSendingAmount+@PartnerServiceCharge)),@walletBalance      
   ,@newTokenNumber,@paymentTypeId,@PaymentType,@BankName,@BankCode,@Branch,@AccountHolderName,@AccountNumber,@WalletName,@WalletCode,@WalletNumber,@WalletHolderName,ISNULL(@transactionApproval,0)      
   ,'','','','','','','50','','','','','51',@IpAddress,@DeviceId      
   ,@UserType,@LoggedInUserId,@LoggedInUser,GETUTCDATE()      
  )      
      
  -----------------INSERT TRANSACTION SENDER DATA-------------------------------      
  INSERT INTO dbo.tbl_transaction_senders      
  (TransactionId,FirstName,NoFirstName,LastName,ContactNumber,Email,CountryCode,Province,City,Zipcode      
  ,Address,RelationshipId,IdProofImgPath1,IdProofImgPath2,PurposeId,Remarks,CreatedDate)      
  VALUES      
  (@newTransactionId,@SenderFirstName,@NoSenderFirstName,@SenderLastName,@SenderContactNumber,@SenderEmail,@SenderCountryCode,@SenderProvince,@SenderCity,@SenderZipcode      
  ,@SenderAddress,@SenderRelationshipId,@SenderIdProofImgPath1,@SenderIdProofImgPath2,@SenderPurposeId,@SenderRemarks,GETUTCDATE())      
      
  -----------------INSERT TRANSACTION RECIPIENT DATA-------------------------------      
  INSERT INTO dbo.tbl_transaction_recipients      
  (TransactionId,RecipientTypeId,FirstName,NoFirstName,LastName,FirstNameJointAcct,NoFirstNameJointAcct,LastNameJointAcct,BusinessName      
  ,ContactNumber,Email,CountryCode,ProvinceCode,DistrictCode,LocalBodyCode,City,Zipcode      
  ,Address,RelationshipId,CreatedDate)      
  VALUES      
  (@newTransactionId,@recipientTypeId,@RecipientFirstName,@NoRecipientFirstName,@RecipientLastName,@JointAccountFirstName,@NoJointAccountFirstName,@JointAccountLastName,@BusinessName      
  ,@RecipientContactNumber,@RecipientEmail,@RecipientCountryCode,@RecipientProvinceCode,@RecipientDistrictCode,@RecipientLocalBodyCode,@RecipientCity,@RecipientZipcode      
  ,@RecipientAddress,@RecipientRelationshipId,GETUTCDATE())      
        
  -------------------DEDUCT BALANCE FROM WALLET AFTER SENDING MONEY-----------------------------      
  INSERT INTO dbo.tbl_partner_wallets_history SELECT * FROM dbo.tbl_partner_wallets WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency;      
  UPDATE dbo.tbl_partner_wallets SET Balance = (Balance + @creditSendingAmount) - (@NetSendingAmount + @PartnerServiceCharge) WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency;      
      
  -------------------DEDUCT CREDIT SEND LIMIT OF PARTNER IF WALLET TYPE IS CONSUMPTION-----------------------------      
  --IF(@fundType='CONSUMPTION')      
  --BEGIN      
  -- UPDATE dbo.tbl_remit_partners SET @creditSendTxnLimitNPR = @creditSendTxnLimitNPR - @NetRecievingAmountNPR WHERE PartnerCode=@PartnerCode;      
  --END      
  --ELSE      
  --BEGIN      
  -- -------------------DEDUCT BALANCE FROM WALLET AFTER SENDING MONEY-----------------------------      
  --INSERT INTO dbo.tbl_partner_wallets_history SELECT * FROM dbo.tbl_partner_wallets WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency;      
  --UPDATE dbo.tbl_partner_wallets SET Balance = (Balance + @creditSendingAmount) - (@NetSendingAmount + @PartnerServiceCharge) WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency;      
      
  --END      
          
  SET NOCOUNT OFF;      
          
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