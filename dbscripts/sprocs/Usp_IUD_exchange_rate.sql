CREATE OR ALTER Proc [dbo].[Usp_IUD_exchange_rate](	@Event char(2) = 'I', 	@Id Int = 0, 	@SourceCurrency varchar(3),	@DestinationCurrency varchar(3),	@UnitValue int,	@BuyingRate decimal(18,4),	@SellingRate decimal(18,4),	@CurrentRate decimal(18,4),	@IsActive bit,		--@LoggedInUser int,	@UserType varchar(50) = NULL,	@LoggedInUserName varchar(100)=null,	@IdentityVal INT = NULL OUTPUT,
    @StatusCode INT = NULL OUTPUT,
	@MsgType VARCHAR(10) = NULL OUTPUT,
	@MsgText VARCHAR(200) = NULL OUTPUT  
)
As
SET @StatusCode = 400
	SET @MsgType = 'Error'
	SET @MsgText = 'Bad Request'
	SET @IdentityVal = 0

DECLARE @ErrorNumber INT 
DECLARE @ErrorMessage NVARCHAR(4000)
DECLARE @ErrorSeverity INT
DECLARE @ErrorState INT
DECLARE @ErrorProcedure NVARCHAR(256)
DECLARE @ErrorLine INT

DECLARE @LoggedInUserId INT;

IF(ISNULL(@UserType,'') = '')
	SET @UserType = 'ADMIN'

SET @LoggedInUserId = (SELECT [dbo].[func_get_userid](@UserType, @LoggedInUserName))


SET @SourceCurrency = TRIM(@SourceCurrency)
	SET @DestinationCurrency = TRIM(@DestinationCurrency)
If @Event = 'I'  --for Insert
Begin 
	--------------------------------------------- Validation Begin ------------------------------------------------------
	IF (ISNULL(@SourceCurrency, '') = '')
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Source Currency is required'
		RETURN
	END

	IF (ISNULL(@DestinationCurrency, '') = '')
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Destination Currency is required'
		RETURN
	END

	IF (@SourceCurrency=@DestinationCurrency)
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Source and Destination Currency can not be same!'
		RETURN
	END

	IF EXISTS (SELECT 1 FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency AND IsDeleted=0)
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Source and Destination Currency combination already exists!'
		RETURN
	END
   --------------------------------------------- Validation End  -------------------------------------------------------
    BEGIN TRY
	Insert Into dbo.tbl_exchange_rate(SourceCurrency,DestinationCurrency,UnitValue,BuyingRate,SellingRate,CurrentRate,IsActive,IsDeleted,CreatedById,CreatedByName,CreatedDate) Values 	(			@SourceCurrency,		@DestinationCurrency,		@UnitValue,		@BuyingRate,		@SellingRate,		@CurrentRate,		@IsActive,		0,		@LoggedInUserId,		@LoggedInUserName,		GETUTCDATE()			)  
	Set @IdentityVal = @@IDENTITY
	Set @MsgText = 'Record Inserted Successfully !'	
	SET @StatusCode = 200
	SET @MsgType = 'Success'
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
End 
Else
If @Event = 'U'    --- Update
Begin
	--------------------------------------------- Validation Begin ------------------------------------------------------
	IF (ISNULL(@Id, 0) = 0)
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Id is required'
		RETURN
	END
	IF (ISNULL(@SourceCurrency, '') = '')
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Source Currency is required'
		RETURN
	END

	IF (ISNULL(@DestinationCurrency, '') = '')
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Destination Currency is required'
		RETURN
	END
	IF (@SourceCurrency=@DestinationCurrency)
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Source and Destination Currency can not be same!'
		RETURN
	END
	IF EXISTS (SELECT 1 FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency AND IsDeleted=0 AND Id<>@Id)
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Source and Destination Currency combination already exists!'
		RETURN
	END
   --------------------------------------------- Validation End  -------------------------------------------------------
   BEGIN TRY

   INSERT INTO dbo.tbl_exchange_rate_history
   SELECT * FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE Id = @Id;

	Update dbo.tbl_exchange_rate Set 		[SourceCurrency] = @SourceCurrency,		[DestinationCurrency] = @DestinationCurrency,		[UnitValue] = @UnitValue,		[BuyingRate] = @BuyingRate,		[SellingRate] = @SellingRate,		[CurrentRate] = @CurrentRate,				[IsActive] = @IsActive,				[UpdatedById] = @LoggedInUserId,		[UpdatedByName] = @LoggedInUserName,		[UpdatedDate] = GETUTCDATE()	 Where [Id] = @Id	Set @IdentityVal = @Id 	Set @MsgText = 'Record Updated Successfully !'	SET @StatusCode = 200
	SET @MsgType = 'Success'
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
	End 
Else
If @Event = 'D'   -- For Delete 
Begin 

--------------------------------------------- Validation Begin ------------------------------------------------------
IF (ISNULL(@Id, 0) = 0)
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Id is required'
		RETURN
	END
--------------------------------------------- Validation End  -------------------------------------------------------
	BEGIN TRY
	UPDATE  dbo.tbl_exchange_rate set 	IsDeleted=1,	[UpdatedById] = @LoggedInUserId,	[UpdatedByName] = @LoggedInUserName,	[UpdatedDate] = GETUTCDATE()	Where [Id] = @Id 	Set @IdentityVal = @Id 	Set @MsgText = 'Record Deleted Successfully !'	
	SET @StatusCode = 200
	SET @MsgType = 'Success'
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
End 