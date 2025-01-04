CREATE OR ALTER PROCEDURE [dbo].[usp_add_roles_to_user]
(
	@UserId INT,  
	@RoleIds VARCHAR(MAX), -- Comma separated ids  
	@StatusCode INT = NULL OUTPUT,  
	@MsgType VARCHAR(10) = NULL OUTPUT,  
	@MsgText VARCHAR(200) = NULL OUTPUT
)
AS
BEGIN
	 SET @MsgType = 'Error'  
	 SET @MsgText = 'Bad Request'  
	 SET @StatusCode = 400;

	 SET @RoleIds = TRIM(ISNULL(@RoleIds, ''))

	 IF (@RoleIds = '')  
	 BEGIN  
		  SET @MsgText = 'Please select at least one role!'  
		  SET @StatusCode = 400;  
		  RETURN
	 END

	IF NOT EXISTS (SELECT 1 FROM [dbo].[tbl_users] WITH (NOLOCK) WHERE [Id] = @UserId)  
	BEGIN  
		SET @MsgText = 'User does not exists'  
		SET @StatusCode = 404;
		RETURN
	END

	SET NOCOUNT ON;

	DELETE FROM [dbo].[tbl_user_roles] WHERE UserId = @UserId;

	INSERT INTO [dbo].[tbl_user_roles]
		(UserId, RoleId)
	SELECT @UserId, rids.Id
		FROM (SELECT TRY_CAST(TRIM(value) AS INT) AS Id FROM STRING_SPLIT(@RoleIds, ',')) rids
		INNER JOIN [dbo].[tbl_roles] r ON rids.Id = r.Id
		WHERE rids.Id IS NOT NULL

	SET NOCOUNT OFF;

	SET @MsgType = 'Success'  
	SET @MsgText = 'Roles added to user'  
	SET @StatusCode = 200;
END
GO