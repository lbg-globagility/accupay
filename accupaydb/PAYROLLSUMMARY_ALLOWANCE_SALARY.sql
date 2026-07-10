/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `PAYROLLSUMMARY_ALLOWANCE_SALARY`;
DELIMITER //
CREATE PROCEDURE `PAYROLLSUMMARY_ALLOWANCE_SALARY`(
	IN `ps_OrganizationID` INT,
	IN `ps_PayPeriodID1` INT,
	IN `ps_PayPeriodID2` INT,
	IN `psi_undeclared` CHAR(1),
	IN `strSalaryDistrib` VARCHAR(50),
	IN `is_keep_in_onesheet` BOOL
)
BEGIN

DECLARE paypdatefrom
        ,paypdateto
		  ,min_paydatefrom
		  ,max_paydateto DATE;

DECLARE sec_per_hour INT(11) DEFAULT 3600;

DECLARE payfreq_rowid
        , div_count
        , div_index
        , div_rowid INT(11) DEFAULT 0;

DECLARE decimal_size INT(11) DEFAULT 6;

DECLARE customDateFormat VARCHAR(50) DEFAULT '%m/%d/%Y';

DECLARE isNullSalaryDistribType BOOL DEFAULT FALSE;

SET isNullSalaryDistribType = strSalaryDistrib IS NULL;

SELECT
    PayFromDate,
    TotalGrossSalary
FROM payperiod
WHERE RowID = ps_PayPeriodID1
INTO
    paypdatefrom,
    payfreq_rowid;

SELECT PayToDate
FROM payperiod
WHERE RowID = IFNULL(ps_PayPeriodID2, ps_PayPeriodID1)
INTO paypdateto;

SELECT MIN(pp.PayFromDate)
, MAX(ppd.PayToDate)
FROM payperiod pp
INNER JOIN payperiod ppd ON ppd.RowID = ps_PayPeriodID2
WHERE pp.RowID = ps_PayPeriodID1
INTO min_paydatefrom, max_paydateto;

CALL GetAccupaySalary(ps_OrganizationID, min_paydatefrom, max_paydateto);

SET @_hasMultiRateSalary=FALSE;
SET @_datefrom=CURDATE();
SET @_dateto=CURDATE();
SET @_eId=0;
SET @_isDaily=FALSE;

