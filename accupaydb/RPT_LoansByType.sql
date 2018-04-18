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
    FORMAT(els.TotalLoanAmount, 2) `DatCol5`,
    FORMAT(els.TotalBalanceLeft, 2) `DatCol6`
FROM scheduledloansperpayperiod slp
INNER JOIN employeeloanschedule els
ON els.RowID = slp.EmployeeLoanRecordID
INNER JOIN employee ee
ON ee.RowID = slp.EmployeeID AND FIND_IN_SET(ee.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
INNER JOIN product p
ON p.RowID = els.LoanTypeID
INNER JOIN payperiod pp
ON pp.RowID = slp.PayPeriodID
WHERE slp.OrganizationID = OrganizID AND
    pp.PayFromDate BETWEEN PayDateFrom AND PayDateTo
GROUP BY slp.EmployeeID, els.RowID
ORDER BY p.PartNo, ee.LastName;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
