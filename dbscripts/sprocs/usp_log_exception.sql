

CREATE OR ALTER PROCEDURE [dbo].[usp_log_exception]
(
	@LogId NVARCHAR(100) = NULL,
	@UserName NVARCHAR(200) = NULL,
	@UserAgent NVARCHAR(500) = NULL,
	@RemoteIpAddress NVARCHAR(100) = NULL,
	@ControllerName NVARCHAR(100) = NULL,
	@ActionName NVARCHAR(100) = NULL,
	@QueryString NVARCHAR(2000) = NULL,
	@Headers NVARCHAR(max) = NULL,
	@RequestUrl NVARCHAR(500) = NULL,
	@HttpMethod NVARCHAR(10) = NULL,
	@RequestBody NVARCHAR(max) = NULL,
	@ExceptionType NVARCHAR(500) = NULL,
	@ExceptionMessage NVARCHAR(max),
	@ExceptionStackTrace NVARCHAR(max),
	@InnerExceptionMessage NVARCHAR(2000) = NULL,
	@InnerExceptionStackTrace NVARCHAR(max) = NULL,
	@MachineName NVARCHAR(200) = NULL,
	@Environment NVARCHAR(200) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

	IF (ISNULL(@LogId, '') = '')
	BEGIN
		SET @LogId = LOWER(NEWID())
	END
    
    INSERT INTO [dbo].[tbl_exception_logs](
		LogId,
		UserName,
		UserAgent,
		RemoteIpAddress,
		ControllerName,
		ActionName,
		QueryString,
		Headers,
		RequestUrl,
		HttpMethod,
		RequestBody,
		ExceptionType,
		ExceptionMessage,
		ExceptionStackTrace,
		InnerExceptionMessage,
		InnerExceptionStackTrace,
		MachineName,
		Environment,
		LogLocalDate,
		LogUtcDate)
    VALUES (
		@LogId,
		@UserName,
		@UserAgent,
		@RemoteIpAddress,
		@ControllerName,
		@ActionName,
		@QueryString,
		@Headers,
		@RequestUrl,
		@HttpMethod,
		@RequestBody,
		@ExceptionType,
		@ExceptionMessage,
		@ExceptionStackTrace,
		@InnerExceptionMessage,
		@InnerExceptionStackTrace,
		@MachineName,
		@Environment,
		GETDATE(),
		GETUTCDATE()
		);
	SET NOCOUNT OFF;
END
GO