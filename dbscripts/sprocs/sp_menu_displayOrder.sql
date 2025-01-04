SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

alter   PROCEDURE [dbo].[sp_menu_displayOrder]
	@Id INT,
	@DisplayOrder INT,
	@LoggedInUser varchar(100)=NULL,
	@IdentityVal INT = NULL OUTPUT,
    @StatusCode INT = NULL OUTPUT,
	@MsgType VARCHAR(10) = NULL OUTPUT,
	@MsgText VARCHAR(200) = NULL OUTPUT
AS
BEGIN
	-- RETURN 404, IF menu does not exists
	IF NOT EXISTS (SELECT M.[Id] FROM [dbo].[tbl_menu] M WITH(NOLOCK) WHERE M.[Id] = @Id)
	Begin
		Set @IdentityVal = 0
	Set @MsgText = 'Record Not Found!'	
	SET @StatusCode = 409;
	SET @MsgType = 'Error'
	Return
		End
		
	
	SET NOCOUNT ON
	UPDATE [dbo].[tbl_menu]
   SET 
        [DisplayOrder] = @DisplayOrder,
        [UpdatedBy] = @LoggedInUser,		[UpdatedLocalDate] = GETDATE(),		[UpdatedUtcDate] = GETUTCDATE(),		[UpdatedNepaliDate] = NULL
     
	WHERE [Id] = @Id;

		
	Set @IdentityVal = @Id 	Set @MsgText = 'Record Updated Successfully !'	Set @StatusCode = 200
	SET @MsgType = 'Sucess'
	Return
	SET NOCOUNT OFF
END
