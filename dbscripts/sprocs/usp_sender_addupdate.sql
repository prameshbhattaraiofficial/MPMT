CREATE OR ALTER PROCEDURE [dbo].[usp_sender_addupdate]     
(    
  @Id int = 0   
  ,@PartnerCode varchar(20) = NULL
 ,@FirstName nvarchar(100) = NULL
 ,@SurName nvarchar(100) = NULL
 ,@IsSurNamePresent bit = NULL
 ,@MobileNumber nvarchar(20) = NULL
 ,@Email nvarchar(150) = NULL
 ,@GenderId int = NULL
 ,@ProfileImagePath nvarchar(500) = NULL
 ,@DateOfBirth datetime = NULL
 ,@CountryCode varchar(10) = NULL
 ,@Province nvarchar(150) = NULL
 ,@City nvarchar(150) = NULL
 ,@Zipcode nvarchar(50) = NULL
 ,@Address nvarchar(300) = NULL
 ,@OccupationId int = NULL
 ,@DocumentTypeId int = NULL
 ,@DocumentNumber nvarchar(50) = NULL
 ,@IssuedDate datetime = NULL
 ,@ExpiryDate datetime = NULL
 ,@IdFrontImgPath nvarchar(500) = NULL
 ,@IdBackImgPath nvarchar(500) = NULL
 ,@BankName nvarchar(200) = NULL
 ,@BankCode nvarchar(50) = NULL
 ,@Branch nvarchar(200) = NULL
 ,@AccountHolderName nvarchar(200) = NULL
 ,@AccountNumber nvarchar(50) = NULL
 ,@IncomeSourceId int = NULL
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

	DECLARE @TblSender TABLE (Id INT); 
	DECLARE @LoggedInUserId INT;

	IF(ISNULL(@UserType,'') = '')
		SET @UserType = 'ADMIN'

	SET @LoggedInUserId = (SELECT [dbo].[func_get_userid](@UserType, @LoggedInUser))

	IF(UPPER(@OperationMode) IN ('A','U'))    
	BEGIN    
		
		IF(TRIM(ISNULL(@PartnerCode,'')) = '')    
		BEGIN    
			SET @MsgText = 'Please provide PartnerCode!'    
			RETURN;    
		END
		IF NOT EXISTS(SELECT 1 FROM tbl_remit_partners WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND IsDeleted=0)    
		BEGIN    
			SET @MsgText = 'Invalid PartnerCode!'    
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
		IF(TRIM(ISNULL(@City,'')) = '')    
		BEGIN    
			SET @MsgText = 'Please provide City!'    
			RETURN;    
		END
		IF(TRIM(ISNULL(@Address,'')) = '')    
		BEGIN    
			SET @MsgText = 'Please provide full address!'    
			RETURN;    
		END
		IF(ISNULL(@OccupationId,0) <= 0)    
		BEGIN    
			SET @MsgText = 'Please select Occupation!'    
			RETURN;    
		END
		IF(ISNULL(@DocumentTypeId,0) <= 0)    
		BEGIN    
			SET @MsgText = 'Please select Document Type!'    
			RETURN;    
		END
		IF(TRIM(ISNULL(@DocumentNumber,'')) = '')    
		BEGIN    
			SET @MsgText = 'Please provide Document number!'    
			RETURN;    
		END

		IF(ISNULL(@IdFrontImgPath,'') = '')    
		BEGIN    
			SET @MsgText = 'Please provide Id Front Image!'    
			RETURN;    
		END		
	END    
    
	IF(UPPER(@OperationMode) = 'A')    
	BEGIN	
		
		DECLARE @memberId varchar(20);
		gn:EXEC usp_generate_random_numeric_string 6, @memberId output		
		IF EXISTS(SELECT 1 FROM dbo.tbl_senders WITH(NOLOCK) WHERE MemberId=@memberId)
		GOTO gn;

		--select @memberId;

		SET NOCOUNT ON;

		INSERT INTO [dbo].[tbl_senders]
		(				
			PartnerCode,MemberId,FirstName,SurName,IsSurNamePresent,MobileNumber,Email,GenderId,DateOfBirth,ProfileImagePath,CountryCode,Province,City,Zipcode,Address	
			,OccupationId,DocumentTypeId,DocumentNumber,IssuedDate,ExpiryDate,IdFrontImgPath,IdBackImgPath,BankName,BankCode,Branch,AccountHolderName,AccountNumber	
			,IncomeSourceId,GMTTimeZone,IsActive,IsDeleted,IsBlocked,KycStatusCode,Is2FAAuthenticated,AccountSecretKey
			,CreatedUserType,CreatedById,CreatedByName,CreatedDate			
		)
		OUTPUT inserted.Id INTO @TblSender
		VALUES
		(
		    @PartnerCode,@memberId,@FirstName,@SurName,@IsSurNamePresent,@MobileNumber,@Email,@GenderId,@DateOfBirth,@ProfileImagePath,@CountryCode,@Province,@City,@Zipcode,@Address
			,@OccupationId,@DocumentTypeId,@DocumentNumber,@IssuedDate,@ExpiryDate,@IdFrontImgPath,@IdBackImgPath,@BankName,@BankCode,@Branch,@AccountHolderName,@AccountNumber
			,@IncomeSourceId,@GMTTimeZone,1,0,0,NULL,NULL,NULL
			,@UserType,@LoggedInUserId,@LoggedInUser,GETUTCDATE()
		)
				
		SET NOCOUNT OFF;
    
		SET @ReturnPrimaryId = (SELECT Id FROM @TblSender)
		SET @StatusCode = 200    
		SET @MsgType = 'Success'    
		SET @MsgText = 'Sender Created Successfully'    
	END    
	ELSE IF(UPPER(@OperationMode) = 'U')    
	BEGIN    
		IF(ISNULL(@Id, 0) <= 0)    
		BEGIN    
			SET @MsgText = 'Invalid Id, Id can not be less than or equal to zero'    
			RETURN;    
		END 
				
		SET NOCOUNT ON;

		UPDATE [dbo].[tbl_senders]
		SET
		FirstName	= @FirstName
		,SurName	= @SurName
		,IsSurNamePresent	= @IsSurNamePresent
		,MobileNumber	= @MobileNumber
		,Email	= @Email
		,GenderId	= @GenderId
		,DateOfBirth	= @DateOfBirth
		,ProfileImagePath	= @ProfileImagePath
		,CountryCode	= @CountryCode
		,Province	= @Province
		,City	= @City
		,Zipcode	= @Zipcode
		,Address	= @Address
		,OccupationId	= @OccupationId
		,DocumentTypeId = @DocumentTypeId
		,DocumentNumber = @DocumentNumber
		,IssuedDate		= @IssuedDate
		,ExpiryDate		= @ExpiryDate
		,IdFrontImgPath	= @IdFrontImgPath
		,IdBackImgPath	= @IdBackImgPath
		,BankName	= @BankName
		,BankCode	= @BankCode
		,Branch		= @Branch
		,AccountHolderName	= @AccountHolderName
		,AccountNumber		= @AccountNumber
		,IncomeSourceId		= @IncomeSourceId
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

		UPDATE dbo.tbl_senders 
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
		SET @MsgText = 'Sender Deleted Successfully' 

	END
	
END 