
CREATE OR ALTER PROCEDURE [dbo].[usp_get_role_by_name]
(
	@RoleName VARCHAR(100)
)
AS
BEGIN
	SET @RoleName = TRIM(@RoleName);

	IF (ISNULL(@RoleName, '') = '')
	RETURN

	SELECT [Id]
		  ,[RoleName]
		  ,[Description]
		  ,[IsSystemRole]
		  ,[IsActive]
		  ,[IsDeleted]
		  ,[CreatedBy]
		  ,[CreatedLocalDate]
		  ,[CreatedUtcDate]
		  ,[CreatedNepaliDate]
		  ,[UpdatedBy]
		  ,[UpdatedLocalDate]
		  ,[UpdatedUtcDate]
		  ,[UpdatedNepaliDate]
	  FROM [dbo].[tbl_roles]
	  WHERE UPPER(RoleName) = UPPER(@RoleName);
END
GO