SELECT
	e.RowID 'EmployeeRowID',
	e.EmployeeID `DatCol2`,
	d.RowID `DivisionID`,
	ROUND(paystub.BasicHours, decimal_size) `BasicHours`,

	@_datefrom:=paystub.PayFromDate `DateFrom`,
	@_dateto:=paystub.PayToDate `DateTo`,
	@_eId:=paystub.EmployeeID `AssignEmployeeID`,
	@_isDaily:=e.EmployeeType = 'Daily' `IsDaily`,
	@_hasMultiRateSalary:=(SELECT
			GROUP_CONCAT(k.RowID)
			FROM (
					SELECT
					t.*
					FROM (SELECT
					     i.*,
					     IFNULL(SUBDATE(ii.EffectiveDateFrom, INTERVAL 1 DAY), LAST_DAY(i.EffectiveDateFrom)) `EffectiveDateTo`
					     FROM `accupaysalary` i
					     LEFT JOIN `accupaysalary` ii ON ii.EmployeeID=i.EmployeeID AND ii.SetNewDay=i.SetNewDay+1
					     ) t
					WHERE (t.`EffectiveDateTo` BETWEEN @_datefrom AND @_dateto) = TRUE
					) k
			WHERE k.EmployeeID=@_eId
			AND @_isDaily
			HAVING COUNT(k.RowID) > 1
			) `HasMultiRateSalary`,

	@salary:=IF(@_hasMultiRateSalary,
		(SELECT
		IF(psi_undeclared=0, SUM(k.Salary), SUM(k.TrueSalary))
		FROM (SELECT
				i.*,
				IFNULL(SUBDATE(ii.EffectiveDateFrom, INTERVAL 1 DAY), @_dateto) `EffectiveDateTo`
				FROM `accupaysalary` i
				LEFT JOIN `accupaysalary` ii ON ii.EmployeeID=i.EmployeeID AND ii.SetNewDay=i.SetNewDay+1      
				WHERE FIND_IN_SET(i.RowID, @_hasMultiRateSalary) > 0
				) k
		INNER JOIN dates d ON d.DateValue BETWEEN k.EffectiveDateFrom AND k.EffectiveDateTo
		INNER JOIN shiftschedules ss ON ss.EmployeeID=k.EmployeeID AND ss.Date=d.DateValue AND ss.IsRestDay=FALSE
		WHERE ss.Date BETWEEN @_datefrom AND @_dateto
		),
		ROUND(GetBasicPay(
					e.RowID,
					paystub.PayFromDate,
					paystub.PayToDate,
					psi_undeclared,
					paystub.BasicHours),
					decimal_size))
	 `BasicPayDefault`,
	 
	@trueSalary:=IF(@_hasMultiRateSalary,
		(SELECT
		IF(TRUE=0, SUM(k.Salary), SUM(k.TrueSalary))
		FROM (SELECT
				i.*,
				IFNULL(SUBDATE(ii.EffectiveDateFrom, INTERVAL 1 DAY), @_dateto) `EffectiveDateTo`
				FROM `accupaysalary` i
				LEFT JOIN `accupaysalary` ii ON ii.EmployeeID=i.EmployeeID AND ii.SetNewDay=i.SetNewDay+1      
				WHERE FIND_IN_SET(i.RowID, @_hasMultiRateSalary) > 0
				) k
		INNER JOIN dates d ON d.DateValue BETWEEN k.EffectiveDateFrom AND k.EffectiveDateTo
		INNER JOIN shiftschedules ss ON ss.EmployeeID=k.EmployeeID AND ss.Date=d.DateValue AND ss.IsRestDay=FALSE
		WHERE ss.Date BETWEEN @_datefrom AND @_dateto
		),
		ROUND(GetBasicPay(
					e.RowID,
					paystub.PayFromDate,
					paystub.PayToDate,
					TRUE,
					paystub.BasicHours),
					decimal_size))
	 `TrueSalary`,
	
	@_x:=@trueSalary / @salary `DifferPercentage`,
	@_xx:=1 - (@salary / @trueSalary) `DifferPercentage`,
	
	(@salary * @_x) * @_xx `BasicPay`,
	
	(IF(psi_undeclared,
		GetActualDailyRate(e.RowID, e.OrganizationID, paystub.PayFromDate),
		GET_employeerateperday(e.RowID, e.OrganizationID, paystub.PayFromDate)) * @_x) * @_xx `Rate`,
	
	ROUND(paystub.RegularHours, decimal_size) `RegularHours`,
	@regularPay:=ROUND((paystub.RegularPay * @_x) * @_xx, decimal_size) `RegularPay`,
	
	ROUND(paystub.OvertimeHours, decimal_size) `OvertimeHours`,
	@overtimePay:=ROUND((paystub.OvertimePay * @_x) * @_xx, decimal_size) `OvertimePay`,
	
	paystub.NightDiffHours `NightDiffHours`,
	@nightDiffPay:=ROUND((paystub.NightDiffPay * @_x) * @_xx, decimal_size) `NightDiffPay`,
	
	paystub.NightDiffOvertimeHours `NightDiffOvertimeHours`,
	@nightDiffOvertimePay:=ROUND((paystub.NightDiffOvertimePay * @_x) * @_xx, decimal_size) `NightDiffOvertimePay`,
	
	paystub.RestDayHours `RestDayHours`,
	@restDayPay:=ROUND((paystub.RestDayPay * @_x) * @_xx, decimal_size) `RestDayPay`,
	
	paystub.RestDayOTHours `RestDayOTHours`,
	@restDayOTPay:=ROUND((paystub.RestDayOTPay * @_x) * @_xx, decimal_size) `RestDayOTPay`,
	
	paystub.RestDayNightDiffHours `RestDayNightDiffHours`,
	@restDayNightDiffPay:=ROUND((paystub.RestDayNightDiffPay * @_x) * @_xx, decimal_size) `RestDayNightDiffPay`,
	
	paystub.RestDayNightDiffOTHours `RestDayNightDiffOTHours`,
	@restDayNightDiffOTPay:=ROUND((paystub.RestDayNightDiffOTPay * @_x) * @_xx, decimal_size) `RestDayNightDiffOTPay`,

	paystub.SpecialHolidayHours `SpecialHolidayHours`,
	@specialHolidayPay:=ROUND((paystub.SpecialHolidayPay * @_x) * @_xx, decimal_size) `SpecialHolidayPay`,
	
	paystub.SpecialHolidayOTHours `SpecialHolidayOTHours`,
	@specialHolidayOTPay:=ROUND((paystub.SpecialHolidayOTPay * @_x) * @_xx, decimal_size) `SpecialHolidayOTPay`,
	
	paystub.SpecialHolidayNightDiffHours `SpecialHolidayNightDiffHours`,
	@specialHolidayNightDiffPay:=ROUND((paystub.SpecialHolidayNightDiffPay * @_x) * @_xx, decimal_size) `SpecialHolidayNightDiffPay`,
	
	paystub.SpecialHolidayNightDiffOTHours `SpecialHolidayNightDiffOTHours`,
	@specialHolidayNightDiffOTPay:=ROUND((paystub.SpecialHolidayNightDiffOTPay * @_x) * @_xx, decimal_size) `SpecialHolidayNightDiffOTPay`,
	
	paystub.SpecialHolidayRestDayHours `SpecialHolidayRestDayHours`,
	@SpecialHolidayRestDayPay:=ROUND((paystub.SpecialHolidayRestDayPay * @_x) * @_xx, decimal_size) `SpecialHolidayRestDayPay`,
	
	paystub.SpecialHolidayRestDayOTHours `SpecialHolidayRestDayOTHours`,
	@specialHolidayRestDayOTPay:=ROUND((paystub.SpecialHolidayRestDayOTPay * @_x) * @_xx, decimal_size) `SpecialHolidayRestDayOTPay`,
	
	paystub.SpecialHolidayRestDayNightDiffHours `SpecialHolidayRestDayNightDiffHours`,
	@specialHolidayRestDayNightDiffPay:=ROUND((paystub.SpecialHolidayRestDayNightDiffPay * @_x) * @_xx, decimal_size) `SpecialHolidayRestDayNightDiffPay`,
	
	paystub.SpecialHolidayRestDayNightDiffOTHours `SpecialHolidayRestDayNightDiffOTHours`,
	@specialHolidayRestDayNightDiffOTPay:=ROUND((paystub.SpecialHolidayRestDayNightDiffOTPay * @_x) * @_xx, decimal_size) `SpecialHolidayRestDayNightDiffOTPay`,

	paystub.RegularHolidayHours `RegularHolidayHours`,
	@regularHolidayPay:=ROUND((paystub.RegularHolidayPay * @_x) * @_xx, decimal_size) `RegularHolidayPay`,
	
	paystub.RegularHolidayOTHours `RegularHolidayOTHours`,
	@regularHolidayOTPay:=ROUND((paystub.RegularHolidayOTPay * @_x) * @_xx, decimal_size) `RegularHolidayOTPay`,
	
	paystub.RegularHolidayNightDiffHours `RegularHolidayNightDiffHours`,
	@regularHolidayNightDiffPay:=ROUND((paystub.RegularHolidayNightDiffPay * @_x) * @_xx, decimal_size) `RegularHolidayNightDiffPay`,
	
	paystub.RegularHolidayNightDiffOTHours `RegularHolidayNightDiffOTHours`,
	@regularHolidayNightDiffOTPay:=ROUND((paystub.RegularHolidayNightDiffOTPay * @_x) * @_xx, decimal_size) `RegularHolidayNightDiffOTPay`,
	
	paystub.RegularHolidayRestDayHours `RegularHolidayRestDayHours`,
	@regularHolidayRestDayPay:=ROUND((paystub.RegularHolidayRestDayPay * @_x) * @_xx, decimal_size) `RegularHolidayRestDayPay`,
	
	paystub.RegularHolidayRestDayOTHours `RegularHolidayRestDayOTHours`,
	@regularHolidayRestDayOTPay:=ROUND((paystub.RegularHolidayRestDayOTPay * @_x) * @_xx, decimal_size) `RegularHolidayRestDayOTPay`,
	
	paystub.RegularHolidayRestDayNightDiffHours `RegularHolidayRestDayNightDiffHours`,
	@regularHolidayRestDayNightDiffPay:=ROUND((paystub.RegularHolidayRestDayNightDiffPay * @_x) * @_xx, decimal_size) `RegularHolidayRestDayNightDiffPay`,
	
	paystub.RegularHolidayRestDayNightDiffOTHours `RegularHolidayRestDayNightDiffOTHours`,
	@regularHolidayRestDayNightDiffOTPay:=ROUND((paystub.RegularHolidayRestDayNightDiffOTPay * @_x) * @_xx, decimal_size) `RegularHolidayRestDayNightDiffOTPay`,
	

	ROUND((paystub.HolidayPay * @_x) * @_xx, decimal_size) `HolidayPay`,
	
	paystub.LeaveHours `LeaveHours`,
	@leavePay:=ROUND((paystub.LeavePay * @_x) * @_xx, decimal_size) `LeavePay`,
	
	-paystub.LateHours `LateHours`,
	ROUND((paystub.LateDeduction * @_x) * @_xx, decimal_size) `LateDeduction`,
	
	-paystub.UndertimeHours `UndertimeHours`,
	ROUND((paystub.UndertimeDeduction * @_x) * @_xx, decimal_size) `UndertimeDeduction`,
	
	-paystub.AbsentHours `AbsentHours`,
	ROUND((paystub.AbsenceDeduction * @_x) * @_xx, decimal_size) `AbsentDeduction`,
	
	0 `TotalAllowance`,
	
	0 `TotalBonus`,
	
	@totalOvertimePay:=(@overtimePay +
	@nightDiffPay +
	@nightDiffOvertimePay +
	@restDayPay +
	@restDayOTPay +
	@restDayNightDiffPay +
	@restDayNightDiffOTPay +
	@specialHolidayPay +
	@specialHolidayOTPay +
	@specialHolidayNightDiffPay +
	@specialHolidayNightDiffOTPay +
	@SpecialHolidayRestDayPay +
	@specialHolidayRestDayOTPay +
	@specialHolidayRestDayNightDiffPay +
	@specialHolidayRestDayNightDiffOTPay +
	@regularHolidayPay +
	@regularHolidayOTPay +
	@regularHolidayNightDiffPay +
	@regularHolidayNightDiffOTPay +
	@regularHolidayRestDayPay +
	@regularHolidayRestDayOTPay +
	@regularHolidayRestDayNightDiffPay +
	@regularHolidayRestDayNightDiffOTPay)	`TotalOvertimePay`,
	
	@grossIncome:=(@regularPay + @leavePay + @totalOvertimePay) `GrossIncome`,
	
	0 `SSS`,
	0 `PhilHealth`,
	0 `HDMF`,
	0 `TaxableIncome`,
	0 `WithholdingTax`,
	0 `TotalLoans`,
