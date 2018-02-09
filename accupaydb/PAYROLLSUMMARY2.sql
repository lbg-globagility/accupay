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
INTO min_paydatefrom
     ,max_paydateto;

IF is_keep_in_onesheet = TRUE THEN

		SELECT
		    e.RowID 'EmployeeRowID',
		    e.EmployeeID `DatCol2`,
			ROUND(IF(
				psi_undeclared,
				GetActualDailyRate(e.RowID, e.OrganizationID, paystub.PayFromDate),
				GET_employeerateperday(e.RowID, e.OrganizationID, paystub.PayFromDate)
			), decimal_size) `Rate`,
		    ROUND(GetBasicPay(e.RowID, paypdatefrom, paypdateto, psi_undeclared, IFNULL(ete.`TotalExpectedHours`, 0)), decimal_size) `BasicPay`,
			ROUND(paystub.RegularHours, decimal_size) `RegularHours`,
		    ROUND(IF(psi_undeclared, paystubactual.RegularPay, paystub.RegularPay), decimal_size) `RegularPay`,
			ROUND(paystub.OvertimeHours, decimal_size) `OvertimeHours`,
		    ROUND(IF(psi_undeclared, paystubactual.OvertimePay, paystub.OvertimePay), decimal_size) 'OvertimePay',
			paystub.NightDiffHours `NightDiffHours`,
		    ROUND(IF(psi_undeclared, paystubactual.NightDiffPay, paystub.NightDiffPay), decimal_size) 'NightDiffPay',
			paystub.NightDiffOvertimeHours `NightDiffOvertimeHours`,
		    ROUND(IF(psi_undeclared, paystubactual.NightDiffOvertimePay, paystub.NightDiffOvertimePay), decimal_size) 'NightDiffOvertimePay',
			paystub.SpecialHolidayHours `SpecialHolidayHours`,
			paystub.RegularHolidayHours `RegularHolidayHours`,
			ROUND(IF(psi_undeclared, paystubactual.HolidayPay, paystub.HolidayPay), decimal_size) 'HolidayPay',
			paystub.RestDayHours `RestDayHours`,
			ROUND(IF(psi_undeclared, paystubactual.RestDayPay, paystub.RestDayPay), decimal_size) `RestDayPay`,
			paystub.RestDayOTHours `RestDayOTHours`,
			ROUND(IF(psi_undeclared, paystubactual.RestDayOTPay, paystub.RestDayOTPay), decimal_size) `RestDayOTPay`,
			paystub.LeaveHours `LeaveHours`,
			ROUND(IF(psi_undeclared, paystubactual.LeavePay, paystub.LeavePay), decimal_size) 'LeavePay',
			-paystub.LateHours `LateHours`,
		    ROUND(IF(psi_undeclared, -paystubactual.LateDeduction, -paystub.LateDeduction), decimal_size) 'LateDeduction',
		 	-paystub.UndertimeHours `UndertimeHours`,
		    ROUND(IF(psi_undeclared, -paystubactual.UndertimeDeduction, -paystub.UndertimeDeduction), decimal_size) 'UndertimeDeduction',
			-paystub.AbsentHours `AbsentHours`,
		    ROUND(IF(psi_undeclared, -paystubactual.AbsenceDeduction, -paystub.AbsenceDeduction), decimal_size) 'AbsentDeduction',
			paystub.TotalAllowance `TotalAllowance`,
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
		    '' `DatCol1`,
		    CONCAT_WS(
		        ' to ',
		        DATE_FORMAT(paystub.PayFromDate, IF(YEAR(paystub.PayFromDate) = YEAR(paystub.PayToDate), '%c/%e', '%c/%e/%Y')),
		        DATE_FORMAT(paystub.PayToDate,'%c/%e/%Y')
		    ) `DatCol20`
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

		LEFT JOIN (SELECT et.EmployeeID
		           ,SUM(et.TotalExpectedHours) `TotalExpectedHours`
		           FROM expectedhours et
					  WHERE et.OrganizationID = ps_OrganizationID
					  AND et.`Date` BETWEEN min_paydatefrom AND max_paydateto
					  GROUP BY et.EmployeeID) ete
		       ON ete.EmployeeID = paystub.EmployeeID

		WHERE paystub.OrganizationID = ps_OrganizationID AND
		    (paystub.PayFromDate >= paypdatefrom OR paystub.PayToDate >= paypdatefrom) AND
		    (paystub.PayFromDate <= paypdateto OR paystub.PayToDate <= paypdateto) AND
		    # LENGTH(IFNULL(TRIM(e.ATMNo), '')) = IF(strSalaryDistrib = 'Cash', 0, LENGTH(IFNULL(TRIM(e.ATMNo), ''))) AND
		    IF(strSalaryDistrib = 'Cash'
		       , (LENGTH(IFNULL(TRIM(e.ATMNo), '')) = 0)
		       , (LENGTH(IFNULL(TRIM(e.ATMNo), '')) > 0)) = TRUE AND
			 -- If employee is paid monthly or daily, employee should have worked for the pay period to appear
		    IF(e.EmployeeType IN ('Monthly', 'Daily'), paystub.RegularHours > 0, TRUE)
		ORDER BY CONCAT(e.LastName, e.FirstName);

