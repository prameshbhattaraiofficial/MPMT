CREATE OR ALTER   PROCEDURE [dbo].[usp_compliance_rule_addupdate]
(
	@Id int = 0
	,@ComplianceCode [varchar](50) = NULL
	,@ComplianceRule [varchar](300) = NULL
	,@Description [varchar](500) = NULL
	,@Count [int] = NULL
	,@NoOfDays [varchar](20) = NULL
	,@ComplianceAction [varchar](50) = NULL
	,@IsActive [bit] = NULL
	,@OperationMode [varchar](20) = NULL
	,@LoggedInUser [varchar](100) = NULL
	,@IdentityVal INT = NULL OUTPUT
	,@StatusCode INT = NULL OUTPUT
	,@MsgType VARCHAR(10) = NULL OUTPUT
	,@MsgText VARCHAR(200) = NULL OUTPUT  
)
AS
BEGIN
	SET @MsgType = 'Error'  
	SET @MsgText = 'Bad Request'  
	SET @StatusCode = 400;

	DECLARE @UserId int;
	SET @UserId = (SELECT Id FROM dbo.tbl_users WITH(NOLOCK) WHERE UserName=@LoggedInUser);

	SET @ComplianceCode = TRIM(ISNULL(@ComplianceCode,''))
	SET @ComplianceRule = TRIM(ISNULL(@ComplianceRule,''))
	SET @Description =	  TRIM(ISNULL(@Description,''))
	SET @ComplianceAction =	  TRIM(ISNULL(@ComplianceAction,''))
	SET @OperationMode =  TRIM(ISNULL(@OperationMode,''))

	IF(@OperationMode = 'A')
	BEGIN
		IF (ISNULL(@ComplianceCode, '') = '')
		BEGIN
			SET @StatusCode = 400
			SET @MsgText = 'ComplianceCode is required'
			RETURN
		END
		IF EXISTS (SELECT 1 FROM [dbo].[tbl_compliance_settings] WITH(NOLOCK) WHERE ComplianceCode = @ComplianceCode)
		BEGIN
			SET @StatusCode = 400
			SET @MsgText = 'ComplianceCode already exists.'
			RETURN
		END
		IF (ISNULL(@ComplianceRule, '') = '')
		BEGIN
			SET @StatusCode = 400
			SET @MsgText = 'ComplianceRule is required'
			RETURN
		END
		IF EXISTS (SELECT 1 FROM [dbo].[tbl_compliance_settings] WITH(NOLOCK) WHERE ComplianceRule = @ComplianceRule)
		BEGIN
			SET @StatusCode = 400
			SET @MsgText = 'ComplianceRule already exists.'
			RETURN
		END
		IF (ISNULL(@ComplianceAction, '') = '')
		BEGIN
			SET @StatusCode = 400
			SET @MsgText = 'ComplianceAction is required'
			RETURN
		END
		IF (ISNULL(@ComplianceAction, '') not in ('Reject','Suspicious'))
		BEGIN
			SET @StatusCode = 400
			SET @MsgText = 'Invalid ComplianceAction!'
			RETURN
		END
		
		SET NOCOUNT ON;

		INSERT INTO [dbo].[tbl_compliance_settings]
				   ([ComplianceCode]
				   ,[ComplianceRule]
				   ,[Description]
				   ,[Count]
				   ,[NoOfDays]
				   ,[ComplianceAction]
				   ,[IsActive]
				   ,[CreatedById]
				   ,[CreatedByName]
				   ,[CreatedDate]
				   )
			 VALUES
				   (@ComplianceCode
				   ,@ComplianceRule
				   ,@Description
				   ,@Count
				   ,@NoOfDays
				   ,@ComplianceAction
				   ,ISNULL(@IsActive, 0)
				   ,@UserId
				   ,@LoggedInUser
				   ,GETUTCDATE())
	
		SET @IdentityVal = SCOPE_IDENTITY();

		SET NOCOUNT OFF;
	
		SET @StatusCode = 200;
		SET @MsgType = 'Success'
		SET @MsgText = 'Compliance Rule added successfully!'

	END
	ELSE IF(@OperationMode = 'U')
	BEGIN
		IF (ISNULL(@Id, 0) <= 0)
		BEGIN
			SET @StatusCode = 400
			SET @MsgText = 'Invalid Id'
			RETURN
		END

		--DECLARE @existingCode VARCHAR(50);
		--SELECT @existingCode=ComplianceCode FROM dbo.tbl_compliance_settings WITH(NOLOCK) WHERE Id=@Id;

		--IF NOT EXISTS(SELECT 1 FROM dbo.tbl_remit_transaction WITH(NOLOCK) WHERE ComplianceStatusCode=@existingCode)
		--BEGIN
		--	IF (ISNULL(@ComplianceCode, '') = '')
		--	BEGIN
		--		SET @StatusCode = 400
		--		SET @MsgText = 'ComplianceCode is required'
		--		RETURN
		--	END
		--	IF EXISTS (SELECT 1 FROM [dbo].[tbl_compliance_settings] WITH(NOLOCK) WHERE ComplianceCode = @ComplianceCode AND Id<>@Id)
		--	BEGIN
		--		SET @StatusCode = 400
		--		SET @MsgText = 'ComplianceCode already exists.'
		--		RETURN
		--	END			
		--END
		--ELSE
		--BEGIN
		--	SET @StatusCode = 400
		--	SET @MsgText = 'ComplianceCode can not be altered, since this code has already been used in transactions!'
		--	RETURN
		--END

		--IF (ISNULL(@ComplianceRule, '') = '')
		--BEGIN
		--	SET @StatusCode = 400
		--	SET @MsgText = 'ComplianceRule is required'
		--	RETURN
		--END
		IF (ISNULL(@ComplianceAction, '') = '')
		BEGIN
			SET @StatusCode = 400
			SET @MsgText = 'ComplianceAction is required'
			RETURN
		END
		IF (ISNULL(@ComplianceAction, '') not in ('Reject','Suspicious'))
		BEGIN
			SET @StatusCode = 400
			SET @MsgText = 'Invalid ComplianceAction!'
			RETURN
		END

		SET NOCOUNT ON;

		UPDATE dbo.tbl_compliance_settings
		SET 
		--ComplianceCode = @ComplianceCode
		--ComplianceRule = @ComplianceRule		
		--[Description] = @Description
		[Count] = @Count
		,NoOfDays = @NoOfDays
		,ComplianceAction = @ComplianceAction
		,isActive = ISNULL(@IsActive,0)
		,UpdatedById = @UserId
		,UpdatedByName = @LoggedInUser
		,UpdatedDate = GETUTCDATE()
		WHERE Id = @Id;

		SET @IdentityVal = @Id;

		SET NOCOUNT OFF;
	
		SET @StatusCode = 200;
		SET @MsgType = 'Success'
		SET @MsgText = 'Compliance Rule updated successfully!'

	END
	ELSE IF(@OperationMode = 'D')
	BEGIN

		IF (ISNULL(@Id, 0) <= 0)
		BEGIN
			SET @StatusCode = 400
			SET @MsgText = 'Invalid Id'
			RETURN
		END

		SET NOCOUNT ON;

		UPDATE dbo.tbl_compliance_settings
		SET 
		isDeleted = 1
		,UpdatedById = @UserId
		,UpdatedByName = @LoggedInUser
		,UpdatedDate = GETUTCDATE()
		WHERE Id = @Id;

		SET @IdentityVal = @Id;

		SET NOCOUNT OFF;
	
		SET @StatusCode = 200;
		SET @MsgType = 'Success'
		SET @MsgText = 'Compliance Rule deleted successfully!'
	END	

END
