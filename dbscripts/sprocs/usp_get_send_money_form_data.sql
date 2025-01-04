CREATE OR ALTER PROCEDURE [dbo].[usp_get_send_money_form_data] --'REM3304621679'
(
@PartnerCode varchar(20) = NULL
)
AS
BEGIN

----------------------GET PAYOUT TYPES-------------------------------
SELECT Id AS PaymentTypeId, PaymentTypeName AS PaymentTypeName, PaymentTypeCode AS PaymentTypeCode
FROM dbo.tbl_payment_type WITH(NOLOCK) WHERE IsActive=1 AND ISNULL(IsDeleted,0) = 0;

----------------------GET PARTNER SOURCE CURRENCY LIST-------------------------------
SELECT SourceCurrency AS [SourceCurrencyText], SourceCurrency AS [SourceCurrencyValue]
FROM dbo.tbl_partner_wallets WITH(NOLOCK) WHERE PartnerCode=@PartnerCode AND IsActive=1 AND ISNULL(IsDeleted,0) = 0
ORDER BY SourceCurrency;

----------------------GET PARTNER SOURCE CURRENCY LIST-------------------------------
SELECT 'NPR' AS [DestinationCurrencyText], 'NPR' AS [DestinationCurrencyValue] 

----------------------GET COUNTRY LIST-------------------------------
SELECT CountryName, CountryCode FROM dbo.tbl_country WITH(NOLOCK) WHERE IsActive=1 ORDER BY CountryName;

----------------------GET PROVINCE LIST-------------------------------
SELECT Province, ProvinceCode, CountryCode FROM dbo.tbl_province WITH(NOLOCK);

----------------------GET DISTRICT LIST-------------------------------
SELECT District, DistrictCode, ProvinceCode FROM dbo.tbl_district WITH(NOLOCK);

----------------------GET LOCAL BODY LIST-------------------------------
SELECT LocalLevel, LocalLevelCode, DistrictCode FROM dbo.tbl_local_level WITH(NOLOCK);

----------------------GET BANK LIST-------------------------------
SELECT BankName, BankCode, CountryCode FROM dbo.tbl_banks WITH(NOLOCK) WHERE IsActive=1 AND ISNULL(IsDeleted,0) = 0 ORDER BY BankName;

----------------------GET RECIPIENT WALLET LIST-------------------------------
SELECT WalletName, WalletCode FROM dbo.tbl_wallets WITH(NOLOCK) WHERE IsActive=1 AND ISNULL(IsDeleted,0) = 0 ORDER BY DisplayOrder;

----------------------GET RELATIONSHIP LIST-------------------------------
SELECT Id AS RelationshipId, RelationName FROM dbo.tbl_relation WITH(NOLOCK) WHERE IsActive=1 AND ISNULL(IsDeleted,0) = 0 ORDER BY RelationName;

----------------------GET TRANSFER PURPOSE LIST-------------------------------
SELECT Id AS PurposeId, PurposeName FROM dbo.tbl_transfer_purpose WITH(NOLOCK) WHERE IsActive=1 AND ISNULL(IsDeleted,0) = 0;

----------------------GET RECIPIENT TYPE LIST-------------------------------
SELECT Id AS RecipientTypeId, RecipientType, LookupName AS RecipientTypeCode FROM dbo.tbl_recipient_type WITH(NOLOCK) WHERE IsActive=1 AND ISNULL(IsDeleted,0) = 0;

----------------------GET DOCUMENT TYPE LIST-------------------------------
SELECT Id, DocumentType, DocumentTypeCode FROM dbo.tbl_document_type WITH(NOLOCK) WHERE IsActive=1 AND ISNULL(IsDeleted,0) = 0;

----------------------GET OCCUPATION LIST-------------------------------
SELECT Id AS OccupationId, OccupationName FROM dbo.tbl_occupation WITH(NOLOCK) WHERE IsActive=1 AND ISNULL(IsDeleted,0) = 0;


END