/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `PAYROLLSUMMARY2`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `PAYROLLSUMMARY2`(IN `ps_OrganizationID` INT, IN `ps_PayPeriodID1` INT, IN `ps_PayPeriodID2` INT, IN `psi_undeclared` CHAR(1), IN `strSalaryDistrib` VARCHAR(50)
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
	    e.EmployeeID `DatCol2`,
	    # IF(psi_undeclared, paystubactual.RegularPay, paystub.RegularPay) `DatCol21`,
	    ROUND(GetBasicPay(e.RowID, paypdatefrom, psi_undeclared, IFNULL(ete.`TotalExpectedHours`, 0)), decimal_size) `DatCol21`,
	    ROUND(IF(psi_undeclared, paystubactual.OvertimePay, paystub.OvertimePay), decimal_size) 'DatCol37',
	    ROUND(IF(psi_undeclared, paystubactual.NightDiffPay, paystub.NightDiffPay), decimal_size) 'DatCol35',
	    ROUND(IF(psi_undeclared, paystubactual.NightDiffOvertimePay, paystub.NightDiffOvertimePay), decimal_size) 'DatCol38',
	    ROUND(IF(psi_undeclared, paystubactual.HolidayPay, paystub.HolidayPay), decimal_size) 'DatCol36',
	    ROUND(IF(psi_undeclared, paystubactual.LateDeduction, paystub.LateDeduction), decimal_size) 'DatCol33',
	    ROUND(IF(psi_undeclared, paystubactual.UndertimeDeduction, paystub.UndertimeDeduction), decimal_size) 'DatCol34',
	    ROUND(IF(psi_undeclared, paystubactual.AbsenceDeduction, paystub.AbsenceDeduction), decimal_size) 'DatCol32',
	    ROUND(paystub.TotalBonus, decimal_size) `DatCol30`,
	    paystub.TotalAllowance `DatCol31`,
	    ROUND(IF(psi_undeclared, paystubactual.TotalGrossSalary, paystub.TotalGrossSalary), decimal_size) `DatCol22`,
	    ROUND(IF(
				        psi_undeclared,
				        paystubactual.TotalNetSalary + IFNULL(agf.DailyFee, 0),
				        paystub.TotalNetSalary + IFNULL(agf.DailyFee, 0)
				    ), decimal_size) `DatCol23`,
	    ROUND(paystub.TotalTaxableSalary, decimal_size) `DatCol24`,
	    ROUND(paystub.TotalEmpSSS, decimal_size) `DatCol25`,
	    ROUND(paystub.TotalEmpPhilhealth, decimal_size) `DatCol27`,
	    ROUND(paystub.TotalEmpHDMF, decimal_size) `DatCol28`,
	    ROUND(paystub.TotalEmpWithholdingTax, decimal_size) `DatCol26`,
	    ROUND(paystub.TotalLoans, decimal_size) `DatCol29`,
	    UCASE(CONCAT_WS(', ', e.LastName, e.FirstName, INITIALS(e.MiddleName,'. ','1'))) `DatCol3`,
	    UCASE(e.FirstName) 'FirstName',
	    INITIALS(e.MiddleName,'. ','1') 'MiddleName',
	    UCASE(e.LastName) 'LastName',
	    UCASE(e.Surname) 'Surname',
	    UCASE(p.PositionName) 'PositionName',
	    UCASE(d.Name) `DatCol1`,
	    ROUND(IFNULL(agf.DailyFee,0), decimal_size) `DatCol39`,
	    ROUND(IFNULL(thirteenthmonthpay.Amount,0), decimal_size) `DatCol40`,
	    CONCAT_WS(
	        ' to ',
	        DATE_FORMAT(paystub.PayFromDate, IF(YEAR(paystub.PayFromDate) = YEAR(paystub.PayToDate), '%c/%e', '%c/%e/%Y')),
	        DATE_FORMAT(paystub.PayToDate,'%c/%e/%Y')
	    ) `DatCol20`,
	    ROUND(paystub.RegularHours, decimal_size) `DatCol41`,
	    ROUND((IF(psi_undeclared, paystubactual.TotalNetSalary, paystub.TotalNetSalary) + IFNULL(thirteenthmonthpay.Amount,0) + IFNULL(agf.DailyFee, 0)), decimal_size) `DatCol42`,
	    ROUND(IF(
				        psi_undeclared,
				        GetActualDailyRate(e.RowID, e.OrganizationID, paystub.PayFromDate),
				        GET_employeerateperday(e.RowID, e.OrganizationID, paystub.PayFromDate)
				    ), decimal_size) `DatCol43`,
	    ROUND(paystub.OvertimeHours, decimal_size) `DatCol44`,
	    ROUND(paystub.TotalAdjustments, decimal_size) `DatCol45`,
	    ROUND(paystub.RestDayPay, decimal_size) `DatCol46`
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
	           # ,SUM(sh.DivisorToDailyRate) `TotalExpectedHours`
	           ,SUM(
						 IF((TIMESTAMPDIFF(SECOND
						                   , CONCAT_DATETIME(et.`Date`, sh.TimeFrom)
												 , CONCAT_DATETIME(ADDDATE(et.`Date`, INTERVAL IS_TIMERANGE_REACHTOMORROW(sh.TimeFrom, sh.TimeTo) DAY), sh.TimeTo)) / sec_per_hour)
						    > (et.RegularHoursWorked + (et.HoursLate + et.UndertimeHours))
							   
							 , (et.RegularHoursWorked + (et.HoursLate + et.UndertimeHours))
						   , sh.DivisorToDailyRate)
				       ) `TotalExpectedHours`
	           
				  FROM employeetimeentry et
				  INNER JOIN employee ee
					       ON ee.RowID=et.EmployeeID
							    AND ee.OrganizationID=et.OrganizationID
								 AND ee.EmploymentStatus NOT IN ('Resigned', 'Terminated')
				  INNER JOIN `position` pos ON pos.RowID=ee.PositionID AND pos.DivisionId = div_rowid
				  INNER JOIN employeeshift esh
					       ON esh.RowID=et.EmployeeShiftID
				  INNER JOIN shift sh
					       ON sh.RowID=esh.ShiftID
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
	
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
