USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


alter  PROCEDURE [dbo].[usp_Update_RemitPartnerStatus] --'REM3304621679',1
(
@PartnerCode as varchar(200),
@IsActive bit,
@LoggedInUser int=0,@userType varchar(100)=null,
@IdentityVal INT = NULL OUTPUT,
@StatusCode INT = NULL OUTPUT,
@MsgType VARCHAR(10) = NULL OUTPUT,
@MsgText VARCHAR(200) = NULL OUTPUT 
)
AS
BEGIN
SET @StatusCode = 400
	SET @MsgType = 'Error'
	SET @MsgText = 'Bad Request'
	SET @IdentityVal = 0	

DECLARE @ErrorNumber INT 
DECLARE @ErrorMessage NVARCHAR(4000)
DECLARE @ErrorSeverity INT
DECLARE @ErrorState INT
DECLARE @ErrorProcedure NVARCHAR(256)
DECLARE @ErrorLine INT
DECLARE @LoggedInUserId INT;
	IF (ISNULL(@PartnerCode, '') = '')
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Partner Code can not be null'
		RETURN
	END

	IF(ISNULL(@UserType,'') = '')
		SET @UserType = 'ADMIN'

	SET @LoggedInUserId = (SELECT [dbo].[func_get_userid](@UserType, @LoggedInUser))

 BEGIN TRY
 update tbl_remit_partners set IsActive=@IsActive,
			[UpdatedById] = @LoggedInUserId,			[UpdatedByName] = @LoggedInUser,			[UpdatedDate] = 	GETUTCDATE()
 where PartnerCode=@PartnerCode
		Set @IdentityVal = 1 	Set @MsgText = 'Record Updated Successfully !'	Set @StatusCode = 200
	SET @MsgType = 'Sucess'	RETURN

	END TRY
	BEGIN CATCH
		SET @ErrorNumber  = ERROR_NUMBER();
        SET @ErrorMessage  = ERROR_MESSAGE();
        SET @ErrorSeverity  = ERROR_SEVERITY();
        SET @ErrorState  = ERROR_STATE();
        SET @ErrorProcedure  = ERROR_PROCEDURE();
        SET @ErrorLine  = ERROR_LINE();

        EXEC [dbo].[usp_logdberror] @ErrorNumber, @ErrorMessage, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine;
		
		-- Re-throw the error to the calling application
        THROW;
	END CATCH
END
--select IsActive,PartnerCode,FirstName from tbl_remit_partners where PartnerCode='REM3304621679'