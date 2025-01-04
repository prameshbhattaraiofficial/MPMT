USE [MpmtDb]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


alter   Proc [dbo].[usp_remit_partners_register] --0,'I',1,'123456','REM0077493','Kusal','Burtel',1,'9842032671',0,'yash@gmail.com',0,'yash',1,'biratnage','yash adhikar','test','test','122'(	@Id Int = 0, 	@Event char(2) = 'I', 	@FormNumber int = 1,	@OTP nvarchar(20)=0,	@PartnerCode varchar(20) ='',	@FirstName nvarchar(100)='',	@SurName nvarchar(100)='',	@Withoutfirstname bit = 1,	@MobileNumber nvarchar(30)='',	@MobileConfirmed bit = 0,	@Email nvarchar(200)='',	@EmailConfirmed bit = 0,	@Post nvarchar(100) = '',
	@BusinessNumber nvarchar(100) ='',
	@FinancialTransactionRegNo nvarchar(100) = '',
	@RemittancRegNumber nvarchar(100) = '',
	@LicensedocImgPath nvarchar(100) = '',
	@LicenseNumber nvarchar(100) = '',
	@ZipCode nvarchar(20) = '',
	@OrgState nvarchar(100) ='',	@Address nvarchar(500) = '',	@PasswordHash nvarchar(512),	@PasswordSalt nvarchar(Max),		@OrganizationName nvarchar(256) = '',	@OrgEmail nvarchar(150)= '',	@OrgEmailConfirmed bit = 0,	@CountryCode varchar(10) = '',	@Callingcode  varchar(10) = '',	@City nvarchar(150)= '',	@FullAddress nvarchar(300)= '',	@GMTTimeZone nvarchar(50)= '',	@RegistrationNumber nvarchar(150)= '',	@SourceCurrency nvarchar(3)= '',	@IpAddress nvarchar(50)= '',	@CompanyLogoImgPath nvarchar(512)= '',	@DocumentTypeId int = 0,	@DocumentNumber nvarchar(150) = '',	@IdFrontImgPath nvarchar(512) = '',	@IdBackImgPath nvarchar(512) = '',	@ExpiryDate datetime2 = '',	@AddressProofTypeId int = 0,	@AddressProofImgPath nvarchar(512) = '',		@IsActive bit = 0,		@Maker bit = 0,	@Checker bit = 0,	@CreatedById int = 0,	@CreatedByName varchar(100)= '',	@CreatedDate datetime2 = '',	@UpdatedById int = 0,	@UpdatedByName varchar(100)= 0,	@UpdatedDate datetime2 = '',	@Directors PartnersDirectorsType READONLY,	@IdentityVal INT = NULL OUTPUT,
    @StatusCode INT = NULL OUTPUT,
	@MsgType VARCHAR(10) = NULL OUTPUT,
	@MsgText VARCHAR(200) = NULL OUTPUT 
	)
As
BEGIN
DECLARE @ErrorNumber INT 
DECLARE @ErrorMessage NVARCHAR(4000)
DECLARE @ErrorSeverity INT
DECLARE @ErrorState INT
DECLARE @ErrorProcedure NVARCHAR(256)
DECLARE @ErrorLine INT

SET @StatusCode = 400
	SET @MsgType = 'Error'
	SET @MsgText = 'Bad Request'
	SET @IdentityVal = 0

	declare @FundTypeId int;


 IF @Event='I'
BEGIN
 declare @sRandom nvarchar(100);
EXEC usp_generate_random_numeric_string 10, @sRandom OUTPUT;

WHILE (EXISTS (SELECT 1 FROM tbl_remit_partners WHERE PartnerCode =('REM'+ @sRandom)))
 BEGIN
    EXEC usp_generate_random_numeric_string 10, @sRandom OUTPUT;
 END

SET @PartnerCode = 'REM' + @sRandom;
END



-----Start Validation Validation-----
IF (@FormNumber = 1)
BEGIN
    -- Check if the first name is required and not provided
    IF (@Withoutfirstname = 0)
    BEGIN
        IF (ISNULL(@FirstName, '') = '')
        BEGIN
            SET @StatusCode = 400
            SET @MsgText = 'First Name is required'
            RETURN
        END
    END

    -- Check if the last name is provided
    IF (ISNULL(@SurName, '') = '')
    BEGIN
        SET @StatusCode = 400
        SET @MsgText = 'Last Name is required'
        RETURN
    END

    -- Check if the email is provided
    IF (ISNULL(@Email, '') = '')
    BEGIN
        SET @StatusCode = 400
        SET @MsgText = 'Email is required'
        RETURN
    END

    -- Check if the phone number is provided
    IF (ISNULL(@MobileNumber, '') = '')
    BEGIN
        SET @StatusCode = 400
        SET @MsgText = 'Phone number is required'
        RETURN
    END

    -- Check if the position is provided
    IF (ISNULL(@Post, '') = '')
    BEGIN
        SET @StatusCode = 400
        SET @MsgText = 'Position is required'
        RETURN
    END

    -- Check if the password is provided
    IF (ISNULL(@PasswordHash, '') = '')
    BEGIN
        SET @StatusCode = 400
        SET @MsgText = 'Password is required'
        RETURN
    END
