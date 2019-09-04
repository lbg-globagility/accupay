/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_13thmonthpay`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_13thmonthpay`(IN `$organizationID` INT, IN `$dateFrom` Date, IN `$dateTo` Date)
    DETERMINISTIC
BEGIN

DECLARE $endDateFrom DATE;

SET $endDateFrom = (
    SELECT PayFromDate
    FROM payperiod p
    WHERE p.PayToDate = $dateTo AND
        p.OrganizationID = $organizationID AND 
		p.TotalGrossSalary = 1); #Added because there is an error here if payperiod has weekly data. This should be improved to support weekly in the future.

SELECT
    e.EmployeeID AS DatCol1,
    CONCAT_WS(', ',IF(e.LastName = '', NULL, e.LastName), e.FirstName) AS DatCol2,
    FORMAT(SUM(t.BasicPay), 2) AS DatCol3,
    FORMAT(SUM(t.Amount), 2) AS DatCol4
FROM thirteenthmonthpay t
INNER JOIN paystub p
ON p.RowID = t.PaystubID
INNER JOIN employee e
ON e.RowID = p.EmployeeID AND
    e.OrganizationID = $organizationID AND
    FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
WHERE t.OrganizationID = $organizationID AND
    p.PayFromDate BETWEEN $dateFrom AND $endDateFrom
GROUP BY p.EmployeeID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
