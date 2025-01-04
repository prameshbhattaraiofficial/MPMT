/*
	DBCC DROPCLEANBUFFERS WITH NO_INFOMSGS;
	SET NOCOUNT ON

	SET STATISTICS io ON
	SET STATISTICS time ON
	GO
-- =============================================
-- Author:		<SAROJ KUMAR CHAUDAHRY>
-- Create date: <2023-SEP-04>
-- Description:	<Returns the range of a week on basis of date provided and first_day_of-week provided>
-- Execution: select * from func_get_week_range(getdate(),'sun')
-- =============================================
*/
CREATE OR ALTER FUNCTION [dbo].[func_get_week_range]
(	
	-- Add the parameters for the function here
	@selectedDate DATE
	,@fristdayofweek varchar(3)

)
RETURNS @tblweekrange TABLE (Week_Start_Date varchar(20), Week_End_Date varchar(20))
AS
BEGIN 

	IF(LOWER(@fristdayofweek) = 'sun')
		BEGIN
			INSERT INTO @tblweekrange
			SELECT DATEADD(DAY, 1 - DATEPART(WEEKDAY, @selectedDate), CAST(@selectedDate AS DATE)) [Week_Start_Date]					
					,DATEADD(DAY, 7 - DATEPART(WEEKDAY, @selectedDate), CAST(@selectedDate AS DATE)) [Week_End_Date]
		END
	ELSE IF(LOWER(@fristdayofweek) = 'mon')
		BEGIN
			INSERT INTO @tblweekrange
			SELECT DATEADD(DAY, 2 - DATEPART(WEEKDAY, @selectedDate), CAST(@selectedDate AS DATE)) [Week_Start_Date]
				,DATEADD(DAY, 8 - DATEPART(WEEKDAY, @selectedDate), CAST(@selectedDate AS DATE)) [Week_End_Date]
		END
	ELSE IF(lower(@fristdayofweek) = 'sat')
		BEGIN
			INSERT INTO @tblweekrange
			SELECT DATEADD(DAY, 0 - DATEPART(WEEKDAY, @selectedDate), CAST(@selectedDate AS DATE)) [Week_Start_Date]
				,DATEADD(DAY, 6 - DATEPART(WEEKDAY, @selectedDate), CAST(@selectedDate AS DATE)) [Week_End_Date]
		END

	RETURN 
END
