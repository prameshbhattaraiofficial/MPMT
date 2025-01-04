
CREATE OR ALTER PROCEDURE [dbo].[usp_update_vendor_api_exception_log3]
(
	@LogId NVARCHAR(100) = NULL,
	@VendorException3 nvarchar(500) = NULL,
	@VendorExceptionStackTrace3 nvarchar(max) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE [dbo].[tbl_vendor_api_logs]
	SET
	VendorException3 = @VendorException3
	,VendorExceptionStackTrace3 = @VendorExceptionStackTrace3
	,UpdatedDate = GETDATE()
	WHERE LogId = @LogId;

	SET NOCOUNT OFF
END