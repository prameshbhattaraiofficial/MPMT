
-- =============================================
-- Author:		<SAROJ KUMAR CHAUDAHRY>
-- Create date: <2023-SEP-10>
-- Description:	<Get amount in USD from any source currency>
-- Execution: SELECT [dbo].[func_convert_to_usd]('NPR',132.64)
-- =============================================
CREATE OR ALTER  FUNCTION [dbo].[func_convert_to_usd] (@SourceCurrency varchar(3),@Amount decimal(18,4))
RETURNS DECIMAL(18,2)
AS
BEGIN
	
	DECLARE @Result DECIMAL(18,2);
	DECLARE @currRate DECIMAL(18,4),@AmountNPR DECIMAL(18,2), @currRateNprToUsd DECIMAL(18,4), @currRateUsdToNpr DECIMAL(18,4)
	
	IF(@SourceCurrency = 'USD')
	BEGIN
		SET @Result = @Amount;
		RETURN @Result;
	END
	
	
	IF(@SourceCurrency='NPR')
	BEGIN
		SET @AmountNPR=@Amount 
	END
	ELSE
	BEGIN
		SELECT @currRate=CurrentRate FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE SourceCurrency=@SourceCurrency AND DestinationCurrency='NPR' AND IsActive=1 AND ISNULL(IsDeleted,0)=0;
		SET @AmountNPR = @Amount * @currRate;
	END

	IF EXISTS(SELECT 1 FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE SourceCurrency='USD' AND DestinationCurrency='NPR' AND IsActive=1 AND ISNULL(IsDeleted,0)=0)
	BEGIN
		SELECT @currRateUsdToNpr=CurrentRate FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE SourceCurrency='USD' AND DestinationCurrency='NPR' AND IsActive=1 AND ISNULL(IsDeleted,0)=0;
		SET @currRateNprToUsd = (1/@currRateUsdToNpr);
	END
	ELSE IF EXISTS(SELECT 1 FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE SourceCurrency='NPR' AND DestinationCurrency='USD' AND IsActive=1 AND ISNULL(IsDeleted,0)=0)
	BEGIN
		SELECT @currRateNprToUsd=CurrentRate FROM dbo.tbl_exchange_rate WITH(NOLOCK) WHERE SourceCurrency='NPR' AND DestinationCurrency='USD' AND IsActive=1 AND ISNULL(IsDeleted,0)=0;
	END

	SET @Result = @AmountNPR * @currRateNprToUsd;
	

	RETURN @Result;
	
END;
