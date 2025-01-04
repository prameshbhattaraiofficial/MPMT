
CREATE TABLE [dbo].[tbl_agent_api_logs](
	[LogId] [nvarchar](100) NOT NULL primary key default (newid()),
	[TransactionId] [nvarchar](50) NULL,
	[TrackerId] [nvarchar](50) NULL,
	[RequestInput] [nvarchar](max) NULL,
	[ResponseOutput] [nvarchar](max) NULL,
	[ResponseHttpStatus] [int] NULL,
	[RequestUrl] [nvarchar](500) NULL,
	[RequestHeaders] [nvarchar](max) NULL,
	[VendorRequestInput] [nvarchar](max) NULL,
	[VendorResponseOutput] [nvarchar](max) NULL,
	[VendorRequestURL] [nvarchar](500) NULL,
	[VendorRequestHeaders] [nvarchar](max) NULL,
	[VendorResponseHttpStatus] [int] NULL,
	[VendorResponseStatus] [bit] NULL,
	[VendorResponseState] [nvarchar](50) NULL,
	[VendorResponseMessage] [nvarchar](500) NULL,
	[VendorTransactionId] [nvarchar](50) NULL,
	[VendorTrackerId] [nvarchar](50) NULL,
	[VendorException] [nvarchar](500) NULL,
	[VendorExceptionStackTrace] [nvarchar](max) NULL,
	[VendorId] [nvarchar](50) NULL,
	[VendorType] [nvarchar](50) NULL,
	[ClientCode] [nvarchar](50) NULL,
	[PartnerCode] [nvarchar](50) NULL,
	[AgentCode] [nvarchar](50) NULL,
	[MemberId] [nvarchar](50) NULL,
	[MemberUserName] [nvarchar](100) NULL,
	[MemberName] [nvarchar](100) NULL,
	[DeviceCode] [nvarchar](500) NULL,
	[IpAddress] [nvarchar](150) NULL,
	[Platform] [nvarchar](100) NULL,
	[MachineName] [nvarchar](100) NULL,
	[Environment] [nvarchar](100) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[VendorRequestInput2] [nvarchar](max) NULL,
	[VendorResponseOutput2] [nvarchar](max) NULL,
	[VendorRequestURL2] [nvarchar](500) NULL,
	[VendorRequestHeaders2] [nvarchar](max) NULL,
	[VendorResponseHttpStatus2] [int] NULL,
	[VendorResponseStatus2] [bit] NULL,
	[VendorResponseState2] [nvarchar](50) NULL,
	[VendorResponseMessage2] [nvarchar](500) NULL,
	[VendorTransactionId2] [nvarchar](50) NULL,
	[VendorTrackerId2] [nvarchar](50) NULL,
	[VendorException2] [nvarchar](500) NULL,
	[VendorExceptionStackTrace2] [nvarchar](max) NULL,
	[VendorId2] [nvarchar](50) NULL,
	[VendorType2] [nvarchar](50) NULL,
	[VendorRequestInput3] [nvarchar](max) NULL,
	[VendorResponseOutput3] [nvarchar](max) NULL,
	[VendorRequestURL3] [nvarchar](500) NULL,
	[VendorRequestHeaders3] [nvarchar](max) NULL,
	[VendorResponseHttpStatus3] [int] NULL,
	[VendorResponseStatus3] [bit] NULL,
	[VendorResponseState3] [nvarchar](50) NULL,
	[VendorResponseMessage3] [nvarchar](500) NULL,
	[VendorTransactionId3] [nvarchar](50) NULL,
	[VendorTrackerId3] [nvarchar](50) NULL,
	[VendorException3] [nvarchar](500) NULL,
	[VendorExceptionStackTrace3] [nvarchar](max) NULL,
	[VendorId3] [nvarchar](50) NULL,
	[VendorType3] [nvarchar](50) NULL
)
GO

