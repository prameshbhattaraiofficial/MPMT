
CREATE OR ALTER PROCEDURE [dbo].[usp_iud_partner_application]
(
	@Operation NVARCHAR(1) = NULL
	,@FirstName NVARCHAR(128) = NULL
	,@LastName NVARCHAR(128) = NULL
	,@OrganizationName NVARCHAR(128) = NULL
	,@OrganizationEmail NVARCHAR(320) = NULL
	,@OrganizationContactNo NVARCHAR(20) = NULL
	,@Designation NVARCHAR(50) = NULL
	,@CountryCode NVARCHAR(10) = NULL
	,@Address NVARCHAR(320) = NULL
	,@Message NVARCHAR(2048) = NULL
	,@IpAddress NVARCHAR(50) = NULL
	,@ReturnPrimaryId INT = NULL OUTPUT
	,@StatusCode INT = NULL OUTPUT
	,@MsgType NVARCHAR(10) = NULL OUTPUT
	,@MsgText NVARCHAR(200) = NULL OUTPUT
)
AS
BEGIN
	SET @StatusCode = 400          
	SET @MsgType = 'Error'          
	SET @MsgText = 'Bad Request'          
	SET @ReturnPrimaryId = 0

	SET @Operation = UPPER(ISNULL(@Operation, ''))
	IF @Operation = '' OR @Operation NOT IN ('I', 'U', 'D')
	BEGIN
		SET @MsgText = 'Invalid Operation.'
		RETURN
	END

	IF ISNULL(@FirstName, '') = ''
	BEGIN
		SET @MsgText = 'FirstName is required.'
		RETURN
	END

	IF ISNULL(@LastName, '') = ''
	BEGIN
		SET @MsgText = 'LastName is required.'
		RETURN
	END

	IF ISNULL(@OrganizationName, '') = ''
	BEGIN
		SET @MsgText = 'OrganizationName is required.'
		RETURN
	END

	IF ISNULL(@OrganizationEmail, '') = ''
	BEGIN
		SET @MsgText = 'OrganizationEmail is required.'
		RETURN
	END

	IF ISNULL(@OrganizationContactNo, '') = ''
	BEGIN
		SET @MsgText = 'OrganizationContactNo is required.'
		RETURN
	END

	IF ISNULL(@OrganizationContactNo, '') = ''
	BEGIN
		SET @MsgText = 'OrganizationContactNo is required.'
		RETURN
	END

	IF ISNULL(@CountryCode, '') = ''
	BEGIN
		SET @MsgText = 'Country is required.'
		RETURN
	END

	IF ISNULL(@Address, '') = ''
	BEGIN
		SET @MsgText = 'Address is required.'
		RETURN
	END

	IF @Operation = 'I'
	BEGIN
		SET NOCOUNT ON
		INSERT INTO [dbo].[tbl_partner_applications]
				   ([FirstName]
				   ,[LastName]
				   ,[OrganizationName]
				   ,[OrganizationEmail]
				   ,[OrganizationContactNo]
				   ,[Designation]
				   ,[CountryCode]
				   ,[Address]
				   ,[Message]
				   ,[IpAddress]
				   ,[CreatedLocalDate])
			 VALUES
				   (@FirstName
				   ,@LastName
				   ,@OrganizationName
				   ,@OrganizationEmail
				   ,@OrganizationContactNo
				   ,@Designation
				   ,@CountryCode
				   ,@Address
				   ,@Message
				   ,@IpAddress
				   ,GETDATE())
		SET NOCOUNT OFF

		SET @ReturnPrimaryId = SCOPE_IDENTITY()
		SET @StatusCode = 200
		SET @MsgType = 'Success'
		SET @MsgText = 'Application submission successful!'
		RETURN
	END

	SET @StatusCode = 200
	SET @MsgType = 'Success'
	SET @MsgText = 'Success!'
	RETURN
END
GO