CREATE OR ALTER PROCEDURE [dbo].[usp_add_role]
(
	@RoleName [varchar](100) = NULL
	,@Description [varchar](300) = NULL
	,@IsSystemRole [bit] = NULL
	,@IsActive [bit] = NULL
	,@IsDeleted [bit] = NULL
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

	SET @RoleName = TRIM(@RoleName)
	SET @Description = TRIM(@Description)

	IF (ISNULL(@RoleName, '') = '')
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'RoleName is required'
		RETURN
	END

	IF EXISTS (SELECT 1 FROM [dbo].[tbl_roles] WITH(NOLOCK) WHERE UPPER(RoleName) = @RoleName)
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Role already exists.'
		RETURN
	END

	SET NOCOUNT ON;

	INSERT INTO [dbo].[tbl_roles]
			   ([RoleName]
			   ,[Description]
			   ,[IsSystemRole]
			   ,[IsActive]
			   ,[IsDeleted]
			   ,[CreatedBy]
			   ,[CreatedLocalDate]
			   ,[CreatedUtcDate])
		 VALUES
			   (@RoleName
			   ,@Description
			   ,ISNULL(@IsSystemRole, 0)
			   ,ISNULL(@IsActive, 0)
			   ,ISNULL(@IsDeleted, 0)
			   ,@LoggedInUser
			   ,GETDATE()
			   ,GETUTCDATE())
	
	SET @IdentityVal = SCOPE_IDENTITY();

	SET NOCOUNT OFF;
	
	SET @StatusCode = 200;
	SET @MsgType = 'Success'
	SET @MsgText = 'Role added successfully!'

END
GO