--------------------------------------------------------------------
--------------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[usp_insert_agent_api_log]
(
	@LogId NVARCHAR(100) = NULL,
	@TransactionId NVARCHAR(50) = NULL,
	@TrackerId nvarchar(50) =  NULL,
	@RequestInput NVARCHAR(MAX) = NULL,
	@ResponseOutput NVARCHAR(MAX) = NULL,
	@ResponseHttpStatus INT = NULL,
	@RequestUrl NVARCHAR(500) = NULL,
	@RequestHeaders NVARCHAR(MAX) = NULL,
	@VendorRequestInput NVARCHAR(MAX) = NULL,
	@VendorResponseOutput NVARCHAR(MAX) = NULL,
	@VendorRequestURL NVARCHAR(500) = NULL,
	@VendorRequestHeaders NVARCHAR(MAX) = NULL,
	@VendorResponseHttpStatus INT = NULL,
	@VendorResponseStatus BIT = NULL,
	@VendorResponseState NVARCHAR(50) = NULL,
	@VendorResponseMessage NVARCHAR(500) = NULL,
	@VendorTransactionId NVARCHAR(50) = NULL,
	@VendorTrackerId NVARCHAR(50) = NULL,
	@VendorException nvarchar(500) = NULL,
	@VendorExceptionStackTrace nvarchar(max) = NULL,
	@VendorId nvarchar(50) = NULL,
	@VendorType nvarchar(50) = NULL,
	@VendorRequestInput2 NVARCHAR(MAX) = NULL,
	@VendorResponseOutput2 NVARCHAR(MAX) = NULL,
	@VendorRequestURL2 NVARCHAR(500) = NULL,
	@VendorRequestHeaders2 NVARCHAR(MAX) = NULL,
	@VendorResponseHttpStatus2 INT = NULL,
	@VendorResponseStatus2 BIT = NULL,
	@VendorResponseState2 NVARCHAR(50) = NULL,
	@VendorResponseMessage2 NVARCHAR(500) = NULL,
	@VendorTransactionId2 NVARCHAR(50) = NULL,
	@VendorTrackerId2 NVARCHAR(50) = NULL,
	@VendorException2 nvarchar(500) = NULL,
	@VendorExceptionStackTrace2 nvarchar(max) = NULL,
	@VendorId2 NVARCHAR(50) = NULL,
	@VendorType2 NVARCHAR(50) = NULL,
	@VendorRequestInput3 NVARCHAR(MAX) = NULL,
	@VendorResponseOutput3 NVARCHAR(MAX) = NULL,
	@VendorRequestURL3 NVARCHAR(500) = NULL,
	@VendorRequestHeaders3 NVARCHAR(MAX) = NULL,
	@VendorResponseHttpStatus3 INT = NULL,
	@VendorResponseStatus3 BIT = NULL,
	@VendorResponseState3 NVARCHAR(50) = NULL,
	@VendorResponseMessage3 NVARCHAR(500) = NULL,
	@VendorTransactionId3 NVARCHAR(50) = NULL,
	@VendorTrackerId3 NVARCHAR(50) = NULL,
	@VendorException3 nvarchar(500) = NULL,
	@VendorExceptionStackTrace3 nvarchar(max) = NULL,
	@VendorId3 NVARCHAR(50) = NULL,
	@VendorType3 NVARCHAR(50) = NULL,
	@ClientCode nvarchar(50) = NULL,
	@PartnerCode nvarchar(50) = NULL,
	@AgentCode nvarchar(50) = NULL,
	@MemberId nvarchar(50) = NULL,
	@MemberUserName NVARCHAR(100) = NULL,
	@MemberName NVARCHAR(100) = NULL,
	@DeviceCode NVARCHAR(500) = NULL,
	@IpAddress NVARCHAR(150) = NULL,
	@Platform NVARCHAR(100) = NULL,	
	@MachineName NVARCHAR(100) = NULL,
	@Environment NVARCHAR(100) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	IF(ISNULL(@LogId,'') = '')
		SET @LogId = LOWER(NEWID());

	INSERT INTO [dbo].[tbl_agent_api_logs]
	(LogId,TransactionId,TrackerId,RequestInput,ResponseOutput,ResponseHttpStatus,RequestUrl,RequestHeaders
	,VendorRequestInput,VendorResponseOutput,VendorRequestUrl,VendorRequestHeaders,VendorResponseHttpStatus,VendorResponseState,VendorResponseMessage,VendorTransactionId
	,VendorTrackerId,VendorException,VendorExceptionStackTrace,VendorId,VendorType
	,VendorRequestInput2,VendorResponseOutput2,VendorRequestURL2,VendorRequestHeaders2,VendorResponseHttpStatus2,VendorResponseStatus2,VendorResponseState2,VendorResponseMessage2,VendorTransactionId2,
	VendorTrackerId2,VendorException2,VendorExceptionStackTrace2,VendorId2,VendorType2
	,VendorRequestInput3,VendorResponseOutput3,VendorRequestURL3,VendorRequestHeaders3,VendorResponseHttpStatus3,VendorResponseStatus3,VendorResponseState3,VendorResponseMessage3,VendorTransactionId3
	,VendorTrackerId3,VendorException3,VendorExceptionStackTrace3,VendorId3,VendorType3
	,ClientCode,PartnerCode,AgentCode,MemberId,MemberUserName,MemberName,DeviceCode,IpAddress,Platform,MachineName,Environment,CreatedDate
	)
	VALUES
	(@LogId,@TransactionId,@TrackerId,@RequestInput,@ResponseOutput,@ResponseHttpStatus,@RequestUrl,@RequestHeaders
	,@VendorRequestInput,@VendorResponseOutput,@VendorRequestUrl, @VendorRequestHeaders, @VendorResponseHttpStatus,@VendorResponseState,@VendorResponseMessage,@VendorTransactionId
	,@VendorTrackerId,@VendorException,@VendorExceptionStackTrace,@VendorId,@VendorType
	,@VendorRequestInput2,@VendorResponseOutput2,@VendorRequestURL2,@VendorRequestHeaders2,@VendorResponseHttpStatus2,@VendorResponseStatus2,@VendorResponseState2,@VendorResponseMessage2,@VendorTransactionId2
	,@VendorTrackerId2,@VendorException2,@VendorExceptionStackTrace2,@VendorId2,@VendorType2
	,@VendorRequestInput3,@VendorResponseOutput3,@VendorRequestURL3,@VendorRequestHeaders3,@VendorResponseHttpStatus3,@VendorResponseStatus3,@VendorResponseState3,@VendorResponseMessage3,@VendorTransactionId3
	,@VendorTrackerId3,@VendorException3,@VendorExceptionStackTrace3,@VendorId3,@VendorType3
	,@ClientCode,@PartnerCode,@AgentCode,@MemberId,@MemberUserName,@MemberName,@DeviceCode,@IpAddress,@Platform,@MachineName,@Environment,GETDATE()
	)

	SET NOCOUNT OFF
END
GO

--------------------------------------------------------------------
--------------------------------------------------------------------



CREATE OR ALTER PROCEDURE [dbo].[usp_update_agent_api_log]
(
	@LogId NVARCHAR(100) = NULL,
	@TransactionId NVARCHAR(50) = NULL,
	@TrackerId nvarchar(50) =  NULL,
	@RequestInput NVARCHAR(MAX) = NULL,
	@ResponseOutput NVARCHAR(MAX) = NULL,
	@ResponseHttpStatus INT = NULL,
	@RequestUrl NVARCHAR(500) = NULL,
	@RequestHeaders NVARCHAR(MAX) = NULL,
	@VendorRequestInput NVARCHAR(MAX) = NULL,
	@VendorResponseOutput NVARCHAR(MAX) = NULL,
	@VendorRequestURL NVARCHAR(500) = NULL,
	@VendorRequestHeaders NVARCHAR(MAX) = NULL,
	@VendorResponseHttpStatus INT = NULL,
	@VendorResponseStatus BIT = NULL,
	@VendorResponseState NVARCHAR(50) = NULL,
	@VendorResponseMessage NVARCHAR(500) = NULL,
	@VendorTransactionId NVARCHAR(50) = NULL,
	@VendorTrackerId NVARCHAR(50) = NULL,
	@VendorException nvarchar(500) = NULL,
	@VendorExceptionStackTrace nvarchar(max) = NULL,
	@VendorId nvarchar(50) = NULL,
	@VendorType nvarchar(50) = NULL,
	@VendorRequestInput2 NVARCHAR(MAX) = NULL,
	@VendorResponseOutput2 NVARCHAR(MAX) = NULL,
	@VendorRequestURL2 NVARCHAR(500) = NULL,
	@VendorRequestHeaders2 NVARCHAR(MAX) = NULL,
	@VendorResponseHttpStatus2 INT = NULL,
	@VendorResponseStatus2 BIT = NULL,
	@VendorResponseState2 NVARCHAR(50) = NULL,
	@VendorResponseMessage2 NVARCHAR(500) = NULL,
	@VendorTransactionId2 NVARCHAR(50) = NULL,
	@VendorTrackerId2 NVARCHAR(50) = NULL,
	@VendorException2 nvarchar(500) = NULL,
	@VendorExceptionStackTrace2 nvarchar(max) = NULL,
	@VendorId2 NVARCHAR(50) = NULL,
	@VendorType2 NVARCHAR(50) = NULL,
	@VendorRequestInput3 NVARCHAR(MAX) = NULL,
	@VendorResponseOutput3 NVARCHAR(MAX) = NULL,
	@VendorRequestURL3 NVARCHAR(500) = NULL,
	@VendorRequestHeaders3 NVARCHAR(MAX) = NULL,
	@VendorResponseHttpStatus3 INT = NULL,
	@VendorResponseStatus3 BIT = NULL,
	@VendorResponseState3 NVARCHAR(50) = NULL,
	@VendorResponseMessage3 NVARCHAR(500) = NULL,
	@VendorTransactionId3 NVARCHAR(50) = NULL,
	@VendorTrackerId3 NVARCHAR(50) = NULL,
	@VendorException3 nvarchar(500) = NULL,
	@VendorExceptionStackTrace3 nvarchar(max) = NULL,
	@VendorId3 NVARCHAR(50) = NULL,
	@VendorType3 NVARCHAR(50) = NULL,
	@ClientCode nvarchar(50) = NULL,
	@PartnerCode nvarchar(50) = NULL,
	@AgentCode nvarchar(50) = NULL,
	@MemberId nvarchar(50) = NULL,
	@MemberUserName NVARCHAR(100) = NULL,
	@MemberName NVARCHAR(100) = NULL,
	@DeviceCode NVARCHAR(500) = NULL,
	@IpAddress NVARCHAR(150) = NULL,
	@Platform NVARCHAR(100) = NULL,	
	@MachineName NVARCHAR(100) = NULL,
	@Environment NVARCHAR(100) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	IF(ISNULL(@LogId,'') = '')
		SET @LogId = LOWER(NEWID());

	UPDATE [dbo].[tbl_agent_api_logs]
		SET [TransactionId] = @TransactionId
		,[TrackerId] = @TrackerId
		,[RequestInput] = @RequestInput
		,[ResponseOutput] = @ResponseOutput
		,[ResponseHttpStatus] = @ResponseHttpStatus
		,[RequestUrl] = @RequestUrl
		,[RequestHeaders] = @RequestHeaders
		,[VendorRequestInput] = @VendorRequestInput
		,[VendorResponseOutput] = @VendorResponseOutput
		,[VendorRequestURL] = @VendorRequestURL
		,[VendorRequestHeaders] = @VendorRequestHeaders
		,[VendorResponseHttpStatus] = @VendorResponseHttpStatus
		,[VendorResponseStatus] = @VendorResponseStatus
		,[VendorResponseState] = @VendorResponseState
		,[VendorResponseMessage] = @VendorResponseMessage
		,[VendorTransactionId] = @VendorTransactionId
		,[VendorTrackerId] = @VendorTrackerId
		,[VendorException] = @VendorException
		,[VendorExceptionStackTrace] = @VendorExceptionStackTrace
		,[VendorId] = @VendorId
		,[VendorType] = @VendorType
		,[ClientCode] = @ClientCode
		,[PartnerCode] = @PartnerCode
		,[AgentCode] = @AgentCode
		,[MemberId] = @MemberId
		,[MemberUserName] = @MemberUserName
		,[MemberName] = @MemberName
		,[DeviceCode] = @DeviceCode
		,[IpAddress] = @IpAddress
		,[Platform] = @Platform
		,[MachineName] = @MachineName
		,[Environment] = @Environment
		,[UpdatedDate] = GETDATE()
		,[VendorRequestInput2] = @VendorRequestInput2
		,[VendorResponseOutput2] = @VendorResponseOutput2
		,[VendorRequestURL2] = @VendorRequestURL2
		,[VendorRequestHeaders2] = @VendorRequestHeaders2
		,[VendorResponseHttpStatus2] = @VendorResponseHttpStatus2
		,[VendorResponseStatus2] = @VendorResponseStatus2
		,[VendorResponseState2] = @VendorResponseState2
		,[VendorResponseMessage2] = @VendorResponseMessage2
		,[VendorTransactionId2] = @VendorTransactionId2
		,[VendorTrackerId2] = @VendorTrackerId2
		,[VendorException2] = @VendorException2
		,[VendorExceptionStackTrace2] = @VendorExceptionStackTrace2
		,[VendorId2] = @VendorId2
		,[VendorType2] = @VendorType2
		,[VendorRequestInput3] = @VendorRequestInput3
		,[VendorResponseOutput3] = @VendorResponseOutput3
		,[VendorRequestURL3] = @VendorRequestURL3
		,[VendorRequestHeaders3] = @VendorRequestHeaders3
		,[VendorResponseHttpStatus3] = @VendorResponseHttpStatus3
		,[VendorResponseStatus3] = @VendorResponseStatus3
		,[VendorResponseState3] = @VendorResponseState3
		,[VendorResponseMessage3] = @VendorResponseMessage3
		,[VendorTransactionId3] = @VendorTransactionId3
		,[VendorTrackerId3] = @VendorTrackerId3
		,[VendorException3] = @VendorException3
		,[VendorExceptionStackTrace3] = @VendorExceptionStackTrace3
		,[VendorId3] = @VendorId3
		,[VendorType3] = @VendorType3
	WHERE LogId = @LogId

	SET NOCOUNT OFF
END
GO

--------------------------------------------------------------------
--------------------------------------------------------------------


CREATE OR ALTER PROCEDURE [dbo].[usp_update_agent_api_response_log]
(
	@LogId NVARCHAR(100) = NULL,
	@TransactionId NVARCHAR(50) = NULL,
	@TrackerId nvarchar(50) =  NULL,
	@ResponseOutput NVARCHAR(MAX) = NULL,
	@ResponseHttpStatus INT = NULL,
	@VendorTransactionId NVARCHAR(50) = NULL,
	@VendorTrackerId NVARCHAR(50) = NULL,
	@VendorId NVARCHAR(50) = NULL,
	@VendorType NVARCHAR(50) = NULL,
	@VendorTransactionId2 NVARCHAR(50) = NULL,
	@VendorTrackerId2 NVARCHAR(50) = NULL,
	@VendorId2 NVARCHAR(50) = NULL,
	@VendorType2 NVARCHAR(50) = NULL,
	@VendorTransactionId3 NVARCHAR(50) = NULL,
	@VendorTrackerId3 NVARCHAR(50) = NULL,
	@VendorId3 NVARCHAR(50) = NULL,
	@VendorType3 NVARCHAR(50) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE [dbo].[tbl_agent_api_logs]
	SET
	TransactionId = @TransactionId
	,TrackerId = @TrackerId
	,ResponseOutput = @ResponseOutput
	,ResponseHttpStatus = @ResponseHttpStatus
	,VendorTransactionId = @VendorTransactionId
	,VendorTrackerId = @VendorTrackerId
	,VendorId = @VendorId
	,VendorType = @VendorType
	,VendorTransactionId2 = @VendorTransactionId2
	,VendorTrackerId2 = @VendorTrackerId2
	,VendorId2 = @VendorId2
	,VendorType2 = @VendorType2
	,VendorTransactionId3 = @VendorTransactionId3
	,VendorTrackerId3 = @VendorTrackerId3
	,VendorId3 = @VendorId3
	,VendorType3 = @VendorType3
	,UpdatedDate = GETDATE()
	WHERE LogId = @LogId;

	SET NOCOUNT OFF
END
GO


--------------------------------------------------------------------
--------------------------------------------------------------------


CREATE OR ALTER PROCEDURE [dbo].[usp_update_agent_api_exception_log]
(
	@LogId NVARCHAR(100) = NULL,
	@VendorException nvarchar(500) = NULL,
	@VendorExceptionStackTrace nvarchar(max) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE [dbo].[tbl_agent_api_logs]
	SET
	VendorException = @VendorException
	,VendorExceptionStackTrace = @VendorExceptionStackTrace
	,UpdatedDate = GETDATE()
	WHERE LogId = @LogId;

	SET NOCOUNT OFF
END
GO


--------------------------------------------------------------------
--------------------------------------------------------------------


CREATE OR ALTER PROCEDURE [dbo].[usp_update_agent_api_exception_log2]
(
	@LogId NVARCHAR(100) = NULL,
	@VendorException2 nvarchar(500) = NULL,
	@VendorExceptionStackTrace2 nvarchar(max) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE [dbo].[tbl_agent_api_logs]
	SET
	VendorException2 = @VendorException2
	,VendorExceptionStackTrace2 = @VendorExceptionStackTrace2
	,UpdatedDate = GETDATE()
	WHERE LogId = @LogId;

	SET NOCOUNT OFF
END
GO


--------------------------------------------------------------------
--------------------------------------------------------------------


CREATE OR ALTER PROCEDURE [dbo].[usp_update_agent_api_exception_log3]
(
	@LogId NVARCHAR(100) = NULL,
	@VendorException3 nvarchar(500) = NULL,
	@VendorExceptionStackTrace3 nvarchar(max) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE [dbo].[tbl_agent_api_logs]
	SET
	VendorException3 = @VendorException3
	,VendorExceptionStackTrace3 = @VendorExceptionStackTrace3
	,UpdatedDate = GETDATE()
	WHERE LogId = @LogId;

	SET NOCOUNT OFF
END
GO


--------------------------------------------------------------------
--------------------------------------------------------------------


CREATE OR ALTER PROCEDURE [dbo].[usp_update_agent_api_response_log]
(
	@LogId NVARCHAR(100) = NULL,
	@VendorRequestInput NVARCHAR(MAX) = NULL,
	@VendorResponseOutput NVARCHAR(MAX) = NULL,
	@VendorRequestURL NVARCHAR(500) = NULL,
	@VendorRequestHeaders NVARCHAR(MAX) = NULL,
	@VendorResponseHttpStatus INT = NULL,
	@VendorResponseStatus BIT = NULL,
	@VendorResponseState NVARCHAR(50) = NULL,
	@VendorResponseMessage NVARCHAR(500) = NULL,
	@VendorTransactionId NVARCHAR(50) = NULL,
	@VendorTrackerId NVARCHAR(50) = NULL,
	@VendorException nvarchar(500) = NULL,
	@VendorExceptionStackTrace nvarchar(max) = NULL,
	@VendorId NVARCHAR(50) = NULL,
	@VendorType NVARCHAR(50) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE [dbo].[tbl_agent_api_logs]
	SET
	VendorRequestInput = @VendorRequestInput
	,VendorResponseOutput = @VendorResponseOutput
	,VendorRequestURL = @VendorRequestURL
	,VendorRequestHeaders = @VendorRequestHeaders
	,VendorResponseHttpStatus = @VendorResponseHttpStatus
	,VendorResponseStatus = @VendorResponseStatus
	,VendorResponseState = @VendorResponseState
	,VendorResponseMessage = @VendorResponseMessage
	,VendorTransactionId = @VendorTransactionId
	,VendorTrackerId = @VendorTrackerId
	,VendorException = @VendorException
	,VendorExceptionStackTrace = @VendorExceptionStackTrace
	,VendorId = @VendorId
	,VendorType = @VendorType
	,UpdatedDate = GETDATE()
	WHERE LogId = @LogId;

	SET NOCOUNT OFF
END
GO

--------------------------------------------------------------------
--------------------------------------------------------------------



CREATE OR ALTER PROCEDURE [dbo].[usp_update_agent_api_response_log2]
(
	@LogId NVARCHAR(100) = NULL,
	@VendorRequestInput2 NVARCHAR(MAX) = NULL,
	@VendorResponseOutput2 NVARCHAR(MAX) = NULL,
	@VendorRequestURL2 NVARCHAR(500) = NULL,
	@VendorRequestHeaders2 NVARCHAR(MAX) = NULL,
	@VendorResponseHttpStatus2 INT = NULL,
	@VendorResponseStatus2 BIT = NULL,
	@VendorResponseState2 NVARCHAR(50) = NULL,
	@VendorResponseMessage2 NVARCHAR(500) = NULL,
	@VendorTransactionId2 NVARCHAR(50) = NULL,
	@VendorTrackerId2 NVARCHAR(50) = NULL,
	@VendorException2 nvarchar(500) = NULL,
	@VendorExceptionStackTrace2 nvarchar(max) = NULL,
	@VendorId2 NVARCHAR(50) = NULL,
	@VendorType2 NVARCHAR(50) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE [dbo].[tbl_agent_api_logs]
	SET
	VendorRequestInput2 = @VendorRequestInput2
	,VendorResponseOutput2 = @VendorResponseOutput2
	,VendorRequestURL2 = @VendorRequestURL2
	,VendorRequestHeaders2 = @VendorRequestHeaders2
	,VendorResponseHttpStatus2 = @VendorResponseHttpStatus2
	,VendorResponseStatus2 = @VendorResponseStatus2
	,VendorResponseState2 = @VendorResponseState2
	,VendorResponseMessage2 = @VendorResponseMessage2
	,VendorTransactionId2 = @VendorTransactionId2
	,VendorTrackerId2 = @VendorTrackerId2
	,VendorException2 = @VendorException2
	,VendorExceptionStackTrace2 = @VendorExceptionStackTrace2
	,VendorId2 = @VendorId2
	,VendorType2 = @VendorType2
	,UpdatedDate = GETDATE()
	WHERE LogId = @LogId;

	SET NOCOUNT OFF
END
GO

--------------------------------------------------------------------
--------------------------------------------------------------------


CREATE OR ALTER PROCEDURE [dbo].[usp_update_agent_api_response_log3]
(
	@LogId NVARCHAR(100) = NULL,
	@VendorRequestInput3 NVARCHAR(MAX) = NULL,
	@VendorResponseOutput3 NVARCHAR(MAX) = NULL,
	@VendorRequestURL3 NVARCHAR(500) = NULL,
	@VendorRequestHeaders3 NVARCHAR(MAX) = NULL,
	@VendorResponseHttpStatus3 INT = NULL,
	@VendorResponseStatus3 BIT = NULL,
	@VendorResponseState3 NVARCHAR(50) = NULL,
	@VendorResponseMessage3 NVARCHAR(500) = NULL,
	@VendorTransactionId3 NVARCHAR(50) = NULL,
	@VendorTrackerId3 NVARCHAR(50) = NULL,
	@VendorException3 nvarchar(500) = NULL,
	@VendorExceptionStackTrace3 nvarchar(max) = NULL,
	@VendorId3 NVARCHAR(50) = NULL,
	@VendorType3 NVARCHAR(50) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE [dbo].[tbl_agent_api_logs]
	SET
	VendorRequestInput3 = @VendorRequestInput3
	,VendorResponseOutput3 = @VendorResponseOutput3
	,VendorRequestURL3 = @VendorRequestURL3
	,VendorRequestHeaders3 = @VendorRequestHeaders3
	,VendorResponseHttpStatus3 = @VendorResponseHttpStatus3
	,VendorResponseStatus3 = @VendorResponseStatus3
	,VendorResponseState3 = @VendorResponseState3
	,VendorResponseMessage3 = @VendorResponseMessage3
	,VendorTransactionId3 = @VendorTransactionId3
	,VendorTrackerId3 = @VendorTrackerId3
	,VendorException3 = @VendorException3
	,VendorExceptionStackTrace3 = @VendorExceptionStackTrace3
	,VendorId3 = @VendorId3
	,VendorType3 = @VendorType3
	,UpdatedDate = GETDATE()
	WHERE LogId = @LogId;

	SET NOCOUNT OFF
END
GO
