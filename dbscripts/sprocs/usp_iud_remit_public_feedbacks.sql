CREATE OR ALTER PROCEDURE [dbo].[usp_iud_remit_public_feedbacks]
(
	@Operation NVARCHAR(1) = NULL
	,@FullName NVARCHAR(128) = NULL
	,@Email NVARCHAR(320) = NULL
	,@ContactNo NVARCHAR(20) = NULL
	,@Subject NVARCHAR(320) = NULL
	,@Message NVARCHAR(2048) = NULL
	,@IpAddress NVARCHAR(50) = NULL
	,@IsReviewed BIT = 0
	,@UpdatedBy NVARCHAR(50) = NULL
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

	IF ISNULL(@FullName, '') = ''
	BEGIN
		SET @MsgText = 'FullName is required.'
		RETURN
	END

	IF ISNULL(@Email, '') = ''
	BEGIN
		SET @MsgText = 'Email is required.'
		RETURN
	END

	IF ISNULL(@ContactNo, '') = ''
	BEGIN
		SET @MsgText = 'ContactNo is required.'
		RETURN
	END

	IF ISNULL(@Subject, '') = ''
	BEGIN
		SET @MsgText = 'Subject is required.'
		RETURN
	END

	IF ISNULL(@Message, '') = ''
	BEGIN
		SET @MsgText = 'Message is required.'
		RETURN
	END

	IF @Operation = 'I'
	BEGIN
		SET NOCOUNT ON
		INSERT INTO [dbo].[tbl_remit_public_feedbacks]
				   ([FullName]
				   ,[Email]
				   ,[ContactNo]
				   ,[Subject]
				   ,[Message]
				   ,[IpAddress]
				   ,[IsReviewed]
				   ,[ReviewedLocalDate]
				   ,[UpdatedBy]
				   ,[CreatedLocalDate]
				   ,[UpdatedLocalDate])
			 VALUES
				   (@FullName
				   ,@Email
				   ,@ContactNo
				   ,@Subject
				   ,@Message
				   ,@IpAddress
				   ,@IsReviewed
				   ,NULL
				   ,NULL
				   ,GETDATE()
				   ,NULL)
		SET NOCOUNT OFF

		SET @ReturnPrimaryId = SCOPE_IDENTITY()
		SET @StatusCode = 200
		SET @MsgType = 'Success'
		SET @MsgText = 'Message sent successfully!'
		RETURN
	END

	SET @StatusCode = 200
	SET @MsgType = 'Success'
	SET @MsgText = 'Success!'
	RETURN
END
GO