CREATE OR ALTER PROCEDURE [dbo].[usp_get_remit_partners] --'','','','',''
@PartnerCode NVARCHAR(200) =NULL
,@Name NVARCHAR(200) = NULL
,@Email NVARCHAR(200)=NULL
,@StartDate datetime=NULL
,@EndDate datetime=NULL
,@GMT varchar(20)=NULL
,@PageNumber INT = 1
,@PageSize INT = 20
,@SortingCol AS VARCHAR(100) ='Date'
,@SortType AS VARCHAR(100) = 'DESC'
,@SearchVal NVARCHAR(300) =''
AS
BEGIN

IF(ISNULL(@GMT,'') = '')
	SET @GMT = 'UTC+5:45';

DECLARE @FilteredCount INT, @TotalCount INT, @TotalPages INT, @PageIndex INT;
DECLARE @tmpTbl AS TABLE(Id INT,Date datetime,DateString varchar(50),FirstName nvarchar(200), Username nvarchar(200),Contact nvarchar(30) ,Email NVARCHAR(200),MarchantId NVARCHAR(200), OrganizationName NVARCHAR(250),CredentialId varchar(50), IsActive bit, CreatedBy NVARCHAR(200), UpdatedBy NVARCHAR(200), IPAddress NVARCHAR(200), APIKey NVARCHAR(MAX), APIPassword NVARCHAR(MAX), PrivateKey NVARCHAR(MAX), PrivatePassword NVARCHAR(MAX),LicensedocImgPath nvarchar(512),Status BIT);
DECLARE @tmpTblFiltered AS TABLE(Id INT,Date datetime,DateString varchar(50),FirstName nvarchar(200), Username nvarchar(200),Contact nvarchar(30) ,Email NVARCHAR(200),MarchantId NVARCHAR(200), OrganizationName NVARCHAR(250),CredentialId varchar(50), IsActive bit, CreatedBy NVARCHAR(200), UpdatedBy NVARCHAR(200), IPAddress NVARCHAR(200), APIKey NVARCHAR(MAX), APIPassword NVARCHAR(MAX), PrivateKey NVARCHAR(MAX), PrivatePassword NVARCHAR(MAX),LicensedocImgPath nvarchar(512),Status BIT);
	SET NOCOUNT ON;
	IF(ISNULL(@PageSize, 0) <= 0)
		SET @PageSize=20;

	IF(ISNULL(@PageSize, 0) > 100)
		SET @PageSize=100;
	
	IF(ISNULL(@PageNumber, 0) <= 0)
		SET @PageNumber = 1;

	IF(ISNULL(@SortingCol,'') = '')
	BEGIN
		SET @SortingCol = 'DATE'
	END
	IF(ISNULL(@SortType,'') = '')
	BEGIN
		SET @SortType = 'DESC'
	END

	----===============INSERT FILTERED DATA INTO TEMPORARY TEBLE=============
	INSERT INTO @tmpTbl
SELECT trp.[Id]
      ,trp.[CreatedDate]
	  ,FORMAT((select [dbo].[func_convert_gmt_to_local_date](trp.[CreatedDate], @GMT)),'yyyy-MM-dd hh:mm:ss tt') AS [CreatedDateString]
	  ,trp.[FirstName]
	  ,trp.[UserName]      
      ,trp.[MobileNumber]
	  ,trp.[Email]
	  ,trp.[PartnerCode]
	  ,trp.[OrganizationName]
	  ,tpc.CredentialId
	  ,trp.[IsActive] 	
	  ,trp.CreatedByName
	  ,trp.UpdatedByName
	  ,tpc.IPAddress
	  ,tpc.ApiKey
	  ,tpc.ApiPassword
	  ,tpc.UserPublicKey
	  ,tpc.UserPrivateKey
	  ,trp.LicensedocImgPath 
	  ,tpc.IsActive
       FROM [dbo].[tbl_remit_partners] trp 	  
	   LEFT JOIN [dbo].tbl_partner_credentials tpc on trp.PartnerCode=tpc.PartnerCode --WITH(NOLOCK)  
	WHERE trp.PartnerCode LIKE '%'+ISNULL(@PartnerCode,'') +'%'  and ISNULL(trp.IsDeleted,0)=0 
	and (trp.UserName LIKE '%'+ISNULL(@Name,'') +'%' or @Name is NULL)
	and (trp.Email=@Email or ISNULL(@Email,'')='')
	--and ((trp.CreatedDate between @StartDate and @EndDate) OR @StartDate is NULL)
	AND ((CAST((select [dbo].[func_convert_gmt_to_local_date](trp.CreatedDate,@GMT)) AS DATE)  BETWEEN @StartDate AND @EndDate) OR ISNULL(@StartDate,'')='')
		
	--ORDER BY ServiceType DESC	

----============GET TOTAL COUNTS ===============	
	SELECT @TotalCount = COUNT(Id) FROM @tmpTbl;

	----============GET FILTERED COUNTS ===============	
	INSERT INTO @tmpTblFiltered
	SELECT *
	FROM @tmpTbl
	

	SELECT  @FilteredCount = COUNT(Id) FROM @tmpTblFiltered;

	----=========== GET FILTERED RESULT SET WITH PAGINATION ===========	

	SELECT * FROM @tmpTblFiltered t
	ORDER BY 
	--t.[Date] DESC
	CASE WHEN @SortingCol = 'Date' AND @SortType ='ASC' THEN t.[Date] END ,
	CASE WHEN @SortingCol = 'Date' AND @SortType ='DESC' THEN t.[Date] END DESC

	OFFSET (@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY;

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


	SELECT @PageNumber AS PageNumber, @PageSize AS PageSize, @TotalCount AS TotalCount, @FilteredCount AS FilteredCount, @TotalPages AS TotalPages;
	

  --SET NOCOUNT OFF;
END
