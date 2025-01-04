
CREATE OR ALTER  PROCEDURE [dbo].[usp_get_compliance_settings] 
(
@ComplianceCode varchar(20) = NULL
,@ComplianceRule varchar(300) = NULL
,@ComplianceAction varchar(50) = NULL

,@LoggedInUser varchar(100) = NULL 
,@PageNumber INT = 1
,@PageSize INT = 20
,@SortingCol AS VARCHAR(100) ='CreatedDate'
,@SortType AS VARCHAR(100) = 'ASC'
,@SearchVal NVARCHAR(300) =''
)
AS
BEGIN

DECLARE @GMTTimeZone varchar(20); 
SET @GMTTimeZone = 'UTC+5:45';
--IF(@UserType = 'ADMIN')
--BEGIN
--SET @PartnerCode = 'admin';
--END
--IF(@UserType = 'PARTNER')
--BEGIN
--	SELECT @GMTTimeZone = GMTTimeZone FROM dbo.tbl_remit_partners WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND IsActive=1 AND ISNULL(IsDeleted,0)=0
--END
----SET @userCurrDateTime =  (select [dbo].[func_convert_gmt_to_local_date](GETUTCDATE(),@GMTTimeZone))


DECLARE @FilteredCount INT, @TotalCount INT, @TotalPages INT, @PageIndex INT;

	SET NOCOUNT ON;
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

	--IF(ISNULL(@StartDate,'') = '')
	--	SET @StartDate = CAST(GETDATE() AS DATE)

	--IF(ISNULL(@EndDate,'') = '')
	--	SET @EndDate = CAST(GETDATE() AS DATE)

	----============GET TOTAL COUNTS ===============	
	SELECT @TotalCount = COUNT(1)
	FROM [dbo].[tbl_compliance_settings]  WITH(NOLOCK)	
	WHERE (ComplianceCode = @ComplianceCode OR ISNULL(@ComplianceCode,'')='')
		AND (ComplianceRule = @ComplianceRule OR ISNULL(@ComplianceRule,'')='')
		AND (ComplianceAction = @ComplianceAction OR ISNULL(@ComplianceAction,'')='')
		
	----============GET FILTERED COUNTS ===============	
	SELECT @FilteredCount = COUNT(1)
	FROM [dbo].[tbl_compliance_settings]  WITH(NOLOCK)	
	WHERE (ComplianceCode = @ComplianceCode OR ISNULL(@ComplianceCode,'')='')
		AND (ComplianceRule = @ComplianceRule OR ISNULL(@ComplianceRule,'')='')
		AND (ComplianceAction = @ComplianceAction OR ISNULL(@ComplianceAction,'')='')
		AND (
			ComplianceCode LIKE '%' + ISNULL(@SearchVal,'') + '%'
			OR ComplianceRule LIKE '%' + ISNULL(@SearchVal,'') + '%' OR ComplianceAction LIKE '%' + ISNULL(@SearchVal,'') + '%'
			)
	
	----=========== GET FILTERED RESULT SET WITH PAGINATION ===========	

	SELECT ROW_NUMBER() OVER (order by (select 1)) AS SN  
		,ComplianceCode
		,ComplianceRule
		,Description
		,[Count]
		,[NoOfDays]
		,ComplianceAction
		,ISNULL(IsActive,0) As IsActive
		,ISNULL(IsDeleted,0) AS IsDeleted
		,CreatedByName 
		--,CreatedDate
		,FORMAT([dbo].[func_convert_gmt_to_local_date](CreatedDate,@GMTTimeZone),'yyyy-MM-dd hh:mm:ss tt') AS CreatedDate
		,UpdatedByName
		--,UpdatedDate
		,FORMAT([dbo].[func_convert_gmt_to_local_date](UpdatedDate,@GMTTimeZone),'yyyy-MM-dd hh:mm:ss tt') AS UpdatedDate
	FROM [dbo].[tbl_compliance_settings]  WITH(NOLOCK)	
	WHERE (ComplianceCode = @ComplianceCode OR ISNULL(@ComplianceCode,'')='')
		AND (ComplianceRule = @ComplianceRule OR ISNULL(@ComplianceRule,'')='')
		AND (ComplianceAction = @ComplianceAction OR ISNULL(@ComplianceAction,'')='')
		AND (
			ComplianceCode LIKE '%' + ISNULL(@SearchVal,'') + '%'
			OR ComplianceRule LIKE '%' + ISNULL(@SearchVal,'') + '%' OR ComplianceAction LIKE '%' + ISNULL(@SearchVal,'') + '%'
			)
	ORDER BY CreatedDate ASC	
	OFFSET (@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY;


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


	SELECT @PageNumber AS PageNumber, @PageSize AS PageSize, @TotalCount AS TotalCount, @FilteredCount AS FilteredCount, @TotalPages AS TotalPages;
	

  SET NOCOUNT OFF;
END
