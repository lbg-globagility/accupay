/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_loans`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_loans`(IN `OrganizID` INT, IN `PayDateFrom` DATE, IN `PayDateTo` DATE, IN `LoanTypeID` INT)
    DETERMINISTIC
BEGIN

SELECT
    /*p.PartNo 'Comments',
    ee.EmployeeID,
    CONCAT(
        ee.LastName,
        ',',
        ee.FirstName,
        IF(ee.MiddleName = '', '', ','),
        INITIALS(ee.MiddleName, '. ', '1')
    ) 'Fullname',
    FORMAT(SUM(IFNULL(slp.DeductionAmount, 0)), 2) 'DeductionAmount',
    FORMAT(els.TotalLoanAmount, 2),
    FORMAT(els.TotalBalanceLeft, 2)*/
    ee.EmployeeID `DatCol1`
    ,CONCAT_WS(', ', ee.LastName, ee.FirstName, ee.MiddleName) `DatCol2`
    ,p.PartNo `DatCol3`
    ,ROUND(slp.DeductionAmount, 2) `DatCol4`
    ,ROUND(slp.TotalBalanceLeft, 2) `DatCol5`
    ,ROUND(els.TotalLoanAmount, 2) `DatCol6`
FROM scheduledloansperpayperiod slp
INNER JOIN employeeloanschedule els
ON els.RowID = slp.EmployeeLoanRecordID
INNER JOIN employee ee
ON ee.RowID = slp.EmployeeID
INNER JOIN product p
ON p.RowID = els.LoanTypeID
INNER JOIN payperiod pp
ON pp.RowID = slp.PayPeriodID
WHERE slp.OrganizationID = OrganizID
# AND pp.PayFromDate BETWEEN PayDateFrom AND PayDateTo
AND (pp.PayFromDate >= PayDateFrom OR pp.PayToDate >= PayDateFrom)
AND (pp.PayFromDate <= PayDateTo OR pp.PayToDate <= PayDateTo)
# GROUP BY slp.EmployeeID, els.RowID
ORDER BY CONCAT(ee.LastName, ee.FirstName), p.PartNo;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
