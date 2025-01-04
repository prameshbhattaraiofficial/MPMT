

Create  PROCEDURE [dbo].[usp_get_Currency_ddl]
AS
BEGIN
	select 
	CurrencyName as [Text],
	Symbol as [Value]
	from tbl_currency with (Nolock) where IsActive=1
END