ELSE

	SELECT COUNT(i.RowID)
	FROM (SELECT dv.RowID
			FROM employee e
			INNER JOIN `position` pos ON pos.RowID=e.PositionID
			INNER JOIN division dv ON dv.RowID=pos.DivisionId
			WHERE e.OrganizationID = ps_OrganizationID
			GROUP BY dv.RowID) i
	INTO div_count;

	SET div_index = 0;

	WHILE div_index < div_count DO

		SELECT i.`RowID`
		FROM (SELECT dv.RowID
				FROM employee e
				INNER JOIN `position` pos ON pos.RowID=e.PositionID
				INNER JOIN division dv ON dv.RowID=pos.DivisionId
				WHERE e.OrganizationID = ps_OrganizationID
				GROUP BY dv.RowID
				ORDER BY dv.Name) i
		LIMIT div_index, 1
		INTO div_rowid;

		# SELECT div_rowid;

		SELECT
		    e.RowID 'EmployeeRowID',
			d.Name `DatCol1`,
		    e.EmployeeID `DatCol2`,
			ROUND(IF(
				psi_undeclared,
				GetActualDailyRate(e.RowID, e.OrganizationID, paystub.PayFromDate),
				GET_employeerateperday(e.RowID, e.OrganizationID, paystub.PayFromDate)
			), decimal_size) `Rate`,
		    ROUND(GetBasicPay(e.RowID, paypdatefrom, paypdateto, psi_undeclared, IFNULL(ete.`TotalExpectedHours`, 0)), decimal_size) `BasicPay`,
			ROUND(paystub.RegularHours, decimal_size) `RegularHours`,
		    ROUND(IF(psi_undeclared, paystubactual.RegularPay, paystub.RegularPay), decimal_size) `RegularPay`,
			ROUND(paystub.OvertimeHours, decimal_size) `OvertimeHours`,
		    ROUND(IF(psi_undeclared, paystubactual.OvertimePay, paystub.OvertimePay), decimal_size) 'OvertimePay',
			paystub.NightDiffHours `NightDiffHours`,
		    ROUND(IF(psi_undeclared, paystubactual.NightDiffPay, paystub.NightDiffPay), decimal_size) 'NightDiffPay',
			paystub.NightDiffOvertimeHours `NightDiffOvertimeHours`,
		    ROUND(IF(psi_undeclared, paystubactual.NightDiffOvertimePay, paystub.NightDiffOvertimePay), decimal_size) 'NightDiffOvertimePay',
			paystub.SpecialHolidayHours `SpecialHolidayHours`,
			paystub.RegularHolidayHours `RegularHolidayHours`,
			ROUND(IF(psi_undeclared, paystubactual.HolidayPay, paystub.HolidayPay), decimal_size) 'HolidayPay',
			paystub.RestDayHours `RestDayHours`,
			ROUND(IF(psi_undeclared, paystubactual.RestDayPay, paystub.RestDayPay), decimal_size) `RestDayPay`,
			paystub.RestDayOTHours `RestDayOTHours`,
			ROUND(IF(psi_undeclared, paystubactual.RestDayOTPay, paystub.RestDayOTPay), decimal_size) `RestDayOTPay`,
			paystub.LeaveHours `LeaveHours`,
			ROUND(IF(psi_undeclared, paystubactual.LeavePay, paystub.LeavePay), decimal_size) 'LeavePay',
			-paystub.LateHours `LateHours`,
		    ROUND(IF(psi_undeclared, -paystubactual.LateDeduction, -paystub.LateDeduction), decimal_size) 'LateDeduction',
		 	-paystub.UndertimeHours `UndertimeHours`,
		    ROUND(IF(psi_undeclared, -paystubactual.UndertimeDeduction, -paystub.UndertimeDeduction), decimal_size) 'UndertimeDeduction',
			-paystub.AbsentHours `AbsentHours`,
		    ROUND(IF(psi_undeclared, -paystubactual.AbsenceDeduction, -paystub.AbsenceDeduction), decimal_size) 'AbsentDeduction',
			paystub.TotalAllowance `TotalAllowance`,
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
		    CONCAT_WS(
		        ' to ',
		        DATE_FORMAT(paystub.PayFromDate, IF(YEAR(paystub.PayFromDate) = YEAR(paystub.PayToDate), '%c/%e', '%c/%e/%Y')),
		        DATE_FORMAT(paystub.PayToDate,'%c/%e/%Y')
		    ) `DatCol20`
		FROM paystub
		LEFT JOIN paystubactual
		ON paystubactual.EmployeeID = paystub.EmployeeID AND
		    paystubactual.PayPeriodID = paystub.PayPeriodID
		INNER JOIN employee e
		        ON e.RowID = paystub.EmployeeID
		INNER JOIN `position` p
		        ON p.RowID = e.PositionID
		INNER JOIN division d
		        ON d.RowID = p.DivisionId AND d.RowID = div_rowid
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

		LEFT JOIN (SELECT et.EmployeeID
		           ,SUM(et.TotalExpectedHours) `TotalExpectedHours`
		           FROM expectedhours et
					  WHERE et.OrganizationID = ps_OrganizationID
					  AND et.`Date` BETWEEN min_paydatefrom AND max_paydateto
					  GROUP BY et.EmployeeID) ete
		       ON ete.EmployeeID = paystub.EmployeeID

		WHERE paystub.OrganizationID = ps_OrganizationID AND
		    (paystub.PayFromDate >= paypdatefrom OR paystub.PayToDate >= paypdatefrom) AND
		    (paystub.PayFromDate <= paypdateto OR paystub.PayToDate <= paypdateto) AND
		    # LENGTH(IFNULL(TRIM(e.ATMNo), '')) = IF(strSalaryDistrib = 'Cash', 0, LENGTH(IFNULL(TRIM(e.ATMNo), ''))) AND
		    IF(strSalaryDistrib = 'Cash'
		       , (LENGTH(IFNULL(TRIM(e.ATMNo), '')) = 0)
		       , (LENGTH(IFNULL(TRIM(e.ATMNo), '')) > 0)) = TRUE AND
			 -- If employee is paid monthly or daily, employee should have worked for the pay period to appear
		    IF(e.EmployeeType IN ('Monthly', 'Daily'), paystub.RegularHours > 0, TRUE)
		ORDER BY d.Name, e.LastName;

		SET div_index = (div_index + 1);

	END WHILE;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
