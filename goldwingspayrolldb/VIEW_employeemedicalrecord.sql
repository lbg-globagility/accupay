/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeemedicalrecord`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeemedicalrecord`(IN `emedr_RowID` INT, IN `emedr_OrganizationID` INT)
    DETERMINISTIC
    COMMENT 'view the employee''s medical record(s) base on date from-to, employee RowID and organization'
BEGIN

SELECT
COALESCE(DATE_FORMAT(emed.DateFrom,'%m/%d/%Y'),'') 'DateFrom'
,COALESCE(DATE_FORMAT(emed.DateTo,'%m/%d/%Y'),'') 'DateTo'
,emed.EmployeeID
FROM employeemedicalrecord emed
WHERE emed.EmployeeID=emedr_RowID
AND emed.OrganizationID=emedr_OrganizationID
GROUP BY emed.DateFrom,emed.DateTo;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