#	ROUND(IFNULL(agf.DailyFee, 0), decimal_size) `AgencyFee`,
	0 `AgencyFee`,
	0 `TotalAdjustments`,
	
	@netPay:=(@grossIncome + 0) `NetPay`,
	
	@_13thMonthPay:=ROUND(IFNULL(thirteenthmonthpay.Amount, 0), decimal_size) `13thMonthPay`,
	
	(@netPay + @_13thMonthPay) `Total`,
	
	UCASE(CONCAT_WS(', ', e.LastName, e.FirstName, INITIALS(e.MiddleName, '. ', '1'))) `DatCol3`,
	UCASE(e.FirstName) 'FirstName',
	INITIALS(e.MiddleName,'. ','1') 'MiddleName',
	UCASE(e.LastName) 'LastName',
	UCASE(e.Surname) 'Surname',
	UCASE(p.PositionName) 'PositionName',
	d.Name `DatCol1`,
	CONCAT_WS(
		' to ',
		DATE_FORMAT(paystub.PayFromDate, IF(YEAR(paystub.PayFromDate) = YEAR(paystub.PayToDate), '%c/%e', '%c/%e/%Y')),
		DATE_FORMAT(paystub.PayToDate,'%c/%e/%Y')
	) `DatCol20`,
	DATE_FORMAT(paystub.PayFromDate, customDateFormat) `From`,
	DATE_FORMAT(paystub.PayToDate, customDateFormat) `To`,
	paystub.RowID AS 'PaystubId'
