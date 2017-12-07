/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeeloanhistory`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeloanhistory`(IN `ehist_EmployeeID` INT, IN `ehist_OrganizationID` INT)
    DETERMINISTIC
BEGIN

SELECT
COALESCE(DATE_FORMAT(DeductionDate,'%m/%d/%Y'),'') 'DeductionDate'
,COALESCE(DeductionAmount,0) 'DeductionAmount'
,COALESCE(Status,'') 'Status'
,COALESCE(Comments,'') 'Comments'
,RowID
FROM employeeloanhistory
WHERE EmployeeID=ehist_EmployeeID
AND OrganizationID=ehist_OrganizationID
ORDER BY DeductionDate DESC;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
