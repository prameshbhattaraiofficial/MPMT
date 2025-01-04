CREATE OR ALTER PROCEDURE [dbo].[usp_get_user_by_id]
(
	@UserId INT
)
AS
BEGIN
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
  WHERE Id = @UserId

END
GO

