
CREATE OR ALTER PROCEDURE [dbo].[usp_get_sender_list]
(
	@FirstName nvarchar(100) = NULL
	,@SurName nvarchar(100) = NULL
	,@MobileNumber nvarchar(20) = NULL
	,@Email nvarchar(150) = NULL
	,@UserStatus int = 0
	,@PageNumber INT = 1
	,@PageSize INT = 20
	,@SortingCol AS VARCHAR(100) ='CreatedDate'
	,@SortType AS VARCHAR(100) = 'ASC'
	,@SearchVal NVARCHAR(300) =''
	,@Export INT = 0
)
AS
BEGIN
	SET NOCOUNT ON;  
	DBCC FREEPROCCACHE; 

	DECLARE @FilteredCount INT, @TotalCount INT, @TotalPages INT, @PageIndex INT;

	IF(ISNULL(@Export,0) = 1)
		SET @Export = 1;
	ELSE
		SET @Export = 0;

	IF(ISNULL(@PageSize, 0) <= 0)
		SET @PageSize=20;

	IF(ISNULL(@PageSize, 0) > 100)
		SET @PageSize=100;
	
	IF(ISNULL(@PageNumber, 0) <= 0)
		SET @PageNumber = 1;

	IF(ISNULL(@SortingCol,'') = '')
	BEGIN
		SET @SortingCol = 'CreatedDate'
	END
	IF(ISNULL(@SortType,'') = '')
	BEGIN
		SET @SortType = 'ASC'
	END

	
IF(@Export = 0)  ------DO PAGINATION
BEGIN
	----===================TOTAL COUNT =========================
	SELECT @TotalCount = COUNT(1) FROM [dbo].tbl_senders S WITH(NOLOCK)	
	WHERE  ISNULL(FirstName,'') LIKE ISNULL(REPLACE(@FirstName,' ',''),'') + '%' 
		AND ISNULL(S.SurName,'') LIKE ISNULL(@SurName,'') + '%'
		AND ISNULL(S.MobileNumber,'') LIKE ISNULL(@MobileNumber,'') + '%'
		AND ISNULL(S.Email,'') LIKE ISNULL(@Email,'') + '%'


	----===================FILTERED COUNT =========================
	SELECT @FilteredCount = COUNT(1) FROM [dbo].tbl_senders S WITH(NOLOCK)
	WHERE  ISNULL(FirstName,'') LIKE ISNULL(REPLACE(@FirstName,' ',''),'') + '%' 
		AND ISNULL(S.SurName,'') LIKE ISNULL(@SurName,'') + '%'
		AND ISNULL(S.MobileNumber,'') LIKE ISNULL(@MobileNumber,'') + '%'
		AND ISNULL(S.Email,'') LIKE ISNULL(@Email,'') + '%'  
		AND (
			CONCAT(ISNULL(S.FirstName, ''), ' ', ISNULL(S.SurName, '')) LIKE '%'+ISNULL(@SearchVal,'')+'%'
			OR S.MobileNumber LIKE '%'+ISNULL(@SearchVal,'')+'%' 
			OR S.Email LIKE '%'+ISNULL(@SearchVal,'')+'%'		
			)

