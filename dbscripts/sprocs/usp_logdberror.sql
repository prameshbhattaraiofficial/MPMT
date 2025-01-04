CREATE OR ALTER PROCEDURE [dbo].[usp_logdberror]
(
	@ErrorNumber INT = NULL,
	@ErrorMessage NVARCHAR(4000) = NULL,
    @ErrorSeverity INT = NULL,
    @ErrorState INT = NULL,
    @ErrorProcedure NVARCHAR(256) = NULL,
    @ErrorLine INT = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO [dbo].[tbl_dberrorlogs]
			   ([ErrorNumber]
			   ,[ErrorMessage]
			   ,[ErrorSeverity]
			   ,[ErrorState]
			   ,[ErrorLine]
			   ,[ErrorProcedure]
			   ,[CreatedLocalDate]
			   ,[CreatedUtcDate])
		 VALUES
			   (@ErrorNumber
			   ,@ErrorMessage
			   ,@ErrorSeverity
			   ,@ErrorState
			   ,@ErrorLine
			   ,@ErrorProcedure
			   ,GETDATE()
			   ,GETUTCDATE())

	SET NOCOUNT OFF
END
GO
