
create or alter procedure [dbo].[usp_get_existing_recipients_by_memberid]	--'1DFRH87784382438'
(
@MemberId varchar(20) = null
)
AS
BEGIN
	SELECT  
	TRIM(REPLACE(r.FirstName,'-','') + ' ' + r.SurName) AS FullName
	,r.RecipientId
	,r.AccountNumber
	,r.BankName
	,r.MobileNumber AS ContactNumber
	,r.Email
	FROM dbo.tbl_recipients r WITH(NOLOCK) 
	INNER JOIN dbo.tbl_senders s WITH(NOLOCK) ON s.Id=r.SenderId
	WHERE s.MemberId=@MemberId 
END