----=========== GET FILTERED RESULT SET WITH PAGINATION ===========	  
	SELECT [Id]
		  ,ROW_NUMBER() OVER (order by (select 1)) AS SN
		  ,[PartnerCode]
		  ,[MemberId]
		  ,[FirstName]
		  ,[SurName]
		  ,[IsSurNamePresent]
		  ,[MobileNumber]
		  ,[Email]
		  ,[GenderId]
		  ,[DateOfBirth]
		  ,[ProfileImagePath]
		  ,[CountryCode]
		  ,[Province]
		  ,[City]
		  ,[Zipcode]
		  ,[Address]
		  ,[OccupationId]
		  ,[DocumentTypeId]
		  ,[DocumentNumber]
		  ,[IssuedDate]
		  ,[ExpiryDate]
		  ,[IdFrontImgPath]
		  ,[IdBackImgPath]
		  ,[BankName]
		  ,[BankCode]
		  ,[Branch]
		  ,[AccountHolderName]
		  ,[AccountNumber]
		  ,[IncomeSourceId]
		  ,[GMTTimeZone]
		  ,[IsActive]
		  ,[IsDeleted]
		  ,[IsBlocked]
		  ,[KycStatusCode]
		  ,[Is2FAAuthenticated]
		  ,[AccountSecretKey]
		  ,[CreatedUserType]
		  ,[CreatedById]
		  ,[CreatedByName]
		  ,[CreatedDate]
		  ,[UpdatedUserType]
		  ,[UpdatedById]
		  ,[UpdatedByName]
		  ,[UpdatedDate]
	FROM [dbo].[tbl_senders] S WITH(NOLOCK)
	WHERE  ISNULL(FirstName,'') LIKE ISNULL(REPLACE(@FirstName,' ',''),'') + '%' 
		AND ISNULL(S.SurName,'') LIKE ISNULL(@SurName,'') + '%'
		AND ISNULL(S.MobileNumber,'') LIKE ISNULL(@MobileNumber,'') + '%'
		AND ISNULL(S.Email,'') LIKE ISNULL(@Email,'') + '%'
		AND (
			CASE 
				WHEN @UserStatus = 0 AND IsActive IN (0, 1) THEN 1
				WHEN @UserStatus = 1 AND IsActive =  1 THEN 1
				WHEN @UserStatus = 2 AND IsActive = 0 THEN 1
				ELSE 0
			END) = 1
		AND (
			CONCAT(ISNULL(S.FirstName, ''), ' ', ISNULL(S.SurName, '')) LIKE '%'+ISNULL(@SearchVal,'')+'%'
			OR S.MobileNumber LIKE '%'+ISNULL(@SearchVal,'')+'%' 
			OR S.Email LIKE '%'+ISNULL(@SearchVal,'')+'%'		
			)
	ORDER BY 

	CASE WHEN @SortingCol = 'CreatedDate' AND @SortType ='ASC' THEN S.CreatedDate END ,
	CASE WHEN @SortingCol = 'CreatedDate' AND @SortType ='DESC' THEN S.CreatedDate END DESC
	OFFSET (@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY;	
END
ELSE
BEGIN
	----=========== GET ALL FILTERED RESULT ===========	  
	SELECT [Id]
		  ,ROW_NUMBER() OVER (order by (select 1)) AS SN
		  ,[PartnerCode]
		  ,[MemberId]
		  ,[FirstName]
		  ,[SurName]
		  ,[IsSurNamePresent]
		  ,[MobileNumber]
		  ,[Email]
		  ,[GenderId]
		  ,[DateOfBirth]
		  ,[ProfileImagePath]
		  ,[CountryCode]
		  ,[Province]
		  ,[City]
		  ,[Zipcode]
		  ,[Address]
		  ,[OccupationId]
		  ,[DocumentTypeId]
		  ,[DocumentNumber]
		  ,[IssuedDate]
		  ,[ExpiryDate]
		  ,[IdFrontImgPath]
		  ,[IdBackImgPath]
		  ,[BankName]
		  ,[BankCode]
		  ,[Branch]
		  ,[AccountHolderName]
		  ,[AccountNumber]
		  ,[IncomeSourceId]
		  ,[GMTTimeZone]
		  ,[IsActive]
		  ,[IsDeleted]
		  ,[IsBlocked]
		  ,[KycStatusCode]
		  ,[Is2FAAuthenticated]
		  ,[AccountSecretKey]
		  ,[CreatedUserType]
		  ,[CreatedById]
		  ,[CreatedByName]
		  ,[CreatedDate]
		  ,[UpdatedUserType]
		  ,[UpdatedById]
		  ,[UpdatedByName]
		  ,[UpdatedDate]
	FROM [dbo].[tbl_senders] S WITH(NOLOCK)
	WHERE  ISNULL(FirstName,'') LIKE ISNULL(REPLACE(@FirstName,' ',''),'') + '%' 
		AND ISNULL(S.SurName,'') LIKE ISNULL(@SurName,'') + '%'
		AND ISNULL(S.MobileNumber,'') LIKE ISNULL(@MobileNumber,'') + '%'
		AND ISNULL(S.Email,'') LIKE ISNULL(@Email,'') + '%'
		AND (
			CASE 
				WHEN @UserStatus = 0 AND IsActive IN (0, 1) THEN 1
				WHEN @UserStatus = 1 AND IsActive =  1 THEN 1
				WHEN @UserStatus = 2 AND IsActive = 0 THEN 1
				ELSE 0
			END) = 1
		AND (
			CONCAT(ISNULL(S.FirstName, ''), ' ', ISNULL(S.SurName, '')) LIKE '%'+ISNULL(@SearchVal,'')+'%'
			OR S.MobileNumber LIKE '%'+ISNULL(@SearchVal,'')+'%' 
			OR S.Email LIKE '%'+ISNULL(@SearchVal,'')+'%'		
			)
	ORDER BY 

	CASE WHEN @SortingCol = 'CreatedDate' AND @SortType ='ASC' THEN S.CreatedDate END ,
	CASE WHEN @SortingCol = 'CreatedDate' AND @SortType ='DESC' THEN S.CreatedDate END DESC
END


----=========== GET PAGINATION DATA ===========	
	IF(@FilteredCount = 0)
	BEGIN
		SET @TotalPages = 1;
	END
	ELSE IF((@FilteredCount % @PageSize) = 0)
	BEGIN
		SET @TotalPages = (@FilteredCount / @PageSize);
	END
	ELSE
	BEGIN
		SET @TotalPages = (@FilteredCount / @PageSize) + 1;
	END

	IF(@PageNumber > @TotalPages)
		SET @PageNumber = 1;

	SELECT @PageNumber AS PageNumber, @PageSize AS PageSize, @TotalCount AS TotalCount, @FilteredCount AS FilteredCount, @TotalPages AS TotalPages;
 
END
GO