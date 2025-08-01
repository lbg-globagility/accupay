/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_LoansByType`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_LoansByType`(IN `OrganizID` INT, IN `PayDateFrom` DATE, IN `PayDateTo` DATE)
    DETERMINISTIC
BEGIN

SELECT
    p.PartNo `DatCol1`,
    ee.EmployeeID `DatCol2`,
    CONCAT(
        ee.LastName,
        ',',
        ee.FirstName,
        IF(ee.MiddleName = '', '', ','),
        INITIALS(ee.MiddleName, '. ', '1')
    ) `DatCol3`,
    FORMAT(SUM(IFNULL(slp.DeductionAmount, 0)), 2) `DatCol4`,
    FORMAT(IFNULL(els.TotalLoanAmount, 0), 2) `DatCol5`,
    @currentBalance := (ROUND(IFNULL(els.TotalLoanAmount, 0),2) - ROUND(IFNULL(
	 	(SELECT SUM(scheduledloansperpayperiod.DeductionAmount)
		FROM scheduledloansperpayperiod
		INNER JOIN payperiod
		ON scheduledloansperpayperiod.PayPeriodID = payperiod.RowID
		WHERE scheduledloansperpayperiod.EmployeeLoanRecordID = els.RowID
		AND payperiod.PayToDate <= PayDateTo)
	 
	 , 0),2)) `CurrentBalance`,
    FORMAT(IF(@currentBalance < 0, (slp.DeductionAmount + @currentBalance), @currentBalance),2) `DatCol6`,
    CONCAT(
	 		DATE_FORMAT((SELECT MIN(payperiod.PayFromDate) FROM scheduledloansperpayperiod
			INNER JOIN payperiod
			ON scheduledloansperpayperiod.PayPeriodID = payperiod.RowID
			WHERE scheduledloansperpayperiod.EmployeeLoanRecordID = els.RowID
			AND payperiod.PayToDate <= PayDateTo), '%m/%d/%Y'),
			' to ',
	 		DATE_FORMAT((SELECT MAX(payperiod.PayToDate) FROM scheduledloansperpayperiod
			INNER JOIN payperiod
			ON scheduledloansperpayperiod.PayPeriodID = payperiod.RowID
			WHERE scheduledloansperpayperiod.EmployeeLoanRecordID = els.RowID
			AND payperiod.PayToDate <= PayDateTo), '%m/%d/%Y')) AS `DatCol7`,
    els.LoanNumber AS `DatCol8`
FROM scheduledloansperpayperiod slp
INNER JOIN employeeloanschedule els
ON els.RowID = slp.EmployeeLoanRecordID
INNER JOIN employee ee
ON ee.RowID = slp.EmployeeID
INNER JOIN product p
ON p.RowID = els.LoanTypeID
INNER JOIN payperiod pp
ON pp.RowID = slp.PayPeriodID
WHERE slp.OrganizationID = OrganizID AND
    pp.PayFromDate BETWEEN PayDateFrom AND PayDateTo
GROUP BY slp.EmployeeID, els.RowID
ORDER BY p.PartNo, ee.LastName, pp.PayFromDate;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
