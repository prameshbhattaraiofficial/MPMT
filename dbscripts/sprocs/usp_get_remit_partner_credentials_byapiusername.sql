CREATE OR ALTER PROC [dbo].[usp_get_remit_partner_credentials_byapiusername]
(
	@ApiUserName NVARCHAR(100)
)
AS
BEGIN
	SET @ApiUserName = TRIM(ISNULL(@ApiUserName, ''))
	IF (@ApiUserName = '')
		RETURN
END
GO