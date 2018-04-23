/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RECOMPUTE_thirteenthmonthpay`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RECOMPUTE_thirteenthmonthpay`(
	IN `OrganizID` INT,
	IN `PayPRowID` INT,
	IN `UserRowID` INT
)
    DETERMINISTIC
BEGIN

DECLARE ispayperiodendofmonth TEXT;

DECLARE newvalue DECIMAL(11,6);

DECLARE payp_month TEXT;

DECLARE payp_year INT;

DECLARE emppayfreqID INT(11);

DECLARE paypmonthlyID VARCHAR(50);

DECLARE last_date DATE;

DECLARE month_firstdate DATE;

DECLARE pf_div INT;

DECLARE overtimeRate DECIMAL(10, 2);

DECLARE HOURS_IN_A_WORKDAY INT DEFAULT 8;

SELECT
    pyp.`Half`,
    pyp.`Month`,
    pyp.`Year`,
    pyp.TotalGrossSalary,
    pyp.PayFromDate,
    pyp.PayToDate,
    MONTH( SUBDATE(MAKEDATE(YEAR(CURDATE()), 1), INTERVAL 1 DAY) )
FROM payperiod pyp
INNER JOIN payfrequency pf
ON pf.RowID = pyp.TotalGrossSalary
WHERE pyp.RowID = PayPRowID
INTO
    ispayperiodendofmonth,
    payp_month,
    payp_year,
    emppayfreqID,
    month_firstdate,
    last_date,
    pf_div;

SELECT payrate.OvertimeRate
FROM payrate
WHERE payrate.Date = last_date AND
    payrate.OrganizationID = OrganizID
INTO overtimeRate;

SELECT GROUP_CONCAT(RowID)
FROM payperiod pp
WHERE pp.OrganizationID = OrganizID AND
    pp.TotalGrossSalary = 1 AND
    pp.`Year` = payp_year AND
    pp.`Month` = payp_month
ORDER BY
    pp.PayFromDate DESC,
    pp.PayToDate DESC
INTO paypmonthlyID;

UPDATE employeeloanschedule els
INNER JOIN scheduledloansperpayperiod slp
ON slp.EmployeeLoanRecordID = els.RowID
LEFT OUTER JOIN scheduledloansperpayperiod slp2
ON (
    els.RowID = slp2.EmployeeLoanRecordID AND
    slp.PayPeriodID < slp2.PayPeriodID
)
INNER JOIN payperiod pyp ON pyp.RowID=slp.PayPeriodID
SET
    els.LastUpd = CURRENT_TIMESTAMP(),
    els.LastUpdBy = UserRowID,
    els.LoanPayPeriodLeft = slp.LoanPayPeriodLeft,
    els.TotalBalanceLeft = slp.TotalBalanceLeft
WHERE slp2.RowID IS NULL AND
    (
        els.Status = 'In Progress' OR
        pyp.PayFromDate >= month_firstdate
    ) AND
    slp.OrganizationID = OrganizID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