END

IF(@FormNumber=2)
BEGIN
DECLARE @RowCount INT;
SET @RowCount = (SELECT COUNT(*) FROM @Directors);
DECLARE @Counter INT = 1;

WHILE @Counter <= @RowCount
BEGIN
    DECLARE @DFirstName NVARCHAR(50);
    DECLARE @DContactNumber NVARCHAR(50);
    DECLARE @DEmail NVARCHAR(50);
    
    SELECT @DFirstName = FirstName, @DContactNumber = ContactNumber, @DEmail = Email
    FROM @Directors   
    
    -- Perform validation checks for the current row
    IF (ISNULL(@DFirstName, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'First Name is required';
        RETURN;
    END
    
    IF (ISNULL(@DContactNumber, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Contact Number is required';
        RETURN;
    END
    
    IF (ISNULL(@DEmail, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Email is required';
        RETURN;
    END

    -- Increment the counter for the next iteration
    SET @Counter = @Counter + 1;
END
END

IF(@FormNumber=3)
BEGIN
IF (ISNULL(@OrganizationName, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Organization Name is required';
        RETURN;
    END

	IF (ISNULL(@RegistrationNumber, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Regestration Number is required';
        RETURN;
    END

	IF (ISNULL(@BusinessNumber, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Business Number is required';
        RETURN;
    END

	IF (ISNULL(@FinancialTransactionRegNo, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Financial Transaction Reg. No. is required';
        RETURN;
    END

	
	IF (ISNULL(@LicenseNumber, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'License  number is required';
        RETURN;
    END

		IF (ISNULL(@RemittancRegNumber, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Remittance Reg. Number is required';
        RETURN;
    END
		IF (ISNULL(@OrgEmail, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Organization Email is required';
        RETURN;
    END

	IF (ISNULL(@CountryCode, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Country is Required';
        RETURN;
    END

	IF NOT EXISTS(SELECT 1 FROM tbl_country WHERE CountryCode = @CountryCode )
	BEGIN
		SET @MsgText = 'Invalid Countrycode Id'     
		RETURN; 
	END
	IF (ISNULL(@OrgState, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Org State is Required';
        RETURN;
    END

		IF (ISNULL(@City, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Org city is Required';
        RETURN;
    END
		IF (ISNULL(@Address, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Org Address is Required';
        RETURN;
    END
	IF (ISNULL(@ZipCode, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Zip Code is Required';
        RETURN;
    END
		IF (ISNULL(@GMTTimeZone, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Zip Code is Required';
        RETURN;
    END

			IF (ISNULL(@SourceCurrency, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Source Currency is Required';
        RETURN;
    END
	IF NOT EXISTS(SELECT * FROM tbl_currency WHERE ShortName = @SourceCurrency )
	BEGIN
		SET @MsgText = 'Invalid Source Currency'     
		RETURN; 
	END

	IF (ISNULL(@CompanyLogoImgPath, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Logo Image  is Required';
        RETURN;
    END
	IF (ISNULL(@LicensedocImgPath, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'License Image  is Required';
        RETURN;
    END

END

IF(@FormNumber=4)
BEGIN

	IF(ISNULL(@DocumentNumber, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'Document Number is required';
        RETURN;
    END

	IF(ISNULL(@IdFrontImgPath, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'ID Front Image is required';
        RETURN;
    END
	IF(ISNULL(@IdBackImgPath, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'ID Back Image is required';
        RETURN;
    END

	 IF NOT EXISTS(SELECT 1 FROM tbl_addressproof_type WHERE Id = @AddressProofTypeId )
  BEGIN
	SET @MsgText = 'Invalid Address Proof Type Id'    
    RETURN; 
  END

  IF(ISNULL(@AddressProofImgPath, '') = '')
    BEGIN
        SET @StatusCode = 400;
        SET @MsgText = 'ID Back Image is required';
        RETURN;
    END

   IF NOT EXISTS(SELECT 1 FROM tbl_document_type WHERE Id = @DocumentTypeId )
  BEGIN
	SET @MsgText = 'Invalid DocumentType Id'    
    RETURN; 
  END

END

-----END OF Validation-------




If (@Event = 'I' and @FormNumber=1 ) --for Insert
Begin 
    BEGIN TRY
	--------------------------------------------- Validation End  -------------------------------------------------------
	INSERT INTO [dbo].[tbl_remit_partners_register]
           ([PartnerCode]
           ,[FirstName]
           ,[SurName]
           ,[Withoutfirstname]
           ,[MobileNumber]
           ,[MobileConfirmed]
           ,[Email]
           ,[EmailConfirmed]
           ,[Post]         
           ,[PasswordHash]
           ,[PasswordSalt]
		   ,[Callingcode]
           ,[Maker]
           ,[Checker]
           ,[CreatedById]
           ,[CreatedByName]
           ,[CreatedDate]
           ,[UpdatedById]
           ,[UpdatedByName]
           ,[UpdatedDate])
     VALUES 	(			@PartnerCode,		@FirstName,		@SurName,		@Withoutfirstname,		@MobileNumber,		@MobileConfirmed,		@Email,		@EmailConfirmed,		@Post,		@PasswordHash,		@PasswordSalt,		@Callingcode,		@Maker,		@Checker,		@CreatedById,		@CreatedByName,		GETUTCDATE(),		NULL,		NULL,		NULL	) 
	Set @IdentityVal = @@IDENTITY
	
	Set @MsgText = 'Record Inserted Successfully !'	
	Set @StatusCode = 200
	SET @MsgType = 'Sucess'
	
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
End


  --- Update
If (@Event = 'U' and @FormNumber=2 )
Begin

BEGIN TRY
	INSERT INTO [dbo].[tbl_partner_directors] (DirectorId,PartnerCode, FirstName, ContactNumber, Email)
	SELECT  NEWID(), @PartnerCode, FirstName, ContactNumber, Email FROM @Directors;

	
	Set @MsgText = 'Record Inserted Successfully !'	
	Set @StatusCode = 200
	SET @MsgType = 'Sucess'
	
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

End


If (@Event = 'U' and @FormNumber=3)  
Begin

   BEGIN TRY
	Update dbo.tbl_remit_partners_register Set 		--[PartnerCode] = @PartnerCode,			[OrganizationName] = @OrganizationName,		[OrgEmail] = @OrgEmail,		[EmailConfirmed]=1,		[OrgEmailConfirmed] = 0,		[CountryCode] = @CountryCode,		[City] = @City,		[FullAddress] = @FullAddress,		[GMTTimeZone] = @GMTTimeZone,				[SourceCurrency] = @SourceCurrency,			[IpAddress] = @IpAddress,		[CompanyLogoImgPath] = @CompanyLogoImgPath,		[LicensedocImgPath] = @LicensedocImgPath,		[LicenseNumber] = @LicenseNumber,		[RegistrationNumber] = @RegistrationNumber,		[FinancialTransactionRegNo] = @FinancialTransactionRegNo,
		[RemittancRegNumber] = @RemittancRegNumber,
		[BusinessNumber] = @BusinessNumber,
		[ZipCode] = @ZipCode,
		[OrgState] = @OrgState,		[Maker] = 1,		[Checker] = 0,				[UpdatedById] = @UpdatedById,		[UpdatedByName] = @UpdatedByName,		[UpdatedDate] = GETUTCDATE()	 Where Email = @Email	Set @IdentityVal = @Id 	Set @MsgText = 'Record Updated Successfully !'	Set @StatusCode = 200
	SET @MsgType = 'Sucess'
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
	End 


If (@Event = 'U' and @FormNumber=4)  
Begin

   BEGIN TRY
	Update dbo.tbl_remit_partners_register Set 	
		[DocumentTypeId] = @DocumentTypeId,		[DocumentNumber] = @DocumentNumber,		[IdFrontImgPath] = @IdFrontImgPath,		[IdBackImgPath] = @IdBackImgPath,		[ExpiryDate] = @ExpiryDate,		[AddressProofTypeId] = @AddressProofTypeId,		[AddressProofImgPath] = @AddressProofImgPath,		[IsActive] = 1,		[Maker] = 1,		[Checker] = 0,				[UpdatedById] = @UpdatedById,		[UpdatedByName] = @UpdatedByName,		[UpdatedDate] = GETUTCDATE()	 Where Email = @Email	Set @IdentityVal = @Id 	Set @MsgText = 'Record Updated Successfully !'	Set @StatusCode = 200
	SET @MsgType = 'Sucess'
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
	End 

END
