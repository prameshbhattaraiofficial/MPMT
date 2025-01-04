USE [MpmtDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  
CREATE OR ALTER PROCEDURE [dbo].[sp_get_fundrequest_list]  
 (  
	 @PartnerCode nvarchar(200) = ''  
	,@TxnId varchar(100) = ''   	
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
	--DBCC FREEPROCCACHE; 

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
	SELECT @TotalCount = COUNT(1) FROM [dbo].tbl_fund_request U WITH(NOLOCK)	
	WHERE  ISNULL(PartnerCode,'') LIKE ISNULL(REPLACE(@PartnerCode,' ',''),'') + '%' 
		AND ISNULL(CAST(U.TransactionId as varchar),'') LIKE '%' + ISNULL(REPLACE(CAST(@TxnId as varchar),' ',''),'') + '%'  
	
		

	----===================FILTERED COUNT =========================
	SELECT @FilteredCount = COUNT(1) FROM [dbo].tbl_fund_request U WITH(NOLOCK)
	WHERE  ISNULL(PartnerCode,'') LIKE ISNULL(REPLACE(@PartnerCode,' ',''),'') + '%' 
		AND ISNULL(CAST(U.TransactionId as varchar),'') LIKE '%' + ISNULL(REPLACE(CAST(@TxnId as varchar),' ',''),'') + '%' 
		
		AND (
			u.PartnerCode LIKE '%'+ISNULL(@SearchVal,'')+'%' 
			OR U.Amount LIKE '%'+ISNULL(@SearchVal,'')+'%' 
			OR u.TransactionId LIKE '%'+ISNULL(@SearchVal,'')+'%' OR cast(u.FundTypeId as varchar(50))  LIKE '%'+'%' OR (select FundType from tbl_fund_type where Id=u.FundTypeId)  LIKE '%'+ISNULL(@SearchVal,'')+'%' 			
			)

----=========== GET FILTERED RESULT SET WITH PAGINATION ===========	  
	SELECT   
	u.Id
	,ROW_NUMBER() OVER (order by (select 1)) AS Sn 	
     ,u.PartnerCode	
	 ,rp.OrganizationName
	,(select FundType from tbl_fund_type where Id=u.FundTypeId) as FundType
	,u.SourceCurrency
	,DestinationCurrency
	,Amount
	,Remarks
	,[Sign]
	,u.TransactionId	
	,(select StatusName from tbl_request_status where Id=u.RequestStatusId) as RequestStatus
	,(select StatusCode from tbl_request_status where Id=u.RequestStatusId) as RequestStatusCode
	,FORMAT(u.CreatedDate,'yyyy-MM-dd HH:mm:ss') AS RegisteredDate	
	,FORMAT(u.UpdatedDate,'yyyy-MM-dd HH:mm:ss') AS UpdatedDate	
	,VoucherImgPath		
	FROM [dbo].tbl_fund_request u WITH(NOLOCK) inner join tbl_remit_partners rp on rp.PartnerCode=u.PartnerCode   
	WHERE  ISNULL(u.PartnerCode,'') LIKE ISNULL(REPLACE(@PartnerCode,' ',''),'') + '%' 
		AND ISNULL(CAST(U.TransactionId as varchar),'') LIKE '%' + ISNULL(REPLACE(CAST(@TxnId as varchar),' ',''),'') + '%' 
		
		AND (
			u.PartnerCode LIKE '%'+ISNULL(@SearchVal,'')+'%' 
			OR U.Amount LIKE '%'+ISNULL(@SearchVal,'')+'%' 
			OR u.TransactionId LIKE '%'+ISNULL(@SearchVal,'')+'%' OR (select FundType from tbl_fund_type where Id=u.FundTypeId)  LIKE '%'+ISNULL(@SearchVal,'')+'%' 			
			)
	ORDER BY 

	CASE WHEN @SortingCol = 'CreatedDate' AND @SortType ='ASC' THEN u.CreatedDate END ,
	CASE WHEN @SortingCol = 'CreatedDate' AND @SortType ='DESC' THEN u.CreatedDate END DESC
	OFFSET (@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY;	
END
ELSE
BEGIN
	----=========== GET ALL FILTERED RESULT ===========	  
	SELECT   
	Id
	,ROW_NUMBER() OVER (order by (select 1)) AS SN  	
     ,PartnerCode	
	,(select FundType from tbl_fund_type where Id=u.FundTypeId) as FundType
	,SourceCurrency
	,DestinationCurrency
	,Amount
	,Remarks
	,[Sign]
	,u.TransactionId	
	,(select StatusName from tbl_request_status where Id=u.RequestStatusId) as RequestStatus
	,(select StatusCode from tbl_request_status where Id=u.RequestStatusId) as RequestStatusCode
	,FORMAT(u.CreatedDate,'yyyy-MM-dd HH:mm:ss') AS RegisteredDate	
	,VoucherImgPath		
	FROM [dbo].tbl_fund_request u WITH(NOLOCK)   
	WHERE  ISNULL(PartnerCode,'') LIKE ISNULL(REPLACE(@PartnerCode,' ',''),'') + '%' 
		AND ISNULL(CAST(U.TransactionId as varchar),'') LIKE ISNULL(REPLACE(CAST(@TxnId as varchar),' ',''),'') + '%' 
		
		AND (
			u.PartnerCode LIKE '%'+ISNULL(@SearchVal,'')+'%' 
			OR U.Amount LIKE '%'+ISNULL(@SearchVal,'')+'%' 
			OR u.TransactionId LIKE '%'+ISNULL(@SearchVal,'')+'%' OR cast(u.FundTypeId as varchar(50))  LIKE '%'+'%' OR (select FundType from tbl_fund_type where Id=u.FundTypeId)  LIKE '%'+ISNULL(@SearchVal,'')+'%' 			
			)
	ORDER BY 

	CASE WHEN @SortingCol = 'CreatedDate' AND @SortType ='ASC' THEN u.CreatedDate END ,
	CASE WHEN @SortingCol = 'CreatedDate' AND @SortType ='DESC' THEN u.CreatedDate END DESC
	OFFSET (@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY;	
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