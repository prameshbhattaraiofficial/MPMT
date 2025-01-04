
CREATE OR ALTER PROCEDURE [dbo].[usp_update_role]
(
	 @RoleId INT
	,@RoleName [varchar](100) = NULL
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

	IF (ISNULL(@RoleId, 0) < 1)
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Invalid Role Id'
		RETURN
	END

	IF (ISNULL(@RoleName, '') = '')
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'RoleName is required'
		RETURN
	END

	IF EXISTS (SELECT 1 FROM [dbo].[tbl_roles] WITH(NOLOCK) WHERE Id <> @RoleId AND UPPER(RoleName) = @RoleName)
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Other role with same name already exists'
		RETURN
	END

	SET NOCOUNT ON;

	UPDATE [dbo].[tbl_roles]
	SET
		RoleName = @RoleName,
		Description = @Description,
		IsSystemRole = @IsSystemRole,
		IsActive = @IsActive,
		IsDeleted = @IsDeleted,
		UpdatedBy = @LoggedInUser,
		UpdatedLocalDate = GETDATE(),
		UpdatedUtcDate = GETUTCDATE()
		--UpdatedNepaliDate = 
	WHERE Id = @RoleId;
	
	SET @IdentityVal = @RoleId;

	SET NOCOUNT OFF;
	
	SET @StatusCode = 200;
	SET @MsgType = 'Success'
	SET @MsgText = 'Role update successful!'

END
GO