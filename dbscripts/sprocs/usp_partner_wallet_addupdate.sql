CREATE OR ALTER PROCEDURE [dbo].[usp_partner_wallet_addupdate]     
(    
  @Id int = 0     
 ,@PartnerCode varchar(20) = NULL
 ,@SourceCurrency varchar(3) = NULL		
 ,@DestinationCurrency varchar(3) = NULL		
 ,@NotificationBalance decimal(18,2) = NULL
 ,@MarkupMinValue decimal(18,2) = NULL
 ,@MarkupMaxValue decimal(18,2) = NULL
 ,@TypeCode varchar(20) = NULL		----PERC=>PERCENTAGE, FLAT=>FLAT
 ,@Remarks nvarchar(300) = NULL
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

	DECLARE @TblWallet TABLE (Id INT); 
	DECLARE @LoggedInUserId INT;

	IF(ISNULL(@UserType,'') = '')
		SET @UserType = 'ADMIN'

	SET @LoggedInUserId = (SELECT [dbo].[func_get_userid](@UserType, @LoggedInUser))

	IF(UPPER(@OperationMode) IN ('A','U'))    
	BEGIN    
		
		IF(TRIM(ISNULL(@PartnerCode,'')) = '')    
		BEGIN    
			SET @MsgText = 'PartnerCode can not be empty!'    
			RETURN;    
		END
		IF NOT EXISTS(SELECT 1 FROM tbl_remit_partners WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND IsDeleted=0)    
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
		IF(ISNULL(@NotificationBalance,0) <= 0)    
		BEGIN    
			SET @MsgText = 'Invalid NotificationBalance!'    
			RETURN;    
		END
		IF(ISNULL(@MarkupMinValue,0) <= 0)    
		BEGIN    
			SET @MsgText = 'Invalid NotificationBalance!'    
			RETURN;    
		END
		IF(ISNULL(@MarkupMaxValue,0) <= 0)    
		BEGIN    
			SET @MsgText = 'Invalid NotificationBalance!'    
			RETURN;    
		END
		IF(@MarkupMinValue > @MarkupMaxValue)    
		BEGIN    
			SET @MsgText = 'Markup Minimum Value can not be greater than Markup Maximum Value!'    
			RETURN;    
		END
		IF(ISNULL(@TypeCode,'') NOT IN ('PERC','PERCENTAGE','FLAT'))    
		BEGIN    
			SET @MsgText = 'Invalid TypeCode!'    
			RETURN;    
		END
		
	END    
    
	IF(UPPER(@OperationMode) = 'A')    
	BEGIN	
	
		IF EXISTS(SELECT 1 FROM dbo.tbl_partner_wallets WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency AND IsDeleted=0)
		BEGIN
			SET @MsgText = 'Specified Wallet is already present!'    
			RETURN;  
		END
    
		SET NOCOUNT ON;

		INSERT INTO [dbo].[tbl_partner_wallets]
		(
			PartnerCode,SourceCurrency,DestinationCurrency,Balance,NotificationBalanceLimit,MarkupMinValue,MarkupMaxValue,TypeCode
			,Remarks,IsActive,IsDeleted,CreatedUserType,CreatedById,CreatedByName,CreatedDate
		)
		OUTPUT inserted.Id INTO @TblWallet
		VALUES
		(
			@PartnerCode,@SourceCurrency,@DestinationCurrency,0,@NotificationBalance,@MarkupMinValue,@MarkupMaxValue,@TypeCode
			,@Remarks,1,0,@UserType,@LoggedInUserId,@LoggedInUser,GETUTCDATE()
		)
				
		SET NOCOUNT OFF;
    
		SET @ReturnPrimaryId = (SELECT Id FROM @TblWallet)
		SET @StatusCode = 200    
		SET @MsgType = 'Success'    
		SET @MsgText = 'Wallet Created Successfully'    
	END    
	ELSE IF(UPPER(@OperationMode) = 'U')    
	BEGIN    
		IF(ISNULL(@Id, 0) <= 0)    
		BEGIN    
			SET @MsgText = 'Invalid Id, Id can not be less than or equal to zero'    
			RETURN;    
		END 
		IF EXISTS(SELECT 1 FROM dbo.tbl_partner_wallets WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND SourceCurrency=@SourceCurrency AND IsDeleted=0 AND Id<>@Id)
		BEGIN
			SET @MsgText = 'Specified Wallet is already present!'    
			RETURN;  
		END
		
		SET NOCOUNT ON;

		UPDATE [dbo].[tbl_partner_wallets]
		SET
		SourceCurrency = @SourceCurrency
		,DestinationCurrency = @DestinationCurrency
		,NotificationBalanceLimit = @NotificationBalance
		,MarkupMinValue	= @MarkupMinValue
		,MarkupMaxValue	= @MarkupMaxValue
		,TypeCode = @TypeCode
		,Remarks  = @Remarks
		,UpdatedUserType = @UserType
		,UpdatedById = @LoggedInUserId
		,UpdatedByName = @LoggedInUser
		,UpdatedDate = GETUTCDATE()
		WHERE Id = @Id;

		SET NOCOUNT OFF;

		SET @ReturnPrimaryId = @Id    
		SET @StatusCode=200    
		SET @MsgType = 'Success'    
		SET @MsgText = 'Wallet Updated Successfully' 
	END 
	ELSE IF(UPPER(@OperationMode) = 'D')
	BEGIN
		IF(ISNULL(@Id, 0) <= 0)    
		BEGIN    
			SET @MsgText = 'Invalid Id, Id can not be less than or equal to zero'    
			RETURN;    
		END

		UPDATE dbo.tbl_partner_wallets 
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
		SET @MsgText = 'Wallet Deleted Successfully' 

	END
	
END 