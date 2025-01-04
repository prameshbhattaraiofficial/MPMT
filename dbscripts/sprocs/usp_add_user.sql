
CREATE OR ALTER PROCEDURE [dbo].[usp_add_user]
(
	@UserName [varchar](100) = NULL
	,@FullName [varchar](200) = NULL
	,@Email [varchar](200) = NULL
	,@EmailConfirmed [bit] = NULL
	,@CountryCallingCode [varchar](10) = NULL
	,@MobileNumber [varchar](30) = NULL
	,@MobileConfirmed [bit] = NULL
	,@Gender [varchar](20) = NULL
	,@Address [varchar](500) = NULL
	,@Department [varchar](150) = NULL
	,@DateOfBirth [datetime2](7) = NULL
	,@DateOfJoining [datetime2](7) = NULL
	,@PasswordHash [nvarchar](512) = NULL
	,@PasswordSalt [nvarchar](max) = NULL
	,@AccessCodeHash [nvarchar](512) = NULL
	,@AccessCodeSalt [nvarchar](max) = NULL
	,@FailedLoginAttempt [int] = NULL
	,@TemporaryLockedTillUtcDate [datetime2](7) = NULL
	,@ProfileImageUrlPath [varchar](512) = NULL
	,@LastIpAddress [varchar](100) = NULL
	,@DeviceId [nvarchar](max) = NULL
	,@IsActive [bit] = NULL
	,@IsDeleted [bit] = NULL
	,@IsBlocked [bit] = NULL
	,@LastLoginDateUtc [datetime2](7) = NULL
	,@LastActivityDateUtc [datetime2](7) = NULL
	,@LoggedInUser [varchar](100) = NULL
	,@IdentityVal INT = NULL OUTPUT
	,@StatusCode INT = NULL OUTPUT
	,@MsgType VARCHAR(10) = NULL OUTPUT
	,@MsgText VARCHAR(200) = NULL OUTPUT
)
AS
BEGIN
	SET @MsgType = 'Error';
	SET @MsgText = 'Bad Request';
	SET @StatusCode = 400;

	SET @UserName = TRIM(@UserName)
	SET @FullName = TRIM(@FullName)
	SET @Email = TRIM(@Email)
	SET @MobileNumber = TRIM(@MobileNumber)

	IF (ISNULL(@UserName, '') = '')
	BEGIN
		SET @MsgText = 'UserName is required'
		SET @StatusCode = 400;
		RETURN;
	END

	IF (ISNULL(@FullName, '') = '')
	BEGIN
		SET @MsgText = 'Name is required'
		SET @StatusCode = 400;
		RETURN;
	END

	IF (ISNULL(@Email, '') = '')
	BEGIN
		SET @MsgText = 'Email is required'
		SET @StatusCode = 400;
		RETURN;
	END

	IF EXISTS(SELECT 1 FROM [dbo].[tbl_users] WITH (NOLOCK) WHERE UPPER(UserName) = @UserName)
	BEGIN
		SET @MsgText = 'UserName not available.'
		SET @StatusCode = 400;
		RETURN;
	END

	IF EXISTS(SELECT 1 FROM [dbo].[tbl_users] WITH (NOLOCK) WHERE UPPER(Email) = @Email)
	BEGIN
		SET @MsgText = 'Email has already been registered.'
		SET @StatusCode = 400;
		RETURN;
	END

	IF (ISNULL(@MobileNumber, '') <> '') AND EXISTS(SELECT 1 FROM [dbo].[tbl_users] WITH (NOLOCK) WHERE UPPER(Email) = @Email)
	BEGIN
		SET @MsgText = 'Mobile number has already been registered.'
		SET @StatusCode = 400;
		RETURN;
	END

	--DECLARE @CurrentNepaliDate VARCHAR(10) = [dbo].func_get_nepali_date(GETDATE());

	SET NOCOUNT ON

	BEGIN TRY
			INSERT INTO [dbo].[tbl_users]
			   ([UserName]
			   ,[FullName]
			   ,[Email]
			   ,[EmailConfirmed]
			   ,[CountryCallingCode]
			   ,[MobileNumber]
			   ,[MobileConfirmed]
			   ,[Gender]
			   ,[Address]
			   ,[Department]
			   ,[DateOfBirth]
			   ,[DateOfJoining]
			   ,[PasswordHash]
			   ,[PasswordSalt]
			   ,[AccessCodeHash]
			   ,[AccessCodeSalt]
			   ,[FailedLoginAttempt]
			   ,[TemporaryLockedTillUtcDate]
			   ,[ProfileImageUrlPath]
			   ,[LastIpAddress]
			   ,[DeviceId]
			   ,[IsActive]
			   ,[IsDeleted]
			   ,[IsBlocked]
			   ,[LastLoginDateUtc]
			   ,[LastActivityDateUtc]
			   ,[CreatedBy]
			   ,[CreatedLocalDate]
			   ,[CreatedUtcDate])
		 VALUES
			   (@UserName
			   ,@FullName
			   ,@Email
			   ,ISNULL(@EmailConfirmed, 0)
			   ,@CountryCallingCode
			   ,@MobileNumber
			   ,ISNULL(@MobileConfirmed, 0)
			   ,@Gender
			   ,@Address
			   ,@Department
			   ,@DateOfBirth
			   ,@DateOfJoining
			   ,@PasswordHash
			   ,@PasswordSalt
			   ,@AccessCodeHash
			   ,@AccessCodeSalt
			   ,@FailedLoginAttempt
			   ,@TemporaryLockedTillUtcDate
			   ,@ProfileImageUrlPath
			   ,@LastIpAddress
			   ,@DeviceId
			   ,ISNULL(@IsActive, 1)
			   ,ISNULL(@IsDeleted, 0)
			   ,ISNULL(@IsBlocked, 0)
			   ,@LastLoginDateUtc
			   ,@LastActivityDateUtc
			   ,@LoggedInUser
			   ,GETDATE()
			   ,GETUTCDATE())

		SET @IdentityVal = SCOPE_IDENTITY();
	END TRY
	BEGIN CATCH
		DECLARE @ErrorNumber INT = ERROR_NUMBER();
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        DECLARE @ErrorProcedure NVARCHAR(256) = ERROR_PROCEDURE();
        DECLARE @ErrorLine INT = ERROR_LINE();

        EXEC [dbo].[usp_logdberror] @ErrorNumber, @ErrorMessage, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine;
		
		-- Re-throw the error to the calling application
        THROW;
	END CATCH

	SET NOCOUNT OFF

	SET @MsgType = 'Success'
	SET @MsgText = 'User registration successful!'
	SET @StatusCode = 200;
END
GO