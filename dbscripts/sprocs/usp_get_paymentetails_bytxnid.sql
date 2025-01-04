USE [MpmtDb]
GO

Alter PROCEDURE [dbo].[usp_get_paymentetails_bytxnid]
(
    @TransactionId VARCHAR(50) = NULL
)
AS
BEGIN
    DECLARE @PaymentType NVARCHAR(100)

    SELECT @PaymentType = PaymentType
    FROM tbl_remit_transaction
    WHERE TransactionId = @TransactionId

    IF @PaymentType = 'Bank'
    BEGIN
        SELECT
            BankName,          
            BankCode,
			Branch,
            AccountNumber,
            AccountHolderName,
            'Bank' AS [Type]
        FROM tbl_remit_transaction WITH (NOLOCK)
        WHERE TransactionId = @TransactionId
    END
    ELSE
    BEGIN
        SELECT
            WalletName AS BankName,			
            WalletCode AS BankCode,
			'' as Branch,
            WalletNumber AS AccountNumber,
            WalletHolderName AS AccountHolderName,
            'Wallet' AS [Type]
        FROM tbl_remit_transaction WITH (NOLOCK)
        WHERE TransactionId = @TransactionId
    END
END
