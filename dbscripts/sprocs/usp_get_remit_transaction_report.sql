CREATE OR ALTER   PROCEDURE [dbo].[usp_get_remit_transaction_report] --'admin','2023-08-28' --'REM3304621679',null,null,null,null,null,null,null,null,null,null,'partner'
(
@PartnerCode VARCHAR(50) =NULL		----ACTUAL PARTNERCODE OR ADMIN
,@StartDate date=NULL
,@EndDate date=NULL
,@SourceCurrency varchar(3) = NULL
,@DestinationCurrency varchar(3) = NULL
,@TransactionId varchar(100) = NULL
,@SignType varchar(50) = NULL		----DEBIT/CREDIT
,@TransactionType varchar(50) = NULL	----LOADWALLET/WALLETCONVERSION/COMMISSION/APITRANSACTION/PARTNERTRANSACTION

,@MerchantMobileNo varchar(30) = NULL
,@GatewayTxnId varchar(100) = NULL
,@TrackerId varchar(100) = NULL

,@UserType varchar(50) = 'PARTNER'
,@LoggedInUser varchar(100) = NULL 
,@PageNumber INT = 1
,@PageSize INT = 20
,@SortingCol AS VARCHAR(100) ='CreatedDate'
,@SortType AS VARCHAR(100) = 'DESC'
,@SearchVal NVARCHAR(300) =''
)
AS
BEGIN

DECLARE @GMTTimeZone varchar(20); 
SET @GMTTimeZone = 'UTC+5:45';
IF(@UserType = 'ADMIN')
BEGIN
SET @PartnerCode = 'admin';
END
IF(@UserType = 'PARTNER')
BEGIN
	SELECT @GMTTimeZone = GMTTimeZone FROM dbo.tbl_remit_partners WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND IsActive=1 AND ISNULL(IsDeleted,0)=0
