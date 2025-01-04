CREATE OR ALTER PROCEDURE [dbo].[usp_get_user_by_email]
(
	@Email VARCHAR(200)
)
AS
BEGIN
	SET @Email = TRIM(@Email)

	IF (ISNULL(@Email, '') = '')
	RETURN

	SELECT [Id]
		  ,[UserName]
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
		  ,[CreatedUtcDate]
		  ,[CreatedNepaliDate]
		  ,[UpdatedBy]
		  ,[UpdatedLocalDate]
		  ,[UpdatedUtcDate]
		  ,[UpdatedNepaliDate]
	  FROM [dbo].[tbl_users] WITH (NOLOCK)
	  WHERE Email = @Email

END
GO

