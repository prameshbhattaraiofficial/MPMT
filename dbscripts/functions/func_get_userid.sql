-- =============================================
-- Author:		<SAROJ KUMAR CHAUDAHRY>
-- Create date: <2023-AUG-20>
-- Description:	<Returns Local date from GMT time offset>
-- Execution: select [dbo].[func_get_userid]('ADMIN','admin')
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[func_get_userid] (@UserType VARCHAR(20), @UserName VARCHAR(100))
RETURNS INT
AS
BEGIN

	DECLARE @UserId INT;
	SET @UserName = TRIM(ISNULL(@UserName,''))
	SET @UserType = UPPER(TRIM(ISNULL(@UserType,'ADMIN')))
	
	IF(TRIM(@UserType) = 'ADMIN')
	BEGIN
	SET @UserId = (SELECT Id FROM dbo.tbl_users WITH(NOLOCK) WHERE IsActive=1 AND UserName=@UserName)
	END
	ELSE IF(TRIM(@UserType) = 'PARTNER')
	BEGIN
	SET @UserId = (SELECT Id FROM dbo.tbl_remit_partners WITH(NOLOCK) WHERE IsActive=1 AND UserName=@UserName)
	END

	RETURN @UserId;
	
END;
GO