FROM paystub
LEFT JOIN paystubactual
ON paystubactual.EmployeeID = paystub.EmployeeID AND
	paystubactual.PayPeriodID = paystub.PayPeriodID AND
	paystubactual.OrganizationID = paystub.OrganizationID
INNER JOIN employee e
		ON e.RowID = paystub.EmployeeID
INNER JOIN `position` p
		ON p.RowID = e.PositionID
INNER JOIN division d
		ON d.RowID = p.DivisionId
LEFT JOIN (
	SELECT
		RowID,
		EmployeeID,
		SUM(DailyFee) AS DailyFee
	FROM agencyfee
	WHERE OrganizationID=ps_OrganizationID AND
		DailyFee > 0 AND
		TimeEntryDate BETWEEN paypdatefrom AND paypdateto
	GROUP BY EmployeeID
) agf
ON IFNULL(agf.RowID, 1) > 0 AND
	agf.EmployeeID=paystub.EmployeeID
LEFT JOIN thirteenthmonthpay
ON thirteenthmonthpay.OrganizationID = paystub.OrganizationID AND
	thirteenthmonthpay.PaystubID = IF(psi_undeclared, paystubactual.RowID, paystub.RowID)

WHERE paystub.OrganizationID = ps_OrganizationID AND
	(paystub.PayFromDate >= paypdatefrom OR paystub.PayToDate >= paypdatefrom) AND
	(paystub.PayFromDate <= paypdateto OR paystub.PayToDate <= paypdateto) AND
	# LENGTH(IFNULL(TRIM(e.ATMNo), '')) = IF(strSalaryDistrib = 'Cash', 0, LENGTH(IFNULL(TRIM(e.ATMNo), ''))) AND
	IF(isNullSalaryDistribType
		, TRUE
			, IF(strSalaryDistrib = 'Cash'
				, (LENGTH(IFNULL(TRIM(e.ATMNo), '')) = 0)
				, (LENGTH(IFNULL(TRIM(e.ATMNo), '')) > 0))) = TRUE AND
		-- If employee is paid monthly or daily, employee should have worked for the pay period to appear
	IF(e.EmployeeType IN ('Monthly', 'Daily'), paystub.WorkPay > 0, TRUE) # RegularHours
ORDER BY CONCAT(e.LastName, e.FirstName), paystub.PayFromDate, paystub.PayToDate;

END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
