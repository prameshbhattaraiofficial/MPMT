CREATE OR ALTER PROCEDURE [dbo].[usp_prefund_request_addupdate]     
(    
  @Id int = 0     
 ,@WalletId int = NULL
 ,@FundTypeId int = NULL	----Prefunding, other fund (from tbl_fund_type)
 ,@Sign varchar(20) = NULL		----DR=>Debit, CR=>Credit
 ,@Amount decimal(18,2) = NULL
 ,@TxnId nvarchar(50) = NULL
 ,@Remarks nvarchar(300) = NULL
 ,@VoucherImagePath nvarchar(500) = NULL
 ,@OperationMode nvarchar(10) = NULL    
 ,@LoggedInUser [nvarchar](100) = NULL    
 ,@UserType [varchar](20) = NULL		----USERTYPE(ADMIN, PARTNER, AGENT)
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

	DECLARE @TblFundRequest TABLE (Id INT); 
	DECLARE @SourceCurrency varchar(3), @DestinationCurrency varchar(3), @PartnerCode varchar(20),@LoggedInUserId INT;

	IF(ISNULL(@UserType,'') = '')
		SET @UserType = 'ADMIN'

	SET @LoggedInUserId = (SELECT [dbo].[func_get_userid](@UserType, @LoggedInUser))

	IF(UPPER(@OperationMode) IN ('A','U'))    
	BEGIN    
		
		IF(ISNULL(@FundTypeId,0) <= 0)    
		BEGIN    
			SET @MsgText = 'Invalid FundType!'    
			RETURN;    
		END
		IF(TRIM(ISNULL(@Sign,'')) = '')    
		BEGIN    
			SET @MsgText = 'Sign can not be empty!'    
			RETURN;    
		END
		IF(ISNULL(@Amount,0) <= 0)    
		BEGIN    
			SET @MsgText = 'Invalid Amount!'    
			RETURN;    
		END
		IF(ISNULL(@TxnId,'') = '')    
		BEGIN    
			SET @MsgText = 'TransactionId can not be empty!'    
			RETURN;    
		END
		IF(ISNULL(@VoucherImagePath,'') = '')    
		BEGIN    
			SET @MsgText = 'Voucher Image can not be empty!'    
			RETURN;    
		END
		
	END    
    
	IF(UPPER(@OperationMode) = 'A')    
	BEGIN
		
		SELECT @SourceCurrency=SourceCurrency, @DestinationCurrency=DestinationCurrency, @PartnerCode=PartnerCode 
			FROM dbo.tbl_partner_wallets WITH(NOLOCK) WHERE Id=@WalletId;

		--IF(TRIM(ISNULL(@PartnerCode,'')) = '')    
		--BEGIN    
		--	SET @MsgText = 'PartnerCode can not be empty!'    
		--	RETURN;    
		--END
		--IF NOT EXISTS(SELECT 1 FROM tbl_remit_partners WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND IsDeleted=0)    
		--BEGIN    
		--	SET @MsgText = 'Invalid PartnerCode!'    
		--	RETURN;    
		--END

		IF(ISNULL(@WalletId,0) <= 0)    
		BEGIN    
			SET @MsgText = 'Invalid WalletId!'    
			RETURN;    
		END
    
		SET NOCOUNT ON;

		INSERT INTO [dbo].[tbl_fund_request]
		(
			PartnerCode,FundTypeId,SourceCurrency,DestinationCurrency,Amount,Sign,TransactionId,VoucherImgPath,Remarks
			,RequestStatusId,Maker,Checker,CreatedUserType, CreatedById,CreatedByName,CreatedDate
		)
		OUTPUT inserted.Id INTO @TblFundRequest
		VALUES
		(
			@PartnerCode,@FundTypeId,@SourceCurrency,@DestinationCurrency,@Amount,@Sign,@TxnId,@VoucherImagePath,@Remarks
			,1,1,0,@UserType,@LoggedInUserId,@LoggedInUser,GETUTCDATE()
		)
				
		SET NOCOUNT OFF;
    
		SET @ReturnPrimaryId = (SELECT Id FROM @TblFundRequest)
		SET @StatusCode = 200    
		SET @MsgType = 'Success'    
		SET @MsgText = 'Prefund temporarily Added Successfully'    
	END    
	ELSE IF(UPPER(@OperationMode) = 'U')    
	BEGIN    
		IF(ISNULL(@Id, 0) <= 0)    
		BEGIN    
			SET @MsgText = 'Invalid Id, Id can not be less than or equal to zero'    
			RETURN;    
		END   
		
		SET NOCOUNT ON;

		UPDATE [dbo].[tbl_fund_request]
		SET
		FundTypeId = @FundTypeId
		,Amount = @Amount
		,Sign	= @Sign
		,TransactionId = @TxnId
		,VoucherImgPath = @VoucherImagePath
		,Remarks	= @Remarks
		,RequestStatusId = 1
		,Maker	= 1
		,Checker = 0
		,UpdatedUserType = @UserType
		,UpdatedById = @LoggedInUserId
		,UpdatedByName = @LoggedInUser
		,UpdatedDate = GETUTCDATE()
		WHERE Id = @Id;

		SET NOCOUNT OFF;

		SET @ReturnPrimaryId = @Id    
		SET @StatusCode=200    
		SET @MsgType = 'Success'    
		SET @MsgText = 'Requested Fund Updated Successfully' 
	END    
	
END 