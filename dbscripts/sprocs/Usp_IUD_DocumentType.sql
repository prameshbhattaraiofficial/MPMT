USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--- Created By		: Bibek Nepal


Create Proc [dbo].[Usp_IUD_DocumentType] (	@Event char(2) = 'I', 	@Id Int = 0, 	@DocumentType varchar(200),	@DocumentTypeCode varchar(20),	@IsExpireable bit,	@Remarks nvarchar(200),			@LoggedInUser int,	@IdentityVal INT = NULL OUTPUT,
    @StatusCode INT = NULL OUTPUT,
	@MsgType VARCHAR(10) = NULL OUTPUT,
	@MsgText VARCHAR(200) = NULL OUTPUT  
)
As
SET @StatusCode = 400
	SET @MsgType = 'Error'
	SET @MsgText = 'Bad Request'
	SET @IdentityVal = 0	

	SET @DocumentType = TRIM(@DocumentType)
	SET @DocumentTypeCode = TRIM(@DocumentTypeCode)
	
		DECLARE @ErrorNumber INT 
        DECLARE @ErrorMessage NVARCHAR(4000)
        DECLARE @ErrorSeverity INT
        DECLARE @ErrorState INT 
        DECLARE @ErrorProcedure NVARCHAR(256) 
        DECLARE @ErrorLine INT 


If @Event = 'I'  --for Insert
Begin 
	--------------------------------------------- Validation Begin ------------------------------------------------------
	IF (ISNULL(@DocumentType, '') = '')
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Document Type is required'
		RETURN
	END

		IF (ISNULL(@DocumentTypeCode, '') = '')
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Document Type Code is required'
		RETURN
	END

   --------------------------------------------- Validation End  -------------------------------------------------------
   BEGIN TRY
	Insert Into dbo.tbl_document_type(DocumentType,DocumentTypeCode,IsExpirable,Remarks,CreatedBy,CreatedLocalDate,CreatedUtcDate) Values 	(			@DocumentType,		@DocumentTypeCode,		@IsExpireable,		@Remarks,				@LoggedInUser,		GETDATE(),		GETUTCDATE()	)  
		Set @IdentityVal = @@IDENTITY
		Set @MsgText = 'Record Inserted Successfully !'	
		Set @StatusCode = 200
		SET @MsgType = 'Sucess'
	END TRY
	BEGIN CATCH
		Set @ErrorNumber  = ERROR_NUMBER();
        Set @ErrorMessage  = ERROR_MESSAGE();
        Set @ErrorSeverity  = ERROR_SEVERITY();
        Set @ErrorState  = ERROR_STATE();
        Set @ErrorProcedure  = ERROR_PROCEDURE();
        Set @ErrorLine  = ERROR_LINE();

        EXEC [dbo].[usp_logdberror] @ErrorNumber, @ErrorMessage, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine;
		
		-- Re-throw the error to the calling application
        THROW;
	END CATCH

End 
Else
If @Event = 'U'    --- Update
Begin
	--------------------------------------------- Validation Begin ------------------------------------------------------
	IF (ISNULL(@Id, 0) = 0)
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Id is required'
		RETURN
	END
	IF (ISNULL(@DocumentType, '') = '')
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Document Type is required'
		RETURN
	END

		IF (ISNULL(@DocumentTypeCode, '') = '')
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Document Type Code is required'
		RETURN
	END

   --------------------------------------------- Validation End  -------------------------------------------------------
   BEGIN TRY
	Update dbo.tbl_document_type Set 		DocumentType = @DocumentType,		DocumentTypeCode = @DocumentTypeCode,		IsExpirable=@IsExpireable,		Remarks = @Remarks,				UpdatedBy = @LoggedInUser,		UpdatedLocalDate = GetDate(),		UpdatedUtcDate = 	GETUTCDATE()	 Where [Id] = @Id	Set @IdentityVal = @Id 	Set @MsgText = 'Record Updated Successfully !'	Set @StatusCode = 200
	SET @MsgType = 'Sucess'		END TRY
	BEGIN CATCH
		Set @ErrorNumber  = ERROR_NUMBER();
        Set @ErrorMessage  = ERROR_MESSAGE();
        Set @ErrorSeverity  = ERROR_SEVERITY();
        Set @ErrorState  = ERROR_STATE();
        Set @ErrorProcedure  = ERROR_PROCEDURE();
        Set @ErrorLine  = ERROR_LINE();

        EXEC [dbo].[usp_logdberror] @ErrorNumber, @ErrorMessage, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine;
		
		-- Re-throw the error to the calling application
        THROW;
	END CATCHEnd 
Else
If @Event = 'D'   -- For Delete 
Begin 

--------------------------------------------- Validation Begin ------------------------------------------------------
IF (ISNULL(@Id, 0) = 0)
	BEGIN
		SET @StatusCode = 400
		SET @MsgText = 'Id is required'
		RETURN
	END
--------------------------------------------- Validation End  -------------------------------------------------------
	
	Delete from dbo.tbl_document_type 	Where [Id] = @Id 	Set @IdentityVal = @Id 	Set @MsgText = 'Record Deleted Successfully !'
	Set @StatusCode = 200
	SET @MsgType = 'Sucess'
End 