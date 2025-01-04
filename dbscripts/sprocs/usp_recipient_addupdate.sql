CREATE OR ALTER PROCEDURE [dbo].[usp_recipient_addupdate]     
(    
  @Id int = 0   
  ,@SenderId int = NULL
 ,@FirstName nvarchar(100) = NULL
 ,@SurName nvarchar(100) = NULL
 ,@IsSurNamePresent bit = NULL
 ,@MobileNumber nvarchar(20) = NULL
 ,@Email nvarchar(150) = NULL
 ,@GenderId int = NULL
 ,@DateOfBirth datetime = NULL
 ,@CountryCode varchar(10) = NULL
 ,@ProvinceCode varchar(20) = NULL
 ,@DistrictCode varchar(20) = NULL
 ,@LocalBodyCode varchar(20) = NULL
 ,@City nvarchar(150) = NULL
 ,@Zipcode nvarchar(50) = NULL
 ,@Address nvarchar(300) = NULL
 --,@OccupationId int = NULL
 --,@DocumentTypeId int = NULL
 --,@DocumentNumber nvarchar(50) = NULL
 --,@IssuedDate datetime = NULL
 --,@ExpiryDate datetime = NULL
 --,@IdFrontImgPath nvarchar(500) = NULL
 --,@IdBackImgPath nvarchar(500) = NULL
 ,@SourceCurrency varchar(3) = NULL
 ,@DestinationCurrency varchar(3) = NULL
 ,@RelationshipId int = NULL
 ,@PayoutTypeId int = NULL
 ,@BankName nvarchar(200) = NULL
 ,@BankCode nvarchar(50) = NULL
 ,@Branch nvarchar(200) = NULL
 ,@AccountHolderName nvarchar(200) = NULL
 ,@AccountNumber nvarchar(50) = NULL
 --,@IncomeSourceId int = NULL
 ,@WalletName nvarchar(200) NULL
 ,@WalletId varchar(20) NULL
 ,@WalletRegisteredName nvarchar(150) NULL
 ,@GMTTimeZone nvarchar(50) = NULL
 ,@IsActive bit = NULL
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
	SET @StatusCode = 400    
	SET @MsgType = 'Error'    
	SET @MsgText = 'Bad Request'    
	SET @ReturnPrimaryId = 0   

	DECLARE @TblRecipient TABLE (Id INT); 
	DECLARE @LoggedInUserId INT;

	IF(ISNULL(@UserType,'') = '')
		SET @UserType = 'ADMIN'

	SET @LoggedInUserId = (SELECT [dbo].[func_get_userid](@UserType, @LoggedInUser))

	IF(UPPER(@OperationMode) IN ('A','U'))    
	BEGIN 	
		
		IF NOT EXISTS(SELECT 1 FROM tbl_senders WITH(NOLOCK) WHERE Id=ISNULL(@SenderId,0) AND IsDeleted=0)    
		BEGIN    
			SET @MsgText = 'Invalid SenderId!'    
			RETURN;    
		END

		IF(TRIM(ISNULL(@FirstName,'')) = '')    
		BEGIN    
			SET @MsgText = 'Please provide Firstname!'    
			RETURN;    
		END
		IF(ISNULL(@IsSurNamePresent,0) = 1 AND TRIM(ISNULL(@SurName,'')) = '')
		BEGIN
			SET @MsgText = 'Please provide Surname!'    
			RETURN;
		END	

		IF(TRIM(ISNULL(@MobileNumber,'')) = '')    
		BEGIN    
			SET @MsgText = 'Please provide Contact number!'    
			RETURN;    
		END
		IF(TRIM(ISNULL(@CountryCode,'')) = '')    
		BEGIN    
			SET @MsgText = 'Please select Country!'    
			RETURN;    
		END
		IF(TRIM(ISNULL(@ProvinceCode,'')) = '')    
		BEGIN    
			SET @MsgText = 'Please select Provice!'    
			RETURN;    
		END
		IF(TRIM(ISNULL(@DistrictCode,'')) = '')    
		BEGIN    
			SET @MsgText = 'Please select District!'    
			RETURN;    
		END
		IF(TRIM(ISNULL(@LocalBodyCode,'')) = '')    
		BEGIN    
			SET @MsgText = 'Please select LocalBody!'    
			RETURN;    
		END
		--IF(TRIM(ISNULL(@City,'')) = '')    
		--BEGIN    
		--	SET @MsgText = 'Please provide City!'    
		--	RETURN;    
		--END
		IF(TRIM(ISNULL(@Address,'')) = '')    
		BEGIN    
			SET @MsgText = 'Please provide full address!'    
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

		IF(ISNULL(@RelationshipId,0) <= 0)    
		BEGIN    
			SET @MsgText = 'Please provide Relationship with Sender!'    
			RETURN;    
		END
		IF(ISNULL(@PayoutTypeId,0) <= 0)    
		BEGIN    
			SET @MsgText = 'Please Select Payout Type!'    
			RETURN;    
		END

		DECLARE @payOutType varchar(50)
		SELECT @payOutType = PaymentTypeCode FROM dbo.tbl_payment_type WITH(NOLOCK) WHERE Id = @PayoutTypeId AND IsDeleted = 0;
		IF(@payOutType = 'BANK')
		BEGIN
			IF(TRIM(ISNULL(@BankName,'')) = '')    
			BEGIN    
				SET @MsgText = 'Please Provide Bank Name!'    
				RETURN;    
			END
			IF(TRIM(ISNULL(@BankCode,'')) = '')    
			BEGIN    
				SET @MsgText = 'Please Provide Bank Code!'    
				RETURN;    
			END
			IF(TRIM(ISNULL(@AccountHolderName,'')) = '')    
			BEGIN    
				SET @MsgText = 'Please Provide Bank Account Holder Name!'    
				RETURN;    
			END
			IF(TRIM(ISNULL(@AccountNumber,'')) = '')    
			BEGIN    
				SET @MsgText = 'Please Provide Bank Account Number!'    
				RETURN;    
			END
		END
		ELSE IF(@payOutType = 'WALLET')
		BEGIN
			IF(TRIM(ISNULL(@WalletName,'')) = '')    
			BEGIN    
				SET @MsgText = 'Please Provide Wallet Name!'    
				RETURN;    
			END
			IF(TRIM(ISNULL(@WalletId,'')) = '')    
			BEGIN    
				SET @MsgText = 'Please Provide Wallet Id (Wallet Mobile number)!'    
				RETURN;    
			END
			--IF(TRIM(ISNULL(@WalletRegisteredName,'')) = '')    
			--BEGIN    
			--	SET @MsgText = 'Please Provide Wallet Registered Name!'    
			--	RETURN;    
			--END
		END
		--ELSE IF(@payOutType = 'CASH')
		--BEGIN
			
		--END		
	END    
    
	IF(UPPER(@OperationMode) = 'A')    
	BEGIN		

		SET NOCOUNT ON;

		INSERT INTO [dbo].[tbl_recipients]
		(				
			SenderId,FirstName,SurName,IsSurNamePresent,MobileNumber,Email,GenderId,DateOfBirth,CountryCode,ProvinceCode,DistrictCode,LocalBodyCode	
			,City,Zipcode,Address,SourceCurrency,DestinationCurrency,RelationshipId,PayoutTypeId,BankName,BankCode,Branch,AccountHolderName,AccountNumber	
			,WalletName,WalletId,WalletRegisteredName,GMTTimeZone,IsActive,IsDeleted,IsBlocked,KycStatusCode	
			,CreatedUserType,CreatedById,CreatedByName,CreatedDate			
		)
		OUTPUT inserted.Id INTO @TblRecipient
		VALUES
		(
		    @SenderId,@FirstName,@SurName,@IsSurNamePresent,@MobileNumber,@Email,@GenderId,@DateOfBirth,@CountryCode,@ProvinceCode,@DistrictCode,@LocalBodyCode
			,@City,@Zipcode,@Address,@SourceCurrency,@DestinationCurrency,@RelationshipId,@PayoutTypeId, @BankName,@BankCode,@Branch,@AccountHolderName,@AccountNumber
			,@WalletId,@WalletId,@WalletRegisteredName,@GMTTimeZone,1,0,0,NULL
			,@UserType,@LoggedInUserId,@LoggedInUser,GETUTCDATE()
		)
				
		SET NOCOUNT OFF;
    
		SET @ReturnPrimaryId = (SELECT Id FROM @TblRecipient)
		SET @StatusCode = 200    
		SET @MsgType = 'Success'    
		SET @MsgText = 'Recipient Created Successfully'    
	END    
	ELSE IF(UPPER(@OperationMode) = 'U')    
	BEGIN    
		IF(ISNULL(@Id, 0) <= 0)    
		BEGIN    
			SET @MsgText = 'Invalid Id, Id can not be less than or equal to zero'    
			RETURN;    
		END 
				
		SET NOCOUNT ON;

		UPDATE [dbo].[tbl_recipients]
		SET
		FirstName	= @FirstName
		,SurName	= @SurName
		,IsSurNamePresent	= @IsSurNamePresent
		,MobileNumber	= @MobileNumber
		,Email	= @Email
		,GenderId	= @GenderId
		,DateOfBirth	= @DateOfBirth
		,CountryCode	= @CountryCode
		,ProvinceCode	= @ProvinceCode
		,DistrictCode	= @DistrictCode
		,LocalBodyCode  = @LocalBodyCode
		,City	= @City
		,Zipcode	= @Zipcode
		,Address	= @Address
		,SourceCurrency = @SourceCurrency
		,DestinationCurrency = @DestinationCurrency
		,RelationshipId  = @RelationshipId
		,PayOutTypeId  = @PayoutTypeId
		
		,BankName	= @BankName
		,BankCode	= @BankCode
		,Branch		= @Branch
		,AccountHolderName	= @AccountHolderName
		,AccountNumber		= @AccountNumber

		,WalletName = @WalletName
		,WalletId = @WalletId
		,WalletRegisteredName  = @WalletRegisteredName
		,GMTTimeZone		= @GMTTimeZone
		,IsActive			= @IsActive
		,UpdatedUserType = @UserType
		,UpdatedById = @LoggedInUserId
		,UpdatedByName = @LoggedInUser
		,UpdatedDate = GETUTCDATE()
		WHERE Id = @Id;

		SET NOCOUNT OFF;

		SET @ReturnPrimaryId = @Id    
		SET @StatusCode=200    
		SET @MsgType = 'Success'    
		SET @MsgText = 'Sender Updated Successfully' 
	END 
	ELSE IF(UPPER(@OperationMode) = 'D')
	BEGIN
		IF(ISNULL(@Id, 0) <= 0)    
		BEGIN    
			SET @MsgText = 'Invalid Id, Id can not be less than or equal to zero'    
			RETURN;    
		END

		UPDATE dbo.tbl_recipients 
		SET 
		IsDeleted = 1
		,UpdatedUserType = @UserType
		,UpdatedById = @LoggedInUserId
		,UpdatedByName = @LoggedInUser
		,UpdatedDate = GETUTCDATE()
		WHERE Id = @Id;

		SET @ReturnPrimaryId = @Id    
		SET @StatusCode=200    
		SET @MsgType = 'Success'    
		SET @MsgText = 'Recipient Deleted Successfully' 

	END
	
END 