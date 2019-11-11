/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeebonus`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeebonus`(IN `ebon_EmployeeID` INT, IN `ebon_OrganizationID` INT)
    DETERMINISTIC
BEGIN

SELECT ebon.RowID
,IFNULL(p.PartNo,'') 'Type'
,COALESCE(ebon.BonusAmount,0) 'BonusAmount'
,IFNULL(ebon.AllowanceFrequency,'') 'AllowanceFrequency'
,COALESCE(DATE_FORMAT(ebon.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
,COALESCE(DATE_FORMAT(ebon.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
,IFNULL(ebon.ProductID,'') 'ProductID'
 FROM employeebonus ebon
 LEFT JOIN product p ON ebon.ProductID=p.RowID
 WHERE ebon.EmployeeID=ebon_EmployeeID
 AND ebon.OrganizationID=ebon_OrganizationID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
