
-- =============================================
-- Author:		<SAROJ KUMAR CHAUDAHRY>
-- Create date: <2023-SEP-10>
-- Description:	<Convert amount from Source currency to Destinatin Currency>
-- Execution: SELECT [dbo].[func_currency_converter]('NPR','USD',100)
-- =============================================
CREATE OR ALTER    FUNCTION [dbo].[func_currency_converter] (@SourceCurrency varchar(3),@DestinationCurrency varchar(3),@Amount decimal(18,4))
RETURNS DECIMAL(18,2)
AS
BEGIN
	
	DECLARE @Result DECIMAL(18,2);
	DECLARE @currRate DECIMAL(18,4),@AmountNPR DECIMAL(18,2), @currRateNpr DECIMAL(18,4), @currRateDst DECIMAL(18,4)
	
	IF(@SourceCurrency = @DestinationCurrency)
	BEGIN
		SET @Result = @Amount;
		RETURN @Result;
	END
	
	IF(@SourceCurrency <> 'NPR')
	BEGIN
		SELECT @currRate=CurrentRate FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE SourceCurrency=@SourceCurrency AND DestinationCurrency='NPR' AND IsActive=1 AND ISNULL(IsDeleted,0)=0;
		SET @AmountNPR = @Amount * @currRate;
	END
	ELSE
	BEGIN
		SET @AmountNPR = @Amount;
	END

	IF(@DestinationCurrency='NPR')
	BEGIN
		SET @Result=@AmountNPR
	END
	ELSE
	BEGIN

	IF EXISTS(SELECT 1 FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE SourceCurrency=@DestinationCurrency AND DestinationCurrency='NPR' AND IsActive=1 AND ISNULL(IsDeleted,0)=0)
	BEGIN
		SELECT @currRateDst=CurrentRate FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE SourceCurrency=@DestinationCurrency AND DestinationCurrency='NPR' AND IsActive=1 AND ISNULL(IsDeleted,0)=0;
		SET @currRateNpr = (1/@currRateDst);
	END
	ELSE IF EXISTS(SELECT 1 FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE SourceCurrency='NPR' AND DestinationCurrency=@DestinationCurrency AND IsActive=1 AND ISNULL(IsDeleted,0)=0)
	BEGIN
		SELECT @currRateNpr=CurrentRate FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE SourceCurrency='NPR' AND DestinationCurrency=@DestinationCurrency AND IsActive=1 AND ISNULL(IsDeleted,0)=0;
	END

	SET @Result = @AmountNPR * @currRateNpr;
	
	END

	RETURN @Result;
	
END;