END


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
		SET @SortType = 'DESC'
	END

	IF(ISNULL(@StartDate,'') = '')
		SET @StartDate = CAST(GETDATE() AS DATE)

	IF(ISNULL(@EndDate,'') = '')
		SET @EndDate = CAST(GETDATE() AS DATE)


	----============GET TOTAL COUNTS ===============	
	SELECT @TotalCount = COUNT(1)
	FROM [dbo].[tbl_remit_transaction] t WITH(NOLOCK)
	INNER JOIN [dbo].[tbl_remit_partners] p WITH(NOLOCK) ON p.PartnerCode=t.PartnerCode
	INNER JOIN [dbo].[tbl_payment_type] pt WITH(NOLOCK) ON pt.Id=t.PaymentTypeId
	INNER JOIN [dbo].[tbl_transaction_status] s WITH(NOLOCK) ON s.StatusCode=t.StatusCode	  
	WHERE (t.PartnerCode = @PartnerCode OR ISNULL(@PartnerCode,'admin')='admin')
		AND (CAST((select [dbo].[func_convert_gmt_to_local_date](t.CreatedDate,@GMTTimeZone)) AS DATE)  BETWEEN @StartDate AND @EndDate)
		AND (t.TransactionId = @TransactionId OR ISNULL(@TransactionId,'') = '')
		AND (t.SourceCurrency = @SourceCurrency OR ISNULL(@SourceCurrency,'') = '')
		AND (t.DestinationCurrency = @DestinationCurrency OR ISNULL(@DestinationCurrency,'') = '')
		AND (t.Sign = @SignType OR ISNULL(@SignType,'') = '')
		AND (p.MobileNumber = @MerchantMobileNo OR ISNULL(@MerchantMobileNo,'') = '')
		AND (t.GatewayTxnId = @GatewayTxnId OR ISNULL(@GatewayTxnId,'') = '')
		AND (t.AgentTrackerId = @TrackerId OR ISNULL(@TrackerId,'') = '')
		AND (t.TransactionType = @TransactionType OR ISNULL(@TransactionType,'') = '');
		
	----============GET FILTERED COUNTS ===============	
	SELECT @FilteredCount = COUNT(1)
	FROM [dbo].[tbl_remit_transaction] t WITH(NOLOCK)
	INNER JOIN [dbo].[tbl_remit_partners] p WITH(NOLOCK) ON p.PartnerCode=t.PartnerCode
	INNER JOIN [dbo].[tbl_payment_type] pt WITH(NOLOCK) ON pt.Id=t.PaymentTypeId
	INNER JOIN [dbo].[tbl_transaction_status] s WITH(NOLOCK) ON s.StatusCode=t.StatusCode	  
	WHERE (t.PartnerCode = @PartnerCode OR ISNULL(@PartnerCode,'admin')='admin')
		AND (CAST((select [dbo].[func_convert_gmt_to_local_date](t.CreatedDate,@GMTTimeZone)) AS DATE)  BETWEEN @StartDate AND @EndDate)
		AND (t.TransactionId = @TransactionId OR ISNULL(@TransactionId,'') = '')
		AND (t.SourceCurrency = @SourceCurrency OR ISNULL(@SourceCurrency,'') = '')
		AND (t.DestinationCurrency = @DestinationCurrency OR ISNULL(@DestinationCurrency,'') = '')
		AND (t.Sign = @SignType OR ISNULL(@SignType,'') = '')
		AND (p.MobileNumber = @MerchantMobileNo OR ISNULL(@MerchantMobileNo,'') = '')
		AND (t.GatewayTxnId = @GatewayTxnId OR ISNULL(@GatewayTxnId,'') = '')
		AND (t.AgentTrackerId = @TrackerId OR ISNULL(@TrackerId,'') = '')
		AND (t.TransactionType = @TransactionType OR ISNULL(@TransactionType,'') = '')
		AND (
			t.PartnerCode LIKE '%' + ISNULL(@SearchVal,'') + '%'
			OR t.SourceCurrency LIKE '%' + ISNULL(@SearchVal,'') + '%' OR t.DestinationCurrency LIKE '%' + ISNULL(@SearchVal,'') + '%'
			OR pt.PaymentTypeName LIKE '%' + ISNULL(@SearchVal,'') + '%'  OR t.AgentTrackerId LIKE '%' + ISNULL(@SearchVal,'') + '%' 
		)
	
	----=========== GET FILTERED RESULT SET WITH PAGINATION ===========	

	SELECT ROW_NUMBER() OVER (order by (select 1)) AS SN 
		,t.CreatedDate AS TransactionDate
		,FORMAT((select [dbo].[func_convert_gmt_to_local_date](t.CreatedDate,@GMTTimeZone)), 'yyyy-MM-dd hh:mm:ss tt') AS TransactionDateString
		,CASE WHEN ISNULL(t.MemberId,'') = '' THEN 'Sender Detail' ELSE t.MemberId END AS SenderDtl
		,CASE WHEN ISNULL(t.RecipientId,'') = '' THEN 'Receiver Detail' ELSE t.RecipientId END AS ReceiverDtl
		,t.TransactionId
		,p.PartnerCode AS PartnerId
		--,TRIM(REPLACE(ISNULL(p.FirstName,''),'-','') + ' ' + p.SurName) AS PartnerFullName
		,ISNULL(t.PartnerName,'') AS PartnerFullName
		,t.SourceCurrency
		,t.DestinationCurrency
		,pt.PaymentTypeName AS PaymentType
		,t.SendingAmount AS Amount
		,ISNULL(t.ServiceCharge, 0) AS ServiceCharge
		,t.NetSendingAmount AS SendAmount
		,t.NetReceivingAmount AS ReceiveAmount
		,t.PartnerServiceCharge		
		,'View' AS Status
		,CASE WHEN ISNULL(t.SenderRegistered,0) = 1 AND s.KycStatusCode='A' THEN 'Registered' ELSE 'Pending' END AS SenderUserStatus
		,CASE WHEN ISNULL(t.RecipientRegistered,0) = 1 AND r.KycStatusCode='A' THEN 'Registered' ELSE 'Pending' END AS ReceiverUserStatus
	FROM [dbo].[tbl_remit_transaction] t WITH(NOLOCK)
	INNER JOIN [dbo].[tbl_remit_partners] p WITH(NOLOCK) ON p.PartnerCode=t.PartnerCode
	INNER JOIN [dbo].[tbl_payment_type] pt WITH(NOLOCK) ON pt.Id=t.PaymentTypeId  
	LEFT JOIN [dbo].[tbl_senders] s WITH(NOLOCK) ON s.MemberId = t.MemberId
	LEFT JOIN [dbo].[tbl_recipients] r WITH(NOLOCK) ON r.RecipientId = t.RecipientId
	WHERE (t.PartnerCode = @PartnerCode OR ISNULL(@PartnerCode,'admin')='admin')
		AND (CAST((select [dbo].[func_convert_gmt_to_local_date](t.CreatedDate,@GMTTimeZone)) AS DATE)  BETWEEN @StartDate AND @EndDate)
		AND (t.TransactionId = @TransactionId OR ISNULL(@TransactionId,'') = '')
		AND (t.SourceCurrency = @SourceCurrency OR ISNULL(@SourceCurrency,'') = '')
		AND (t.DestinationCurrency = @DestinationCurrency OR ISNULL(@DestinationCurrency,'') = '')
		AND (t.Sign = @SignType OR ISNULL(@SignType,'') = '')
		AND (p.MobileNumber = @MerchantMobileNo OR ISNULL(@MerchantMobileNo,'') = '')
		AND (t.GatewayTxnId = @GatewayTxnId OR ISNULL(@GatewayTxnId,'') = '')
		AND (t.AgentTrackerId = @TrackerId OR ISNULL(@TrackerId,'') = '')
		AND (t.TransactionType = @TransactionType OR ISNULL(@TransactionType,'') = '')
		AND (
			t.PartnerCode LIKE '%' + ISNULL(@SearchVal,'') + '%'
			OR t.SourceCurrency LIKE '%' + ISNULL(@SearchVal,'') + '%' OR t.DestinationCurrency LIKE '%' + ISNULL(@SearchVal,'') + '%'
			OR pt.PaymentTypeName LIKE '%' + ISNULL(@SearchVal,'') + '%' OR t.AgentTrackerId LIKE '%' + ISNULL(@SearchVal,'') + '%' 
		)
	ORDER BY t.CreatedDate DESC
	--CASE WHEN @SortingCol = 'CreatedDate' AND @SortType ='ASC' THEN t.CreatedDate END ,
	--CASE WHEN @SortingCol = 'CreatedDate' AND @SortType ='DESC' THEN t.CreatedDate END DESC
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
