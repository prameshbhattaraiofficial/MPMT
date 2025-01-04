CREATE OR ALTER   PROCEDURE [dbo].[usp_prefund_request_status_update]     
(    
  @FundRequestId int = 0     
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

	DECLARE @SourceCurrency varchar(3), @DestinationCurrency varchar(3), @PartnerCode varchar(20), @Sign varchar(20),@Amount decimal(18,2);
	DECLARE @CreatedByUserId int, @UpdatedByUserId int, @LoggedInUserId INT;

	IF(ISNULL(@UserType,'') = '')
		SET @UserType = 'ADMIN'

	SET @LoggedInUserId = (SELECT [dbo].[func_get_userid](@UserType, @LoggedInUser))

	SELECT @SourceCurrency=SourceCurrency, @DestinationCurrency=DestinationCurrency, @PartnerCode=PartnerCode, @Sign=Sign, @Amount=Amount
			,@CreatedByUserId=CreatedById, @UpdatedByUserId=UpdatedById FROM dbo.tbl_fund_request WITH(NOLOCK) WHERE Id=@FundRequestId;

	IF(ISNULL(@FundRequestId,0) <= 0)    
	BEGIN    
		SET @MsgText = 'Invalid FundRequestId!'    
		RETURN;    
	END
	IF(@LoggedInUserId IN (@CreatedByUserId, @UpdatedByUserId))
	BEGIN
		SET @MsgText = 'UnAuthorized User, same user do not have permission to APPROVE or REJECT the requested record!'    
		RETURN; 
	END
    
	IF(UPPER(@OperationMode) = 'APPROVE')    
	BEGIN		
    
		SET NOCOUNT ON;

		INSERT INTO dbo.tbl_partner_wallets_history
			SELECT * FROM dbo.tbl_partner_wallets WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency;

		UPDATE dbo.tbl_partner_wallets
		SET	
		Balance = CASE WHEN (@Sign='DR' OR @Sign='DEBIT') THEN Balance - @Amount 
							WHEN (@Sign='CR' OR @Sign='CREDIT') THEN Balance + @Amount END
		,UpdatedUserType = @UserType
		,UpdatedById = @LoggedInUserId
		,UpdatedByName = @LoggedInUser
		,UpdatedDate = GETUTCDATE()
		WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency AND IsDeleted=0;

		INSERT INTO dbo.tbl_fund_request_approved
		(Id,PartnerCode,FundTypeId,SourceCurrency,DestinationCurrency,Amount,Sign,TransactionId,VoucherImgPath,Remarks,RequestStatusId,maker,checker
		 ,CreatedUserType,CreatedById,CreatedByName,CreatedDate,UpdatedUserType,UpdatedById,UpdatedByName,UpdatedDate)
		 SELECT Id,PartnerCode,FundTypeId,SourceCurrency,DestinationCurrency,Amount,Sign,TransactionId,VoucherImgPath,Remarks,2,maker,1
			,CreatedUserType,CreatedById,CreatedByName,CreatedDate,@UserType,@LoggedInUserId,@LoggedInUser,GETUTCDATE() 
		FROM dbo.tbl_fund_request WHERE Id=@FundRequestId;
		
		DELETE dbo.tbl_fund_request WHERE Id=@FundRequestId;

		SET NOCOUNT OFF;
    
		SET @ReturnPrimaryId = @FundRequestId
		SET @StatusCode = 200    
		SET @MsgType = 'Success'    
		SET @MsgText = 'Requested Fund APPROVED Successfully'    
	END    
	ELSE IF(UPPER(@OperationMode) = 'REJECT')    
	BEGIN    
				
		SET NOCOUNT ON;

		UPDATE dbo.tbl_fund_request
		SET	
		RequestStatusId = 3
		,Checker=1
		,UpdatedUserType = @UserType
		,UpdatedById = @LoggedInUserId
		,UpdatedByName = @LoggedInUser
		,UpdatedDate = GETUTCDATE()
		WHERE Id = @FundRequestId;

		SET NOCOUNT OFF;

		SET @ReturnPrimaryId = @FundRequestId    
		SET @StatusCode=200    
		SET @MsgType = 'Success'    
		SET @MsgText = 'Requested Fund REJECTED Successfully' 
	END    
	
END 