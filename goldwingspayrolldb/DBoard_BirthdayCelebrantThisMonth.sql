/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `DBoard_BirthdayCelebrantThisMonth`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `DBoard_BirthdayCelebrantThisMonth`(IN `OrganizID` INT)
    DETERMINISTIC
BEGIN



SELECT
e.EmployeeID
,CONCAT(e.LastName,',',e.FirstName,',',INITIALS(e.MiddleName,'.','1')) 'Employee Fullname'
,CONCAT(DATE_FORMAT(e.Birthdate,'%m/%e/%Y'), IF(ADDDATE(e.Birthdate, INTERVAL 60 YEAR) <= CURDATE(), '(Senior Citizen)', '')) 'Birthdate'
FROM employee e
WHERE MONTH(e.Birthdate)=MONTH(CURDATE())
AND e.OrganizationID=OrganizID
AND e.EmploymentStatus NOT IN ('Resigned','Terminated')
ORDER BY DAY(e.Birthdate);

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
