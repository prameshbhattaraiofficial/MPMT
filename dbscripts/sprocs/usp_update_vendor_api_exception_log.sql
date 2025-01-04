
CREATE OR ALTER PROCEDURE [dbo].[usp_update_vendor_api_exception_log]
(
	@LogId NVARCHAR(100) = NULL,
	@VendorException nvarchar(500) = NULL,
	@VendorExceptionStackTrace nvarchar(max) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE [dbo].[tbl_vendor_api_logs]
	SET
	VendorException = @VendorException
	,VendorExceptionStackTrace = @VendorExceptionStackTrace
	,UpdatedDate = GETDATE()
	WHERE LogId = @LogId;

	SET NOCOUNT OFF
END