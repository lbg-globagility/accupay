/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeeloanhistory`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeloanhistory`(
    IN `ehist_EmployeeID` INT,
    IN `ehist_OrganizationID` INT,
    IN `ehist_LoanType` VARCHAR(50)
)
    DETERMINISTIC
BEGIN

SELECT
    COALESCE(DATE_FORMAT(pyp.PayToDate, '%m/%d/%Y'), '') AS `DeductionDate`,
    COALESCE(ROUND(slp.DeductionAmount, 2), 0) AS `DeductionAmount`,
    COALESCE(els.Status, '') AS `Status`,
    COALESCE(prd.PartNo, '') AS `LoanType`,
    slp.RowID
FROM scheduledloansperpayperiod slp
INNER JOIN employeeloanschedule els
ON els.RowID = slp.EmployeeLoanRecordID
INNER JOIN payperiod pyp
ON pyp.RowiD = slp.PayPeriodID
INNER JOIN product prd
ON prd.RowID = els.LoanTypeID
WHERE els.EmployeeID = ehist_EmployeeID AND
    els.OrganizationID = ehist_OrganizationID AND
    (ehist_LoanType IS NULL) OR prd.PartNo = ehist_LoanType
ORDER BY pyp.PayToDate DESC;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
