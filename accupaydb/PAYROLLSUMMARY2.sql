/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `PAYROLLSUMMARY2`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `PAYROLLSUMMARY2`(
	IN `ps_OrganizationID` INT,
	IN `ps_PayPeriodID1` INT,
	IN `ps_PayPeriodID2` INT,
	IN `psi_undeclared` CHAR(1),
	IN `strSalaryDistrib` VARCHAR(50)
,
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

DECLARE decimal_size INT(11) DEFAULT 2;

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

SELECT
	e.RowID 'EmployeeRowID',
	e.EmployeeID `DatCol2`,
	d.RowID `DivisionID`,
	ROUND(IF(
		psi_undeclared,
		GetActualDailyRate(e.RowID, e.OrganizationID, paystub.PayFromDate),
		GET_employeerateperday(e.RowID, e.OrganizationID, paystub.PayFromDate)
	), decimal_size) `Rate`,
	ROUND(paystub.BasicHours, decimal_size) `BasicHours`,
	ROUND(IF(psi_undeclared, paystubactual.BasicPay, paystub.BasicPay), decimal_size) `BasicPay`,

	ROUND(paystub.RegularHours, decimal_size) `RegularHours`,
	ROUND(IF(psi_undeclared, paystubactual.RegularPay, paystub.RegularPay), decimal_size) `RegularPay`,
	ROUND(paystub.OvertimeHours, decimal_size) `OvertimeHours`,
	ROUND(IF(psi_undeclared, paystubactual.OvertimePay, paystub.OvertimePay), decimal_size) 'OvertimePay',
	paystub.NightDiffHours `NightDiffHours`,
	ROUND(IF(psi_undeclared, paystubactual.NightDiffPay, paystub.NightDiffPay), decimal_size) 'NightDiffPay',
	paystub.NightDiffOvertimeHours `NightDiffOvertimeHours`,
	ROUND(IF(psi_undeclared, paystubactual.NightDiffOvertimePay, paystub.NightDiffOvertimePay), decimal_size) 'NightDiffOvertimePay',
	paystub.RestDayHours `RestDayHours`,
	ROUND(IF(psi_undeclared, paystubactual.RestDayPay, paystub.RestDayPay), decimal_size) `RestDayPay`,
	paystub.RestDayOTHours `RestDayOTHours`,
	ROUND(IF(psi_undeclared, paystubactual.RestDayOTPay, paystub.RestDayOTPay), decimal_size) `RestDayOTPay`,
	paystub.RestDayNightDiffHours `RestDayNightDiffHours`,
	ROUND(IF(psi_undeclared, paystubactual.RestDayNightDiffPay, paystub.RestDayNightDiffPay), decimal_size) `RestDayNightDiffPay`,
	paystub.RestDayNightDiffOTHours `RestDayNightDiffOTHours`,
	ROUND(IF(psi_undeclared, paystubactual.RestDayNightDiffOTPay, paystub.RestDayNightDiffOTPay), decimal_size) `RestDayNightDiffOTPay`,

	paystub.SpecialHolidayHours `SpecialHolidayHours`,
	ROUND(IF(psi_undeclared, paystubactual.SpecialHolidayPay, paystub.SpecialHolidayPay), decimal_size) `SpecialHolidayPay`,
	paystub.SpecialHolidayOTHours `SpecialHolidayOTHours`,
	ROUND(IF(psi_undeclared, paystubactual.SpecialHolidayOTPay, paystub.SpecialHolidayOTPay), decimal_size) `SpecialHolidayOTPay`,
	paystub.SpecialHolidayNightDiffHours `SpecialHolidayNightDiffHours`,
	ROUND(IF(psi_undeclared, paystubactual.SpecialHolidayNightDiffPay, paystub.SpecialHolidayNightDiffPay), decimal_size) `SpecialHolidayNightDiffPay`,
	paystub.SpecialHolidayNightDiffOTHours `SpecialHolidayNightDiffOTHours`,
	ROUND(IF(psi_undeclared, paystubactual.SpecialHolidayNightDiffOTPay, paystub.SpecialHolidayNightDiffOTPay), decimal_size) `SpecialHolidayNightDiffOTPay`,
	paystub.SpecialHolidayRestDayHours `SpecialHolidayRestDayHours`,
	ROUND(IF(psi_undeclared, paystubactual.SpecialHolidayRestDayPay, paystub.SpecialHolidayRestDayPay), decimal_size) `SpecialHolidayRestDayPay`,
	paystub.SpecialHolidayRestDayOTHours `SpecialHolidayRestDayOTHours`,
	ROUND(IF(psi_undeclared, paystubactual.SpecialHolidayRestDayOTPay, paystub.SpecialHolidayRestDayOTPay), decimal_size) `SpecialHolidayRestDayOTPay`,
	paystub.SpecialHolidayRestDayNightDiffHours `SpecialHolidayRestDayNightDiffHours`,
	ROUND(IF(psi_undeclared, paystubactual.SpecialHolidayRestDayNightDiffPay, paystub.SpecialHolidayRestDayNightDiffPay), decimal_size) `SpecialHolidayRestDayNightDiffPay`,
	paystub.SpecialHolidayRestDayNightDiffOTHours `SpecialHolidayRestDayNightDiffOTHours`,
	ROUND(IF(psi_undeclared, paystubactual.SpecialHolidayRestDayNightDiffOTPay, paystub.SpecialHolidayRestDayNightDiffOTPay), decimal_size) `SpecialHolidayRestDayNightDiffOTPay`,

	paystub.RegularHolidayHours `RegularHolidayHours`,
	ROUND(IF(psi_undeclared, paystubactual.RegularHolidayPay, paystub.RegularHolidayPay), decimal_size) `RegularHolidayPay`,
	paystub.RegularHolidayOTHours `RegularHolidayOTHours`,
	ROUND(IF(psi_undeclared, paystubactual.RegularHolidayOTPay, paystub.RegularHolidayOTPay), decimal_size) `RegularHolidayOTPay`,
	paystub.RegularHolidayNightDiffHours `RegularHolidayNightDiffHours`,
	ROUND(IF(psi_undeclared, paystubactual.RegularHolidayNightDiffPay, paystub.RegularHolidayNightDiffPay), decimal_size) `RegularHolidayNightDiffPay`,
	paystub.RegularHolidayNightDiffOTHours `RegularHolidayNightDiffOTHours`,
	ROUND(IF(psi_undeclared, paystubactual.RegularHolidayNightDiffOTPay, paystub.RegularHolidayNightDiffOTPay), decimal_size) `RegularHolidayNightDiffOTPay`,
	paystub.RegularHolidayRestDayHours `RegularHolidayRestDayHours`,
	ROUND(IF(psi_undeclared, paystubactual.RegularHolidayRestDayPay, paystub.RegularHolidayRestDayPay), decimal_size) `RegularHolidayRestDayPay`,
	paystub.RegularHolidayRestDayOTHours `RegularHolidayRestDayOTHours`,
	ROUND(IF(psi_undeclared, paystubactual.RegularHolidayRestDayOTPay, paystub.RegularHolidayRestDayOTPay), decimal_size) `RegularHolidayRestDayOTPay`,
	paystub.RegularHolidayRestDayNightDiffHours `RegularHolidayRestDayNightDiffHours`,
	ROUND(IF(psi_undeclared, paystubactual.RegularHolidayRestDayNightDiffPay, paystub.RegularHolidayRestDayNightDiffPay), decimal_size) `RegularHolidayRestDayNightDiffPay`,
	paystub.RegularHolidayRestDayNightDiffOTHours `RegularHolidayRestDayNightDiffOTHours`,
	ROUND(IF(psi_undeclared, paystubactual.RegularHolidayRestDayNightDiffOTPay, paystub.RegularHolidayRestDayNightDiffOTPay), decimal_size) `RegularHolidayRestDayNightDiffOTPay`,

	ROUND(IF(psi_undeclared, paystubactual.HolidayPay, paystub.HolidayPay), decimal_size) 'HolidayPay',
	paystub.LeaveHours `LeaveHours`,
	ROUND(IF(psi_undeclared, paystubactual.LeavePay, paystub.LeavePay), decimal_size) `LeavePay`,
	-paystub.LateHours `LateHours`,
	ROUND(IF(psi_undeclared, -paystubactual.LateDeduction, -paystub.LateDeduction), decimal_size) 'LateDeduction',
	-paystub.UndertimeHours `UndertimeHours`,
	ROUND(IF(psi_undeclared, -paystubactual.UndertimeDeduction, -paystub.UndertimeDeduction), decimal_size) 'UndertimeDeduction',
	-paystub.AbsentHours `AbsentHours`,
	ROUND(IF(psi_undeclared, -paystubactual.AbsenceDeduction, -paystub.AbsenceDeduction), decimal_size) 'AbsentDeduction',
	paystub.TotalAllowance + paystub.TotalTaxableAllowance `TotalAllowance`,
	ROUND(paystub.TotalBonus, decimal_size) `TotalBonus`,
	ROUND(IF(psi_undeclared, paystubactual.TotalGrossSalary, paystub.TotalGrossSalary), decimal_size) `GrossIncome`,
	ROUND(-paystub.TotalEmpSSS, decimal_size) `SSS`,
	ROUND(-paystub.TotalEmpPhilhealth, decimal_size) `PhilHealth`,
	ROUND(-paystub.TotalEmpHDMF, decimal_size) `HDMF`,
	ROUND(paystub.TotalTaxableSalary, decimal_size) `TaxableIncome`,
	ROUND(-paystub.TotalEmpWithholdingTax, decimal_size) `WithholdingTax`,
	ROUND(-paystub.TotalLoans, decimal_size) `TotalLoans`,
	ROUND(IFNULL(agf.DailyFee, 0), decimal_size) `AgencyFee`,
	ROUND(IF(psi_undeclared, paystubactual.TotalAdjustments, paystub.TotalAdjustments), decimal_size) `TotalAdjustments`,
	ROUND(IF(
		psi_undeclared,
		paystubactual.TotalNetSalary + IFNULL(agf.DailyFee, 0),
		paystub.TotalNetSalary + IFNULL(agf.DailyFee, 0)
	), decimal_size) `NetPay`,
	ROUND(IFNULL(thirteenthmonthpay.Amount, 0), decimal_size) `13thMonthPay`,
	ROUND((IF(psi_undeclared, paystubactual.TotalNetSalary, paystub.TotalNetSalary) + IFNULL(thirteenthmonthpay.Amount,0) + IFNULL(agf.DailyFee, 0)), decimal_size) `Total`,
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

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
