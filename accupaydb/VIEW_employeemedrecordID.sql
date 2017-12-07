/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `VIEW_employeemedrecordID`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `VIEW_employeemedrecordID`(`emedrecord_EmployeeID` INT, `emedrecord_DateFrom` DATE, `emedrecord_DateTo` DATE, `emedrecord_ProductID` INT, `emedrecord_OrganizationID` INT) RETURNS int(11)
    DETERMINISTIC
    COMMENT 'return the RowID from employeemedicalrecord base on date from - to, ProductID, employeeID and organization'
BEGIN

DECLARE empmedrecordID INT(11) DEFAULT 0;

SELECT emedrecord.RowID
FROM employeemedicalrecord emedrecord
WHERE COALESCE(emedrecord.EmployeeID,0)=COALESCE(emedrecord_EmployeeID,0)
AND COALESCE(emedrecord.ProductID,0)=COALESCE(emedrecord_ProductID,0)
AND emedrecord.OrganizationID=emedrecord_OrganizationID
AND emedrecord.DateFrom=emedrecord_DateFrom
AND IF(emedrecord.DateTo IS NULL, emedrecord.DateTo IS NULL, emedrecord.DateTo=emedrecord_DateTo)
LIMIT 1
INTO empmedrecordID;

RETURN empmedrecordID;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
