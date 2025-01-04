CREATE OR ALTER PROCEDURE [dbo].[usp_check_user_exists_by_username]
(
	@UserName [nvarchar](100)
)
AS
BEGIN
	SELECT CASE
		WHEN (ISNULL(TRIM(@Username), '') <> '' AND EXISTS (SELECT 1 FROM [dbo].[tbl_users] WITH (NOLOCK) WHERE UPPER(UserName) = UPPER(@UserName))) THEN 1
		ELSE 0
	END;
END
GO
