/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeeallowance`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeallowance`(IN `eallow_EmployeeID` INT, IN `eallow_OrganizationID` INT)
    DETERMINISTIC
BEGIN


SELECT eall.RowID
,IFNULL(p.PartNo,'') 'Type'
,COALESCE(eall.AllowanceAmount,0) 'AllowanceAmount'
,IFNULL(eall.AllowanceFrequency,'') 'AllowanceFrequency'
,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
,IF(IFNULL(TaxableFlag,0) = 0,'No','Yes') 'TaxableFlag'
,IFNULL(eall.ProductID,'') 'ProductID'
 FROM employeeallowance eall
 LEFT JOIN product p ON eall.ProductID=p.RowID
 WHERE eall.EmployeeID=eallow_EmployeeID
 AND eall.OrganizationID=eallow_OrganizationID;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
