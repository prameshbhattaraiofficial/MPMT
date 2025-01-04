CREATE OR ALTER  PROCEDURE [dbo].[usp_ServiceTypeListInsert]
(
@SourceCurrency varchar(3) = NULL
,@DestinationCurrency varchar(3) = NULL
,@ChargeCategoryId int = NULL
,@PaymentTypeId int = NULL
,@ServiceChargeSettingsType ServiceChargeSettingsType READONLY
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
	
	SET NOCOUNT ON;

	SET @StatusCode = 400    
	SET @MsgType = 'Error'    
	SET @MsgText = 'Bad Request'    
	SET @ReturnPrimaryId = 0 
	   
	DECLARE @LoggedInUserId INT;

	IF(ISNULL(@UserType,'') = '')
		SET @UserType = 'ADMIN'

	SET @LoggedInUserId = (SELECT [dbo].[func_get_userid](@UserType, @LoggedInUser))

	DECLARE @ErrorNumber INT 
	DECLARE @ErrorMessage NVARCHAR(4000)
	DECLARE @ErrorSeverity INT
	DECLARE @ErrorState INT
	DECLARE @ErrorProcedure NVARCHAR(256)
	DECLARE @ErrorLine INT

	------=================VALIDATION BEGINS=======================================================

	IF(@OperationMode='A')
	BEGIN
		IF EXISTS(SELECT 1 FROM dbo.tbl_service_charge_settings WITH(NOLOCK) WHERE SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency 
				AND ChargeCategoryId=@ChargeCategoryId AND PaymentTypeId=@PaymentTypeId)		------INSERT RECORD
		BEGIN
			SET @MsgText = 'Invalid Operation. Data already present with mentioned Source and Destination currency, so please do UPDATE!' 
			RETURN;
		END
	END


	DECLARE @sn INT, @length INT, @minSlab DECIMAL(18,2), @maxSlab DECIMAL(18,2);	
	DECLARE @tmpCharge AS TABLE (Id INT IDENTITY(1,1), MinSlab DECIMAL(18,2), MaxSlab DECIMAL(18,2));
	INSERT INTO @tmpCharge
	SELECT MinAmountSlab,MaxAmountSlab FROM @ServiceChargeSettingsType;
	
	SET @sn = 1;
	SET @length = (SELECT COUNT(*) FROM @tmpCharge);
	WHILE (@sn <= @length)
	BEGIN
		SELECT @minSlab = MinSlab, @maxSlab=MaxSlab FROM @tmpCharge WHERE Id=@sn;
		IF(@minSlab > @maxSlab)
		BEGIN
			SET @MsgText = 'Invalid Slab. Min slab can not be greater than Max slab!' 
			RETURN;
		END

		IF EXISTS(SELECT 1 FROM @tmpCharge WHERE ((@minSlab between MinSlab AND MaxSlab) OR  (@maxSlab between MinSlab AND MaxSlab)) AND Id<>@sn)
		BEGIN
			SET @MsgText = 'Invalid Slab range!' 
			RETURN;
		END

		SET @sn = @sn +1;
	END	


	IF EXISTS(SELECT 1 FROM @ServiceChargeSettingsType WHERE ServiceChargePercent >= 100 OR CommissionPercent >= 100)
	BEGIN
		SET @MsgText = 'Invalid ServiceCharge/Commission Percentage value!' 
		RETURN;
	END
	IF EXISTS(SELECT 1 FROM @ServiceChargeSettingsType WHERE MinServiceCharge > MaxServiceCharge)
	BEGIN
		SET @MsgText = 'MinServiceCharge should not be greater than MaxServiceCharge!' 
		RETURN;
	END
	IF EXISTS(SELECT 1 FROM @ServiceChargeSettingsType WHERE MinCommission > MaxCommission)
	BEGIN
		SET @MsgText = 'MinCommission should not be greater than MaxCommision!' 
		RETURN;
	END

	------=================VALIDATION ENDS=======================================================

	IF NOT EXISTS(SELECT 1 FROM dbo.tbl_service_charge_settings WITH(NOLOCK) WHERE SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency 
				AND ChargeCategoryId=@ChargeCategoryId AND PaymentTypeId=@PaymentTypeId)		------INSERT RECORD
	BEGIN
		BEGIN TRY
		
		Insert INTO [dbo].[tbl_service_charge_settings]([ChargeCategoryId],[SourceCurrency],[DestinationCurrency],[PaymentTypeId]
		,[MinAmountSlab],[MaxAmountSlab],[ServiceChargePercent],[ServiceChargeFixed],[MinServiceCharge]
		,[MaxServiceCharge],[CommissionPercent],[CommissionFixed],[MinCommission],[MaxCommission],[FromDate]
		,[ToDate],[IsActive],[IsDeleted],[Maker],[Checker],[CreatedById],[CreatedByName],[CreatedDate]) 

		SELECT					
	   @ChargeCategoryId
      ,@SourceCurrency
      ,@DestinationCurrency
      ,@PaymentTypeId
      ,[MinAmountSlab]
      ,[MaxAmountSlab]
      ,[ServiceChargePercent]
      ,[ServiceChargeFixed]
      ,[MinServiceCharge]
      ,[MaxServiceCharge]
      ,[CommissionPercent]
      ,[CommissionFixed]
      ,[MinCommission]
      ,[MaxCommission]
      ,[FromDate]
      ,[ToDate]
      ,1
      ,0
      ,NULL
      ,NULL
      ,@LoggedInUserId
      ,@LoggedInUser
      ,GETUTCDATE()
      from @ServiceChargeSettingsType;

	 SET @StatusCode = 200    
	SET @MsgType = 'Success'    
	SET @MsgText = 'Data added successfuly!'    
	SET @ReturnPrimaryId = @@IDENTITY

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

	END
	ELSE			------UPDATE RECORD
	BEGIN
		BEGIN TRY
		
		INSERT INTO dbo.tbl_service_charge_settings_history
			select * from dbo.tbl_service_charge_settings where SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency 
				AND ChargeCategoryId=@ChargeCategoryId AND PaymentTypeId=@PaymentTypeId;

		DELETE FROM dbo.tbl_service_charge_settings where SourceCurrency=@SourceCurrency AND DestinationCurrency=@DestinationCurrency 
				AND ChargeCategoryId=@ChargeCategoryId AND PaymentTypeId=@PaymentTypeId;

		Insert INTO [dbo].[tbl_service_charge_settings]([ChargeCategoryId],[SourceCurrency],[DestinationCurrency],[PaymentTypeId]
		,[MinAmountSlab],[MaxAmountSlab],[ServiceChargePercent],[ServiceChargeFixed],[MinServiceCharge]
		,[MaxServiceCharge],[CommissionPercent],[CommissionFixed],[MinCommission],[MaxCommission],[FromDate]
		,[ToDate],[IsActive],[IsDeleted],[Maker],[Checker],[UpdatedById],[UpdatedByName],[UpdatedDate]) 

		SELECT					
	   @ChargeCategoryId
      ,@SourceCurrency
      ,@DestinationCurrency
      ,@PaymentTypeId
      ,[MinAmountSlab]
      ,[MaxAmountSlab]
      ,[ServiceChargePercent]
      ,[ServiceChargeFixed]
      ,[MinServiceCharge]
      ,[MaxServiceCharge]
      ,[CommissionPercent]
      ,[CommissionFixed]
      ,[MinCommission]
      ,[MaxCommission]
      ,[FromDate]
      ,[ToDate]
      ,1
      ,0
      ,NULL
      ,NULL
      ,@LoggedInUserId
      ,@LoggedInUser
      ,GETUTCDATE()
      from @ServiceChargeSettingsType;

	  SET @StatusCode = 200    
	SET @MsgType = 'Success'    
	SET @MsgText = 'Data updated successfuly!'    
	SET @ReturnPrimaryId = @@IDENTITY

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
	END

	SET NOCOUNT OFF;
	RETURN 1;
End