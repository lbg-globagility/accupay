/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeeawards`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeawards`(IN `eawar_EmployeeID` INT, IN `eawar_OrganizationID` INT)
    DETERMINISTIC
    COMMENT 'view all employee''s award(s) base on employee RowID and organiztion'
BEGIN

SELECT
eawar.RowID
,eawar.EmployeeID
,COALESCE(eawar.AwardType,'') 'AwardType'
,COALESCE(eawar.AwardDescription,'') 'AwardDescription'
,COALESCE(eawar.AwardDate,'') 'AwardDate'
,DATE_FORMAT(eawar.Created,'%m-%d-%Y') 'Created'
,CONCAT(u.FirstName,' ',u.LastName) 'CreatedBy'
,COALESCE(DATE_FORMAT(eawar.LastUpd,'%m-%d-%Y'),'') 'LastUpd'
,COALESCE((SELECT CONCAT(FirstName,' ',LastName) FROM user WHERE RowID=eawar.LastUpdBy),'') 'LastUpdBy'
FROM employeeawards eawar
LEFT JOIN user u ON u.RowID=eawar.CreatedBy
WHERE eawar.OrganizationID=eawar_OrganizationID
AND eawar.EmployeeID=eawar_EmployeeID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
