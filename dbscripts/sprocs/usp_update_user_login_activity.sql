
CREATE OR ALTER PROCEDURE [dbo].[usp_update_user_login_activity]
(
	@UserId INT,
	@FailedLoginAttempt INT = NULL,
	@TemporaryLockedTillUtcDate DATETIME = NULL,
	@LastIpAddress VARCHAR(100) = NULL,
	@DeviceId NVARCHAR(MAX) = NULL,
	@IsActive BIT = NULL,
	@IsBlocked BIT = NULL,
	@LastLoginDateUtc DATETIME2 = NULL,
	@LastActivityDateUtc DATETIME2 = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY
		UPDATE [dbo].[tbl_users]
		SET
			FailedLoginAttempt = @FailedLoginAttempt,
			TemporaryLockedTillUtcDate = @TemporaryLockedTillUtcDate,
			LastIpAddress = @LastIpAddress,
			DeviceId = @DeviceId,
			IsActive = @IsActive,
			IsBlocked = @IsBlocked,
			LastLoginDateUtc = @LastLoginDateUtc,
			LastActivityDateUtc = @LastActivityDateUtc
		WHERE Id = @UserId;
	END TRY
	BEGIN CATCH
		DECLARE @ErrorNumber INT = ERROR_NUMBER();
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        DECLARE @ErrorProcedure NVARCHAR(256) = ERROR_PROCEDURE();
        DECLARE @ErrorLine INT = ERROR_LINE();

        EXEC [dbo].[usp_logdberror] @ErrorNumber, @ErrorMessage, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine;

		THROW
	END CATCH
	
	SET NOCOUNT OFF
END
GO