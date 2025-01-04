CREATE OR ALTER PROCEDURE [dbo].[usp_check_user_exists_by_email]
(
	@Email [nvarchar](200)
)
AS
BEGIN
	SELECT CASE
		WHEN (ISNULL(TRIM(@Email), '') <> '' AND EXISTS (SELECT 1 FROM [dbo].[tbl_users] WITH (NOLOCK) WHERE UPPER(Email) = UPPER(@Email))) THEN 1
		ELSE 0
	END;
END
GO
